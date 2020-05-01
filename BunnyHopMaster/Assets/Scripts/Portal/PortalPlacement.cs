using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CameraMove))]
public class PortalPlacement : MonoBehaviour
{
    [SerializeField]
    private PortalPair portals;

    [SerializeField]
    private LayerMask layerMask;

    // Leaving this in until crosshair is added
    //[SerializeField]
    //private Crosshair crosshair;

    private CameraMove cameraMove;
    private PlayerPortalableController playerPortalable;

    private Quaternion flippedYRotation = Quaternion.Euler(0.0f, 180.0f, 0.0f);
    private const float portalRaycastDistance = 250;
    private const string portalTag = "Portal";
    private bool isPortalLevel = false;


    private void Awake()
    {
        cameraMove = GetComponent<CameraMove>();
        playerPortalable = GetComponent<PlayerPortalableController>();
    }

    private void Update()
    {
        isPortalLevel = GameManager.GetCurrentLevel().isPortalLevel;
        if (Time.timeScale == 0 || !isPortalLevel || playerPortalable.IsInPortal())
        {
            return;
        }

        if (InputManager.GetKeyDown(PlayerConstants.Portal1))
        {
            FirePortal(0, cameraMove.playerCamera.transform.position, cameraMove.playerCamera.transform.forward, portalRaycastDistance);
        }
        else if (InputManager.GetKeyDown(PlayerConstants.Portal2))
        {
            FirePortal(1, cameraMove.playerCamera.transform.position, cameraMove.playerCamera.transform.forward, portalRaycastDistance);
        }
    }

    private void FirePortal(int portalID, Vector3 pos, Vector3 dir, float distance)
    {
        RaycastHit hit;
        Physics.Raycast(pos, dir, out hit, distance, layerMask, QueryTriggerInteraction.Collide);

        if (hit.collider != null)
        {
            // If we hit a portal, spawn a portal through this portal
            if (hit.collider.gameObject.layer == PlayerConstants.PortalLayer)
            {
                var inPortal = hit.collider.GetComponent<Portal>();

                if (inPortal == null || !inPortal.IsPlaced())
                {
                    return;
                }

                var outPortal = inPortal.GetOtherPortal();

                // Update position of raycast origin with small offset.
                Vector3 relativePos = inPortal.transform.InverseTransformPoint(hit.point + dir);
                relativePos = flippedYRotation * relativePos;
                pos = outPortal.transform.TransformPoint(relativePos);

                // Update direction of raycast.
                Vector3 relativeDir = inPortal.transform.InverseTransformDirection(dir);
                relativeDir = flippedYRotation * relativeDir;
                dir = outPortal.transform.TransformDirection(relativeDir);

                distance -= Vector3.Distance(pos, hit.point);

                FirePortal(portalID, pos, dir, distance);

                return;
            }
            else if (hit.collider.gameObject.layer == PlayerConstants.PortalMaterialLayer)
            {
                var cameraRotation = cameraMove.TargetRotation;
                var portalRight = cameraRotation * Vector3.right;

                if(Mathf.Abs(portalRight.x) >= 0)
                {
                    portalRight = (portalRight.x >= 0) ? Vector3.right : -Vector3.right;
                }
                else
                {
                    portalRight = (portalRight.z >= 0) ? Vector3.forward : -Vector3.forward;
                }

                var portalForward = -hit.normal;

                var portalUp = -Vector3.Cross(portalRight, portalForward);

                if (portalForward.x != 0 || portalForward.z != 0)
                {
                    portalUp = Vector3.up;
                }


                var portalRotation = Quaternion.LookRotation(portalForward, portalUp);
                portals.Portals[portalID].PlacePortal(hit.point, portalRotation);
            }
        }
    }
}