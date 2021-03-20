using Photon.Pun;
using UI;
using Units;

namespace Managers
{
    public class GameManager : MonoBehaviourPun
    {
        public PlayerController leftPlayer;
        public PlayerController rightPlayer;

        public PlayerController curPlayer; // the player who's currently having their turn

        public float postGameTime; // time between the game ending and us going back to the menu

        // Create a singleton instance of the Game Manager
        public static GameManager instance;

        private void Awake() => instance = this;

        /*
         * Master server initialises players / units
        */
        private void Start() => SetPlayers();

        /*
        * Master client initialises the player data and spawns units for each player
        */
        private void SetPlayers()
        {
            // Return if not master
            if (!PhotonNetwork.IsMasterClient) return;

            // Assign left and right player to a photon view ID
            leftPlayer.photonView.TransferOwnership(1);
            rightPlayer.photonView.TransferOwnership(2);

            // Initialize the players
            // AllBuffered is used in case all clients haven't connected yet
            leftPlayer.photonView.RPC("Initialize", RpcTarget.AllBuffered, PhotonNetwork.CurrentRoom.GetPlayer(1));
            rightPlayer.photonView.RPC("Initialize", RpcTarget.AllBuffered, PhotonNetwork.CurrentRoom.GetPlayer(2));

            // Tell all clients that player 1 is going first and player 2 is going second
            photonView.RPC("SetNextTurn", RpcTarget.AllBuffered);
        }

        /*
        * Messages other clients to set next turn locally for them too
        * The first call will be set as current player and will go first (typically player 1)
        * Player 2 will be assigned as the right player and go second
        */
        [PunRPC]
        private void SetNextTurn()
        {
            // Called on the very first turn
            if (curPlayer == null)
                curPlayer = leftPlayer;
            else
                curPlayer = curPlayer == leftPlayer ? rightPlayer : leftPlayer;

            // if it's our turn - enable the end turn button
            if (curPlayer == PlayerController.me)
                PlayerController.me.BeginTurn();
            
            // toggle the end turn button
            GameUI.instance.ToggleEndTurnButton(curPlayer == PlayerController.me);
        }

        /*
        * Returns the opposing player from the one sent
        */
        public PlayerController GetOtherPlayer(PlayerController player)
        {
            return player == leftPlayer ? rightPlayer : leftPlayer;
        }

        /*
        * Called by a player when their unit dies
        * If this reaches 0 we invoke Win Game!
        */
        public void CheckWinCondition()
        {
            if (PlayerController.me.units.Count == 0)
                photonView.RPC("WinGame", RpcTarget.All, PlayerController.enemy == leftPlayer ? 0 : 1);
        }

        /*
        * Called when a player has defeated all of the other player's units
        */
        [PunRPC]
        private void WinGame(int winner)
        {
            // get the winning player
            PlayerController player = winner == 0 ? leftPlayer : rightPlayer;

            // Display the winning text
            GameUI.instance.DisplayWinText(player.photonPlayer.NickName);

            // go back to the menu after a few seconds
            Invoke("GoBackToMenu", postGameTime);
        }

        /*
        * Returns to main menu
        */
        private void GoBackToMenu()
        {
            PhotonNetwork.LeaveRoom();
            NetworkManager.instance.ChangeScene("Menu");
        }
    }
}