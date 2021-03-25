using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using Units;
using UnityEngine;

namespace SoloDebugging
{
    /*
     * This version of the player controller is used solely to set up pathfinding
     */

    public class SoloController : Pathfinding
    {
        private Unit selectedUnit;

        /*
         * Cache all tiles on the map on the first frame when starting the player controller
         */
        private void Start() => CacheAllTiles();

        private void Update()
        {
            SelectUnit();

            if (selectedUnit != null)
            {
                if (!moving)
                {
                    FindSelectableTiles(selectedUnit);
                    SelectTileInRange();
                }
                else
                {
                    Move(selectedUnit);
                }
            }
        }


        /*
         * Checks if a unit has been selected!
         */
        private void SelectUnit()
        {
            if (Input.GetMouseButtonUp(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
                {
                    //TODO check if this is our unit somewhere
                    if (hit.collider.CompareTag("Unit"))
                    {
                        Unit unit = hit.collider.GetComponent<Unit>();

                        Debug.Log("Unit Selected");
                        selectedUnit = unit;
                    }
                }
            }
        }

        /*
         * Invoked IF a tile is selected within the selected units range
         */
        private void SelectTileInRange()
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
                        {
                            MoveToTile(t);
                        }
                    }
                }
            }
        }
    }
}