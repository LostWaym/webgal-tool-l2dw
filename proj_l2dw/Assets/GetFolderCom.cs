using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class GetFolderCom : MonoBehaviour
{
    public InputField pathSource;
    public string hintText;
    public string key;
    public bool isFile = false;
    public string extension = "json";
    void Awake()
    {
        GetComponent<Button>().onClick.AddListener(OnClick);
    }

    void OnClick()
    {
        string pathKey = string.IsNullOrEmpty(key) ? "last_folder" : key;
        string title = string.IsNullOrEmpty(hintText) ? "选择" : hintText;

        string[] paths;
        if (isFile)
        {
            paths = L2DWUtils.OpenFileDialog(title, pathKey, extension);
        }
        else
        {
            paths = L2DWUtils.OpenFolderDialog(title, pathKey);
        }
        if (paths == null || paths.Length == 0)
            return;

        pathSource.text = paths[0];
        pathSource.onEndEdit.Invoke(paths[0]);
        if (isFile)
        {
            PlayerPrefs.SetString(pathKey, Path.GetDirectoryName(paths[0]));
        }
        else
        {
            PlayerPrefs.SetString(pathKey, paths[0]);
        }
    }
    
}
