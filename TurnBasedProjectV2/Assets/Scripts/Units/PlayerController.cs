using System.Collections.Generic;
using Managers;
using Photon.Pun;
using Photon.Realtime;
using UI;
using UnityEngine;

namespace Units
{
    public class PlayerController : MonoBehaviourPun
    {
        [Header("Reference to Photon Player")] public Player photonPlayer; // Photon.Realtime.Player class

        [Header("Units for this Player")] public string[] unitsToSpawn;
        public Transform[] unitSpawnPositions; // array of all spawn positions for this player
        public List<Unit> units = new List<Unit>(); // list of all our units
        public Unit selectedUnit; // currently selected unit

        [Header("Reference to P1 and P2")] public static PlayerController me; // local player
        public static PlayerController enemy; // non-local enemy player

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
                WaitToSelectUnit();
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
                clickedUnit.ToggleSelect(true);
                selectedUnit = clickedUnit;
                // TODO update this FindSelectableTiles(selectedUnit);
                // Will display selected unit for us or enemy
                GameUI.instance.SetUnitInfoText(selectedUnit);
                GameUI.instance.ToggleUnitBar(selectedUnit);
            }
        }

        /*
        * Deselects the currently selected unity
        */
        private void DeselectUnit()
        {
            if (selectedUnit != null)
            {
                selectedUnit.ToggleSelect(false);
                selectedUnit.GetComponent<UnitController>().DeselectTiles();
            }
            
            selectedUnit = null;
            // disable unit info text
            GameUI.instance.unitInfoText.gameObject.SetActive(false);
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
            DeselectUnit();
            
            //Remove selectable tiles if it was there
            foreach (Unit unit in units)
                unit.GetComponent<UnitController>().DeselectTiles();
            
            // Invoke the next turn method for the other player!
            GameManager.instance.photonView.RPC("SetNextTurn", RpcTarget.All);
        }

        /*
         * Called when the player initiates a new turn and updates the UI
        */
        public void BeginTurn()
        {
            foreach (Unit unit in units)
            {
                unit.ToggleMovedThisTurn(false);
                unit.ToggleAttackedThisTurn(false);
            }
               
            GameUI.instance.UpdateWaitingUnitsText(units.Count);
        } 
    }
}