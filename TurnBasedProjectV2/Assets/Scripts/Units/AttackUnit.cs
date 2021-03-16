using System.Collections.Generic;
using System.Runtime.CompilerServices;
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
        public List<Tile> tilesInRange = new List<Tile>();

        //Enum representing the current state in the attack cycle
        public enum AttackType
        {
            BasicAttack,
            BasicDefence,
            Standby
        }
        public AttackType attackStatus;

        private void Update()
        {
            
            switch (attackStatus)
            {
                case (AttackType.BasicAttack):
                    WaitToSelectUnitInRange();
                    break;
                
                case (AttackType.BasicDefence):
                    Debug.Log("Waiting to raise defence of units around me");
                    break;
                
                case (AttackType.Standby):
                    //DO NOTHING
                    break;
                
                default:
                    Debug.Log("Default switch statement");
                    break;
            }
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

        private void OnEnable()
        {
            Debug.Log("Setting the selected unit");
            unit = PlayerController.me.selectedUnit;
            attackStatus = AttackType.Standby;
        }

        private void OnDisable()
        {
            unit = null;
            unitsInRange.Clear();
            //attackStatus = AttackType.Standby;
        }

        /*
         * basic attack (one unit left, right, up or down)
         */
        public void OnClickBasicAttack()
        {
            //return if unit has already attacked this turn
            if (unit.AttackedThisTurn())
                return;

            FindUnitsInRange();
            //HighlightTilesInRange();

            if (unitsInRange.Count > 0)
                attackStatus = AttackType.BasicAttack;
                
        }

        /*
         * Basic defend unit (one unit left, right, up or down)
         */
        public void OnClickBasicDefence()
        {
            //return if unit has already attacked this turn
            if (unit.AttackedThisTurn())
                return;

            FindUnitsInRange();
            //TODO Work on this method - HighlightTilesInRange();

            if (unitsInRange.Count > 0)
                attackStatus = AttackType.BasicDefence;
        }


        /*
         * TODO move this methods into a inheritable class 
         *
         * 
         */

        /*
         * Returns units one tile in range
         */
        private void FindUnitsInRange()
        {
            unitsInRange.Clear();
            FindUnitWithDirection(Vector3.forward);
            FindUnitWithDirection(-Vector3.forward);
            FindUnitWithDirection(Vector3.right);
            FindUnitWithDirection(-Vector3.right);

            Debug.Log("Units in range: " + unitsInRange.Count);
        }

        /*
         * Searches a particular direction, if an enemy is in range, adds it to the list!
         */
        private void FindUnitWithDirection(Vector3 direction)
        {
            RaycastHit hit;
            Unit otherUnit = null;


            if (Physics.Raycast(unit.gameObject.transform.position, direction, out hit, 1))
            {
                otherUnit = hit.collider.GetComponent<Unit>();
                print(hit.collider.gameObject.name);
                unitsInRange.Add(otherUnit);
            }
        }


        /*
         * Invokes method to attack an enemy unit
         */
        [PunRPC]
        private void AttackEnemyUnit(Unit unitToAttack)
        {
            //stop update loop
            attackStatus = AttackType.Standby;
            
            //prevent unit from being able to move after attacking
            unit.ToggleAttackedThisTurn(true);

            //reset tiles in range
            DeselectTilesInRange();

            //TODO for a random range: Random.Range(minDamage, maxDamage + 1)
            unitToAttack.photonView.RPC("TakeDamage", PlayerController.enemy.photonPlayer, 1);
        }
        
        /*
         * Invokes method to attack an enemy unit
         */
        [PunRPC]
        private void BuffUnit(Unit unitToDefend)
        {
            //stop update loop
            attackStatus = AttackType.Standby;
            
            //prevent unit from being able to move after attacking
            unit.ToggleAttackedThisTurn(true);

            //reset tiles in range
            DeselectTilesInRange();
            
            unitToDefend.photonView.RPC("BuffDefence", PlayerController.enemy.photonPlayer, 1);
        }

        /*
         * Finds the tile below the unit, the 4 adjacent units and selects them as red (to illustrate attackable)
         */
        private void HighlightTilesInRange()
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
    }
}