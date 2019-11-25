using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    [SerializeField]
    public int level;
    
    public Material completedMaterial;
    public MeshRenderer renderer;

    private void Start()
    {
        renderer = GetComponent<MeshRenderer>();
    }

    public void SetCompleted()
    {
        Debug.Log("Reached checkpoint: " + level);
        renderer.sharedMaterial = completedMaterial;
    }
}
