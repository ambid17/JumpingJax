using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SurfRampEasyMeshGen))]
public class CreateEasySurfRamp : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        SurfRampEasyMeshGen myScript = (SurfRampEasyMeshGen)target;
        
        if (GUILayout.Button("Build Object"))
        {
            myScript.BuildObject();
        }
    }
}
