using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using UnityEngine.Experimental.AssetBundlePatching;
using UnityEditor.VersionControl;
using UnityEngine.SceneManagement;

public class CompileWorkshopMap : MonoBehaviour
{
    [MenuItem("Tools/Compile Map")]
    static void CompileMap()
    {
        AssetBundleBuild[] buildMap = new AssetBundleBuild[1];
        buildMap[0].assetBundleName = "mapBundle";
        buildMap[0].assetNames = new string[] { "Assets/Scenes/BunnyHop1.unity" };


        AssetBundleManifest x = BuildPipeline.BuildAssetBundles("Assets/StreamingAssets",
                                        buildMap,
                                        BuildAssetBundleOptions.None,
                                        BuildTarget.StandaloneWindows);
    }
}
