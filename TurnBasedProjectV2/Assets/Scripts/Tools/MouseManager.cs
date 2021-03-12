using System.Collections;
using System.Collections.Generic;
using Photon.Realtime;
using Units;
using UnityEngine;

/*
 * Short Mouse Manager Script used to simulate hovering over units, selecting them and changing the colour!
 * https://www.youtube.com/watch?v=OOkVADKo0IM&t=1979s&ab_channel=quill18creates - great tutorial!
 *
 * SELECTION TILE REFERENCED FREE USE HERE:
 * http://quill18.com/unity_tutorials/
 */
public class MouseManager : MonoBehaviour
{
    public Unit hoveredUnit;
    
    // Detects mouse movement!
    private void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.CompareTag("Unit"))
            {
                hoveredUnit = hit.collider.GetComponent<Unit>();
                HoverObject(hoveredUnit);
            }
            else
                ClearHover();
        }
    }
    
    
    /*
     * Invoked when merely hovering over a unit to display a box around it
     */
    private void HoverObject(Unit unit)
    {
        //return if  unit is already null
        if (hoveredUnit == null) return;
        //return if unit is already the hovered unit
        if (unit == hoveredUnit) return;

        ClearHover();

    }

    /*
     * Invoked when mouse leaves unit
     */
    private void ClearHover() => hoveredUnit = null;
}