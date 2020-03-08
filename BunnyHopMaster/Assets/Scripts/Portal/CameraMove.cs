using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    private const float moveSpeed = 7.5f;
    private const float cameraSpeed = 3.0f;

    public Quaternion TargetRotation { private set; get; }
    public Camera playerCamera;
    
    //private Vector3 moveVector = Vector3.zero;
    //private float moveY = 0.0f;

    //private new Rigidbody rigidbody;

    private void Awake()
    {
        //rigidbody = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        TargetRotation = transform.rotation;
        playerCamera = Camera.main;
    }

    private void Update()
    {
        // Rotate the camera.
        var rotation = new Vector2(-Input.GetAxis("Mouse Y"), Input.GetAxis("Mouse X"));
        var targetEuler = TargetRotation.eulerAngles + (Vector3)rotation * cameraSpeed;
        if(targetEuler.x > 180.0f)
        {
            targetEuler.x -= 360.0f;
        }
        targetEuler.x = Mathf.Clamp(targetEuler.x, -75.0f, 75.0f);
        TargetRotation = Quaternion.Euler(targetEuler);

        Quaternion rotationToApply = Quaternion.Slerp(playerCamera.transform.rotation, TargetRotation,
            Time.deltaTime * 15.0f);
        playerCamera.transform.rotation = rotationToApply;
        transform.rotation = Quaternion.AngleAxis(TargetRotation.eulerAngles.y, Vector3.up);

        // Move the camera.
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        //moveVector = new Vector3(x, 0.0f, z) * moveSpeed;

        //moveY = Input.GetAxis("Elevation");
    }

    private void FixedUpdate()
    {
        //Vector3 newVelocity = transform.TransformDirection(moveVector);
        //newVelocity.y += moveY * moveSpeed;
        //rigidbody.velocity = newVelocity;
    }

    public void ResetTargetRotation()
    {
        TargetRotation = Quaternion.LookRotation(transform.forward, Vector3.up);
    }
}
