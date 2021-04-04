using System;
using System.Collections;
using Managers;
using Photon.Pun;
using UI;
using UnityEngine;

namespace Units
{
    public class Unit : MonoBehaviourPun
    {
        [Header("Unit Properties")]
        //
        [SerializeField] private float moveSpeed; // units movement speed
        [SerializeField] private int moveDistance; // max distance we can move per turn
        [SerializeField] private int maxHP; // maximum health points a unit has
        [SerializeField] private int maxDefence; // current defence points a unit has
        [SerializeField] private int unitID; // ID reference of unit
        [SerializeField] private string unitName; // ID reference of unit
        [SerializeField] private string unitInformation; // ID reference of unit
        [SerializeField] private GameObject quad; //quad that shows up when in range
        [SerializeField] private GameObject selectionQuad; //quad that shows up when selected
        
        private int currentHP; // current hit points a unit has
        private int currentDef; // current defence a unit has
        private int actionPoints; //current number of action points a unit can have

        private bool hasMovedThisTurn;
        private bool attatckedThisTurn;
        private bool isSelected;
        private bool missTurn;
        
        /*
         * Initiate units current health and defence (which are variables
         */
        private void Start()
        {
            currentHP = maxHP;
            currentDef = maxDefence;
            quad.SetActive(false);
            selectionQuad.SetActive(false);
        }

        // called when the unit is spawned in
        [PunRPC]
        private void Initialize(bool isMine)
        {
            if (isMine) PlayerController.me.units.Add(this);
            else GameManager.instance.GetOtherPlayer(PlayerController.me).units.Add(this);
        }
        
        /*
         * Increments action points, called each turn - max amount 6
         */
        public void IncrementActionPoints(int amount)
        {
            actionPoints += amount;
            if (actionPoints > 6)
                actionPoints = 6;
        }
        
        /*
         * Decrements action points, called when a move is used by a unit 
         */
        public void DecrementActionPoints(int amount)
        {
            actionPoints -= amount;
            if (actionPoints < 0)
                actionPoints = 0;
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
        }

        /*
         * First subtracts damage from defence, and then from health
         */
        private void CalculateDamage(int damage)
        {
            int defenceDamage = Math.Min(currentDef, damage);
            int healthDamage = Math.Min(currentHP, damage - defenceDamage);
            currentDef -= defenceDamage;
            currentHP -= healthDamage;
        }

        /*
         * Invoked when another unit is trying to remove shields
         */
        [PunRPC]
        private void DamageShields(int damage)
        {
            //reduce current defence by damage
            currentDef -= damage;

            //but if it's 0, set to 0
            if (currentDef < 0)
                currentDef = 0;
            
            Debug.Log("Unit taking damage, current health = " + currentHP + "current defence = " + currentDef);
        }

        /*
         * Invoked when another unit attacks this unit and bypasses the defence
         */
        [PunRPC]
        private void BypassDefence(int damage)
        {
            //reduce current defence by damage
            currentHP -= damage;

            //but if it's 0, set to 0
            if (currentHP < 0)
                currentHP = 0;

            Debug.Log("Unit taking damage, current health = " + currentHP + "current defence = " + currentDef);

            if (currentHP <= 0)
                photonView.RPC("UnitHasDied", RpcTarget.All);
        }

        /*
         * Invoked when a units defence is boosted
         */
        public void BoostDefence(int defence)
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
        private void UnitHasDied()
        {
            if (!photonView.IsMine)
                PlayerController.enemy.units.Remove(this);
            else
            {
                PlayerController.me.units.Remove(this);
                GameManager.instance.CheckWinCondition();
                PhotonNetwork.Destroy(gameObject);
            }
        }

        /*
         * Causes unit to miss a turn
         */
        [PunRPC]
        private void MissTurn() => missTurn = true;
        

        /*
        * Invoked to change a units selected status
        */
        public void ToggleSelect(bool selected)
        {
            isSelected = selected;
            
            //updates selected quad
            if (isSelected)
                selectionQuad.SetActive(true);
            else 
                selectionQuad.SetActive(false);
        } 

        /*
        * Invoked to change a units used status
        */
        public void ToggleMovedThisTurn(bool hasMoved) => hasMovedThisTurn = hasMoved;

        /*
        * Invoked to change a units used status
        */
        public void ToggleAttackedThisTurn(bool attacked) => attatckedThisTurn = attacked;

        /*
         * Invoked to unmiss a turn
         */
        public void ToggleMissTurn(bool b) => missTurn = b;
        
        /*
         * Invoked to toggle if a unit is selected in range 
         */
        public void ToggleUnitInRange(bool inRange)
        {
            //if destroyed
            if (this == null) return;
            
            if (inRange)
                quad.SetActive(true);
            else
                quad.SetActive(false);
        }   

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
        public bool ShouldMissTurn() => missTurn;
        public int GetActionPoints() => actionPoints;
    }
}