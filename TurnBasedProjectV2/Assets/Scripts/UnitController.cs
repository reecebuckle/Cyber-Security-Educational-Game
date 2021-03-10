using System;
using System.Collections;
using System.Collections.Generic;
using Units;
using UnityEngine;

public class UnitController : Pathfinding
{
    public Unit unit;


    /*
     * Get reference of unit
     */
    private void Start()
    {
        unit = GetComponent<Unit>();
        CacheAllTiles();
    }


    /*
     * Checks if the unit can move
     */
    private void Update()
    {
        Debug.DrawRay(transform.position, transform.forward);

        if (unit.IsSelected())
        {
            if (!moving)
            {
                FindSelectableTiles(unit);
                WaitToSelectTileInRange();
            }
            else
            {
                Move(unit);
            }
        }
    }

    /*
    * Invoked IF a tile is selected within the selected units range
    */
    private void WaitToSelectTileInRange()
    {
        if (Input.GetMouseButtonUp(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.CompareTag("Tile"))
                {
                    Debug.Log("Tile selected");
                    Tile t = hit.collider.GetComponent<Tile>();

                    if (t.selectable)
                        MoveToTile(t);
                }
            }
        }
    }
    
}