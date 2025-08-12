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
        m_btnSetParam = transform.Find("LabelValueH (2)/m_btnSetParam").GetComponent<Button>();
        m_btnRemoveParam = transform.Find("LabelValueH (2)/m_btnRemoveParam").GetComponent<Button>();

        m_iptPath.onValueChanged.AddListener(OnInputFieldPathChange);
        m_iptPath.onEndEdit.AddListener(OnInputFieldPathEndEdit);
        m_iptName.onValueChanged.AddListener(OnInputFieldNameChange);
        m_iptName.onEndEdit.AddListener(OnInputFieldNameEndEdit);
        m_btnSetInputParam.onClick.AddListener(OnButtonSetInputParamClick);
        m_iptValue.onValueChanged.AddListener(OnInputFieldValueChange);
        m_iptValue.onEndEdit.AddListener(OnInputFieldValueEndEdit);
        m_btnSetParam.onClick.AddListener(OnButtonSetParamClick);
        m_btnRemoveParam.onClick.AddListener(OnButtonRemoveParamClick);
    }
    #endregion


    #region auto generated events
    private void OnInputFieldPathChange(string value)
    {
        Debug.Log("OnInputFieldPathChange");
    }
    private void OnInputFieldPathEndEdit(string value)
    {
        Debug.Log("OnInputFieldPathEndEdit");
    }
    private void OnInputFieldNameChange(string value)
    {
        Debug.Log("OnInputFieldNameChange");
    }
    private void OnInputFieldNameEndEdit(string value)
    {
        Debug.Log("OnInputFieldNameEndEdit");
    }
    private void OnButtonSetInputParamClick()
    {
        m_iptName.text = m_lblInputParamName.text;
    }
    private void OnInputFieldValueChange(string value)
    {
        Debug.Log("OnInputFieldValueChange");
    }
    private void OnInputFieldValueEndEdit(string value)
    {
        Debug.Log("OnInputFieldValueEndEdit");
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
        var files = Directory.GetFiles(path, "*.mtn");
        return files;
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
    #endregion

    #region auto generated binders
    protected override void CodeGenBindMembers()
    {

    }
    #endregion

    #region auto generated events
    #endregion

}