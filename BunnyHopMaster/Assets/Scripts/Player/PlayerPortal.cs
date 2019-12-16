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
        if (Time.timeScale == 0)
        {
            return;
        }

        //Create Blue portal
        if (Input.GetMouseButtonDown(0))
        {
            CreatePortal(PortalType.blue);
        }
        //Create Red portal
        if (Input.GetMouseButtonDown(1))
        {
            CreatePortal(PortalType.red);
        }
    }

    void CreatePortal(PortalType portalType)
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position, transform.forward, out hit, 1000, layerMask))
        {
            CleanPortals(portalType);
            CreateNewPortal(portalType, hit);


            
        }
    }

    void CleanPortals(PortalType portalType)
    {
        if(portalType == PortalType.blue)
        {
            if(bluePortalInstance != null)
            {
                Destroy(bluePortalInstance.gameObject);
            }

            if (redPortalInstance != null)
            {
                redPortalInstance.destinationPortal = null;
            }
        }
        else
        {
            if (redPortalInstance != null)
            {
                Destroy(redPortalInstance.gameObject);
            }

            if (bluePortalInstance != null)
            {
                bluePortalInstance.destinationPortal = null;
            }
        }
    }
    
    void CreateNewPortal(PortalType portalType, RaycastHit hit)
    {
        if(portalType == PortalType.blue)
        {
            GameObject instance = Instantiate(bluePortalPrefab, hit.point, Quaternion.FromToRotation(Vector3.forward, hit.normal));
            Portal portal = instance.GetComponent<Portal>();
            portal.portalType = portalType;
            bluePortalInstance = portal;

            if (redPortalInstance != null)
            {
                portal.destinationPortal = redPortalInstance.transform;
                redPortalInstance.destinationPortal = portal.transform;
            }
        }
        else
        {
            GameObject instance = Instantiate(redPortalPrefab, hit.point, Quaternion.FromToRotation(Vector3.forward, hit.normal));
            Portal portal = instance.GetComponent<Portal>();
            portal.portalType = portalType;
            redPortalInstance = portal;

            if (bluePortalInstance != null)
            {
                portal.destinationPortal = bluePortalInstance.transform;
                bluePortalInstance.destinationPortal = portal.transform;
            }
        }
    }
}
