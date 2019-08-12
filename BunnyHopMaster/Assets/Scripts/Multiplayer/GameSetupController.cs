using Photon.Pun;
using System.IO;
using UnityEngine;

public class GameSetupController : MonoBehaviour
{
    // This script will be added to any multiplayer scene
    void Start()
    {
        CreatePlayer(); //Create a networked player object for each player that loads into the multiplayer scenes.
    }

    private void CreatePlayer()
    {
        Debug.Log("Creating Player");
        PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PhotonPlayer"), Vector3.zero, Quaternion.identity);
    }
}
