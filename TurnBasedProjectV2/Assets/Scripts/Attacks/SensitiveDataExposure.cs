using System.Collections;
using System.Collections.Generic;
using Attacks;
using Photon.Pun;
using Units;
using UnityEngine;

public class SensitiveDataExposure : AttackUnit
{
    private Unit unit;

    //private Unit unitToAttack;
    private List<Unit> unitsInRange = new List<Unit>();
    private bool waiting;
    private bool unitSelected;
    [SerializeField] private int damage = 1; //2 damage to one units shield
    
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
        unitsInRange.Clear();
        waiting = false;
    }

    /*
    * Event input system for receiving an asic attack (one unit left, right, up or down)
    */
    public void OnClickDataExposureAttack()
    {
        Debug.Log("Initiating attack");
        //Always clear if there were previous units in range
        unitsInRange.Clear();

        //return if unit has already attacked this turn, or instructed to miss
        if (unit.AttackedThisTurn() || unit.ShouldMissTurn()) return;

        //returns units in range
        unitsInRange = FindUnitsInRange(unit, 1);

        //TODO Remove HighlightTilesInRange();

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
                        Debug.Log("Unit Selected, attacking");
                        ReduceDefence(clickedUnit, damage);
                        waiting = false;
                    }
                }
            }
        }
    }
}

