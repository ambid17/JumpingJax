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

    private Quaternion flippedYRotation = Quaternion.Euler(0.0f, 180.0f, 0.0f);
    private const float portalRaycastDistance = 250;
    private void Awake()
    {
        cameraMove = GetComponent<CameraMove>();
    }

    private void Update()
    {
        if (Time.timeScale == 0)
        {
            return;
        }

        if (Input.GetButtonDown("Fire1"))
        {
            FirePortal(0, transform.position, transform.forward, portalRaycastDistance);
        }
        else if (Input.GetButtonDown("Fire2"))
        {
            FirePortal(1, transform.position, transform.forward, portalRaycastDistance);
        }
    }

    private void FirePortal(int portalID, Vector3 pos, Vector3 dir, float distance)
    {
        RaycastHit hit;
        Physics.Raycast(pos, dir, out hit, distance, layerMask);

        if(hit.collider != null)
        {
            // If we hit a portal, spawn a portal through this portal
            if (hit.collider.tag == "Portal")
            {
                var inPortal = hit.collider.GetComponent<Portal>();

                if(inPortal == null)
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

            var cameraRotation = cameraMove.TargetRotation;

            var portalRight = cameraRotation * Vector3.right;
            
            if(Mathf.Abs(portalRight.x) >= Mathf.Abs(portalRight.z))
            {
                portalRight = Vector3.right * ((portalRight.x >= 0) ? 1 : -1);
            }
            else
            {
                portalRight = Vector3.forward * ((portalRight.z >= 0) ? 1 : -1);
            }

            var portalForward = -hit.normal;
            var portalUp = -Vector3.Cross(portalRight, portalForward);

            var portalRotation = Quaternion.LookRotation(portalForward, portalUp);
            
            portals.Portals[portalID].PlacePortal(hit.collider, hit.point, portalRotation);

            // leaving this in until i figure out how i want to handle the crosshair
            //crosshair.SetPortalPlaced(portalID, true);
        }
    }
}
