using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;

public class GlobalDebug: MonoBehaviour
{
    public Text debugText;
    public GameObject debugScrollView;

    //Logs to a text box in the UI and console, adds the caller to the statement
    public void Log(string message)
    {
        var callingMethod = new StackTrace().GetFrame(1).GetMethod();
        string callingMethodName = callingMethod.Name;
        string callingClassName = callingMethod.ReflectedType.Name;
        string debugMessage = (">" + callingClassName + "." + callingMethodName + "(): " + message + "\n");

        debugText.text += debugMessage;
        UnityEngine.Debug.Log(debugMessage);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            debugScrollView.SetActive(!debugScrollView.activeInHierarchy);
        }
    }
}
