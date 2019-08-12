using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvatarSetup : MonoBehaviour
{
    private PhotonView PV;
    public int characterValue;
    public GameObject myCharacter;

    public int playerHealth;
    public int playerDamage;

    public Camera myCamera;
    public AudioListener myAudio;

    void Start()
    {
        PV = GetComponent<PhotonView>();
        if (PV.IsMine)
        {
            PV.RPC("RPC_AddCharacter", RpcTarget.AllBuffered, PlayerInfo.PI.mySelectedCharacter);
        }
        else
        {
            Destroy(myCamera);
            Destroy(myAudio);
        }
    }

    [PunRPC]
    void RPC_AddCharacter(int character)
    {
        characterValue = character;
        myCharacter = Instantiate(PlayerInfo.PI.allCharacters[character], transform.position, transform.rotation, transform);
    }
}
