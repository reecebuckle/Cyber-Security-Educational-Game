
using UnityEngine;
using Photon.Pun;
using System.Collections.Generic;
using Photon.Realtime;

public class PlayerListingMenu : MonoBehaviourPunCallbacks
{
    [SerializeField] private PlayerListing playerListingsPrefab; //Instantiate directly to script
    [SerializeField] private Transform content;
    private List<PlayerListing> listings = new List<PlayerListing>();

    void Awake() => GetCurrentRoomPLayers();


    /*
    * not sure what used for just yet!
    */
    // public override void OnEnable()
    // {
    //     base.OnEnable();
    //     //SetReadyUp(false);
    //     GetCurrentRoomPLayers();
    // }

    // public override void OnDisable()
    // {
    //     base.OnDisable();
    //     for (int i = 0; i < listings.Count; i++) 
    //         Destroy(listings[i].gameObject);

    //     listings.Clear();
    //}

    /*
    * TODO: ADD error tracking so you aren't in a room before calling this
    * Returns the current players within the room
    */
    private void GetCurrentRoomPLayers()
    {
        Debug.Log("Retrieving current players in room", this);

        // if any of these are null (or not connected), return to prevent null exception!
        if (!PhotonNetwork.IsConnected || PhotonNetwork.CurrentRoom == null || PhotonNetwork.CurrentRoom.Players == null) 
            return;

        // Players returns a dictionary of players, so irterate through dictionary as key value pair
        foreach (KeyValuePair<int, Player> playerInfo in PhotonNetwork.CurrentRoom.Players)
            AddPlayerListing(playerInfo.Value); //
    }

    /*
    * Instantiate a new player listing prefab and set the player's info to it
    */
    private void AddPlayerListing(Player player)
    {
        Debug.Log("Adding player: " + player.NickName, this);
        
        // First check if a listing exists, just update it rather than create a new one
        int index = listings.FindIndex(x => x.Player == player);
        if (index != -1)
            listings[index].SetPlayerInfo(player);

        // Create a new listing   
        else
        {
            PlayerListing listing = Instantiate(playerListingsPrefab, content);
            if (listing != null)
            {
                listing.SetPlayerInfo(player);
                listings.Add(listing);
            }
        }
    }

    /*
    * Overrided PUN method, used to add player to a listing when entering a room
    */
    public override void OnPlayerEnteredRoom(Player newPlayer) => AddPlayerListing(newPlayer);

    /*
    * Overrided PUN method, used to remove player when leaving a room
    * TODO: Delete this entirely
    */
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
       
        //compare room names (which are unique)
        //return index of whatever listing has the same roomname as name received
        int index = listings.FindIndex(x => x.Player == otherPlayer);

        //if -1 which means if its found, destroy listing and remove from index
        if (index != -1)
        {
            Debug.Log("Removing player: " + otherPlayer.NickName + " from room", this);
            Destroy(listings[index].gameObject);
            listings.RemoveAt(index);
        }

    }

    /*
    * Invoked when a player leaves the room, destroys old player listings
    */
    public override void OnLeftRoom() => content.DestroyChildren();

    /*
    * Invoked when the client has joined their own room
    */
    public override void OnJoinedRoom()
    {
        Debug.Log("Client host has joined their own room");
        GetCurrentRoomPLayers();
    }
}
