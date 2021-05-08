using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;


namespace Managers
{
    public class NetworkManager : MonoBehaviourPun
    {
        //Create a singleton instance of network manager
        public static NetworkManager instance;

        /*
         * Assign singleton Instance
         * Invoked when MonoBheaviour is created (the default constructor)
         */
        private void Awake()
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }

        /*
         * Invoked after awake when all Initialisation is done
         * Essentially called during the first frame as the behaviour begins running
         * Because of this, this can lead into a co-routine if necessary
         */
        private void Start() => PhotonNetwork.ConnectUsingSettings();


        /*
         * Utility function:
         * Joins a room if one is available, else creates a room with 2 max players
         */
        public void CreateOrJoinRoom()
        {
            if (PhotonNetwork.CountOfRooms > 0)
                PhotonNetwork.JoinRandomRoom();

            else
            {
                RoomOptions options = new RoomOptions();
                options.MaxPlayers = 2;
                PhotonNetwork.CreateRoom(null, options);
            }
        }

        /*
         * Load Game scene after menu
         */
        [PunRPC]
        public void LoadGameScene() => PhotonNetwork.LoadLevel(1);


        /*
         * Load Menu Scene after winning/lossing
         */
        public void LoadMenu() => SceneManager.LoadScene(0);
    }
}