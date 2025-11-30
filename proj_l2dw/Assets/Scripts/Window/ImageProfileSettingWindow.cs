using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageProfileSettingWindow : BaseWindow<ImageProfileSettingWindow>
{
    public static Action<ImageModelMeta> onSaveAction;

    #region auto generated members
    private InputField m_iptName;
    private InputField m_iptModelFilePath;
    private InputField m_iptFormatText;
    private Button m_btnGenFormatText;
    private InputField m_iptTransformFormatText;
    private Button m_btnGenTransformFormatText;
    private Button m_btnSave;
    private Button m_btnCancel;
    #endregion

    #region auto generated binders
    protected override void CodeGenBindMembers()
    {
        m_iptName = transform.Find("Background/Popup/Window/Pages/ScrollRect/Viewport/Content/角色名/Value/InputField/m_iptName").GetComponent<InputField>();
        m_iptModelFilePath = transform.Find("Background/Popup/Window/Pages/ScrollRect/Viewport/Content/主模型路径/Value/InputField/m_iptModelFilePath").GetComponent<InputField>();
        m_iptFormatText = transform.Find("Background/Popup/Window/Pages/ScrollRect/Viewport/Content/立绘指令模板/Value/InputField/m_iptFormatText").GetComponent<InputField>();
        m_btnGenFormatText = transform.Find("Background/Popup/Window/Pages/ScrollRect/Viewport/Content/立绘指令模板/Label/m_btnGenFormatText").GetComponent<Button>();
        m_iptTransformFormatText = transform.Find("Background/Popup/Window/Pages/ScrollRect/Viewport/Content/变换指令模板/Value/InputField/m_iptTransformFormatText").GetComponent<InputField>();
        m_btnGenTransformFormatText = transform.Find("Background/Popup/Window/Pages/ScrollRect/Viewport/Content/变换指令模板/Label/m_btnGenTransformFormatText").GetComponent<Button>();
        m_btnSave = transform.Find("Background/Popup/Window/Bottom/m_btnSave").GetComponent<Button>();
        m_btnCancel = transform.Find("Background/Popup/Window/Top/m_btnCancel").GetComponent<Button>();

        m_iptName.onValueChanged.AddListener(OnInputFieldNameChange);
        m_iptName.onEndEdit.AddListener(OnInputFieldNameEndEdit);
        m_iptModelFilePath.onValueChanged.AddListener(OnInputFieldModelFilePathChange);
        m_iptModelFilePath.onEndEdit.AddListener(OnInputFieldModelFilePathEndEdit);
        m_iptFormatText.onValueChanged.AddListener(OnInputFieldFormatTextChange);
        m_iptFormatText.onEndEdit.AddListener(OnInputFieldFormatTextEndEdit);
        m_btnGenFormatText.onClick.AddListener(OnButtonGenFormatTextClick);
        m_iptTransformFormatText.onValueChanged.AddListener(OnInputFieldTransformFormatTextChange);
        m_iptTransformFormatText.onEndEdit.AddListener(OnInputFieldTransformFormatTextEndEdit);
        m_btnGenTransformFormatText.onClick.AddListener(OnButtonGenTransformFormatTextClick);
        m_btnSave.onClick.AddListener(OnButtonSaveClick);
        m_btnCancel.onClick.AddListener(OnButtonCancelClick);
    }
    #endregion

    #region auto generated events
    private void OnInputFieldNameChange(string value)
    {
        Debug.Log("OnInputFieldNameChange");
    }
    private void OnInputFieldNameEndEdit(string value)
    {
        Debug.Log("OnInputFieldNameEndEdit");
    }
    private void OnInputFieldModelFilePathChange(string value)
    {
        Debug.Log("OnInputFieldModelFilePathChange");
    }
    private void OnInputFieldModelFilePathEndEdit(string value)
    {
        if (!L2DWUtils.TryGetRelativePath(value, Global.ModelPath, out var relativePath))
        {
            MessageTipWindow.instance.Show("错误", "图片文件路径错误！\n你应该选择模型文件夹下的图片文件");
            m_iptModelFilePath.SetTextWithoutNotify(m_meta.imgPath);
            return;
        }
    }
    private void OnInputFieldFormatTextChange(string value)
    {
        Debug.Log("OnInputFieldFormatTextChange");
    }
    private void OnInputFieldFormatTextEndEdit(string value)
    {
        Debug.Log("OnInputFieldFormatTextEndEdit");
    }
    private void OnButtonGenFormatTextClick()
    {
        m_iptFormatText.text = L2DWUtils.GenerateFormatText(
            m_iptName.text.Trim(),
            m_iptModelFilePath.text.Trim(),
            Array.Empty<string>()
        );
    }
    private void OnInputFieldTransformFormatTextChange(string value)
    {
        Debug.Log("OnInputFieldTransformFormatTextChange");
    }
    private void OnInputFieldTransformFormatTextEndEdit(string value)
    {
        Debug.Log("OnInputFieldTransformFormatTextEndEdit");
    }
    private void OnButtonGenTransformFormatTextClick()
    {
        m_iptTransformFormatText.text = L2DWUtils.GenerateTransformFormatText(
            m_iptName.text.Trim(),
            1
        );
    }
    private void OnButtonSaveClick()
    {
        m_meta.name = m_iptName.text;
        m_meta.imgPath = m_iptModelFilePath.text;
        m_meta.motionTemplate = m_iptFormatText.text;
        m_meta.transformTemplate = m_iptTransformFormatText.text;
        m_meta.CalculateRelativePath();
        onSaveAction?.Invoke(m_meta);
        Close();
    }
    private void OnButtonCancelClick()
    {
        Close();
    }
    #endregion

    private ImageModelMeta m_meta;

    internal void Load(ImageModelMeta meta)
    {
        m_iptName.SetTextWithoutNotify(meta.name);
        m_iptModelFilePath.SetTextWithoutNotify(meta.imgPath);
        m_iptFormatText.SetTextWithoutNotify(meta.motionTemplate);
        m_iptTransformFormatText.SetTextWithoutNotify(meta.transformTemplate);
        m_meta = meta;
    }
}
