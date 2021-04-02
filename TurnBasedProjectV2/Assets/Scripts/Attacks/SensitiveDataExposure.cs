using System.Collections.Generic;
using System.Linq;
using UI;
using Units;
using UnityEngine;

namespace Attacks
{
    /*
     * Sensitive data exposure is a shield wrecking move
     */
    public class SensitiveDataExposure : AttackUnit
    {
        private Unit unit;
        private List<Unit> unitsInRange = new List<Unit>();
        private bool waiting;
        private bool unitSelected;

        [Header("Move Attributes")] 
        [SerializeField] private int attackRange = 2;
        [SerializeField] private int actionPoints = 2;
        [SerializeField] private int damage = 3; //3 damage to one units shield

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
            waiting = false;
        }

        /*
        * Event input system for receiving an asic attack (one unit left, right, up or down)
        */
        public void OnClickDataExposureAttack()
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

            waiting = true;

            //Loop through units in range and show them
            foreach (Unit u in unitsInRange.Where(u => PlayerController.enemy.units.Contains(u)))
                u.ToggleUnitInRange(true);
        }

        /*
         * Wait until a unit is selected
         */
        private void Update()
        {
            if (waiting)
                WaitToSelectUnitInRange();
        }

        /*
        * Selects enemy units by default as selecting our unit will change
        */
        private void WaitToSelectUnitInRange()
        {
            //wait for player input
            if (Input.GetMouseButtonUp(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.collider.CompareTag("Unit"))
                    {
                        Unit clickedUnit = hit.collider.GetComponent<Unit>();

                        if (unitsInRange.Contains(clickedUnit))
                        {
                            //make unit unable to attack or move again
                            unit.ToggleAttackedThisTurn(true);

                            ReduceDefence(clickedUnit, damage);

                            //decrement points cost and update UI
                            unit.DecrementActionPoints(actionPoints);
                            GameUI.instance.DisplayUnitStats(unit);
                            //stop waiting for input
                            waiting = false;
                            //prevent from showing movement tiles after
                            PlayerController.me.DeselectUnit();
                        }
                    }
                }
            }
        }
    }
}