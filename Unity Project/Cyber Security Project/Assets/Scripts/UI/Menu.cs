using Managers;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class Menu : MonoBehaviourPunCallbacks
    {
        [Header("UI Elements")]
        public GameObject menu;
        public GameObject connecting;
        public Button playButton;
        public TextMeshProUGUI player1;
        public TextMeshProUGUI player2;
        public TextMeshProUGUI gameStartingText;

        /*
         * Invoked in the first frame
         */
        private void Start()
        {
            playButton.interactable = false;
            gameStartingText.gameObject.SetActive(false);
        }

        /*
         * Invoked when we connect to the master server 
         */
        public override void OnConnectedToMaster() => playButton.interactable = true;
    
        /*
         * Invoked when we create a room
         */
        public override void OnJoinedRoom()
        {
            menu.SetActive(false);
            connecting.SetActive(true);
            photonView.RPC("UpdateLobbyUI", RpcTarget.All);
        }
        
        /*
         * Invoked when player leaves room
         */
        public override void OnPlayerLeftRoom(Player otherPlayer) => UpdateLobbyUI();

        /*
         * Invoked when a player inputs a name
         */
        public void OnUpdatePlayerNameInput(TMP_InputField nameInput) => PhotonNetwork.NickName = nameInput.text;
        
        /*
         * Invoked When Play Pressed
         */
        public void PlayButton() => NetworkManager.instance.CreateOrJoinRoom();
        
        /*
       * Invoked when the "Leave" button is pressed
       */
        public void LeaveButton()
        {
            PhotonNetwork.LeaveRoom();
            connecting.SetActive(false);
            menu.SetActive(true);
        }
        
        /*
         * Assigns player names to the UI text components
         */
        [PunRPC]
        private void UpdateLobbyUI()
        {
            // set the player name texts
            player1.text = PhotonNetwork.CurrentRoom.GetPlayer(1).NickName;
            player2.text = PhotonNetwork.PlayerList.Length == 2 ? PhotonNetwork.CurrentRoom.GetPlayer(2).NickName : "...";

            // return if not two people
            if (PhotonNetwork.PlayerList.Length != 2) return;
            
            gameStartingText.gameObject.SetActive(true);

            if(PhotonNetwork.IsMasterClient)
                Invoke("StartGame", 3.0f);
        }

        /*
         * Checks if 2 players are in the lobby and if so - start the game
         */
        private void StartGame()
        {
            // if we have 2 players in the lobby, load the Game scene
            if (PhotonNetwork.PlayerList.Length == 2)
                NetworkManager.instance.photonView.RPC("LoadGameScene", RpcTarget.All);
            else
                gameStartingText.gameObject.SetActive(false);
        }

      
    }
}