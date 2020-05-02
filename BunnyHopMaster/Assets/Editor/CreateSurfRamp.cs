using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SurfRampMeshGen))]
public class CreateSurfRamp : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        //SceneView sceneView = SceneView.sceneViews[0] as SceneView;
        //sceneView.in2DMode = true;

        SurfRampMeshGen myScript = (SurfRampMeshGen)target;

        if (GUILayout.Button("Add ramp point"))
        {
            myScript.AddRampPoint();
        }

        if (GUILayout.Button("Build Object"))
        {
            myScript.BuildObject();
        }
    }
}
