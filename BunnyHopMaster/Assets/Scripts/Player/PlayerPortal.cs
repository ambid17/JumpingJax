using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPortal : MonoBehaviour
{
    public const int portalMaterialLayer = 10;
    public const int portalLayer = 11;
    int layerMask;

    public GameObject bluePortalPrefab;
    public GameObject redPortalPrefab;

    public Portal bluePortalInstance;
    public Portal redPortalInstance;

   

    void Start()
    {
        layerMask = 1 << portalMaterialLayer;
    }

    void Update()
    {
        //Create Blue portal
        if (Input.GetMouseButtonDown(0))
        {
            CreateBluePortal();
        }
        //Create Red portal
        if (Input.GetMouseButtonDown(1))
        {
            CreateRedPortal();
        }
    }

    void CreateBluePortal()
    {
        RaycastHit hit;

        if(Physics.Raycast(transform.position, transform.forward, out hit, 1000, layerMask))
        {
            if(bluePortalInstance != null)
            {
                if(redPortalInstance.otherPortal != null)
                {
                    redPortalInstance.otherPortal = null;
                }
                Destroy(bluePortalInstance.gameObject);
            }
            bluePortalInstance = Instantiate(bluePortalPrefab, hit.point, Quaternion.FromToRotation(Vector3.forward, hit.normal)).GetComponent<Portal>();
            Vector3 crossX = Vector3.Cross(hit.normal, Vector3.forward);
            bluePortalInstance.transform.Rotate(-crossX, Time.deltaTime * 10f);

            Portal portal = bluePortalInstance.GetComponent<Portal>();
            portal.portalType = PortalType.blue;

            if(redPortalInstance != null)
            {
                portal.otherPortal = redPortalInstance.gameObject;
                redPortalInstance.otherPortal = portal.gameObject;
            }
        }
    }

    void CreateRedPortal()
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position, transform.forward, out hit, 1000, layerMask))
        {
            if (redPortalInstance != null)
            {
                if (bluePortalInstance.otherPortal != null)
                {
                    bluePortalInstance.otherPortal = null;
                }
                Destroy(redPortalInstance.gameObject);
            }
            redPortalInstance = Instantiate(redPortalPrefab, hit.point, Quaternion.FromToRotation(Vector3.forward, hit.normal)).GetComponent<Portal>();
            Vector3 crossX = Vector3.Cross(hit.normal, Vector3.forward);
            redPortalInstance.transform.Rotate(-crossX, Time.deltaTime * 10f);

            Portal portal = redPortalInstance.GetComponent<Portal>();
            portal.portalType = PortalType.red;

            if (bluePortalInstance != null)
            {
                portal.otherPortal = bluePortalInstance.gameObject;
                bluePortalInstance.otherPortal = portal.gameObject;
            }
        }
    }
}
