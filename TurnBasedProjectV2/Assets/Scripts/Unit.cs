using System.Collections;
using Photon.Pun;
using UnityEngine;

namespace Units
{
    public class Unit : MonoBehaviourPun
    {
        [Header("Unit Properties")] [SerializeField]
        private float moveSpeed; // units per second when moving

        [SerializeField] private int moveDistance; // max distance we can move per turn

        [SerializeField] private int attackDistance; // max distance we can attack
        private bool usedThisTurn; // has this unit been used this turn?
        private bool isSelected;
        
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

        //Getter method for attack distance
        public int GetAttackDistance()
        {
            return attackDistance;
        }

        // Getter method to see if we can use this unit
        public bool CanSelect()
        {
            return usedThisTurn;
        }


        /*public int curHp;               // current health
    public int maxHp;               // maximum health
    public float moveSpeed;         // units per second when moving
    public int minDamage;           // minimum damage
    public int maxDamage;           // maximum damage

    public int maxMoveDistance;     // max distance we can move per turn
    public int maxAttackDistance;   // max distance we can attack

    public bool usedThisTurn;       // has this unit been used this turn?

    public GameObject selectedVisual;   // selection circle sprite
    public SpriteRenderer spriteVisual; // sprite of the unit

    [Header("UI")]
    public Image healthFillImage;       // health bar fill image

    [Header("Sprite Variants")]
    public Sprite leftPlayerSprite;     // left player sprite (blue)
    public Sprite rightPlayerSprite;    // right player sprite (red)

    // called when the unit is spawned in
    [PunRPC]
    void Initialize (bool isMine)
    {
        if(isMine) PlayerController.me.units.Add(this);
        else GameManager.instance.GetOtherPlayer(PlayerController.me).units.Add(this);

        healthFillImage.fillAmount = 1.0f;

        // set sprite variant
        spriteVisual.sprite = transform.position.x < 0 ? leftPlayerSprite : rightPlayerSprite;

        // rotate the unit
        spriteVisual.transform.up = transform.position.x < 0 ? Vector3.left : Vector3.right;
    }

    // can we be selected?
    public bool CanSelect ()
    {
        if(usedThisTurn) return false;
        else return true;
    }

    // can we move to this position?
    public bool CanMove (Vector3 movePos)
    {
        if(Vector3.Distance(transform.position, movePos) <= maxMoveDistance)
            return true;
        else return false;
    }

    // can we attack this position?
    public bool CanAttack (Vector3 attackPos)
    {
        if(Vector3.Distance(transform.position, attackPos) <= maxAttackDistance)
            return true;
        else return false;
    }

    /*
     * Invoked when a unit is selected
     #1#
    public void ToggleSelect (bool selected) => selectedVisual.SetActive(selected);
    

    public void Move (Vector3 targetPos)
    {
        usedThisTurn = true;

        // rotate sprite
        Vector3 dir = (transform.position - targetPos).normalized;
        spriteVisual.transform.up = dir;

        StartCoroutine(MoveOverTime());

        IEnumerator MoveOverTime ()
        {
            while(transform.position != targetPos)
            {
                transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);
                yield return null;
            }
        }
    }

    // attacks the requested enemy unit
    public void Attack (Unit unitToAttack)
    {
        usedThisTurn = true;
        unitToAttack.photonView.RPC("TakeDamage", PlayerController.enemy.photonPlayer, Random.Range(minDamage, maxDamage + 1));
    }

    /*
     * Invoked when attatcked by another unit
     #1#
    [PunRPC]
    void TakeDamage (int damage)
    {
        curHp -= damage;

        if (curHp <= 0)
            photonView.RPC("Die", RpcTarget.All);
        else
        {
            // update health UI
            photonView.RPC("UpdateHealthBar", RpcTarget.All, (float)curHp / (float)maxHp);
        }
    }

    /*
     * Updates UI Health Bar
     #1#
    [PunRPC]
    void UpdateHealthBar (float fillAmount) => healthFillImage.fillAmount = fillAmount;
    
    /*
     * Invoked when unit's HP reaches 0
     #1#
    [PunRPC]
    void Die ()
    {
        if(!photonView.IsMine)
            PlayerController.enemy.units.Remove(this);
        else
        {
            PlayerController.me.units.Remove(this);

            // check the win condition
            GameManager.instance.CheckWinCondition();

            // destroy the unit across the network
            PhotonNetwork.Destroy(gameObject);
        }
    }*/
    }
}