using System.Collections.Generic;
using Photon.Pun;
using UI;
using Units;
using UnityEngine;

namespace Attacks
{
    public class AttackUnit : MonoBehaviourPun
    {
        private List<Unit> _unitsInRange = new List<Unit>();
        private List<Tile> _tilesInRange = new List<Tile>();

        /*
         * Returns units one tile in range
         */
        protected List<Unit> FindUnitsInRange(Unit sourceUnit, int distance)
        {
            _unitsInRange.Clear();
            FindUnitWithDirection(sourceUnit, Vector3.forward, distance);
            FindUnitWithDirection(sourceUnit, -Vector3.forward, distance);
            FindUnitWithDirection(sourceUnit, Vector3.right, distance);
            FindUnitWithDirection(sourceUnit, -Vector3.right, distance);

            Debug.Log("Units in range: " + _unitsInRange.Count);
            return _unitsInRange;
        }

        /*
         * Searches a particular direction, if an enemy is in range, adds it to the list!
         */
        private void FindUnitWithDirection(Unit sourceUnit, Vector3 direction, int distance)
        {
            RaycastHit hit;
            Unit otherUnit = null;

            if (Physics.Raycast(sourceUnit.gameObject.transform.position, direction, out hit, distance))
            {
                otherUnit = hit.collider.GetComponent<Unit>();
                print(hit.collider.gameObject.name);
                _unitsInRange.Add(otherUnit);
            }
        }
        
        /*
        * Invokes method to attack an enemy unit
        */
        protected void AttackEnemyUnit(Unit unitToAttack, int damage)
        {
            //reset tiles in range
            //DeselectTilesInRange();
            unitToAttack.photonView.RPC("TakeDamage", RpcTarget.All, damage);
            //update enemy stats on your display
            GameUI.instance.DisplayEnemyStats(unitToAttack);
        }
        
        /*
        * Invokes method to attack an enemy unit
         */
        protected void DefendAllyUnit(Unit unitToDefend, int defenceAmount)
        {
            unitToDefend.BoostDefence(defenceAmount);
        }
        
        /*
         * Invokes method to attack an enemy unit's defence
        */
        protected void ReduceDefence(Unit unitToAttack, int damage)
        {
            unitToAttack.photonView.RPC("DamageShields",  RpcTarget.All, damage);
            //update enemy stats on your display
            GameUI.instance.DisplayEnemyStats(unitToAttack);
        }
        
        /*
         * Invokes method to attack an enemy unit's defence
        */
        protected void BypassShields(Unit unitToAttack, int damage)
        {
            unitToAttack.photonView.RPC("BypassDefence",  RpcTarget.All, damage);
            //update enemy stats on your display
            GameUI.instance.DisplayEnemyStats(unitToAttack);
        }
        
        /*
         * Invokes method to attack an enemy unit's defence
        */
        protected void DDoSAttack(Unit unitToAttack)
        {
            if (unitToAttack.GetUnitID() > 4)
                Debug.Log("remove an AP from each unit");
            else
                unitToAttack.photonView.RPC("MissTurn", RpcTarget.All);
        }
        
        /*
         * Invoked at the start of selection in case any old units are selected
         */
        protected void ResetSelection()
        {
            foreach (Unit u in PlayerController.me.units)
                u.ToggleSelect(false);
            
            foreach (Unit u in PlayerController.enemy.units)
                u.ToggleSelect(false);
        }

        /*
         * TODO Displays UI showing no units in range
         */
        protected void NoUnitsInRange()
        {
            Debug.Log("No units in range");
        }
        
        /*
        * TODO Displays UI showing no units in range
        */
        protected void NotEnoughActionPoints()
        {
            Debug.Log("Not enough action points");
        }
        

        /*
         * TODO Remove methods
         */
        
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
                    _tilesInRange.Add(t);
                    t.attack = true;
                }
            }
        }
        
        private void DeselectTilesInRange()
        {
            //reset tiles in range
            foreach (Tile t in _tilesInRange)
                t.Reset();

            _tilesInRange.Clear();
        }
        
        private Tile GetTargetTile()
        {
            RaycastHit hit;
            Tile tile = null;

            if (Physics.Raycast(transform.position, Vector3.down, out hit, 1))
                tile = hit.collider.GetComponent<Tile>();

            return tile;
        }
        
    }
}