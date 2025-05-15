using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;


public class OpenFolderCom : MonoBehaviour
{
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
