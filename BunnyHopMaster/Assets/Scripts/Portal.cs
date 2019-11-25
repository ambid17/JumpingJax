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
