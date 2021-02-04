
using UnityEngine;
using Photon.Pun;
using System.Collections.Generic;
using Photon.Realtime;

public class PlayerListingBox : MonoBehaviourPunCallbacks
{
    [SerializeField] private PlayerListing _playerListingsPrefab; //Instantiate directly to script
    [SerializeField] private Transform _content;
    private List<PlayerListing> _players = new List<PlayerListing>();

    //if this returns null pointer, call it from start or from the create lobby method!
    private void Awake() => GetCurrentRoomPLayers();

    /*
    * TODO: ADD error tracking so you aren't in a room before calling this
    * Returns the current players within the room
    */
    private void GetCurrentRoomPLayers() {
    
        // Players returns a dictionary of players, so irterate through dictionary as key value pair
        foreach (KeyValuePair<int, Player> playerInfo in PhotonNetwork.CurrentRoom.Players) 
            AddPlayerListing(playerInfo.Value); //
    }

    /*
    * Instantiate a new player listing prefab and set the player's info to it
    */
    private void AddPlayerListing(Player player) {
       PlayerListing listing = Instantiate(_playerListingsPrefab, _content);
        if (listing != null)
        {
            listing.SetPlayerInfo(player);
            _players.Add(listing);
        }
    }

    /*
    * Overrided PUN method, used to add player to a listing when entering a room
    */
    public override void OnPlayerEnteredRoom(Player newPlayer) => AddPlayerListing(newPlayer);

    /*
    * Overrided PUN method, used to remove player when leaving a room
    */
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        //compare room names (which are unique)
        //return index of whatever listing has the same roomname as name received
        int index = _players.FindIndex(x => x.Player == otherPlayer);

        //if -1 which means if its found, destroy listing and remove from index
        if (index != -1)
        {
            Destroy(_players[index].gameObject);
            _players.RemoveAt(index);
        }

    }
}
