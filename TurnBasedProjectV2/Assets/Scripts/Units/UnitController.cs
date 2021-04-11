using System.Collections;
using Map;
using Photon.Realtime;
using UnityEngine;

namespace Units
{
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
            //continue moving if another unit is selected, hence put this first!
            if (moving)
                Move(unit);
            
            //return if unit isn't selected by the player controller
            if (!unit.IsSelected()) return;

            //Uncomment this to allow unit to move AFTER attacking
            if (unit.AttackedThisTurn()) return;
            
            //return if unit has moved this turn or forced to skipp
            if (unit.MovedThisTurn() || unit.ShouldMissTurn()) return;
            
            //don't allow target to move if waiting to attack
            if (unit.WaitingToAttack()) return;
            
            //this part of the block actually initiates the moving so checked last
            if (!moving)
            {
                FindSelectableTiles(unit);
                WaitToSelectTileInRange();
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
                        Tile t = hit.collider.GetComponent<Tile>();

                        if (t.selectable)
                        {
                            PlayerController.me.DecrementUnitsRemaining();
                            MoveToTile(t);
                        }


                    }
                }
            }

        }


        /*
        * Removes selectable tiles
        */
        public void DeselectTiles() => RemoveSelectableTiles();
        
    }
}