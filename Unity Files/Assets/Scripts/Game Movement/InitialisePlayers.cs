using UnityEngine;
using Photon.Pun;

/*
* Initialises player game objects depending on the connections (from each client)
*/
public class InitialisePlayers : MonoBehaviour
{

    [Header("Reference Player Spawn Points")]
    private GameObject player1 = null;
    private GameObject player2 = null;
    private GameObject player3 = null;
    private GameObject player4 = null;

    [SerializeField] private Transform player1Spawn;
    [SerializeField] private Transform player2Spawn;
    [SerializeField] private Transform player3Spawn;
    [SerializeField] private Transform player4Spawn;


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
            player1 = PhotonNetwork.Instantiate("Player 1", // make sure this prefab is located in photon/resources folder
                player1Spawn.transform.position,
                player1Spawn.transform.rotation, 0);
        }

        //spawn player 2
        else if (player2 == null)
        {
            Debug.Log("Initilising Player 2", this);
            player2 = PhotonNetwork.Instantiate("Player 2",
                player2Spawn.transform.position,
                player2Spawn.transform.rotation, 0);
        }

        //spawn player 3
        else if (player3 == null)
        {
            Debug.Log("Initilising Player 3", this);
            player3 = PhotonNetwork.Instantiate("Player 3",
                player3Spawn.transform.position,
                player3Spawn.transform.rotation, 0);
        }
        //spawn player 4
        else if (player4 == null)
        {
            Debug.Log("Initilising Player 4", this);
            player4 = PhotonNetwork.Instantiate("Player 4",
                player4Spawn.transform.position,
                player4Spawn.transform.rotation, 0);
        }
    }
}

