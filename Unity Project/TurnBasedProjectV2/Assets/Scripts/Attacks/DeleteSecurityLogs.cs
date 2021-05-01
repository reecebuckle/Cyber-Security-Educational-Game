using System.Collections.Generic;
using System.Linq;
using Managers;
using UI;
using Units;
using UnityEngine;

namespace Attacks
{
    /*
     * Delete Security Logs is a one target move that lowers the defence of all units in range
     */
    public class DeleteSecurityLogs : AttackUnit
    {
        private Unit unit;
        private List<Unit> unitsInRange = new List<Unit>();
        private bool unitSelected;
        private bool waiting;

        [Header("Move Attributes")] 
        [SerializeField] private int actionPoints = 2;
        [SerializeField] private int attackRange = 1;
        [SerializeField] private int damage = 1; //1 damage to all unit's shields

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
            unit.WaitingToAttack = false;
            
            ResetAllTiles();
        }

        /*
        * Event input system for receiving an asic attack (one unit left, right, up or down)
        */
        public void OnClickSecurityLogs()
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
            GameUI.instance.UpdateStatusBar("Targets shields of all enemy units in range. Critically hits Analysts. Select any unit in range...");

            //Loop through units in range, if they're OUR ENEMY unit, reduce their defences
            foreach (Unit unitToAttack in unitsInRange.Where(u => PlayerController.enemy.units.Contains(u)))
                unitToAttack.ToggleUnitInRange(true);
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
                        //Loop through units in range, if they're OUR ENEMY unit, reduce their defences
                        foreach (Unit unitToAttack in unitsInRange.Where(u => PlayerController.enemy.units.Contains(u)))
                            ReduceMultiDefence(unit, unitToAttack, damage);
            
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