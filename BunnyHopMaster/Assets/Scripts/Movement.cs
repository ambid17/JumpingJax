// Author: Crayz
// https://youtube.com/crayz92

using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public int PlayerLayer = 12;
    public LayerMask notPlayerLayerMask;
    public float MoveSpeed = 10f;
    public float MaxVelocity = 100f;

    public float Gravity = 9.8f;
    public float JumpPower = 5f;

    public float GroundAcceleration = 7f;
    public float AirAcceleration = 2.5f;

    public float StopSpeed = 8f;
    public float Friction = 6f;
    public float normalSurfaceFriction = 1f;

    public float AirCap = 3f;
    public BoxCollider AABB;
    public Vector3 _newVelocity;
    [SerializeField]
    private bool _grounded;
    [SerializeField]
    private bool _surfing;

    private void Start()
    {
        AABB = GetComponent<BoxCollider>();
        notPlayerLayerMask = LayerMask.GetMask("Default");
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
            ApplyGroundAcceleration(wishDir, wishSpeed, normalSurfaceFriction);
            ClampVelocity(MoveSpeed);
            ApplyFriction();
        }
        else
        {
            ApplyAirAcceleration(wishDir, wishSpeed);
        }

        ClampVelocity(MaxVelocity);

        transform.position += _newVelocity * Time.deltaTime;

        ResolveCollisions();
    }

    private void ApplyGravity()
    {
        if (!_grounded)
        {
            _newVelocity.y -= Gravity * Time.deltaTime;
        }
    }

    private void CheckGrounded()
    {
        _surfing = false;

        var hits = Physics.BoxCastAll(center: AABB.bounds.center,
            halfExtents: transform.localScale,
            direction: Vector3.down,
            orientation: transform.rotation,
            maxDistance: 0.01f
            );
        
        var wasGrounded = _grounded;
        var validHits = hits
            .ToList()
            .FindAll(hit => hit.normal.y >= 0.7f)
            .OrderBy(hit => hit.distance)
            .Where(hit => hit.collider.name != "TestPlayer");

        _grounded = validHits.Count() > 0;
        if (_grounded)
        {
            var closestHit = validHits.First();

            
            //if (!wasGrounded)
            //{
            //    //bounce off the ground on first hit
            //    transform.position = new Vector3(transform.position.x, closestHit.point.y + .1, transform.position.z);
            //}

            //If the ground is NOT perfectly flat
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
        if (_grounded && Input.GetKey(KeyCode.Space))
        {
            _newVelocity.y += JumpPower;
            _grounded = false;
        }
    }

    private Vector3 GetInputVector()
    {
        var horiz = Input.GetKey(KeyCode.A) ? -MoveSpeed : Input.GetKey(KeyCode.D) ? MoveSpeed : 0;
        var vert = Input.GetKey(KeyCode.S) ? -MoveSpeed : Input.GetKey(KeyCode.W) ? MoveSpeed : 0;
        var inputVelocity = new Vector3(horiz, 0, vert);
        if (inputVelocity.magnitude > MoveSpeed)
        {
            inputVelocity *= MoveSpeed / inputVelocity.magnitude;
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

        var acceleration = GroundAcceleration * Time.deltaTime; //acceleration to apply in the newest direction

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
        var wishSpd = Mathf.Min(wishSpeed, AirCap);
        var currentSpeed = Vector3.Dot(_newVelocity, wishDir);
        var speedToAdd = wishSpd - currentSpeed;

        if (speedToAdd <= 0)
        {
            return;
        }

        var accelspeed = Mathf.Min(speedToAdd, AirAcceleration * wishSpeed * Time.deltaTime);
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

        var lossInSpeed = speed * Friction * Time.deltaTime;
        var newSpeed = Mathf.Max(speed - lossInSpeed, 0);

        if (newSpeed != speed)
        {
            _newVelocity *= newSpeed / speed; //Scale velocity based on friction
        }
    }

    private void ClampVelocity(float range)
    {
        _newVelocity = Vector3.ClampMagnitude(_newVelocity, MaxVelocity);
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
        var center = transform.position + AABB.center; // get center of bounding box in world space
        var overlaps = Physics.OverlapBox(center, AABB.bounds.size, Quaternion.identity);

        foreach (var other in overlaps)
        {
            // If the collider is my own, check the next one
            if(other == AABB)
            {
                continue;
            }

            if (Physics.ComputePenetration(AABB, transform.position, transform.rotation,
                other, other.transform.position, other.transform.rotation,
                out Vector3 dir, out float dist))
            {
                if (Vector3.Dot(dir, _newVelocity.normalized) > 0)
                {
                    continue;
                }

                Vector3 penetrationVector = dir * dist; // The vector needed to get outside of the collision

                if (!_surfing)
                {
                    transform.position += penetrationVector;
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