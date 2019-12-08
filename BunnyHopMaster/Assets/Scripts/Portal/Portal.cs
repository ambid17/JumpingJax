using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PortalType
{
    red, blue
}

public class Portal : MonoBehaviour
{
    public PortalType portalType;
    public GameObject otherPortal;

    public Transform playerCamera;
    public Transform portalCamera;

    private void Start()
    {
        playerCamera = Camera.main.transform;

        if (playerCamera == null)
        {
            Debug.Log("Could not find player camera");
        }

        portalCamera = GetComponentInChildren<Camera>().transform;

        if(portalCamera == null)
        {
            Debug.Log("Could not find child with camera");
        }
    }

    private void Update()
    {
        if(playerCamera != null && portalCamera != null && otherPortal != null)
        {
            Vector3 playerOffsetFromPortal = playerCamera.position - otherPortal.transform.position;
            Vector3 newPos = transform.position - playerOffsetFromPortal;
            portalCamera.transform.position = newPos;

            Debug.Log("portal: " + gameObject.name
                + " offset: " + playerOffsetFromPortal
                + " mypos: " + transform.position
                + " otherpos: " + otherPortal.transform.position
                + " player: " + playerCamera.transform.position
                + " newPos: " + newPos);

            float angularDifferenceBetweenPortalRotations = Quaternion.Angle(transform.rotation, otherPortal.transform.rotation);

            Quaternion portalRotationalDifference = Quaternion.AngleAxis(angularDifferenceBetweenPortalRotations, Vector3.up);
            Vector3 newCameraDirection = portalRotationalDifference * playerCamera.forward;
            portalCamera.rotation = Quaternion.LookRotation(newCameraDirection, Vector3.up);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("triggerEnter: " + other.gameObject.name);
        //Check that the trigger is the playerCollider, not the water
        if (otherPortal != null && other.GetComponent<BoxCollider>())
        {
            TeleportPlayer(other.gameObject);
        }

        Destroy(otherPortal);
        Destroy(this.gameObject);
    }

    private void TeleportPlayer(GameObject playerObject)
    {
        Vector3 portalToPlayer = playerObject.transform.position - transform.position;
        float dotProduct = Vector3.Dot(transform.up, portalToPlayer);

        //Player has moved across portal, need to teleport them
        if(dotProduct < 0f)
        {
            float rotationDiff = Quaternion.Angle(transform.rotation, otherPortal.transform.rotation);
            rotationDiff += 180;
            playerObject.transform.Rotate(Vector3.up, rotationDiff);

            Vector3 positionOffset = Quaternion.Euler(0f, rotationDiff, 0f) * portalToPlayer;
            playerObject.transform.position = otherPortal.transform.position + positionOffset;
        }

        playerObject.transform.parent.position = otherPortal.transform.position;
    }
}
