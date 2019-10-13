using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerProgress : MonoBehaviour
{
    private Checkpoint currentCheckpoint;
    public PlayerUI playerUI;
    public bool didWin = false;

    private void Start()
    {
        didWin = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            Debug.Log("resp");  
            Respawn();
        }

        if(gameObject.transform.position.y < -50) 
        {
            Respawn();
        }

        if(currentCheckpoint != null && !didWin)
        {
            if (currentCheckpoint.level == LevelData.LD.numberOfCheckpoints)
            {
                didWin = true;
                playerUI.ShowWinScreen();
            }
        }
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Checkpoint checkPointHit = hit.gameObject.GetComponent<Checkpoint>();
        if (checkPointHit)
        {
            HitNewCheckPoint(checkPointHit);
        }
    }

    public void HitNewCheckPoint(Checkpoint checkpoint)
    {
        if (currentCheckpoint == null)
        {
            currentCheckpoint = checkpoint;
        }
        else
        {
            if (currentCheckpoint.level <= checkpoint.level)
            {
                currentCheckpoint = checkpoint;
            }
        }
    }

    void Respawn()
    {
        Vector3 newPos = GetCurrentCheckpointPosition();
        newPos.y += 0.2f;
        GetComponent<CharacterController>().enabled = false;
        transform.position = newPos;
        GetComponent<CharacterController>().enabled = true;
    }

    public Vector3 GetCurrentCheckpointPosition()
    {
        return currentCheckpoint.gameObject.transform.position;
    }
}
