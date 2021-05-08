using System.Collections;
using Managers;
using Map;
using Photon.Realtime;
using UnityEngine;

namespace Units
{
    public class UnitController : Pathfinding
    {
        private Unit _unit;

        /*
        * Get reference of unit
        */
        private void Start()
        {
            _unit = GetComponent<Unit>();
            CacheAllTiles();
        }

        /*
        * Checks if the unit can move
        */
        private void Update()
        {
            //continue moving if another unit is selected, hence put this first!
            if (moving)
                Move(_unit);

            //return if unit isn't selected by the player controller
            if (!_unit.IsSelected()) return;

            //Uncomment this to allow unit to move AFTER attacking
            if (_unit.AttackedThisTurn()) return;

            //return if unit has moved this turn 
            if (_unit.MovedThisTurn()) return;

            //return if this should skip turn
            if (_unit.ShouldMissTurn()) return;

            //don't allow target to move if waiting to attack
            if (_unit.WaitingToAttack) return;

            //this part of the block actually initiates the moving so checked last
            if (!moving)
                WaitToSelectTileInRange();
        }

        /*
        * Invoked IF a tile is selected within the selected units range
        */
        private void WaitToSelectTileInRange()
        {
            //return if any other button that isn't LMC 
            if (!Input.GetMouseButtonUp(0)) return;

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                //return if collider isn't a tile
                if (!hit.collider.CompareTag("Tile")) return;

                Tile tile = hit.collider.GetComponent<Tile>();

                //return if tile isn't selectable
                if (!tile.selectable) return;
                
                SoundManager.instance.PlaySelectTile();
                PlayerController.me.DecrementUnitsRemaining();
                MoveToTile(tile);
            }
        }

        /*
         * Find selectable tiles
         */
        public void FindTiles() => FindSelectableTiles(_unit);

        /*
        * Removes selectable tiles
        */
        public void DeselectTiles() => RemoveSelectableTiles();
    }
}