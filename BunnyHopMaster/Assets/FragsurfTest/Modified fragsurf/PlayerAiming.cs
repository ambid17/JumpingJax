using UnityEngine;

public class PlayerAiming : MonoBehaviour
{
	[Header("References")]
	public Transform bodyTransform;
    public Transform camera;

	[Header("Sensitivity")]
	public float sensitivityMultiplier = 1f;
	public float horizontalSensitivity = 1f;
	public float verticalSensitivity   = 1f;

	[Header("Restrictions")]
	public float minYRotation = -90f;
	public float maxYRotation = 90f;

	//The real rotation of the camera without recoil
	private Vector3 realRotation;

	private void Start()
	{
		// Lock the mouse
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible   = false;
        sensitivityMultiplier = OptionsPreferencesManager.GetSensitivity();
        camera = Camera.main.transform;
	}

	private void Update()
	{
		// Fix pausing
		if (Mathf.Abs(Time.timeScale) <= 0)
			return;

		// Input
		float xMovement = Input.GetAxisRaw("Mouse X") * horizontalSensitivity * sensitivityMultiplier * 10;
		float yMovement = -Input.GetAxisRaw("Mouse Y") * verticalSensitivity  * sensitivityMultiplier * 10;

		// Calculate real rotation from input
		realRotation   = new Vector3(Mathf.Clamp(realRotation.x + yMovement, minYRotation, maxYRotation), realRotation.y + xMovement, realRotation.z);
		realRotation.z = Mathf.Lerp(realRotation.z, 0f, Time.deltaTime * 3f);

        //Apply real rotation to body
        Vector3 playerRotation = realRotation;
        playerRotation.x = 0;
        playerRotation.z = 0;
		bodyTransform.eulerAngles = playerRotation;

        camera.eulerAngles = realRotation;
	}
}
