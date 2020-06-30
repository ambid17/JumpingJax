using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        AssetBundle bundle = AssetBundle.LoadFromFile("Assets/StreamingAssets/Maps/test.1");
        string[] x = bundle.GetAllScenePaths();
        SceneManager.LoadScene(x[0]);
        Debug.Log("X");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
