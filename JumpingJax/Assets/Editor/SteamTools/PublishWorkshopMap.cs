using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class PublishWorkshopMap : EditorWindow
{
    [MenuItem("Tools/Publish Map")]
    private static void PackageMap()
    {
        EditorWindow.GetWindow(typeof(PublishWorkshopMap));
    }

    private void OnGUI()
    {
        GUILayout.Label("Map Name: ");
        string mapName = GUILayout.TextField("");
        
        GUILayout.Label("Author: ");
        string author = GUILayout.TextField("");

        GUILayout.Label("Version: ");
        string version = GUILayout.TextField("");

        GUILayout.Label("Description: ");
        string description = GUILayout.TextArea("");

        GUILayout.BeginHorizontal();
        GUILayout.Label("No File Selected! ");
        GUILayout.Button("Select a file");
        GUILayout.EndHorizontal();

        GUILayout.Button("Compile");
    }

    private async void UploadMap()
    {
        var result = await Steamworks.Ugc.Editor.NewCommunityFile
                    .WithTitle("My New Item")
                    .WithDescription("Map1")
                    .WithTag("Map")
                    .WithContent(Application.streamingAssetsPath + "/maps")
                    .SubmitAsync();
    }
}
