using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProgress : MonoBehaviour
{
    [SerializeField]
    public Checkpoint currentCheckpoint;
    public PlayerUI playerUI;
    [Tooltip("This is the minimum Y value the player can fall to before they are respawned to the last checkpoint")]
    public float playerFallingBoundsReset = 0;

    public PlayerMovement playerMovement;
    public CameraMove cameraMove;

    private void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
        cameraMove = GetComponent<CameraMove>();
    }

    void Update()
    {
        if (Time.timeScale == 0)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            Respawn();
        }

        if (gameObject.transform.position.y < playerFallingBoundsReset)
        {
            Respawn();
        }

        if (currentCheckpoint != null && !GameManager.Instance.didWinCurrentLevel)
        {
            if (currentCheckpoint.level == GameManager.GetCurrentLevel().numberOfCheckpoints)
            {
                GameManager.FinishedLevel();
                playerUI.ShowWinScreen();
                Time.timeScale = 0;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Checkpoint checkPointHit = other.gameObject.GetComponent<Checkpoint>();
        if (checkPointHit)
        {
            HitNewCheckPoint(checkPointHit);
        }
    }

    public void HitNewCheckPoint(Checkpoint checkpoint)
    {
        if (currentCheckpoint == null)
        {
            checkpoint.SetCompleted();
            currentCheckpoint = checkpoint;
        }
        else
        {
            if (currentCheckpoint.level <= checkpoint.level)
            {
                checkpoint.SetCompleted();
                currentCheckpoint = checkpoint;
            }
        }
    }

    public void Respawn()
    {
        Vector3 respawnPosition = currentCheckpoint.gameObject.transform.position + PlayerConstants.PlayerSpawnOffset;
        transform.position = respawnPosition;
<<<<<<< HEAD

        playerMovement.newVelocity = Vector3.zero;

        cameraMove.ResetTargetRotation();

        // If the player is restarting at the beginning, reset timer
        if(currentCheckpoint.level == 1)
        {
            GameManager.Instance.currentCompletionTime = 0;
        }
=======
        playerMovement.newVelocity = Vector3.zero;
        cameraMove.ResetTargetRotation();
>>>>>>> 5f7eea4... Initial commit
    }
}
