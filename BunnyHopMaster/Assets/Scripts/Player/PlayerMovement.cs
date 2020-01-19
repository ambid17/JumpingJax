using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public LayerMask layersToIgnore;

    public BoxCollider myCollider;
    public Vector3 _newVelocity;
    [SerializeField]
    private bool _grounded;
    [SerializeField]
    private bool _surfing;

    private void Start()
    {
        myCollider = GetComponent<BoxCollider>();
    }

    private void FixedUpdate()
    {
        ApplyGravity();
        CheckGrounded();
        CheckJump();

        var inputVector = GetInputVector();
        var wishDir = inputVector.normalized;
        var wishSpeed = inputVector.magnitude;

        if (_grounded)
        {
            ApplyGroundAcceleration(wishDir, wishSpeed, PlayerConstants.normalSurfaceFriction);
            ClampVelocity(PlayerConstants.MoveSpeed);
            ApplyFriction();
        }
        else
        {
            ApplyAirAcceleration(wishDir, wishSpeed);
        }

        ClampVelocity(PlayerConstants.MaxVelocity);

        transform.position += _newVelocity * Time.deltaTime;

        ResolveCollisions();
    }

    private void ApplyGravity()
    {
        if (!_grounded)
        {
            _newVelocity.y -= PlayerConstants.Gravity * Time.deltaTime;
        }
    }

    private void CheckGrounded()
    {
        _surfing = false;

        var hits = Physics.BoxCastAll(center: myCollider.bounds.center,
            halfExtents: PlayerConstants.BoxCastExtents,
            direction: -transform.up,
            orientation: Quaternion.identity,
            maxDistance: PlayerConstants.BoxCastDistance,
            layerMask: layersToIgnore
            );

        var wasGrounded = _grounded;
        var validHits = hits
            .ToList()
            .FindAll(hit => hit.normal.y >= 0.7f)
            .OrderBy(hit => hit.distance)
            .Where(hit => !hit.collider.isTrigger);

        _grounded = validHits.Count() > 0;

        if (_grounded)
        {
            var closestHit = validHits.First();

            //If the ground is NOT perfectly flat, slide across it
            if (closestHit.normal.y < 1)
            {
                ClipVelocity(closestHit.normal);
            }
            else
            {
                _newVelocity.y = 0;
            }
        }
        else
        {
            //Find the closest collision where the slope is at least 45 degrees
            var surfHits = hits.ToList().FindAll(x => x.normal.y < 0.7f).OrderBy(x => x.distance);
            if (surfHits.Count() > 0)
            {
                transform.position += surfHits.First().normal * 0.02f;
                ClipVelocity(surfHits.First().normal);
                _surfing = true;
            }
        }
    }

    private void CheckJump()
    {
        if (_grounded && Input.GetKey(HotKeyManager.instance.GetKeyFor(PlayerConstants.Jump)))
        {
            _newVelocity.y += PlayerConstants.JumpPower;
            _grounded = false;
        }
    }

    private Vector3 GetInputVector()
    {
        KeyCode left = HotKeyManager.instance.GetKeyFor(PlayerConstants.Left);
        KeyCode right = HotKeyManager.instance.GetKeyFor(PlayerConstants.Right);
        KeyCode forward = HotKeyManager.instance.GetKeyFor(PlayerConstants.Forward);
        KeyCode back = HotKeyManager.instance.GetKeyFor(PlayerConstants.Back);

        var horiz = Input.GetKey(left) ? -PlayerConstants.MoveSpeed : Input.GetKey(right) ? PlayerConstants.MoveSpeed : 0;
        var vert = Input.GetKey(back) ? -PlayerConstants.MoveSpeed : Input.GetKey(forward) ? PlayerConstants.MoveSpeed : 0;
        var inputVelocity = new Vector3(horiz, 0, vert);
        if (inputVelocity.magnitude > PlayerConstants.MoveSpeed)
        {
            inputVelocity *= PlayerConstants.MoveSpeed / inputVelocity.magnitude;
        }

        //Get the velocity vector in world space coordinates
        return transform.TransformDirection(inputVelocity);
    }

    //wishDir: the direction the player wishes to go in the newest frame
    //wishSpeed: the speed the player wishes to go this frame
    private void ApplyGroundAcceleration(Vector3 wishDir, float wishSpeed, float surfaceFriction)
    {
        var currentSpeed = Vector3.Dot(_newVelocity, wishDir); //Vector projection of the current velocity onto the new direction
        var speedToAdd = wishSpeed - currentSpeed;

        var acceleration = PlayerConstants.GroundAcceleration * Time.deltaTime; //acceleration to apply in the newest direction

        if (speedToAdd <= 0)
        {
            return;
        }

        var accelspeed = Mathf.Min(acceleration * wishSpeed * surfaceFriction, speedToAdd);
        _newVelocity += accelspeed * wishDir; //add acceleration in the new direction
    }

    //wishDir: the direction the player  wishes to goin the newest frame
    //wishSpeed: the speed the player wishes to go this frame
    private void ApplyAirAcceleration(Vector3 wishDir, float wishSpeed)
    {
        var wishSpd = Mathf.Min(wishSpeed, PlayerConstants.AirAccelerationCap);
        var currentSpeed = Vector3.Dot(_newVelocity, wishDir);
        var speedToAdd = wishSpd - currentSpeed;

        if (speedToAdd <= 0)
        {
            return;
        }

        var accelspeed = Mathf.Min(speedToAdd, PlayerConstants.AirAcceleration * wishSpeed * Time.deltaTime);
        _newVelocity += accelspeed * wishDir;
    }

    private void ApplyFriction()
    {
        var speed = _newVelocity.magnitude;

        //Don't apply friction if the player isn't moving
        //Clear speed if it's too low to prevent accidental movement
        if (speed < 0.01f)
        {
            _newVelocity = Vector3.zero;
            return;
        }

        var lossInSpeed = speed * PlayerConstants.Friction * Time.deltaTime;
        var newSpeed = Mathf.Max(speed - lossInSpeed, 0);

        if (newSpeed != speed)
        {
            _newVelocity *= newSpeed / speed; //Scale velocity based on friction
        }
    }

    private void ClampVelocity(float range)
    {
        _newVelocity = Vector3.ClampMagnitude(_newVelocity, PlayerConstants.MaxVelocity);
    }

    //Slide off of the impacting surface
    private void ClipVelocity(Vector3 normal)
    {
        // Determine how far along plane to slide based on incoming direction.
        var backoff = Vector3.Dot(_newVelocity, normal);

        for (int i = 0; i < 3; i++)
        {
            var change = normal[i] * backoff;
            _newVelocity[i] -= change;
        }

        // iterate once to make sure we aren't still moving through the plane
        var adjust = Vector3.Dot(_newVelocity, normal);
        if (adjust < 0.0f)
        {
            _newVelocity -= (normal * adjust);
        }
    }

    private void ResolveCollisions()
    {
        var center = transform.position + myCollider.center; // get center of bounding box in world space
        var overlaps = Physics.OverlapBox(center, PlayerConstants.BoxCastExtents, Quaternion.identity);

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
                if (Vector3.Dot(dir, _newVelocity.normalized) > 0)
                {
                    continue;
                }

                Vector3 depenetrationVector = dir * dist; // The vector needed to get outside of the collision

                Debug.Log("depen: " + depenetrationVector.ToString("F5") + " proj " + Vector3.Project(_newVelocity, -dir).ToString("F5"));

                if (!_surfing)
                {
                    transform.position += depenetrationVector;
                    _newVelocity -= Vector3.Project(_newVelocity, -dir);
                }
                else
                {
                    ClipVelocity(dir);
                }
            }
        }
    }

}