using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Attacks;
using Photon.Pun;
using Units;
using UnityEngine;

public class DeleteSecurityLogs : AttackUnit
{
    private Unit unit;
    private List<Unit> unitsInRange = new List<Unit>();
    private bool waiting;
    private bool unitSelected;
    [SerializeField] private int damage = 1; //1 damage to all unit's shields

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
    public void OnClickSecurityLogs()
    {
        //Always clear if there were previous units in range
        unitsInRange.Clear();

        //return if unit has already attacked this turn, or instructed to miss
        if (unit.AttackedThisTurn() || unit.ShouldMissTurn()) return;

        //returns units in 1 x 1 range
        unitsInRange = FindUnitsInRange(unit, 1);

        //Return if no units in range
        if (unitsInRange.Count > 0) return;

        //TODO input a check to make sure we have a valid unit in range, otherwise wasted move?

        //Loop through units in range, if they're OUR ENEMY unit, reduce their defences
        foreach (Unit u in unitsInRange.Where(u => PlayerController.enemy.units.Contains(u)))
        {
            unit.ToggleAttackedThisTurn(true);
            ReduceDefence(u, damage);
        }
    }
}