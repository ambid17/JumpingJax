using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPortalableController : PortalableObject
{
    private CameraMove cameraMove;
    private PlayerMovement playerMovement;

    protected override void Awake()
    {
        CreateClone();
        playerMovement = GetComponent<PlayerMovement>();

        cameraMove = GetComponent<CameraMove>();
    }

    private void CreateClone()
    {
        cloneObject = new GameObject();
        cloneObject.SetActive(false);
        var meshFilter = cloneObject.AddComponent<MeshFilter>();
        var meshRenderer = cloneObject.AddComponent<MeshRenderer>();

        meshFilter.mesh = GetComponentInChildren<MeshFilter>().mesh;
        meshRenderer.materials = GetComponentInChildren<MeshRenderer>().materials;
        cloneObject.transform.localScale = transform.localScale;

        collider = GetComponent<Collider>();
    }

    public override void Warp()
    {
        WarpPlayer();
        cameraMove.ResetTargetRotation();
    }

    public virtual void WarpPlayer()
    {
        var inTransform = inPortal.transform;
        var outTransform = outPortal.transform;

        // Update position of object.
        Vector3 relativePos = inTransform.InverseTransformPoint(transform.position);
        relativePos = halfTurn * relativePos;
        transform.position = outTransform.TransformPoint(relativePos);

        // Update rotation of object.
        Quaternion relativeRot = Quaternion.Inverse(inTransform.rotation) * transform.rotation;
        relativeRot = halfTurn * relativeRot;
        transform.rotation = outTransform.rotation * relativeRot;

        // Update velocity of rigidbody.
        Vector3 relativeVel = inTransform.InverseTransformDirection(playerMovement.newVelocity);
        relativeVel = halfTurn * relativeVel;
        playerMovement.newVelocity = outTransform.TransformDirection(relativeVel);

        // Swap portal references.
        var tmp = inPortal;
        inPortal = outPortal;
        outPortal = tmp;
    }
}
