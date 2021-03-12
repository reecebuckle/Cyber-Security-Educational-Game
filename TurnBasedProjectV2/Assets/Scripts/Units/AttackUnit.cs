using System.Collections.Generic;
using UnityEngine;

namespace Units
{
    public class AttackUnit : MonoBehaviour
    {
        public Unit unit;

        public Unit unitToAttack;
        public List<Unit> unitsInRange;
        private bool attackSelected = false;

        private void OnEnable()
        {
            Debug.Log("Setting the selected unit");
            unit = PlayerController.me.selectedUnit;
        }

        private void OnDisable()
        {
            unit = null;
            unitsInRange.Clear();
            attackSelected = false;
        }


        private void FindEnemiesInRange()
        {
            unitsInRange.Clear();
            Debug.Log("Finding enemies in range");
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

            Debug.DrawLine(unit.transform.position, direction, Color.green);
        }

        /*
         * When button for attack is initiated
         */
        public void OnClickAttack()
        {
            FindEnemiesInRange();

            if (unitsInRange.Count > 0)
                attackSelected = true;
        }

        private void Update()
        {
            if (attackSelected)
            {
                WaitToSelectUnitInRange();
            }
        }


        /*
        * Invoked IF a tile is selected within the selected units range
        */
        private void WaitToSelectUnitInRange()
        {
            Debug.Log("Waiting to select unit in range!");
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
                        {
                            Debug.Log("Try attack this unit here!");
                            attackSelected = false;
                        }

                            
                    }
                }
            }
        }
    }
}