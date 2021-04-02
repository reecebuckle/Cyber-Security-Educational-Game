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
            unit = null;

            foreach (Unit u in unitsInRange)
                u.ToggleUnitInRange(false);

            unitsInRange.Clear();
        }

        /*
        * Event input system for receiving an asic attack (one unit left, right, up or down)
        */
        public void OnClickAccessControl()
        {
            //Reset other selected units if swapping
            ResetSelection();

            //Always clear if there were previous units in range
            unitsInRange.Clear();

            //return if unit has already attacked this turn,
            if (unit.AttackedThisTurn()) return;

            // if unit instructed to miss
            if (unit.ShouldMissTurn()) return;

            if (unit.GetActionPoints() < actionPoints)
            {
                NotEnoughActionPoints();
                return;
            }

            //returns units in range
            unitsInRange = FindUnitsInRange(unit, attackRange);

            //return if no units in range
            if (unitsInRange.Count <= 0)
            {
                NoUnitsInRange();
                return;
            }

            //Loop through units in range, if they're OUR unit, boost their defence by one
            foreach (Unit u in unitsInRange.Where(u => PlayerController.me.units.Contains(u)))
            {
                unit.ToggleAttackedThisTurn(true); //only need to do once, but no harm doing multiple times
                u.ToggleUnitInRange(true);
                DefendAllyUnit(u, defenceBoost);
            }

            //decrement points cost and update UI
            unit.DecrementActionPoints(actionPoints);
            GameUI.instance.DisplayUnitStats(unit);
            //prevent from showing movement tiles after
            PlayerController.me.DeselectUnit();
        }
    }
}