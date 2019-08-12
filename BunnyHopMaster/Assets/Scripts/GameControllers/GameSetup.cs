using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
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

    public void DisconnectPlayer()
    {
        StartCoroutine(DisconnectAndLoad());
    }

    IEnumerator DisconnectAndLoad()
    {
        //TODO: make sure to sync all variables and make sure everything correct is destroyed 
        //SO, once object picking up is done, make sure the object isn't deleted when a player disconnects
        //Use PhotonNetwork.InstantiateSceneObject() instead of .Instantiate()
        PhotonNetwork.LeaveRoom();
        while (PhotonNetwork.InRoom)
        {
            yield return null;
        }

        //TODO use MultiplayerSettings.menuScene
        SceneManager.LoadScene(0);
    }
}
