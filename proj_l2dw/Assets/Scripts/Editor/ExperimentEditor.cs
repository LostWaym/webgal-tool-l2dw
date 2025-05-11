

using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;


public class ExperimentCommand : EditorWindow
{
    [MenuItem("Experiment/Parse")]
    public static void ShowWindow()
    {
        GetWindow<ExperimentCommand>("Experiment");
    }
    
    private string command;
    private void OnGUI()
    {
        GUILayout.BeginHorizontal();
        command = GUILayout.TextField(command);
        if (GUILayout.Button("Parse"))
        {
            Parse();
        }
        GUILayout.EndHorizontal();
    }

    private void Parse()
    {
        var commandInfo = Experiment.Parse(command);
        Debug.Log(commandInfo.command);
        Debug.Log(commandInfo.commandParam);
        foreach (var parameter in commandInfo.otherParameters)
        {
            Debug.Log(parameter.Key + "=" + parameter.Value);
        }

        var transformObject = Experiment.ParseJson(commandInfo.GetParameter("transform"));
        if (transformObject != null)
        {
            var positionObject = transformObject["position"];
            var x = positionObject["x"];
            var y = positionObject["y"];
            var z = positionObject["z"];
            var alpha = transformObject["alpha"];

            Debug.Log("x=" + x);
            Debug.Log("y=" + y);
            Debug.Log("z=" + z);
            Debug.Log("alpha=" + alpha);

            Debug.Log(transformObject.ToString(false));
        }
    }
}

public class TestMotionParser : EditorWindow
{
    [MenuItem("Test/MotionParser")]
    public static void ShowWindow()
    {
        GetWindow<TestMotionParser>("TestMotionParser");
    }

    private string motionPath;
    private void OnGUI()
    {
        GUILayout.BeginHorizontal();
        motionPath = GUILayout.TextField(motionPath);
        if (GUILayout.Button("Parse"))
        {
            Parse();
        }
        GUILayout.EndHorizontal();
    }

    private void Parse()
    {
        var motionData = new Live2dMotionInfo();
        var text = File.ReadAllText(motionPath);
        motionData.Parse(text);
        Debug.Log(motionData.Print());
        GUIUtility.systemCopyBuffer = motionData.Print();
    }
}