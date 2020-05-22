#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

public class DeletePlayerPrefs
{    
    [MenuItem(itemName: "Tools/OneLeif/Delete PlayerPrefs")]
    public void DeletePrefs()
    {
        PlayerPrefs.DeleteAll();
    }
}
#endif