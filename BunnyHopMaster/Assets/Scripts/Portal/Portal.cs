using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class Portal : MonoBehaviour
{
    [SerializeField]
    private Portal otherPortal;

    [SerializeField]
    private Renderer outlineRenderer;

    [SerializeField]
    private Color portalColour;

    [SerializeField]
    private LayerMask placementMask;

    [SerializeField]
    private LayerMask overhangMask;

    private bool isPlaced = false;

    [SerializeField]
    private Collider wallCollider;

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
        outlineRenderer.material.SetColor("_OutlineColour", colour);
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
        Debug.Log("triggerEntered: " + other.gameObject.name);
        var obj = other.GetComponent<PortalableObject>();
        if (obj != null)
        {
            portalObjects.Add(obj);
            obj.SetIsInPortal(this, otherPortal, wallCollider);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        var obj = other.GetComponent<PortalableObject>();

        if (portalObjects.Contains(obj))
        {
            portalObjects.Remove(obj);
            obj.ExitPortal(wallCollider);
        }
    }

    public void PlacePortal(Collider wallCollider, Vector3 pos, Quaternion rot)
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
        this.wallCollider = wallCollider;
        transform.position = pos;
        transform.rotation = rot;
        transform.position -= transform.forward * 0.001f;
        gameObject.SetActive(true);

        FixOverhangs();
        FixPortalOverlaps();
    }

    // Ensure the portal cannot extend past the edge of a surface, or intersect a corner
    private void FixOverhangs()
    {
        var testPoints = new List<Vector3>
        {
            new Vector3(-1.1f,  0, 0),
            new Vector3( 1.1f,  0, 0),
            new Vector3( 0, -2.1f, 0),
            new Vector3( 0,  2.1f, 0)
        };

        for (int i = 0; i < 4; ++i)
        {
            Vector3 overhangTestPosition = transform.TransformPoint(testPoints[i]);

            // If the point isn't touching anything, it overhangs
            if (!Physics.CheckSphere(overhangTestPosition, sphereCastSize, overhangMask))
            {
                Vector3 portalOverhangOffset = FindOverhangOffset(testPoints[i]);
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

    private void FixPortalOverlaps()
    {
        // TODO: create a method of depenetrating overlapping portals
        //if(MathUtil.DoBoxesIntersect(collider, otherPortal.collider))
        //{
        //    Vector3 depenetration = MathUtil.GetBoxDepenetration(collider, otherPortal.collider);
        //    transform.Translate(depenetration);
        //}
    }

    public void ResetPortal()
    {
        gameObject.SetActive(false);
        isPlaced = false;
        boxCollider.enabled = false;
        transform.position = new Vector3(100, 100, 100);
        SetPortalRendererMaterial();
    }

    public bool IsPlaced()
    {
        return isPlaced;
    }
    private void SetPortalRendererMaterial()
    {
        if (name == "Portal1")
        {
            renderer.material = meshMaterialBlue;
        }
        else
        {
            renderer.material = meshMaterialOrange;
        }
    }
}