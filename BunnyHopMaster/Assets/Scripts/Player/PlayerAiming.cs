using UnityEngine;

public class PlayerAiming : MonoBehaviour
{
    [Header("References")]
    public Transform playerCamera;

    [Header("Sensitivity")]
    public float sensitivityMultiplier = 1f;
    public float horizontalSensitivity = 1f;
    public float verticalSensitivity = 1f;

    [Header("Restrictions")]
    public float minYRotation = -90f;
    public float maxYRotation = 90f;

    //The real rotation of the camera without recoil
    private Vector3 realRotation;

    private void Start()
    {
        // Lock the mouse
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        sensitivityMultiplier = OptionsPreferencesManager.GetSensitivity();
        playerCamera = Camera.main.transform;
    }

    private void Update()
    {
        // Fix pausing
        if (Mathf.Abs(Time.timeScale) <= 0)
            return;

        // Input
        float xMovement = Input.GetAxisRaw(PlayerConstants.MouseX) * horizontalSensitivity * sensitivityMultiplier * 10;
        float yMovement = -Input.GetAxisRaw(PlayerConstants.MouseY) * verticalSensitivity * sensitivityMultiplier * 10;

        // Calculate real rotation from input
        realRotation = new Vector3(Mathf.Clamp(realRotation.x + yMovement, minYRotation, maxYRotation), realRotation.y + xMovement, realRotation.z);
        realRotation.z = Mathf.Lerp(realRotation.z, 0f, Time.deltaTime * 3f);

        //Apply real rotation to body, but only apply "y" rotation so the bounding box doesn't move oddly
        Vector3 playerRotation = realRotation;
        playerRotation.x = 0;
        playerRotation.z = 0;
        transform.eulerAngles = playerRotation;

        playerCamera.eulerAngles = realRotation;
    }
}