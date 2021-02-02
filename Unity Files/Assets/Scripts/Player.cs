using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Player Information")]
    public int diceRoll; //simulates a dice roll
    public Pathway pathway; //reference to pathway
    [SerializeField] private int currentPos; 
    [SerializeField] private bool isMoving; 
    

    void Update() {
        //if we press space and not moving, we can simulate rolling the dice
        if (Input.GetKeyDown(KeyCode.Space) && !isMoving) {
            //full integers are always reduced by 1
            diceRoll = Random.Range(1,7);
            Debug.Log("Dice Rolled " + diceRoll);

            //for monopoly style (move from end to start)
            StartCoroutine(MonopolyMove());  

            //ensure if we reached endpoint, we can't move anymore, or we just move until the end point (like LUDO, not monopoly)
            // if (currentPos + diceRoll < pathway.nodeList.Count) 
            //     StartCoroutine(Move());
            // else {
            //     Debug.Log("Rolled number is too high, capping limit"); 
            // }
    

        }

    }

    /*
    * Avoid using update loop for anyhting other than player input, use coroutines!
    * For monopoly style movement
    */
    IEnumerator MonopolyMove() {
        //break check if moving
        if (isMoving) {
            yield break;
        }
    
        isMoving = true;

        //simulate moving until diceroll reaches 0
        while (diceRoll > 0) {

            //For monopoly style logic
            currentPos++;
            //not able to overdo count, reset back to 0
            currentPos %= pathway.nodeList.Count;

            //find next position to move to, and move to it
            Vector3 nextPos = pathway.nodeList[currentPos].position;

            //wait until we reach to next point
            while (MoveToNextNode(nextPos)) {

                yield return null;
            }

            //update dice rolls remaining and increment currentPos
            yield return new WaitForSeconds(0.1f);
            diceRoll--;
        }
     
        isMoving = false;
    }

    /*
    * For Ludo style movement
    */
    IEnumerator LudoMove() {
        //break check if moving
        if (isMoving) 
            yield break;
    
        isMoving = true;

        //simulate moving until diceroll reaches 0
        while (diceRoll > 0) {
            //find next position to move to, and move to it
            Vector3 nextPos = pathway.nodeList[currentPos + 1].position;

            //wait until we reach to next point
            while (MoveToNextNode(nextPos)) 
                yield return null;

            //update dice rolls remaining and increment currentPos
            yield return new WaitForSeconds(0.1f);
            diceRoll--;
            currentPos++;
        }
        isMoving = false;
    }

    /*
    * Checks if we can move to the next node
    */
    bool MoveToNextNode (Vector3 targetNode) {
        // bool targetReached = false;
        // //attempt to move towards from current pos, to target node at speed 1f
        // transform.position = Vector3.MoveTowards(transform.position, targetNode, 1f * Time.deltaTime);
        // //check if target node reached this position
        // if (targetNode == transform.position) 
        //     targetReached = true;

        // return targetReached;
        return targetNode != (transform.position = Vector3.MoveTowards(transform.position, targetNode, 1f * Time.deltaTime));

    }

}
