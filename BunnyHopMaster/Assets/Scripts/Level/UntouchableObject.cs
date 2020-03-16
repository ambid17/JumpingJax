using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UntouchableObject : MonoBehaviour
{
    private void Start()
    {
        Collider myCollider = GetComponent<Collider>();
        myCollider.isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        PlayerProgress playerProgress = other.GetComponent<PlayerProgress>();
        if(playerProgress != null) {
            playerProgress.Respawn();
        }
    }
}
