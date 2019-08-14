using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class PhotonPlayer : MonoBehaviour
{
    private const string prefabFolder = "PhotonPrefabs";
    private const string prefabName = "PlayerAvatar";

    public PhotonView PV;
    public GameObject myAvatar;
    public int team;

    void Start()
    {
        PV = GetComponent<PhotonView>();
        if (PV.IsMine)
        {
            PV.RPC("RPC_GetTeam", RpcTarget.MasterClient);
        }
        
    }

    void Update()
    {
        //wait for the team variable to be set and spawn at the correct team location
        if(myAvatar == null && team != 0)
        {
            if (team == 1)
            {
                int spawnPicker = Random.Range(0, GameSetup.GS.spawnPointsHuman.Length);
                Transform spawnPoint = GameSetup.GS.spawnPointsHuman[spawnPicker];
                if (PV.IsMine)
                {
                    myAvatar = PhotonNetwork.Instantiate(Path.Combine(prefabFolder, prefabName), spawnPoint.position, spawnPoint.rotation, 0);
                }
            }

            if (team == 2)
            {
                int spawnPicker = Random.Range(0, GameSetup.GS.spawnPointsInfected.Length);
                Transform spawnPoint = GameSetup.GS.spawnPointsInfected[spawnPicker];
                if (PV.IsMine)
                {
                    myAvatar = PhotonNetwork.Instantiate(Path.Combine(prefabFolder, prefabName), spawnPoint.position, spawnPoint.rotation, 0);
                }
            }
        }
    }

    [PunRPC]
    void RPC_GetTeam()
    {
        //set this players team on the master client
        team = GameSetup.GS.nextPlayersTeam;
        GameSetup.GS.UpdateTeam();
        PV.RPC("RPC_SentTeam", RpcTarget.OthersBuffered, team);
    }

    [PunRPC]
    void RPC_SentTeam(int team)
    {
        this.team = team;
    }
}
