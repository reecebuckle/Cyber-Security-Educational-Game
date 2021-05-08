using System.Collections.Generic;
using System.Linq;
using Managers;
using UI;
using Units;
using UnityEngine;

namespace Attacks
{
    /*
     * Firewall is a defence boosting move that boosts one units defence by 2
     */
    public class Firewall : AttackUnit
    {
        private Unit unit;
        private List<Unit> unitsInRange = new List<Unit>();
        private bool waiting;
        private bool unitSelected;

        [Header("Move Attributes")] 
        [SerializeField] private int actionPoints = 3;
        [SerializeField] private int attackRange = 1;
        [SerializeField] private int defenceBoost = 2; //2 defence boost

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
            unit.WaitingToAttack = false;
            
            ResetAllTiles();
        }

        /*
        * Event input system for receiving an asic attack (one unit left, right, up or down)
        */
        public void OnClickFirewallDefend()
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
            GameUI.instance.UpdateStatusBar("Restores shields. Select a friendly unit in range...");

            //Loop through units in range, if they're OUR unit, highlight them
            foreach (Unit u in unitsInRange.Where(u => PlayerController.me.units.Contains(u)))
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
            //wait for player input (right click for now)
            if (Input.GetMouseButtonUp(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
                {
                    // return if collider is not a unit!
                    if (!hit.collider.CompareTag("Unit")) return;

                    Unit clickedUnit = hit.collider.GetComponent<Unit>();

                    // return if unit is not in range 
                    if (!unitsInRange.Contains(clickedUnit)) return;

                    // return if unit is not OURS
                    if (!PlayerController.me.units.Contains(clickedUnit)) return;
                    
                    DefendAllyUnit(unit, clickedUnit, defenceBoost);
                    //decrement points cost and update UI
                    unit.DecrementActionPoints(actionPoints);
                    GameUI.instance.DisplayUnitStats(unit);
                    //stop waiting for input
                    waiting = false;
                    //prevent from showing movement tiles after
                    PlayerController.me.DeselectUnit();
                    DeselectMove();
                }
            }
        }
    }
}