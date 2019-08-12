using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class GameSetup : MonoBehaviour
{
    public static GameSetup GS;

    public Transform[] spawnPoints;

    public Text healthText;

    private const string prefabFolder = "PhotonPrefabs";
    private const string prefabName = "PhotonPlayer";

    private void OnEnable()
    {
        if(GameSetup.GS == null)
        {
            GameSetup.GS = this;
        }
    }

    private void Start()
    {
        Debug.Log("Creating Player");
        PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PhotonPlayer"), Vector3.zero, Quaternion.identity);
    }
}
