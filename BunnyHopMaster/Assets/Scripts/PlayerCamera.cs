using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour {
    Camera mainCamera;
    public Vector3 headPositionOffset;
    // Start is called before the first frame update
    void Start() {
        headPositionOffset = new Vector3(0, 1.7f, 0);
        mainCamera = Camera.main;
    }

    // Update is called once per frame
    void Update() {
        mainCamera.transform.position = transform.position + headPositionOffset;
        mainCamera.transform.rotation = transform.rotation;
    }
}
