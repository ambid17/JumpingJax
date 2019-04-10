using UnityEngine;

// Contains the command the user wishes upon the character
struct PlayerWishCommand {
    public float forwardMove;
    public float rightMove;
    public float upMove;
}

public class PlayerMovement : MonoBehaviour {
    public Transform playerCamera;
    private float rotX = 0.0f;
    private float rotY = 0.0f;

    public float playerViewYOffset = 1.8f; // The height at which the camera is bound to
    public float mouseSensitivity = 1;

    // Frame occuring factors
    public float gravity = 10;
    public float friction = 5; //Ground friction

    public float moveSpeed = 6;                // Ground move speed
    public float runAcceleration = 12;         // Ground accel
    public float runDeacceleration = 12;       // Deacceleration that occurs when running on the ground
    public float airAcceleration = 1.8f;          // Air accel
    public float airDecceleration = 7;         // Deacceleration experienced when ooposite strafing
    public float sideStrafeAcceleration = 40;  // How fast acceleration occurs to get up to sideStrafeSpeed when
    public float sideStrafeSpeed = 1;          // What the max speed to generate when side strafing
    //gives a vertical jump height of ~1m accounting for gravity
    public float jumpSpeed = 4.5f;                // The speed at which the character's up axis gains when hitting jump
    public bool holdJumpToBhop = false;           // When enabled allows player to just hold jump button to keep on bhopping perfectly. Beware: smells like casual.

    public GUIStyle style;

    //FPS
    public float fpsDisplayRate = 4.0f; // 4 updates per sec
    private int frameCount = 0;
    private float deltaTime = 0.0f;
    private float fps = 0.0f;

    private CharacterController characterController;

    private Vector3 playerVelocity = Vector3.zero;
    private float playerTopVelocity = 0.0f;

    // Q3: players can queue the next jump just before he hits the ground
    private bool wishJump = false;

    // Used to display real time fricton values
    private float playerFriction = 0.0f;

    // Player commands, stores wish commands that the player asks for (Forward, back, jump, etc)
    private PlayerWishCommand _cmd;

    private void Start() {
        // Hide the cursor
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        if (playerCamera == null) {
            Camera mainCamera = Camera.main;
            if (mainCamera != null)
                playerCamera = mainCamera.gameObject.transform;
        }

        // Put the camera inside the capsule collider
        playerCamera.position = new Vector3(
            transform.position.x,
            transform.position.y + playerViewYOffset,
            transform.position.z);

        characterController = GetComponent<CharacterController>();
    }

    private void Update() {
        // Do FPS calculation every fpsDisplayRate frames
        frameCount++;
        deltaTime += Time.deltaTime;
        if (deltaTime > 1.0 / fpsDisplayRate) {
            fps = Mathf.Round(frameCount / deltaTime);
            frameCount = 0;
            deltaTime -= 1.0f / fpsDisplayRate;
        }

        // Ensure that the cursor is locked into the screen
        if (Cursor.lockState != CursorLockMode.Locked) {
            if (Input.GetButtonDown("Fire1"))
                Cursor.lockState = CursorLockMode.Locked;
        }

        // Camera rotation stuff, mouse controls this shit 
        rotX -= Input.GetAxisRaw("Mouse Y") * mouseSensitivity;
        rotY += Input.GetAxisRaw("Mouse X") * mouseSensitivity;

        // Clamp the X rotation
        if (rotX < -90)
            rotX = -90;
        else if (rotX > 90)
            rotX = 90;

        transform.rotation = Quaternion.Euler(0, rotY, 0); // Rotates the collider
        playerCamera.rotation = Quaternion.Euler(rotX, rotY, 0); // Rotates the camera

        // Check and set movement
        QueueJump();
        if (characterController.isGrounded)
            GroundMove();
        else if (!characterController.isGrounded)
            AirMove();

        // Apply movement
        characterController.Move(playerVelocity * Time.deltaTime);

        // Calculate top velocity
        Vector3 udp = playerVelocity;
        udp.y = 0.0f;
        if (udp.magnitude > playerTopVelocity)
            playerTopVelocity = udp.magnitude;

        //Need to move the camera after the player has been moved because otherwise 
        //the camera will clip the player if going fast enough and will always be 1 frame behind.
        playerCamera.position = new Vector3(
            transform.position.x,
            transform.position.y + playerViewYOffset,
            transform.position.z);
    }

    private void CheckWishMovement() {
        _cmd.forwardMove = Input.GetAxisRaw("Vertical");
        _cmd.rightMove = Input.GetAxisRaw("Horizontal");
    }

    private void QueueJump() {
        if (holdJumpToBhop) {
            wishJump = Input.GetButton("Jump");
            return;
        }

        if (Input.GetButtonDown("Jump") && !wishJump)
            wishJump = true;
        if (Input.GetButtonUp("Jump"))
            wishJump = false;
    }


    // Execs when the player is in the air
    private void AirMove() {
        CheckWishMovement();

        float wishvel = airAcceleration;
        float accel;

        Vector3 wishdir = new Vector3(_cmd.rightMove, 0, _cmd.forwardMove);
        wishdir = transform.TransformDirection(wishdir);

        float wishspeed = wishdir.magnitude;
        wishspeed *= moveSpeed;

        wishdir.Normalize();

        // CSGO-like Aircontrol
        if (Vector3.Dot(playerVelocity, wishdir) < 0)
            accel = airDecceleration;
        else
            accel = airAcceleration;

        // If the player is ONLY holding left OR right
        if (_cmd.forwardMove == 0 && _cmd.rightMove != 0) {
            if (wishspeed > sideStrafeSpeed)
                wishspeed = sideStrafeSpeed;
            accel = sideStrafeAcceleration;
        }

        Accelerate(wishdir, wishspeed, accel);

        // Apply gravity
        playerVelocity.y -= gravity * Time.deltaTime;
    }

    // Called every frame when the engine detects that the player is on the ground
    private void GroundMove() {
        // Do not apply friction if the player is queueing up the next jump
        if (!wishJump)
            ApplyFriction(1.0f);
        else
            ApplyFriction(0);

        CheckWishMovement();

        Vector3 wishdir = new Vector3(_cmd.rightMove, 0, _cmd.forwardMove);
        wishdir = transform.TransformDirection(wishdir);
        wishdir.Normalize();

        var wishspeed = wishdir.magnitude;
        wishspeed *= moveSpeed;

        Accelerate(wishdir, wishspeed, runAcceleration);

        // Reset the gravity velocity
        playerVelocity.y = -gravity * Time.deltaTime;

        if (wishJump) {
            playerVelocity.y = jumpSpeed;
            wishJump = false;
        }
    }

    // Applies friction to the player, called in both the air and on the ground
    private void ApplyFriction(float t) {
        Vector3 vec = playerVelocity;
        float speed;
        float newSpeed;
        float control;
        float dampFactor;

        vec.y = 0.0f;
        speed = vec.magnitude;
        dampFactor = 0.0f;

        // Only if the player is on the ground then apply friction
        if (characterController.isGrounded) {
            control = speed < runDeacceleration ? runDeacceleration : speed;
            dampFactor = control * friction * Time.deltaTime * t;
        }

        newSpeed = speed - dampFactor;
        playerFriction = newSpeed;
        if (newSpeed < 0)
            newSpeed = 0;
        if (speed > 0)
            newSpeed /= speed;

        playerVelocity.x *= newSpeed;
        playerVelocity.z *= newSpeed;
    }

    private void Accelerate(Vector3 wishdir, float wishspeed, float accel) {
        float addspeed;
        float accelspeed;
        float currentspeed;

        currentspeed = Vector3.Dot(playerVelocity, wishdir);
        addspeed = wishspeed - currentspeed;
        if (addspeed <= 0)
            return;
        accelspeed = accel * Time.deltaTime * wishspeed;
        if (accelspeed > addspeed)
            accelspeed = addspeed;

        playerVelocity.x += accelspeed * wishdir.x;
        playerVelocity.z += accelspeed * wishdir.z;
    }

    private void OnGUI() {
        GUI.Label(new Rect(0, 0, 400, 100), "FPS: " + fps, style);
        var ups = characterController.velocity;
        ups.y = 0;
        GUI.Label(new Rect(0, 15, 400, 100), "Speed: " + Mathf.Round(ups.magnitude * 100) / 100 + "m/s", style);
        GUI.Label(new Rect(0, 30, 400, 100), "Top Speed: " + Mathf.Round(playerTopVelocity * 100) / 100 + "m/s", style);
    }
}