using System.Collections.Generic;
using System.Linq;
using Managers;
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
        private bool waiting;

        [Header("Move Attributes")] 
        //
        [SerializeField] private int defenceBoost = 1;
        [SerializeField] private int actionPoints = 3;
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
            ResetAllTiles();
            SoundManager.instance.PlayUIClick();
            
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

            GameUI.instance.UpdateStatusBar("Restores shields of all friendly units in range. Right-click a friendly in range...");
            waiting = true;
            
            //Loop through units in range, if they're OUR unit, boost their defence by one
            foreach (Unit unitToDefend in unitsInRange.Where(u => PlayerController.me.units.Contains(u)))
                unitToDefend.ToggleUnitInRange(true);
            
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
            if (Input.GetMouseButtonUp(1))
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
                            //Loop through units in range, if they're OUR unit, boost their defence by one
                            foreach (Unit unitToDefend in unitsInRange.Where(u => PlayerController.me.units.Contains(u)))
                                DefendAllyUnit(unit, unitToDefend, defenceBoost);

                            //decrement points cost and update UI
                            unit.DecrementActionPoints(actionPoints);
                            GameUI.instance.DisplayUnitStats(unit);
                            //prevent from showing movement tiles after
                            PlayerController.me.DeselectUnit();
                            DeselectMove();
                        }
                    }
                }
            }
        }
    }
}