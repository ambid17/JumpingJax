using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class PhotonPlayer : MonoBehaviour
{
    private const string prefabFolder = "PhotonPrefabs";
    private const string prefabName = "PlayerAvatar";
    private PhotonView PV;

    public GameObject myAvatar;

    void Start()
    {
        PV = GetComponent<PhotonView>();
        int spawnPicker = Random.RandomRange(0, GameSetup.GS.spawnPoints.Length);
        Transform spawnPoint = GameSetup.GS.spawnPoints[spawnPicker];
        if (PV.IsMine)
        {
            myAvatar = PhotonNetwork.Instantiate(Path.Combine(prefabFolder, prefabName), spawnPoint.position, spawnPoint.rotation, 0);
        }
    }

    void Update()
    {
        
    }
}
