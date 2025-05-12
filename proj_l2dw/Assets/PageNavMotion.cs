

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class PageNavMotion : UIPageWidget<PageNavMotion>
{
    #region auto generated members
    private Button m_btnChara;
    private Button m_btnMotion;
    private Button m_btnApply;
    private Button m_btnEdit;
    private Button m_btnSave;
    private Button m_btnRecord;
    private Button m_btnOperation;
    private Toggle m_toggleFilter;
    private InputField m_iptFilter;
    private Toggle m_toggleCurveMode;
    private Button m_btnNavHome;
    private Button m_btnNavLeft;
    private Button m_btnPlay;
    private Button m_btnNavRight;
    private Button m_btnNavEnd;
    private InputField m_iptDuration;
    private InputField m_iptFrame;
    private Transform m_itemLabels;
    private TouchArea m_touchLabels;
    private InputField m_iptFrameIndex;
    private Transform m_tfTrackHeaderRoot;
    private Transform m_itemTrackHeader;
    private Transform m_tfTrackRoot;
    private Transform m_itemTrack;
    private Image m_imgLine;
    private Slider m_sliderH;
    private Slider m_sliderV;
    #endregion

    #region auto generated binders
    protected override void CodeGenBindMembers()
    {
        m_btnChara = transform.Find("CharaArea/ToolBar/m_btnChara").GetComponent<Button>();
        m_btnMotion = transform.Find("CharaArea/ToolBar/m_btnMotion").GetComponent<Button>();
        m_btnApply = transform.Find("CharaArea/ToolBar/m_btnApply").GetComponent<Button>();
        m_btnEdit = transform.Find("CharaArea/ToolBar/m_btnEdit").GetComponent<Button>();
        m_btnSave = transform.Find("CharaArea/ToolBar/m_btnSave").GetComponent<Button>();
        m_btnRecord = transform.Find("TimelineArea/ToolBar/Left/Top/m_btnRecord").GetComponent<Button>();
        m_btnOperation = transform.Find("TimelineArea/ToolBar/Left/Top/m_btnOperation").GetComponent<Button>();
        m_toggleFilter = transform.Find("TimelineArea/ToolBar/Left/Bottom/m_toggleFilter").GetComponent<Toggle>();
        m_iptFilter = transform.Find("TimelineArea/ToolBar/Left/Bottom/m_iptFilter").GetComponent<InputField>();
        m_toggleCurveMode = transform.Find("TimelineArea/ToolBar/Right/Top/m_toggleCurveMode").GetComponent<Toggle>();
        m_btnNavHome = transform.Find("TimelineArea/ToolBar/Right/Top/GameObject/m_btnNavHome").GetComponent<Button>();
        m_btnNavLeft = transform.Find("TimelineArea/ToolBar/Right/Top/GameObject/m_btnNavLeft").GetComponent<Button>();
        m_btnPlay = transform.Find("TimelineArea/ToolBar/Right/Top/GameObject/m_btnPlay").GetComponent<Button>();
        m_btnNavRight = transform.Find("TimelineArea/ToolBar/Right/Top/GameObject/m_btnNavRight").GetComponent<Button>();
        m_btnNavEnd = transform.Find("TimelineArea/ToolBar/Right/Top/GameObject/m_btnNavEnd").GetComponent<Button>();
        m_iptDuration = transform.Find("TimelineArea/ToolBar/Right/Top/m_iptDuration").GetComponent<InputField>();
        m_iptFrame = transform.Find("TimelineArea/ToolBar/Right/Top/m_iptFrame").GetComponent<InputField>();
        m_itemLabels = transform.Find("TimelineArea/ToolBar/Right/Bottom/m_itemLabels").GetComponent<Transform>();
        m_touchLabels = transform.Find("TimelineArea/ToolBar/Right/Bottom/m_touchLabels").GetComponent<TouchArea>();
        m_iptFrameIndex = transform.Find("TimelineArea/ToolBar/Right/Bottom/m_iptFrameIndex").GetComponent<InputField>();
        m_tfTrackHeaderRoot = transform.Find("TimelineArea/Bottom/Left/m_tfTrackHeaderRoot").GetComponent<Transform>();
        m_itemTrackHeader = transform.Find("TimelineArea/Bottom/Left/m_tfTrackHeaderRoot/m_itemTrackHeader").GetComponent<Transform>();
        m_tfTrackRoot = transform.Find("TimelineArea/Bottom/Right/m_tfTrackRoot").GetComponent<Transform>();
        m_itemTrack = transform.Find("TimelineArea/Bottom/Right/m_tfTrackRoot/m_itemTrack").GetComponent<Transform>();
        m_imgLine = transform.Find("TimelineArea/Bottom/Right/m_imgLine").GetComponent<Image>();
        m_sliderH = transform.Find("TimelineArea/Bottom/Right/m_sliderH").GetComponent<Slider>();
        m_sliderV = transform.Find("TimelineArea/Bottom/Right/m_sliderV").GetComponent<Slider>();

        m_btnChara.onClick.AddListener(OnButtonCharaClick);
        m_btnMotion.onClick.AddListener(OnButtonMotionClick);
        m_btnApply.onClick.AddListener(OnButtonApplyClick);
        m_btnEdit.onClick.AddListener(OnButtonEditClick);
        m_btnSave.onClick.AddListener(OnButtonSaveClick);
        m_btnRecord.onClick.AddListener(OnButtonRecordClick);
        m_btnOperation.onClick.AddListener(OnButtonOperationClick);
        m_toggleFilter.onValueChanged.AddListener(OnToggleFilterChange);
        m_iptFilter.onValueChanged.AddListener(OnInputFieldFilterChange);
        m_iptFilter.onEndEdit.AddListener(OnInputFieldFilterEndEdit);
        m_toggleCurveMode.onValueChanged.AddListener(OnToggleCurveModeChange);
        m_btnNavHome.onClick.AddListener(OnButtonNavHomeClick);
        m_btnNavLeft.onClick.AddListener(OnButtonNavLeftClick);
        m_btnPlay.onClick.AddListener(OnButtonPlayClick);
        m_btnNavRight.onClick.AddListener(OnButtonNavRightClick);
        m_btnNavEnd.onClick.AddListener(OnButtonNavEndClick);
        m_iptDuration.onValueChanged.AddListener(OnInputFieldDurationChange);
        m_iptDuration.onEndEdit.AddListener(OnInputFieldDurationEndEdit);
        m_iptFrame.onValueChanged.AddListener(OnInputFieldFrameChange);
        m_iptFrame.onEndEdit.AddListener(OnInputFieldFrameEndEdit);
        m_iptFrameIndex.onValueChanged.AddListener(OnInputFieldFrameIndexChange);
        m_iptFrameIndex.onEndEdit.AddListener(OnInputFieldFrameIndexEndEdit);
        m_sliderH.onValueChanged.AddListener(OnSliderHChange);
        m_sliderV.onValueChanged.AddListener(OnSliderVChange);
    }
    #endregion



    #region auto generated events
    private void OnButtonCharaClick()
    {
        Debug.Log("OnButtonCharaClick");
    }
    private void OnButtonMotionClick()
    {
        Debug.Log("OnButtonMotionClick");
    }
    private void OnButtonApplyClick()
    {
        var curTarget = GetValidTarget();
        if (curTarget == null)
        {
            return;
        }
        var json = GUIUtility.systemCopyBuffer;
        m_motionData.Load(json);
        curTarget.SetDisplayMode(ModelDisplayMode.MotionEditor, true);
        RefreshAll();
    }
    private void OnButtonEditClick()
    {
        var json = m_motionData.Save();
        GUIUtility.systemCopyBuffer = json;
        Debug.Log(json);
    }
    private void OnButtonSaveClick()
    {
        m_motionData.BakeAllFrames();
        var text = m_motionData.info.Print();
        Debug.Log(text);
        GUIUtility.systemCopyBuffer = text;
    }
    private void OnButtonRecordClick()
    {
        Debug.Log("OnButtonRecordClick");
    }
    private void OnButtonOperationClick()
    {
        Debug.Log("OnButtonOperationClick");
    }
    private void OnToggleFilterChange(bool value)
    {
        RefreshAll();
    }
    private void OnInputFieldFilterChange(string value)
    {
        RefreshAll();
    }
    private void OnInputFieldFilterEndEdit(string value)
    {
        Debug.Log("OnInputFieldFilterEndEdit");
    }
    private void OnToggleCurveModeChange(bool value)
    {
        Debug.Log("OnToggleCurveModeChange");
    }
    private void OnButtonNavHomeClick()
    {
        SetFrameIndex(0);
    }
    private void OnButtonNavLeftClick()
    {
        SetFrameIndex(curFrameIndex - 1);
    }
    private void OnButtonPlayClick()
    {
        if (isPlaying)
        {
            StopSample();
        }
        else
        {
            PlaySample();
        }

    }
    private void OnButtonNavRightClick()
    {
        SetFrameIndex(curFrameIndex + 1);
    }
    private void OnButtonNavEndClick()
    {
        SetFrameIndex(m_motionData.info.frameCount - 1);
    }
    private void OnInputFieldDurationChange(string value)
    {
        Debug.Log("OnInputFieldDurationChange");
    }
    private void OnInputFieldDurationEndEdit(string value)
    {
        if (int.TryParse(value, out int val))
        {
            m_motionData.info.frameCount = val;
            RefreshAll();
        }
    }
    private void OnInputFieldFrameChange(string value)
    {
        Debug.Log("OnInputFieldFrameChange");
    }
    private void OnInputFieldFrameEndEdit(string value)
    {
        var frameIndex = m_iptFrame.text;
        if (int.TryParse(frameIndex, out int frame))
        {
            SetFps(frame);
        }
    }
    private void OnInputFieldFrameIndexChange(string value)
    {
        Debug.Log("OnInputFieldFrameIndexChange");
    }
    private void OnInputFieldFrameIndexEndEdit(string value)
    {
        if (int.TryParse(value, out int frameIndex))
        {
            SetFrameIndex(frameIndex - 1);
            return;
        }

        RefreshAll();
    }

    private void OnSliderHChange(float value)
    {
        RefreshMotionTrack();
    }
    private void OnSliderVChange(float value)
    {
        RefreshMotionTrackHeader();
        RefreshMotionTrack();
    }

    #endregion

    private List<MotionTrackWidget> m_listMotionTrack = new List<MotionTrackWidget>();
    private List<MotionTrackHeaderWidget> m_listMotionTrackHeader = new List<MotionTrackHeaderWidget>();
    private PageNavMotionLabelWidget m_pageNavMotionLabel;
    private List<Live2DParamInfo> paramKeys = new List<Live2DParamInfo>();

    private Live2dMotionData m_motionData;

    public const int MAX_TRACK_DISPLAY_COUNT = 13;
    public const int MAX_FRAME_DISPLAY_COUNT = 31;

    public int curFrameIndex = 0;

    protected override void OnInit()
    {
        m_pageNavMotionLabel = PageNavMotionLabelWidget.CreateWidget(m_itemLabels.gameObject);

        m_touchLabels._OnPointerMove += OnTouchLabelsPointerMove;
    }

    private void OnTouchLabelsPointerMove(Vector2 position)
    {
        // 获取position离m_itemLabels最近的label
        int minIndex = m_pageNavMotionLabel.GetRelativeLabelByPosition(position);
        SetFrameIndex(minIndex);
    }

    public override void OnPageShown()
    {
        base.OnPageShown();

        MainControl.Instance.UpdateBeat += Update;

        var curTarget = GetValidTarget();
        if (curTarget == null)
        {
            RefreshAll();
            return;
        }

        curTarget.SetDisplayMode(ModelDisplayMode.MotionEditor);
        m_motionData = Live2dMotionData.Create();
        paramKeys = curTarget.GetEmotionEditorList().list;
        RefreshAll();
    }

    public override void OnPageHidden()
    {
        base.OnPageHidden();
        MainControl.Instance.UpdateBeat -= Update;
    }

    public bool isPlaying;
    public float startTime;
    public int startFrameIndex;
    
    private void Update()
    {
        if (isPlaying)
        {
            var delta = Time.time - startTime;
            int frameIndex = startFrameIndex + (int)(delta * 30);
            SetFrameIndex(frameIndex, false);
        }
    }

    public void PlaySample()
    {
        isPlaying = true;
        startTime = Time.time;
        startFrameIndex = curFrameIndex;
    }

    public void StopSample()
    {
        isPlaying = false;
    }

    public void SetFps(int fps)
    {
        m_motionData.info.fps = fps;
        RefreshAll();
    }

    public void SetFrameIndex(int index, bool stopSample = true)
    {
        if (index == curFrameIndex)
        {
            return;
        }

        if (stopSample)
        {
            StopSample();
        }

        curFrameIndex = Mathf.Clamp(index, 0, m_motionData.info.frameCount - 1);

        int h = (int)m_sliderH.value;
        int hmax = m_motionData.info.frameCount;
        int hrange = h + MAX_FRAME_DISPLAY_COUNT;
        if (hrange > hmax)
        {
            hrange = hmax;
        }
        
        if (curFrameIndex < h)
        {
            m_sliderH.SetValueWithoutNotify(curFrameIndex);
        }
        else if (curFrameIndex >= hrange)
        {
            var val = curFrameIndex - MAX_FRAME_DISPLAY_COUNT + 1;
            m_sliderH.SetValueWithoutNotify(val);
        }
        
        RefreshAll();
    }

    public void SampleFrame()
    {
        var curTarget = MainControl.Instance.curTarget;
        if (curTarget == null || !curTarget.SupportAnimationMode)
        {
            return;
        }

        foreach (var track in m_motionData.tracks)
        {
            string paramName = track.Key;
            if (m_motionData.info.TryGetKeyFrameValue(paramName, curFrameIndex, out float value))
            {
                curTarget.Sample(paramName, value);
            }
        }
    }

    public void RefreshAll()
    {
        var curTarget = MainControl.Instance.curTarget;
        if (curTarget == null || !curTarget.SupportAnimationMode)
        {
            return;
        }
        RefreshMotionTrackHeader();
        RefreshMotionTrack();
        RefreshSlider();
        SampleFrame();
        RefreshTrackLabels();
        m_iptDuration.SetTextWithoutNotify(m_motionData.info.frameCount.ToString());
        m_iptFrame.SetTextWithoutNotify(m_motionData.info.fps.ToString());
        m_iptFrameIndex.SetTextWithoutNotify((curFrameIndex + 1).ToString());
    }

    public void LoadFromMotion(string motionName)
    {
        var curTarget = MainControl.Instance.curTarget;
        if (curTarget == null || !curTarget.SupportAnimationMode)
        {
            return;
        }

        var data = curTarget.MyGOConfig.motions[motionName];
        // 转换byte[]为string
        string text = System.Text.Encoding.UTF8.GetString(data);
        m_motionData = Live2dMotionData.Create(text);
        paramKeys = curTarget.GetEmotionEditorList().list;
        RefreshAll();
    }

    public void RefreshFrameLine()
    {
        if (m_pageNavMotionLabel.GetLabelPosition(curFrameIndex, out var position))
        {
            var linePos = m_imgLine.rectTransform.position;
            linePos.x = position.x;
            m_imgLine.rectTransform.position = linePos;
            m_imgLine.gameObject.SetActive(true);
        }
        else
        {
            m_imgLine.gameObject.SetActive(false);
        }
    }

    public void RefreshSlider()
    {
        m_sliderH.maxValue = m_motionData.info.frameCount - MAX_FRAME_DISPLAY_COUNT;
        m_sliderV.maxValue = filteredParamKeys.Count - MAX_TRACK_DISPLAY_COUNT;

        m_sliderH.maxValue = Mathf.Max(m_sliderH.maxValue, 0);
        m_sliderV.maxValue = Mathf.Max(m_sliderV.maxValue, 0);
    }

    public void RefreshTrackLabels()
    {
        var trackIndex = (int)m_sliderH.value;
        m_pageNavMotionLabel.SetData(trackIndex);
        RefreshFrameLine();
    }

    private ModelAdjusterBase GetValidTarget()
    {
        var curTarget = MainControl.Instance.curTarget;
        if (curTarget == null || !curTarget.SupportAnimationMode)
        {
            return null;
        }

        return curTarget;
    }

    private List<Live2DParamInfo> filteredParamKeys = new List<Live2DParamInfo>();
    public void UpdateFilter()
    {
        if (string.IsNullOrWhiteSpace(m_iptFilter.text))
        {
            filteredParamKeys = paramKeys;
        }
        else
        {
            var filters = m_iptFilter.text.Split(' ').Where(s => !string.IsNullOrEmpty(s)).Select(s => s.ToLower());
            filteredParamKeys = paramKeys.Where(x => filters.All(f => x.name.ToLower().Contains(f))).ToList();
        }

        if (m_toggleFilter.isOn)
        {
            filteredParamKeys = filteredParamKeys.Where(x => m_motionData.TryGetTrack(x.name, true).keyFrames.Count > 0).ToList();
        }

        m_sliderV.maxValue = filteredParamKeys.Count;
    }

    public void RefreshMotionTrackHeader()
    {
        if (GetValidTarget() == null)
        {
            return;
        }

        UpdateFilter();
        var headerIndex = (int)m_sliderV.value;
        var headerCount = Mathf.Min(filteredParamKeys.Count - headerIndex, MAX_TRACK_DISPLAY_COUNT);
        SetListItem(m_listMotionTrackHeader, m_itemTrackHeader.gameObject, m_tfTrackHeaderRoot, headerCount, OnMotionTrackHeaderItemCreate);
        for (int i = 0; i < headerCount; i++)
        {
            var paramInfo = filteredParamKeys[headerIndex + i];
            if (!m_motionData.info.TryGetKeyFrameValue(paramInfo.name, curFrameIndex, out float value))
            {
                value = paramInfo.value;
            }
            var track = m_motionData.TryGetTrack(paramInfo.name, true);
            bool hasKeyFrameInIndex = track.HasKeyFrame(curFrameIndex);
            bool hasKeyFrames = track.keyFrames.Count > 0;
            m_listMotionTrackHeader[i].SetData(paramInfo, value, hasKeyFrameInIndex, hasKeyFrames);
        }
    }

    private void OnMotionTrackHeaderItemCreate(MotionTrackHeaderWidget widget)
    {
        widget._OnSliderValueChange += OnMotionTrackHeaderSliderValueChange;
        widget._OnInputFieldValueEndEdit += OnMotionTrackHeaderInputFieldValueEndEdit;
        widget._OnButtonStatusClick += OnMotionTrackHeaderButtonStatusClick;
    }

    private Live2DParamInfo GetParamInfo(string paramName)
    {
        foreach (var param in paramKeys)
        {
            if (param.name == paramName)
            {
                return param;
            }
        }

        return null;
    }

    public void SetTrackValue(string paramName, int frameIndex, float value)
    {
        var paramInfo = GetParamInfo(paramName);
        if (paramInfo == null)
        {
            return;
        }

        var track = m_motionData.TryGetTrack(paramName, true);
        var finalValue = Mathf.Clamp(value, paramInfo.min, paramInfo.max);
        if (!track.keyFrames.ContainsKey(0))
        {
            track.keyFrames[0] = finalValue;
        }
        track.keyFrames[frameIndex] = finalValue;
        m_motionData.BakeFrames(paramName);

        RefreshAll();
    }

    public void RemoveTrackValue(string paramName, int frameIndex)
    {
        var track = m_motionData.TryGetTrack(paramName, true);
        track.keyFrames.Remove(frameIndex);
        m_motionData.BakeFrames(paramName);

        RefreshAll();
    }

    private void OnMotionTrackHeaderSliderValueChange(MotionTrackHeaderWidget widget, float value)
    {
        SetTrackValue(widget.info.name, curFrameIndex, value);
    }

    private void OnMotionTrackHeaderInputFieldValueEndEdit(MotionTrackHeaderWidget widget, string value)
    {
        if (float.TryParse(value, out float val))
        {
            SetTrackValue(widget.info.name, curFrameIndex, val);
        }
    }

    private void OnMotionTrackHeaderButtonStatusClick(MotionTrackHeaderWidget widget)
    {
        var track = m_motionData.TryGetTrack(widget.info.name, true);
        if (track.HasKeyFrame(curFrameIndex))
        {
            RemoveTrackValue(widget.info.name, curFrameIndex);
        }
        else
        {
            if (!m_motionData.info.TryGetKeyFrameValue(widget.info.name, curFrameIndex, out float value))
            {
                value = widget.info.value;
            }
            SetTrackValue(widget.info.name, curFrameIndex, value);
        }
    }

    public void RefreshMotionTrack()
    {
        var trackIndex = (int)m_sliderV.value;
        var frameIndex = (int)m_sliderH.value;
        var trackCount = Mathf.Min(filteredParamKeys.Count - trackIndex, MAX_TRACK_DISPLAY_COUNT);
        SetListItem(m_listMotionTrack, m_itemTrack.gameObject, m_tfTrackRoot, trackCount, OnMotionTrackItemCreate);
        for (int i = 0; i < trackCount; i++)
        {
            m_listMotionTrack[i].SetData(m_motionData.TryGetTrack(filteredParamKeys[trackIndex + i].name), frameIndex);
        }
        RefreshTrackLabels();
    }

    private void OnMotionTrackItemCreate(MotionTrackWidget widget)
    {
    }

}

public class MotionTrackHeaderWidget : UIItemWidget<MotionTrackHeaderWidget>
{
    #region auto generated members
    private Text m_lblTitle;
    private Slider m_sliderValue;
    private InputField m_iptValue;
    private Button m_btnStatus;
    private MonoKeyUIStyle m_keystyleButton;
    private GameObject m_goMask;
    #endregion

    #region auto generated binders
    protected override void CodeGenBindMembers()
    {
        m_lblTitle = transform.Find("m_lblTitle").GetComponent<Text>();
        m_sliderValue = transform.Find("m_sliderValue").GetComponent<Slider>();
        m_iptValue = transform.Find("m_iptValue").GetComponent<InputField>();
        m_btnStatus = transform.Find("m_btnStatus").GetComponent<Button>();
        m_keystyleButton = transform.Find("m_keystyleButton").GetComponent<MonoKeyUIStyle>();
        m_goMask = transform.Find("m_goMask").gameObject;

        m_sliderValue.onValueChanged.AddListener(OnSliderValueChange);
        m_iptValue.onValueChanged.AddListener(OnInputFieldValueChange);
        m_iptValue.onEndEdit.AddListener(OnInputFieldValueEndEdit);
        m_btnStatus.onClick.AddListener(OnButtonStatusClick);
    }
    #endregion


    #region auto generated events
    private void OnSliderValueChange(float value)
    {
        if (settingValues)
        {
            return;
        }
        
        _OnSliderValueChange?.Invoke(this, value);
    }
    private void OnInputFieldValueChange(string value)
    {
    }
    private void OnInputFieldValueEndEdit(string value)
    {
        _OnInputFieldValueEndEdit?.Invoke(this, value);
    }
    private void OnButtonStatusClick()
    {
        _OnButtonStatusClick?.Invoke(this);
    }
    #endregion

    public Live2DParamInfo info;
    public Action<MotionTrackHeaderWidget, float> _OnSliderValueChange;
    public Action<MotionTrackHeaderWidget, string> _OnInputFieldValueEndEdit;
    public Action<MotionTrackHeaderWidget> _OnButtonStatusClick;

    bool settingValues = false;

    public void SetData(Live2DParamInfo info, float value, bool hasKeyFrameInIndex, bool hasKeyFrames)
    {
        this.info = info;
        m_lblTitle.text = info.name;
        settingValues = true;
        m_sliderValue.minValue = info.min;
        m_sliderValue.maxValue = info.max;
        settingValues = false;
        SetValue(value);
        m_keystyleButton.style.ApplyObject(hasKeyFrameInIndex ? "has_key" : "no_key");
        m_goMask.SetActive(!hasKeyFrames);
    }

    public void SetValue(float value)
    {
        m_sliderValue.SetValueWithoutNotify(value);
        m_iptValue.SetTextWithoutNotify(value.ToString());
    }
}

public class MotionTrackWidget : UIItemWidget<MotionTrackWidget>
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

    private List<Button> m_buttons = new List<Button>();
    public Action<Live2dMotionData.Track, int> _OnDotClicked;
    protected override void OnInit()
    {
        base.OnInit();
        m_buttons.AddRange(gameObject.GetComponentsInChildren<Button>());
        for (int i = 0; i < m_buttons.Count; i++)
        {
            Button button = m_buttons[i];
            button.onClick.AddListener(() => OnButtonClick(i));
        }
    }

    private void OnButtonClick(int index)
    {
        _OnDotClicked?.Invoke(track, startIndex + index);
    }

    public Live2dMotionData.Track track;
    public int startIndex;
    public void SetData(Live2dMotionData.Track track, int startIndex)
    {
        this.track = track;
        this.startIndex = startIndex;

        for (int i = 0; i < m_buttons.Count; i++)
        {
            int frame = startIndex + i;
            Button button = m_buttons[i];
            bool hasKeyFrame = track != null && track.HasKeyFrame(frame);
            button.gameObject.SetActive(hasKeyFrame);
        }
    }
}

public class PageNavMotionLabelWidget : UIItemWidget<PageNavMotionLabelWidget>
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

    private List<Text> m_labels = new List<Text>();
    private int startIndex;
    protected override void OnInit()
    {
        base.OnInit();
        m_labels.AddRange(gameObject.GetComponentsInChildren<Text>(true));
    }

    public void SetData(int index)
    {
        this.startIndex = index;
        for (int i = 0; i < m_labels.Count; i++)
        {
            m_labels[i].text = (index + i + 1).ToString();
        }
    }

    public bool GetLabelPosition(int frameIndex, out Vector2 position)
    {
        int min = startIndex;
        int max = startIndex + 30;
        if (frameIndex < min || frameIndex > max)
        {
            position = Vector2.zero;
            return false;
        }

        position = m_labels[frameIndex - startIndex].rectTransform.position;
        return true;
    }

    public int GetRelativeLabelByPosition(Vector2 position)
    {
        float minDistance = float.MaxValue;
        int minIndex = 0;
        for (int i = 0; i < m_labels.Count; i++)
        {
            var dist = Vector2.Distance(position, m_labels[i].rectTransform.position);
            if (dist < minDistance)
            {
                minDistance = dist;
                minIndex = i;
            }
        }
        return minIndex + startIndex;
    }
}
