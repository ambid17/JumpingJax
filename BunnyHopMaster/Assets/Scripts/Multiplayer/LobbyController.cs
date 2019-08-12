using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LobbyController : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private GameObject lobbyConnectButton; //button used for joining a Lobby.
    [SerializeField]
    private GameObject lobbyPanel; //panel for displaying lobby.
    [SerializeField]
    private GameObject mainPanel; //panel for displaying the main menu
    [SerializeField]
    private InputField playerNameInput; //Input field so player can change their NickName

    private string roomName; //string for saving room name
    private int roomSize; //int for saving room size

    private List<RoomInfo> roomListings; //list of current rooms
    [SerializeField]
    private Transform roomsContainer; //container for holding all the room listings
    [SerializeField]
    private GameObject roomListingPrefab; //prefab for displayer each room in the lobby

    public override void OnConnectedToMaster() //Callback function for when the first connection is established successfully.
    {
        PhotonNetwork.AutomaticallySyncScene = true; //Makes it so whatever scene the master client has loaded is the scene all other clients will load
        lobbyConnectButton.SetActive(true); //activate button for connecting to lobby
        roomListings = new List<RoomInfo>(); //initializing roomListing
        
        //check for player name saved to player prefs
        if(PlayerPrefs.HasKey("NickName"))
        {
            if (PlayerPrefs.GetString("NickName") == "")
            {
                PhotonNetwork.NickName = "Player " + Random.Range(0, 1000); //random player name when not set
            }
            else
            {
                PhotonNetwork.NickName = PlayerPrefs.GetString("NickName"); //get saved player name
            }
        }
        else
        {
            PhotonNetwork.NickName = "Player " + Random.Range(0, 1000); //random player name when not set
        }
        playerNameInput.text = PhotonNetwork.NickName; //update input field with player name
    }

    public void PlayerNameUpdateInputChanged(string nameInput) //input function for player name. paired to player name input field
    {
        PhotonNetwork.NickName = nameInput;
        PlayerPrefs.SetString("NickName", nameInput);
    }

    public void JoinLobbyOnClick() //Paired to the Delay Start button
    {
        mainPanel.SetActive(false);
        lobbyPanel.SetActive(true);
        PhotonNetwork.JoinLobby(); //First tries to join a lobby
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList) //Once in lobby this function is called every time there is an update to the room list
    {
        int tempIndex;
        foreach (RoomInfo room in roomList) //loop through each room in room list
        {
            if (roomListings != null) //try to find existing room listing
            {
                tempIndex = roomListings.FindIndex(ByName(room.Name)); 
            }
            else
            {
                tempIndex = -1;
            }
            if (tempIndex != -1) //remove listing because it has been closed
            {
                roomListings.RemoveAt(tempIndex);
                Destroy(roomsContainer.GetChild(tempIndex).gameObject);
            }
            if (room.PlayerCount > 0) //add room listing because it is new
            {
                roomListings.Add(room);
                ListRoom(room);
            }
        }
    }


    static System.Predicate<RoomInfo> ByName(string name) //predicate function for seach through room list
    {
        return delegate (RoomInfo room)
        {
            return room.Name == name;
        };
    }

    void ListRoom(RoomInfo room) //displays new room listing for the current room
    {
        if (room.IsOpen && room.IsVisible)
        {
            GameObject tempListing = Instantiate(roomListingPrefab, roomsContainer);
            RoomButton tempButton = tempListing.GetComponent<RoomButton>();
            tempButton.SetRoom(room.Name, room.MaxPlayers, room.PlayerCount);
        }
    }

    public void RoomNameInputChanged(string nameIn) //input function for changing room name. paired to room name input field
    {
        roomName = nameIn;
    }
    public void OnRoomSizeInputChanged(string sizeIn) //input function for changing room size. paired to room size input field
    {
        roomSize = int.Parse(sizeIn);
    }

    public void CreateRoomOnClick() //function paired to the create room button
    {
        Debug.Log("Creating room now");
        RoomOptions roomOps = new RoomOptions() { IsVisible = true, IsOpen = true, MaxPlayers = (byte)roomSize };
        PhotonNetwork.CreateRoom(roomName, roomOps); //attempting to create a new room
    }

    public override void OnCreateRoomFailed(short returnCode, string message) //create room will fail if room already exists
    {
        Debug.Log("Tried to create a new room but failed, there must already be a room with the same name");
    }

    

    public void MatchmakingCancelOnClick() //Paired to the cancel button. Used to go back to the main menu
    {
        mainPanel.SetActive(true);
        lobbyPanel.SetActive(false);
        PhotonNetwork.LeaveLobby();
    }
}
