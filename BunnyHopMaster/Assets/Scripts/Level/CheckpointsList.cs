using System;
using UnityEngine;

public class CheckpointsList : MonoBehaviour
{
    [SerializeField]
    Level scriptableObjectLevel = null;

    Checkpoint[] checkpointsInScene;
    int[] checkpointNumbersToCompare;

    // Start is called before the first frame update
    void Start()
    {
        checkpointsInScene = GetComponentsInChildren<Checkpoint>();
        CheckNumberOfCheckpoints();
        Array.Sort(checkpointsInScene, new CheckpointComparer());
        FillArrayBasedOnCheckpointsLengthAndCompare();
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void CheckNumberOfCheckpoints()
    {
        if (scriptableObjectLevel.numberOfCheckpoints != checkpointsInScene.Length)
        {
            Debug.LogError("Number of checkpoints in scene does not equal number of checkpoints in SO");
        }
    }

    private void FillArrayBasedOnCheckpointsLengthAndCompare()
    {
        checkpointNumbersToCompare = new int[checkpointsInScene.Length];
        int numberToFill = 1;
        for (int i = 0; i < checkpointsInScene.Length; i++)
        {
            checkpointNumbersToCompare[i] = numberToFill;
            if (checkpointNumbersToCompare[i] != checkpointsInScene[i].level)
            {
                Debug.LogError("Wrong property level set in checkpoint");
            }
            numberToFill++;
        }
    }
}