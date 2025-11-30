using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;


public class OpenFolderCom : MonoBehaviour
{
    public GameObject msgHandler;
    public string data;
    public InputField pathSource;
    public bool isFile = false;

    public bool isModelPath = false;

    void Awake()
    {
        GetComponent<Button>().onClick.AddListener(OnClick);
    }

    void OnClick()
    {
        var path = pathSource.text;
        if (string.IsNullOrEmpty(path))
            return;

        if (isFile && isModelPath)
        {
            path = L2DWUtils.TryParseModelAbsolutePath(path);
        }

        if (msgHandler != null && msgHandler.TryGetComponent<IOpenFolderMsgHandler>(out var msgHandlerComponent))
        {
            msgHandlerComponent.Handle(this, path, pathSource.text, out var newPath);
            if (newPath != null)
            {
                path = newPath;
            }
        }

        string dir = Path.GetDirectoryName(path);
        if (string.IsNullOrEmpty(dir))
            return;
            
        if (!Directory.Exists(dir))
            return;

        if (isFile)
            System.Diagnostics.Process.Start("explorer.exe", "/select," + path);
        else
            System.Diagnostics.Process.Start(dir);
    }
}

public interface IOpenFolderMsgHandler
{
    public void Handle(OpenFolderCom com, string path, string originalPath, out string newPath);
}