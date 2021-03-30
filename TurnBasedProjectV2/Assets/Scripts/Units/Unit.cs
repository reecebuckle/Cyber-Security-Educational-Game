using System;
using System.Collections;
using Managers;
using Photon.Pun;
using UnityEngine;

namespace Units
{
    public class Unit : MonoBehaviourPun
    {
        [Header("Unit Properties")]
        [SerializeField] private float moveSpeed; // units movement speed
        [SerializeField] private int moveDistance; // max distance we can move per turn
        [SerializeField] private int maxHP; // maximum health points a unit has
        [SerializeField] private int maxDefence; // current defence points a unit has
        [SerializeField] private int unitID; // ID reference of unit
        [SerializeField] private string unitName; // ID reference of unit
        [SerializeField] private string unitInformation; // ID reference of unit
        
        private int currentHP; // current hit points a unit has
        private int currentDef; // current defence a unit has

        private bool hasMovedThisTurn;
        private bool attatckedThisTurn;
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
         * Invoked when another unit attacks this unit, and instructs this unit to take damage
         */
        [PunRPC]
        private void TakeDamage(int damage)
        {
            //Removes damage from armour then, then remainder from health
            CalculateDamage(damage);
            
            Debug.Log("Unit taking damage, current health = " + currentHP + "current defence = " + currentDef);

            if (currentHP <= 0)
                photonView.RPC("UnitHasDied", RpcTarget.All);
            //else
            //  photonView.RPC("UpdateHealthBar", RpcTarget.All, (float) currentHP / (float) maxHP);
        }

        /*
         * First subtracts damage from defence, and then from health
         */
        private void CalculateDamage(int damage)
        {
            int armorDamage = Math.Min(currentDef, damage);
            int healthDamage = Math.Min(currentHP, damage - armorDamage);
            currentDef -= armorDamage;
            currentHP -= healthDamage;
        }
        
        /*
         * Buffs defence up to a maximum level
         */
        [PunRPC]
        private void BoostDefence(int defence)
        {
            currentDef += defence;

            if (currentDef > maxDefence)
                currentDef = maxDefence;
            
            Debug.Log("Unit being buffed, current defence = " + currentDef);
        }
        
        /*
         * 
         */
        public void BoostDefences(int defence)
        {
            currentDef += defence;

            if (currentDef > maxDefence)
                currentDef = maxDefence;
            
            Debug.Log("Unit being buffed, current defence = " + currentDef);
        }

        /*
         * Invoked  when the unit's health reaches 0
         */
        [PunRPC]
        private void UnitHasDied ()
        {
            if(!photonView.IsMine)
                PlayerController.enemy.units.Remove(this);
            else
            {
                PlayerController.me.units.Remove(this);
                GameManager.instance.CheckWinCondition();
                PhotonNetwork.Destroy(gameObject);
            }
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
        * Invoked to change a units used status
        */
        public void ToggleAttackedThisTurn(bool attacked) => attatckedThisTurn = attacked;
        

        /*
         * Read only getter methods for all private variables 
         */
        public bool MovedThisTurn() => hasMovedThisTurn;
        public bool IsSelected() => isSelected;
        public int GetMovementDistance() => moveDistance;
        public float GetMovementSpeed() => moveSpeed;
        public int GetCurrentHp() => currentHP;
        public int GetMaxHp() => maxHP;
        public int GetCurrentDef() => currentDef;
        public int GetMaxDef() => maxDefence;
        public bool AttackedThisTurn() => attatckedThisTurn;
        public int GetUnitID() => unitID;
        public string GetUnitName() => unitName;
        public string GetUnitInformation() => unitInformation;
    }
}