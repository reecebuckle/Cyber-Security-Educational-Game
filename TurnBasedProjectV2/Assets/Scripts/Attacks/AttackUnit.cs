using System;
using System.Collections.Generic;
using System.IO;
using Photon.Pun;
using UI;
using Units;
using UnityEngine;
using UnityEngine.SocialPlatforms;

namespace Attacks
{
    public class AttackUnit : MonoBehaviourPun
    {
        private List<Unit> _unitsInRange = new List<Unit>();
        private List<Tile> _tilesInRange = new List<Tile>();
        public bool moveSelected;
        private GameObject[] tiles;

        /*
        * Cache all tiles right away for used when resetting tile options
        */
        private void Start() => tiles = GameObject.FindGameObjectsWithTag("Tile");

        /*
         * Returns units one tile in range
         */
        protected List<Unit> FindUnitsInRange(Unit sourceUnit, int distance)
        {
            _unitsInRange.Clear();
            FindUnitWithDirection(sourceUnit, Vector3.forward, distance);
            FindUnitWithDirection(sourceUnit, -Vector3.forward, distance);
            FindUnitWithDirection(sourceUnit, Vector3.right, distance);
            FindUnitWithDirection(sourceUnit, -Vector3.right, distance);

            Debug.Log("Units in range: " + _unitsInRange.Count);
            return _unitsInRange;
        }

        /*
         * Searches a particular direction, if an enemy is in range, adds it to the list!
         */
        private void FindUnitWithDirection(Unit sourceUnit, Vector3 direction, int distance)
        {
            RaycastHit hit;
            Unit otherUnit = null;

            if (Physics.Raycast(sourceUnit.gameObject.transform.position, direction, out hit, distance))
            {
                otherUnit = hit.collider.GetComponent<Unit>();
                print(hit.collider.gameObject.name);
                _unitsInRange.Add(otherUnit);
            }
        }
        
        /*
         * Checks the attack flow process to validate attacks
         */
        protected void AttackFlowProcess(Unit unitAttacking, int actionPoints)
        {
            //Reset other selected units if swapping
            ResetSelection();
            unitAttacking.ToggleWaitingToAttack(false);
            
            //return if unit has already attacked this turn,
            if (unitAttacking.AttackedThisTurn()) return;

            // if unit instructed to miss
            if (unitAttacking.ShouldMissTurn()) return;

            if (unitAttacking.GetActionPoints() < actionPoints)
            {
                NotEnoughActionPoints();
                return;
            }

            //highlight tiles in range anyway
            moveSelected = true;
            unitAttacking.ToggleWaitingToAttack(true);
            HighlightTilesInRange(unitAttacking);
        }

        /*
        * Invokes method to attack an enemy unit
        */
        protected void AttackEnemyUnit(Unit unitAttacking, Unit unitToAttack, int damage, int actionPoints)
        {
            //Toggle that unit has attacked
            unitAttacking.ToggleAttackedThisTurn(true);
           
            //Decrement action points cost
            unitAttacking.DecrementActionPoints(actionPoints);
            unitToAttack.photonView.RPC("TakeDamage", RpcTarget.All, damage);
            
            //deselect mopve
            moveSelected = false;
            
            //update your own stats 
            GameUI.instance.DisplayUnitStats(unitAttacking);
            //update enemy stats on your display
            GameUI.instance.DisplayEnemyStats(unitToAttack);
            //update status bar
            GameUI.instance.AppendHistoryLog(damage + " done to " + unitToAttack.GetUnitName());
            
            //Deselect unit by default 
            PlayerController.me.DeselectUnit();
        }

        /*
        * Invokes method to attack an enemy unit
         */
        protected void DefendAllyUnit(Unit unitActing, Unit unitToDefend, int defenceAmount)
        {
            //Toggle that unit has attacked, only need to do once, but no harm doing multiple times
            unitActing.ToggleAttackedThisTurn(true);
            unitToDefend.ToggleUnitInRange(true);
            
            unitToDefend.BoostDefence(defenceAmount);
            //update status bar
            GameUI.instance.AppendHistoryLog("Boosting defence of " + unitToDefend.GetUnitName() + " by " +
                                             defenceAmount);
        }

        /*
         * Invokes method to attack an enemy unit's defence
        */
        protected void ReduceDefence(Unit unitAttacking, Unit unitToAttack, int damage, int actionPoints)
        {
            //Toggle that unit has attacked
            unitAttacking.ToggleAttackedThisTurn(true);
            
            //Decrement action points cost
            unitAttacking.DecrementActionPoints(actionPoints);
            
            unitToAttack.photonView.RPC("DamageShields", RpcTarget.All, damage);
            
            //update your own stats 
            GameUI.instance.DisplayUnitStats(unitAttacking);
            //update enemy stats on your display
            GameUI.instance.DisplayEnemyStats(unitToAttack);
            //update status bar
            GameUI.instance.AppendHistoryLog("Targeting shields of " + unitToAttack.GetUnitName());
            
            //Deselect unit by default 
            PlayerController.me.DeselectUnit();
        }
        
        /*
         * Invokes method to attack an enemy unit's defence
        */
        protected void ReduceMultiDefence(Unit unitAttacking, Unit unitToAttack, int damage)
        {
            //toggle in range for display
            
            unitToAttack.ToggleUnitInRange(true);
            //Toggle that unit has attacked
            unitAttacking.ToggleAttackedThisTurn(true);
            
            unitToAttack.photonView.RPC("DamageShields", RpcTarget.All, damage);
            
            //update history log
            GameUI.instance.AppendHistoryLog("Targeting shields of " + unitToAttack.GetUnitName());
        }

        /*
         * Invokes method to attack an enemy unit's defence
        */
        protected void BypassShields(Unit unitAttacking, Unit unitToAttack, int damage, int actionPoints)
        {
            //Toggle that unit has attacked
            unitAttacking.ToggleAttackedThisTurn(true);
            
            //Decrement action points cost
            unitAttacking.DecrementActionPoints(actionPoints);
            
            unitToAttack.photonView.RPC("BypassDefence", RpcTarget.All, damage);
            
            //update your own stats 
            GameUI.instance.DisplayUnitStats(unitAttacking);
            //update enemy stats on your display
            GameUI.instance.DisplayEnemyStats(unitToAttack);
            //update status bar
            GameUI.instance.AppendHistoryLog("Bypassing shields, " + damage + " done to " + unitToAttack.GetUnitName());
            
            //Deselect unit by default 
            PlayerController.me.DeselectUnit();
        }

        /*
         * Invokes method to attack an enemy unit's defence
        */
        protected void DDoSAttack(Unit unitAttacking, Unit unitToAttack, int actionPoints)
        {
            //Toggle that unit has attacked
            unitAttacking.ToggleAttackedThisTurn(true);
            
            //Decrement action points cost
            unitAttacking.DecrementActionPoints(actionPoints);
            
            if (unitToAttack.GetUnitID() > 4)
                Debug.Log("remove an AP from each unit");
            else
                unitToAttack.photonView.RPC("MissTurn", RpcTarget.All);
            
            //update your own stats 
            GameUI.instance.DisplayUnitStats(unitAttacking);
            
            //update status bar
            GameUI.instance.AppendHistoryLog("Disabling enemy " + unitToAttack.GetUnitName() + " from acting next turn");
            
            //Deselect unit by default 
            PlayerController.me.DeselectUnit();
        }

        /*
         * Invoked at the start of selection in case any old units are selected
         */
        private void ResetSelection()
        {
            moveSelected = false;

            foreach (Unit u in PlayerController.me.units)
                u.ToggleSelect(false);

            foreach (Unit u in PlayerController.enemy.units)
                u.ToggleSelect(false);
        }

        /*
        * Updates status bar
        */
        protected void NoUnitsInRange() => GameUI.instance.UpdateStatusBar("No units in range...");


        /*
        * Updates status bar
        */
        private void NotEnoughActionPoints() => GameUI.instance.UpdateStatusBar("Not enough action points...");

        /*
         * Updates status bar
         */
        protected void UnitsAreInRange(int amount)
        {
            if (amount == 1)
                GameUI.instance.UpdateStatusBar(amount + "unit in range...");
            else
                GameUI.instance.UpdateStatusBar(amount + "units in range...");
        }


        public void HighlightTilesInRange(Unit unit)
        {
            //Deselect any previous tiles
            ResetAllTiles();
            DeselectTilesInRange();

            //get tile below
            RaycastHit hit;
            Tile tile = null;

            if (Physics.Raycast(unit.gameObject.transform.position, Vector3.down, out hit, 1))
                tile = hit.collider.GetComponent<Tile>();

            if (tile != null)
            {
                tile.FindNeighboursInRange();

                foreach (Tile t in tile.adjacencyList)
                {
                    _tilesInRange.Add(t);
                    t.attack = true;
                }
            }
        }

        protected void ResetAllTiles()
        {
            foreach (var tile in tiles)
                tile.GetComponent<Tile>().Reset();
        }

        protected void DeselectTilesInRange()
        {
            //reset tiles in range
            foreach (Tile t in _tilesInRange)
                t.Reset();

            _tilesInRange.Clear();
        }

        private Tile GetTargetTile()
        {
            RaycastHit hit;
            Tile tile = null;

            if (Physics.Raycast(transform.position, Vector3.down, out hit, 1))
                tile = hit.collider.GetComponent<Tile>();

            return tile;
        }
        
        
    }
}