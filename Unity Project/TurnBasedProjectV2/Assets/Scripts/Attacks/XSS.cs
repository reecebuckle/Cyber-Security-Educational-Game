using System.Collections.Generic;
using System.Linq;
using Photon.Realtime;
using UI;
using Units;
using UnityEngine;

namespace Attacks
{
    /*
     * XXS is a single hitting close combat move (high damage)
     */
    public class XSS : AttackUnit
    {
        private Unit unit;
        private List<Unit> unitsInRange = new List<Unit>();
        private bool waiting;
        private bool unitSelected;

        [Header("Move Attributes")] [SerializeField]
        private int damage = 2;

        [SerializeField] private int attackRange = 1;
        [SerializeField] private int actionPoints = 3;

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
            waiting = false;
            unit.ToggleWaitingToAttack(false);
            
            //ResetTilesInList();
        }
        

        /*
        * Event input system for receiving an asic attack (one unit left, right, up or down)
        */
        public void OnClickXSSAttack()
        {
            //Always clear if there were previous units in range
            unitsInRange.Clear();
            
            //Go through basic attack flow process (equivalent for each unit)
            if (!AttackFlowProcess(unit, actionPoints, attackRange)) return;

            //returns units in range
            unitsInRange = FindUnitsInRange(unit, attackRange);

            //return if no units in range
            if (unitsInRange.Count <= 0)
            {
                NoUnitsInRange();
                return;
            }

            waiting = true;
            GameUI.instance.UpdateStatusBar("Damages target. Select an enemy unit to attack...");

            //Loop through units in range, if they're OUR ENEMY unit, reduce their defences
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
                            //Deal critical damage to web server
                            if (clickedUnit.GetUnitID() == 6)
                                AttackEnemyUnit(unit, clickedUnit, damage + 1, actionPoints);
                            else
                                AttackEnemyUnit(unit, clickedUnit, damage, actionPoints);
                           
                            //stop waiting for input
                            waiting = false;
                            DeselectMove();
                        }
                    }
                }
            }
        }
    }
}