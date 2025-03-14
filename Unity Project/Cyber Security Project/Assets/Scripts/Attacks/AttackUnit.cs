﻿using System.Collections.Generic;
using Map;
using Photon.Pun;
using UI;
using Units;
using UnityEngine;

namespace Attacks
{
    public class AttackUnit : MonoBehaviourPun
    {
        private List<Unit> _unitsInRange = new List<Unit>();
        protected bool moveSelected;
        private List<Tile> _tiles;

        [Header("Move Information")]
        //
        [SerializeField] private string abilityName;
        [SerializeField] private string[] information;

        /*
        * Cache all tiles right away for used when resetting tile options
        */
        private void Start() => _tiles = PlayerController.me._tiles;
        
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
                _unitsInRange.Add(otherUnit);
            }
        }

        /*
         * Checks the attack flow process to validate attacks
         */
        protected bool AttackFlowProcess(Unit unitAttacking, int actionPoints, int attackRange)
        {
            // Reset other selected units if swapping
            ResetSelection();
            unitAttacking.WaitingToAttack = false;

            // return if unit has already attacked this turn,
            if (unitAttacking.AttackedThisTurn())
            {
                UnitHasAlreadyAttacked();
                return false;
            }

            // if unit instructed to miss
            if (unitAttacking.ShouldMissTurn())
            {
                UnitCannotMove();
                return false;
            }

            // if unit does not have enough action points
            if (unitAttacking.ActionPoints < actionPoints)
            {
                NotEnoughActionPoints();
                return false;
            }

            //highlight tiles in range anyway
            moveSelected = true;
            unitAttacking.WaitingToAttack = true;

            HighlightTilesInExtendedRange(unitAttacking, attackRange);
            return true;
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

            //deselect move
            moveSelected = false;

            //update your own stats 
            GameUI.instance.DisplayUnitStats(unitAttacking);
            //update enemy stats on your display
            GameUI.instance.DisplayEnemyStats(unitToAttack);
            //update status bar
            GameUI.instance.UpdateStatusBar("Targeting " + unitToAttack.GetUnitName());
            //update history log
            GameUI.instance.AppendHistoryLog("Dealing " + damage + " damage to " + unitToAttack.GetUnitName());

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

            unitToDefend.BoostDefence(defenceAmount);

            //update status bar
            GameUI.instance.UpdateStatusBar("Restoring shields of " + unitToDefend.GetUnitName() + " by " +
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
            GameUI.instance.UpdateStatusBar("Targeting shields of " + unitToAttack.GetUnitName());
            //update history log
            GameUI.instance.AppendHistoryLog("Damaging shields of " + unitToAttack.GetUnitName() + " by " +damage);
            //Deselect unit by default 
            PlayerController.me.DeselectUnit();
        }

        /*
         * Invokes method to attack an enemy unit's defence
        */
        protected void ReduceMultiDefence(Unit unitAttacking, Unit unitToAttack, int damage)
        {
            //Toggle that unit has attacked
            unitAttacking.ToggleAttackedThisTurn(true);
            
            // Deal triple-critical damage to analyst units 
            if (unitToAttack.GetUnitID() == 4)
            {
                
                unitToAttack.photonView.RPC("DamageShields", RpcTarget.All, (damage * 3));
                //update history log
                GameUI.instance.AppendHistoryLog("Critical hit on: " + unitToAttack.GetUnitName() + "! Dealing " + (damage*3));
                //update status bar
                GameUI.instance.UpdateStatusBar("Targeting shields of " + unitToAttack.GetUnitName() + ". Critical Hit!");
            }
            else
            {
                unitToAttack.photonView.RPC("DamageShields", RpcTarget.All, damage);
                //update history log
                GameUI.instance.AppendHistoryLog("Damaging shields of " + unitToAttack.GetUnitName() + " by " +damage);
                //update status bar
                GameUI.instance.UpdateStatusBar("Targeting shields of " + unitToAttack.GetUnitName());
            }

            
            
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
            GameUI.instance.UpdateStatusBar("Bypassing shields of " + unitToAttack.GetUnitName());
            //update history log
            GameUI.instance.AppendHistoryLog(
                "Dealing " + damage + " physical damage to " + unitToAttack.GetUnitName());

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
            GameUI.instance.UpdateStatusBar("Targeting " + unitToAttack.GetUnitName());
            //update history log
            GameUI.instance.AppendHistoryLog("Disabling enemy " + unitToAttack.GetUnitName() +
                                             " from acting next turn");

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
        private void UnitHasAlreadyAttacked() =>
            GameUI.instance.UpdateStatusBar("Unit has already attacked this turn...");

        /*
        * Updates status bar
        */
        private void UnitCannotMove() => GameUI.instance.UpdateStatusBar("Unit has had it's turn disabled...");

        /*
        * Updates status bar
        */
        protected void NoUnitsInRange() => GameUI.instance.UpdateStatusBar("No valid units in range...");

        /*
        * Updates status bar
        */
        private void NotEnoughActionPoints() =>
            GameUI.instance.UpdateStatusBar("Not enough action points to use action...");

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


        private void HighlightTilesInExtendedRange(Unit unit, int attackRange)
        {
            //Deselect any previous tiles
            ResetAllTiles();

            RaycastHit hit;
            Tile tile = null;

            if (Physics.Raycast(unit.gameObject.transform.position, Vector3.down, out hit, 1))
            {
                tile = hit.collider.GetComponent<Tile>();
                if (tile != null)
                {
                    tile.MarkAttack();
                    tile.FindNeighboursInExtendedRange(attackRange);
                }
            }
        }

        /*
         * Resets all tiles
         */
        protected void ResetAllTiles()
        {
            foreach (var tile in _tiles)
                tile.Reset();
        }

        /*
        * Use expression body to return read only values pertaining to information about a unit ability 
        */
        public string Name() => abilityName;
        public string[] Information() => information;
    }
}
