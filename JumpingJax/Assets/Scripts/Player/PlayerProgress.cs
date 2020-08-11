using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProgress : MonoBehaviour
{
    [SerializeField]
    public Checkpoint currentCheckpoint;

    public PlayerUI playerUI;

    public PlayerMovement playerMovement;
    public CameraMove cameraMove;
    public PlayerGhostRun playerGhostRun;

    private PortalPair portals;


    private void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
        cameraMove = GetComponent<CameraMove>();
        playerGhostRun = GetComponent<PlayerGhostRun>();
        if (GameManager.GetCurrentLevel().isPortalLevel)
        {
            portals = GameObject.FindGameObjectWithTag("Portal").GetComponent<PortalPair>();
        }
    }

    private void Update()
    {
        if (Time.timeScale == 0)
        {
            return;
        }

        if (InputManager.GetKeyDown(PlayerConstants.ResetLevel))
        {
            Respawn();
        }

        if (currentCheckpoint != null && !GameManager.Instance.didWinCurrentLevel)
        {
            if (currentCheckpoint.level == GameManager.GetCurrentLevel().numberOfCheckpoints)
            {
                playerGhostRun.SaveCurrentRunData();
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
        Vector3 respawnPosition = currentCheckpoint.transform.position + PlayerConstants.PlayerSpawnOffset;
        transform.position = respawnPosition;
        transform.rotation = currentCheckpoint.transform.rotation;

        playerMovement.newVelocity = Vector3.zero;

        cameraMove.ResetTargetRotation();

        // If the player is restarting at the beginning, reset timer
        if (currentCheckpoint.level == 1)
        {
            if (GameManager.GetCurrentLevel().isPortalLevel)
            {
                portals.Portals[0].ResetPortal();
                portals.Portals[1].ResetPortal();
            }
            GameManager.Instance.currentCompletionTime = 0;
            playerGhostRun.RestartRun();
        }
    }
}