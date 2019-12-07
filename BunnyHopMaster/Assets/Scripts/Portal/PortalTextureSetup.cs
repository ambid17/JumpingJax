using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalTextureSetup : MonoBehaviour
{
    public Camera portalCamera;

    public Material cameraMaterial;
    public Shader cutoutShader;
    Resolution resolution;

    void Start()
    {
        portalCamera = GetComponent<Camera>();
        cutoutShader = Shader.Find("Unlit/ScreenCutoutShader");
        cameraMaterial = new Material(cutoutShader);
        resolution = Screen.currentResolution;
        UpdateRenderTexture();
        UpdatePortalMaterial();
    }

    void Update()
    {
        if(resolution.width != Screen.currentResolution.width || resolution.height != Screen.currentResolution.height)
        {
            resolution = Screen.currentResolution;
            UpdateRenderTexture();
        }
    }

    void UpdatePortalMaterial()
    {
        MeshRenderer renderer = GetComponentInParent<MeshRenderer>();
        renderer.sharedMaterial = cameraMaterial;
    }

    void UpdateRenderTexture()
    {
        if (portalCamera.targetTexture != null)
        {
            portalCamera.targetTexture.Release();
        }

        portalCamera.targetTexture = new RenderTexture(Screen.width, Screen.height, 24);
        cameraMaterial.mainTexture = portalCamera.targetTexture;
    }
}
