

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class PageNavMotion : UIPageWidget<PageNavMotion>
{
    public static float dot_space = 16;
    public static float dot_padding = 8;
    public static int dot_count = 5;

    #region auto generated members
    private Button m_btnDelete;
    private Button m_btnMotion;
    private Text m_lblMotion;
    private RectTransform m_rectSelectMotionArea;
    private Button m_btnApply;
    private Button m_btnEdit;
    private Button m_btnSave;
    private RawImage m_rawChara;
    private Button m_btnRecord;
    private Button m_btnOperation;
    private RectTransform m_rectOperationTitleArea;
    private RectTransform m_rectOperationContentArea;
    private Toggle m_toggleFilter;
    private InputField m_iptFilter;
    private Button m_btnClearFilter;
    private Toggle m_toggleDotSheetMode;
    private Toggle m_toggleCurveMode;
    private Button m_btnNavHome;
    private Button m_btnNavLeft;
    private Button m_btnPlay;
    private Button m_btnNavRight;
    private Button m_btnNavEnd;
    private InputField m_iptDuration;
    private InputField m_iptFrame;
    private RectTransform m_rectLabels;
    private Text m_lblLabelIndex;
    private TouchArea m_touchLabels;
    private InputField m_iptFrameIndex;
    private GameObject m_goLeft;
    private Transform m_tfTrackHeaderRoot;
    private Transform m_itemTrackHeader;
    private GameObject m_goRight;
    private Transform m_tfTrackRoot;
    private Transform m_itemTrack;
    private TouchArea m_touchTrackArea;
    private Image m_imgRect;
    private Image m_imgLine;
    private Slider m_sliderH;
    private Slider m_sliderV;
    #endregion

    #region auto generated binders
    protected override void CodeGenBindMembers()
    {
        m_btnDelete = transform.Find("CharaArea/ToolBar/m_btnDelete").GetComponent<Button>();
        m_btnMotion = transform.Find("CharaArea/ToolBar/m_btnMotion").GetComponent<Button>();
        m_lblMotion = transform.Find("CharaArea/ToolBar/m_btnMotion/m_lblMotion").GetComponent<Text>();
        m_rectSelectMotionArea = transform.Find("CharaArea/ToolBar/m_btnMotion/m_rectSelectMotionArea").GetComponent<RectTransform>();
        m_btnApply = transform.Find("CharaArea/ToolBar/m_btnApply").GetComponent<Button>();
        m_btnEdit = transform.Find("CharaArea/ToolBar/m_btnEdit").GetComponent<Button>();
        m_btnSave = transform.Find("CharaArea/ToolBar/m_btnSave").GetComponent<Button>();
        m_rawChara = transform.Find("CharaArea/m_rawChara").GetComponent<RawImage>();
        m_btnRecord = transform.Find("TimelineArea/ToolBar/Left/Top/m_btnRecord").GetComponent<Button>();
        m_btnOperation = transform.Find("TimelineArea/ToolBar/Left/Top/m_btnOperation").GetComponent<Button>();
        m_rectOperationTitleArea = transform.Find("TimelineArea/ToolBar/Left/Top/m_rectOperationTitleArea").GetComponent<RectTransform>();
        m_rectOperationContentArea = transform.Find("TimelineArea/ToolBar/Left/Top/m_rectOperationTitleArea/m_rectOperationContentArea").GetComponent<RectTransform>();
        m_toggleFilter = transform.Find("TimelineArea/ToolBar/Left/Bottom/m_toggleFilter").GetComponent<Toggle>();
        m_iptFilter = transform.Find("TimelineArea/ToolBar/Left/Bottom/m_iptFilter").GetComponent<InputField>();
        m_btnClearFilter = transform.Find("TimelineArea/ToolBar/Left/Bottom/m_iptFilter/m_btnClearFilter").GetComponent<Button>();
        m_toggleDotSheetMode = transform.Find("TimelineArea/ToolBar/Right/Top/Container/ToggleGroup/m_toggleDotSheetMode").GetComponent<Toggle>();
        m_toggleCurveMode = transform.Find("TimelineArea/ToolBar/Right/Top/Container/ToggleGroup/m_toggleCurveMode").GetComponent<Toggle>();
        m_btnNavHome = transform.Find("TimelineArea/ToolBar/Right/Top/Container/GameObject/m_btnNavHome").GetComponent<Button>();
        m_btnNavLeft = transform.Find("TimelineArea/ToolBar/Right/Top/Container/GameObject/m_btnNavLeft").GetComponent<Button>();
        m_btnPlay = transform.Find("TimelineArea/ToolBar/Right/Top/Container/GameObject/m_btnPlay").GetComponent<Button>();
        m_btnNavRight = transform.Find("TimelineArea/ToolBar/Right/Top/Container/GameObject/m_btnNavRight").GetComponent<Button>();
        m_btnNavEnd = transform.Find("TimelineArea/ToolBar/Right/Top/Container/GameObject/m_btnNavEnd").GetComponent<Button>();
        m_iptDuration = transform.Find("TimelineArea/ToolBar/Right/Top/Container/m_iptDuration").GetComponent<InputField>();
        m_iptFrame = transform.Find("TimelineArea/ToolBar/Right/Top/Container/m_iptFrame").GetComponent<InputField>();
        m_rectLabels = transform.Find("TimelineArea/ToolBar/Right/Bottom/m_rectLabels").GetComponent<RectTransform>();
        m_lblLabelIndex = transform.Find("TimelineArea/ToolBar/Right/Bottom/m_rectLabels/m_lblLabelIndex").GetComponent<Text>();
        m_touchLabels = transform.Find("TimelineArea/ToolBar/Right/Bottom/m_touchLabels").GetComponent<TouchArea>();
        m_iptFrameIndex = transform.Find("TimelineArea/ToolBar/Right/Bottom/m_iptFrameIndex").GetComponent<InputField>();
        m_goLeft = transform.Find("TimelineArea/Bottom/m_goLeft").gameObject;
        m_tfTrackHeaderRoot = transform.Find("TimelineArea/Bottom/m_goLeft/m_tfTrackHeaderRoot").GetComponent<Transform>();
        m_itemTrackHeader = transform.Find("TimelineArea/Bottom/m_goLeft/m_tfTrackHeaderRoot/m_itemTrackHeader").GetComponent<Transform>();
        m_goRight = transform.Find("TimelineArea/Bottom/m_goRight").gameObject;
        m_tfTrackRoot = transform.Find("TimelineArea/Bottom/m_goRight/m_tfTrackRoot").GetComponent<Transform>();
        m_itemTrack = transform.Find("TimelineArea/Bottom/m_goRight/m_tfTrackRoot/m_itemTrack").GetComponent<Transform>();
        m_touchTrackArea = transform.Find("TimelineArea/Bottom/m_goRight/m_touchTrackArea").GetComponent<TouchArea>();
        m_imgRect = transform.Find("TimelineArea/Bottom/m_goRight/m_touchTrackArea/m_imgRect").GetComponent<Image>();
        m_imgLine = transform.Find("TimelineArea/Bottom/m_goRight/m_imgLine").GetComponent<Image>();
        m_sliderH = transform.Find("TimelineArea/Bottom/m_goRight/m_sliderH").GetComponent<Slider>();
        m_sliderV = transform.Find("TimelineArea/Bottom/m_goRight/m_sliderV").GetComponent<Slider>();

        m_btnDelete.onClick.AddListener(OnButtonDeleteClick);
        m_btnMotion.onClick.AddListener(OnButtonMotionClick);
        m_btnApply.onClick.AddListener(OnButtonApplyClick);
        m_btnEdit.onClick.AddListener(OnButtonEditClick);
        m_btnSave.onClick.AddListener(OnButtonSaveClick);
        m_btnRecord.onClick.AddListener(OnButtonRecordClick);
        m_btnOperation.onClick.AddListener(OnButtonOperationClick);
        m_toggleFilter.onValueChanged.AddListener(OnToggleFilterChange);
        m_iptFilter.onValueChanged.AddListener(OnInputFieldFilterChange);
        m_iptFilter.onEndEdit.AddListener(OnInputFieldFilterEndEdit);
        m_btnClearFilter.onClick.AddListener(OnButtonClearFilterClick);
        m_toggleDotSheetMode.onValueChanged.AddListener(OnToggleDotSheetModeChange);
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
    private void OnButtonDeleteClick()
    {
        var motionName = m_motionData.motionDataName;
        ConfirmUI.Instance.SetData($"确定要删除{motionName}吗？", "删除后将无法恢复", () =>
        {
            m_listMotionData.Remove(m_motionData);
            if (m_listMotionData.Count > 0)
            {
                SelectMotionData(m_listMotionData[0]);
            }
            else
            {
                SelectOrCreateDefaultMotionData();
            }
            RefreshAll();
        }, null);
    }
    private void OnButtonMotionClick()
    {
        OpenMotionDataSelectDialog();
    }
    private void OnButtonApplyClick()
    {
        OpenLoadMotionDialog();
    }
    private void OnButtonEditClick()
    {
        OpenMotionProfileDialog();
    }

    #region 保存按钮相关
    private const string SAVE_OPT_SAVE_PROJ_TO_CLIPBOARD = "保存工程到剪贴板";
    private const string SAVE_OPT_LOAD_PROJ_FROM_CLIPBOARD = "从剪贴板加载覆盖当前工程";
    private const string SAVE_OPT_SAVE_MOTION_TO_CLIPBOARD = "保存motion到剪贴板";
    private List<string> m_listSaveOpt = new List<string>()
    {
        SAVE_OPT_SAVE_PROJ_TO_CLIPBOARD,
        SAVE_OPT_LOAD_PROJ_FROM_CLIPBOARD,
        SAVE_OPT_SAVE_MOTION_TO_CLIPBOARD,
    };
    private void OnButtonSaveClick()
    {
        TargetSelectUI.Instance.SetData(m_rectOperationTitleArea, m_rectOperationContentArea, m_listSaveOpt);
        TargetSelectUI.Instance._OnTargetItemClick = (operationName) =>
        {
            TargetSelectUI.Instance.Close();
            switch (operationName)
            {
                case SAVE_OPT_SAVE_PROJ_TO_CLIPBOARD:
                    DoSaveProjToClipboard();
                    break;
                case SAVE_OPT_LOAD_PROJ_FROM_CLIPBOARD:
                    DoLoadProjFromClipboard();
                    break;
                case SAVE_OPT_SAVE_MOTION_TO_CLIPBOARD:
                    DoSaveMotionToClipboard();
                    break;
            }
        };
    }
    private void DoSaveProjToClipboard()
    {
        var json = m_motionData.Save();
        GUIUtility.systemCopyBuffer = json;
        Debug.Log(json);
    }
    private void DoLoadProjFromClipboard()
    {
        var curTarget = GetValidTarget();
        if (curTarget == null)
        {
            return;
        }
        var json = GUIUtility.systemCopyBuffer;
        m_motionData.Load(json);
        curTarget.SetDisplayMode(ModelDisplayMode.MotionEditor, true);
        CheckAndFixName(m_motionData);
        SelectMotionData(m_motionData);
        RefreshAll();
    }
    private void DoSaveMotionToClipboard()
    {
        m_motionData.BakeAllFrames();
        var text = m_motionData.info.Print();
        Debug.Log(text);
        GUIUtility.systemCopyBuffer = text;
    }
    #endregion

    private void OnButtonRecordClick()
    {
        Debug.Log("OnButtonRecordClick");
    }
    private const string OPERATION_LINEAR_UNBAKE = "线性反烘焙";
    private const string OPERATION_DELETE_SELECTED_DOT = "删除框选点";
    private const string OPERATION_CLONE_SELECTED_DOT = "复制框选点到当前帧（不建议使用）";
    private const string OPERATION_CACHE_CUR_FRAME = "缓存当前帧";
    private const string OPERATION_RESTORE_CUR_FRAME = "缓存帧覆盖到当前帧";
    private const string OPERATION_CACHE_SELECTED_DOTS = "缓存框选点（可跨动画工程）";
    private const string OPERATION_RESTORE_SELECTED_DOTS = "缓存框选点覆盖到当前帧";
    private List<string> m_listOperation = new List<string>()
    {
        OPERATION_LINEAR_UNBAKE,
        OPERATION_DELETE_SELECTED_DOT,
        OPERATION_CLONE_SELECTED_DOT,
        // OPERATION_CACHE_CUR_FRAME,
        // OPERATION_RESTORE_CUR_FRAME,
        OPERATION_CACHE_SELECTED_DOTS,
        OPERATION_RESTORE_SELECTED_DOTS,
    };
    private void OnButtonOperationClick()
    {
        var curTarget = GetValidTarget();
        if (curTarget == null)
        {
            return;
        }

        TargetSelectUI.Instance.SetData(m_rectOperationTitleArea, m_rectOperationContentArea, m_listOperation);
        TargetSelectUI.Instance._OnTargetItemClick = (operationName) =>
        {
            TargetSelectUI.Instance.Close();
            switch (operationName)
            {
                case OPERATION_LINEAR_UNBAKE:
                    DoLinearUnbake();
                    break;
                case OPERATION_DELETE_SELECTED_DOT:
                    DoDeleteSelectedDot();
                    break;
                case OPERATION_CLONE_SELECTED_DOT:
                    DoCloneSelectedDot();
                    break;
                case OPERATION_CACHE_CUR_FRAME:
                    DoCacheCurFrame();
                    break;
                case OPERATION_RESTORE_CUR_FRAME:
                    DoRestoreCurFrame();
                    break;
                case OPERATION_CACHE_SELECTED_DOTS:
                    DoCacheSelectedDots();
                    break;
                case OPERATION_RESTORE_SELECTED_DOTS:
                    DoRestoreSelectedDots();
                    break;
            }
        };
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
    private void OnButtonClearFilterClick()
    {
        m_iptFilter.SetTextWithoutNotify("");
        RefreshAll();
    }
    private void OnToggleDotSheetModeChange(bool value)
    {
        Debug.Log($"OnToggleDotSheetModeChange:{value}");
    }
    private void OnToggleCurveModeChange(bool value)
    {
        Debug.Log($"OnToggleCurveModeChange:{value}");
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
    private PageNavMotionLabelBarWidget m_pageNavMotionLabel;
    private List<Live2DParamInfo> paramKeys = new List<Live2DParamInfo>();

    private Live2dMotionData m_motionData;
    private List<Live2dMotionData> m_listMotionData;

    public int MAX_TRACK_DISPLAY_COUNT
    {
        get
        {
            var rect = m_tfTrackHeaderRoot.GetComponent<RectTransform>();
            var size = rect.rect.height;
            var height = m_itemTrackHeader.GetComponent<RectTransform>().sizeDelta.y;
            int count = Mathf.FloorToInt((size / height) + 0.5f);
            return count;
        }
    }
    public int MAX_FRAME_DISPLAY_COUNT {
        get
        {
            return (int)(m_rectLabels.rect.width / PageNavMotion.dot_space);
        }
    }

    public int curFrameIndex = 0;

    protected override void OnInit()
    {
        m_pageNavMotionLabel = PageNavMotionLabelBarWidget.CreateWidget(m_rectLabels.gameObject);

        m_imgRect.gameObject.SetActive(false);

        m_touchLabels._OnPointerMove += OnTouchLabelsPointerMove;
        m_touchTrackArea._OnPointerMove += OnTouchTrackAreaPointerMove;
        m_touchTrackArea._OnPointerUp += OnTouchTrackAreaPointerUp;
        m_touchTrackArea._OnPointerDown += OnTouchTrackAreaPointerDown;
    }

    private Vector2 m_selectStartPosition;
    private void OnTouchTrackAreaPointerDown(Vector2 vector)
    {
        m_imgRect.gameObject.SetActive(true);
        m_selectStartPosition = vector;
    }

    private void OnTouchTrackAreaPointerMove(Vector2 vector)
    {
        m_imgRect.gameObject.SetActive(true);
        var pos1 = m_selectStartPosition;
        var pos2 = vector;
        var width = Mathf.Abs(pos1.x - pos2.x);
        var height = Mathf.Abs(pos1.y - pos2.y);
        m_imgRect.rectTransform.sizeDelta = new Vector2(width, height);
        m_imgRect.rectTransform.position = Vector2.Lerp(pos1, pos2, 0.5f);
    }

    private void OnTouchTrackAreaPointerUp(Vector2 vector)
    {
        m_imgRect.gameObject.SetActive(false);
        SelectDotArea();
    }

    public static Dictionary<string, HashSet<int>> s_selectedDotIndexes = new Dictionary<string, HashSet<int>>();

    private void SelectDotArea()
    {
        var curTarget = GetValidTarget();
        if (curTarget == null)
        {
            return;
        }

        if (!Input.GetKey(KeyCode.LeftControl))
        {
            s_selectedDotIndexes.Clear();
        }

        HashSet<int> TryGetSet(string key)
        {
            if (s_selectedDotIndexes.TryGetValue(key, out var set))
            {
                return set;
            }

            set = new HashSet<int>();
            s_selectedDotIndexes[key] = set;
            return set;
        }

        var selectedRect = m_imgRect.rectTransform;
        var selectedArea = new Rect((Vector2)selectedRect.position - selectedRect.rect.size / 2, selectedRect.rect.size);
        for (int i = 0; i < TrackItemCount; i++)
        {
            MotionTrackWidget track = m_listMotionTrack[i];
            for (int j = 0; j < track.DotFrameCount; j++)
            {
                var dot = track.m_dots[j];
                var dotWorldPos = dot.rectTransform.position;
                var dotScreenPos = RectTransformUtility.WorldToScreenPoint(null, dotWorldPos);
                if (selectedArea.Contains(dotScreenPos))
                {
                    var frameIndex = dot.frameIndex;
                    var set = TryGetSet(track.track.name);
                    set.Add(frameIndex);
                }
            }
        }

        RefreshAll();
    }

    public static bool IsDotSelected(string key, int frameIndex)
    {
        if (s_selectedDotIndexes.TryGetValue(key, out var set))
        {
            return set.Contains(frameIndex);
        }
        return false;
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
        if (curTarget == null || !curTarget.SupportAnimationMode)
        {
            m_rawChara.texture = null;
            m_listMotionData = null;
            RefreshAll();
            return;
        }

        curTarget.SetDisplayMode(ModelDisplayMode.MotionEditor);
        m_listMotionData = curTarget.motionDataList;
        SelectOrCreateDefaultMotionData();
        paramKeys = curTarget.GetEmotionEditorList().list;
        m_rawChara.texture = curTarget.GetCharaTexture();
        RefreshAll();
    }

    private void AddMotionData(Live2dMotionData motionData)
    {
        m_listMotionData.Add(motionData);
        CheckAndFixName(motionData);
    }

    private void CheckAndFixName(Live2dMotionData motionData)
    {
        motionData.motionDataName = motionData.motionDataName.Trim();
        var sameNameCount = m_listMotionData.Count(x => x.motionDataName == motionData.motionDataName);
        if (sameNameCount > 1)
        {
            motionData.motionDataName = $"{motionData.motionDataName} ({motionData.GetHashCode()})";
        }
    }

    private bool motionDataDirty = false;
    private void SelectMotionData(Live2dMotionData motionData)
    {
        if (m_motionData != null)
        {
            m_motionData.m_state_curFrameIndex = curFrameIndex;
        }

        m_motionData = motionData;
        curFrameIndex = motionData.m_state_curFrameIndex;
        m_lblMotion.text = $"> {motionData.motionDataName}";
        motionDataDirty = true;
    }

    private void SelectOrCreateDefaultMotionData()
    {
        if (m_listMotionData.Count == 0)
        {
            var motionData = Live2dMotionData.Create();
            AddMotionData(motionData);
            SelectMotionData(motionData);
        }
        else
        {
            SelectMotionData(m_listMotionData[0]);
        }
    }

    public override void OnPageHidden()
    {
        base.OnPageHidden();
        MainControl.Instance.UpdateBeat -= Update;

        var curTarget = GetValidTarget();
        if (curTarget != null)
        {
            curTarget.SetDisplayMode(ModelDisplayMode.Normal);
        }
    }

    public bool isPlaying;
    public float startTime;
    public int startFrameIndex;

    private int m_lastFrameDisplayCount;
    private int m_lastTrackDisplayCount;
    private void Update()
    {
        if (isPlaying)
        {
            var fps = m_motionData.info.fps;
            var delta = Time.time - startTime;
            int frameIndex = startFrameIndex + (int)(delta * fps);
            SetFrameIndex(frameIndex, false);
        }
        
        if (MAX_FRAME_DISPLAY_COUNT != m_lastFrameDisplayCount || MAX_TRACK_DISPLAY_COUNT != m_lastTrackDisplayCount)
        {
            m_lastFrameDisplayCount = MAX_FRAME_DISPLAY_COUNT;
            m_lastTrackDisplayCount = MAX_TRACK_DISPLAY_COUNT;
            RefreshAll();
        }

        var wheel = Input.GetAxis("Mouse ScrollWheel");
        if (wheel != 0)
        {
            if (Input.GetKey(KeyCode.LeftControl))
            {
                if (wheel != 0)
                {
                    dot_space += wheel * 10;
                    dot_space = Mathf.Clamp(dot_space, 4, 32);
                    m_lastFrameDisplayCount = MAX_FRAME_DISPLAY_COUNT;
                    m_lastTrackDisplayCount = MAX_TRACK_DISPLAY_COUNT;
                    RefreshAll();
                }
            }
            else
            {
                //如果鼠标位置在m_goLeft上
                if (RectTransformUtility.RectangleContainsScreenPoint(m_goLeft.GetComponent<RectTransform>(), Input.mousePosition))
                {
                    m_sliderV.value -= wheel * 10;
                }
                else if (RectTransformUtility.RectangleContainsScreenPoint(m_goRight.GetComponent<RectTransform>(), Input.mousePosition))
                {
                    int sign = wheel >= 0 ? 1 : -1;
                    float moveValue = Mathf.Abs(wheel) * MAX_FRAME_DISPLAY_COUNT;
                    int value = Mathf.Max(1, (int)moveValue);
                    m_sliderH.value += value * sign;
                }
            }
        }
    }

    private void OpenMotionDataSelectDialog()
    {
        var motionNames = m_listMotionData.Select(x => x.motionDataName).ToList();
        TargetSelectUI.Instance.SetData(m_btnMotion.gameObject.GetComponent<RectTransform>(), m_rectSelectMotionArea, motionNames);
        TargetSelectUI.Instance._OnTargetItemClick = (motionName) =>
        {
            TargetSelectUI.Instance.Close();
            var motionData = m_listMotionData.FirstOrDefault(x => x.motionDataName == motionName);
            if (motionData != null)
            {
                SelectMotionData(motionData);
                RefreshAll();
            }
        };
    }

    private void OpenMotionProfileDialog()
    {
        MotionDataSettingUI.Instance.SetData(m_motionData, (motionData) =>
        {
            CheckAndFixName(motionData);
            SelectMotionData(motionData);
            RefreshAll();
        });
        MotionDataSettingUI.Instance.Show();
    }

    private void OpenLoadMotionDialog()
    {
        var curTarget = GetValidTarget();
        if (curTarget == null)
        {
            return;
        }
        var motionNames = curTarget.MyGOConfig.motions.Keys.ToList().OrderBy(x => x).ToList();
        motionNames.Insert(0, "空白工程");
        TargetSelectUI.Instance.SetData(m_btnMotion.gameObject.GetComponent<RectTransform>(), m_rectSelectMotionArea, motionNames);
        TargetSelectUI.Instance._OnTargetItemClick = (motionName) =>
        {
            bool isEmptyMotion = motionName == "空白工程";
            void OnSubmit()
            {
                TargetSelectUI.Instance.Close();
                if (!isEmptyMotion)
                {
                    var bytes = curTarget.MyGOConfig.motions[motionName];
                    string text = System.Text.Encoding.UTF8.GetString(bytes);
                    var motionData = Live2dMotionData.Create(text);
                    motionData.motionDataName = $"{motionName} (clone)";
                    AddMotionData(motionData);
                    SelectMotionData(motionData);
                    RefreshAll();
                }
                else
                {
                    var motionData = Live2dMotionData.Create();
                    AddMotionData(motionData);
                    SelectMotionData(motionData);
                    RefreshAll();
                }
            }

            // 保留该注释
            // ConfirmUI.Instance.SetData("确定要导入motion吗？", "motion文件将替换当前motion\n请注意好保存数据！", OnSubmit, null);
            OnSubmit();
        };
    }

    #region 操作

    private class CacheEntry
    {
        public string trackName;
        public int frameIndex;
        public float value;
    }

    private List<CacheEntry> m_cacheEntries = new List<CacheEntry>();

    private void DoCacheCurFrame()
    {
        var curTarget = GetValidTarget();
        if (curTarget == null)
        {
            return;
        }

        m_cacheEntries.Clear();
        foreach (var entry in m_motionData.tracks)
        {
            var track = entry.Value;
            bool hasKeyFrames = track.keyFrames.Count > 0;
            if (hasKeyFrames)
            {
                if (m_motionData.info.TryGetFrame(entry.Key, curFrameIndex, out float value))
                {
                    m_cacheEntries.Add(new CacheEntry()
                    {
                        trackName = entry.Key,
                        frameIndex = curFrameIndex,
                        value = value,
                    });
                }
            }
        }
    }
    private void DoRestoreCurFrame()
    {
        var curTarget = GetValidTarget();
        if (curTarget == null)
        {
            return;
        }

        foreach (var entry in m_cacheEntries)
        {
            var track = m_motionData.TryGetTrack(entry.trackName, false);
            if (track == null)
                continue;
            
            track.keyFrames[entry.frameIndex] = entry.value;
        }

        m_motionData.BakeAllFrames();
        RefreshAll();
    }
    private void DoLinearUnbake()
    {
        var curTarget = GetValidTarget();
        if (curTarget == null)
        {
            return;
        }
        
        m_motionData.UnBakeAllFramesByLinear();
        RefreshAll();
    }

    private void DoDeleteSelectedDot()
    {
        var curTarget = GetValidTarget();
        if (curTarget == null)
        {
            return;
        }

        foreach (var entry in s_selectedDotIndexes)
        {
            var track = m_motionData.TryGetTrack(entry.Key, false);
            if (track == null)
                continue;

            foreach (var frameIndex in entry.Value)
            {
                track.keyFrames.Remove(frameIndex);
            }
        }

        s_selectedDotIndexes.Clear();
        m_motionData.BakeAllFrames();

        RefreshAll();
    }

    private void DoCloneSelectedDot()
    {
        var curTarget = GetValidTarget();
        if (curTarget == null)
        {
            return;
        }

        Dictionary<string, Dictionary<int, float>> trackCacheValues = new Dictionary<string, Dictionary<int, float>>();
        Dictionary<int, float> TryGetDict(string key)
        {
            if (trackCacheValues.TryGetValue(key, out var dict))
            {
                return dict;
            }

            dict = new Dictionary<int, float>();
            trackCacheValues[key] = dict;
            return dict;
        }

        int minFrameIndex = int.MaxValue;
        foreach (var entry in s_selectedDotIndexes)
        {
            foreach (var frameIndex in entry.Value)
            {
                minFrameIndex = Mathf.Min(minFrameIndex, frameIndex);
            }
        }

        foreach (var entry in s_selectedDotIndexes)
        {
            var track = m_motionData.TryGetTrack(entry.Key, false);
            if (track == null)
                continue;

            var dict = TryGetDict(track.name);
            foreach (var frameIndex in entry.Value)
            {
                dict[frameIndex - minFrameIndex + curFrameIndex] = track.keyFrames[frameIndex];
            }
        }

        foreach (var entry in trackCacheValues)
        {
            var track = m_motionData.TryGetTrack(entry.Key, true);
            foreach (var entry2 in entry.Value)
            {
                var frameIndex = entry2.Key;
                var value = entry2.Value;
                track.keyFrames[frameIndex] = value;
            }
        }

        s_selectedDotIndexes.Clear();
        m_motionData.BakeAllFrames();
        RefreshAll();
    }

    public class CacheSelectedDotsEntry
    {
        public string trackName;
        public int frameIndex;
        public float value;
    }
    public List<CacheSelectedDotsEntry> m_cacheSelectedDotsEntries = new List<CacheSelectedDotsEntry>();
    private void DoCacheSelectedDots()
    {
        m_cacheSelectedDotsEntries.Clear();
        foreach (var entry in s_selectedDotIndexes)
        {
            var track = m_motionData.TryGetTrack(entry.Key, false);
            if (track == null)
                continue;

            foreach (var frameIndex in entry.Value)
            {
                m_cacheSelectedDotsEntries.Add(new CacheSelectedDotsEntry()
                {
                    trackName = track.name,
                    frameIndex = frameIndex,
                    value = track.keyFrames[frameIndex],
                });
            }
        }

        int minFrameIndex = int.MaxValue;
        foreach (var entry in m_cacheSelectedDotsEntries)
        {
            minFrameIndex = Mathf.Min(minFrameIndex, entry.frameIndex);
        }

        foreach (var entry in m_cacheSelectedDotsEntries)
        {
            entry.frameIndex -= minFrameIndex;
        }
    }

    private void DoRestoreSelectedDots()
    {
        foreach (var entry in m_cacheSelectedDotsEntries)
        {
            var track = m_motionData.TryGetTrack(entry.trackName, false);
            if (track == null)
                continue;

            track.keyFrames[curFrameIndex + entry.frameIndex] = entry.value;
        }

        m_motionData.BakeAllFrames();
        RefreshAll();
    }

    #endregion

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
        isPlaying = false;
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
        dot_count = MAX_FRAME_DISPLAY_COUNT;

        if (motionDataDirty)
        {
            motionDataDirty = false;
            curTarget.SetDisplayMode(ModelDisplayMode.MotionEditor, true);
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

        paramKeys = curTarget.GetEmotionEditorList().list;
        var data = curTarget.MyGOConfig.motions[motionName];
        // 转换byte[]为string
        string text = System.Text.Encoding.UTF8.GetString(data);
        var motionData = Live2dMotionData.Create(text);
        motionData.motionDataName = motionName;
        AddMotionData(motionData);
        SelectMotionData(motionData);
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

    private int TrackItemCount
    {
        get
        {
            return Mathf.Min(filteredParamKeys.Count - (int)m_sliderV.value, MAX_TRACK_DISPLAY_COUNT);
        }
    }
    public void RefreshMotionTrack()
    {
        var trackIndex = (int)m_sliderV.value;
        var frameIndex = (int)m_sliderH.value;
        var trackCount = TrackItemCount;
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
        m_iptValue.SetTextWithoutNotify(L2DWUtils.GetShortNumberString(value));
    }
}

public class MotionTrackWidget : UIItemWidget<MotionTrackWidget>
{
    #region auto generated members
    private Transform m_itemDot;
    #endregion

    #region auto generated binders
    protected override void CodeGenBindMembers()
    {
        m_itemDot = transform.Find("m_itemDot").GetComponent<Transform>();

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
    public List<MotionTrackDotWidget> m_dots = new List<MotionTrackDotWidget>();
    private List<int> m_dotIndexes = new List<int>();
    public int DotFrameCount => m_dotIndexes.Count;
    public void SetData(Live2dMotionData.Track track, int startIndex)
    {
        this.track = track;
        this.startIndex = startIndex;

        m_dotIndexes.Clear();
        for (int i = 0; i < PageNavMotion.dot_count; i++)
        {
            int frame = startIndex + i;
            bool hasKeyFrame = track != null && track.HasKeyFrame(frame);
            if (hasKeyFrame)
            {
                m_dotIndexes.Add(frame);
            }
        }
        
        int shownDotCount = m_dotIndexes.Count;
        SetListItem(m_dots, m_itemDot.gameObject, gameObject.transform, shownDotCount, OnDotItemCreate);
        for (int i = 0; i < shownDotCount; i++)
        {
            var dot = m_dots[i];
            var relativeIndex = m_dotIndexes[i] - startIndex;
            var pos = dot.rectTransform.anchoredPosition;
            pos.x = relativeIndex * PageNavMotion.dot_space + PageNavMotion.dot_padding;
            dot.rectTransform.anchoredPosition = pos;
            var key = track.name;
            var isSelected = PageNavMotion.IsDotSelected(key, m_dotIndexes[i]);
            dot.SetData(m_dotIndexes[i], isSelected);
        }
    }

    private void OnDotItemCreate(MotionTrackDotWidget dot)
    {
    }
}

public class MotionTrackDotWidget : UIItemWidget<MotionTrackDotWidget>
{
    public RectTransform rectTransform;
    public Image imgDot;
    public int frameIndex;
    protected override void OnInit()
    {
        base.OnInit();
        rectTransform = gameObject.GetComponent<RectTransform>();
        imgDot = gameObject.GetComponent<Image>();
    }

    public void SetData(int frameIndex, bool isSelected)
    {
        this.frameIndex = frameIndex;
        imgDot.color = isSelected ? Color.green : Color.white;
    }
}
public class PageNavMotionLabelBarWidget : UIItemWidget<PageNavMotionLabelBarWidget>
{
    #region auto generated members
    private Text m_lblLabelIndex;
    #endregion

    #region auto generated binders
    protected override void CodeGenBindMembers()
    {
        m_lblLabelIndex = transform.Find("m_lblLabelIndex").GetComponent<Text>();

    }
    #endregion


    #region auto generated events
    #endregion

    private List<Text> m_labels = new List<Text>();
    private RectTransform rectTransform;
    private int startIndex;
    private int labelCount;
    private int dotCount => PageNavMotion.dot_count;
    protected override void OnInit()
    {
        base.OnInit();
        m_labels.AddRange(gameObject.GetComponentsInChildren<Text>(true));
        rectTransform = gameObject.GetComponent<RectTransform>();
    }

    private List<PageNavMotionLabelWidget> m_labelWidgets = new List<PageNavMotionLabelWidget>();

    public void SetData(int index)
    {
        startIndex = index;
        labelCount = dotCount / 5 + 1;
        SetListItem(m_labelWidgets, m_lblLabelIndex.gameObject, gameObject.transform, labelCount, OnLabelItemCreate);
        for (int i = 0; i < labelCount; i++)
        {
            var label = PageNavMotionLabelWidget.CreateWidget(m_labelWidgets[i].gameObject);
            var pos = label.rectTransform.anchoredPosition;
            pos.x = i * PageNavMotion.dot_space * 5 + PageNavMotion.dot_padding;
            label.rectTransform.anchoredPosition = pos;
            var frameIndex = startIndex + i * 5 + 1;
            label.label.text = frameIndex.ToString();
        }
    }

    private void OnLabelItemCreate(PageNavMotionLabelWidget widget)
    {
    }

    public bool GetLabelPosition(int frameIndex, out Vector2 position)
    {
        int min = startIndex;
        int max = startIndex + dotCount - 1;
        if (frameIndex < min || frameIndex > max)
        {
            position = Vector2.zero;
            return false;
        }

        int relativeFrameIndex = frameIndex - startIndex;
        var pos = m_labelWidgets[0].rectTransform.position;
        pos.x += relativeFrameIndex * PageNavMotion.dot_space;
        position = pos;
        return true;
    }

    public int GetRelativeLabelByPosition(Vector2 position)
    {
        float minDistance = float.MaxValue;
        int minIndex = 0;
        var startPosX = m_labelWidgets[0].rectTransform.position.x;
        for (int i = 0; i < dotCount; i++)
        {
            var dist = Mathf.Abs(position.x - (startPosX + i * PageNavMotion.dot_space));
            if (dist < minDistance)
            {
                minDistance = dist;
                minIndex = i;
            }
        }
        return minIndex + startIndex;
    }
}

public class PageNavMotionLabelWidget : UIItemWidget<PageNavMotionLabelWidget>
{
    public Text label;
    public RectTransform rectTransform;

    protected override void OnInit()
    {
        base.OnInit();
        label = gameObject.GetComponent<Text>();
        rectTransform = gameObject.GetComponent<RectTransform>();
    }
}