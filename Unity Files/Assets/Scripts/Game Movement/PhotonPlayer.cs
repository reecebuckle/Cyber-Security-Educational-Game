using UnityEngine;
using Photon.Pun;
using System.IO;

/*
* Simple script to test synchronicity between moving cubes
*/
public class PhotonPlayer : MonoBehaviour
{

    // TODO: Add Application.Quit(); to handle when a player is left
    public float moveSpeed = 3f;
    [SerializeField] private GameObject myAvatar;
    [SerializeField] private PhotonView PV;

    void Update()
    {
        //if not instantiated yet
        // if (PV != null)
        //   return;

        if (PV.IsMine)
        {
            //Moves Forward and back along z axis                           //Up/Down
            transform.Translate(Vector3.forward * Time.deltaTime * Input.GetAxis("Vertical") * moveSpeed);
            //Moves Left and right along x Axis                               //Left/Right
            transform.Translate(Vector3.right * Time.deltaTime * Input.GetAxis("Horizontal") * moveSpeed);
        }
    }
}
