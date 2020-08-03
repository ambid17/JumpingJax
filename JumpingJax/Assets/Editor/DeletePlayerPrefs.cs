using UnityEditor;
using UnityEngine;

public class DeletePlayerPrefs
{    
    [MenuItem(itemName: "Tools/OneLeif/Delete PlayerPrefs")]
    private static void DeletePrefs()
    {
        PlayerPrefs.DeleteAll();
    }
}
