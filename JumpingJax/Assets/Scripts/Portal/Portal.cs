using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class Portal : MonoBehaviour
{
    private const string portalName = "Portal1";

    [SerializeField]
    private Portal otherPortal = null;

    [SerializeField]
    private Renderer outlineRenderer = null;

    [SerializeField]
    private Color portalColour;

    [SerializeField]
    private LayerMask placementMask;

    [SerializeField]
    private LayerMask overhangMask;

    private bool isPlaced = false;

    [SerializeField]
    private List<Collider> wallColliders;

    public bool isDebug = true;

    private List<PortalableObject> portalObjects = new List<PortalableObject>();

    private Material meshMaterialBlue;
    private Material meshMaterialOrange;

    private string pathToPortalMaterials = "Materials/portal/";

    private Material meshMaterialMain;
    private new Renderer renderer;
    private BoxCollider boxCollider;

    private float sphereCastSize = 0.02f;
    private float bigSphereCastSize = 0.04f;

    private List<Vector3> overHangTestPoints = new List<Vector3>
        {
            new Vector3(-1.51f,  0, 0),
            new Vector3( 1.51f,  0, 0),
            new Vector3( 0, -1.51f, 0),
            new Vector3( 0,  1.51f, 0)
        };

    private float minCutOff = 0.01f;
    private float maxCutOff = 0.8f;
    private float cutOffValue = 0.01f;
    private float cutOffInterval = 0.002f;
    private bool isIncrementing = true;


    private void Awake()
    {
        boxCollider = GetComponent<BoxCollider>();
        renderer = GetComponent<Renderer>();
        meshMaterialMain = renderer.material;
        meshMaterialBlue = Resources.Load<Material>(pathToPortalMaterials + "PortalOutline");
        meshMaterialOrange = Resources.Load<Material>(pathToPortalMaterials + "PortalOutline 1");

        ResetPortal();
    }

    private void Start()
    {
        SetColour(portalColour);
    }

    private void Update()
    {
        if(cutOffValue >= maxCutOff)
        {
            isIncrementing = false;
        }

        if(cutOffValue <= minCutOff)
        {
            isIncrementing = true;
        }


        cutOffValue += isIncrementing ? cutOffInterval : -cutOffInterval;
        outlineRenderer.material.SetFloat("_Cutoff", cutOffValue);
        outlineRenderer.gameObject.transform.Rotate(Vector3.forward, 0.1f); 

        for (int i = 0; i < portalObjects.Count; ++i)
        {
            Vector3 objPos = transform.InverseTransformPoint(portalObjects[i].transform.position);

            if (objPos.z > 0.0f)
            {
                portalObjects[i].Warp();
            }
        }
    }

    public Portal GetOtherPortal()
    {
        return otherPortal;
    }

    public Color GetColour()
    {
        return portalColour;
    }

    public void SetColour(Color colour)
    {
        meshMaterialMain.SetColor("_Colour", colour);
    }

    public void SetMaskID(int id)
    {
        meshMaterialMain.SetInt("_MaskID", id);
    }

    public void SetTexture(RenderTexture tex)
    {
        meshMaterialMain.mainTexture = tex;
    }

    public bool IsRendererVisible()
    {
        return renderer.isVisible;
    }

    private void OnTriggerEnter(Collider other)
    {
        var obj = other.GetComponent<PortalableObject>();
        if (obj != null && otherPortal.IsPlaced())
        {
            portalObjects.Add(obj);
            obj.SetIsInPortal(this, otherPortal, wallColliders);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        var portalable = other.GetComponent<PortalableObject>();

        ResetObjectInPortal(portalable);
    }

    public void ResetObjectInPortal(PortalableObject portalable)
    {
        if (portalObjects.Contains(portalable))
        {
            portalObjects.Remove(portalable);
            portalable.ExitPortal(wallColliders);
        }
    }

    public void PlacePortal(Vector3 pos, Quaternion rot)
    {
        isPlaced = true;
        if (isPlaced && !otherPortal.isPlaced)
        {
            boxCollider.enabled = false;
            SetPortalRendererMaterial();
        }
        else
        {
            renderer.material = meshMaterialMain;
            otherPortal.renderer.material = otherPortal.meshMaterialMain;
            if (!boxCollider.enabled || !otherPortal.boxCollider.enabled)
            {
                boxCollider.enabled = true;
                otherPortal.boxCollider.enabled = true;
            }
        }

        transform.position = pos;
        transform.rotation = rot;
        transform.position -= transform.forward * 0.001f;
        gameObject.SetActive(true);

        FixOverhangs();
        FixPortalOverlaps();
        GetWallColliders();
    }

    // Ensure the portal cannot extend past the edge of a surface, or intersect a corner
    private void FixOverhangs()
    {
        for (int i = 0; i < 4; ++i)
        {
            Vector3 overhangTestPosition = transform.TransformPoint(overHangTestPoints[i]);

            // If the point isn't touching anything, it overhangs
            if (!Physics.CheckSphere(overhangTestPosition, sphereCastSize, overhangMask))
            {
                Vector3 portalOverhangOffset = FindOverhangOffset(overHangTestPoints[i]);
                transform.Translate(portalOverhangOffset, Space.Self);
            }
        }
    }

    // This method finds the closest point where the object is no longer overhanging
    private Vector3 FindOverhangOffset(Vector3 testPoint)
    {
        Vector3 overhangOffset = -testPoint;

        int steps = Mathf.FloorToInt(testPoint.magnitude / sphereCastSize);

        for (int i = 0; i < steps; i++)
        {
            float interpolationFactor = (float) i / (float) steps;
            Vector3 stepPosition = Vector3.Lerp(testPoint, Vector3.zero, interpolationFactor);
            Vector3 worldSpaceStepPosition = transform.TransformPoint(stepPosition);

            if (Physics.CheckSphere(worldSpaceStepPosition, bigSphereCastSize, overhangMask))
            {
                return stepPosition - testPoint;
            }
        }

        return overhangOffset;
    }

    private bool GetIsOverhanging()
    {
        for (int i = 0; i < 4; ++i)
        {
            Vector3 overhangTestPosition = transform.TransformPoint(overHangTestPoints[i]);

            // If the point isn't touching anything, it overhangs
            if (!Physics.CheckSphere(overhangTestPosition, bigSphereCastSize, overhangMask))
            {
                return true;
            }
        }

        return false;
    }

    // If the new portal overlaps an already placed one
    // try to fix the overlap, or delete it
    private void FixPortalOverlaps()
    {
        Vector3 worldSpaceCenter = transform.TransformPoint(boxCollider.center);
        Collider[] overlappingBoxes = Physics.OverlapBox(worldSpaceCenter, boxCollider.bounds.extents);

        foreach(Collider otherCollider in overlappingBoxes)
        {
            if (otherCollider.gameObject == otherPortal.gameObject)
            {
                GetClosestOverlapFix();
            }
        }

        if (GetIsOverhanging())
        {
            ResetPortal();
            ResetOtherPortal();
        }
    }

    private void GetClosestOverlapFix()
    {
        Vector3 overlapFix = Vector3.zero;

        Vector3 localOffset = transform.InverseTransformPoint(otherPortal.transform.position);

        bool isInsideX = localOffset.x < PlayerConstants.portalWidth;
        bool isInsideY = localOffset.y < PlayerConstants.portalHeight;


        float offsetX = PlayerConstants.portalWidth - Mathf.Abs(localOffset.x);
        float offsetY = PlayerConstants.portalHeight - Mathf.Abs(localOffset.y);

        Vector3 offsetFixX = Vector3.zero;
        Vector3 offsetFixY = Vector3.zero;

        if (localOffset.x < 0)
        {
            offsetFixX = new Vector3(offsetX, 0, 0);
        }
        else {
            offsetFixX = new Vector3(-offsetX, 0, 0);
        }

        if (localOffset.y < 0)
        {
            offsetFixY = new Vector3(0, offsetY, 0);
        }
        else
        {
            offsetFixY = new Vector3(0, -offsetY, 0);
        }

        if (isInsideX && isInsideY)
        {
            if(offsetX < offsetY)
            {
                overlapFix = offsetFixX;
            }
            else
            {
                overlapFix = offsetFixY;

            }
        }
        else if (isInsideX)
        {
            overlapFix = offsetFixX;
        }
        else if (isInsideY)
        {
            overlapFix = offsetFixY;
        }

        transform.Translate(overlapFix, Space.Self);
    }

    private void GetWallColliders()
    {
        Vector3 worldSpaceCenter = transform.TransformPoint(boxCollider.center);
        Collider[] overlappingBoxes = Physics.OverlapBox(worldSpaceCenter, PlayerConstants.PortalColliderExtents, transform.rotation, overhangMask);

        wallColliders = new List<Collider>(overlappingBoxes);
    }

    public void ResetPortal()
    {
        gameObject.SetActive(false);
        isPlaced = false;
        boxCollider.enabled = false;
        transform.position = new Vector3(100, 100, 100);
        SetPortalRendererMaterial();

        for(int i = 0; i < portalObjects.Count; i++)
        {
            ResetObjectInPortal(portalObjects[i]);
        }
    }

    public void ResetOtherPortal()
    {
        otherPortal.SetPortalRendererMaterial();
    }

    public bool IsPlaced()
    {
        return isPlaced;
    }

    public void SetPortalRendererMaterial()
    {
        if (name == portalName)
        {
            renderer.material = meshMaterialBlue;
        }
        else
        {
            renderer.material = meshMaterialOrange;
        }
    }
}