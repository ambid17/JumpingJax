using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectibleHandler : MonoBehaviour
{
    public int collectibleNumber;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == PlayerConstants.PlayerLayer)
        {
            Level currentLevel = GameManager.GetCurrentLevel();
            currentLevel.collectibles[collectibleNumber].isCollected = true;
            Destroy(gameObject);
        }
    }
}
