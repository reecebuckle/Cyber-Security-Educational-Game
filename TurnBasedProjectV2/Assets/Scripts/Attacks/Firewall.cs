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
        //unitToAttack = null;
        unitsInRange.Clear();
        waiting = false;
    }

    /*
    * Event input system for receiving an asic attack (one unit left, right, up or down)
    */
    public void OnClickFirewallAttack()
    {
        //Always clear if there were previous units in range
        unitsInRange.Clear();

        //return if unit has already attacked this turn
        if (unit.AttackedThisTurn())
            return;

        //returns units in 1 x 1 range
        unitsInRange = FindUnits1X1InRange(unit);
        
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
                
                DefendAllyUnit(clickedUnit);
                waiting = false;
            }
        }
    }
    
    /*
    * Invokes method to attack an enemy unit
    */
    [PunRPC]
    private void DefendAllyUnit(Unit unitToDefend)
    {
        unit.ToggleAttackedThisTurn(true);
        unitToDefend.BoostDefences(2);
        //unitToDefend.photonView.RPC("BoostDefence", PlayerController.enemy.photonPlayer, 2);
    }
}