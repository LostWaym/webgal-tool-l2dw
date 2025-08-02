using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MainUIWindow : BaseWindow<MainUIWindow>
{
    #region auto generated members
    private Toggle m_togglePreview;
    private Toggle m_toggleMotion;
    private Toggle m_toggleInstructions;
    private Button m_btnSetting;
    private Transform m_itemPageNavPreview;
    private Transform m_itemPageNavMotion;
    private Transform m_itemPageNavInstructions;
    #endregion

    #region auto generated binders
    protected override void CodeGenBindMembers()
    {
        m_togglePreview = transform.Find("Tabs/toggles/m_togglePreview").GetComponent<Toggle>();
        m_toggleMotion = transform.Find("Tabs/toggles/m_toggleMotion").GetComponent<Toggle>();
        m_toggleInstructions = transform.Find("Tabs/toggles/m_toggleInstructions").GetComponent<Toggle>();
        m_btnSetting = transform.Find("Tabs/m_btnSetting").GetComponent<Button>();
        m_itemPageNavPreview = transform.Find("pages/m_itemPageNavPreview").GetComponent<Transform>();
        m_itemPageNavMotion = transform.Find("pages/m_itemPageNavMotion").GetComponent<Transform>();
        m_itemPageNavInstructions = transform.Find("pages/m_itemPageNavInstructions").GetComponent<Transform>();

        m_togglePreview.onValueChanged.AddListener(OnTogglePreviewChange);
        m_toggleMotion.onValueChanged.AddListener(OnToggleMotionChange);
        m_toggleInstructions.onValueChanged.AddListener(OnToggleInstructionsChange);
        m_btnSetting.onClick.AddListener(OnButtonSettingClick);
    }
    #endregion




    #region auto generated events
    private void OnTogglePreviewChange(bool value)
    {
    }
    private void OnToggleMotionChange(bool value)
    {
    }
    private void OnToggleInstructionsChange(bool value)
    {
    }
    private void OnButtonSettingClick()
    {
        MainControl.Instance.ShowSettingUIWindow();
    }
    #endregion

    private PageNavPreview m_pageNavPreview;
    private PageNavMotion m_pageNavMotion;
    private PageNavInstructions m_pageNavInstructions;
    protected override void OnInit()
    {
        base.OnInit();
        m_pageNavPreview = PageNavPreview.CreateWidget(m_itemPageNavPreview.gameObject);
        m_pageNavPreview.BindToToggle(m_togglePreview);
        m_pageNavMotion = PageNavMotion.CreateWidget(m_itemPageNavMotion.gameObject);
        m_pageNavMotion.BindToToggle(m_toggleMotion);
        m_pageNavInstructions = PageNavInstructions.CreateWidget(m_itemPageNavInstructions.gameObject);
        m_pageNavInstructions.BindToToggle(m_toggleInstructions);
    }

    public void Update()
    {
        if (m_pageNavPreview.IsActive)
        {
            m_pageNavPreview.Update();
        }
        if (m_pageNavMotion.IsActive)
        {
            // m_pageNavMotion.Update();
        }
    }
}

internal class PageNavInstructions : UIPageWidget<PageNavInstructions>
{
}

public class PageNavPreview : UIPageWidget<PageNavPreview>
{

    public class ToolWidget : UIItemWidget<ToolWidget>
    {
        #region auto generated members
        private Toggle m_toggleMove;
        private Toggle m_toggleRotate;
        private Toggle m_toggleScale;
        private Text m_lblToolTitle;
        private Dropdown m_dropdownInstCopy;
        #endregion

        #region auto generated binders
        protected override void CodeGenBindMembers()
        {
            m_toggleMove = transform.Find("ToolContainer/Flow/m_toggleMove").GetComponent<Toggle>();
            m_toggleRotate = transform.Find("ToolContainer/Flow/m_toggleRotate").GetComponent<Toggle>();
            m_toggleScale = transform.Find("ToolContainer/Flow/m_toggleScale").GetComponent<Toggle>();
            m_lblToolTitle = transform.Find("ToolContainer/m_lblToolTitle").GetComponent<Text>();
            m_dropdownInstCopy = transform.Find("NextAction/m_dropdownInstCopy").GetComponent<Dropdown>();

            m_toggleMove.onValueChanged.AddListener(OnToggleMoveChange);
            m_toggleRotate.onValueChanged.AddListener(OnToggleRotateChange);
            m_toggleScale.onValueChanged.AddListener(OnToggleScaleChange);
            m_dropdownInstCopy.onValueChanged.AddListener(OnDropdownInstCopyChange);
        }
        #endregion

        #region auto generated events
        private void OnToggleMoveChange(bool value)
        {
            if (value)
            {
                SetCurrentToggle(m_toggleMove);
            }
        }
        private void OnToggleRotateChange(bool value)
        {
            if (value)
            {
                SetCurrentToggle(m_toggleRotate);
            }
        }
        private void OnToggleScaleChange(bool value)
        {
            if (value)
            {
                SetCurrentToggle(m_toggleScale);
            }
        }
        private void OnDropdownInstCopyChange(int value)
        {
            Global.InstNextMode = (InstDealOperation)value;
        }

        #endregion

        private Toggle m_currentToggle;

        private void SetCurrentToggle(Toggle toggle)
        {
            m_currentToggle = toggle;
            if (m_currentToggle.isOn != true)
            {
                m_currentToggle.isOn = true;
            }
            prevMousePos = Input.mousePosition;
        }

        protected override void OnInit()
        {
            base.OnInit();
            m_keyToToggle[KeyCode.W] = m_toggleMove;
            m_keyToToggle[KeyCode.E] = m_toggleRotate;
            m_keyToToggle[KeyCode.R] = m_toggleScale;
            SetCurrentToggle(m_toggleMove);

            m_dropdownInstCopy.SetValueWithoutNotify((int)Global.InstNextMode);
        }

        private Dictionary<KeyCode, Toggle> m_keyToToggle = new Dictionary<KeyCode, Toggle>();

        private Vector3 prevMousePos;
        private bool ctrl, shift, alt;
        private bool canSingleKey;
        private bool isFocusOnInputField;
        private bool isMouseOnUI;
        private bool moveProcessable;
        private bool inFreezeProcess;
        private Vector3 worldDelta;
        public void Update()
        {
            ClearButtonSelection();

            isFocusOnInputField = EventSystem.current.currentSelectedGameObject != null && EventSystem.current.currentSelectedGameObject.GetComponent<InputField>() != null;
            bool isInputFreeze = MainControl.Instance.freezeProcessInputFrame > 0;
            bool anyTopViewActive = MainControl.Instance.AnyTopViewActive;

            if (isInputFreeze)
            {
                prevMousePos = Input.mousePosition;
                if (IsMouseButton(0) || IsMouseButton(1) || IsMouseButton(2))
                {
                    inFreezeProcess = true;
                }
            }
            var mainCamera = MainControl.Instance.mainCamera;
            var mouseDiff = Input.mousePosition - prevMousePos;
            worldDelta = mainCamera.ScreenToWorldPoint(mouseDiff) - mainCamera.ScreenToWorldPoint(Vector3.zero);

            if (!isFocusOnInputField && !isInputFreeze && !anyTopViewActive)
            {
                ctrl = Input.GetKey(KeyCode.LeftControl);
                shift = Input.GetKey(KeyCode.LeftShift);
                alt = Input.GetKey(KeyCode.LeftAlt);
                canSingleKey = !ctrl && !shift && !alt;
                isMouseOnUI = IsMouseOnUI();
                ProcessChangeTool();
                ProcessToolInput();
                ProcessShortCut();

            }

            prevMousePos = Input.mousePosition;
            if (!isInputFreeze)
            {
                inFreezeProcess = false;
            }
        }

        private void ClearButtonSelection()
        {
            // 不要Button的Submit事件
            if (EventSystem.current.currentSelectedGameObject)
            {
                var hasButtonComponent = EventSystem.current.currentSelectedGameObject.GetComponent<Button>();
                if (hasButtonComponent != null)
                {
                    EventSystem.current.SetSelectedGameObject(null);
                }
            }
        }

        private void ProcessChangeTool()
        {
            if (!canSingleKey)
            {
                return;
            }

            foreach (var key in m_keyToToggle.Keys)
            {
                if (Input.GetKeyDown(key))
                {
                    SetCurrentToggle(m_keyToToggle[key]);
                    moveProcessable = false;
                    break;
                }
            }
        }

        #region 快捷键
        private void ProcessShortCut()
        {
            ProcessGlobalShortCut();

            var editType = MainControl.Instance.editType;
            if (editType == EditType.Group && MainControl.Instance.curGroup != null)
            {
                ProcessGroupShortCut();
            }
            else if (editType == EditType.Model && MainControl.Instance.curTarget != null)
            {
                ProcessModelShortCut();
            }
            else if (editType == EditType.Background)
            {
                ProcessBackgroundShortCut();
            }
        }

        private void ProcessGlobalShortCut()
        {
            if (canSingleKey)
            {
                if (Input.GetKeyDown(KeyCode.X))
                {
                    MainControl.Instance.LockX = !MainControl.Instance.LockX;
                    UIEventBus.SendEvent(UIEventType.LockXChanged);
                }
                else if (Input.GetKeyDown(KeyCode.Y))
                {
                    MainControl.Instance.LockY = !MainControl.Instance.LockY;
                    UIEventBus.SendEvent(UIEventType.LockYChanged);
                }

            }
        }

        private void ProcessGroupShortCut()
        {
            var group = MainControl.Instance.curGroup;
            if (group == null)
            {
                return;
            }

            if (ctrl)
            {
                if (Input.GetKeyDown(KeyCode.A))
                {
                    MainControl.Instance.CopyAllGroup();
                }
                else if (Input.GetKeyDown(KeyCode.F))
                {
                    MainControl.Instance.CopyMotionGroup();
                }
                else if (Input.GetKeyDown(KeyCode.T))
                {
                    MainControl.Instance.CopyTransformGroup();
                }
                else if (Input.GetKeyDown(KeyCode.X))
                {
                    MainControl.Instance.CopyAllGroupSpilt();
                }
            }
        }

        private void ProcessModelShortCut()
        {
            var target = MainControl.Instance.curTarget;
            if (target == null)
            {
                return;
            }

            if (target.DisplayMode == ModelDisplayMode.EmotionEditor)
            {
                if (ctrl)
                {
                    if (Input.GetKeyDown(KeyCode.C))
                    {
                        MainControl.Instance.CopyMotionEditor();
                    }
                }
            }
            else if (target.DisplayMode == ModelDisplayMode.Normal)
            {
                if (ctrl)
                {
                    if (Input.GetKeyDown(KeyCode.A))
                    {
                        MainControl.Instance.CopyAll();
                    }
                    else if (Input.GetKeyDown(KeyCode.F))
                    {
                        MainControl.Instance.CopyMotion();
                    }
                    else if (Input.GetKeyDown(KeyCode.T))
                    {
                        MainControl.Instance.CopyTransform();
                    }
                    else if (Input.GetKeyDown(KeyCode.X))
                    {
                        MainControl.Instance.CopyAllSpilt();
                    }
                    else if (Input.GetKeyDown(KeyCode.R))
                    {
                        target.ReloadTextures();
                    }
                }
            }

        }

        private void ProcessBackgroundShortCut()
        {
            if (ctrl)
            {
                if (Input.GetKeyDown(KeyCode.A))
                {
                    MainControl.Instance.CopyBackgroundAll();
                }
                else if (Input.GetKeyDown(KeyCode.F))
                {
                    MainControl.Instance.CopyBackgroundChange();
                }
                else if (Input.GetKeyDown(KeyCode.T))
                {
                    MainControl.Instance.CopyBackgroundTransform();
                }
            }
        }

        #endregion

        private void ProcessToolInput()
        {
            // 永远执行镜头移动
            ProcessCameraInput();
            
            if (m_currentToggle == null)
            {
                return;
            }
            else if (MainControl.Instance.editType == EditType.Group && MainControl.Instance.curGroup != null)
            {
                ProcessGroupInput();
            }
            else if (MainControl.Instance.editType == EditType.Model && MainControl.Instance.curTarget != null)
            {
                ProcessModelInput();
            }
            else if (MainControl.Instance.editType == EditType.Background)
            {
                ProcessBackgroundInput();
            }
        }

        #region 功能输入

        private void ProcessCameraInput()
        {
            if (isMouseOnUI)
                return;
            
            if (IsMouseButton(2))
            {
                var delta = Input.mousePosition - prevMousePos;
                var camera = MainControl.Instance.mainCamera;
                var worldDelta = -(camera.ScreenToWorldPoint(delta) - camera.ScreenToWorldPoint(Vector3.zero));
                camera.transform.position += new Vector3(worldDelta.x, worldDelta.y, 0);
                UIEventBus.SendEvent(UIEventType.CameraTransformChanged);
            }
            if (HasWheel())
            {
                var delta = 0f;
                if (HasWheel())
                {
                    delta = -Input.GetAxis("Mouse ScrollWheel");
                }
                var camera = MainControl.Instance.mainCamera;
                bool isBoost = Input.GetKey(KeyCode.LeftControl);
                var zoomFactor = (isBoost ? Global.CameraZoomBoostFactor : Global.CameraZoomFactor);
                if (math.sign(delta) > 0)
                    camera.orthographicSize *= zoomFactor;
                else
                    camera.orthographicSize /= zoomFactor;
                UIEventBus.SendEvent(UIEventType.CameraTransformChanged);
            }
        }

        private void ProcessGroupInput()
        {
            var group = MainControl.Instance.curGroup;
            if (m_currentToggle == m_toggleMove)
            {
                if (IsKeyboardDown(KeyCode.P) || IsMouseButtonDown(1) || IsMouseButtonDown(0))
                {
                    moveProcessable = !isMouseOnUI;
                }

                if ((IsKeyboard(KeyCode.P) || IsMouseButton(1) || IsMouseButton(0)) && moveProcessable)
                {
                    var worldPos = MainControl.Instance.mainCamera.ScreenToWorldPoint(Input.mousePosition);
                    if (IsMouseButton(0))
                    {
                        worldPos = group.root.position + worldDelta;
                    }
                    if (MainControl.Instance.LockX)
                    {
                        worldPos.x = group.root.position.x;
                    }
                    if (MainControl.Instance.LockY)
                    {
                        worldPos.y = group.root.position.y;
                    }
                    worldPos.z = group.root.position.z;
                    if (IsMouseButton(1) || IsMouseButton(0))
                    {
                        group.SetPosition(worldPos);
                    }
                    else if (IsKeyboard(KeyCode.P))
                    {
                        group.SetPivotPositon(worldPos);
                    }
                }
                else
                {
                    moveProcessable = false;
                }
            }
            else if (m_currentToggle == m_toggleRotate && !isMouseOnUI)
            {
                if (IsMouseButtonDown(0))
                {
                    moveProcessable = !isMouseOnUI;
                }

                if ((IsMouseButton(0)) && moveProcessable)
                {
                    var worldPos = MainControl.Instance.mainCamera.ScreenToWorldPoint(Input.mousePosition);
                    var oldVector = new Vector3(
                        worldPos.x - worldDelta.x - group.root.position.x,
                        worldPos.y - worldDelta.y - group.root.position.y,
                        0
                    );
                    var newVector = new Vector3(
                        worldPos.x - group.root.position.x,
                        worldPos.y - group.root.position.y,
                        0
                    );
                    group.SetRotation(group.RotationDeg + Vector3.SignedAngle(oldVector, newVector, Vector3.forward));
                }
                else
                {
                    moveProcessable = false;
                }
            }
            else if (m_currentToggle == m_toggleScale && !isMouseOnUI)
            {
                if (IsMouseButtonDown(0))
                {
                    moveProcessable = !isMouseOnUI;
                }

                if ((IsMouseButton(0)) && moveProcessable)
                {
                    var worldPos = MainControl.Instance.mainCamera.ScreenToWorldPoint(Input.mousePosition);
                    var oldDistance = new Vector3(
                        worldPos.x - worldDelta.x - group.root.position.x,
                        worldPos.y - worldDelta.y - group.root.position.y,
                        0
                    ).magnitude;
                    var newDistance = new Vector3(
                        worldPos.x - group.root.position.x,
                        worldPos.y - group.root.position.y,
                        0
                    ).magnitude;
                    group.SetScale(group.Scale * (newDistance / oldDistance));
                }
                else
                {
                    moveProcessable = false;
                }
            }
        }

        private void ProcessModelInput()
        {
            var target = MainControl.Instance.curTarget;
            if (m_currentToggle == m_toggleMove)
            {
                if (IsMouseButtonDown(1) || IsMouseButtonDown(0))
                {
                    moveProcessable = !isMouseOnUI;
                }

                if ((IsMouseButton(1) || IsMouseButton(0)) && moveProcessable)
                {
                    var worldPos = MainControl.Instance.mainCamera.ScreenToWorldPoint(Input.mousePosition);
                    if (IsMouseButton(0))
                    {
                        worldPos = target.MainPos.position + worldDelta;
                    }
                    if (MainControl.Instance.LockX)
                    {
                        worldPos.x = target.MainPos.position.x;
                    }
                    if (MainControl.Instance.LockY)
                    {
                        worldPos.y = target.MainPos.position.y;
                    }
                    target.SetCharacterWorldPosition(worldPos.x, worldPos.y);
                    UIEventBus.SendEvent(UIEventType.ModelTransformChanged);
                }
                else
                {
                    moveProcessable = false;
                }
            }
            else if (m_currentToggle == m_toggleRotate && !isMouseOnUI)
            {
                if (IsMouseButtonDown(0))
                {
                    moveProcessable = !isMouseOnUI;
                }

                if ((IsMouseButton(0)) && moveProcessable)
                {
                    var oldPos = target.MainPos.position;
                    var worldPos = MainControl.Instance.mainCamera.ScreenToWorldPoint(Input.mousePosition);
                    var oldVector = new Vector3(
                        worldPos.x - worldDelta.x - target.MainPos.position.x,
                        worldPos.y - worldDelta.y - target.MainPos.position.y,
                        0
                    );
                    var newVector = new Vector3(
                        worldPos.x - target.MainPos.position.x,
                        worldPos.y - target.MainPos.position.y,
                        0
                    );
                    target.SetRotation(target.RootRotation + Vector3.SignedAngle(oldVector, newVector, Vector3.forward));
                    target.SetCharacterWorldPosition(oldPos.x, oldPos.y);
                    UIEventBus.SendEvent(UIEventType.ModelTransformChanged);
                }
                else
                {
                    moveProcessable = false;
                }
            }
            else if (m_currentToggle == m_toggleScale && !isMouseOnUI)
            {
                if (IsMouseButtonDown(0))
                {
                    moveProcessable = !isMouseOnUI;
                }

                if ((IsMouseButton(0)) && moveProcessable)
                {
                    var oldPos = target.MainPos.position;
                    var worldPos = MainControl.Instance.mainCamera.ScreenToWorldPoint(Input.mousePosition);
                    var oldDistance = new Vector3(
                        worldPos.x - worldDelta.x - target.MainPos.position.x,
                        worldPos.y - worldDelta.y - target.MainPos.position.y,
                        0
                    ).magnitude;
                    var newDistance = new Vector3(
                        worldPos.x - target.MainPos.position.x,
                        worldPos.y - target.MainPos.position.y,
                        0
                    ).magnitude;
                    target.SetScale(target.RootScaleValue * (newDistance / oldDistance));
                    target.SetCharacterWorldPosition(oldPos.x, oldPos.y);
                    UIEventBus.SendEvent(UIEventType.ModelTransformChanged);
                }
                else
                {
                    moveProcessable = false;
                }
            }
        }

        private void ProcessBackgroundInput()
        {
            var bgContainer = MainControl.Instance.bgContainer;
            if (m_currentToggle == m_toggleMove)
            {
                if (IsMouseButtonDown(1) || IsMouseButtonDown(0))
                {
                    moveProcessable = !isMouseOnUI;
                }

                if ((IsMouseButton(1) || IsMouseButton(0)) && moveProcessable)
                {
                    var mainCamera = MainControl.Instance.mainCamera;
                    var worldPos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
                    if (IsMouseButton(0))
                    {
                        worldPos = bgContainer.root.position + worldDelta;
                    }
                    if (MainControl.Instance.LockX)
                    {
                        worldPos.x = bgContainer.root.position.x;
                    }
                    if (MainControl.Instance.LockY)
                    {
                        worldPos.y = bgContainer.root.position.y;
                    }
                    bgContainer.SetWorldPosition(worldPos.x, worldPos.y);
                }
                else
                {
                    moveProcessable = false;
                }
            }
            else if (m_currentToggle == m_toggleRotate && !isMouseOnUI)
            {
                if (IsMouseButtonDown(0))
                {
                    moveProcessable = !isMouseOnUI;
                }

                if ((IsMouseButton(0)) && moveProcessable)
                {
                    var worldPos = MainControl.Instance.mainCamera.ScreenToWorldPoint(Input.mousePosition);
                    var oldVector = new Vector3(
                        worldPos.x - worldDelta.x - bgContainer.root.position.x,
                        worldPos.y - worldDelta.y - bgContainer.root.position.y,
                        0
                    );
                    var newVector = new Vector3(
                        worldPos.x - bgContainer.root.position.x,
                        worldPos.y - bgContainer.root.position.y,
                        0
                    );
                    bgContainer.SetRotation(bgContainer.rootRotation + Vector3.SignedAngle(oldVector, newVector, Vector3.forward));
                }
                else
                {
                    moveProcessable = false;
                }
            }
            else if (m_currentToggle == m_toggleScale && !isMouseOnUI)
            {
                if (IsMouseButtonDown(0))
                {
                    moveProcessable = !isMouseOnUI;
                }

                if ((IsMouseButton(0)) && moveProcessable)
                {
                    var worldPos = MainControl.Instance.mainCamera.ScreenToWorldPoint(Input.mousePosition);
                    var oldDistance = new Vector3(
                        worldPos.x - worldDelta.x - bgContainer.root.position.x,
                        worldPos.y - worldDelta.y - bgContainer.root.position.y,
                        0
                    ).magnitude;
                    var newDistance = new Vector3(
                        worldPos.x - bgContainer.root.position.x,
                        worldPos.y - bgContainer.root.position.y,
                        0
                    ).magnitude;
                    bgContainer.SetScale(bgContainer.rootScale * (newDistance / oldDistance));
                }
                else
                {
                    moveProcessable = false;
                }
            }
        }

        #endregion

        private bool IsKeyboard(KeyCode keyCode)
        {
            return !isMouseOnUI && Input.GetKey(keyCode);
        }
        
        private bool IsKeyboardDown(KeyCode keyCode)
        {
            return Input.GetKeyDown(keyCode)  || inFreezeProcess && IsKeyboard(keyCode);
        }
        
        private bool IsMouseButton(int button)
        {
            return Input.GetMouseButton(button);
        }

        private bool IsMouseButtonDown(int button)
        {
            return Input.GetMouseButtonDown(button) || inFreezeProcess && IsMouseButton(button);
        }

        private bool IsMouseButtonUp(int button)
        {
            return Input.GetMouseButtonUp(button);
        }

        private bool HasWheel()
        {
            return !isMouseOnUI && Input.GetAxis("Mouse ScrollWheel") != 0;
        }

        private bool IsMouseOnUI()
        {
            var mousePos = Input.mousePosition;
            var isMouseInsideGameWindow = 
                mousePos.x >= 0.0f
                && mousePos.x <= Screen.width
                && mousePos.y >= 0.0f
                && mousePos.y <= Screen.height;
            
            return !isMouseInsideGameWindow || EventSystem.current.IsPointerOverGameObject();
        }
    }

    #region auto generated members
    private Transform m_tfCursor;
    private Transform m_tfFunctions;
    private Toggle m_toggleChara;
    private Toggle m_toggleGroup;
    private Toggle m_toggleBackGround;
    private Transform m_itemPageCharaMenu;
    private Transform m_itemPageGroupMenu;
    private Transform m_itemPageBackgroundMenu;
    private Transform m_itemPageCharaFunctions;
    private Transform m_itemPageGroupFunctions;
    private Transform m_itemPageBackgroundFunctions;
    private Transform m_itemTools;
    #endregion

    #region auto generated binders
    protected override void CodeGenBindMembers()
    {
        m_tfCursor = transform.Find("m_tfCursor").GetComponent<Transform>();
        m_tfFunctions = transform.Find("Left/m_tfFunctions").GetComponent<Transform>();
        m_toggleChara = transform.Find("Left/m_tfFunctions/m_toggleChara").GetComponent<Toggle>();
        m_toggleGroup = transform.Find("Left/m_tfFunctions/m_toggleGroup").GetComponent<Toggle>();
        m_toggleBackGround = transform.Find("Left/m_tfFunctions/m_toggleBackGround").GetComponent<Toggle>();
        m_itemPageCharaMenu = transform.Find("Left/pages/m_itemPageCharaMenu").GetComponent<Transform>();
        m_itemPageGroupMenu = transform.Find("Left/pages/m_itemPageGroupMenu").GetComponent<Transform>();
        m_itemPageBackgroundMenu = transform.Find("Left/pages/m_itemPageBackgroundMenu").GetComponent<Transform>();
        m_itemPageCharaFunctions = transform.Find("Right/pages/m_itemPageCharaFunctions").GetComponent<Transform>();
        m_itemPageGroupFunctions = transform.Find("Right/pages/m_itemPageGroupFunctions").GetComponent<Transform>();
        m_itemPageBackgroundFunctions = transform.Find("Right/pages/m_itemPageBackgroundFunctions").GetComponent<Transform>();
        m_itemTools = transform.Find("Center/TopBar/m_itemTools").GetComponent<Transform>();

        m_toggleChara.onValueChanged.AddListener(OnToggleCharaChange);
        m_toggleGroup.onValueChanged.AddListener(OnToggleGroupChange);
        m_toggleBackGround.onValueChanged.AddListener(OnToggleBackGroundChange);
    }
    #endregion

    #region auto-generated code event
    private void OnToggleCharaChange(bool value)
    {
        // Debug.Log("OnToggleCharaChange");
    }
    private void OnToggleGroupChange(bool value)
    {
        // Debug.Log("OnToggleGroupChange");
    }
    private void OnToggleBackGroundChange(bool value)
    {
        // Debug.Log("OnToggleBackGroundChange");
    }
    #endregion

    private PageCharaFunctions m_pageCharaFunctions;
    private PageCharaMenu m_pageCharaMenu;
    private PageGroupMenu m_pageGroupMenu;
    private PageGroupFunctions m_pageGroupFunctions;
    private PageBackgroundMenu m_pageBackgroundMenu;
    private PageBackgroundFunctions m_pageBackgroundFunctions;

    private ToolWidget m_toolWidget;
    protected override void OnInit()
    {
        base.OnInit();
        m_pageCharaMenu = PageCharaMenu.CreateWidget(m_itemPageCharaMenu.gameObject);
        m_pageCharaFunctions = PageCharaFunctions.CreateWidget(m_itemPageCharaFunctions.gameObject);
        m_pageCharaMenu.Inject(m_pageCharaFunctions);
        m_pageCharaMenu.BindToToggle(m_toggleChara);
        m_pageCharaFunctions.BindToToggle(m_toggleChara);

        m_pageGroupMenu = PageGroupMenu.CreateWidget(m_itemPageGroupMenu.gameObject);
        m_pageGroupFunctions = PageGroupFunctions.CreateWidget(m_itemPageGroupFunctions.gameObject);
        m_pageGroupMenu.Inject(m_pageGroupFunctions, m_tfCursor);
        m_pageGroupMenu.BindToToggle(m_toggleGroup);
        m_pageGroupFunctions.BindToToggle(m_toggleGroup);

        m_pageBackgroundMenu = PageBackgroundMenu.CreateWidget(m_itemPageBackgroundMenu.gameObject);
        m_pageBackgroundFunctions = PageBackgroundFunctions.CreateWidget(m_itemPageBackgroundFunctions.gameObject);
        m_pageBackgroundMenu.BindToToggle(m_toggleBackGround);
        m_pageBackgroundFunctions.BindToToggle(m_toggleBackGround);
        m_pageBackgroundMenu.Inject(m_pageBackgroundFunctions);

        m_toolWidget = ToolWidget.CreateWidget(m_itemTools.gameObject);
    }

    public void Update()
    {
        m_toolWidget.Update();
    }
}

public class MotionEntryWidget : UIItemWidget<MotionEntryWidget>
{
    public string name;

    #region auto-generated code
    private Image m_imgBG;
    private Text m_lblTitle;

    protected override void CodeGenBindMembers()
    {
        m_imgBG = transform.Find("m_imgBG").GetComponent<Image>();
        m_lblTitle = transform.Find("m_lblTitle").GetComponent<Text>();
    }
    #endregion
    #region auto-generated code event
    #endregion


    public void SetData(string name)
    {
        this.name = name;
        m_lblTitle.text = name;
    }

    public override void SetStateStyle(UIStateStyle.UIState state)
    {
        base.SetStateStyle(state);
        StateStyle.SetColor(m_imgBG, state);
    }
}

public class CharaItemWidget : UIItemWidget<CharaItemWidget>
{
    #region auto-generated code
    private Text m_lblTitle;
    private Button m_btnVisible;
    private MonoUIStyle m_styleVisible;
    private Button m_btnDelete;

    protected override void CodeGenBindMembers()
    {
        m_lblTitle = transform.Find("m_lblTitle").GetComponent<Text>();
        m_btnVisible = transform.Find("m_btnVisible").GetComponent<Button>();
        m_styleVisible = transform.Find("m_btnVisible/m_styleVisible").GetComponent<MonoUIStyle>();
        m_btnDelete = transform.Find("m_btnDelete").GetComponent<Button>();
        m_btnVisible.onClick.AddListener(OnVisibleClick);
        m_btnDelete.onClick.AddListener(OnDeleteClick);
    }
    #endregion


    #region auto-generated code event
    public void OnVisibleClick()
    {
        _OnVisibleClick?.Invoke(this);
    }
    public void OnDeleteClick()
    {
        _OnDeleteClick?.Invoke(this);
    }
    #endregion


    public Action<CharaItemWidget> _OnVisibleClick;
    public Action<CharaItemWidget> _OnDeleteClick;
    public override void SetStateStyle(UIStateStyle.UIState state)
    {
        StateStyle.SetActiveObject(state);
        StateStyle.SetColor(m_lblTitle, state);
    }

    public void SetVisibleStyle(UIStateStyle.UIState state)
    {
        m_styleVisible.style.SetActiveObject(state);
        m_styleVisible.style.SetObjectsColor(state);
    }

    protected override void OnInit()
    {
        base.OnInit();
        SetStateStyle(UIStateStyle.UIState.Normal);
    }

    public void SetData(string name)
    {
        m_lblTitle.text = name;
    }
}

public class LabelInputFieldWidget : UIItemWidget<LabelInputFieldWidget>
{

    #region auto-generated code
    private Text m_lblTitle;
    private InputField m_iptField;
    private Text m_lblPrefix;
    private Toggle m_toggle;

    protected override void CodeGenBindMembers()
    {
        m_lblTitle = transform.Find("Label/m_lblTitle").GetComponent<Text>();
        m_iptField = transform.Find("Value/InputField/m_iptField").GetComponent<InputField>();
        m_lblPrefix = transform.Find("Value/InputField/m_lblPrefix").GetComponent<Text>();
        m_toggle = transform.Find("Value/InputField/m_toggle").GetComponent<Toggle>();
    }
    #endregion

    #region auto-generated code event
    #endregion

    public string Data => m_iptField.text;

    private Action<string> m_onSubmit;

    private Action<Toggle, bool> _onToggleChange;
    
    protected override void OnInit()
    {
        base.OnInit();
        m_iptField.onEndEdit.AddListener(OnEndEdit);
        m_toggle.onValueChanged.AddListener(OnToggleChange);
    }

    private void OnToggleChange(bool arg0)
    {
        _onToggleChange?.Invoke(m_toggle, arg0);
    }

    private void OnEndEdit(string value)
    {
        m_onSubmit?.Invoke(value);
    }
    
    public void SetData(string value)
    {
        m_iptField.SetTextWithoutNotify(value);
    }

    public void SetDataSubmit(Action<string> onSubmit)
    {
        m_onSubmit = onSubmit;
    }

    public void SetToggleChange(Action<Toggle, bool> onToggleChange)
    {
        _onToggleChange = onToggleChange;
    }

    public void SetToggleValue(bool value, bool notify = true)
    {
        if (notify)
        {
            m_toggle.isOn = value;
        }
        else
        {
            m_toggle.SetIsOnWithoutNotify(value);
        }
    }
}

public class PageCharaMenu : UIPageWidget<PageCharaMenu>
{
    #region auto generated members
    private Button m_btnLoadConf;
    private Button m_btnLoadJson;
    private Button m_btnLoadImg;
    private ScrollRect m_scrollChara;
    private Transform m_tfCharaItems;
    private Transform m_itemChara;
    private Button m_btnMoreProp;
    private Button m_btnSaveProfile;
    private Button m_btnTop;
    private Button m_btnBottom;
    private Button m_btnZSortHelp;
    private Button m_btnUp;
    private Button m_btnDown;
    private Button m_btnSaveTransform;
    private Button m_btnLoadTransform;
    private Transform m_itemPosX;
    private Transform m_itemPosY;
    private Transform m_itemScale;
    private Transform m_itemRotation;
    private Button m_btnReloadModel;
    private Button m_btnReloadTexture;
    private Button m_btnOpenModelPath;
    #endregion

    #region auto generated binders
    protected override void CodeGenBindMembers()
    {
        m_btnLoadConf = transform.Find("Top/m_btnLoadConf").GetComponent<Button>();
        m_btnLoadJson = transform.Find("Top/m_btnLoadJson").GetComponent<Button>();
        m_btnLoadImg = transform.Find("Top/m_btnLoadImg").GetComponent<Button>();
        m_scrollChara = transform.Find("m_scrollChara").GetComponent<ScrollRect>();
        m_tfCharaItems = transform.Find("m_scrollChara/Viewport/m_tfCharaItems").GetComponent<Transform>();
        m_itemChara = transform.Find("m_scrollChara/Viewport/m_tfCharaItems/m_itemChara").GetComponent<Transform>();
        m_btnMoreProp = transform.Find("Properties/Title/m_btnMoreProp").GetComponent<Button>();
        m_btnSaveProfile = transform.Find("Properties/Title/m_btnSaveProfile").GetComponent<Button>();
        m_btnTop = transform.Find("Properties/Container/zsetgroup/m_btnTop").GetComponent<Button>();
        m_btnBottom = transform.Find("Properties/Container/zsetgroup/m_btnBottom").GetComponent<Button>();
        m_btnZSortHelp = transform.Find("Properties/Container/zsetgroup/m_btnZSortHelp").GetComponent<Button>();
        m_btnUp = transform.Find("Properties/Container/zsetgroup/m_btnUp").GetComponent<Button>();
        m_btnDown = transform.Find("Properties/Container/zsetgroup/m_btnDown").GetComponent<Button>();
        m_btnSaveTransform = transform.Find("Properties/Container/GameObject (1)/m_btnSaveTransform").GetComponent<Button>();
        m_btnLoadTransform = transform.Find("Properties/Container/GameObject (1)/m_btnLoadTransform").GetComponent<Button>();
        m_itemPosX = transform.Find("Properties/Container/m_itemPosX").GetComponent<Transform>();
        m_itemPosY = transform.Find("Properties/Container/m_itemPosY").GetComponent<Transform>();
        m_itemScale = transform.Find("Properties/Container/m_itemScale").GetComponent<Transform>();
        m_itemRotation = transform.Find("Properties/Container/m_itemRotation").GetComponent<Transform>();
        m_btnReloadModel = transform.Find("Properties/Container/GameObject/m_btnReloadModel").GetComponent<Button>();
        m_btnReloadTexture = transform.Find("Properties/Container/GameObject/m_btnReloadTexture").GetComponent<Button>();
        m_btnOpenModelPath = transform.Find("Properties/Container/GameObject/m_btnOpenModelPath").GetComponent<Button>();

        m_btnLoadConf.onClick.AddListener(OnButtonLoadConfClick);
        m_btnLoadJson.onClick.AddListener(OnButtonLoadJsonClick);
        m_btnLoadImg.onClick.AddListener(OnButtonLoadImgClick);
        m_btnMoreProp.onClick.AddListener(OnButtonMorePropClick);
        m_btnSaveProfile.onClick.AddListener(OnButtonSaveProfileClick);
        m_btnTop.onClick.AddListener(OnButtonTopClick);
        m_btnBottom.onClick.AddListener(OnButtonBottomClick);
        m_btnZSortHelp.onClick.AddListener(OnButtonZSortHelpClick);
        m_btnUp.onClick.AddListener(OnButtonUpClick);
        m_btnDown.onClick.AddListener(OnButtonDownClick);
        m_btnSaveTransform.onClick.AddListener(OnButtonSaveTransformClick);
        m_btnLoadTransform.onClick.AddListener(OnButtonLoadTransformClick);
        m_btnReloadModel.onClick.AddListener(OnButtonReloadModelClick);
        m_btnReloadTexture.onClick.AddListener(OnButtonReloadTextureClick);
        m_btnOpenModelPath.onClick.AddListener(OnButtonOpenModelPathClick);
    }
    #endregion

    #region auto-generated code event
    public void OnButtonLoadConfClick()
    {
        MainControl.Instance.LoadConf();
    }
    public void OnButtonLoadJsonClick()
    {
        MainControl.Instance.LoadConfig();
    }
    private void OnButtonLoadImgClick()
    {
        MainControl.Instance.LoadImg();
    }

    public void OnButtonMorePropClick()
    {
        MainControl.Instance.ShowConfigEditor();
    }

    private void OnButtonSaveProfileClick()
    {
        MainControl.Instance.SaveConf();
    }

    private void OnButtonTopClick()
    {
        SetTargetZSort(true, true);
    }
    private void OnButtonUpClick()
    {
        SetTargetZSort(true, false);
    }
    private void OnButtonZSortHelpClick()
    {
        MessageTipWindow.Instance.Show("Z轴排序帮助", "目前只是在编辑器里看的，和webgal里的z-index毫无关系\n排序做得还不是很完美\n立绘会有锯齿\n这个以后在优化.");
    }

    private void OnButtonDownClick()
    {
        SetTargetZSort(false, false);
    }
    private void OnButtonBottomClick()
    {
        SetTargetZSort(false, true);
    }

    private void OnButtonSaveTransformClick()
    {
        MainControl.Instance.SaveTransform();
    }
    private void OnButtonLoadTransformClick()
    {
        if (MainControl.Instance.curTarget == null)
        {
            MainControl.Instance.ShowErrorDebugText("请先选择一个模型");
            return;
        }

        if (MainControl.Instance.transformData == null)
        {
            MainControl.Instance.ShowErrorDebugText("请先保存一个变换");
            return;
        }

        TransformCopyUI.Instance.Show();
    }

    public void OnButtonReloadModelClick()
    {
        var model = MainControl.Instance.curTarget;
        if (model != null)
        {
            MainControl.Instance.ReloadModel();
        }
    }
    public void OnButtonReloadTextureClick()
    {
        var model = MainControl.Instance.curTarget;
        if (model != null)
        {
            model.ReloadTextures();
        }
    }
    public void OnButtonOpenModelPathClick()
    {
        var meta = MainControl.Instance.curMeta;
        if (meta != null)
        {
            System.Diagnostics.Process.Start("explorer.exe", Path.GetDirectoryName(meta.GetValidModelFilePath(0)));
        }
    }
    #endregion

    private LabelInputFieldWidget m_liptPosX;
    private LabelInputFieldWidget m_liptPosY;
    private LabelInputFieldWidget m_liptScale;
    private LabelInputFieldWidget m_liptRotation;
    private List<CharaItemWidget> m_listChara = new List<CharaItemWidget>();
    private PageCharaFunctions m_pageCharaFunctions;

    protected override void OnInit()
    {
        base.OnInit();
        m_liptPosX = LabelInputFieldWidget.CreateWidget(m_itemPosX.gameObject);
        m_liptPosY = LabelInputFieldWidget.CreateWidget(m_itemPosY.gameObject);
        m_liptScale = LabelInputFieldWidget.CreateWidget(m_itemScale.gameObject);
        m_liptRotation = LabelInputFieldWidget.CreateWidget(m_itemRotation.gameObject);

        m_liptPosX.SetDataSubmit(OnXPosSubmit);
        m_liptPosX.SetToggleChange(OnLockXChange);
        m_liptPosY.SetDataSubmit(OnYPosSubmit);
        m_liptPosY.SetToggleChange(OnLockYChange);
        m_liptScale.SetDataSubmit(OnScaleSubmit);
        m_liptScale.SetToggleChange(OnFlipChange);
        m_liptRotation.SetDataSubmit(OnRotationSubmit);
    }

    public override void OnPageShown()
    {
        base.OnPageShown();
        MainControl.OnLoadConf += OnLoadConf;
        MainControl.OnMetaChanged += OnMetaChanged;
        UIEventBus.AddListener(UIEventType.ModelTransformChanged, OnModelTransformChanged);
        UIEventBus.AddListener(UIEventType.OnModelChanged, OnModelChanged);
        UIEventBus.AddListener(UIEventType.OnModelDeleted, OnModelDeleted);

        UIEventBus.AddListener(UIEventType.LockXChanged, OnLockXChanged);
        UIEventBus.AddListener(UIEventType.LockYChanged, OnLockYChanged);

        MainControl.Instance.editType = EditType.Model;

        RefreshAll();
    }

    public override void OnPageHidden()
    {
        base.OnPageHidden();
        MainControl.OnLoadConf -= OnLoadConf;
        MainControl.OnMetaChanged -= OnMetaChanged;
        UIEventBus.RemoveListener(UIEventType.ModelTransformChanged, OnModelTransformChanged);
        UIEventBus.RemoveListener(UIEventType.OnModelChanged, OnModelChanged);
        UIEventBus.RemoveListener(UIEventType.OnModelDeleted, OnModelDeleted);

        UIEventBus.RemoveListener(UIEventType.LockXChanged, OnLockXChanged);
        UIEventBus.RemoveListener(UIEventType.LockYChanged, OnLockYChanged);
    }

    public void SetTargetZSort(bool isUp, bool isFinal)
    {
        var curTarget = MainControl.Instance.curTarget;
        if (curTarget == null)
            return;

        var models = MainControl.Instance.models;
        var index = models.IndexOf(curTarget);
        
        if (isFinal)
        {
            models.RemoveAt(index);
            if (isUp)
            {
                models.Insert(0, curTarget);
            }
            else
            {
                models.Add(curTarget);
            }
        }
        else
        {
            if (isUp && index > 0)
            {
                models.RemoveAt(index);
                models.Insert(index - 1, curTarget);
            }
            else if (!isUp && index < models.Count - 1)
            {
                models.RemoveAt(index);
                models.Insert(index + 1, curTarget);
            }
        }

        UIEventBus.SendEvent(UIEventType.OnModelChanged);
    }

    public void Inject(PageCharaFunctions pageCharaFunctions)
    {
        m_pageCharaFunctions = pageCharaFunctions;
    }

    private void OnLockXChange(Toggle toggle, bool value)
    {
        MainControl.Instance.LockX = value;
    }

    private void OnLockYChange(Toggle toggle, bool value)
    {
        MainControl.Instance.LockY = value;
    }
    
    private void OnFlipChange(Toggle toggle, bool arg2)
    {
        var model = MainControl.Instance.curTarget;
        if (model != null)
        {
            model.SetReverseXScale(toggle.isOn);
        }
        RefreshCharaTransform();
    }

    private void OnLockXChanged()
    {
        m_liptPosX.SetToggleValue(MainControl.Instance.LockX);
    }

    private void OnLockYChanged()
    {
        m_liptPosY.SetToggleValue(MainControl.Instance.LockY);
    }

    private void OnModelDeleted()
    {
        OnModelChanged();
    }

    private void OnModelChanged()
    {
        RefreshAll();
        var model = MainControl.Instance.curTarget;
        if (model == null)
        {
            if (m_pageCharaFunctions.PageExpressionEditor.IsActive)
            {
                m_pageCharaFunctions.PageExpressionEditor.RefreshAll();
            }
            else if (m_pageCharaFunctions.PageCharacterPreview.IsActive)
            {
                m_pageCharaFunctions.PageCharacterPreview.RefreshAll();
            }
            return;
        }
        var modelDisplayMode = model.DisplayMode;
        if (modelDisplayMode == ModelDisplayMode.EmotionEditor)
        {
            m_pageCharaFunctions.PageExpressionEditor.TrySwitchTo();
            m_pageCharaFunctions.PageExpressionEditor.RefreshAll();
        }
        else if (modelDisplayMode == ModelDisplayMode.Normal)
        {
            m_pageCharaFunctions.PageCharacterPreview.TrySwitchTo();
            m_pageCharaFunctions.PageCharacterPreview.RefreshAll();
        }
    }

    private void OnRotationSubmit(string obj)
    {
        var model = MainControl.Instance.curTarget;
        if (model != null)
        {
            if (float.TryParse(obj, out float rotation))
            {
                var oldPos = model.MainPos.position;
                model.SetRotation(rotation);
                model.SetCharacterWorldPosition(oldPos.x, oldPos.y);
            }
            RefreshCharaTransform();
        }
    }

    private void OnScaleSubmit(string obj)
    {
        var model = MainControl.Instance.curTarget;
        if (model != null)
        {
            if (float.TryParse(obj, out float scale))
            {
                var oldPos = model.MainPos.position;
                model.SetScale(scale);
                model.SetCharacterWorldPosition(oldPos.x, oldPos.y);
            }
            RefreshCharaTransform();
        }
    }
    private void OnYPosSubmit(string obj)
    {
        var model = MainControl.Instance.curTarget;
        if (model != null)
        {
            if (float.TryParse(obj, out float y))
            {
                model.SetPosition(model.RootPosition.x, y);
            }
            RefreshCharaTransform();
        }
    }

    private void OnXPosSubmit(string obj)
    {
        var model = MainControl.Instance.curTarget;
        if (model != null)
        {
            if (float.TryParse(obj, out float x))
            {
                model.SetPosition(x, model.RootPosition.y);
            }
            RefreshCharaTransform();
        }
    }

    private void OnModelTransformChanged()
    {
        RefreshCharaTransform();
    }

    public void RefreshAll()
    {
        RefreshCharaList();
        if (m_pageCharaFunctions.PageCharacterPreview.IsActive)
        {
            m_pageCharaFunctions.PageCharacterPreview.RefreshMotionList();
            m_pageCharaFunctions.PageCharacterPreview.RefreshExpressionList();
        }
        RefreshCharaTransform();
    }

    public void RefreshCharaList()
    {
        var models = MainControl.Instance.models;
        var selectedModel = MainControl.Instance.curTarget;
        SetListItem(m_listChara, m_itemChara.gameObject, m_tfCharaItems, models.Count, OnCharaItemCreate);
        for (int i = 0; i < models.Count; i++)
        {
            var item = m_listChara[i];
            item.SetData(models[i].Name);

            var model = models[i];
            bool visible = model.gameObject.activeSelf;
            var state = visible ? UIStateStyle.UIState.Normal : UIStateStyle.UIState.Disabled;
            if (selectedModel == model)
            {
                item.SetStateStyle(UIStateStyle.UIState.Selected);
            }
            else
            {
                item.SetStateStyle(state);
            }
            item.SetVisibleStyle(state);
        }

    }

    private void OnCharaItemCreate(CharaItemWidget item)
    {
        item._OnVisibleClick += OnCharaVisibleClick;
        item._OnDeleteClick += OnCharaDeleteClick;
        item.AddClickEvent(() => OnCharaClicked(item));
    }
    
    private void OnCharaVisibleClick(CharaItemWidget item)
    {
        var index = GetListItemIndex(m_listChara, item);
        var model = MainControl.Instance.models[index];
        MainControl.Instance.SetModelVisible(model, !model.gameObject.activeSelf);
        RefreshCharaList();
    }

    private void OnCharaDeleteClick(CharaItemWidget item)
    {
        var index = GetListItemIndex(m_listChara, item);
        MainControl.Instance.DeleteModel(MainControl.Instance.models[index]);
    }

    private void OnCharaClicked(CharaItemWidget item)
    {
        var index = GetListItemIndex(m_listChara, item);
        MainControl.Instance.SetCharacter(MainControl.Instance.models[index]);
    }

    private void OnLoadConf()
    {
        RefreshAll();
    }

    private void OnMetaChanged()
    {
        RefreshCharaList();
    }

    public void RefreshCharaTransform()
    {
        m_liptPosX.SetToggleValue(MainControl.Instance.LockX);
        m_liptPosY.SetToggleValue(MainControl.Instance.LockY);
        
        var model = MainControl.Instance.curTarget;
        if (model == null)
        {
            return;
        }

        m_liptPosX.SetData(model.RootPosition.x.ToString("F3"));
        m_liptPosY.SetData(model.RootPosition.y.ToString("F3"));

        m_liptScale.SetData(model.RootScaleValue.ToString("F3"));
        m_liptScale.SetToggleValue(model.ReverseXScale);

        m_liptRotation.SetData(model.RootRotation.ToString("F3"));
    }

}

public class PageCharaFunctions : UIPageWidget<PageCharaFunctions>
{

    #region auto generated members
    private Transform m_tfFunctions;
    private Toggle m_togglePreview;
    private Toggle m_toggleEdit;
    private Toggle m_toggleFilterSet;
    private Toggle m_toggleAnim;
    private Transform m_itemPageCharacterPreview;
    private Transform m_itemPageExpressionEdit;
    private Transform m_itemPageFilterSet;
    #endregion

    #region auto generated binders
    protected override void CodeGenBindMembers()
    {
        m_tfFunctions = transform.Find("m_tfFunctions").GetComponent<Transform>();
        m_togglePreview = transform.Find("m_tfFunctions/m_togglePreview").GetComponent<Toggle>();
        m_toggleEdit = transform.Find("m_tfFunctions/m_toggleEdit").GetComponent<Toggle>();
        m_toggleFilterSet = transform.Find("m_tfFunctions/m_toggleFilterSet").GetComponent<Toggle>();
        m_toggleAnim = transform.Find("m_tfFunctions/m_toggleAnim").GetComponent<Toggle>();
        m_itemPageCharacterPreview = transform.Find("pages/m_itemPageCharacterPreview").GetComponent<Transform>();
        m_itemPageExpressionEdit = transform.Find("pages/m_itemPageExpressionEdit").GetComponent<Transform>();
        m_itemPageFilterSet = transform.Find("pages/m_itemPageFilterSet").GetComponent<Transform>();

        m_togglePreview.onValueChanged.AddListener(OnTogglePreviewChange);
        m_toggleEdit.onValueChanged.AddListener(OnToggleEditChange);
        m_toggleFilterSet.onValueChanged.AddListener(OnToggleFilterSetChange);
        m_toggleAnim.onValueChanged.AddListener(OnToggleAnimChange);
    }
    #endregion

    #region auto generated events
    private void OnTogglePreviewChange(bool value)
    {
    }
    private void OnToggleEditChange(bool value)
    {
    }
    private void OnToggleFilterSetChange(bool value)
    {
    }
    private void OnToggleAnimChange(bool value)
    {
    }
    #endregion


    public PageCharacterPreview PageCharacterPreview => m_pageCharacterPreview;
    public PageExpressionEditor PageExpressionEditor => m_pageExpressionEditor;
    public PageFilterSet PageFilterSet => m_pageFilterSet;
    private PageCharacterPreview m_pageCharacterPreview;
    private PageExpressionEditor m_pageExpressionEditor;
    private PageFilterSet m_pageFilterSet;

    protected override void OnInit()
    {
        base.OnInit();
        
        m_pageCharacterPreview = PageCharacterPreview.CreateWidget(m_itemPageCharacterPreview.gameObject);
        m_pageExpressionEditor = PageExpressionEditor.CreateWidget(m_itemPageExpressionEdit.gameObject);
        //m_pageAnimationEditor = PageAnimationEditor.CreateWidget(m_itemPageAnimationEditor.gameObject);
        m_pageFilterSet = PageFilterSet.CreateWidget(m_itemPageFilterSet.gameObject);
    }

    public override void OnPageShown()
    {
        base.OnPageShown();
        m_pageCharacterPreview.BindToToggle(m_togglePreview);
        m_pageExpressionEditor.BindToToggle(m_toggleEdit);
        m_pageFilterSet.BindToToggle(m_toggleFilterSet);
    }
}

public class PageFilterSet : UIPageWidget<PageFilterSet>
{
    private ModelAdjusterBase m_model;
    private BGContainer m_bgContainer;
    private FilterSetData m_filterSetData;


    #region auto generated members
    private Button m_btnHelpFilter;
    private Button m_btnAddPreset;
    private Button m_btnLoadPreset;
    private InputField m_iptAlpha;
    private InputField m_iptBlur;
    private InputField m_iptBrightness;
    private InputField m_iptContrast;
    private InputField m_iptSaturation;
    private InputField m_iptGamma;
    private ColorPicker m_colAdjustment;
    private InputField m_iptBloom;
    private InputField m_iptBloomBrightness;
    private InputField m_iptBloomBlur;
    private InputField m_iptBloomThreshold;
    private Toggle m_toggleBevelLegacy;
    private Button m_btnHelpBevel;
    private InputField m_iptBevel;
    private InputField m_iptBevelThickness;
    private InputField m_iptBevelRotation;
    private InputField m_iptBevelSoftness;
    private ColorPicker m_colBevel;
    private Toggle m_toggleOldFilm;
    private Toggle m_toggleDotFilm;
    private Toggle m_toggleReflectionFilm;
    private Toggle m_toggleGlitchFilm;
    private Toggle m_toggleRgbFilm;
    private Toggle m_toggleGodrayFilm;
    #endregion

    #region auto generated binders
    protected override void CodeGenBindMembers()
    {
        m_btnHelpFilter = transform.Find("Parameters/Container/Scroll/Viewport/Content/Operation/Title/m_btnHelpFilter").GetComponent<Button>();
        m_btnAddPreset = transform.Find("Parameters/Container/Scroll/Viewport/Content/Operation/Container/m_btnAddPreset").GetComponent<Button>();
        m_btnLoadPreset = transform.Find("Parameters/Container/Scroll/Viewport/Content/Operation/Container/m_btnLoadPreset").GetComponent<Button>();

        m_iptAlpha = transform.Find("Parameters/Container/Scroll/Viewport/Content/Effect/Container/透明度/Value/InputField/m_iptAlpha").GetComponent<InputField>();
        m_iptBlur = transform.Find("Parameters/Container/Scroll/Viewport/Content/Effect/Container/高斯模糊/Value/InputField/m_iptBlur").GetComponent<InputField>();
        
        m_iptBrightness = transform.Find("Parameters/Container/Scroll/Viewport/Content/Adjustment/Container/亮度/Value/InputField/m_iptBrightness").GetComponent<InputField>();
        m_iptContrast = transform.Find("Parameters/Container/Scroll/Viewport/Content/Adjustment/Container/对比度/Value/InputField/m_iptContrast").GetComponent<InputField>();
        m_iptSaturation = transform.Find("Parameters/Container/Scroll/Viewport/Content/Adjustment/Container/饱和度/Value/InputField/m_iptSaturation").GetComponent<InputField>();
        m_iptGamma = transform.Find("Parameters/Container/Scroll/Viewport/Content/Adjustment/Container/伽马值/Value/InputField/m_iptGamma").GetComponent<InputField>();
        m_colAdjustment = transform.Find("Parameters/Container/Scroll/Viewport/Content/Adjustment/Container/色调/Value/m_colAdjustment").GetComponent<ColorPicker>();
        
        m_iptBloom = transform.Find("Parameters/Container/Scroll/Viewport/Content/Bloom/Container/强度/Value/InputField/m_iptBloom").GetComponent<InputField>();
        m_iptBloomBrightness = transform.Find("Parameters/Container/Scroll/Viewport/Content/Bloom/Container/亮度/Value/InputField/m_iptBloomBrightness").GetComponent<InputField>();
        m_iptBloomBlur = transform.Find("Parameters/Container/Scroll/Viewport/Content/Bloom/Container/模糊/Value/InputField/m_iptBloomBlur").GetComponent<InputField>();
        m_iptBloomThreshold = transform.Find("Parameters/Container/Scroll/Viewport/Content/Bloom/Container/阈值/Value/InputField/m_iptBloomThreshold").GetComponent<InputField>();
        
        m_toggleBevelLegacy = transform.Find("Parameters/Container/Scroll/Viewport/Content/Bevel/Container/m_toggleBevelLegacy").GetComponent<Toggle>();
        m_btnHelpBevel = transform.Find("Parameters/Container/Scroll/Viewport/Content/Bevel/Container/m_toggleBevelLegacy/m_btnHelpBevel").GetComponent<Button>();
        m_iptBevel = transform.Find("Parameters/Container/Scroll/Viewport/Content/Bevel/Container/透明度/Value/InputField/m_iptBevel").GetComponent<InputField>();
        m_iptBevelThickness = transform.Find("Parameters/Container/Scroll/Viewport/Content/Bevel/Container/粗细/Value/InputField/m_iptBevelThickness").GetComponent<InputField>();
        m_iptBevelRotation = transform.Find("Parameters/Container/Scroll/Viewport/Content/Bevel/Container/旋转/Value/InputField/m_iptBevelRotation").GetComponent<InputField>();
        m_iptBevelSoftness = transform.Find("Parameters/Container/Scroll/Viewport/Content/Bevel/Container/软化/Value/InputField/m_iptBevelSoftness").GetComponent<InputField>();
        m_colBevel = transform.Find("Parameters/Container/Scroll/Viewport/Content/Bevel/Container/色调/Value/m_colBevel").GetComponent<ColorPicker>();
        
        m_toggleOldFilm = transform.Find("Parameters/Container/Scroll/Viewport/Content/Filter/Container/m_toggleOldFilm").GetComponent<Toggle>();
        m_toggleDotFilm = transform.Find("Parameters/Container/Scroll/Viewport/Content/Filter/Container/m_toggleDotFilm").GetComponent<Toggle>();
        m_toggleReflectionFilm = transform.Find("Parameters/Container/Scroll/Viewport/Content/Filter/Container/m_toggleReflectionFilm").GetComponent<Toggle>();
        m_toggleGlitchFilm = transform.Find("Parameters/Container/Scroll/Viewport/Content/Filter/Container/m_toggleGlitchFilm").GetComponent<Toggle>();
        m_toggleRgbFilm = transform.Find("Parameters/Container/Scroll/Viewport/Content/Filter/Container/m_toggleRgbFilm").GetComponent<Toggle>();
        m_toggleGodrayFilm = transform.Find("Parameters/Container/Scroll/Viewport/Content/Filter/Container/m_toggleGodrayFilm").GetComponent<Toggle>();

        m_btnHelpFilter.onClick.AddListener(OnButtonHelpFilterClick);
        m_btnAddPreset.onClick.AddListener(OnButtonAddPresetClick);
        m_btnLoadPreset.onClick.AddListener(OnButtonLoadPresetClick);


        m_iptAlpha.onValueChanged.AddListener(OnInputFieldAlphaChange);
        m_iptAlpha.onEndEdit.AddListener(OnInputFieldAlphaEndEdit);
        m_iptBlur.onValueChanged.AddListener(OnInputFieldBlurChange);
        m_iptBlur.onEndEdit.AddListener(OnInputFieldBlurEndEdit);
        
        m_iptBrightness.onValueChanged.AddListener(OnInputFieldBrightnessChange);
        m_iptBrightness.onEndEdit.AddListener(OnInputFieldBrightnessEndEdit);
        m_iptContrast.onValueChanged.AddListener(OnInputFieldContrastChange);
        m_iptContrast.onEndEdit.AddListener(OnInputFieldContrastEndEdit);
        m_iptSaturation.onValueChanged.AddListener(OnInputFieldSaturationChange);
        m_iptSaturation.onEndEdit.AddListener(OnInputFieldSaturationEndEdit);
        m_iptGamma.onValueChanged.AddListener(OnInputFieldGammaChange);
        m_iptGamma.onEndEdit.AddListener(OnInputFieldGammaEndEdit);
        m_colAdjustment._OnColorChanged += OnColorPickerAdjustmentChanged;
        
        m_iptBloom.onValueChanged.AddListener(OnInputFieldBloomChange);
        m_iptBloom.onEndEdit.AddListener(OnInputFieldBloomEndEdit);
        m_iptBloomBrightness.onValueChanged.AddListener(OnInputFieldBloomBrightnessChange);
        m_iptBloomBrightness.onEndEdit.AddListener(OnInputFieldBloomBrightnessEndEdit);
        m_iptBloomBlur.onValueChanged.AddListener(OnInputFieldBloomBlurChange);
        m_iptBloomBlur.onEndEdit.AddListener(OnInputFieldBloomBlurEndEdit);
        m_iptBloomThreshold.onValueChanged.AddListener(OnInputFieldBloomThresholdChange);
        m_iptBloomThreshold.onEndEdit.AddListener(OnInputFieldBloomThresholdEndEdit);
        
        m_toggleBevelLegacy.onValueChanged.AddListener(OnToggleBevelLegacyChange);
        m_btnHelpBevel.onClick.AddListener(OnButtonHelpBevelClick);
        m_iptBevel.onValueChanged.AddListener(OnInputFieldBevelChange);
        m_iptBevel.onEndEdit.AddListener(OnInputFieldBevelEndEdit);
        m_iptBevelThickness.onValueChanged.AddListener(OnInputFieldBevelThicknessChange);
        m_iptBevelThickness.onEndEdit.AddListener(OnInputFieldBevelThicknessEndEdit);
        m_iptBevelRotation.onValueChanged.AddListener(OnInputFieldBevelRotationChange);
        m_iptBevelRotation.onEndEdit.AddListener(OnInputFieldBevelRotationEndEdit);
        m_iptBevelSoftness.onValueChanged.AddListener(OnInputFieldBevelSoftnessChange);
        m_iptBevelSoftness.onEndEdit.AddListener(OnInputFieldBevelSoftnessEndEdit);
        m_colBevel._OnColorChanged += OnColorPickerBevelChanged;
        
        m_toggleOldFilm.onValueChanged.AddListener(OnToggleOldFilmChange);
        m_toggleDotFilm.onValueChanged.AddListener(OnToggleDotFilmChange);
        m_toggleReflectionFilm.onValueChanged.AddListener(OnToggleReflectionFilmChange);
        m_toggleGlitchFilm.onValueChanged.AddListener(OnToggleGlitchFilmChange);
        m_toggleRgbFilm.onValueChanged.AddListener(OnToggleRgbFilmChange);
        m_toggleGodrayFilm.onValueChanged.AddListener(OnToggleGodrayFilmChange);
    }
    #endregion

    #region auto generated events
    private void OnButtonHelpFilterClick()
    {
        string text = "目前软化功能只在4.5.14默认模板和bandoricrafr1.2生效，mygo 2.5暂不支持";
        text += "\n" + "Webgal 默认引擎 <= 4.5.13, mygo <=2.5, bandoricraft engine <= 1.1, 需开启旧版倒角效果, 否则显示效果与引擎不一致";
        MessageTipWindow.Instance.Show("帮助", text);
    }
    private void OnButtonAddPresetClick()
    {
        var json = new JSONObject();
        m_filterSetData.ApplyToJson(json);
        FilterSetPresetData preset = new FilterSetPresetData();
        preset.name = "New Preset";
        preset.jsonObject = json;
        FilterUtils.AddFilterSetPreset(preset);
        FilterSetUI.Instance.SetData(preset, (data) => {
            m_filterSetData.ReadFromJson(data.jsonObject);
            AskTargetApplyFilterSetData();
            RefreshFilterSet();
        });
    }
    private void OnButtonLoadPresetClick()
    {
        FilterSetUI.Instance.SetData(null, (data) => {
            m_filterSetData.ReadFromJson(data.jsonObject);
            AskTargetApplyFilterSetData();
            RefreshFilterSet();
        });
    }

    private void OnInputFieldAlphaChange(string value)
    {
    }
    private void OnInputFieldAlphaEndEdit(string value)
    {
        if (m_filterSetData == null)
            return;

        if (float.TryParse(value, out float alpha))
        {
            m_filterSetData.Alpha = alpha;
            SendFilterSetDataChanged(ModelAdjusterBase.FilterProperty.Alpha);
        }
        RefreshFilterSet();
    }
    private void OnInputFieldBlurChange(string value)
    {
    }
    private void OnInputFieldBlurEndEdit(string value)
    {
        if (m_filterSetData == null)
            return;

        if (int.TryParse(value, out int blur))
        {
            m_filterSetData.Blur = blur;
            SendFilterSetDataChanged(ModelAdjusterBase.FilterProperty.Blur);
        }
        RefreshFilterSet();
    }
    private void OnInputFieldBrightnessChange(string value)
    {
    }
    private void OnInputFieldBrightnessEndEdit(string value)
    {
        if (m_filterSetData == null)
            return;

        if (float.TryParse(value, out float brightness))
        {
            m_filterSetData.Brightness = brightness;
            SendFilterSetDataChanged(ModelAdjusterBase.FilterProperty.Adjustment);
        }
        RefreshFilterSet();
    }
    private void OnInputFieldContrastChange(string value)
    {
    }
    private void OnInputFieldContrastEndEdit(string value)
    {
        if (m_filterSetData == null)
            return;

        if (float.TryParse(value, out float contrast))
        {
            m_filterSetData.Contrast = contrast;
            SendFilterSetDataChanged(ModelAdjusterBase.FilterProperty.Adjustment);
        }
        RefreshFilterSet();
    }
    private void OnInputFieldSaturationChange(string value)
    {
    }
    private void OnInputFieldSaturationEndEdit(string value)
    {
        if (m_filterSetData == null)
            return;

        if (float.TryParse(value, out float saturation))
        {
            m_filterSetData.Saturation = saturation;
            SendFilterSetDataChanged(ModelAdjusterBase.FilterProperty.Adjustment);
        }
        RefreshFilterSet();
    }
    private void OnInputFieldGammaChange(string value)
    {
    }
    private void OnInputFieldGammaEndEdit(string value)
    {
        if (m_filterSetData == null)
            return;

        if (float.TryParse(value, out float gamma))
        {
            m_filterSetData.Gamma = gamma;
            SendFilterSetDataChanged(ModelAdjusterBase.FilterProperty.Adjustment);
        }
        RefreshFilterSet();
    }

    private void OnColorPickerAdjustmentChanged(Color color)
    {
        if (m_filterSetData == null)
            return;

        m_filterSetData.ColorRed = color.r * 255.0f;
        m_filterSetData.ColorGreen = color.g * 255.0f;
        m_filterSetData.ColorBlue = color.b * 255.0f;
        SendFilterSetDataChanged(ModelAdjusterBase.FilterProperty.Adjustment);
        RefreshFilterSet();
    }

    private void OnInputFieldBloomChange(string value)
    {
    }
    private void OnInputFieldBloomEndEdit(string value)
    {
        if (m_filterSetData == null)
            return;

        if (float.TryParse(value, out float bloom))
        {
            m_filterSetData.Bloom = bloom;
            SendFilterSetDataChanged(ModelAdjusterBase.FilterProperty.Bloom);
        }
        RefreshFilterSet();
    }
    private void OnInputFieldBloomBrightnessChange(string value)
    {
    }
    private void OnInputFieldBloomBrightnessEndEdit(string value)
    {
        if (m_filterSetData == null)
            return;

        if (float.TryParse(value, out float bloomBrightness))
        {
            m_filterSetData.BloomBrightness = bloomBrightness;
            SendFilterSetDataChanged(ModelAdjusterBase.FilterProperty.Bloom);
        }
        RefreshFilterSet();
    }
    private void OnInputFieldBloomBlurChange(string value)
    {
    }
    private void OnInputFieldBloomBlurEndEdit(string value)
    {
        if (m_filterSetData == null)
            return;

        if (float.TryParse(value, out float bloomBlur))
        {
            m_filterSetData.BloomBlur = bloomBlur;
            SendFilterSetDataChanged(ModelAdjusterBase.FilterProperty.Bloom);
        }
        RefreshFilterSet();
    }
    private void OnInputFieldBloomThresholdChange(string value)
    {
    }
    private void OnInputFieldBloomThresholdEndEdit(string value)
    {
        if (m_filterSetData == null)
            return;

        if (float.TryParse(value, out float bloomThreshold))
        {
            m_filterSetData.BloomThreshold = bloomThreshold;
            SendFilterSetDataChanged(ModelAdjusterBase.FilterProperty.Bloom);
        }
        RefreshFilterSet();
    }
    private void OnToggleBevelLegacyChange(bool value)
    {
        if (m_filterSetData == null)
            return;
        
        m_filterSetData.BevelLegacy = value;
        SendFilterSetDataChanged(ModelAdjusterBase.FilterProperty.Bevel);
        RefreshFilterSet();
    }
    private void OnButtonHelpBevelClick()
    {
        string text = "Webgal 默认引擎 <= 4.5.13, mygo <=2.5, bandoricraft engine <= 1.1, 需开启旧版倒角效果, 否则显示效果与引擎不一致";
        MessageTipWindow.Instance.Show("帮助", text);
    }
    private void OnInputFieldBevelChange(string value)
    {
    }
    private void OnInputFieldBevelEndEdit(string value)
    {
        if (m_filterSetData == null)
            return;

        if (float.TryParse(value, out float bevel))
        {
            m_filterSetData.Bevel = bevel;
            SendFilterSetDataChanged(ModelAdjusterBase.FilterProperty.Bevel);
        }
        RefreshFilterSet();
    }
    private void OnInputFieldBevelThicknessChange(string value)
    {
    }
    private void OnInputFieldBevelThicknessEndEdit(string value)
    {
        if (m_filterSetData == null)
            return;

        if (float.TryParse(value, out float bevelThickness))
        {
            m_filterSetData.BevelThickness = bevelThickness;
            SendFilterSetDataChanged(ModelAdjusterBase.FilterProperty.Bevel);
        }
        RefreshFilterSet();
    }
    private void OnInputFieldBevelRotationChange(string value)
    {
    }
    private void OnInputFieldBevelRotationEndEdit(string value)
    {
        if (m_filterSetData == null)
            return;

        if (float.TryParse(value, out float bevelRotation))
        {
            m_filterSetData.BevelRotation = bevelRotation;
            SendFilterSetDataChanged(ModelAdjusterBase.FilterProperty.Bevel);
        }
        RefreshFilterSet();
    }
    private void OnInputFieldBevelSoftnessChange(string value)
    {
    }
    private void OnInputFieldBevelSoftnessEndEdit(string value)
    {
        if (m_filterSetData == null)
            return;

        if (float.TryParse(value, out float bevelSoftness))
        {
            m_filterSetData.BevelSoftness = bevelSoftness;
            SendFilterSetDataChanged(ModelAdjusterBase.FilterProperty.Bevel);
        }
        RefreshFilterSet();
    }

    private void OnColorPickerBevelChanged(Color color)
    {
        if (m_filterSetData == null)
            return;

        m_filterSetData.BevelRed = color.r * 255.0f;
        m_filterSetData.BevelGreen = color.g * 255.0f;
        m_filterSetData.BevelBlue = color.b * 255.0f;
        SendFilterSetDataChanged(ModelAdjusterBase.FilterProperty.Bevel);
        RefreshFilterSet();
    }
    private void OnToggleOldFilmChange(bool value)
    {
        if (m_filterSetData == null)
            return;

        m_filterSetData.OldFilm = value;
        SendFilterSetDataChanged(ModelAdjusterBase.FilterProperty.OldFilm);
        RefreshFilterSet();
    }
    private void OnToggleDotFilmChange(bool value)
    {
        if (m_filterSetData == null)
            return;

        m_filterSetData.DotFilm = value;
        SendFilterSetDataChanged(ModelAdjusterBase.FilterProperty.DotFilm);
        RefreshFilterSet();
    }
    private void OnToggleReflectionFilmChange(bool value)
    {
        if (m_filterSetData == null)
            return;

        m_filterSetData.ReflectionFilm = value;
        SendFilterSetDataChanged(ModelAdjusterBase.FilterProperty.ReflectionFilm);
        RefreshFilterSet();
    }
    private void OnToggleGlitchFilmChange(bool value)
    {
        if (m_filterSetData == null)
            return;

        m_filterSetData.GlitchFilm = value;
        SendFilterSetDataChanged(ModelAdjusterBase.FilterProperty.GlitchFilm);
        RefreshFilterSet();
    }
    private void OnToggleRgbFilmChange(bool value)
    {
        if (m_filterSetData == null)
            return;

        m_filterSetData.RgbFilm = value;
        SendFilterSetDataChanged(ModelAdjusterBase.FilterProperty.RgbFilm);
        RefreshFilterSet();
    }
    private void OnToggleGodrayFilmChange(bool value)
    {
        if (m_filterSetData == null)
            return;

        m_filterSetData.GodrayFilm = value;
        SendFilterSetDataChanged(ModelAdjusterBase.FilterProperty.GodrayFilm);
        RefreshFilterSet();
    }
    #endregion

    public override void OnPageShown()
    {
        base.OnPageShown();
        TryInitAsModel();
    }

    private void TryInitAsModel()
    {
        var model = MainControl.Instance.curTarget;
        if (model == null)
        {
            return;
        }
        m_model = model;
        m_filterSetData = model.filterSetData;
        m_bgContainer = null;
        RefreshFilterSet();
    }

    public void TryInitAsBG()
    {
        var bg = MainControl.Instance.bgContainer;
        if (bg == null)
        {
            return;
        }
        m_filterSetData = bg.filterSetData;
        m_model = null;
        m_bgContainer = bg;
        RefreshFilterSet();
    }

    private void SendFilterSetDataChanged(ModelAdjusterBase.FilterProperty property)
    {
        if (m_bgContainer != null)
        {
            m_bgContainer.OnFilterSetDataChanged(property);
        }
        else if (m_model != null)
        {
            m_model.OnFilterSetDataChanged(property);
        }
    }

    private void AskTargetApplyFilterSetData()
    {
        if (m_bgContainer != null)
        {
            m_bgContainer.UpdateAllFilter();
        }
        else if (m_model != null)
        {
            m_model.UpdateAllFilter();
        }   
    }

    public void RefreshFilterSet()
    {
        var filterSetData = m_filterSetData;
        if (filterSetData == null)
        {
            return;
        }

        m_iptAlpha.SetTextWithoutNotify(filterSetData.Alpha.ToString("F2"));
        m_iptBlur.SetTextWithoutNotify(filterSetData.Blur.ToString());
        
        m_iptBrightness.SetTextWithoutNotify(filterSetData.Brightness.ToString("F2"));
        m_iptContrast.SetTextWithoutNotify(filterSetData.Contrast.ToString("F2"));
        m_iptSaturation.SetTextWithoutNotify(filterSetData.Saturation.ToString("F2"));
        m_iptGamma.SetTextWithoutNotify(filterSetData.Gamma.ToString("F2"));
        m_colAdjustment.color = new Color(
            filterSetData.ColorRed / 255.0f,
            filterSetData.ColorGreen / 255.0f,
            filterSetData.ColorBlue / 255.0f
        );
        
        m_iptBloom.SetTextWithoutNotify(filterSetData.Bloom.ToString("F2"));
        m_iptBloomBrightness.SetTextWithoutNotify(filterSetData.BloomBrightness.ToString("F2"));
        m_iptBloomBlur.SetTextWithoutNotify(filterSetData.BloomBlur.ToString("F2"));
        m_iptBloomThreshold.SetTextWithoutNotify(filterSetData.BloomThreshold.ToString("F2"));
        
        m_toggleBevelLegacy.SetIsOnWithoutNotify(filterSetData.BevelLegacy);
        m_iptBevel.SetTextWithoutNotify(filterSetData.Bevel.ToString("F2"));
        m_iptBevelThickness.SetTextWithoutNotify(filterSetData.BevelThickness.ToString("F2"));
        m_iptBevelRotation.SetTextWithoutNotify(filterSetData.BevelRotation.ToString("F2"));
        m_iptBevelSoftness.SetTextWithoutNotify(filterSetData.BevelSoftness.ToString("F2"));
        m_colBevel.color = new Color(
            filterSetData.BevelRed / 255.0f,
            filterSetData.BevelGreen / 255.0f,
            filterSetData.BevelBlue / 255.0f
        );
            
        m_toggleOldFilm.SetIsOnWithoutNotify(filterSetData.OldFilm);
        m_toggleDotFilm.SetIsOnWithoutNotify(filterSetData.DotFilm);
        m_toggleReflectionFilm.SetIsOnWithoutNotify(filterSetData.ReflectionFilm);
        m_toggleGlitchFilm.SetIsOnWithoutNotify(filterSetData.GlitchFilm);
        m_toggleRgbFilm.SetIsOnWithoutNotify(filterSetData.RgbFilm);
        m_toggleGodrayFilm.SetIsOnWithoutNotify(filterSetData.GodrayFilm);
    }

}

public class PageCharacterPreview : UIPageWidget<PageCharacterPreview>
{
    #region auto generated members
    private Button m_btnCopyMotionExp;
    private Button m_btnCopyTransform;
    private Button m_btnCopyAll;
    private Button m_btnCopyAllSpilt;
    private InputField m_iptFilterMotion;
    private ScrollRect m_scrollMotion;
    private Transform m_tfMotionItems;
    private Transform m_itemMotion;
    private InputField m_iptFilterExpression;
    private ScrollRect m_scrollExpression;
    private Transform m_tfExpressionItems;
    private Transform m_itemExpression;
    #endregion

    #region auto generated binders
    protected override void CodeGenBindMembers()
    {
        m_btnCopyMotionExp = transform.Find("CopyCommand/Container/m_btnCopyMotionExp").GetComponent<Button>();
        m_btnCopyTransform = transform.Find("CopyCommand/Container/m_btnCopyTransform").GetComponent<Button>();
        m_btnCopyAll = transform.Find("CopyCommand/Container/m_btnCopyAll").GetComponent<Button>();
        m_btnCopyAllSpilt = transform.Find("CopyCommand/Container/m_btnCopyAllSpilt").GetComponent<Button>();
        m_iptFilterMotion = transform.Find("Motion/Title/InputField/m_iptFilterMotion").GetComponent<InputField>();
        m_scrollMotion = transform.Find("Motion/Container/m_scrollMotion").GetComponent<ScrollRect>();
        m_tfMotionItems = transform.Find("Motion/Container/m_scrollMotion/Viewport/m_tfMotionItems").GetComponent<Transform>();
        m_itemMotion = transform.Find("Motion/Container/m_scrollMotion/Viewport/m_tfMotionItems/m_itemMotion").GetComponent<Transform>();
        m_iptFilterExpression = transform.Find("Expression/Title/InputField/m_iptFilterExpression").GetComponent<InputField>();
        m_scrollExpression = transform.Find("Expression/Container/m_scrollExpression").GetComponent<ScrollRect>();
        m_tfExpressionItems = transform.Find("Expression/Container/m_scrollExpression/Viewport/m_tfExpressionItems").GetComponent<Transform>();
        m_itemExpression = transform.Find("Expression/Container/m_scrollExpression/Viewport/m_tfExpressionItems/m_itemExpression").GetComponent<Transform>();

        m_btnCopyMotionExp.onClick.AddListener(OnButtonCopyMotionExpClick);
        m_btnCopyTransform.onClick.AddListener(OnButtonCopyTransformClick);
        m_btnCopyAll.onClick.AddListener(OnButtonCopyAllClick);
        m_btnCopyAllSpilt.onClick.AddListener(OnButtonCopyAllSpiltClick);
        m_iptFilterMotion.onValueChanged.AddListener(OnInputFieldFilterMotionChange);
        m_iptFilterMotion.onEndEdit.AddListener(OnInputFieldFilterMotionEndEdit);
        m_iptFilterExpression.onValueChanged.AddListener(OnInputFieldFilterExpressionChange);
        m_iptFilterExpression.onEndEdit.AddListener(OnInputFieldFilterExpressionEndEdit);
    }
    #endregion


    #region auto-generated code event

    private void OnButtonCopyMotionExpClick()
    {
        MainControl.Instance.CopyMotion();
    }
    private void OnButtonCopyTransformClick()
    {
        MainControl.Instance.CopyTransform();
    }
    private void OnButtonCopyAllClick()
    {
        MainControl.Instance.CopyAll();
    }
    private void OnButtonCopyAllSpiltClick()
    {
        MainControl.Instance.CopyAllSpilt();
    }
    private void OnInputFieldFilterMotionChange(string value)
    {
        var model = MainControl.Instance.curTarget;
        if (model == null || !model.HasMotions)
        {
            return;
        }
        model.meta.m_filterMotion = value;
        RefreshMotionList();
    }
    private void OnInputFieldFilterMotionEndEdit(string value)
    {
        // Debug.Log("OnInputFieldFilterMotionEndEdit");
    }
    private void OnInputFieldFilterExpressionChange(string value)
    {
        var model = MainControl.Instance.curTarget;
        if (model == null || !model.HasMotions)
        {
            return;
        }
        model.meta.m_filterExp = value;
        RefreshExpressionList();
    }
    private void OnInputFieldFilterExpressionEndEdit(string value)
    {
        // Debug.Log("OnInputFieldFilterExpressionEndEdit");
    }

    #endregion
    
    private List<MotionEntryWidget> m_listMotion = new List<MotionEntryWidget>();
    private List<MotionEntryWidget> m_listExpression = new List<MotionEntryWidget>();


    public override void OnPageShown()
    {
        base.OnPageShown();
        var model = MainControl.Instance.curTarget;
        if (model == null || !model.HasMotions)
        {
            RefreshMotionList();
            RefreshExpressionList();
            return;
        }
        m_iptFilterMotion.SetTextWithoutNotify(model.meta.m_filterMotion);
        m_iptFilterExpression.SetTextWithoutNotify(model.meta.m_filterExp);
        RefreshMotionList();
        RefreshExpressionList();
        model.SetDisplayMode(ModelDisplayMode.Normal);
    }

    public void RefreshMotionList()
    {
        var model = MainControl.Instance.curTarget;
        if (model == null || !model.HasMotions)
        {
            SetListItem(m_listMotion, m_itemMotion.gameObject, m_tfMotionItems, 0, OnMotionItemCreate);
            return;
        }

        var pairs = model.MotionPairs;

        if (!string.IsNullOrEmpty(model.meta.m_filterMotion))
        {
            var filters = model.meta.m_filterMotion.Split(' ').Where(s => !string.IsNullOrEmpty(s)).Select(s => s.ToLower());
            pairs = pairs.Where(x => filters.All(f => x.name.ToLower().Contains(f))).ToList();
        }

        SetListItem(m_listMotion, m_itemMotion.gameObject, m_tfMotionItems, pairs.Count, OnMotionItemCreate);
        var selectedMotion = model.curMotionName;
        for (int i = 0; i < pairs.Count; i++)
        {
            var item = m_listMotion[i];
            var pair = pairs[i];
            item.SetData(pair.name);
            if (pair.name == selectedMotion)
            {
                item.SetStateStyle(UIStateStyle.UIState.Selected);
            }
            else
            {
                item.SetStateStyle(UIStateStyle.UIState.Normal);
            }
        }
    }

    public void RefreshAll()
    {
        UpdateFilterText();
        RefreshExpressionList();
        RefreshMotionList();
    }

    private void UpdateFilterText()
    {
        var model = MainControl.Instance.curTarget;
        if (model == null || !model.HasMotions)
        {
            return;
        }
        m_iptFilterMotion.SetTextWithoutNotify(model.meta.m_filterMotion);
        m_iptFilterExpression.SetTextWithoutNotify(model.meta.m_filterExp);
    }

    public void RefreshExpressionList()
    {
        var model = MainControl.Instance.curTarget;
        if (model == null || !model.HasMotions)
        {
            SetListItem(m_listExpression, m_itemExpression.gameObject, m_tfExpressionItems, 0, OnExpressionItemCreate);
            return;
        }

        var pairs = model.ExpPairs;

        if (!string.IsNullOrEmpty(model.meta.m_filterExp))
        {
            var filters = model.meta.m_filterExp.Split(' ').Where(s => !string.IsNullOrEmpty(s)).Select(s => s.ToLower());
            pairs = pairs.Where(x => filters.All(f => x.name.ToLower().Contains(f))).ToList();
        }

        SetListItem(m_listExpression, m_itemExpression.gameObject, m_tfExpressionItems, pairs.Count, OnExpressionItemCreate);
        var selectedExpression = model.curExpName;
        for (int i = 0; i < pairs.Count; i++)
        {
            var item = m_listExpression[i];
            var pair = pairs[i];
            item.SetData(pair.name);
            if (pair.name == selectedExpression)
            {
                item.SetStateStyle(UIStateStyle.UIState.Selected);
            }
            else
            {
                item.SetStateStyle(UIStateStyle.UIState.Normal);
            }
        }
    }

    private void OnExpressionItemCreate(MotionEntryWidget widget)
    {
        widget.AddClickEvent(() => OnExpressionClicked(widget));
    }

    private void OnExpressionClicked(MotionEntryWidget widget)
    {
        MainControl.Instance.PlayExp(widget.name);
        RefreshExpressionList();
    }

    private void OnMotionItemCreate(MotionEntryWidget widget)
    {
        widget.AddClickEvent(() => OnMotionClicked(widget));
    }

    private void OnMotionClicked(MotionEntryWidget widget)
    {
        MainControl.Instance.PlayMotion(widget.name);
        RefreshMotionList();
    }
}

public class PageExpressionEditor : UIPageWidget<PageExpressionEditor>
{
    #region auto generated members
    private Button m_btnCopySelectedExp;
    private Button m_btnCopyExpData;
    private Toggle m_toggleShowSelectedOnly;
    private Toggle m_toggleLock;
    private InputField m_iptFilter;
    private ScrollRect m_scrollExpression;
    private Transform m_tfExpEntries;
    private Transform m_itemExpEntry;
    #endregion

    #region auto generated binders
    protected override void CodeGenBindMembers()
    {
        m_btnCopySelectedExp = transform.Find("QuickCommand/Container/m_btnCopySelectedExp").GetComponent<Button>();
        m_btnCopyExpData = transform.Find("QuickCommand/Container/m_btnCopyExpData").GetComponent<Button>();
        m_toggleShowSelectedOnly = transform.Find("QuickCommand/Container/m_toggleShowSelectedOnly").GetComponent<Toggle>();
        m_toggleLock = transform.Find("QuickCommand/Container/m_toggleLock").GetComponent<Toggle>();
        m_iptFilter = transform.Find("Parameters/Title/InputField/m_iptFilter").GetComponent<InputField>();
        m_scrollExpression = transform.Find("Parameters/Container/m_scrollExpression").GetComponent<ScrollRect>();
        m_tfExpEntries = transform.Find("Parameters/Container/m_scrollExpression/Viewport/m_tfExpEntries").GetComponent<Transform>();
        m_itemExpEntry = transform.Find("Parameters/Container/m_scrollExpression/Viewport/m_tfExpEntries/m_itemExpEntry").GetComponent<Transform>();

        m_btnCopySelectedExp.onClick.AddListener(OnButtonCopySelectedExpClick);
        m_btnCopyExpData.onClick.AddListener(OnButtonCopyExpDataClick);
        m_toggleShowSelectedOnly.onValueChanged.AddListener(OnToggleShowSelectedOnlyChange);
        m_toggleLock.onValueChanged.AddListener(OnToggleLockChange);
        m_iptFilter.onValueChanged.AddListener(OnInputFieldFilterChange);
        m_iptFilter.onEndEdit.AddListener(OnInputFieldFilterEndEdit);
    }
    #endregion



    #region auto-generated code event
    public void OnButtonCopySelectedExpClick()
    {
        var model = MainControl.Instance.curTarget;
        if (model == null || !model.SupportExpressionMode)
        {
            return;
        }
        var curExp = model.CurExp;
        if (curExp != null)
        {
            model.CopyFromExp(curExp);
        }
        else
        {
            MainControl.Instance.ShowErrorDebugText("当前没有表情，请先选一个！");
        }

        RefreshExpEntryList();
    }
    public void OnButtonCopyExpDataClick()
    {
        MainControl.Instance.CopyMotionEditor();
    }
    private void OnToggleShowSelectedOnlyChange(bool value)
    {
        RefreshExpEntryList();
    }
    private void OnToggleLockChange(bool value)
    {
    }
    private void OnInputFieldFilterChange(string value)
    {
        RefreshExpEntryList();
    }
    private void OnInputFieldFilterEndEdit(string value)
    {
    }

    #endregion

    private List<ExpEntryWidget> m_listExpEntry = new List<ExpEntryWidget>();

    public override void OnPageShown()
    {
        base.OnPageShown();
        var model = MainControl.Instance.curTarget;
        if (model != null)
        {
            model.SetDisplayMode(ModelDisplayMode.EmotionEditor);
        }
        RefreshAll();
    }

    public void RefreshAll()
    {
        var model = MainControl.Instance.curTarget;
        if (model == null || !model.SupportExpressionMode)
        {
            RefreshExpEntryList();
            return;
        }

        if (model.DisplayMode != ModelDisplayMode.EmotionEditor)
        {
            return;
        }

        RefreshExpEntryList();
    }

    public void RefreshExpEntryList()
    {
        var model = MainControl.Instance.curTarget;
        if (model == null || !model.SupportExpressionMode)
        {
            SetListItem(m_listExpEntry, m_itemExpEntry.gameObject, m_tfExpEntries, 0, OnExpItemCreate);
            return;
        }

        var expKeyList = model.GetEmotionEditorList().list;

        if (m_toggleShowSelectedOnly.isOn)
        {
            expKeyList = expKeyList.Where(x => model.IsMotionParamSetContains(x.name)).ToList();
        }

        if (!string.IsNullOrEmpty(m_iptFilter.text))
        {
            var filters = m_iptFilter.text.Split(' ').Where(s => !string.IsNullOrEmpty(s)).Select(s => s.ToLower());
            expKeyList = expKeyList.Where(x => filters.All(f => x.name.ToLower().Contains(f))).ToList();
        }

        SetListItem(m_listExpEntry, m_itemExpEntry.gameObject, m_tfExpEntries, expKeyList.Count, OnExpItemCreate);
        for (int i = 0; i < expKeyList.Count; i++)
        {
            var item = m_listExpEntry[i];
            var key = expKeyList[i];
            var value = model.GetMotionParamValue(key.name);
            item.SetData(key.name, value, key.min, key.max);
            bool contain = model.IsMotionParamSetContains(key.name);
            if (contain)
            {
                item.SetStateStyle(UIStateStyle.UIState.Selected);
            }
            else
            {
                item.SetStateStyle(UIStateStyle.UIState.Normal);
            }
        }
    }

    private void OnExpItemCreate(ExpEntryWidget widget)
    {
        widget._OnSliderValueChanged = OnSliderValueChanged;
        widget._OnTitleClick = OnExpClicked;
        widget._OnInputFieldValueEndEdit = OnInputFieldValueEndEdit;
    }

    private void OnExpClicked(ExpEntryWidget widget)
    {
        if (m_toggleLock.isOn)
        {
            return;
        }

        var model = MainControl.Instance.curTarget;
        if (model == null || !model.SupportExpressionMode)
        {
            return;
        }
        bool contain = model.IsMotionParamSetContains(widget.name);
        if (contain)
        {
            model.RemoveMotionParamControl(widget.name);
            model.ApplyMotionParamValue();
        }
        else
        {
            model.AddMotionParamControl(widget.name);
        }
        RefreshExpEntryList();
    }

    private void OnSliderValueChanged(string name, float value)
    {
        var model = MainControl.Instance.curTarget;
        if (model == null)
        {
            return;
        }

        bool contain = model.IsMotionParamSetContains(name);
        if (contain)
        {
            model.SetMotionParamValue(name, value);
            model.ApplyMotionParamValue();
        }
        RefreshExpEntryList();
    }

    private void OnInputFieldValueEndEdit(ExpEntryWidget widget)
    {
        var model = MainControl.Instance.curTarget;
        if (model == null)
        {
            return;
        }
        if (float.TryParse(widget.TextValue, out float value))
        {
            var clampValue = Mathf.Clamp(value, widget.MinValue, widget.MaxValue);
            model.SetMotionParamValue(widget.name, clampValue);
            model.ApplyMotionParamValue();
            RefreshExpEntryList();
        }
        else
        {
            widget.SetText(widget.TextValue);
        }
    }
}

public class ExpEntryWidget : UIItemWidget<ExpEntryWidget>
{

    #region auto generated members
    private Button m_btnTitle;
    private Image m_imgBG;
    private Text m_lblTitle;
    private MonoUIStyle m_styleTitle;
    private Slider m_sliderValue;
    private Image m_imgSliderFill;
    private Image m_imgSliderHandle;
    private Button m_btnPreview;
    private Image m_imgBtnPreview;
    private Text m_lblPreview;
    private MonoUIStyle m_styleButton;
    private RectTransform m_iptField;
    private InputField m_iptValue;
    private Text m_lblMin;
    private Text m_lblMax;
    #endregion

    #region auto generated binders
    protected override void CodeGenBindMembers()
    {
        m_btnTitle = transform.Find("m_btnTitle").GetComponent<Button>();
        m_imgBG = transform.Find("m_btnTitle/m_imgBG").GetComponent<Image>();
        m_lblTitle = transform.Find("m_btnTitle/m_lblTitle").GetComponent<Text>();
        m_styleTitle = transform.Find("m_btnTitle/m_styleTitle").GetComponent<MonoUIStyle>();
        m_sliderValue = transform.Find("Value/m_sliderValue").GetComponent<Slider>();
        m_imgSliderFill = transform.Find("Value/m_sliderValue/Fill Area/m_imgSliderFill").GetComponent<Image>();
        m_imgSliderHandle = transform.Find("Value/m_sliderValue/Handle Slide Area/m_imgSliderHandle").GetComponent<Image>();
        m_btnPreview = transform.Find("Value/m_btnPreview").GetComponent<Button>();
        m_imgBtnPreview = transform.Find("Value/m_btnPreview/m_imgBtnPreview").GetComponent<Image>();
        m_lblPreview = transform.Find("Value/m_btnPreview/m_lblPreview").GetComponent<Text>();
        m_styleButton = transform.Find("Value/m_btnPreview/m_styleButton").GetComponent<MonoUIStyle>();
        m_iptField = transform.Find("m_iptField").GetComponent<RectTransform>();
        m_iptValue = transform.Find("m_iptField/m_iptValue").GetComponent<InputField>();
        m_lblMin = transform.Find("m_iptField/m_lblMin").GetComponent<Text>();
        m_lblMax = transform.Find("m_iptField/m_lblMax").GetComponent<Text>();

        m_btnTitle.onClick.AddListener(OnButtonTitleClick);
        m_sliderValue.onValueChanged.AddListener(OnSliderValueChange);
        m_btnPreview.onClick.AddListener(OnButtonPreviewClick);
        m_iptValue.onValueChanged.AddListener(OnInputFieldValueChange);
        m_iptValue.onEndEdit.AddListener(OnInputFieldValueEndEdit);
    }
    #endregion


    #region auto-generated code event
    public void OnButtonTitleClick()
    {
        _OnTitleClick?.Invoke(this);
    }
    private void OnInputFieldValueChange(string value)
    {
        //empty
    }
    private void OnInputFieldValueEndEdit(string value)
    {
        ShowEdit(false);
        _OnInputFieldValueEndEdit?.Invoke(this);
    }
    private void OnSliderValueChange(float value)
    {
        if (blockSetMinMax)
        {
            return;
        }
        _OnSliderValueChanged?.Invoke(name, value);
    }

    private void OnButtonPreviewClick()
    {
        ShowEdit(true);
    }


    #endregion
    
    public string name;
    public Action<string, float> _OnSliderValueChanged;
    public Action<ExpEntryWidget> _OnTitleClick;
    public Action<ExpEntryWidget> _OnInputFieldValueEndEdit;
    public string TextValue => m_iptValue.text;
    public float MinValue => m_sliderValue.minValue;
    public float MaxValue => m_sliderValue.maxValue;

    private bool blockSetMinMax;

    public override void SetStateStyle(UIStateStyle.UIState state)
    {
        base.SetStateStyle(state);
        StateStyle.SetColor(m_imgBG, state);
        m_styleTitle.style.SetColor(m_lblTitle, state);
        m_styleTitle.style.SetColor(m_imgSliderFill, state);
        m_styleTitle.style.SetColor(m_imgSliderHandle, state);
        m_styleTitle.style.SetColor(m_lblPreview, state);
        m_styleButton.style.SetColor(m_imgBtnPreview, state);

        m_sliderValue.interactable = state == UIStateStyle.UIState.Selected;
        m_btnPreview.interactable = state == UIStateStyle.UIState.Selected;
    }

    public void SetData(string name, float value, float min, float max)
    {
        this.name = name;
        m_lblTitle.text = name;
        blockSetMinMax = true;
        m_sliderValue.minValue = min;
        m_sliderValue.maxValue = max;
        blockSetMinMax = false;
        m_sliderValue.SetValueWithoutNotify(value);
        m_iptValue.SetTextWithoutNotify(value.ToString("F2"));
        m_lblPreview.text = value.ToString("F2");
        m_lblMin.text = min.ToString("F2");
        m_lblMax.text = max.ToString("F2");
        ShowEdit(false);
    }

    public void SetText(string text)
    {
        m_iptValue.SetTextWithoutNotify(text);
    }

    public void ShowEdit(bool show)
    {
        m_iptField.gameObject.SetActive(show);
        m_sliderValue.gameObject.SetActive(!show);
        m_btnPreview.gameObject.SetActive(!show);
        m_btnTitle.gameObject.SetActive(!show);

        if (show)
        {
            EventSystem.current.SetSelectedGameObject(m_iptValue.gameObject);
        }
    }
}

public class PageGroupMenu : UIPageWidget<PageGroupMenu>
{
    #region auto generated members
    private Button m_btnAddGroup;
    private ScrollRect m_scrollGroup;
    private Transform m_tfGroupItems;
    private Transform m_itemGroup;
    private Text m_lblTitle;
    private InputField m_iptGroupName;
    private Transform m_itemPosX;
    private Transform m_itemPosY;
    private Transform m_itemScale;
    private Transform m_itemRot;
    private Button m_btnSetPivotCenter;
    private Button m_btnApply;
    private Button m_btnHelpAutoApply;
    private Toggle m_toggleAutoApply;
    #endregion

    #region auto generated binders
    protected override void CodeGenBindMembers()
    {
        m_btnAddGroup = transform.Find("m_btnAddGroup").GetComponent<Button>();
        m_scrollGroup = transform.Find("m_scrollGroup").GetComponent<ScrollRect>();
        m_tfGroupItems = transform.Find("m_scrollGroup/Viewport/m_tfGroupItems").GetComponent<Transform>();
        m_itemGroup = transform.Find("m_scrollGroup/Viewport/m_tfGroupItems/m_itemGroup").GetComponent<Transform>();
        m_lblTitle = transform.Find("Properties/Container/GroupName/Label/m_lblTitle").GetComponent<Text>();
        m_iptGroupName = transform.Find("Properties/Container/GroupName/Value/InputField/m_iptGroupName").GetComponent<InputField>();
        m_itemPosX = transform.Find("Properties/Container/m_itemPosX").GetComponent<Transform>();
        m_itemPosY = transform.Find("Properties/Container/m_itemPosY").GetComponent<Transform>();
        m_itemScale = transform.Find("Properties/Container/m_itemScale").GetComponent<Transform>();
        m_itemRot = transform.Find("Properties/Container/m_itemRot").GetComponent<Transform>();
        m_btnSetPivotCenter = transform.Find("Properties/Container/m_btnSetPivotCenter").GetComponent<Button>();
        m_btnApply = transform.Find("Properties/Container/m_btnApply").GetComponent<Button>();
        m_btnHelpAutoApply = transform.Find("Properties/Container/m_btnApply/m_btnHelpAutoApply").GetComponent<Button>();
        m_toggleAutoApply = transform.Find("Properties/Container/m_toggleAutoApply").GetComponent<Toggle>();

        m_btnAddGroup.onClick.AddListener(OnButtonAddGroupClick);
        m_iptGroupName.onValueChanged.AddListener(OnInputFieldGroupNameChange);
        m_iptGroupName.onEndEdit.AddListener(OnInputFieldGroupNameEndEdit);
        m_btnSetPivotCenter.onClick.AddListener(OnButtonSetPivotCenterClick);
        m_btnApply.onClick.AddListener(OnButtonApplyClick);
        m_btnHelpAutoApply.onClick.AddListener(OnButtonHelpAutoApplyClick);
        m_toggleAutoApply.onValueChanged.AddListener(OnToggleAutoApplyChange);
    }
    #endregion

    #region auto generated events
    private void OnButtonAddGroupClick()
    {
        MainControl.Instance.AddGroup();
        RefreshAll();
    }

    private void OnButtonSetPivotCenterClick()
    {
        var group = MainControl.Instance.curGroup;
        if (group == null)
        {
            return;
        }
        
        group.RemoveInvalidModel();
        if (group.modelAdjusters.Count == 0)
            return;

        var worldPosition = Vector3.zero;
        foreach (var model in group.modelAdjusters)
        {
            worldPosition += model.MainPos.position;
        }
        worldPosition /= group.modelAdjusters.Count;
        group.SetPivotPositon(worldPosition);
    }

    private void OnInputFieldGroupNameChange(string value)
    {
        //empty
    }

    private void OnInputFieldGroupNameEndEdit(string value)
    {
        var group = MainControl.Instance.curGroup;
        if (group == null)
        {
            return;
        }

        if (string.IsNullOrEmpty(value))
        {
            m_iptGroupName.SetTextWithoutNotify(group.groupName);
            return;
        }

        group.groupName = value;
        RefreshAll();
    }

    private void OnButtonApplyClick()
    {
        var group = MainControl.Instance.curGroup;
        if (group == null)
        {
            return;
        }
        group.ApplyGroup();
        RefreshAll();
    }

    private void OnToggleAutoApplyChange(bool value)
    {
        var group = MainControl.Instance.curGroup;
        if (group == null)
        {
            return;
        }
        group.autoApply = value;
        RefreshAll();
    }

    private void OnButtonHelpAutoApplyClick()
    {
        // res in chinese
        MessageTipWindow.Instance.Show("自动应用", "当你在修改组的时候，会自动将组的缩放和旋转回到默认值\n但是凡事都有例外，你可能想要精确操纵某个数值\n所以这里提供了新的功能让你自己根据需要调整。");
    }

    #endregion

    private PageGroupFunctions m_pageGroupFunctions;
    private Transform m_tfCursor;
    private LabelInputFieldWidget m_liptPosX;
    private LabelInputFieldWidget m_liptPosY;
    private LabelInputFieldWidget m_liptScale;
    private LabelInputFieldWidget m_liptRot;

    protected override void OnInit()
    {
        base.OnInit();
        m_liptPosX = LabelInputFieldWidget.CreateWidget(m_itemPosX.gameObject);
        m_liptPosY = LabelInputFieldWidget.CreateWidget(m_itemPosY.gameObject);
        m_liptScale = LabelInputFieldWidget.CreateWidget(m_itemScale.gameObject);
        m_liptRot = LabelInputFieldWidget.CreateWidget(m_itemRot.gameObject);

        m_liptPosX.SetDataSubmit(OnGroupPosXSubmit);
        m_liptPosY.SetDataSubmit(OnGroupPosYSubmit);
        m_liptPosX.SetToggleChange(OnLockXChange);
        m_liptPosY.SetToggleChange(OnLockYChange);

        m_liptRot.SetDataSubmit(OnGroupRotSubmit);
        m_liptScale.SetDataSubmit(OnGroupScaleSubmit);
    }

    public void Inject(PageGroupFunctions pageGroupFunctions, Transform tfCursor)
    {
        m_pageGroupFunctions = pageGroupFunctions;
        m_tfCursor = tfCursor;
    }

    public override void OnPageShown()
    {
        base.OnPageShown();
        UIEventBus.AddListener(UIEventType.GroupTransformChanged, OnGroupTransformChanged);
        UIEventBus.AddListener(UIEventType.CameraTransformChanged, OnCameraTransformChanged);
        UIEventBus.AddListener(UIEventType.LockXChanged, RefreshGroupUI);
        UIEventBus.AddListener(UIEventType.LockYChanged, RefreshGroupUI);
        MainControl.Instance.editType = EditType.Group;
        RefreshAll();
    }

    public override void OnPageHidden()
    {
        base.OnPageHidden();
        UIEventBus.RemoveListener(UIEventType.GroupTransformChanged, OnGroupTransformChanged);
        UIEventBus.RemoveListener(UIEventType.CameraTransformChanged, OnCameraTransformChanged);
        UIEventBus.RemoveListener(UIEventType.LockXChanged, RefreshGroupUI);
        UIEventBus.RemoveListener(UIEventType.LockYChanged, RefreshGroupUI);
        m_tfCursor.gameObject.SetActive(false);
    }

    private void OnGroupTransformChanged()
    {
        UpdateCursorPosition();
        RefreshGroupUI();
    }

    private void OnCameraTransformChanged()
    {
        UpdateCursorPosition();
    }

    private void UpdateCursorPosition()
    {
        if (MainControl.Instance.curGroup == null)
        {
            m_tfCursor.gameObject.SetActive(false);
            return;
        }
        var screenPos = MainControl.Instance.mainCamera.WorldToScreenPoint(MainControl.Instance.curGroup.root.position);
        screenPos.z = m_tfCursor.position.z;
        m_tfCursor.gameObject.SetActive(true);
        m_tfCursor.position = screenPos;
    }

    private void RefreshGroupUI()
    {
        var group = MainControl.Instance.curGroup;
        if (group == null)
        {
            return;
        }

        var localPos = group.LocalPosition;
        m_liptPosX.SetData(localPos.x.ToString());
        m_liptPosY.SetData(localPos.y.ToString());

        m_liptPosX.SetToggleValue(MainControl.Instance.LockX);
        m_liptPosY.SetToggleValue(MainControl.Instance.LockY);

        m_liptRot.SetData(group.RotationDeg.ToString("F2"));
        m_liptScale.SetData(group.Scale.ToString("F2"));

        m_toggleAutoApply.SetIsOnWithoutNotify(group.autoApply);
    }

    private void OnGroupPosXSubmit(string x)
    {
        var group = MainControl.Instance.curGroup;
        if (group == null)
        {
            return;
        }
        var localPos = group.LocalPosition;
        if (float.TryParse(x, out float xValue))
        {
            group.SetLocalPosition(new Vector3(xValue, localPos.y, localPos.z));
        }
    }

    private void OnGroupPosYSubmit(string y)
    {
        var group = MainControl.Instance.curGroup;
        if (group == null)
        {
            return;
        }
        var localPos = group.LocalPosition;
        if (float.TryParse(y, out float yValue))
        {
            group.SetLocalPosition(new Vector3(localPos.x, yValue, localPos.z));
        }
    }

    private void OnLockXChange(Toggle toggle, bool value)
    {
        MainControl.Instance.LockX = value;
        UIEventBus.SendEvent(UIEventType.LockXChanged);
    }

    private void OnLockYChange(Toggle toggle, bool value)
    {
        MainControl.Instance.LockY = value;
        UIEventBus.SendEvent(UIEventType.LockYChanged);
    }

    private void OnGroupRotSubmit(string rot)
    {
        var group = MainControl.Instance.curGroup;
        if (group == null)
        {
            return;
        }
        if (float.TryParse(rot, out float rotValue))
        {
            group.SetRotation(rotValue);
        }
        RefreshGroupUI();
    }

    private void OnGroupScaleSubmit(string scale)
    {
        var group = MainControl.Instance.curGroup;
        if (group == null)
        {
            return;
        }
        if (float.TryParse(scale, out float scaleValue))
        {
            group.SetScale(scaleValue);
        }
        RefreshGroupUI();
    }

    private List<GroupItemWidget> m_listGroupItem = new List<GroupItemWidget>();
    public void RefreshAll()
    {
        var curGroup = MainControl.Instance.curGroup;
        var allGroups = MainControl.Instance.modelGroups;
        SetListItem(m_listGroupItem, m_itemGroup.gameObject, m_tfGroupItems, allGroups.Count, OnGroupItemCreate);
        for (int i = 0; i < allGroups.Count; i++)
        {
            var group = allGroups[i];
            var item = m_listGroupItem[i];
            item.SetData(group);
            bool isCurGroup = curGroup == group;
            item.SetStateStyle(isCurGroup ? UIStateStyle.UIState.Selected : UIStateStyle.UIState.Normal);
        }
        m_pageGroupFunctions.RefreshAll();
        UpdateCursorPosition();
        RefreshGroupUI();

        if (curGroup != null)
        {
            m_iptGroupName.SetTextWithoutNotify(curGroup.groupName);
        }
    }

    private void OnGroupItemCreate(GroupItemWidget widget)
    {
        widget.AddClickEvent(() => OnGroupItemClicked(widget));
        widget._OnDeleteClick = OnGroupItemDelete;
    }

    private void OnGroupItemClicked(GroupItemWidget widget)
    {
        MainControl.Instance.SetGroup(widget.group);
        RefreshAll();
    }

    private void OnGroupItemDelete(GroupItemWidget widget)
    {
        MainControl.Instance.RemoveGroup(widget.group);
        RefreshAll();
    }
}

public class GroupItemWidget : UIItemWidget<GroupItemWidget>
{
    public ModelGroup group;
    #region auto-generated code
    private Text m_lblTitle;
    private Button m_btnDelete;

    protected override void CodeGenBindMembers()
    {
        m_lblTitle = transform.Find("m_lblTitle").GetComponent<Text>();
        m_btnDelete = transform.Find("m_btnDelete").GetComponent<Button>();
        m_btnDelete.onClick.AddListener(OnDeleteClick);
    }
    #endregion
    #region auto-generated code event
    public void OnDeleteClick()
    {
        _OnDeleteClick?.Invoke(this);
    }
    #endregion

    public Action<GroupItemWidget> _OnDeleteClick;

    public void SetData(ModelGroup group)
    {
        this.group = group;
        m_lblTitle.text = group.groupName;
    }

    public override void SetStateStyle(UIStateStyle.UIState state)
    {
        base.SetStateStyle(state);
        StateStyle.SetActiveObject(state);
    }
}

public class PageGroupFunctions : UIPageWidget<PageGroupFunctions>
{
    #region auto generated members
    private Button m_btnCopyMotionExp;
    private Button m_btnCopyTransform;
    private Button m_btnCopyAll;
    private Button m_btnCopyAllSpilt;
    private Transform m_itemBackgroundTransform;
    private Transform m_itemBackgroundTransformCopy;
    private ScrollRect m_scrollChara;
    private Transform m_tfCharas;
    private Transform m_itemSelectedChara;
    #endregion

    #region auto generated binders
    protected override void CodeGenBindMembers()
    {
        m_btnCopyMotionExp = transform.Find("CopyCommand/Container/m_btnCopyMotionExp").GetComponent<Button>();
        m_btnCopyTransform = transform.Find("CopyCommand/Container/m_btnCopyTransform").GetComponent<Button>();
        m_btnCopyAll = transform.Find("CopyCommand/Container/m_btnCopyAll").GetComponent<Button>();
        m_btnCopyAllSpilt = transform.Find("CopyCommand/Container/m_btnCopyAllSpilt").GetComponent<Button>();
        m_itemBackgroundTransform = transform.Find("Special/Container/m_itemBackgroundTransform").GetComponent<Transform>();
        m_itemBackgroundTransformCopy = transform.Find("Special/Container/m_itemBackgroundTransformCopy").GetComponent<Transform>();
        m_scrollChara = transform.Find("CharacterList/Container/m_scrollChara").GetComponent<ScrollRect>();
        m_tfCharas = transform.Find("CharacterList/Container/m_scrollChara/Viewport/m_tfCharas").GetComponent<Transform>();
        m_itemSelectedChara = transform.Find("CharacterList/Container/m_scrollChara/Viewport/m_tfCharas/m_itemSelectedChara").GetComponent<Transform>();

        m_btnCopyMotionExp.onClick.AddListener(OnButtonCopyMotionExpClick);
        m_btnCopyTransform.onClick.AddListener(OnButtonCopyTransformClick);
        m_btnCopyAll.onClick.AddListener(OnButtonCopyAllClick);
        m_btnCopyAllSpilt.onClick.AddListener(OnButtonCopyAllSpiltClick);
    }
    #endregion


    #region auto-generated code event
    public void OnButtonCopyMotionExpClick()
    {
        MainControl.Instance.CopyMotionGroup();
    }
    public void OnButtonCopyTransformClick()
    {
        MainControl.Instance.CopyTransformGroup();
    }
    public void OnButtonCopyAllClick()
    {
        MainControl.Instance.CopyAllGroup();
    }
    private void OnButtonCopyAllSpiltClick()
    {
        MainControl.Instance.CopyAllGroupSpilt();
    }

    #endregion

    
    private List<GroupSelectedItemWidget> m_listGroupItem = new List<GroupSelectedItemWidget>();
    private GroupSelectedItemWidget m_itemBackground;
    private GroupSelectedItemWidget m_itemBackgroundCopy;

    protected override void OnInit()
    {
        base.OnInit();
        m_itemBackground = GroupSelectedItemWidget.CreateWidget(m_itemBackgroundTransform.gameObject);
        m_itemBackground._OnTitleClick = OnBackgroundItemClicked;

        m_itemBackgroundCopy = GroupSelectedItemWidget.CreateWidget(m_itemBackgroundTransformCopy.gameObject);
        m_itemBackgroundCopy._OnTitleClick = OnBackgroundCopyItemClicked;
    }
    public override void OnPageShown()
    {
        base.OnPageShown();
        RefreshAll();
    }

    public void RefreshAll()
    {
        ModelGroup group = MainControl.Instance.curGroup;
        if (group == null)
        {
            SetListItem(m_listGroupItem, m_itemSelectedChara.gameObject, m_tfCharas, 0, OnGroupItemCreate);
            return;
        }
        
        var allModels = MainControl.Instance.models;
        SetListItem(m_listGroupItem, m_itemSelectedChara.gameObject, m_tfCharas, allModels.Count, OnGroupItemCreate);
        for (int i = 0; i < allModels.Count; i++)
        {
            var model = allModels[i];
            var groupItem = m_listGroupItem[i];
            groupItem.SetData(model.Name);
            bool modelInGroup = group.modelAdjusters.Contains(model);
            groupItem.SetStateStyle(modelInGroup ? UIStateStyle.UIState.Selected : UIStateStyle.UIState.Normal);
        }

        m_itemBackground.SetStateStyle(group.containBackground ? UIStateStyle.UIState.Selected : UIStateStyle.UIState.Normal);
        m_itemBackgroundCopy.SetStateStyle(group.containBackgroundCopy ? UIStateStyle.UIState.Selected : UIStateStyle.UIState.Normal);
    }

    private void OnGroupItemCreate(GroupSelectedItemWidget widget)
    {
        widget._OnTitleClick = OnGroupItemClicked;
    }

    private void OnGroupItemClicked(GroupSelectedItemWidget widget)
    {
        ModelGroup group = MainControl.Instance.curGroup;
        if (group == null)
        {
            return;
        }
        var model = MainControl.Instance.models[GetListItemIndex(m_listGroupItem, widget)];
        bool contain = group.modelAdjusters.Contains(model);
        if (contain)
        {
            group.modelAdjusters.Remove(model);
        }
        else
        {
            MainControl.Instance.AddModelToGroup(group, model);
        }
        RefreshAll();
    }

    private void OnBackgroundItemClicked(GroupSelectedItemWidget widget)
    {
        var group = MainControl.Instance.curGroup;
        if (group == null)
        {
            return;
        }
        group.containBackground = !group.containBackground;
        RefreshAll();
    }

    private void OnBackgroundCopyItemClicked(GroupSelectedItemWidget widget)
    {
        var group = MainControl.Instance.curGroup;
        if (group == null)
        {
            return;
        }
        group.containBackgroundCopy = !group.containBackgroundCopy;
        RefreshAll();
    }
}

public class GroupSelectedItemWidget : UIItemWidget<GroupSelectedItemWidget>
{
    #region auto-generated code
    private Button m_btnTitle;
    private Image m_imgBG;
    private Text m_lblTitle;
    private MonoUIStyle m_styleTitle;

    protected override void CodeGenBindMembers()
    {
        m_btnTitle = transform.Find("m_btnTitle").GetComponent<Button>();
        m_imgBG = transform.Find("m_btnTitle/m_imgBG").GetComponent<Image>();
        m_lblTitle = transform.Find("m_btnTitle/m_lblTitle").GetComponent<Text>();
        m_styleTitle = transform.Find("m_btnTitle/m_styleTitle").GetComponent<MonoUIStyle>();
        m_btnTitle.onClick.AddListener(OnTitleClick);
    }
    #endregion
    #region auto-generated code event
    public void OnTitleClick()
    {
        _OnTitleClick?.Invoke(this);
    }
    #endregion

    public Action<GroupSelectedItemWidget> _OnTitleClick;

    public void SetData(string name)
    {
        m_lblTitle.text = name;
    }

    public override void SetStateStyle(UIStateStyle.UIState state)
    {
        base.SetStateStyle(state);
        StateStyle.SetColor(m_imgBG, state);
        m_styleTitle.style.SetColor(m_lblTitle, state);
    }
}


public class PageBackgroundMenu : UIPageWidget<PageBackgroundMenu>
{
    #region auto generated members
    private Button m_btnChange;
    private Transform m_itemPosX;
    private Transform m_itemPosY;
    private Transform m_itemScale;
    private Transform m_itemRotation;
    #endregion

    #region auto generated binders
    protected override void CodeGenBindMembers()
    {
        m_btnChange = transform.Find("m_btnChange").GetComponent<Button>();
        m_itemPosX = transform.Find("Properties/Container/m_itemPosX").GetComponent<Transform>();
        m_itemPosY = transform.Find("Properties/Container/m_itemPosY").GetComponent<Transform>();
        m_itemScale = transform.Find("Properties/Container/m_itemScale").GetComponent<Transform>();
        m_itemRotation = transform.Find("Properties/Container/m_itemRotation").GetComponent<Transform>();

        m_btnChange.onClick.AddListener(OnButtonChangeClick);
    }
    #endregion

    #region auto generated events
    private void OnButtonChangeClick()
    {
        if (string.IsNullOrEmpty(Global.BGPath))
        {
            MessageTipWindow.Instance.Show("错误", "请先设置背景文件夹");
            return;
        }

        var result = L2DWUtils.OpenFileDialog("选择背景文件夹", "BGPath", "png|jpg");
        if(result.Length > 0)
        {
            MainControl.Instance.LoadBackground(result[0]);
        }
    }
    #endregion

    private PageBackgroundFunctions m_pageBackgroundFunctions;
    public void Inject(PageBackgroundFunctions pageBackgroundFunctions)
    {
        m_pageBackgroundFunctions = pageBackgroundFunctions;
    }

    private LabelInputFieldWidget m_liptPosX;
    private LabelInputFieldWidget m_liptPosY;
    private LabelInputFieldWidget m_liptScale;
    private LabelInputFieldWidget m_liptRotation;
    protected override void OnInit()
    {
        base.OnInit();
        m_liptPosX = LabelInputFieldWidget.CreateWidget(m_itemPosX.gameObject);
        m_liptPosY = LabelInputFieldWidget.CreateWidget(m_itemPosY.gameObject);
        m_liptScale = LabelInputFieldWidget.CreateWidget(m_itemScale.gameObject);
        m_liptRotation = LabelInputFieldWidget.CreateWidget(m_itemRotation.gameObject);

        m_liptPosX.SetDataSubmit(OnPosXSubmit);
        m_liptPosY.SetDataSubmit(OnPosYSubmit);
        m_liptScale.SetDataSubmit(OnScaleSubmit);
        m_liptRotation.SetDataSubmit(OnRotationSubmit);

        m_liptPosX.SetToggleChange(OnUILockXChange);
        m_liptPosY.SetToggleChange(OnUILockYChange);
    }

    public override void OnPageShown()
    {
        base.OnPageShown();

        UIEventBus.AddListener(UIEventType.BGTransformChanged, RefreshAll);
        UIEventBus.AddListener(UIEventType.LockXChanged, OnLockXChange);
        UIEventBus.AddListener(UIEventType.LockYChanged, OnLockYChange);

        MainControl.Instance.editType = EditType.Background;

        RefreshAll();
    }

    public override void OnPageHidden()
    {
        base.OnPageHidden();
        UIEventBus.RemoveListener(UIEventType.BGTransformChanged, RefreshAll);
        UIEventBus.RemoveListener(UIEventType.LockXChanged, OnLockXChange);
        UIEventBus.RemoveListener(UIEventType.LockYChanged, OnLockYChange);
    }

    public void RefreshAll()
    {
        var bgContainer = MainControl.Instance.bgContainer;
        m_liptPosX.SetData(bgContainer.rootPosition.x.ToString());
        m_liptPosY.SetData(bgContainer.rootPosition.y.ToString());
        m_liptScale.SetData(bgContainer.rootScale.ToString());
        m_liptRotation.SetData(bgContainer.rootRotation.ToString());

        m_liptPosX.SetToggleValue(MainControl.Instance.LockX);
        m_liptPosY.SetToggleValue(MainControl.Instance.LockY);
    }

    private void OnLockXChange()
    {
        m_liptPosX.SetToggleValue(MainControl.Instance.LockX);
    }

    private void OnLockYChange()
    {
        m_liptPosY.SetToggleValue(MainControl.Instance.LockY);
    }

    private void OnUILockXChange(Toggle toggle, bool value)
    {
        if (value == MainControl.Instance.LockX)
        {
            return;
        }

        MainControl.Instance.LockX = value;
        UIEventBus.SendEvent(UIEventType.LockXChanged);
    }

    private void OnUILockYChange(Toggle toggle, bool value)
    {
        if (value == MainControl.Instance.LockY)
        {
            return;
        }

        MainControl.Instance.LockY = value;
        UIEventBus.SendEvent(UIEventType.LockYChanged);
    }

    private void OnPosXSubmit(string value)
    {
        if (float.TryParse(value, out float x))
        {
            MainControl.Instance.bgContainer.SetPosition(x, MainControl.Instance.bgContainer.rootPosition.y);
        }
        else
        {
            m_liptPosX.SetData(MainControl.Instance.bgContainer.rootPosition.x.ToString());
        }
    }

    private void OnPosYSubmit(string value)
    {
        if (float.TryParse(value, out float y))
        {
            MainControl.Instance.bgContainer.SetPosition(MainControl.Instance.bgContainer.rootPosition.x, y);
        }
        else
        {
            m_liptPosY.SetData(MainControl.Instance.bgContainer.rootPosition.y.ToString());
        }
    }
    
    private void OnScaleSubmit(string value)
    {
        if (float.TryParse(value, out float scale))
        {
            MainControl.Instance.bgContainer.SetScale(scale);
        }
        else
        {
            m_liptScale.SetData(MainControl.Instance.bgContainer.rootScale.ToString());
        }
    }

    private void OnRotationSubmit(string value)
    {
        if (float.TryParse(value, out float rotation))
        {
            MainControl.Instance.bgContainer.SetRotation(rotation);
        }
        else
        {
            m_liptRotation.SetData(MainControl.Instance.bgContainer.rootRotation.ToString());
        }
    }
}


public class PageBackgroundFunctions : UIPageWidget<PageBackgroundFunctions>
{
    #region auto generated members
    private Button m_btnCopyScene;
    private Button m_btnCopyTransform;
    private Button m_btnCopyAll;
    private Transform m_itemPageFilterSet;
    #endregion

    #region auto generated binders
    protected override void CodeGenBindMembers()
    {
        m_btnCopyScene = transform.Find("CopyCommand/Container/m_btnCopyScene").GetComponent<Button>();
        m_btnCopyTransform = transform.Find("CopyCommand/Container/m_btnCopyTransform").GetComponent<Button>();
        m_btnCopyAll = transform.Find("CopyCommand/Container/m_btnCopyAll").GetComponent<Button>();
        m_itemPageFilterSet = transform.Find("m_itemPageFilterSet").GetComponent<Transform>();

        m_btnCopyScene.onClick.AddListener(OnButtonCopySceneClick);
        m_btnCopyTransform.onClick.AddListener(OnButtonCopyTransformClick);
        m_btnCopyAll.onClick.AddListener(OnButtonCopyAllClick);
    }
    #endregion

    #region auto generated events
    private void OnButtonCopySceneClick()
    {
        MainControl.Instance.CopyBackgroundChange();
    }
    private void OnButtonCopyTransformClick()
    {
        MainControl.Instance.CopyBackgroundTransform();
    }
    private void OnButtonCopyAllClick()
    {
        MainControl.Instance.CopyBackgroundAll();
    }
    #endregion

    private FilterSetData m_filterSetData;
    private PageFilterSet m_pageFilterSet;
    protected override void OnInit()
    {
        base.OnInit();
        m_pageFilterSet = PageFilterSet.CreateWidget(m_itemPageFilterSet.gameObject);
        m_filterSetData = new FilterSetData();
    }

    public override void OnPageShown()
    {
        base.OnPageShown();
        m_pageFilterSet.TryInitAsBG();
    }
}
