using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using UnityEngine.Experimental.AssetBundlePatching;
using UnityEditor.VersionControl;

public class CompileWorkshopMap : EditorWindow
{
    [MenuItem("Tools/Compile Map")]
    private static void CompileMap()
    {
        string assetBundleDirectory = Application.streamingAssetsPath + "/Maps";
        if (!Directory.Exists(assetBundleDirectory))
        {
            Directory.CreateDirectory(assetBundleDirectory);
        }
        
        BuildPipeline.BuildAssetBundles(assetBundleDirectory,
                                        BuildAssetBundleOptions.None,
                                        BuildTarget.StandaloneWindows);
    }
}
