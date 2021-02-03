
using UnityEngine;
using Photon.Pun;
using System.Collections.Generic;
using Photon.Realtime;

public class RoomListingBox : MonoBehaviourPunCallbacks
{
    [SerializeField] private RoomListing _roomListingsPrefab; //Instantiate directly to script
    [SerializeField] private Transform _content;
    private List<RoomListing> _listings = new List<RoomListing>();

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        foreach (RoomInfo info in roomList)
        {

            //if removed from list
            if (info.RemovedFromList)
            {
                //compare room names (which are unique)
                //return index of whatever listing has the same roomname as name received
                int index = _listings.FindIndex( x => x.getRoomInfo.Name == info.Name);

                //if -1 which means if its found, destroy listing and remove from index
                if (index != -1) {
                    Destroy(_listings[index].gameObject);
                    _listings.RemoveAt(index);
                }
            }

            //add them to list
            else
            {
                RoomListing listing = Instantiate(_roomListingsPrefab, _content);
                if (listing != null)
                {
                    listing.SetRoomInfo(info);
                    _listings.Add(listing);
                }
            }


        }
    }
}
