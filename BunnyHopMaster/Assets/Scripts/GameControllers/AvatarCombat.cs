using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AvatarCombat : MonoBehaviour
{
    private PhotonView PV;

    public int currentDamage;
    public Transform rayOrigin;
    public float shotRange;

    public Text healthDisplay;
    public int playerHealth;

    public Text teamDisplay;
    public int team;

    public int humanCharacterValue;
    public int infectedCharacterValue;
    public GameObject myCharacter;

    public Camera myCamera;
    public AudioListener myAudio;


    void Start()
    {
        PV = GetComponent<PhotonView>();
        healthDisplay = GameSetup.GS.healthText;
        teamDisplay = GameSetup.GS.teamText;
        myCamera = GetComponentInChildren<Camera>();
        myAudio = GetComponentInChildren<AudioListener>();
        playerHealth = 100;

        if (PV.IsMine)
        {
            if(team == 1)
            {
                PV.RPC("RPC_AddCharacter", RpcTarget.AllBuffered, PlayerInfo.PI.selectedInfectedCharacter);
            }else if (team == 2)
            {
                PV.RPC("RPC_AddCharacter", RpcTarget.AllBuffered, PlayerInfo.PI.selectedHumanCharacter);
            }

        }
        else
        {
            Destroy(myCamera);
            Destroy(myAudio);
        }
    }

    void Update()
    {
        if (!PV.IsMine)
        {
            return;
        }

        if (Input.GetMouseButton(0))
        {
            PV.RPC("RPC_Shoot", RpcTarget.All);
        }

        healthDisplay.text = playerHealth.ToString();
        if(team == 1)
        {
            teamDisplay.text = "infected";
        }else if (team == 2)
        {
            teamDisplay.text = "human";
        }
        else
        {
            Debug.Log("NO TEAM");
        }

        if(playerHealth <= 0)
        {
            Debug.Log(gameObject.name + " has died");
            int spawnPicker = Random.Range(0, GameSetup.GS.spawnPointsInfected.Length);
            transform.position = GameSetup.GS.spawnPointsInfected[spawnPicker].position;
            transform.rotation = GameSetup.GS.spawnPointsInfected[spawnPicker].rotation;
            playerHealth = 100;
        }
    }

    [PunRPC]
    void RPC_Shoot()
    {
        RaycastHit hit;
        if (Physics.Raycast(rayOrigin.position, rayOrigin.TransformDirection(Vector3.forward), out hit, shotRange))
        {
            Debug.DrawRay(rayOrigin.position, rayOrigin.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);
            Debug.Log("hit: " + hit.collider.gameObject.name);

            if (hit.transform.tag == "Avatar")
            {
                AvatarCombat avatarCombat = hit.transform.gameObject.GetComponent<AvatarCombat>();
                if(avatarCombat.team != team)
                {
                    Debug.Log("hit avatar: their team: " + avatarCombat.team + " our team: " + team);
                    if(team == 1)
                    {
                        //if an infected is attacking a human
                        avatarCombat.playerHealth = 0;
                        avatarCombat.team = 1;
                    }else if (team == 1)
                    {
                        //if a human is attacking an infected
                        avatarCombat.playerHealth -= currentDamage;
                    }
                }
                
            }
        }
        else
        {
            Debug.DrawRay(rayOrigin.position, rayOrigin.TransformDirection(Vector3.forward) * shotRange, Color.white);
        }
    }

    [PunRPC]
    void RPC_AddCharacter(int character)
    {
        if (team == 1)
        {
            infectedCharacterValue = character;
            myCharacter = Instantiate(PlayerInfo.PI.infectedCharacters[character], transform.position, transform.rotation, transform);
        }
        else if (team == 2)
        {
            humanCharacterValue = character;
            myCharacter = Instantiate(PlayerInfo.PI.humanCharacters[character], transform.position, transform.rotation, transform);
        }
    }
}
