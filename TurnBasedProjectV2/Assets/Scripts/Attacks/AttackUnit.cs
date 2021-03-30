using System.Collections.Generic;
using Photon.Pun;
using Units;
using UnityEngine;

namespace Attacks
{
    public class AttackUnit : MonoBehaviourPun
    {
        //private Unit unit;
        //public Unit unitToAttack;
        private List<Unit> _unitsInRange = new List<Unit>();
        private List<Tile> _tilesInRange = new List<Tile>();


        /*
         * Returns units one tile in range
         */
        protected List<Unit> FindUnits1X1InRange(Unit sourceUnit)
        {
            _unitsInRange.Clear();
            FindUnitWithDirection(sourceUnit, Vector3.forward);
            FindUnitWithDirection(sourceUnit, -Vector3.forward);
            FindUnitWithDirection(sourceUnit, Vector3.right);
            FindUnitWithDirection(sourceUnit, -Vector3.right);

            Debug.Log("Units in range: " + _unitsInRange.Count);
            return _unitsInRange;
        }

        /*
         * Searches a particular direction, if an enemy is in range, adds it to the list!
         */
        private void FindUnitWithDirection(Unit sourceUnit, Vector3 direction)
        {
            RaycastHit hit;
            Unit otherUnit = null;

            if (Physics.Raycast(sourceUnit.gameObject.transform.position, direction, out hit, 1))
            {
                otherUnit = hit.collider.GetComponent<Unit>();
                print(hit.collider.gameObject.name);
                _unitsInRange.Add(otherUnit);
            }
        }
    }
}


/*
 * TODO MOVE CODE ABOVE
 *
 *
 *
 *
 *
 *
 *
 *
 * 
 */
        
        
        /*/*
         * Invokes method to attack an enemy unit
         #1#
        [PunRPC]
        private void BuffUnit(Unit unitToDefend)
        {
            //stop update loop
           
            
            //prevent unit from being able to move after attacking
            //unit.ToggleAttackedThisTurn(true);

            //reset tiles in range
            DeselectTilesInRange();
            
            unitToDefend.photonView.RPC("BuffDefence", PlayerController.enemy.photonPlayer, 1);
        }

        /*
         * Finds the tile below the unit, the 4 adjacent units and selects them as red (to illustrate attackable)
         #1#
        private void HighlightTilesInRange(Unit unit)
        {
            //get tile below
            RaycastHit hit;
            Tile tile = null;

            if (Physics.Raycast(unit.gameObject.transform.position, Vector3.down, out hit, 1))
                tile = hit.collider.GetComponent<Tile>();

            if (tile != null)
            {
                tile.FindNeighboursInRange();

                foreach (Tile t in tile.adjacencyList)
                {
                    tilesInRange.Add(t);
                    t.attack = true;
                }
            }
        }

        /*
         * Deselects tiles in range
         * TODO fix this for selecting other characters or ending turn..
         #1#
        private void DeselectTilesInRange()
        {
            //reset tiles in range
            foreach (Tile t in tilesInRange)
                t.Reset();

            tilesInRange.Clear();
        }

        /*
         * Returns the target tile underneath the unit, or from the units collider
        #1#
        private Tile GetTargetTile()
        {
            RaycastHit hit;
            Tile tile = null;

            if (Physics.Raycast(transform.position, Vector3.down, out hit, 1))
                tile = hit.collider.GetComponent<Tile>();

            return tile;
        }
        
        private void WaitToBuffUnitsInRange()
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                foreach (Unit u in unitsInRange)
                {
                    BuffUnit(u);
                }
                
                 
            }
        }

    }*/
