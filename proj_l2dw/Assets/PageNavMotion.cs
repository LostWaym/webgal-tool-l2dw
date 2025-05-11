

using System;
using System.Collections.Generic;
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
    private Button m_btnFilter;
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
        m_btnFilter = transform.Find("TimelineArea/ToolBar/Left/Bottom/m_btnFilter").GetComponent<Button>();
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
        m_btnFilter.onClick.AddListener(OnButtonFilterClick);
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
        Debug.Log("OnButtonApplyClick");
    }
    private void OnButtonEditClick()
    {
        Debug.Log("OnButtonEditClick");
    }
    private void OnButtonSaveClick()
    {
        Debug.Log("OnButtonSaveClick");
    }
    private void OnButtonRecordClick()
    {
        Debug.Log("OnButtonRecordClick");
    }
    private void OnButtonOperationClick()
    {
        Debug.Log("OnButtonOperationClick");
    }
    private void OnButtonFilterClick()
    {
        Debug.Log("OnButtonFilterClick");
    }
    private void OnInputFieldFilterChange(string value)
    {
        Debug.Log("OnInputFieldFilterChange");
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
        Debug.Log("OnInputFieldDurationEndEdit");
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
            SetFrameIndex(frame);
        }
    }
    private void OnSliderHChange(float value)
    {
        RefreshMotionTrack();
    }
    private void OnSliderVChange(float value)
    {
        RefreshMotionTrackHeader();
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
        m_iptFrame.SetTextWithoutNotify(curFrameIndex.ToString());
        RefreshFrameLine();
        SampleFrame();
        RefreshMotionTrackHeader();
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

        var data = curTarget.MyGOConfig.motions["idle01"];
        // 转换byte[]为string
        string text = System.Text.Encoding.UTF8.GetString(data);
        m_motionData = Live2dMotionData.Create(text);

        paramKeys = curTarget.GetEmotionEditorList().list;
        RefreshMotionTrack();
        RefreshMotionTrackHeader();
        RefreshSlider();
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
        m_sliderV.maxValue = paramKeys.Count - MAX_TRACK_DISPLAY_COUNT;

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

    public void RefreshMotionTrackHeader()
    {
        if (GetValidTarget() == null)
        {
            return;
        }

        var headerIndex = (int)m_sliderV.value;
        var headerCount = Mathf.Min(paramKeys.Count - headerIndex, MAX_TRACK_DISPLAY_COUNT);
        SetListItem(m_listMotionTrackHeader, m_itemTrackHeader.gameObject, m_tfTrackHeaderRoot, headerCount, OnMotionTrackHeaderItemCreate);
        for (int i = 0; i < headerCount; i++)
        {
            var paramInfo = paramKeys[headerIndex + i];
            if (!m_motionData.info.TryGetKeyFrameValue(paramInfo.name, curFrameIndex, out float value))
            {
                value = paramInfo.value;
            }
            m_listMotionTrackHeader[i].SetData(paramInfo, value);
        }

        RefreshMotionTrack();
    }

    private void OnMotionTrackHeaderItemCreate(MotionTrackHeaderWidget widget)
    {
        widget._OnSliderValueChange += OnMotionTrackHeaderSliderValueChange;
        widget._OnInputFieldValueEndEdit += OnMotionTrackHeaderInputFieldValueEndEdit;
        widget._OnButtonStatusClick += OnMotionTrackHeaderButtonStatusClick;
    }

    private void OnMotionTrackHeaderSliderValueChange(MotionTrackHeaderWidget widget, float value)
    {
        Debug.Log("OnMotionTrackHeaderSliderValueChange");
    }

    private void OnMotionTrackHeaderInputFieldValueEndEdit(MotionTrackHeaderWidget widget, string value)
    {
        Debug.Log("OnMotionTrackHeaderInputFieldValueEndEdit");
    }

    private void OnMotionTrackHeaderButtonStatusClick(MotionTrackHeaderWidget widget)
    {
        Debug.Log("OnMotionTrackHeaderButtonStatusClick");
    }
    public void RefreshMotionTrack()
    {
        var trackIndex = (int)m_sliderV.value;
        var frameIndex = (int)m_sliderH.value;
        var trackCount = Mathf.Min(paramKeys.Count - trackIndex, MAX_TRACK_DISPLAY_COUNT);
        SetListItem(m_listMotionTrack, m_itemTrack.gameObject, m_tfTrackRoot, trackCount, OnMotionTrackItemCreate);
        for (int i = 0; i < trackCount; i++)
        {
            m_listMotionTrack[i].SetData(m_motionData.TryGetTrack(paramKeys[trackIndex + i].name), frameIndex);
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
    #endregion

    #region auto generated binders
    protected override void CodeGenBindMembers()
    {
        m_lblTitle = transform.Find("m_lblTitle").GetComponent<Text>();
        m_sliderValue = transform.Find("m_sliderValue").GetComponent<Slider>();
        m_iptValue = transform.Find("m_iptValue").GetComponent<InputField>();
        m_btnStatus = transform.Find("m_btnStatus").GetComponent<Button>();

        m_sliderValue.onValueChanged.AddListener(OnSliderValueChange);
        m_iptValue.onValueChanged.AddListener(OnInputFieldValueChange);
        m_iptValue.onEndEdit.AddListener(OnInputFieldValueEndEdit);
        m_btnStatus.onClick.AddListener(OnButtonStatusClick);
    }
    #endregion

    #region auto generated events
    private void OnSliderValueChange(float value)
    {
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

    public void SetData(Live2DParamInfo info, float value)
    {
        this.info = info;
        m_lblTitle.text = info.name;
        m_sliderValue.minValue = info.min;
        m_sliderValue.maxValue = info.max;
        SetValue(value);
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
