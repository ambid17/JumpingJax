﻿using System.Collections;
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
            Vector3 playerOffsetFromRedPortal = playerCamera.position - otherPortal.transform.position;
            portalCamera.position = transform.position + playerOffsetFromRedPortal;

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
            if (portalType == PortalType.red)
            {
                other.transform.parent.position = otherPortal.transform.position;
            }
            else
            {
                other.transform.parent.position = otherPortal.transform.position;
            }
        }

        Destroy(otherPortal);
        Destroy(this.gameObject);
    }
}