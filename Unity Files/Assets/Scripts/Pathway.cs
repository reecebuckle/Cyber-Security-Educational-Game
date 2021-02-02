using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathway : MonoBehaviour
{   
    [Header("Reference to child nodes")]
    public List<Transform> nodeList = new List<Transform>(); //list of children nodes
    [SerializeField] private Transform[] nodes;  //Array of transforms for each node

    // void Awake() {
    //     nodeList = new List<Transform>();
    // }

    //Used to visualise the pathway
    //Can remove at end
    void OnDrawGizmos() {
        Gizmos.color = Color.green;
        FillNodes();

        //loop through node list 
        for (int i = 0; i < nodeList.Count; i++)
        {
            //find vector position of current node in the list
            Vector3 currentPos = nodeList[i].position;
            if (i > 0) {
                //check I is bigger than 0 (not first node) so you can take the previous ones
                Vector3 prevPosition = nodeList[i-1].position;
                Gizmos.DrawLine(prevPosition, currentPos);
            }
        }
    }
    void FillNodes() {
        nodeList.Clear();
        //receive all transforms of all children nodes
        nodes = GetComponentsInChildren<Transform>();

        //loop through each node
        foreach (Transform node in nodes) {

            if (node != this.transform) {
                nodeList.Add(node);
            }
        }
    }


}
