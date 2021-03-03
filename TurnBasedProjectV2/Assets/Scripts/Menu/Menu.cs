using Managers;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Menu
{
    /*
     * TODO: Write a disconnected method that reroutes to home menu
     * TODO: Write a lobby feature (similar to what I've already implemented)
     */
    public class Menu : MonoBehaviourPunCallbacks
    {
        [Header("Screens")]
        public GameObject mainScreen;
        public GameObject lobbyScreen;

        [Header("Main Screen")]
        public Button playButton;

        [Header("Lobby Screen")]
        public TextMeshProUGUI player1NameText;
        public TextMeshProUGUI player2NameText;
        public TextMeshProUGUI gameStartingText;

        /*
         * Initial methods
         */
        private void Start ()
        {
            // disable the play button before we connect to the master server
            playButton.interactable = false;
            gameStartingText.gameObject.SetActive(false);
        }

        /*
         * Change UI Screens
         */
        private void SetScreen (GameObject screen)
        {
            // disable all screens
            mainScreen.SetActive(false);
            lobbyScreen.SetActive(false);

            // enable the requested screen
            screen.SetActive(true);
        }
        
        /*
        * Starts game if there are at least 2 players in room
        */
        private void TryStartGame ()
        {
            if(PhotonNetwork.PlayerList.Length == 2)
                NetworkManager.instance.photonView.RPC("ChangeScene", RpcTarget.All, "Game 2");
            else 
                gameStartingText.gameObject.SetActive(false);
        }

        /*
         * Called when player presses the leave button, and invokes PhotonNetwork.LeaveRoom
         */
        public void OnLeaveButton ()
        {
            PhotonNetwork.LeaveRoom();
            SetScreen(mainScreen);
        }
        
        /*
         * Updates lobby UI for all connected players
         */
        [PunRPC]
        private void UpdateLobbyUI ()
        {
            // set the player name texts
            player1NameText.text = PhotonNetwork.CurrentRoom.GetPlayer(1).NickName;
            player2NameText.text = PhotonNetwork.PlayerList.Length == 2 ? PhotonNetwork.CurrentRoom.GetPlayer(2).NickName : "...";

            // return if we have too many or too few players
            if (PhotonNetwork.PlayerList.Length != 2) return;
            
            gameStartingText.gameObject.SetActive(true);

            if(PhotonNetwork.IsMasterClient)
                Invoke("TryStartGame", 3.0f);
        }
        
        /*
         * When WE connect to another server
         */
        public override void OnConnectedToMaster () => playButton.interactable = true;
        
        /*
         * Updates player nickname
         */
        public void OnUpdatePlayerNameInput (TMP_InputField nameInput) => PhotonNetwork.NickName = nameInput.text;
        
        /*
         * When play button is pressed, you either join a room as a client, or create a room as a master
         */
        public void OnPlayButton () => NetworkManager.CreateOrJoinRoom();

        /*
         * Called when local player joins a room
         */
        public override void OnJoinedRoom ()
        {
            SetScreen(lobbyScreen);
            photonView.RPC("UpdateLobbyUI", RpcTarget.All);
        }

        /*
         * Called when player leaves a room
         */
        public override void OnPlayerLeftRoom (Player otherPlayer) => UpdateLobbyUI();
    }
}