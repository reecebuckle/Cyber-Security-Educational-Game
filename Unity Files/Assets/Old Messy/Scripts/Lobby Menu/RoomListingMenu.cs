
using UnityEngine;
using Photon.Pun;
using System.Collections.Generic;
using Photon.Realtime;
using Utilities;

public class RoomListingMenu : MonoBehaviourPunCallbacks
{
    [SerializeField] private RoomListing roomListingsPrefab; //Instantiate directly to script
    [SerializeField] private Transform content;
    private List<RoomListing> listings = new List<RoomListing>();

    /*
    * Invoked when a player joins a room, destroy old room listings
    */
    public override void OnJoinedRoom()
    {
        content.DestroyChildren();
        listings.Clear();
    }

    /*
    * Auto invoked when the room list is updated
    * Used to populate information about rooms available to other connections
    */
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        Debug.Log("New room added/deleted, updating list accordingly", this);
        foreach (RoomInfo info in roomList)
        {

            //if removed from list, add them to list
            if (info.RemovedFromList)
            {
                //compare room names (which are unique)
                //return index of whatever listing has the same roomname as name received
                int index = listings.FindIndex(x => x.RoomInfo.Name == info.Name);

                //if -1 which means if its found, destroy listing and remove from index
                if (index != -1)
                {
                    Debug.Log("Removing: " + info.Name + " from list", this);
                    Destroy(listings[index].gameObject);
                    listings.RemoveAt(index);
                }
            }

            //add them to list
            else
            {
                //first check that listing already exists before attempting to add
                // int index = listings.FindIndex(x => x.RoomInfo.Name == info.Name);

                // if (index != -1)
                // {
                RoomListing listing = Instantiate(roomListingsPrefab, content);
                if (listing != null)
                {
                    Debug.Log("Adding: " + info.Name + " to list", this);
                    listing.SetRoomInfo(info);
                    listings.Add(listing);
                }

            }
        }

    }
}


