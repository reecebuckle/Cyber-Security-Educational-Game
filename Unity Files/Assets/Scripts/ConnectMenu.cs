using UnityEngine;
using Photon.Pun;
using TMPro;

// public class LoadMenu : MonoBehaviourPunCallbacks
// {
//     [Header("Connect Menu Information")]
//     [SerializeField] private GameObject opponentPanel = null;
//     [SerializeField] private GameObject waitingPanel = null;
//     [SerializeField] private TextMeshProUGUI waitingText = null;
//     [SerializeField] private bool isConnecting = false;
//     [SerializeField] const string GameVersion = "0.1"; //esnures same versions are matched together
//     [SerializeField] const int MaxPlayers = 4;
//     [SerializeField] const int MinPlayers = 2;

//     //all players will go to the scene
//     private void Awake() => PhotonNetwork.AutomaticallySyncScene = true;
   
//     public void MatchWithPlayers() {
//         isConnecting = true;

//         opponentPanel.SetActive(false);
//         waitingPanel.SetActive(true);
//         waitingText.text = "Searching...";

//         //if already connected, join a random room
//         if (PhotonNetwork.IsConnected) 
//             PhotonNetwork.JoinRandomRoom();

//         //else connect with users on the same game version   
//         else {
//             PhotonNetwork.GameVersion = GameVersion;
//             PhotonNetwork.ConnectUsingSettings();
//         }

//     }

//     public override void OnConnectedToMaster() {
//         Debug.Log("Connected to Master");

//         if (isConnecting)
//             PhotonNetwork.JoinRandomRoom();
//     }

//     public override void OnDisconnected(DisconnectCause cause) {
//         waitingPanel.SetActive(false);
//         opponentPanel.SetActive(true);

//         Debug.LogWarning($"Disconnected due to: {cause}");
//     }

//     public override void OnJoinRandomFailed(short returnCode, string message) {
//         Debug.Log("No clients are waiting for an opponent - will proceed to create new room");

//         //default name (null), establish min and max players
//         PhotonNetwork.CreateRoom(null, new RoomOptions{ MaxPlayers = MaxPlayers, MinPlayers = MinPlayers});
//     }

//     public override void OnJoinedRoom() {
//         Debug.Log("Client successfully joined");
//         int playerCount = PhotonNetwork.CurrentRoom.PlayerCount;

//         if (playerCount != MaxPlayers) {
//             waitingText.text = "Waiting for players to join...";
//             Debug.Log("Client is waiting for an opponent");
//         }
//         else {
//             waitingText.text = "Player found / matched with";
//         }
//     }

//     public override void OnPlayerEnteredRoom(Player newPlayer) {
//         if (PhotonNetwork.CurrentRoom.PlayerCount == MaxPlayers) {
//             //close room
//             PhotonNetwork.CurrentRoom.IsOpen = false;
            
//             waitingText.text = "Player has joined the room";

//             //Load the main game scene!
//             PhotonNetwork.LoadLevel("Main Game");
//         }
//     }
// 
//}
