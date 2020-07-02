using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Crosshair : MonoBehaviour
{
    [SerializeField]
    private PortalPair portalPair = null;

    [SerializeField]
    private Image inPortalImg = null;

    [SerializeField]
    private Image outPortalImg = null;

    private void Start()
    {
        var portals = portalPair.Portals;

        inPortalImg.color = portals[0].GetColour();
        outPortalImg.color = portals[1].GetColour();

        inPortalImg.gameObject.SetActive(false);
        outPortalImg.gameObject.SetActive(false);
    }

    public void SetPortalPlaced(int portalID, bool isPlaced)
    {
        if(portalID == 0)
        {
            inPortalImg.gameObject.SetActive(isPlaced);
        }
        else
        {
            outPortalImg.gameObject.SetActive(isPlaced);
        }
    }
}
