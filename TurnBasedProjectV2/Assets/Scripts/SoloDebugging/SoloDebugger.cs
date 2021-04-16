using Photon.Pun;
using UnityEngine;

namespace SoloDebugging
{
    /*
    * Solo debugger used for instantiating units
    */
    public class SoloDebugger : MonoBehaviour
    {
        [Header("Reference to Solo Spawn Prefabs")]
        private GameObject player1 = null;
        //private GameObject player2 = null;
        [SerializeField] private Transform unit1Spawn;
        [SerializeField] private Transform unit2Spawn;
    
        /*
     * Script used for the purposes of spawning and testing units in single player!
     */
        private void Start()
        {
            Debug.Log("Initialiting units");
        
            player1 = PhotonNetwork.Instantiate("Soldier", 
                unit1Spawn.transform.position,
                unit1Spawn.transform.rotation, 0);
        
            player1 = PhotonNetwork.Instantiate("Tank", 
                unit2Spawn.transform.position,
                unit2Spawn.transform.rotation, 0);
        }

    }
}



