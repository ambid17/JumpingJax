using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    [SerializeField]
    public int level;

    public Material completedMaterial;
    public MeshRenderer myRenderer;

    private void Start()
    {
        myRenderer = GetComponent<MeshRenderer>();
    }

    public void SetCompleted()
    {
        myRenderer.sharedMaterial = completedMaterial;
        transform.GetChild(0).gameObject.SetActive(false); // turn off the light when you hit a checkpoint
    }
}