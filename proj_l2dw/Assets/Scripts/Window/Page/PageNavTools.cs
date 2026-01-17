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
    private Toggle m_toggleExpParam;
    private GameObject m_goRight;
    private Transform m_itemPageTools_Motions;
    private Transform m_itemPageTools_Param;
    private Transform m_itemPageTools_ExpParam;
    #endregion

    #region auto generated binders
    protected override void CodeGenBindMembers()
    {
        m_goLeft = transform.Find("m_goLeft").gameObject;
        m_tfTrackHeaderRoot = transform.Find("m_goLeft/m_tfTrackHeaderRoot").GetComponent<Transform>();
        m_toggleMotions = transform.Find("m_goLeft/m_tfTrackHeaderRoot/m_toggleMotions").GetComponent<Toggle>();
        m_toggleParam = transform.Find("m_goLeft/m_tfTrackHeaderRoot/m_toggleParam").GetComponent<Toggle>();
        m_toggleExpParam = transform.Find("m_goLeft/m_tfTrackHeaderRoot/m_toggleExpParam").GetComponent<Toggle>();
        m_goRight = transform.Find("m_goRight").gameObject;
        m_itemPageTools_Motions = transform.Find("m_goRight/m_itemPageTools_Motions").GetComponent<Transform>();
        m_itemPageTools_Param = transform.Find("m_goRight/m_itemPageTools_Param").GetComponent<Transform>();
        m_itemPageTools_ExpParam = transform.Find("m_goRight/m_itemPageTools_ExpParam").GetComponent<Transform>();

        m_toggleMotions.onValueChanged.AddListener(OnToggleMotionsChange);
        m_toggleParam.onValueChanged.AddListener(OnToggleParamChange);
        m_toggleExpParam.onValueChanged.AddListener(OnToggleExpParamChange);
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
    private void OnToggleExpParamChange(bool value)
    {
        Debug.Log("OnToggleExpParamChange");
    }
    #endregion

    private PageNavTools_Motions m_pageNavTools_Motions;
    private PageNavTools_Param m_pageNavTools_Param;
    private PageNavTools_ExpParam m_pageNavTools_ExpParam;
    
    protected override void OnInit()
    {
        base.OnInit();
        m_pageNavTools_Motions = PageNavTools_Motions.CreateWidget(m_itemPageTools_Motions.gameObject);
        m_pageNavTools_Param = PageNavTools_Param.CreateWidget(m_itemPageTools_Param.gameObject);
        m_pageNavTools_ExpParam = PageNavTools_ExpParam.CreateWidget(m_itemPageTools_ExpParam.gameObject);

        m_pageNavTools_Motions.BindToToggle(m_toggleMotions);
        m_pageNavTools_Param.BindToToggle(m_toggleParam);
        m_pageNavTools_ExpParam.BindToToggle(m_toggleExpParam);

        BindChild(m_pageNavTools_Motions);
        BindChild(m_pageNavTools_Param);
        BindChild(m_pageNavTools_ExpParam);
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
        m_iptPath = transform.Find("Controls/LabelValueH/Value/InputField/m_iptPath").GetComponent<InputField>();
        m_iptName = transform.Find("Controls/LabelValueH (1)/Value/InputField/m_iptName").GetComponent<InputField>();
        m_btnSetInputParam = transform.Find("Controls/LabelValueH (1)/m_btnSetInputParam").GetComponent<Button>();
        m_lblInputParamName = transform.Find("Controls/LabelValueH (1)/m_btnSetInputParam/m_lblInputParamName").GetComponent<Text>();
        m_iptValue = transform.Find("Controls/LabelValueH (2)/Value/InputField/m_iptValue").GetComponent<InputField>();
        m_toggleIncludeChildren = transform.Find("Controls/LabelValueH (2)/m_toggleIncludeChildren").GetComponent<Toggle>();
        m_btnSetParam = transform.Find("Controls/LabelValueH (2)/m_btnSetParam").GetComponent<Button>();
        m_btnRemoveParam = transform.Find("Controls/LabelValueH (2)/m_btnRemoveParam").GetComponent<Button>();

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

public class PageNavTools_ExpParam : UIPageWidget<PageNavTools_ExpParam>
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
    private Toggle m_toggleSetCalc;
    private Toggle m_toggleCalcAdd;
    private Toggle m_toggleCalcMult;
    private Toggle m_toggleCalcSet;
    private Button m_btnApplySingle;
    private Button m_btnApplyAll;
    #endregion

    #region auto generated binders
    protected override void CodeGenBindMembers()
    {
        m_iptPath = transform.Find("Controls/LabelValueH/Value/InputField/m_iptPath").GetComponent<InputField>();
        m_iptName = transform.Find("Controls/LabelValueH (1)/Value/InputField/m_iptName").GetComponent<InputField>();
        m_btnSetInputParam = transform.Find("Controls/LabelValueH (1)/m_btnSetInputParam").GetComponent<Button>();
        m_lblInputParamName = transform.Find("Controls/LabelValueH (1)/m_btnSetInputParam/m_lblInputParamName").GetComponent<Text>();
        m_iptValue = transform.Find("Controls/LabelValueH (2)/Value/InputField/m_iptValue").GetComponent<InputField>();
        m_toggleIncludeChildren = transform.Find("Controls/LabelValueH (2)/m_toggleIncludeChildren").GetComponent<Toggle>();
        m_btnSetParam = transform.Find("Controls/LabelValueH (2)/m_btnSetParam").GetComponent<Button>();
        m_btnRemoveParam = transform.Find("Controls/LabelValueH (2)/m_btnRemoveParam").GetComponent<Button>();
        m_toggleSetCalc = transform.Find("Controls/GameObject/m_toggleSetCalc").GetComponent<Toggle>();
        m_toggleCalcAdd = transform.Find("Controls/GameObject/m_toggleCalcAdd").GetComponent<Toggle>();
        m_toggleCalcMult = transform.Find("Controls/GameObject/m_toggleCalcMult").GetComponent<Toggle>();
        m_toggleCalcSet = transform.Find("Controls/GameObject/m_toggleCalcSet").GetComponent<Toggle>();
        m_btnApplySingle = transform.Find("Controls/GameObject (1)/m_btnApplySingle").GetComponent<Button>();
        m_btnApplyAll = transform.Find("Controls/GameObject (1)/m_btnApplyAll").GetComponent<Button>();

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
        m_toggleSetCalc.onValueChanged.AddListener(OnToggleSetCalcChange);
        m_toggleCalcAdd.onValueChanged.AddListener(OnToggleCalcAddChange);
        m_toggleCalcMult.onValueChanged.AddListener(OnToggleCalcMultChange);
        m_toggleCalcSet.onValueChanged.AddListener(OnToggleCalcSetChange);
        m_btnApplySingle.onClick.AddListener(OnButtonApplySingleClick);
        m_btnApplyAll.onClick.AddListener(OnButtonApplyAllClick);
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
        if (!CheckFolderValid(FolderPath))
        {
            MessageTipWindow.Instance.Show("提示", "请输入正确的路径");
            return;
        }
        SetFilesValue(GetFiles(FolderPath), ParamName, float.Parse(ParamValue));
        MessageTipWindow.Instance.Show("提示", "设置成功");
    }
    private void OnButtonRemoveParamClick()
    {
        if (!CheckFolderValid(FolderPath))
        {
            MessageTipWindow.Instance.Show("提示", "请输入正确的路径");
            return;
        }
        RemoveFilesValue(GetFiles(FolderPath), ParamName);
        MessageTipWindow.Instance.Show("提示", "删除成功");
    }

    private void OnToggleSetCalcChange(bool value)
    {
        Debug.Log("OnToggleSetMultChange");
    }
    private void OnToggleCalcAddChange(bool value)
    {
        Debug.Log("OnToggleCalcAddChange");
    }
    private void OnToggleCalcMultChange(bool value)
    {
        Debug.Log("OnToggleCalcMultChange");
    }
    private void OnToggleCalcSetChange(bool value)
    {
        Debug.Log("OnToggleCalcSetChange");
    }
    private void OnButtonApplySingleClick()
    {
        Debug.Log("OnButtonApplySingleClick");
        var calc = GetCalc();
        if (calc == null)
        {
            MessageTipWindow.Instance.Show("提示", "请选择计算类型");
            return;
        }
        ApplyFilesSingleParamCalc(GetFiles(FolderPath), ParamName, calc);
        MessageTipWindow.Instance.Show("提示", "应用成功");
    }
    private void OnButtonApplyAllClick()
    {
        Debug.Log("OnButtonApplyAllClick");
        var calc = GetCalc();
        if (calc == null)
        {
            MessageTipWindow.Instance.Show("提示", "请选择计算类型");
            return;
        }
        ApplyFilesCalc(GetFiles(FolderPath), calc);
        MessageTipWindow.Instance.Show("提示", "应用成功");
    }

    #endregion

    private string FolderPath => m_iptPath.text;
    private string ParamName => m_iptName.text;
    private string ParamValue => m_iptValue.text;
    private bool IncludeChildren => m_toggleIncludeChildren.isOn;

    private bool CheckFolderValid(string path)
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

    private const string EXTENSION = "*.json";
    private string[] GetFiles(string path)
    {
        if (!CheckFolderValid(path))
        {
            return new string[0];
        }
        if (IncludeChildren)
            return Directory.GetFiles(path, EXTENSION, SearchOption.AllDirectories);
        else
            return Directory.GetFiles(path, EXTENSION);
    }

    private void WriteFile(string filePath, JSONObject jsonObj)
    {
        File.WriteAllText(filePath, jsonObj.ToString(true));
    }

    private string GetCalc()
    {
        if (m_toggleCalcAdd.isOn)
            return MygoExp.CALC_TYPE_ADD;
        if (m_toggleCalcMult.isOn)
            return MygoExp.CALC_TYPE_MULT;
        if (m_toggleCalcSet.isOn)
            return MygoExp.CALC_TYPE_SET;
        return null;
    }

    private void TrySetCalc(JSONObject paramObj)
    {
        if (m_toggleSetCalc.isOn)
        {
            var calc = GetCalc();
            if (calc != null)
                paramObj.SetField("calc", JSONObject.StringObject(calc));
        }
    }

    private void SetFileValue(string filePath, string param, float value)
    {
        var jsonObj = GetJsonObject(filePath);
        var paramsArray = jsonObj.GetField("params");

        if (paramsArray == null)
        {
            paramsArray = new JSONObject(JSONObject.Type.ARRAY);
            jsonObj.SetField("params", paramsArray);
        }
        
        bool found = false;
        for (int i = 0; i < paramsArray.Count; i++)
        {
            var paramObj = paramsArray[i];
            if (paramObj.GetField("id")?.str == param)
            {
                paramObj.SetField("val", value);
                TrySetCalc(paramObj);
                found = true;
                break;
            }
        }

        if (!found)
        {
            var paramObj = new JSONObject(JSONObject.Type.OBJECT);
            paramObj.SetField("id", JSONObject.StringObject(param));
            paramObj.SetField("val", value);
            TrySetCalc(paramObj);
            paramsArray.Add(paramObj);
        }

        WriteFile(filePath, jsonObj);
    }
    private JSONObject GetJsonObject(string filePath)
    {
        var text = File.ReadAllText(filePath);
        return new JSONObject(text);
    }

    private void RemoveFileValue(string filePath, string param)
    {
        var jsonObj = GetJsonObject(filePath);
        var paramsArray = jsonObj.GetField("params");

        if (paramsArray == null)
            return;

        for (int i = 0; i < paramsArray.Count; i++)
        {
            var paramObj = paramsArray[i];
            if (paramObj.GetField("id")?.str == param)
            {
                paramsArray.list.RemoveAt(i);
                break;
            }
        }
        WriteFile(filePath, jsonObj);
    }

    private void ApplyFileCalc(string filePath, string calc)
    {
        var jsonObj = GetJsonObject(filePath);
        var paramsArray = jsonObj.GetField("params");

        if (paramsArray == null)
            return;

        for (int i = 0; i < paramsArray.Count; i++)
        {
            var paramObj = paramsArray[i];
            paramObj.SetField("calc", JSONObject.StringObject(calc));
        }
        WriteFile(filePath, jsonObj);
    }

    private void ApplyFileSingleParamCalc(string filePath, string param, string calc)
    {
        var jsonObj = GetJsonObject(filePath);
        var paramsArray = jsonObj.GetField("params");
        if (paramsArray == null)
            return;
        
        for (int i = 0; i < paramsArray.Count; i++)
        {
            var paramObj = paramsArray[i];
            if (paramObj.GetField("id")?.str == param)
            {
                paramObj.SetField("calc", JSONObject.StringObject(calc));
            }
        }

        WriteFile(filePath, jsonObj);
    }

    private void SetFilesValue(string[] files, string param, float value)
    {
        foreach (var file in files)
        {
            SetFileValue(file, param, value);
        }
    }

    private void RemoveFilesValue(string[] files, string param)
    {
        foreach (var file in files)
        {
            RemoveFileValue(file, param);
        }
    }

    private void ApplyFilesCalc(string[] files, string calc)
    {
        foreach (var file in files)
        {
            ApplyFileCalc(file, calc);
        }
    }

    private void ApplyFilesSingleParamCalc(string[] files, string param, string calc)
    {
        foreach (var file in files)
        {
            ApplyFileSingleParamCalc(file, param, calc);
        }
    }
}

public class PageNavTools_Motions : UIPageWidget<PageNavTools_Motions>
{
    #region auto generated members
    private Toggle m_toggleCurrentModel;
    private Text m_lblCurrentModelName;
    private InputField m_iptJsonPath;
    private InputField m_iptPrefix;
    private Button m_btnMotionEdit;
    private Button m_btnExpressionEdit;
    private Button m_btnSelectAddAll;
    private InputField m_iptFolder;
    private Toggle m_toggleIncludeChildren;
    private Button m_btnAddMotion;
    private Button m_btnAddExp;
    private Button m_btnAddAll;
    private Button m_btnRemoveMotion;
    private Button m_btnRemoveExp;
    private Button m_btnRemoveAll;
    #endregion

    #region auto generated binders
    protected override void CodeGenBindMembers()
    {
        m_toggleCurrentModel = transform.Find("Controls/Sources/m_toggleCurrentModel").GetComponent<Toggle>();
        m_lblCurrentModelName = transform.Find("Controls/CurrentModelContainer/m_lblCurrentModelName").GetComponent<Text>();
        m_iptJsonPath = transform.Find("Controls/SelectModelContainer/json文件路径/Value/InputField/m_iptJsonPath").GetComponent<InputField>();
        m_iptPrefix = transform.Find("Controls/前缀/Value/InputField/m_iptPrefix").GetComponent<InputField>();
        m_btnMotionEdit = transform.Find("Controls/Operations/m_btnMotionEdit").GetComponent<Button>();
        m_btnExpressionEdit = transform.Find("Controls/Operations/m_btnExpressionEdit").GetComponent<Button>();
        m_btnSelectAddAll = transform.Find("Controls/ManualAddContainer/m_btnSelectAddAll").GetComponent<Button>();
        m_iptFolder = transform.Find("Controls/BulkAddContainer/LabelValueH (1)/Value/InputField/m_iptFolder").GetComponent<InputField>();
        m_toggleIncludeChildren = transform.Find("Controls/BulkAddContainer/GameObject/m_toggleIncludeChildren").GetComponent<Toggle>();
        m_btnAddMotion = transform.Find("Controls/BulkAddContainer/GameObject/m_btnAddMotion").GetComponent<Button>();
        m_btnAddExp = transform.Find("Controls/BulkAddContainer/GameObject/m_btnAddExp").GetComponent<Button>();
        m_btnAddAll = transform.Find("Controls/BulkAddContainer/GameObject/m_btnAddAll").GetComponent<Button>();
        m_btnRemoveMotion = transform.Find("Controls/BulkRemoveContainer/GameObject (1)/m_btnRemoveMotion").GetComponent<Button>();
        m_btnRemoveExp = transform.Find("Controls/BulkRemoveContainer/GameObject (1)/m_btnRemoveExp").GetComponent<Button>();
        m_btnRemoveAll = transform.Find("Controls/BulkRemoveContainer/GameObject (1)/m_btnRemoveAll").GetComponent<Button>();

        m_toggleCurrentModel.onValueChanged.AddListener(OnToggleCurrentModelChange);
        m_iptJsonPath.onValueChanged.AddListener(OnInputFieldJsonPathChange);
        m_iptJsonPath.onEndEdit.AddListener(OnInputFieldJsonPathEndEdit);
        m_iptPrefix.onValueChanged.AddListener(OnInputFieldPrefixChange);
        m_iptPrefix.onEndEdit.AddListener(OnInputFieldPrefixEndEdit);
        m_btnMotionEdit.onClick.AddListener(OnButtonMotionEditClick);
        m_btnExpressionEdit.onClick.AddListener(OnButtonExpressionEditClick);
        m_btnSelectAddAll.onClick.AddListener(OnButtonSelectAddAllClick);
        m_iptFolder.onValueChanged.AddListener(OnInputFieldFolderChange);
        m_iptFolder.onEndEdit.AddListener(OnInputFieldFolderEndEdit);
        m_toggleIncludeChildren.onValueChanged.AddListener(OnToggleIncludeChildrenChange);
        m_btnAddMotion.onClick.AddListener(OnButtonAddMotionClick);
        m_btnAddExp.onClick.AddListener(OnButtonAddExpClick);
        m_btnAddAll.onClick.AddListener(OnButtonAddAllClick);
        m_btnRemoveMotion.onClick.AddListener(OnButtonRemoveMotionClick);
        m_btnRemoveExp.onClick.AddListener(OnButtonRemoveExpClick);
        m_btnRemoveAll.onClick.AddListener(OnButtonRemoveAllClick);

        
    }
    #endregion

    #region auto generated events
    private void OnToggleCurrentModelChange(bool value)
    {
        useCurrentModel = value;
    }
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
    private void OnButtonMotionEditClick()
    {
        MEEditUI.Instance.SetData(MEEditUI.MEEditType.Motion, GetModelJsonPaths());
    }
    private void OnButtonExpressionEditClick()
    {
        MEEditUI.Instance.SetData(MEEditUI.MEEditType.Expression, GetModelJsonPaths());
    }
    private void OnToggleIncludeChildrenChange(bool value)
    {
    }
    private void OnButtonAddMotionClick()
    {
        DoExecute(true, false, false);
    }
    private void OnButtonAddExpClick()
    {
        DoExecute(false, true, false);
    }
    private void OnButtonAddAllClick()
    {
        DoExecute(true, true, false);
    }
    private void OnButtonRemoveMotionClick()
    {
        DoExecute(true, false, true);
    }
    private void OnButtonRemoveExpClick()
    {
        DoExecute(false, true, true);
    }
    private void OnButtonRemoveAllClick()
    {
        DoExecute(true, true, true);
    }

    private void OnButtonSelectAddAllClick()
    {
        var files = L2DWUtils.OpenFileDialog("选择动作或表情文件", "motion", "json|mtn", true);
        if (files == null || files.Length == 0)
            return;
        
        DoAdds(files);
    }
    #endregion

    private bool useCurrentModel = false;
    private string FolderPath => m_iptFolder.text;
    private string Prefix => m_iptPrefix.text;
    private bool IncludeChildren => m_toggleIncludeChildren.isOn;
    
    
    protected override void OnPageShown()
    {
        UIEventBus.AddListener(UIEventType.OnModelChanged, RefreshAll);
        useCurrentModel = m_toggleCurrentModel.isOn;
        RefreshAll();

        base.OnPageShown();
    }
    
    protected override void OnPageHidden()
    {
        UIEventBus.RemoveListener(UIEventType.OnModelChanged, RefreshAll);

        base.OnPageHidden();
    }

    private void RefreshAll()
    {
        if (MainControl.Instance.curTarget == null || !m_lblCurrentModelName)
        {
            m_lblCurrentModelName.text = "当前模型: 无";
        }
        else
        {
            m_lblCurrentModelName.text = $"当前模型: {MainControl.Instance.curTarget.Name}";
        }
    }

    private List<string> GetModelJsonPaths()
    {
        if (useCurrentModel)
        {
            return L2DWUtils.GetModelJsonPaths();
        }
        else
        {
            var result = new List<string>();
            result.Add(m_iptJsonPath.text);
            return result;
        }
    }

    private void DoAdds(string[] files)
    {
        var jsonPaths = GetModelJsonPaths();
        
        foreach (var jsonPath in jsonPaths)
        {
            if (!IsJsonPathValid(jsonPath))
            {
                MessageTipWindow.Instance.Show("提示", $"Json路径无效\n{jsonPath}");
                return;
            }
        }

        foreach (var jsonPath in jsonPaths)
        {
            foreach (var file in files)
            {
                if(!L2DWUtils.IsSameDrive(jsonPath, file))
                {
                    MessageTipWindow.Instance.Show("错误", $"动作表情文件路径保存不能跨盘符！");
                    return;
                }
            }
        }

        var namePrefix = Prefix;
        if (!string.IsNullOrWhiteSpace(namePrefix) && !namePrefix.EndsWith("/"))
        {
            namePrefix += "/";
        }
        
        foreach (var jsonPath in jsonPaths)
        {
            var jsonObject = GetJsonObject(jsonPath);

            var motionNames = new List<string>();
            var motionPaths = new List<string>();
            var expressionNames = new List<string>();
            var expressionPaths = new List<string>();
            foreach (var file in files)
            {
                string name = Path.GetFileNameWithoutExtension(file);
                name = name.Split(".")[0];
                string extension = Path.GetExtension(file);
                var finalName = namePrefix + name;
                
                if (extension == ".mtn")
                {
                    motionNames.Add(finalName);
                    motionPaths.Add(file);
                }
                else if (extension == ".json" || extension == ".exp")
                {
                    expressionNames.Add(finalName);
                    expressionPaths.Add(file);
                }
            }

            if (motionPaths.Count > 0)
            {
                var motionError = L2DWUtils.AddMotionsToModelJson(
                    ref jsonObject,
                    jsonPath,
                    motionPaths,
                    motionNames
                );
                if (!string.IsNullOrEmpty(motionError))
                {
                    MessageTipWindow.Instance.Show("错误", motionError);
                    return;
                }
            }

            if (expressionPaths.Count > 0)
            {
                var expError = L2DWUtils.AddExpressionsToModelJson(
                    ref jsonObject,
                    jsonPath,
                    expressionPaths,
                    expressionNames
                );
                if (!string.IsNullOrEmpty(expError))
                {
                    MessageTipWindow.Instance.Show("错误", expError);
                    return;
                }
            }

            File.WriteAllText(jsonPath, jsonObject.ToString(true));
        }

        MessageTipWindow.Instance.Show("提示", $"动作表情文件已成功注册到当前模型\n你需要重载模型生效！");
    }

    private void DoExecute(bool isMotion, bool isExp, bool isRemove)
    {
        var jsonPaths = GetModelJsonPaths();
        
        foreach (var jsonPath in jsonPaths)
        {
            if (!IsJsonPathValid(jsonPath))
            {
                MessageTipWindow.Instance.Show("提示", $"模型 Json 路径无效\n{jsonPath}");
                return;
            }
        }

        if (!IsFolderValid() && !isRemove)
        {
            MessageTipWindow.Instance.Show("提示", "请输入有效的动作表情文件夹路径");
            return;
        }

        var namePrefix = Prefix;
        if (!string.IsNullOrWhiteSpace(namePrefix) && !namePrefix.EndsWith("/"))
        {
            namePrefix += "/";
        }


        foreach (var jsonPath in jsonPaths)
        {
            var jsonObject = GetJsonObject(jsonPath);
            if (isMotion)
            {
                if (isRemove)
                {
                    jsonObject.SetField("motions", new JSONObject(JSONObject.Type.OBJECT));
                }
                else
                {
                    var motionNames = new List<string>();
                    var motionPaths = GetMotionFiles(FolderPath).ToList();
                    foreach (var motionPath in motionPaths)
                    {
                        var motionRelativePath = PathHelper.GetRelativePath(FolderPath + "\\", motionPath);
                        var fixedName = GetFixedName(motionRelativePath);
                        motionNames.Add(namePrefix + fixedName);
                    }

                    var error = L2DWUtils.AddMotionsToModelJson(
                        ref jsonObject,
                        jsonPath,
                        motionPaths,
                        motionNames
                    );
                    if (!string.IsNullOrEmpty(error))
                    {
                        MessageTipWindow.Instance.Show("错误", error);
                        return;
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
                    var expressionNames = new List<string>();
                    var expressionPaths = GetExpFiles(FolderPath).ToList();
                    foreach (var expressionPath in expressionPaths)
                    {
                        var expressionRelativePath = PathHelper.GetRelativePath(FolderPath + "\\", expressionPath);
                        var fixedName = GetFixedName(expressionRelativePath);
                        expressionNames.Add(namePrefix + fixedName);
                    }

                    var error = L2DWUtils.AddExpressionsToModelJson(
                        ref jsonObject,
                        jsonPath,
                        expressionPaths,
                        expressionNames
                    );
                    if (!string.IsNullOrEmpty(error))
                    {
                        MessageTipWindow.Instance.Show("错误", error);
                        return;
                    }
                }
            }
            
            File.WriteAllText(jsonPath, jsonObject.ToString(true));
        }
        
        var fileType = isMotion ? (isExp ? "动作和表情" : "动作") : "表情";
        var action = isRemove ? "移除出模型" : "注册到模型";
        MessageTipWindow.Instance.Show("提示", $"{fileType}文件已成功{action}\n你需要重载模型生效！");
    }

    private bool IsJsonPathValid(string jsonPath)
    {
        if (string.IsNullOrEmpty(jsonPath) || !File.Exists(jsonPath))
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