using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

namespace Units
{
    public class AttackUnit : MonoBehaviourPun
    {
        public Unit unit;

        public Unit unitToAttack;
        public List<Unit> unitsInRange;
        private bool _attackSelected = false;
        public List<Tile> tilesInRange = new List<Tile>();
        
        private void Update()
        {
            if (_attackSelected)
                WaitToSelectUnitInRange();
            
        }

        private void OnEnable()
        {
            Debug.Log("Setting the selected unit");
            unit = PlayerController.me.selectedUnit;
        }

        private void OnDisable()
        {
            unit = null;
            unitsInRange.Clear();
            _attackSelected = false;
        }
        
        private void FindEnemiesInRange()
        {
            unitsInRange.Clear();
            FindEnemyWithDirection(Vector3.forward);
            FindEnemyWithDirection(-Vector3.forward);
            FindEnemyWithDirection(Vector3.right);
            FindEnemyWithDirection(-Vector3.right);

            Debug.Log("Units in range: " + unitsInRange.Count);
        }

        /*
         * Searches a particular direction, if an enemy is in range, adds it to the list!
         */
        private void FindEnemyWithDirection(Vector3 direction)
        {
            RaycastHit hit;
            Unit enemyUnit = null;


            if (Physics.Raycast(unit.gameObject.transform.position, direction, out hit, 1))
            {
                enemyUnit = hit.collider.GetComponent<Unit>();
                print(hit.collider.gameObject.name);
                unitsInRange.Add(enemyUnit);
            }
        }

        /*
         * When button for attack is initiated
         */
        public void OnClickAttack()
        {
            //return if unit has already attacked this turn
            if (unit.AttackedThisTurn())
                return;
            
            FindEnemiesInRange();
            HighlightTilesInRange();

            if (unitsInRange.Count > 0)
                _attackSelected = true;
        }

        /*
         * Invokes method to attack an enemy unit
         */
        [PunRPC]
        private void AttackEnemyUnit(Unit unitToAttack)
        {
            //stop update loop
            _attackSelected = false;
            //prevent unit from being able to move after attacking
            unit.ToggleAttackedThisTurn(true);
            
            //reset tiles in range
            DeselectTilesInRange();

            //TODO for a random range: Random.Range(minDamage, maxDamage + 1)
            unitToAttack.photonView.RPC("TakeDamage", PlayerController.enemy.photonPlayer, 1);
        }
        
        /*
         * Finds the tile below the unit, the 4 adjacent units and selects them as red (to illustrate attackable)
         */
        private void HighlightTilesInRange() {
            
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
         */
        private void DeselectTilesInRange()
        {
            //reset tiles in range
            foreach (Tile t in tilesInRange)
                t.Reset();
            
            tilesInRange.Clear();
        }
        
        /*
         * Returns the target tile underneath the unit, or from the units collider
        */
        private Tile GetTargetTile()
        {
            RaycastHit hit;
            Tile tile = null;

            if (Physics.Raycast(transform.position, Vector3.down, out hit, 1))
                tile = hit.collider.GetComponent<Tile>();

            return tile;
        }
        
        /*
        * Invoked IF a tile is selected within the selected units range
        */
        private void WaitToSelectUnitInRange()
        {
            if (Input.GetMouseButtonUp(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.collider.CompareTag("Unit"))
                    {
                        Unit clickedUnit = hit.collider.GetComponent<Unit>();

                        if (unitsInRange.Contains(clickedUnit))
                            AttackEnemyUnit(clickedUnit);
                    }
                }
            }
        }
    }
}