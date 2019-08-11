using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProgress : MonoBehaviour
{
    private Checkpoint currentCheckpoint;

    void Update()
    {
        if(gameObject.transform.position.y < -50) 
        {
            Respawn();
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

    //Returns whether or not the checkpoint hit is farther than the current
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
            else
            {
                Respawn();
            }
        }
    }

    void Respawn()
    {
        Vector3 newPos = GetCurrentCheckpointPosition();
        newPos.y += 0.2f;
        transform.position = newPos;
    }

    public Vector3 GetCurrentCheckpointPosition()
    {
        return currentCheckpoint.gameObject.transform.position;
    }
}
