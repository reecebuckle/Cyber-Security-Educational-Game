using System.Collections.Generic;
using System.Linq;
using UI;
using Units;
using UnityEngine;

namespace Attacks
{
    /*
     * Access control is a multi-target move that boosts all units defence in the area by 1
     */
    public class AccessControl : AttackUnit
    {
        private Unit unit;
        private List<Unit> unitsInRange = new List<Unit>();
        private bool unitSelected;

        [Header("Move Attributes")] 
        [SerializeField] private int defenceBoost = 1;
        [SerializeField] private int actionPoints = 2;
        [SerializeField] private int attackRange = 1;

        /*
        * Whenever the unit is selected, this is enabled (as we can't reference a prefab)
        */
        private void OnEnable() => unit = PlayerController.me.selectedUnit;

        /*
        * Whenever another unit is selected, this is cleared
        */
        private void OnDisable()
        {
            DeselectMove();
            unit = null;
        }
        
        /*
         * Reset move status if deselecting unit or just selecting a different move
         */
        private void DeselectMove()
        {
            foreach (Unit u in unitsInRange)
                u.ToggleUnitInRange(false);

            unitsInRange.Clear();
            moveSelected = false;
            unit.ToggleWaitingToAttack(false);
            
            ResetAllTiles();
        }

        /*
        * Event input system for receiving an asic attack (one unit left, right, up or down)
        */
        public void OnClickAccessControl()
        {
            //Always clear if there were previous units in range
            unitsInRange.Clear();
            
            //Go through basic attack flow process (equivalent for each unit)
            AttackFlowProcess(unit, actionPoints);

            //returns units in range
            unitsInRange = FindUnitsInRange(unit, attackRange);

            //return if no units in range
            if (unitsInRange.Count <= 0)
            {
                NoUnitsInRange();
                return;
            }
            
            //Loop through units in range, if they're OUR unit, boost their defence by one
            foreach (Unit unitToDefend in unitsInRange.Where(u => PlayerController.me.units.Contains(u)))
                DefendAllyUnit(unit, unitToDefend, defenceBoost);
            

            //decrement points cost and update UI
            unit.DecrementActionPoints(actionPoints);
            GameUI.instance.DisplayUnitStats(unit);
            //prevent from showing movement tiles after
            DeselectMove();
            PlayerController.me.DeselectUnit();
            DeselectMove();
        }
    }
}