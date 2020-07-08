using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Tooltip("Red line is current velocity, blue is the new direction")]
    public bool showDebugGizmos = false;
    public LayerMask layersToIgnore;
    public BoxCollider myCollider;
    public CameraMove cameraMove;

    //The velocity applied at the end of every physics frame
    public Vector3 newVelocity;

    [SerializeField]
    private float airAcceleration = PlayerConstants.AirAcceleration;

    [SerializeField]
    private bool grounded;
    [SerializeField]
    private bool surfing;
    [SerializeField]
    private bool crouching;
    [SerializeField]
    private bool wasCrouching;

    private void Start()
    {
        myCollider = GetComponent<BoxCollider>();
        cameraMove = GetComponent<CameraMove>();
    }

    private void FixedUpdate()
    {
        CheckCrouch();
        ApplyGravity();
        CheckGrounded();
        CheckJump();

        var inputVector = GetWorldSpaceInputVector();
        var wishDir = inputVector.normalized;
        var wishSpeed = inputVector.magnitude;

        if (grounded)
        {
            if (IsPlayerWalkingBackwards())
            {
                wishSpeed *= PlayerConstants.BackWardsMoveSpeedScale;
            }
            ApplyGroundAcceleration(wishDir, wishSpeed, PlayerConstants.NormalSurfaceFriction);
            ClampVelocity(PlayerConstants.MoveSpeed);
            ApplyFriction();
        }
        else
        {
            ApplyAirAcceleration(wishDir, wishSpeed);
        }

        ClampVelocity(PlayerConstants.MaxVelocity);

        transform.position += newVelocity * Time.fixedDeltaTime;

        ResolveCollisions();
    }

    private void CheckCrouch()
    {
        wasCrouching = crouching;

        if (InputManager.GetKey(PlayerConstants.Crouch))
        {
            crouching = true;
            myCollider.size = PlayerConstants.CrouchingBoxColliderSize;
        }
        else
        {
            crouching = false;

            if (grounded && wasCrouching)
            {
                transform.position += new Vector3(0, 1, 0);
            }

            myCollider.size = PlayerConstants.BoxColliderSize;
        }
    }

    private void ApplyGravity()
    {
        if (!grounded)
        {
            float gravityScale = GameManager.GetCurrentLevel().gravityMultiplier;
            newVelocity.y -= gravityScale * PlayerConstants.Gravity * Time.fixedDeltaTime;
        }
    }

    private void CheckGrounded()
    {
        surfing = false;

        Vector3 extents = PlayerConstants.BoxCastExtents;
        if (crouching)
        {
            extents = PlayerConstants.CrouchingBoxCastExtents;
        }

        var hits = Physics.BoxCastAll(
            center: myCollider.bounds.center,
            halfExtents: extents,
            direction: -transform.up,
            orientation: Quaternion.identity,
            maxDistance: PlayerConstants.BoxCastDistance,
            layerMask: layersToIgnore
            );
        
        var wasGrounded = grounded;
        var validHits = hits
            .ToList()
            .FindAll(hit => hit.normal.y >= 0.7f)
            .OrderBy(hit => hit.distance)
            .Where(hit => !hit.collider.isTrigger)
            .Where(hit => !Physics.GetIgnoreCollision(hit.collider, myCollider))
            .Where(hit => hit.point.y < transform.position.y && hit.point != Vector3.zero);

        grounded = validHits.Count() > 0;

        if (!grounded)
        {
            grounded = ConfirmGrounded(validHits);
        }

        if (grounded)
        {
            newVelocity.y = 0;
        }
        else
        {
            //Find the closest collision where the slope is at least 45 degrees
            var surfHits = hits.ToList().FindAll(x => x.normal.y < 0.7f).OrderBy(x => x.distance);
            if (surfHits.Count() > 0)
            {
                transform.position += surfHits.First().normal * 0.02f;
                ClipVelocity(surfHits.First().normal);
                surfing = true;
            }
        }
    }

    private bool ConfirmGrounded(IEnumerable<RaycastHit> hits)
    {
        Vector3 extents = PlayerConstants.BoxCastExtents;
        if (crouching)
        {
            extents = PlayerConstants.CrouchingBoxCastExtents;
        }

        // We have to manually check if there is a collision, because boxcastall 
        // doesn't return the correct information when already colliding
        var overlappingColliders = Physics.OverlapBox(
            center: myCollider.bounds.center,
            halfExtents: extents,// + new Vector3(0.1f, 0.1f, 0.1f),
            orientation: Quaternion.identity,
            layerMask: layersToIgnore);

        

        foreach (Collider collider in overlappingColliders)
        {
            if (collider.isTrigger)
            {
                continue;
            }

            if(collider.transform.position.y < transform.position.y && !Physics.GetIgnoreCollision(collider, myCollider))
            {
                return true;
            }
        }

        return false;
    }

    private void CheckJump()
    {
        if (grounded && InputManager.GetKey(PlayerConstants.Jump))
        {
            newVelocity.y += crouching ? PlayerConstants.CrouchingJumpPower : PlayerConstants.JumpPower;
            grounded = false;
        }
    }

    private Vector3 GetWorldSpaceInputVector()
    {
        float moveSpeed = crouching ? PlayerConstants.CrouchingMoveSpeed : PlayerConstants.MoveSpeed;

        var inputVelocity = GetInputVelocity(moveSpeed);
        if (inputVelocity.magnitude > moveSpeed)
        {
            inputVelocity *= moveSpeed / inputVelocity.magnitude;
        }

        //Get the velocity vector in world space coordinates, by rotating around the camera's y-axis
        return Quaternion.AngleAxis(cameraMove.playerCamera.transform.rotation.eulerAngles.y, Vector3.up) * inputVelocity;
    }

    private Vector3 GetInputVelocity(float moveSpeed)
    {
        float horizontalSpeed = 0;
        float verticalSpeed = 0;

        if (InputManager.GetKey(PlayerConstants.Left))
        {
            horizontalSpeed = -moveSpeed;
        }

        if (InputManager.GetKey(PlayerConstants.Right))
        {
            horizontalSpeed = moveSpeed;
        }

        if (InputManager.GetKey(PlayerConstants.Back))
        {
            verticalSpeed = -moveSpeed;
        }

        if (InputManager.GetKey(PlayerConstants.Forward))
        {
            verticalSpeed = moveSpeed;
        }

        return new Vector3(horizontalSpeed, 0, verticalSpeed);
    }

    private bool IsPlayerWalkingBackwards()
    {
        Vector3 inputDirection = GetInputVelocity(PlayerConstants.MoveSpeed);

        return inputDirection.z < 0;
    }

    //wishDir: the direction the player wishes to go in the newest frame
    //wishSpeed: the speed the player wishes to go this frame
    private void ApplyGroundAcceleration(Vector3 wishDir, float wishSpeed, float surfaceFriction)
    {
        var currentSpeed = Vector3.Dot(newVelocity, wishDir); //Vector projection of the current velocity onto the new direction
        var speedToAdd = wishSpeed - currentSpeed;

        var acceleration = PlayerConstants.GroundAcceleration * Time.fixedDeltaTime; //acceleration to apply in the newest direction

        if (speedToAdd <= 0)
        {
            return;
        }

        var accelspeed = Mathf.Min(acceleration * wishSpeed * surfaceFriction, speedToAdd);
        newVelocity += accelspeed * wishDir; //add acceleration in the new direction
    }

    //wishDir: the direction the player  wishes to goin the newest frame
    //wishSpeed: the speed the player wishes to go this frame
    private void ApplyAirAcceleration(Vector3 wishDir, float wishSpeed)
    {
        var wishSpd = Mathf.Min(wishSpeed, PlayerConstants.AirAccelerationCap);
        var currentSpeed = Vector3.Dot(newVelocity, wishDir);
        var speedToAdd = wishSpd - currentSpeed;
        
        if (speedToAdd <= 0)
        {
            return;
        }

        var accelspeed = Mathf.Min(speedToAdd, airAcceleration * wishSpeed * Time.fixedDeltaTime);
        var velocityTransformation = accelspeed * wishDir;

        if (showDebugGizmos)
        {
            Debug.DrawRay(transform.position, newVelocity + velocityTransformation, Color.red, 1);
            Debug.DrawRay(transform.position, wishDir, Color.blue, 1);
            Debug.DrawRay(transform.position, velocityTransformation, Color.green, 1);
        }

        newVelocity += velocityTransformation;
    }

    private void ApplyFriction()
    {
        var speed = newVelocity.magnitude;

        // Don't apply friction if the player isn't moving
        // Clear speed if it's too low to prevent accidental movement
        // Also makes the player's friction feel more snappy
        if (speed < PlayerConstants.MinimumSpeedCutoff)
        {
            newVelocity = Vector3.zero;
            return;
        }

        var control = (speed < PlayerConstants.StopSpeed) ? PlayerConstants.StopSpeed : speed;
        var lossInSpeed = control * PlayerConstants.Friction * Time.fixedDeltaTime;
        var newSpeed = Mathf.Max(speed - lossInSpeed, 0);

        if (newSpeed != speed)
        {
            newVelocity *= newSpeed / speed; //Scale velocity based on friction
        }
    }

    private void ClampVelocity(float range)
    {
        newVelocity = Vector3.ClampMagnitude(newVelocity, PlayerConstants.MaxVelocity);
    }

    // Slide off of the impacting surface
    private void ClipVelocity(Vector3 normal)
    {
        // Determine how far along plane to slide based on incoming direction.
        var backoff = Vector3.Dot(newVelocity, normal);

        if(backoff < 0)
        {
            backoff *= PlayerConstants.Overbounce;
        }
        else
        {
            backoff /= PlayerConstants.Overbounce;
        }

        for (int i = 0; i < 3; i++)
        {
            float change = normal[i] * backoff;
            newVelocity[i] -= change;
        }
    }

    private void ResolveCollisions()
    {
        var center = transform.position + myCollider.center; // get center of bounding box in world space

        Vector3 extents = myCollider.bounds.extents;
        if (crouching)
        {
            extents = PlayerConstants.CrouchingBoxCastExtents;
        }

        var overlaps = Physics.OverlapBox(center, extents, Quaternion.identity);

        foreach (var other in overlaps)
        {
            // If the collider is my own, check the next one
            if (other == myCollider || other.isTrigger)
            {
                continue;
            }

            if (Physics.ComputePenetration(myCollider, transform.position, transform.rotation,
                other, other.transform.position, other.transform.rotation,
                out Vector3 dir, out float dist))
            {
                if (Vector3.Dot(dir, newVelocity.normalized) > 0 ||
                    Physics.GetIgnoreCollision(myCollider, other))
                {
                    continue;
                }

                Vector3 depenetrationVector = dir * dist; // The vector needed to get outside of the collision

                if (showDebugGizmos)
                {
                    Debug.Log("depen: " + depenetrationVector.ToString("F8") + " proj " + Vector3.Project(newVelocity, -dir).ToString("F5"));
                }

                if (!surfing)
                {
                    transform.position += depenetrationVector;
                }
                else
                {
                    ClipVelocity(dir);
                }
            }
        }
    }

}