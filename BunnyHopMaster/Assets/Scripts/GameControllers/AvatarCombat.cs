using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AvatarCombat : MonoBehaviour
{
    private PhotonView PV;
    private AvatarSetup avatarSetup;
    public Transform rayOrigin;
    public float shotRange;

    public Text healthDisplay;

    void Start()
    {
        PV = GetComponent<PhotonView>();
        avatarSetup = GetComponent<AvatarSetup>();
        healthDisplay = GameSetup.GS.healthText;
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

        healthDisplay.text = avatarSetup.playerHealth.ToString();
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
                hit.transform.gameObject.GetComponent<AvatarSetup>().playerHealth -= avatarSetup.playerDamage;
            }
        }
        else
        {
            Debug.DrawRay(rayOrigin.position, rayOrigin.TransformDirection(Vector3.forward) * shotRange, Color.yellow);
            Debug.Log("did not hit: ");
        }
    }
}
