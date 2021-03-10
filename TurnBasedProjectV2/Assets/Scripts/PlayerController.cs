using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using Units;
using UnityEditor;

public class PlayerController : Pathfinding
{
    [Header("Reference to Photon Player")] public Player photonPlayer; // Photon.Realtime.Player class

    [Header("Units for this Player")] public string[] unitsToSpawn;
    public Transform[] unitSpawnPositions; // array of all spawn positions for this player
    public List<Unit> units = new List<Unit>(); // list of all our units
    private Unit selectedUnit; // currently selected unit

    [Header("Reference to P1 and P2")] 
    public static PlayerController me; // local player
    public static PlayerController enemy; // non-local enemy player

    enum PlayerState
    {
        BeginPhase,
        UnitSelected,
        Attacking,
        Waiting
    }

    /*
     * Cache all tiles on the map on the first frame when starting the player controller
     */
    private void Start() => CacheAllTiles();

    /*
     * Called when the game begins
     */
    [PunRPC]
    void Initialize(Player player)
    {
        Debug.Log("initialise is called");
        photonPlayer = player;
        if (player.IsLocal)
        {
            me = this;
            SpawnUnits();
        }
        else
            enemy = this;

        // set the player text
        GameUI.instance.SetPlayerText(this);
    }

    /*
    * Instantiates prefabs from the Photon Resources Folder
    */
    private void SpawnUnits()
    {
        Debug.Log("Spawning Units... ");

        for (int x = 0; x < unitsToSpawn.Length; ++x)
        {
            GameObject unit =
                PhotonNetwork.Instantiate(unitsToSpawn[x], unitSpawnPositions[x].position, Quaternion.identity);
            unit.GetPhotonView().RPC("Initialize", RpcTarget.Others, false);
            unit.GetPhotonView().RPC("Initialize", photonPlayer, true);
        }
    }

    private void Update()
    {
        // only the local player can control this player
        if (!photonView.IsMine)
            return;

        // Only run if it's our turn
        if (GameManager.instance.curPlayer == this)
            PlayerPhase();
        
    }

    /*
     * Called when the player is in their player phase 
     */
    private void PlayerPhase()
    {
        // Attempt to select a unit if nothing is selected
        WaitToSelectUnit();

        // If unit is selected, find selectable tiles and allow them to select a tile in range
        if (selectedUnit != null)
        {
            if (selectedUnit.MovedThisTurn() == false)
            {
                //FindSelectableTiles(selectedUnit);

                if (!moving)
                {
                    //FindSelectableTiles(selectedUnit);
                    WaitToSelectTileInRange();
                }
                else
                {
                    Move(selectedUnit);
                    GameUI.instance.UpdateWaitingUnitsText(units.FindAll(x => !x.MovedThisTurn()).Count);
                }
            }

            // TODO  allow the unit to attack another unit
        }
    }
    

    /*
     * Checks if a unit has been selected!
     * TODO: Deprecated method removing to externalise the mouse manager
     */
    private void WaitToSelectUnit()
    {
        if (Input.GetMouseButtonUp(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.CompareTag("Unit"))
                {
                    Unit clickedUnit = hit.collider.GetComponent<Unit>();
                    SelectUnit(clickedUnit);
                }
            }
        }
    }

    /*
     * Invoked when we select a unit
     * TODO: Decide how to handle selecting enemy unit and ours when attacking, + updating the instance
     */
    private void SelectUnit(Unit clickedUnit)
    {
        // If we click a unit we've already selected, DO nothing
        if (clickedUnit.IsSelected())
            return;

        // Unselect the current unit IF one is selected
        if (selectedUnit != null)
            DeselectUnit();

        // Select the unit IF it belongs to us 
        if (units.Contains(clickedUnit))
        {
            Debug.Log("Unit selected");
            clickedUnit.ToggleSelect(true);
            selectedUnit = clickedUnit;
            FindSelectableTiles(selectedUnit);
            // Will display selected unit for us or enemy
            GameUI.instance.SetUnitInfoText(clickedUnit);
        }
    }
    
    /*
   * Deselects the currently selected unity
   */
    private void DeselectUnit()
    {
        selectedUnit.ToggleSelect(false);
        selectedUnit = null;
        //remove found tiles of old unit
        RemoveSelectableTiles();
        // disable unit info text
        GameUI.instance.unitInfoText.gameObject.SetActive(false);
    }

    /*
     * Invoked IF a tile is selected within the selected units range
     */
    private void WaitToSelectTileInRange()
    {
        if (Input.GetMouseButtonUp(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.CompareTag("Tile"))
                {
                    Debug.Log("Tile selected");
                    Tile t = hit.collider.GetComponent<Tile>();

                    if (t.selectedTile)
                        MoveToTile(t);
                    
                }
            }
        }
    }
    
    /*
    * Invokes the move function (within pathfanding) to move the unit
    */
    private void MoveUnit()
    {
        Move(selectedUnit);
        selectedUnit.ToggleMovedThisTurn(true);
        
        //DeselectUnit();
        //SelectNextAvailableUnit();
        GameUI.instance.UpdateWaitingUnitsText(units.FindAll(x => !x.MovedThisTurn()).Count);
    }

    /*
     * Selects the next available unit
     */
    private void SelectNextAvailableUnit()
    {
        Unit availableUnit = units.Find(x => !x.MovedThisTurn());

        if (availableUnit != null)
            selectedUnit = availableUnit;
    }
    

    /*
     * Called when our turn ends
     */
    public void EndTurn()
    {
        if (selectedUnit != null)
            DeselectUnit();
        
        foreach (Unit unit in units)
            unit.ToggleMovedThisTurn(false);

        // Invoke the next turn method for the other player!
        GameManager.instance.photonView.RPC("SetNextTurn", RpcTarget.All);
    }
    
    /*
     * Called when the player initiates a new turn
     */
    public void BeginTurn()
    {
        
        // update the UI
        GameUI.instance.UpdateWaitingUnitsText(units.Count);
    }
}