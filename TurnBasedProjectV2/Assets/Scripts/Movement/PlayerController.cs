using System;
using System.Collections;
using System.Collections.Generic;
using Movement;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using Tiles;

public class PlayerController : Pathfinding
{
    [Header("Player Controller Attributes")]
    public Player photonPlayer; // Photon.Realtime.Player class

    public string[] unitsToSpawn;
    public Transform[] spawnPositions; // array of all spawn positions for this player

    //TODO: Can this be private?
    public List<Unit> units = new List<Unit>(); // list of all units
    private Unit selectedUnit; // currently selected unit

    public static PlayerController me; // local player
    public static PlayerController enemy; // non-local enemy player

    public void Awake()
    {
        //InitPathfinding();
    }

    /*
     * This is called by PUN when the game first initialises
     * Used to spawn units for local player, and assign the enemy player
     */
    [PunRPC]
    private void Initialize(Player player)
    {
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
     * Spawns player units
     */
    private void SpawnUnits()
    {
        for (int x = 0; x < unitsToSpawn.Length; ++x)
        {
            //GameObject unit = PhotonNetwork.Instantiate(unitsToSpawn[x], unitSpawnPositions[x].position, Quaternion.identity);

            Debug.Log("Instantiating unit: " + x);

            GameObject unit = PhotonNetwork.Instantiate(
                unitsToSpawn[x],
                spawnPositions[x].transform.position,
                spawnPositions[x].transform.rotation,
                0);

            unit.GetPhotonView().RPC("Initialize", RpcTarget.Others, false);
            unit.GetPhotonView().RPC("Initialize", photonPlayer, true);
        }
    }

    /*
     * Called once per frame (multiplayer update)
     */
    /*private void Update()
    {
       
        // return if the player is trying to select a unit they do not own
        if (!photonView.IsMine)
            return;
        
        // when we press the LMB and it's our turn 
        if (Input.GetMouseButtonDown(0) && GameManager.instance.curPlayer == this)
        {
            SelectTileUponMouseClick();
            // calculate the tile we selected and try to select whatever that is
            // Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            // TrySelect(new Vector3(Mathf.RoundToInt(pos.x), Mathf.RoundToInt(pos.y), 0));
        }
    }*/

    /*
     * This method only exists for single player debugging purposes
     * TODO: SELECT MAP
     */
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            //FindSelectableTiles();
            //SelectTileUponMouseClick();
            Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            //TrySelect(new Vector3(Mathf.RoundToInt(pos.x), Mathf.RoundToInt(pos.y), 0));
        }
    }


    /*
     * Method adapted from my initial path finding tactics stile
     */
    private void SelectTileUponMouseClick()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.CompareTag("Tile"))
            {
                Tile tile = hit.collider.GetComponent<Tile>();

                Debug.Log("Tile Selected!");
                
                
                if (tile.selectedTile)
                {
                    
                }
                    
            }
        }
    }


    /*
     *
     *  TEMPORARILY NOT USING THESE METHODS
     *
     *
     *
     *
     *
     * 
     */


    /*void TrySelect(Vector3 selectPos)
    {
        // see if we're selecting one of our units
        Unit unit = units.Find(x => x.transform.position == selectPos);

        // if we're selecting our unit - select it
        if (unit != null)
        {
            Debug.Log("Selecting Unit");
            SelectUnit(unit);
            return;
        }

        // if we don't have a selected unit - don't do anything else
        if (!selectedUnit) return;

        // are we selecting an enemy unit?
        Unit enemyUnit = enemy.units.Find(x => x.transform.position == selectPos);

        if (enemyUnit != null)
        {
            TryAttack(enemyUnit);
            return;
        }

        // if we're not selecting a unit or attacking an enemy, try to move the selected unit
        TryMove(selectPos);
    }

    // called when we click on a unit
    void SelectUnit(Unit unitToselect)
    {
        // can we select the unit?
        if (!unitToselect.CanSelect())
            return;

        // un-select the current unit
        if (selectedUnit != null)
            selectedUnit.ToggleSelect(false);

        // select the new unit
        selectedUnit = unitToselect;
        selectedUnit.ToggleSelect(true);

        // set the unit info text
        GameUI.instance.SetUnitInfoText(unitToselect);
    }

    // de-selects the selected unit
    void DeSelectUnit()
    {
        selectedUnit.ToggleSelect(false);
        selectedUnit = null;

        // disable unit info text
        GameUI.instance.unitInfoText.gameObject.SetActive(false);
    }

    // selects a unit which is able to move / attack
    void SelectNextAvailableUnit()
    {
        Unit availableUnit = units.Find(x => x.CanSelect());

        if (availableUnit != null)
            SelectUnit(availableUnit);
        else
            DeSelectUnit();
    }

    // attempts to attack the requested enemy unit
    void TryAttack(Unit enemyUnit)
    {
        // can we attack the enemy unit?
        if (selectedUnit.CanAttack(enemyUnit.transform.position))
        {
            selectedUnit.Attack(enemyUnit);
            SelectNextAvailableUnit();
            GameUI.instance.UpdateWaitingUnitsText(units.FindAll(x => x.CanSelect()).Count);
        }
    }

    // attempts to move to the requested position
    void TryMove(Vector3 movePos)
    {
        // can we move to the position?
        if (selectedUnit.CanMove(movePos))
        {
            selectedUnit.Move(movePos);
            SelectNextAvailableUnit();
            GameUI.instance.UpdateWaitingUnitsText(units.FindAll(x => x.CanSelect()).Count);
        }
    }

    // called when our turn ends
    public void EndTurn()
    {
        // de-select unit
        if (selectedUnit != null)
            DeSelectUnit();

        // start the next turn
        GameManager.instance.photonView.RPC("SetNextTurn", RpcTarget.All);
    }

    // called when our turn has begun
    public void BeginTurn()
    {
        foreach (Unit unit in units)
            unit.usedThisTurn = false;

        // update the UI
        GameUI.instance.UpdateWaitingUnitsText(units.Count);
    }*/
}