using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {
    public float friction = 1;
    public float ground_accelerate = 1;
    public float max_velocity_ground = 2.5F;
    public float air_accelerate = 1;
    public float max_velocity_air = 5;


    void Start() {

    }

    void FixedUpdate() {
        float inputX = Input.GetAxis("Horizontal");
        float inputY = Input.GetAxis("Vertical");

        Vector3 desiredMove = transform.forward * inputY + transform.right * inputX;
        //desiredMove = Vector3.ProjectOnPlane(desiredMove, m_GroundContactNormal).normalized;

    }

    // accelDir: normalized direction that the player has requested to move (taking into account the movement keys and look direction)
    // prevVelocity: The current velocity of the player, before any additional calculations
    // accelerate: The server-defined player acceleration value
    // max_velocity: The server-defined maximum player velocity (this is not strictly adhered to due to strafejumping)
    private Vector3 Accelerate(Vector3 accelDir, Vector3 prevVelocity, float accelerate, float max_velocity) {
        float projVel = Vector3.Dot(prevVelocity, accelDir); // Vector projection of Current velocity onto accelDir.
        float accelVel = accelerate * Time.fixedDeltaTime; // Accelerated velocity in direction of movment

        // If necessary, truncate the accelerated velocity so the vector projection does not exceed max_velocity
        if (projVel + accelVel > max_velocity)
            accelVel = max_velocity - projVel;

        return prevVelocity + accelDir * accelVel;
    }

    private Vector3 MoveGround(Vector3 accelDir, Vector3 prevVelocity) {
        // Apply Friction
        float speed = prevVelocity.magnitude;
        if (speed != 0) // To avoid divide by zero errors
        {
            float drop = speed * friction * Time.fixedDeltaTime;
            prevVelocity *= Mathf.Max(speed - drop, 0) / speed; // Scale the velocity based on friction.
        }

        // ground_accelerate and max_velocity_ground are server-defined movement variables
        return Accelerate(accelDir, prevVelocity, ground_accelerate, max_velocity_ground);
    }

    private Vector3 MoveAir(Vector3 accelDir, Vector3 prevVelocity) {
        // air_accelerate and max_velocity_air are server-defined movement variables
        return Accelerate(accelDir, prevVelocity, air_accelerate, max_velocity_air);
    }
}
