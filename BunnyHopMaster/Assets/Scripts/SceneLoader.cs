using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    string assetURL = "https://drive.google.com/uc?export=download&id=1xosLNpo20QuvvwMAVtakH6Ppe0NQvQd2";
    void Start()
    {
        GetRemoteAssetBundle();
        //GetLocalAssetBundle();
    }

    void GetLocalAssetBundle()
    {
        AssetBundle bundle = AssetBundle.LoadFromFile("Assets/StreamingAssets/test");
        string[] x = bundle.GetAllScenePaths();
        SceneManager.LoadScene(x[0]);
    }

    void GetRemoteAssetBundle()
    {
        StartCoroutine(DownloadAssetBundle());
    }

    IEnumerator DownloadAssetBundle()
    {
        WWW www = new WWW(assetURL);
        UnityWebRequest request = UnityWebRequest.Get(assetURL);
        yield return request.SendWebRequest();
        AssetBundle bundle = AssetBundle.LoadFromMemory(request.downloadHandler.data);
        string[] x = bundle.GetAllScenePaths();
        SceneManager.LoadScene(x[0]);
        if (request.isNetworkError || request.isHttpError)
        {
            Debug.LogError(request.error);
        }
        else
        {
            Debug.Log("File successfully downloaded and saved");
        }
    }
}
