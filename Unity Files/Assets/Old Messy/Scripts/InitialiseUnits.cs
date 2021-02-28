using UnityEngine;
using Photon.Pun;

/*
* Initialises player game objects depending on the connections (from each client)
*/
public class InitialiseUnits: MonoBehaviour
{

    [Header("Reference Player Spawn Points")]
    private GameObject player1 = null;
    private GameObject player2 = null;

    [SerializeField] private Transform player1Spawn;
    [SerializeField] private Transform player2Spawn;

    
    /*
    * Invoked when game scene is loaded, spawns players dependent on the connections
    * As players are initialised, their objects are instantiated and no longer null
    */
    void Start()
    {

        //spawn player one
        if (PhotonNetwork.IsMasterClient && player1 == null)
        {
            Debug.Log("Initilising Player 1 / Host", this);
            player1 = PhotonNetwork.Instantiate("Mock P 1", // make sure this prefab is located in photon/resources folder
                player1Spawn.transform.position,
                player1Spawn.transform.rotation, 0);
        }

        //spawn player 2
        else if (player2 == null)
        {
            Debug.Log("Initilising Player 2", this);
            player2 = PhotonNetwork.Instantiate("Mock P 2",
                player2Spawn.transform.position,
                player2Spawn.transform.rotation, 0);
        }
    }
}