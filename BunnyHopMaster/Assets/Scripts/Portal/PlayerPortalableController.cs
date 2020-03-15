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

    protected override void LateUpdate()
    {
        if (inPortal == null || outPortal == null)
        {
            return;
        }

        if (cloneObject.activeSelf && inPortal.IsPlaced() && outPortal.IsPlaced())
        {
            var inTransform = inPortal.transform;
            var outTransform = outPortal.transform;

            // Update position of clone.
            Vector3 relativePos = inTransform.InverseTransformPoint(transform.position);
            relativePos = halfTurn * relativePos;
            cloneObject.transform.position = outTransform.TransformPoint(relativePos);

            // Update rotation of clone.
            Quaternion playerYRotation = cameraMove.playerCamera.transform.rotation;
            playerYRotation.eulerAngles = new Vector3(0, playerYRotation.eulerAngles.y, 0);
            Quaternion relativeRot = Quaternion.Inverse(inTransform.rotation) * playerYRotation;
            relativeRot = halfTurn * relativeRot;
            cloneObject.transform.rotation = outTransform.rotation * relativeRot;
        }
        else
        {
            cloneObject.transform.position = cloneSpawnPosition;
        }
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
        cloneObject.layer = PlayerConstants.PlayerLayer;

        collider = GetComponent<Collider>();
    }

    public override void Warp()
    {
        WarpPlayer();
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
        Quaternion relativeRot = Quaternion.Inverse(inTransform.rotation) * cameraMove.TargetRotation;
        relativeRot = halfTurn * relativeRot;
        cameraMove.SetTargetRotation(outTransform.rotation * relativeRot);

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
