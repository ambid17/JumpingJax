using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class RoomButton : MonoBehaviour {

    [SerializeField]
    private Text nameText; //display for room name
    [SerializeField]
    private Text sizeText; //display for room size

    private string roomName; //string for saving room name
    private int roomSize; //int for saving room size
    private int playerCount;

    public void JoinRoomOnClick() //paired the button that is the room listing. joins the player a room by its name
    {
        PhotonNetwork.JoinRoom(roomName);
    }

    public void SetRoom(string nameInput, int sizeInput, int countInput) //public function called in CMM lobby contoller for each new room listing created
    {
        roomName = nameInput;
        roomSize = sizeInput;
        playerCount = countInput;
        nameText.text = nameInput;
        sizeText.text = countInput + "/" + sizeInput;
    }
}
