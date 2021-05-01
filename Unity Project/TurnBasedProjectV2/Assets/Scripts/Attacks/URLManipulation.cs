using System.Collections.Generic;
using System.Linq;
using Managers;
using UI;
using Units;
using UnityEngine;

namespace Attacks
{
    /*
     * URL is a weak hitting single unit move (but bypasses shields)
     */
    public class URLManipulation : AttackUnit
    {
        private Unit unit;
        private List<Unit> unitsInRange = new List<Unit>();
        private bool waiting;
        private bool unitSelected;
        
        [Header("Move Attributes")]
        [SerializeField] private int damage = 1; 
        [SerializeField] private int attackRange = 1;
        [SerializeField] private int actionPoints = 3; //action points cost

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
            
            ResetAllTiles();
        }

        /*
        * Event input system for receiving an asic attack (one unit left, right, up or down)
        */
        public void OnClickURLManipulation()
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

            waiting = true;
            GameUI.instance.UpdateStatusBar("Bypasses shields. Select an enemy unit to attack...");

            //Loop through units in range, if they're OUR ENEMY unit, show that it's in range
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
                            //special method for bypassing shields
                            BypassShields(unit, clickedUnit, damage, actionPoints);
                            waiting = false;
                            DeselectMove();
                        }
                    }
                }
            }
        }
    }
}