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
            //return if unit isn't selected by the player controller
            if (!unit.IsSelected())
                return;

            //TODO return if unit has attacked this turn already - maybe
            //if (unit.AttackedThisTurn())
            //    return;
            
            //return if unit has moved this turn 
            if (unit.MovedThisTurn())
                return;


            if (!moving)
            {
                FindSelectableTiles(unit);
                WaitToSelectTileInRange();
            }
            else
            {
                PlayerController.me.DecrementUnitsRemaining();
                Move(unit);
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
                            MoveToTile(t);
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