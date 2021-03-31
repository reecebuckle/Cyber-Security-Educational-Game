using System.Collections;
using System.Collections.Generic;
using Attacks;
using Photon.Pun;
using Units;
using UnityEngine;

public class Firewall : AttackUnit
{
    private Unit unit;

    //private Unit unitToAttack;
    private List<Unit> unitsInRange = new List<Unit>();
    private bool waiting;
    private bool unitSelected;
    [SerializeField] private int defenceBoost = 2; //2 defence boost

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
        unitsInRange.Clear();
        waiting = false;
    }

    /*
    * Event input system for receiving an asic attack (one unit left, right, up or down)
    */
    public void OnClickFirewallDefend()
    {
        //Always clear if there were previous units in range
        unitsInRange.Clear();

        //return if unit has already attacked this turn, or instructed to miss
        if (unit.AttackedThisTurn() || unit.ShouldMissTurn()) return;

        //returns units in 1 x 1 range
        unitsInRange = FindUnitsInRange(unit, 1);
        
        //If there were units in range, begin coroutine waiting to select a target
        if (unitsInRange.Count > 0)
            waiting = true;
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
        if (Input.GetMouseButtonUp(1))
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
                
                unit.ToggleAttackedThisTurn(true);
                DefendAllyUnit(clickedUnit, defenceBoost);
                waiting = false;
            }
        }
    }
}