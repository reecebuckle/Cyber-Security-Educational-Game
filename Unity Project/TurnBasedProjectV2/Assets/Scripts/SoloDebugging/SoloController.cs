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
    * Solo Defence move used for performance analysis without connecting multiplayer
    */
    public class SoloController : Pathfinding
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

        private void Update()
        {
            //continue moving if another unit is selected, hence put this first!
            if (moving)
                Move(unit);

            //return if unit isn't selected by the player controller
            if (!unit.IsSelected()) return;

            //Uncomment this to allow unit to move AFTER attacking
            //if (selectedUnit.AttackedThisTurn()) return;

            //return if unit has moved this turn or forced to skipp
            //if (selectedUnit.MovedThisTurn() || selectedUnit.ShouldMissTurn()) return;

            //don't allow target to move if waiting to attack
            // if (selectedUnit.WaitingToAttack()) return;

            //this part of the block actually initiates the moving so checked last
            if (!moving)
                WaitToSelectTileInRange();
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

        /*
         * Find selectable tiles
         */
        public void FindTiles() => FindSelectableTiles(unit);

        /*
        * Removes selectable tiles
        */
        public void DeselectTiles() => RemoveSelectableTiles();
        
    }
}