<<<<<<< HEAD
﻿using System.Collections;
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

    private bool isPlaced = true;
    [SerializeField]
    private Collider wallCollider;

    public bool isDebug = true;

    private List<PortalableObject> portalObjects = new List<PortalableObject>();

    private Material material;
    private new Renderer renderer;
    private new BoxCollider collider;

    private float sphereCastSize = 0.02f;
    private float bigSphereCastSize = 0.04f;

    private void Awake()
    {
        collider = GetComponent<BoxCollider>();
        renderer = GetComponent<Renderer>();
        material = renderer.material;
    }

    private void Start()
    {
        PlacePortal(wallCollider, transform.position, transform.rotation);
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
        material.SetColor("_Colour", colour);
        outlineRenderer.material.SetColor("_OutlineColour", colour);
    }

    public void SetMaskID(int id)
    {
        material.SetInt("_MaskID", id);
    }

    public void SetTexture(RenderTexture tex)
    {
        material.mainTexture = tex;
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

        if(portalObjects.Contains(obj))
        {
            portalObjects.Remove(obj);
            obj.ExitPortal(wallCollider);
        }
    }

    public void PlacePortal(Collider wallCollider, Vector3 pos, Quaternion rot)
    {
        this.wallCollider = wallCollider;
        transform.position = pos;
        transform.rotation = rot;
        transform.position -= transform.forward * 0.001f;

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

        for(int i = 0; i < 4; ++i)
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

    public void RemovePortal()
    {
        gameObject.SetActive(false);
        isPlaced = false;
    }

    public bool IsPlaced()
    {
        return isPlaced;
    }
}
=======
﻿using System.Collections;
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

    private bool isPlaced = true;
    [SerializeField]
    private Collider wallCollider;

    private List<PortalableObject> portalObjects = new List<PortalableObject>();

    private Material material;
    private new Renderer renderer;
    private new BoxCollider collider;

    private float sphereCastSize = 0.05f;
    private float overhangCheckDistance = 2.1f;

    private void Awake()
    {
        collider = GetComponent<BoxCollider>();
        renderer = GetComponent<Renderer>();
        material = renderer.material;
    }

    private void Start()
    {
        PlacePortal(wallCollider, transform.position, transform.rotation);
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
        material.SetColor("_Colour", colour);
        outlineRenderer.material.SetColor("_OutlineColour", colour);
    }

    public void SetMaskID(int id)
    {
        material.SetInt("_MaskID", id);
    }

    public void SetTexture(RenderTexture tex)
    {
        material.mainTexture = tex;
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

        if(portalObjects.Contains(obj))
        {
            portalObjects.Remove(obj);
            obj.ExitPortal(wallCollider);
        }
    }

    public void PlacePortal(Collider wallCollider, Vector3 pos, Quaternion rot)
    {
        this.wallCollider = wallCollider;
        transform.position = pos;
        transform.rotation = rot;
        transform.position -= transform.forward * 0.001f;

        FixOverhangs();
        FixIntersects();
    }

    // Ensure the portal cannot extend past the edge of a surface.
    private void FixOverhangs()
    {
        var testPoints = new List<Vector3>
        {
            new Vector3(-1.1f,  0.0f, 0.1f),
            new Vector3( 1.1f,  0.0f, 0.1f),
            new Vector3( 0.0f, -2.1f, 0.1f),
            new Vector3( 0.0f,  2.1f, 0.1f)
        };

        var testDirs = new List<Vector3>
        {
             Vector3.right,
            -Vector3.right,
             Vector3.up,
            -Vector3.up
        };

        for(int i = 0; i < 4; ++i)
        {
            RaycastHit hit;
            Vector3 raycastPos = transform.TransformPoint(testPoints[i]);
            Vector3 raycastDir = transform.TransformDirection(testDirs[i]);

            // If the point is already in a wall, it's not overhanging
            if(Physics.CheckSphere(raycastPos, sphereCastSize, placementMask))
            {
                break;
            }
            else if(Physics.Raycast(raycastPos, raycastDir, out hit, overhangCheckDistance, placementMask))
            {
                var offset = hit.point - raycastPos;
                transform.Translate(offset, Space.World);
            }
        }
    }

    // Ensure the portal cannot intersect a section of wall.
    private void FixIntersects()
    {
        var testDirs = new List<Vector3>
        {
             Vector3.right,
            -Vector3.right,
             Vector3.up,
            -Vector3.up
        };

        var testDists = new List<float> { 1.1f, 1.1f, 2.1f, 2.1f };

        for (int i = 0; i < 4; ++i)
        {
            RaycastHit hit;
            Vector3 raycastPos = transform.TransformPoint(0.0f, 0.0f, -0.1f);
            Vector3 raycastDir = transform.TransformDirection(testDirs[i]);

            if (Physics.Raycast(raycastPos, raycastDir, out hit, testDists[i], placementMask))
            {
                var offset = (hit.point - raycastPos);
                var newOffset = -raycastDir * (testDists[i] - offset.magnitude);
                transform.Translate(newOffset, Space.World);
            }
        }
    }

    // Once positioning has taken place, ensure the portal isn't intersecting anything.
    private bool CheckOverlap()
    {
        var checkPosition = transform.position - new Vector3(0.0f, 0.0f, 0.1f);
        var checkExtents = new Vector3(0.9f, 1.9f, 0.05f);
        if (Physics.CheckBox(checkPosition, checkExtents, transform.rotation, placementMask))
        {
            return false;
        }
        return true;
    }

    public void RemovePortal()
    {
        gameObject.SetActive(false);
        isPlaced = false;
    }

    public bool IsPlaced()
    {
        return isPlaced;
    }
}
>>>>>>> 5f7eea4... Initial commit
