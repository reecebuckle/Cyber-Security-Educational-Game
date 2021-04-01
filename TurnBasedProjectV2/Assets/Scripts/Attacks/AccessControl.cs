using System.Collections.Generic;
using System.Linq;
using Units;
using UnityEngine;

namespace Attacks
{
    public class AccessControl : AttackUnit
    {
        private Unit unit;
        private List<Unit> unitsInRange = new List<Unit>();
        private bool waiting;
        private bool unitSelected;
        [SerializeField] private int defenceBoost = 2; //boost defence by 2

        /*
     * Whenever the unit is selected, this is enabled (as we can't reference a prefab)
     */
        private void OnEnable()
        {
            Debug.Log("Setting the selected unit");
            unit = PlayerController.me.selectedUnit;
        }

        /*
         * Whenever another unit is selected, this is cleared
         */
        private void OnDisable()
        {
            Debug.Log("Disabling the attack handler");
            unit = null;

            foreach (Unit u in unitsInRange)
                u.ToggleUnitInRange(false);

            unitsInRange.Clear();
            waiting = false;
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

            //return if unit has already attacked this turn
            if (unit.AttackedThisTurn()) return;

            // if unit instructed to miss
            if (unit.ShouldMissTurn()) return;

            //returns units in 1 x 1 range
            unitsInRange = FindUnitsInRange(unit, 1);

            //return if no units in range
            if (unitsInRange.Count <= 0)
                NoUnitsInRange();
            else
            {
                //TODO check that using this move isn't a waste of time?

                //Loop through units in range, if they're OUR unit, boost their defence by one
                foreach (Unit u in unitsInRange.Where(u => PlayerController.me.units.Contains(u)))
                {
                    unit.ToggleAttackedThisTurn(true); //only need to do once, but no harm doing multiple times
                    u.ToggleUnitInRange(true);
                    DefendAllyUnit(u, defenceBoost);
                }

                //prevent from showing movement tiles after
                PlayerController.me.DeselectUnit();
            }
        }
    }
}