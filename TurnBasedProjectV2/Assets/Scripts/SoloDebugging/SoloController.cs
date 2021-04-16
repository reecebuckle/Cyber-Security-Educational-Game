using System.Collections.Generic;
using Map;
using Photon.Pun;
using Photon.Realtime;
using UI;
using Units;
using UnityEngine;

namespace SoloDebugging
{
    /*
     * This version of the player controller is used solely to set up pathfinding
     */

    public class SoloController : Pathfinding
    {
        public Unit selectedUnit;
        
        /*
        * Get reference of unit
        */
        private void Start()
        {
            //unit = GetComponent<Unit>();
            CacheAllTiles();
        }

        private void Update()
        {
            SelectUnit();

            if (selectedUnit != null)
            {
                //continue moving if another unit is selected, hence put this first!
                if (moving)
                    Move(selectedUnit);
            
                //return if unit isn't selected by the player controller
                //if (!selectedUnit.IsSelected()) return;

                //Uncomment this to allow unit to move AFTER attacking
                //if (selectedUnit.AttackedThisTurn()) return;
            
                //return if unit has moved this turn or forced to skipp
                //if (selectedUnit.MovedThisTurn() || selectedUnit.ShouldMissTurn()) return;
            
                //don't allow target to move if waiting to attack
               // if (selectedUnit.WaitingToAttack()) return;
            
                //this part of the block actually initiates the moving so checked last
                if (!moving)
                {
                    //FindSelectableTiles(selectedUnit);
                    WaitToSelectTileInRange();
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
                        Unit u = hit.collider.GetComponent<Unit>();
                        selectedUnit = u;
                        
                        //Finds selectable tiles in range - only needs to be called ONCE
                        FindSelectableTiles(selectedUnit);
                        
                    }
                }
            }
        }

        /*
       * Invoked IF a tile is selected within the selected units range
       */
        private void WaitToSelectTileInRange()
        {
            if (!Input.GetMouseButtonUp(0)) return;
            
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if (!hit.collider.CompareTag("Tile")) return;
                    
                Tile t = hit.collider.GetComponent<Tile>();

                if (t.selectable)
                    MoveToTile(t);
            }

        }
    }
}