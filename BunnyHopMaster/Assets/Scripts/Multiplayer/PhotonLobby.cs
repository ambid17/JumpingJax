using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PhotonLobby : MonoBehaviourPunCallbacks
{
    public GlobalDebug debugger;

    public static PhotonLobby lobby;

    public GameObject joinButton;
    public GameObject cancelButton;

    private void Awake()
    {
        debugger = GetComponent<GlobalDebug>();
        lobby = this; //creates singleton in lobby scene
    }

    void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    void Update()
    {
        
    }

    public override void OnConnectedToMaster()
    {
        debugger.Log("Player connected to master server");
        joinButton.SetActive(true);

    }
    

    public void OnJoinButtonClicked()
    {
        joinButton.SetActive(false);
        cancelButton.SetActive(true);
        PhotonNetwork.JoinRandomRoom();
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        debugger.Log("Couldn't join random room, there must not be any available, creating one now");
        CreateRoom();
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        debugger.Log("Couldn't create room, must be one with same name, making a new one");
        CreateRoom();
    }

    public override void OnJoinedRoom()
    {
        debugger.Log("We have joined a room");
    }

    void CreateRoom()
    {
        debugger.Log("trying to create a new room");
        int randomRoomName = Random.Range(0, 1000);
        RoomOptions roomOptions = new RoomOptions() { IsVisible = true, IsOpen = true, MaxPlayers = 10 };
        PhotonNetwork.CreateRoom("Room" + randomRoomName, roomOptions);
    }

    public void OnCancelButtonClicked()
    {
        cancelButton.SetActive(false);
        joinButton.SetActive(true);
        PhotonNetwork.LeaveRoom();
    }
}
