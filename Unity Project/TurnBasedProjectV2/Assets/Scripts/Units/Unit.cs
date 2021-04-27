using System;
using System.Collections;
using Managers;
using Photon.Pun;
using UI;
using UnityEngine;
using UnityEngine.Serialization;

namespace Units
{
    public class Unit : MonoBehaviourPun
    {
      
        /*
         * Set in inspector once at call time
         */
        [Header("Unit Properties")]
        //
        [SerializeField] private int setHp; 
        [SerializeField] private int setDefence;
        [SerializeField] private float moveSpeed; // units movement speed
        [SerializeField] private int moveDistance; // max distance we can move per turn
        [SerializeField] private int unitID; // ID reference of unit
        [SerializeField] private string unitName; // ID reference of unit
        [SerializeField] private string[] unitInformation; // ID reference of unit
        [SerializeField] private GameObject quad; //quad that shows up when in range
        [SerializeField] private GameObject selectionQuad; //quad that shows up when selected

        public int MaxHp {get; set; } // max health points a unit has
        public int MaxDefence {get; set; } // max defence points a unit has
        public int CurrentHp {get; set; } // current HP
        public int CurrentDef {get; set; } // current defence points a unit has
        public int ActionPoints { get; set; } //current number of action points a unit can have
        public int MaxActionPoints { get; set; } //current number of action points a unit can have

        private bool _hasMovedThisTurn;
        private bool _attackedThisTurn;
        private bool _isSelected;
        private bool _missTurn;
        private bool _waitingToAttack;
        
        /*
         * Initiate units current health and defence (which are variables)
         */
        private void Start()
        {
            CurrentHp = setHp;
            MaxHp = setHp;
            CurrentDef = setDefence;
            MaxDefence = setDefence;
            quad.SetActive(false);
            selectionQuad.SetActive(false);
            MaxActionPoints = 6;
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
            if (amount < 0)
            {
                Debug.Log("Action point value is less than 0!");
                amount = 0;
            }
            
            ActionPoints += amount;

            if (ActionPoints > MaxActionPoints)
                ActionPoints = MaxActionPoints;
        }
        
        /*
         * Decrements action points, called when a move is used by a unit 
         */
        public void DecrementActionPoints(int amount)
        {
            //Remove negative inputs and throw an error
            if (amount < 0)
            {
                Debug.Log("Action point value is less than 0!");
                amount = 0;
            }
            
            ActionPoints -= amount;

            if (ActionPoints < 0)
                ActionPoints = 0;
        }

        /*
         * Invoked when another unit attacks this unit, and instructs this unit to take damage
         */
        [PunRPC]
        private void TakeDamage(int damage)
        {
            //Removes damage from armour then, then remainder from health
            CalculateDamage(damage);
            
            if (photonView.IsMine)
                GameUI.instance.AppendHistoryLog(unitName + " taking damage. Health: " + CurrentHp + ". Defence: " + CurrentDef + ".");
            
            if (CurrentHp <= 0)
                photonView.RPC("UnitHasDied", RpcTarget.All);
        }

        /*
         * First subtracts damage from defence, and then from health
         */
        public void CalculateDamage(int damage)
        {
            //Remove negative inputs and throw an error
            if (damage < 0)
            {
                Debug.Log("Damage is less than 0!");
                damage = 0;
            }
            
            int defenceDamage = Math.Min(CurrentDef, damage);
            int healthDamage = Math.Min(CurrentHp, damage - defenceDamage);
            
            CurrentDef -= defenceDamage;
            CurrentHp -= healthDamage;

        }

        /*
         * Invoked when another unit is trying to remove shields
         */
        [PunRPC]
        private void DamageShields(int damage)
        {
            if (CurrentDef == 0)
            {
                GameUI.instance.AppendHistoryLog(unitName + " shields already down. No net result!");
                return;
            }

            //reduce current defence by damage
            CurrentDef -= damage;

            //but if it's 0, set to 0
            if (CurrentDef < 0)
                CurrentDef = 0;
            
            if (photonView.IsMine)
                GameUI.instance.AppendHistoryLog(unitName + " shields taking damage. Defence: " + CurrentDef + ".");
        }

        /*
         * Invoked when another unit attacks this unit and bypasses the defence
         */
        [PunRPC]
        private void BypassDefence(int damage)
        {
            CalculateBypassDefence(damage);
            
            if (photonView.IsMine)
                GameUI.instance.AppendHistoryLog(unitName + " taking damage (ignoring shields). Health: " + CurrentHp + ".");
            
            if (CurrentHp <= 0)
                photonView.RPC("UnitHasDied", RpcTarget.All);
        }

        /*
         * Invoked when a units defence is bypassed
         */
        public void CalculateBypassDefence(int damage)
        {
            
            //Remove negative inputs and throw an error
            if (damage < 0)
            {
                Debug.Log("Damage is less than 0!");
                damage = 0;
            }
            //reduce current defence by damage
            CurrentHp -= damage;
   

            //but if it's 0, set to 0
            if (CurrentHp < 0)
                CurrentHp = 0;
        }

        /*
         * Invoked when a units defence is boosted
         */
        public void BoostDefence(int defence)
        {
            CalculateBoostDefence(defence);
            GameUI.instance.AppendHistoryLog(unitName + " shields restored to " + CurrentDef);
        }

        public void CalculateBoostDefence(int defence)
        {
            //Remove negative inputs and throw an error
            if (defence < 0)
            {
                Debug.Log("Defence is less than 0!");
                defence = 0;
            }
            
            CurrentDef += defence;
            
            if (CurrentDef > MaxDefence)
                CurrentDef = MaxDefence;
        }

        /*
         * Invoked  when the unit's health reaches 0
         */
        [PunRPC]
        private void UnitHasDied()
        {
            if (!photonView.IsMine)
            {
                GameUI.instance.AppendHistoryLog("Enemy " + unitName + " has died!");
                PlayerController.enemy.units.Remove(this);
            }
            else
            {
                GameUI.instance.AppendHistoryLog(unitName + " has died!");
                PlayerController.me.units.Remove(this);
                GameManager.instance.CheckWinCondition();
                PhotonNetwork.Destroy(gameObject);
            }
        }

        /*
         * Causes unit to miss a turn
         */
        [PunRPC]
        private void MissTurn()
        {
            //update status bar
            if (photonView.IsMine)
                GameUI.instance.AppendHistoryLog(unitName + " will miss the next turn");
            _missTurn = true;
        } 
        

        /*
        * Invoked to change a units selected status
        */
        public void ToggleSelect(bool selected)
        {
            _isSelected = selected;
            
            //updates selected quad
            if (_isSelected)
                selectionQuad.SetActive(true);
            else 
                selectionQuad.SetActive(false);
        } 

        /*
        * Invoked to change a units used status
        */
        public void ToggleMovedThisTurn(bool hasMoved) => _hasMovedThisTurn = hasMoved;

        /*
        * Invoked to change a units used status
        */
        public void ToggleAttackedThisTurn(bool attacked) => _attackedThisTurn = attacked;
        
        /*
         * Invoked to allow unit to select to something when waiting to attack
        */
        public void ToggleWaitingToAttack(bool isWaiting) => _waitingToAttack = isWaiting;

        /*
         * Invoked to unmiss a turn
         */
        public void ToggleMissTurn(bool b) => _missTurn = b;
        
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
        public bool MovedThisTurn() => _hasMovedThisTurn;
        public bool IsSelected() => _isSelected;
        public int GetMovementDistance() => moveDistance;
        public float GetMovementSpeed() => moveSpeed;
        public bool AttackedThisTurn() => _attackedThisTurn;
        public int GetUnitID() => unitID;
        public string GetUnitName() => unitName;
        public string[] GetUnitInformation() => unitInformation;
        public bool ShouldMissTurn() => _missTurn;
        public bool WaitingToAttack() => _waitingToAttack;
    }
}