using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class PageNavTools : UIPageWidget<PageNavTools>
{
    #region auto generated members
    private GameObject m_goLeft;
    private Transform m_tfTrackHeaderRoot;
    private Toggle m_toggleMotions;
    private Toggle m_toggleParam;
    private GameObject m_goRight;
    private Transform m_itemPageTools_Motions;
    private Transform m_itemPageTools_Param;
    #endregion

    #region auto generated binders
    protected override void CodeGenBindMembers()
    {
        m_goLeft = transform.Find("m_goLeft").gameObject;
        m_tfTrackHeaderRoot = transform.Find("m_goLeft/m_tfTrackHeaderRoot").GetComponent<Transform>();
        m_toggleMotions = transform.Find("m_goLeft/m_tfTrackHeaderRoot/m_toggleMotions").GetComponent<Toggle>();
        m_toggleParam = transform.Find("m_goLeft/m_tfTrackHeaderRoot/m_toggleParam").GetComponent<Toggle>();
        m_goRight = transform.Find("m_goRight").gameObject;
        m_itemPageTools_Motions = transform.Find("m_goRight/m_itemPageTools_Motions").GetComponent<Transform>();
        m_itemPageTools_Param = transform.Find("m_goRight/m_itemPageTools_Param").GetComponent<Transform>();

        m_toggleMotions.onValueChanged.AddListener(OnToggleMotionsChange);
        m_toggleParam.onValueChanged.AddListener(OnToggleParamChange);
    }
    #endregion

    #region auto generated events
    private void OnToggleMotionsChange(bool value)
    {
        Debug.Log("OnToggleMotionsChange");
    }
    private void OnToggleParamChange(bool value)
    {
        Debug.Log("OnToggleParamChange");
    }
    #endregion

    private PageNavTools_Motions m_pageNavTools_Motions;
    private PageNavTools_Param m_pageNavTools_Param;

    protected override void OnInit()
    {
        base.OnInit();
        m_pageNavTools_Motions = PageNavTools_Motions.CreateWidget(m_itemPageTools_Motions.gameObject);
        m_pageNavTools_Param = PageNavTools_Param.CreateWidget(m_itemPageTools_Param.gameObject);

        m_pageNavTools_Motions.BindToToggle(m_toggleMotions);
        m_pageNavTools_Param.BindToToggle(m_toggleParam);
    }
}

public class PageNavTools_Param : UIPageWidget<PageNavTools_Param>
{
    #region auto generated members
    private InputField m_iptPath;
    private InputField m_iptName;
    private Button m_btnSetInputParam;
    private Text m_lblInputParamName;
    private InputField m_iptValue;
    private Toggle m_toggleIncludeChildren;
    private Button m_btnSetParam;
    private Button m_btnRemoveParam;
    #endregion

    #region auto generated binders
    protected override void CodeGenBindMembers()
    {
        m_iptPath = transform.Find("LabelValueH/Value/InputField/m_iptPath").GetComponent<InputField>();
        m_iptName = transform.Find("LabelValueH (1)/Value/InputField/m_iptName").GetComponent<InputField>();
        m_btnSetInputParam = transform.Find("LabelValueH (1)/m_btnSetInputParam").GetComponent<Button>();
        m_lblInputParamName = transform.Find("LabelValueH (1)/m_btnSetInputParam/m_lblInputParamName").GetComponent<Text>();
        m_iptValue = transform.Find("LabelValueH (2)/Value/InputField/m_iptValue").GetComponent<InputField>();
        m_toggleIncludeChildren = transform.Find("LabelValueH (2)/m_toggleIncludeChildren").GetComponent<Toggle>();
        m_btnSetParam = transform.Find("LabelValueH (2)/m_btnSetParam").GetComponent<Button>();
        m_btnRemoveParam = transform.Find("LabelValueH (2)/m_btnRemoveParam").GetComponent<Button>();

        m_iptPath.onValueChanged.AddListener(OnInputFieldPathChange);
        m_iptPath.onEndEdit.AddListener(OnInputFieldPathEndEdit);
        m_iptName.onValueChanged.AddListener(OnInputFieldNameChange);
        m_iptName.onEndEdit.AddListener(OnInputFieldNameEndEdit);
        m_btnSetInputParam.onClick.AddListener(OnButtonSetInputParamClick);
        m_iptValue.onValueChanged.AddListener(OnInputFieldValueChange);
        m_iptValue.onEndEdit.AddListener(OnInputFieldValueEndEdit);
        m_toggleIncludeChildren.onValueChanged.AddListener(OnToggleIncludeChildrenChange);
        m_btnSetParam.onClick.AddListener(OnButtonSetParamClick);
        m_btnRemoveParam.onClick.AddListener(OnButtonRemoveParamClick);
    }
    #endregion



    #region auto generated events
    private void OnInputFieldPathChange(string value)
    {
    }
    private void OnInputFieldPathEndEdit(string value)
    {
    }
    private void OnInputFieldNameChange(string value)
    {
    }
    private void OnInputFieldNameEndEdit(string value)
    {
    }
    private void OnButtonSetInputParamClick()
    {
        m_iptName.text = m_lblInputParamName.text;
    }
    private void OnInputFieldValueChange(string value)
    {
    }
    private void OnInputFieldValueEndEdit(string value)
    {
    }
    private void OnToggleIncludeChildrenChange(bool value)
    {
    }
    private void OnButtonSetParamClick()
    {
        if (!CheckMtnFolderValid(FolderPath))
        {
            MessageTipWindow.Instance.Show("提示", "请输入正确的路径");
            return;
        }
        SetMtnFilesValue(GetMtnFiles(FolderPath), ParamName, float.Parse(ParamValue));
        MessageTipWindow.Instance.Show("提示", "设置成功");
    }
    private void OnButtonRemoveParamClick()
    {
        if (!CheckMtnFolderValid(FolderPath))
        {
            MessageTipWindow.Instance.Show("提示", "请输入正确的路径");
            return;
        }
        RemoveMtnFilesValue(GetMtnFiles(FolderPath), ParamName);
        MessageTipWindow.Instance.Show("提示", "删除成功");
    }
    #endregion

    private string FolderPath => m_iptPath.text;
    private string ParamName => m_iptName.text;
    private string ParamValue => m_iptValue.text;
    private bool IncludeChildren => m_toggleIncludeChildren.isOn;

    private bool CheckMtnFolderValid(string path)
    {
        if (string.IsNullOrEmpty(path))
        {
            return false;
        }
        if (!Directory.Exists(path))
        {
            return false;
        }
        return true;
    }

    private string[] GetMtnFiles(string path)
    {
        if (!CheckMtnFolderValid(path))
        {
            return new string[0];
        }
        if (IncludeChildren)
            return Directory.GetFiles(path, "*.mtn", SearchOption.AllDirectories);
        else
            return Directory.GetFiles(path, "*.mtn");
    }

    private void WriteMtnFile(string filePath, List<string> lines)
    {
        File.WriteAllText(filePath, string.Join(System.Environment.NewLine + System.Environment.NewLine, lines.Where(line => !string.IsNullOrEmpty(line))));
    }

    private void SetMtnFileValue(string filePath, string param, string value)
    {
        var file = File.ReadAllText(filePath);
        var lines = file.Replace("\r", "").Split('\n').ToList();
        bool found = false;
        for (int i = 0; i < lines.Count; i++)
        {
            if (string.IsNullOrEmpty(lines[i]))
                continue;

            if (lines[i].StartsWith("#"))
                continue;

            var parts = lines[i].Split('=');
            if (parts.Length == 2 && parts[0] == param)
            {
                lines[i] = param + "=" + value;
                found = true;
            }
        }
        if (!found)
        {
            lines.Add("\n" + param + "=" + value);
        }
        WriteMtnFile(filePath, lines);
    }

    private void RemoveMtnFileValue(string filePath, string param)
    {
        var file = File.ReadAllText(filePath);
        var lines = file.Replace("\r", "").Split('\n').ToList();
        for (int i = 0; i < lines.Count; i++)
        {
            if (string.IsNullOrEmpty(lines[i]))
                continue;

            if (lines[i].StartsWith("#"))
                continue;

            var parts = lines[i].Split('=');
            if (parts.Length == 2 && parts[0] == param)
            {
                if (i + 1 < lines.Count && string.IsNullOrWhiteSpace(lines[i + 1]))
                {
                    lines.RemoveAt(i + 1);
                }
                lines.RemoveAt(i);
                break;
            }
        }
        WriteMtnFile(filePath, lines);
    }

    private void SetMtnFilesValue(string[] files, string param, float value)
    {
        foreach (var file in files)
        {
            SetMtnFileValue(file, param, value.ToString());
        }
    }

    private void RemoveMtnFilesValue(string[] files, string param)
    {
        foreach (var file in files)
        {
            RemoveMtnFileValue(file, param);
        }
    }
}

public class PageNavTools_Motions : UIPageWidget<PageNavTools_Motions>
{
    #region auto generated members
    private InputField m_iptJsonPath;
    private InputField m_iptFolder;
    private InputField m_iptPrefix;
    private Toggle m_toggleIncludeChildren;
    private Button m_btnAddMotion;
    private Button m_btnAddExp;
    private Button m_btnAddAll;
    private Button m_btnSelectAddAll;
    private Button m_btnRemoveMotion;
    private Button m_btnRemoveExp;
    private Button m_btnRemoveAll;
    #endregion

    #region auto generated binders
    protected override void CodeGenBindMembers()
    {
        m_iptJsonPath = transform.Find("LabelValueH/Value/InputField/m_iptJsonPath").GetComponent<InputField>();
        m_iptFolder = transform.Find("LabelValueH (1)/Value/InputField/m_iptFolder").GetComponent<InputField>();
        m_iptPrefix = transform.Find("LabelValueH (2)/Value/InputField/m_iptPrefix").GetComponent<InputField>();
        m_toggleIncludeChildren = transform.Find("GameObject/m_toggleIncludeChildren").GetComponent<Toggle>();
        m_btnAddMotion = transform.Find("GameObject/m_btnAddMotion").GetComponent<Button>();
        m_btnAddExp = transform.Find("GameObject/m_btnAddExp").GetComponent<Button>();
        m_btnAddAll = transform.Find("GameObject/m_btnAddAll").GetComponent<Button>();
        m_btnSelectAddAll = transform.Find("GameObject (2)/m_btnSelectAddAll").GetComponent<Button>();
        m_btnRemoveMotion = transform.Find("GameObject (1)/m_btnRemoveMotion").GetComponent<Button>();
        m_btnRemoveExp = transform.Find("GameObject (1)/m_btnRemoveExp").GetComponent<Button>();
        m_btnRemoveAll = transform.Find("GameObject (1)/m_btnRemoveAll").GetComponent<Button>();

        m_iptJsonPath.onValueChanged.AddListener(OnInputFieldJsonPathChange);
        m_iptJsonPath.onEndEdit.AddListener(OnInputFieldJsonPathEndEdit);
        m_iptFolder.onValueChanged.AddListener(OnInputFieldFolderChange);
        m_iptFolder.onEndEdit.AddListener(OnInputFieldFolderEndEdit);
        m_iptPrefix.onValueChanged.AddListener(OnInputFieldPrefixChange);
        m_iptPrefix.onEndEdit.AddListener(OnInputFieldPrefixEndEdit);
        m_toggleIncludeChildren.onValueChanged.AddListener(OnToggleIncludeChildrenChange);
        m_btnAddMotion.onClick.AddListener(OnButtonAddMotionClick);
        m_btnAddExp.onClick.AddListener(OnButtonAddExpClick);
        m_btnAddAll.onClick.AddListener(OnButtonAddAllClick);
        m_btnSelectAddAll.onClick.AddListener(OnButtonSelectAddAllClick);
        m_btnRemoveMotion.onClick.AddListener(OnButtonRemoveMotionClick);
        m_btnRemoveExp.onClick.AddListener(OnButtonRemoveExpClick);
        m_btnRemoveAll.onClick.AddListener(OnButtonRemoveAllClick);
    }
    #endregion

    #region auto generated events
    private void OnInputFieldJsonPathChange(string value)
    {
    }
    private void OnInputFieldJsonPathEndEdit(string value)
    {
    }
    private void OnInputFieldFolderChange(string value)
    {
    }
    private void OnInputFieldFolderEndEdit(string value)
    {
    }
    private void OnInputFieldPrefixChange(string value)
    {
    }
    private void OnInputFieldPrefixEndEdit(string value)
    {
    }
    private void OnToggleIncludeChildrenChange(bool value)
    {
    }
    private void OnButtonAddMotionClick()
    {
        DoExecute(true, false, false);
        MessageTipWindow.Instance.Show("提示", "复制成功");
    }
    private void OnButtonAddExpClick()
    {
        DoExecute(false, true, false);
        MessageTipWindow.Instance.Show("提示", "复制成功");
    }
    private void OnButtonAddAllClick()
    {
        DoExecute(true, true, false);
        MessageTipWindow.Instance.Show("提示", "复制成功");
    }
    private void OnButtonRemoveMotionClick()
    {
        DoExecute(true, false, true);
        MessageTipWindow.Instance.Show("提示", "删除成功");
    }
    private void OnButtonRemoveExpClick()
    {
        DoExecute(false, true, true);
        MessageTipWindow.Instance.Show("提示", "删除成功");
    }
    private void OnButtonRemoveAllClick()
    {
        DoExecute(true, true, true);
        MessageTipWindow.Instance.Show("提示", "删除成功");
    }

    private void OnButtonSelectAddAllClick()
    {
        var files = L2DWUtils.OpenFileDialog("选择动作或表情文件", "motion", "json|mtn", true);
        if (files == null || files.Length == 0)
            return;
        
        DoAdds(files);
        MessageTipWindow.Instance.Show("提示", "添加成功！");
    }
    #endregion

    private string JsonPath => m_iptJsonPath.text;
    private string FolderPath => m_iptFolder.text;
    private string Prefix => m_iptPrefix.text;
    private bool IncludeChildren => m_toggleIncludeChildren.isOn;

    private void DoAdds(string[] files)
    {
        if (!IsJsonPathValid())
        {
            MessageTipWindow.Instance.Show("提示", "请输入正确的Json路径");
            return;
        }

        var namePrefix = Prefix;
        if (!string.IsNullOrWhiteSpace(namePrefix) && !namePrefix.EndsWith("/"))
        {
            namePrefix += "/";
        }

        var jsonObject = GetJsonObject(JsonPath);

        var motions_obj = jsonObject.GetField("motions");
        if (motions_obj == null)
        {
            motions_obj = new JSONObject(JSONObject.Type.OBJECT);
            jsonObject.SetField("motions", motions_obj);
        }
        var exps_obj = jsonObject.GetField("expressions");
        if (exps_obj == null)
        {
            exps_obj = new JSONObject(JSONObject.Type.ARRAY);
            jsonObject.SetField("expressions", exps_obj);
        }

        files = files.Select(file => PathHelper.GetRelativePath(JsonPath, file)).ToArray();
        foreach (var file in files)
        {
            string name = Path.GetFileNameWithoutExtension(file);
            name = name.Split(".")[0];
            string extension = Path.GetExtension(file);
            var finalName = namePrefix + name;
            if (extension == ".mtn")
            {
                motions_obj.SetField(finalName, CreateMotionObj(file));
            }
            else if (extension == ".json" || extension == ".exp")
            {
                exps_obj.Add(CreateExpObj(finalName, file));
            }
        }

        GUIUtility.systemCopyBuffer = jsonObject.ToString(true);
        File.WriteAllText(JsonPath, jsonObject.ToString(true));
    }

    private JSONObject CreateMotionObj(string path)
    {
        var objarr = new JSONObject(JSONObject.Type.ARRAY);
        var obj = new JSONObject(JSONObject.Type.OBJECT);
        obj.SetField("file", JSONObject.StringObject(path));
        objarr.Add(obj);
        return objarr;
    }

    private JSONObject CreateExpObj(string name, string path)
    {
        var obj = new JSONObject(JSONObject.Type.OBJECT);
        obj.SetField("name", JSONObject.StringObject(name));
        obj.SetField("file", JSONObject.StringObject(path));
        return obj;
    }

    private void DoExecute(bool isMotion, bool isExp, bool isRemove)
    {
        if (!IsJsonPathValid())
        {
            MessageTipWindow.Instance.Show("提示", "请输入正确的Json路径");
            return;
        }

        if (!IsFolderValid() && !isRemove)
        {
            MessageTipWindow.Instance.Show("提示", "请输入正确的文件夹路径");
            return;
        }

        var namePrefix = Prefix;
        if (!string.IsNullOrWhiteSpace(namePrefix) && !namePrefix.EndsWith("/"))
        {
            namePrefix += "/";
        }


        var jsonObject = GetJsonObject(JsonPath);
        if (isMotion)
        {
            if (isRemove)
            {
                jsonObject.SetField("motions", new JSONObject(JSONObject.Type.OBJECT));
            }
            else
            {
                var files = GetMotionFiles(FolderPath);
                var relativeFiles = files.Select(file => PathHelper.GetRelativePath(JsonPath, file)).ToList();

                var motions_obj = jsonObject.GetField("motions");
                if (motions_obj == null)
                {
                    motions_obj = new JSONObject(JSONObject.Type.OBJECT);
                    jsonObject.SetField("motions", motions_obj);
                }

                var relativeFolderPath = PathHelper.GetRelativePath(JsonPath, FolderPath);
                foreach (var relativeFile in relativeFiles)
                {
                    var fixedName = GetFixedName(relativeFile.Replace(relativeFolderPath, ""));
                    var motion_arrobj = new JSONObject(JSONObject.Type.ARRAY);
                    var motion_obj = new JSONObject(JSONObject.Type.OBJECT);
                    motion_obj.SetField("file", JSONObject.StringObject(relativeFile));
                    motion_arrobj.Add(motion_obj);
                    motions_obj.SetField(namePrefix + fixedName, motion_arrobj);
                }
            }
        }

        if (isExp)
        {
            if (isRemove)
            {
                jsonObject.SetField("expressions", new JSONObject(JSONObject.Type.ARRAY));
            }
            else
            {
                var files = GetExpFiles(FolderPath);
                var relativeFiles = files.Select(file => PathHelper.GetRelativePath(JsonPath, file)).ToList();

                var exps_obj = jsonObject.GetField("expressions");
                if (exps_obj == null)
                {
                    exps_obj = new JSONObject(JSONObject.Type.ARRAY);
                    jsonObject.SetField("expressions", exps_obj);
                }

                var relativeFolderPath = PathHelper.GetRelativePath(JsonPath, FolderPath);
                foreach (var relativeFile in relativeFiles)
                {
                    var fixedName = GetFixedName(relativeFile.Replace(relativeFolderPath, ""));
                    var exp_obj = new JSONObject(JSONObject.Type.OBJECT);
                    exp_obj["name"] = JSONObject.StringObject(namePrefix + fixedName);
                    exp_obj["file"] = JSONObject.StringObject(relativeFile);
                    exps_obj.Add(exp_obj);
                }
            }
        }

        GUIUtility.systemCopyBuffer = jsonObject.ToString(true);
        File.WriteAllText(JsonPath, jsonObject.ToString(true));
    }

    private bool IsJsonPathValid()
    {
        if (string.IsNullOrEmpty(JsonPath))
        {
            return false;
        }
        return true;
    }

    private bool IsFolderValid()
    {
        if (string.IsNullOrEmpty(FolderPath))
        {
            return false;
        }
        return true;
    }

    private string[] GetMotionFiles(string path)
    {
        if (!IsFolderValid())
        {
            return new string[0];
        }
        if (IncludeChildren)
            return Directory.GetFiles(path, "*.mtn", SearchOption.AllDirectories);
        else
            return Directory.GetFiles(path, "*.mtn");
    }

    private string[] GetExpFiles(string path)
    {
        if (!IsFolderValid())
        {
            return new string[0];
        }
        if (IncludeChildren)
            return Directory.GetFiles(path, "*.json", SearchOption.AllDirectories);
        else
            return Directory.GetFiles(path, "*.json");
    }

    private string GetFixedName(string relativePath)
    {
        // 处理逻辑：
        // 1. A/B/C.json -> A/B/C
        // 2. ../A.json -> __/A

        if (relativePath.StartsWith("/"))
        {
            relativePath = relativePath.Substring(1);
        }

        // 获取不带扩展名的文件名
        var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(relativePath);
        fileNameWithoutExtension = fileNameWithoutExtension.Split(".")[0];

        // 获取目录部分（处理根目录情况）
        var directoryPart = Path.GetDirectoryName(relativePath);
        if (string.IsNullOrEmpty(directoryPart))
        {
            // 如果没有目录部分（同一目录下的文件）
            return fileNameWithoutExtension;
        }

        // 将目录中的".."替换为"__"，并统一路径分隔符为"/"
        var fixedDirectory = directoryPart.Replace("..", "__").Replace(Path.DirectorySeparatorChar, '/');

        // 组合目录和文件名
        var fixedName = $"{fixedDirectory}/{fileNameWithoutExtension}";

        // 处理特殊情况：如果路径以"/"或"./"开头，移除多余的斜杠
        if (fixedName.StartsWith("/"))
        {
            fixedName = fixedName.Substring(1);
        }
        else if (fixedName.StartsWith("./"))
        {
            fixedName = fixedName.Substring(2);
        }

        return fixedName;
    }

    private JSONObject GetJsonObject(string filePath)
    {
        var file = File.ReadAllText(filePath);
        return new JSONObject(file);
    }
}