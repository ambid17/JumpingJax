using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    [SerializeField]
    public int level;
    public Light light;
    public Material completedMaterial;
    private Color completedColor = new Color(0, 1,0);

    private Renderer myRenderer;

    private void Start()
    {
        myRenderer = GetComponent<Renderer>();
    }

    public void SetCompleted()
    {
        myRenderer.sharedMaterial = completedMaterial;
        light.color = completedColor;
    }
}