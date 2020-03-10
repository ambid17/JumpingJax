using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    public float sensitivityMultiplier;

    public Quaternion TargetRotation { private set; get; }
    public Camera playerCamera;

    private void Awake()
    {
        //rigidbody = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        TargetRotation = transform.rotation;
        playerCamera = Camera.main;
    }

    private void Start()
    {
        sensitivityMultiplier = OptionsPreferencesManager.GetSensitivity();
    }

    private void Update()
    {
        if(Time.timeScale == 0)
        {
            return;
        }

        // Rotate the camera.
        var rotation = new Vector2(-Input.GetAxis("Mouse Y"), Input.GetAxis("Mouse X"));
        var targetEuler = TargetRotation.eulerAngles + (Vector3)rotation * sensitivityMultiplier;
        if(targetEuler.x > 180.0f)
        {
            targetEuler.x -= 360.0f;
        }
        targetEuler.x = Mathf.Clamp(targetEuler.x, -75.0f, 75.0f);
        TargetRotation = Quaternion.Euler(targetEuler);

        playerCamera.transform.rotation = TargetRotation;
        transform.rotation = Quaternion.AngleAxis(TargetRotation.eulerAngles.y, Vector3.up);
    }

    public void ResetTargetRotation()
    {
        TargetRotation = Quaternion.LookRotation(transform.forward, Vector3.up);
    }
}
