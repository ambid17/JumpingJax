using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalTextureSetup : MonoBehaviour
{
    public Camera camera;

    public Material cameraMaterial;
    Resolution resolution;

    void Start()
    {
        resolution = Screen.currentResolution;
        UpdateRenderTexture();
    }

    void Update()
    {
        if(resolution.width != Screen.currentResolution.width || resolution.height != Screen.currentResolution.height)
        {
            resolution = Screen.currentResolution;
            UpdateRenderTexture();
        }
    }

    void UpdateRenderTexture()
    {
        if (camera.targetTexture != null)
        {
            camera.targetTexture.Release();
        }

        camera.targetTexture = new RenderTexture(Screen.width, Screen.height, 24);
        cameraMaterial.mainTexture = camera.targetTexture;
    }
}
