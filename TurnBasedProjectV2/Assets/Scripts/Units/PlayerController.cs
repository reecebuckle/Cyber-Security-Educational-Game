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
        [Header("Reference to Photon Player")]
        //
        public Player photonPlayer; // Photon.Realtime.Player class

        [Header("Units for this Player")] 
        //
        public string[] unitsToSpawn;
        public Transform[] unitSpawnPositions; // array of all spawn positions for this player
        public List<Unit> units = new List<Unit>(); // list of all our units
        public Unit selectedUnit; // currently selected unit
        public Unit selectedEnemyUnit; // currently selected enemy unit

        [Header("Reference to P1 and P2")] 
        //
        public static PlayerController me; // local player
        public static PlayerController enemy; // non-local enemy player

        private int _unitsRemaining;
        private int _round = 0; //starts off at 0

        /*
        * Called when the game begins
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

                        if (units.Contains(clickedUnit))
                            SelectUnit(clickedUnit);
                        else
                            SelectEnemeyUnit(clickedUnit);
                    }
                }
            }
        }

        /*
         * Invoked when enemy unit is selected
         */
        private void SelectEnemeyUnit(Unit clickedUnit)
        {
            selectedEnemyUnit = clickedUnit;
            GameUI.instance.DisplayEnemyStats(selectedEnemyUnit);
            GameUI.instance.UpdateStatusBar("Selecting enemy unit: " + clickedUnit.GetUnitName());
        }

        /*
        * Invoked when we select a unit and it belongs to us
        */
        private void SelectUnit(Unit clickedUnit)
        {
            // If we click a unit we've already selected, DO nothing
            if (clickedUnit.IsSelected())
                return;

            // Unselect the current unit IF one is selected
            if (selectedUnit != null)
                DeselectUnit();

            clickedUnit.ToggleSelect(true);
            selectedUnit = clickedUnit;

            // TODO update this FindSelectableTiles(selectedUnit); here rather than in update??
            // Will display selected unit for us or enemy
            GameUI.instance.ToggleUnitBar(selectedUnit);
            GameUI.instance.DisplayUnitStats(selectedUnit);
            GameUI.instance.UpdateStatusBar("Selecting unit: " + clickedUnit.GetUnitName());
        }

        /*
        * Deselects the currently selected unity
        */
        public void DeselectUnit()
        {
            if (selectedUnit != null)
            {
                //exclude database and web server
                if (selectedUnit.GetUnitID() < 5)
                    selectedUnit.GetComponent<UnitController>().DeselectTiles();

                selectedUnit.ToggleSelect(false);
            }

            selectedUnit = null;
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
            {
                //exclude database and web server
                if (unit.GetUnitID() < 5)
                {
                    unit.GetComponent<UnitController>().DeselectTiles();
                    unit.ToggleMissTurn(false);
                }
            }

            // Invoke the next turn method for the other player!
            GameManager.instance.photonView.RPC("SetNextTurn", RpcTarget.All);
        }

        /*
         * Called when the player initiates a new turn and updates the UI
        */
        public void BeginTurn()
        {
            _unitsRemaining = units.Count;

            //how many action points to give to all units
            int actionPointGain = 2;

            //check to see we have a webserver
            Unit webserver = units.Find(unit => unit.GetUnitID() == 5);
            if (webserver != null)
            {
                actionPointGain++;
                _unitsRemaining--; //not counting webserver as a valid movable unit
            }

            //check to see we have a database and increment action points
            Unit database = units.Find(unit => unit.GetUnitID() == 6);
            if (database != null)
            {
                actionPointGain++;
                _unitsRemaining--; //not counting webserver as a valid movable unit
            }
            
            GameUI.instance.UpdateWaitingUnitsText(_unitsRemaining);
            
            //reset all units moved/attacked and increment action points
            foreach (Unit u in units)
            {
                u.ToggleMovedThisTurn(false);
                u.ToggleAttackedThisTurn(false);
                u.IncrementActionPoints(actionPointGain);
            }

            //increment round number
            _round++;
            GameUI.instance.UpdateRoundText(_round);
            //update status bar
            GameUI.instance.UpdateStatusBar("Each unit gained: " + actionPointGain + " AP");
        }


        /*
         * Decrements units remaining after a unit has moved
         */
        public void DecrementUnitsRemaining()
        {
            _unitsRemaining--;
            if (_unitsRemaining < 0)
                _unitsRemaining = 0;

            GameUI.instance.UpdateWaitingUnitsText(_unitsRemaining);
        }
    }
}