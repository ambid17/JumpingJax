using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    [SerializeField]
    public int level;
    public Light light;
    private Color completedColor = new Color(0, 0.5f, 1);

    private void Start()
    {
    }

    public void SetCompleted()
    {
        light.color = completedColor;
    }
}