using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PortalType
{
    red, blue
}

public static class MathUtil
{
    public static Quaternion QuaternionFromMatrix(Matrix4x4 m)
    {
        return Quaternion.LookRotation(m.GetColumn(2), m.GetColumn(1));
    }

    public static Vector4 PosToV4(Vector3 v) { return new Vector4(v.x, v.y, v.z, 1.0f); }
    public static Vector3 ToV3(Vector4 v) { return new Vector3(v.x, v.y, v.z); }

    public static Vector3 ZeroV3 = new Vector3(0.0f, 0.0f, 0.0f);
    public static Vector3 OneV3 = new Vector3(1.0f, 1.0f, 1.0f);
}

public class Portal : MonoBehaviour
{
    public PortalType portalType;
    public Transform source;

    public Camera playerCamera;
    public Camera portalCamera;

    private void Start()
    {
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
        if(playerCamera != null && portalCamera != null && source != null)
        {
            // Rotate Source 180 degrees so PortalCamera is mirror image of MainCamera
            Matrix4x4 destinationFlipRotation =
                Matrix4x4.TRS(MathUtil.ZeroV3, Quaternion.AngleAxis(180.0f, Vector3.up), MathUtil.OneV3);
            Matrix4x4 sourceInvMat = destinationFlipRotation * source.worldToLocalMatrix;

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
        Debug.Log("triggerEnter: " + other.gameObject.name);
        //Check that the trigger is the playerCollider, not the water
        if (source != null && other.GetComponent<BoxCollider>())
        {
            TeleportPlayer(other.gameObject);
        }

        Destroy(source);
        Destroy(this.gameObject);
    }

    private void TeleportPlayer(GameObject playerObject)
    {
        Vector3 portalToPlayer = playerObject.transform.position - transform.position;
        float dotProduct = Vector3.Dot(transform.up, portalToPlayer);

        //Player has moved across portal, need to teleport them
        if(dotProduct < 0f)
        {
            float rotationDiff = Quaternion.Angle(transform.rotation, source.transform.rotation);
            rotationDiff += 180;
            playerObject.transform.Rotate(Vector3.up, rotationDiff);

            Vector3 positionOffset = Quaternion.Euler(0f, rotationDiff, 0f) * portalToPlayer;
            playerObject.transform.position = source.transform.position + positionOffset;
        }

        playerObject.transform.parent.position = source.transform.position;
    }
}
