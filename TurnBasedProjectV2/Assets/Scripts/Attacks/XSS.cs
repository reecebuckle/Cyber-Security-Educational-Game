using System.Collections.Generic;
using System.Linq;
using Photon.Realtime;
using Units;
using UnityEngine;

namespace Attacks
{
    public class XSS : AttackUnit
    {
        private Unit unit;
        private List<Unit> unitsInRange = new List<Unit>();
        private bool waiting;
        private bool unitSelected;
        [SerializeField] private int damage = 2;
        [SerializeField] private int moveRange = 1;

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
        public void OnClickXSSAttack()
        {
            Debug.Log("Initiating XSS attack");
            //Reset other selected units if swapping
            ResetSelection();

            //Always clear if there were previous units in range
            unitsInRange.Clear();

            //return if unit has already attacked this turn,
            if (unit.AttackedThisTurn()) return;

            // if unit instructed to miss
            if (unit.ShouldMissTurn()) return;

            //returns units in range
            unitsInRange = FindUnitsInRange(unit, moveRange);

            //return if no units in range
            if (unitsInRange.Count <= 0)
                NoUnitsInRange();

            else
            {
                waiting = true;

                //Loop through units in range, if they're OUR ENEMY unit, reduce their defences
                foreach (Unit u in unitsInRange.Where(u => PlayerController.enemy.units.Contains(u)))
                    u.ToggleUnitInRange(true);
            }
        }

        /*
         * Wait until a unit is selected
        */
        private void Update()
        {
            if (waiting)
                WaitToSelectUnitInRange();
        }

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
                            Debug.Log("Unit Selected, attacking");
                            unit.ToggleAttackedThisTurn(true);
                            AttackEnemyUnit(clickedUnit, damage);
                            waiting = false;
                            PlayerController.me.DeselectUnit();
                        }
                    }
                }
            }
        }
    }
}