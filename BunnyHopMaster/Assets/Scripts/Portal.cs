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
        CharacterController characterController = other.GetComponent<CharacterController>();
        if (characterController != null && otherPortal != null)
        {
            characterController.enabled = false;
            if (portalType == PortalType.red)
            {
                other.transform.position = otherPortal.transform.position;
            }
            else
            {
                other.transform.position = otherPortal.transform.position;
            }
            characterController.enabled = true;
        }

        Destroy(otherPortal);
        Destroy(this.gameObject);
    }
}
