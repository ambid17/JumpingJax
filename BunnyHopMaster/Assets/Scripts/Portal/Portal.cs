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
    public Transform destinationPortal;

    public Camera playerCamera;
    public Camera portalCamera;

    bool canTeleport;
    float teleportTimer;
    float timeToTeleport;

    private void Start()
    {
        canTeleport = true;
        teleportTimer = 0f;
        timeToTeleport = 1f;

        playerCamera = Camera.main;

        if (playerCamera == null)
        {
            Debug.Log("Could not find player camera");
        }

        portalCamera = GetComponentInChildren<Camera>();

        if(portalCamera == null)
        {
            Debug.Log("Could not find child with camera");
        }
    }

    private void Update()
    {
        CheckCanTeleport();
        TransformPortalCamera();
    }
    
    private void CheckCanTeleport()
    {
        if(destinationPortal == null)
        {
            canTeleport = false;
            return;
        }


        if (!canTeleport)
        {
            teleportTimer += Time.deltaTime;
        }

        if(teleportTimer >= timeToTeleport)
        {
            canTeleport = true;
        }
    }

    private void TransformPortalCamera()
    {
        if (playerCamera != null && portalCamera != null && destinationPortal != null)
        {
            // Rotate Source 180 degrees so PortalCamera is mirror image of MainCamera
            Matrix4x4 destinationFlipRotation =
                Matrix4x4.TRS(MathUtil.ZeroV3, Quaternion.AngleAxis(180.0f, Vector3.up), MathUtil.OneV3);
            Matrix4x4 sourceInvMat = destinationFlipRotation * destinationPortal.worldToLocalMatrix;

            // Calculate translation and rotation of MainCamera in Source space
            Vector3 cameraPositionInSourceSpace =
                MathUtil.ToV3(sourceInvMat * MathUtil.PosToV4(playerCamera.transform.position));
            Quaternion cameraRotationInSourceSpace =
                MathUtil.QuaternionFromMatrix(sourceInvMat) * playerCamera.transform.rotation;

            // Transform Portal Camera to World Space relative to Destination transform,
            // matching the Main Camera position/orientation
            portalCamera.transform.position = transform.TransformPoint(cameraPositionInSourceSpace);
            portalCamera.transform.rotation = transform.rotation * cameraRotationInSourceSpace;

            // Calculate clip plane for portal (for culling of objects in-between destination camera and portal)
            Vector4 clipPlaneWorldSpace =
                new Vector4(
                    transform.forward.x,
                    transform.forward.y,
                    transform.forward.z,
                    Vector3.Dot(transform.position, -transform.forward));

            Vector4 clipPlaneCameraSpace =
                Matrix4x4.Transpose(Matrix4x4.Inverse(portalCamera.worldToCameraMatrix)) * clipPlaneWorldSpace;

            // Update projection based on new clip plane
            // Note: http://aras-p.info/texts/obliqueortho.html and http://www.terathon.com/lengyel/Lengyel-Oblique.pdf
            portalCamera.projectionMatrix = playerCamera.CalculateObliqueMatrix(clipPlaneCameraSpace);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (canTeleport)
        {
            Debug.Log("triggerEnter: " + other.gameObject.name);
            if (other.GetComponentInParent<PlayerProgress>() && canTeleport)
            {
                //Keep the player from teleporting for 1 second
                DidTeleport();
                TeleportPlayer(other.transform);
            }
        }
    }

    private void TeleportPlayer(Transform playerObject)
    {
        //The box collider is a child of the player, thus we need to move the parent
        playerObject.parent.position = destinationPortal.transform.position;
    }

    public void DidTeleport()
    {
        canTeleport = false;
        teleportTimer = 0;

        Portal otherPortal = destinationPortal.GetComponent<Portal>();
        if (otherPortal != null){
            otherPortal.canTeleport = false;
            otherPortal.teleportTimer = 0;
        }
        else
        {
            Debug.Log("Could not find destination portal");
        }
    }
}
