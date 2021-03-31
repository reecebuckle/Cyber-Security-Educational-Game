using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Attacks;
using Photon.Pun;
using Units;
using UnityEngine;

public class AccessControl : AttackUnit
{
    private Unit unit;
    private List<Unit> unitsInRange = new List<Unit>();
    private bool waiting;
    private bool unitSelected;
    [SerializeField] private int defenceBoost = 2; //boost defence by 2

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
    public void OnClickAccessControl()
    {
        //Always clear if there were previous units in range
        unitsInRange.Clear();

        //return if unit has already attacked this turn
        if (unit.AttackedThisTurn()) return;
        
        //return if unit has already attacked this turn, or instructed to miss
        if (unit.AttackedThisTurn() || unit.ShouldMissTurn()) return;

        //returns units in 1 x 1 range
        unitsInRange = FindUnitsInRange(unit, 1);
        
        //Return if no units in range
        if (unitsInRange.Count > 0) return;
        
        //TODO check that using this move isn't a waste of time?

        //Loop through units in range, if they're OUR unit, boost their defence by one
        foreach (Unit u in unitsInRange.Where(u => PlayerController.me.units.Contains(u)))
        {
            unit.ToggleAttackedThisTurn(true); //only need to do once, but no harm doing multiple times
            DefendAllyUnit(u, defenceBoost);
        }
    }
}