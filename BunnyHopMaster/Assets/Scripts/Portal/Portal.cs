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
    public GameObject otherPortal;

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
        if(playerCamera != null && portalCamera != null && otherPortal != null)
        {
            Transform Source = otherPortal.transform;
            Transform Destination = transform;
            // Rotate Source 180 degrees so PortalCamera is mirror image of MainCamera
            Matrix4x4 destinationFlipRotation =
                Matrix4x4.TRS(MathUtil.ZeroV3, Quaternion.AngleAxis(180.0f, Vector3.up), MathUtil.OneV3);
            Matrix4x4 sourceInvMat = destinationFlipRotation * Source.worldToLocalMatrix;

            // Calculate translation and rotation of MainCamera in Source space
            Vector3 cameraPositionInSourceSpace =
                MathUtil.ToV3(sourceInvMat * MathUtil.PosToV4(playerCamera.transform.position));
            Quaternion cameraRotationInSourceSpace =
                MathUtil.QuaternionFromMatrix(sourceInvMat) * playerCamera.transform.rotation;

            // Transform Portal Camera to World Space relative to Destination transform,
            // matching the Main Camera position/orientation
            portalCamera.transform.position = Destination.TransformPoint(cameraPositionInSourceSpace);
            portalCamera.transform.rotation = Destination.rotation * cameraRotationInSourceSpace;

            // Calculate clip plane for portal (for culling of objects in-between destination camera and portal)
            Vector4 clipPlaneWorldSpace =
                new Vector4(
                    Destination.forward.x,
                    Destination.forward.y,
                    Destination.forward.z,
                    Vector3.Dot(Destination.position, -Destination.forward));

            Vector4 clipPlaneCameraSpace =
                Matrix4x4.Transpose(Matrix4x4.Inverse(portalCamera.worldToCameraMatrix)) * clipPlaneWorldSpace;

            // Update projection based on new clip plane
            // Note: http://aras-p.info/texts/obliqueortho.html and http://www.terathon.com/lengyel/Lengyel-Oblique.pdf
            portalCamera.projectionMatrix = playerCamera.CalculateObliqueMatrix(clipPlaneCameraSpace);
            
            //Vector3 cameraPositionInSourceSpace = source.InverseTransformPoint(playerCamera.transform.position);
            //Quaternion cameraRotationInSourceSpace = Quaternion.Inverse(source.rotation) * playerCamera.transform.rotation;

            //portalCamera.transform.position = destination.transform.TransformPoint(cameraPositionInSourceSpace);
            //portalCamera.transform.rotation = destination.transform.rotation * cameraRotationInSourceSpace;



            //Vector3 playerOffsetFromPortal = transform.position - playerCamera.position;

            //Quaternion otherRotation = otherPortal.transform.rotation;
            //Vector3 transformedOffset = playerOffsetFromPortal;
            //Vector3 transformedOffsetX = Quaternion.AngleAxis(otherRotation.x, Vector3.right) * transformedOffset;
            //Vector3 transformedOffsetY = Quaternion.AngleAxis(otherRotation.y, Vector3.up) * transformedOffsetX;
            //Vector3 transformedOffsetZ = Quaternion.AngleAxis(otherRotation.z, Vector3.forward) * transformedOffsetY;

            //Vector3 newPosition = otherPortal.transform.position + transformedOffsetZ;
            //portalCamera.transform.position = newPosition;


            //float angularDifferenceBetweenPortalRotations = Quaternion.Angle(transform.rotation, otherPortal.transform.rotation);

            //Quaternion portalRotationalDifference = Quaternion.AngleAxis(angularDifferenceBetweenPortalRotations, Vector3.up);
            //Vector3 newCameraDirection = portalRotationalDifference * playerCamera.forward;
            //portalCamera.rotation = Quaternion.LookRotation(newCameraDirection, Vector3.up);

            //Debug.Log(gameObject.name
            //    + "\n" + transform.position
            //    + "\n" + playerCamera.transform.position
            //    + "\n" + playerOffsetFromPortal
            //    + "\n" + otherPortal.transform.position
            //    + "\n" + otherRotation
            //    + "\n" + transformedOffset
            //    + "\n" + transformedOffsetX
            //    + "\n" + transformedOffsetY
            //    + "\n" + transformedOffsetZ
            //    + "\n" + newPosition
            //    + "\n" + angularDifferenceBetweenPortalRotations
            //    + "\n" + portalRotationalDifference
            //    + "\n" + newCameraDirection
            //    );



            //Debug.Log("me: " + gameObject.name
            //    + " myPos: " + transform.position
            //    + " play: " + playerCamera.transform.position
            //    + " Off(me-play)" + playerOffsetFromPortal
            //    + " otherpos: " + otherPortal.transform.position
            //    + " transOff: " + transformedOffset
            //    + " new(oth+tran)" + newPosition
            //    + " angDiff " + angularDifferenceBetweenPortalRotations
            //    + " rotDiff " + portalRotationalDifference
            //    + " newDir " + newCameraDirection
            //    );
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
