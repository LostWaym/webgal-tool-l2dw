using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProfileSettingWindow : BaseWindow<ProfileSettingWindow>
{
    #region auto generated members
    private InputField m_iptName;
    private InputField m_iptModelFilePath;
    private InputField m_iptFormatText;
    private InputField m_iptTransformFormatText;
    private Transform m_tfSubModelRoot;
    private Transform m_itemSubModel;
    private Button m_btnAddSubModelPath;
    private Button m_btnSave;
    private Button m_btnCancel;
    #endregion

    #region auto generated binders
    protected override void CodeGenBindMembers()
    {
        m_iptName = transform.Find("Background/Popup/Window/Pages/ScrollRect/Viewport/Content/角色名/Value/InputField/m_iptName").GetComponent<InputField>();
        m_iptModelFilePath = transform.Find("Background/Popup/Window/Pages/ScrollRect/Viewport/Content/主模型路径/Value/InputField/m_iptModelFilePath").GetComponent<InputField>();
        m_iptFormatText = transform.Find("Background/Popup/Window/Pages/ScrollRect/Viewport/Content/立绘指令模板/Value/InputField/m_iptFormatText").GetComponent<InputField>();
        m_iptTransformFormatText = transform.Find("Background/Popup/Window/Pages/ScrollRect/Viewport/Content/变换指令模板/Value/InputField/m_iptTransformFormatText").GetComponent<InputField>();
        m_tfSubModelRoot = transform.Find("Background/Popup/Window/Pages/ScrollRect/Viewport/Content/子立绘路径/m_tfSubModelRoot").GetComponent<Transform>();
        m_itemSubModel = transform.Find("Background/Popup/Window/Pages/ScrollRect/Viewport/Content/子立绘路径/m_tfSubModelRoot/m_itemSubModel").GetComponent<Transform>();
        m_btnAddSubModelPath = transform.Find("Background/Popup/Window/Pages/ScrollRect/Viewport/Content/子立绘路径/GameObject/m_btnAddSubModelPath").GetComponent<Button>();
        m_btnSave = transform.Find("Background/Popup/Window/Bottom/m_btnSave").GetComponent<Button>();
        m_btnCancel = transform.Find("Background/Popup/Window/Top/m_btnCancel").GetComponent<Button>();

        m_iptName.onValueChanged.AddListener(OnInputFieldNameChange);
        m_iptName.onEndEdit.AddListener(OnInputFieldNameEndEdit);
        m_iptModelFilePath.onValueChanged.AddListener(OnInputFieldModelFilePathChange);
        m_iptModelFilePath.onEndEdit.AddListener(OnInputFieldModelFilePathEndEdit);
        m_iptFormatText.onValueChanged.AddListener(OnInputFieldFormatTextChange);
        m_iptFormatText.onEndEdit.AddListener(OnInputFieldFormatTextEndEdit);
        m_iptTransformFormatText.onValueChanged.AddListener(OnInputFieldTransformFormatTextChange);
        m_iptTransformFormatText.onEndEdit.AddListener(OnInputFieldTransformFormatTextEndEdit);
        m_btnAddSubModelPath.onClick.AddListener(OnButtonAddSubModelPathClick);
        m_btnSave.onClick.AddListener(OnButtonSaveClick);
        m_btnCancel.onClick.AddListener(OnButtonCancelClick);
    }
    #endregion



    #region auto generated events
    private void OnInputFieldNameChange(string value)
    {
    }
    private void OnInputFieldNameEndEdit(string value)
    {
    }
    private void OnInputFieldModelFilePathChange(string value)
    {
    }
    private void OnInputFieldModelFilePathEndEdit(string value)
    {
        m_iptModelFilePath.SetTextWithoutNotify(L2DWUtils.TryParseModelRelativePath(value));
    }
    private void OnInputFieldFormatTextChange(string value)
    {
    }
    private void OnInputFieldFormatTextEndEdit(string value)
    {
    }
    private void OnInputFieldTransformFormatTextChange(string value)
    {
    }
    private void OnInputFieldTransformFormatTextEndEdit(string value)
    {
    }
    private void OnButtonAddSubModelPathClick()
    {
        AddSubModel("0.0");
    }

    private void OnButtonSaveClick()
    {
        Save();
        onSaveAction?.Invoke(meta);
        Close();    
    }
    private void OnButtonCancelClick()
    {
        Close();
    }
    #endregion
    private MyGOLive2DExMeta meta;

    public static Action<MyGOLive2DExMeta> onSaveAction;

    protected override void OnInit()
    {
        base.OnInit();
    }

    public void Load(MyGOLive2DExMeta meta)
    {
        this.meta = meta;
        m_iptName.text = meta.name;
        m_iptFormatText.text = meta.formatText;
        m_iptModelFilePath.text = meta.modelFilePath;
        m_iptTransformFormatText.text = meta.transformFormatText;
        LoadSubModels();
    }

    public void Save()
    {
        meta.name = m_iptName.text;
        meta.formatText = m_iptFormatText.text;
        meta.modelFilePath = m_iptModelFilePath.text;
        meta.transformFormatText = m_iptTransformFormatText.text;
        SaveSubModels();
    }

    #region SubModels

    private List<ProfileSettingSubModelInfo> subModelInfos = new List<ProfileSettingSubModelInfo>();
    private List<ProfileSettingSubModelWidget> subModelWidgets = new List<ProfileSettingSubModelWidget>();
    private void LoadSubModels()
    {
        // m_iptModelFilePaths.text = string.Join("\n", meta.modelFilePaths);
        subModelInfos.Clear();
        for (int i = 0; i < meta.modelFilePaths.Count; i++)
        {
            string filePath = meta.modelFilePaths[i];
            meta.GetModelOffset(i + 1, out var offsetX, out var offsetY);
            subModelInfos.Add(new ProfileSettingSubModelInfo() { filePath = filePath, offsetX = offsetX, offsetY = offsetY });
        }
        RefreshSubModelWidgets();
    }

    private void RefreshSubModelWidgets()
    {
        SetListItem(subModelWidgets, m_itemSubModel.gameObject, m_tfSubModelRoot, subModelInfos.Count, OnSubModelItemCreate);
        for (int i = 0; i < subModelInfos.Count; i++)
        {
            subModelWidgets[i].SetData(i, subModelInfos[i]);
        }
    }

    private void OnSubModelItemCreate(ProfileSettingSubModelWidget widget)
    {
        widget._OnDeleteClick = OnSubModelDeleteClick;
        widget._OnUpClick = OnSubModelUpClick;
        widget._OnDownClick = OnSubModelDownClick;
        widget._OnModelFilePathChange = OnSubModelFilePathChange;
        widget._OnXChange = OnSubModelXChange;
        widget._OnYChange = OnSubModelYChange;
    }

    private void OnSubModelFilePathChange(ProfileSettingSubModelWidget widget)
    {
        subModelInfos[widget.index].filePath = widget.Text;
    }

    private void OnSubModelXChange(ProfileSettingSubModelWidget widget, float value)
    {
        subModelInfos[widget.index].offsetX = value;
    }

    private void OnSubModelYChange(ProfileSettingSubModelWidget widget, float value)
    {
        subModelInfos[widget.index].offsetY = value;
    }

    public void SaveSubModels()
    {
        meta.modelFilePaths.Clear();
        foreach (var info in subModelInfos)
        {
            if (!string.IsNullOrEmpty(info.filePath))
            {
                meta.modelFilePaths.Add(info.filePath);
            }
        }
        meta.modelOffset.Clear();
        foreach (var info in subModelInfos)
        {
            meta.modelOffset.Add(info.offsetX);
            meta.modelOffset.Add(info.offsetY);
        }
    }

    public void AddSubModel(string filePath)
    {
        subModelInfos.Add(new ProfileSettingSubModelInfo() { filePath = filePath, offsetX = 0.0f, offsetY = 0.0f });
        RefreshSubModelWidgets();
    }

    private void OnSubModelDeleteClick(ProfileSettingSubModelWidget widget)
    {
        subModelInfos.RemoveAt(widget.index);
        RefreshSubModelWidgets();
    }

    private void OnSubModelUpClick(ProfileSettingSubModelWidget widget)
    {
        var index = widget.index;
        if (index > 0)
        {
            var temp = subModelInfos[index];
            subModelInfos[index] = subModelInfos[index - 1];
            subModelInfos[index - 1] = temp;
            RefreshSubModelWidgets();
        }
    }

    private void OnSubModelDownClick(ProfileSettingSubModelWidget widget)
    {
        var index = widget.index;
        if (index < subModelInfos.Count - 1)
        {
            var temp = subModelInfos[index];
            subModelInfos[index] = subModelInfos[index + 1];
            subModelInfos[index + 1] = temp;
            RefreshSubModelWidgets();
        }
    }

    #endregion
}

public class ProfileSettingSubModelInfo
{
    public string filePath;
    public float offsetX;
    public float offsetY;
}

public class ProfileSettingSubModelWidget : UIItemWidget<ProfileSettingSubModelWidget>
{
    #region auto generated members
    private InputField m_iptModelFilePaths;
    private InputField m_iptX;
    private InputField m_iptY;
    private Button m_btnDelete;
    private Button m_btnUp;
    private Button m_btnDown;
    #endregion

    #region auto generated binders
    protected override void CodeGenBindMembers()
    {
        m_iptModelFilePaths = transform.Find("InputField/m_iptModelFilePaths").GetComponent<InputField>();
        m_iptX = transform.Find("OffsetX/m_iptX").GetComponent<InputField>();
        m_iptY = transform.Find("OffsetY/m_iptY").GetComponent<InputField>();
        m_btnDelete = transform.Find("m_btnDelete").GetComponent<Button>();
        m_btnUp = transform.Find("m_btnUp").GetComponent<Button>();
        m_btnDown = transform.Find("m_btnDown").GetComponent<Button>();

        m_iptModelFilePaths.onValueChanged.AddListener(OnInputFieldModelFilePathsChange);
        m_iptModelFilePaths.onEndEdit.AddListener(OnInputFieldModelFilePathsEndEdit);
        m_iptX.onValueChanged.AddListener(OnInputFieldXChange);
        m_iptX.onEndEdit.AddListener(OnInputFieldXEndEdit);
        m_iptY.onValueChanged.AddListener(OnInputFieldYChange);
        m_iptY.onEndEdit.AddListener(OnInputFieldYEndEdit);
        m_btnDelete.onClick.AddListener(OnButtonDeleteClick);
        m_btnUp.onClick.AddListener(OnButtonUpClick);
        m_btnDown.onClick.AddListener(OnButtonDownClick);
    }
    #endregion


    #region auto generated events
    private void OnInputFieldModelFilePathsChange(string value)
    {
        _OnModelFilePathChange?.Invoke(this);
    }
    private void OnInputFieldModelFilePathsEndEdit(string value)
    {
        m_iptModelFilePaths.SetTextWithoutNotify(L2DWUtils.TryParseModelRelativePath(value));
        _OnModelFilePathChange?.Invoke(this);
    }
    private void OnInputFieldXChange(string value)
    {
    }
    private void OnInputFieldXEndEdit(string value)
    {
        if (float.TryParse(value, out var result))
        {
            _OnXChange?.Invoke(this, result);
            return;
        }
        m_iptX.SetTextWithoutNotify(info.offsetX.ToString());
    }
    private void OnInputFieldYChange(string value)
    {
    }
    private void OnInputFieldYEndEdit(string value)
    {
        if (float.TryParse(value, out var result))
        {
            _OnYChange?.Invoke(this, result);
            return;
        }
        m_iptY.SetTextWithoutNotify(info.offsetY.ToString());
    }

    private void OnButtonDeleteClick()
    {
        _OnDeleteClick?.Invoke(this);
    }
    private void OnButtonUpClick()
    {
        _OnUpClick?.Invoke(this);
    }
    private void OnButtonDownClick()
    {
        _OnDownClick?.Invoke(this);
    }
    #endregion

    public Action<ProfileSettingSubModelWidget> _OnDeleteClick;
    public Action<ProfileSettingSubModelWidget> _OnUpClick;
    public Action<ProfileSettingSubModelWidget> _OnDownClick;
    public Action<ProfileSettingSubModelWidget> _OnModelFilePathChange;
    public Action<ProfileSettingSubModelWidget, float> _OnXChange;
    public Action<ProfileSettingSubModelWidget, float> _OnYChange;

    public int index;
    public string Text => m_iptModelFilePaths.text;
    private ProfileSettingSubModelInfo info;
    public void SetData(int index, ProfileSettingSubModelInfo info)
    {
        this.index = index;
        this.info = info;
        m_iptModelFilePaths.SetTextWithoutNotify(info.filePath);
        m_iptX.SetTextWithoutNotify(info.offsetX.ToString());
        m_iptY.SetTextWithoutNotify(info.offsetY.ToString());
    }

}

