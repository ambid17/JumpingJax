using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;

public enum LogLevel
{
    Debug, Error
}

public class GlobalDebug
{
    public Text debugText;
    public GameObject debugScrollView;
    public static LogLevel logLevel = LogLevel.Debug;

    //Logs to a text box in the UI and console, adds the caller to the statement
    public static void Error(string message)
    {
        var callingMethod = new StackTrace().GetFrame(1).GetMethod();
        string callingMethodName = callingMethod.Name;
        string callingClassName = callingMethod.ReflectedType.Name;
        string debugMessage = (">" + callingClassName + "." + callingMethodName + "(): " + message + "\n");

        //debugText.text += debugMessage;
        UnityEngine.Debug.Log(debugMessage);
    }

    public static void Debug(string message)
    {
        if (LogLevel.Debug == logLevel)
        {
            var callingMethod = new StackTrace().GetFrame(1).GetMethod();
            string callingMethodName = callingMethod.Name;
            string callingClassName = callingMethod.ReflectedType.Name;
            string debugMessage = (">" + callingClassName + "." + callingMethodName + "(): " + message + "\n");

            //debugText.text += debugMessage;
            UnityEngine.Debug.Log(debugMessage);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(PlayerConstants.DebugMenu))
        {
            debugScrollView.SetActive(!debugScrollView.activeInHierarchy);
        }
    }
}
