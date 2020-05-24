using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SurfRampMeshGenerator))]
public class CreateSurfRamp : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        SurfRampMeshGenerator myScript = (SurfRampMeshGenerator)target;
        
        if (GUILayout.Button("Build Object"))
        {
            myScript.BuildObject();
        }
    }
}
