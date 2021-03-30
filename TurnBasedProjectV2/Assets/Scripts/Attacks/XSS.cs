using System.Collections;
using System.Collections.Generic;
using Attacks;
using UnityEngine;

public class XSS : AttackUnit
{
    /*
    * Event input system for receiving an asic attack (one unit left, right, up or down)
    */
    public void OnClickXSSAttack()
    {
        //return if unit has already attacked this turn
        if (unit.AttackedThisTurn())
            return;

        FindUnitsInRange();
        //HighlightTilesInRange();

        if (unitsInRange.Count > 0)
            attackStatus = AttackType.BasicAttack;
                
    }
}
