using System.Collections;
using Photon.Pun;
using UnityEngine;

namespace Units
{
    public class Unit : MonoBehaviourPun
    {
        //TODO Convert these into public variables?
        [Header("Unit Properties")] 
        
        [SerializeField] private float moveSpeed; // units movement speed
        [SerializeField] private int moveDistance; // max distance we can move per turn
        [SerializeField] private int maxHP; // maximum health points a unit has
        [SerializeField] private int maxDefence; // current defence points a unit has
        
        private int currentHP; // current hit points a unit has
        private int currentDef; // current defence a unit has
        
        private bool hasMovedThisTurn; 
        private bool isSelected;

        /*
         * Initiate units current health and defence (which are variables
         */
        private void Start()
        {
            currentHP = maxHP;
            currentDef = maxDefence;
        }
        
        // called when the unit is spawned in
        [PunRPC]
        private void Initialize(bool isMine)
        {
            if (isMine) PlayerController.me.units.Add(this);
            else GameManager.instance.GetOtherPlayer(PlayerController.me).units.Add(this);
        }

        /*
        * Invoked to change a units selected status
        */
        public void ToggleSelect(bool selected) => isSelected = selected;
        
        /*
        * Invoked to change a units used status
        */
        public void ToggleMovedThisTurn(bool hasMoved) => hasMovedThisTurn = hasMoved;
        
        
        
        /*
         * Invoked when another unit attacks this unit, and instructs this unit to take damage
         * TODO: Incorporate defence into this maybe??
         */
        [PunRPC]
        private void TakeDamage (int damage)
        {
            currentHP -= damage;

            if (currentHP <= 0)
                photonView.RPC("UnitHasDied", RpcTarget.All);
            else
            {
                // update health UI
                photonView.RPC("UpdateHealthBar", RpcTarget.All, (float) currentHP / (float) maxHP);
            }
        }
        
        
        /*
         * GETTER METHODS
         */

        // Getter method to see if we can use this unit
        public bool MovedThisTurn()
        {
            return hasMovedThisTurn;
        }
        
        // Getter method to check if unit is already selected
        public bool IsSelected()
        {
            return isSelected;
        }
        
        //Getter method for move distance in pathfinding
        public int GetMovementDistance()
        {
            return moveDistance;
        }
        
        //Getter method for move speed when moving
        public float GetMovementSpeed()
        {
            return moveSpeed;
        }

        public int GetCurrentHp()
        {
            return currentHP;
        }
        
        public int GetMaxHp()
        {
            return maxHP;
        }
        
        
    }
}