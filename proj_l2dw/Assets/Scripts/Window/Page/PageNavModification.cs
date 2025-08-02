using UnityEngine;
using UnityEngine.UI;

public class PageNavModification : UIPageWidget<PageNavModification>
{
    #region auto generated members
    private Transform m_tfCharaRoot;
    private RawImage m_rawChara;
    private TouchArea m_touchChara;
    private Button m_btnResetCharaArea;
    private Button m_btnMotion;
    private Text m_lblMotion;
    private RectTransform m_rectSelectMotionArea;
    private Button m_btnSave;
    private GameObject m_goLeft;
    private Transform m_tfTrackHeaderRoot;
    private Toggle m_toggleParts;
    private Toggle m_toggleInitParams;
    private Toggle m_toggleMotion;
    private Toggle m_toggleExpression;
    private GameObject m_goRight;
    private Transform m_itemPageModification_Parts;
    private Transform m_itemPageModification_InitParams;
    private Transform m_itemPageModification_Motion;
    private Transform m_itemPageModification_Expression;
    #endregion

    #region auto generated binders
    protected override void CodeGenBindMembers()
    {
        m_tfCharaRoot = transform.Find("CharaArea/m_tfCharaRoot").GetComponent<Transform>();
        m_rawChara = transform.Find("CharaArea/m_tfCharaRoot/m_rawChara").GetComponent<RawImage>();
        m_touchChara = transform.Find("CharaArea/m_touchChara").GetComponent<TouchArea>();
        m_btnResetCharaArea = transform.Find("CharaArea/m_btnResetCharaArea").GetComponent<Button>();
        m_btnMotion = transform.Find("CharaArea/ToolBar/m_btnMotion").GetComponent<Button>();
        m_lblMotion = transform.Find("CharaArea/ToolBar/m_btnMotion/m_lblMotion").GetComponent<Text>();
        m_rectSelectMotionArea = transform.Find("CharaArea/ToolBar/m_btnMotion/m_rectSelectMotionArea").GetComponent<RectTransform>();
        m_btnSave = transform.Find("CharaArea/ToolBar/m_btnSave").GetComponent<Button>();
        m_goLeft = transform.Find("m_goLeft").gameObject;
        m_tfTrackHeaderRoot = transform.Find("m_goLeft/m_tfTrackHeaderRoot").GetComponent<Transform>();
        m_toggleParts = transform.Find("m_goLeft/m_tfTrackHeaderRoot/m_toggleParts").GetComponent<Toggle>();
        m_toggleInitParams = transform.Find("m_goLeft/m_tfTrackHeaderRoot/m_toggleInitParams").GetComponent<Toggle>();
        m_toggleMotion = transform.Find("m_goLeft/m_tfTrackHeaderRoot/m_toggleMotion").GetComponent<Toggle>();
        m_toggleExpression = transform.Find("m_goLeft/m_tfTrackHeaderRoot/m_toggleExpression").GetComponent<Toggle>();
        m_goRight = transform.Find("m_goRight").gameObject;
        m_itemPageModification_Parts = transform.Find("m_goRight/m_itemPageModification_Parts").GetComponent<Transform>();
        m_itemPageModification_InitParams = transform.Find("m_goRight/m_itemPageModification_InitParams").GetComponent<Transform>();
        m_itemPageModification_Motion = transform.Find("m_goRight/m_itemPageModification_Motion").GetComponent<Transform>();
        m_itemPageModification_Expression = transform.Find("m_goRight/m_itemPageModification_Expression").GetComponent<Transform>();

        m_btnResetCharaArea.onClick.AddListener(OnButtonResetCharaAreaClick);
        m_btnMotion.onClick.AddListener(OnButtonMotionClick);
        m_btnSave.onClick.AddListener(OnButtonSaveClick);
        m_toggleParts.onValueChanged.AddListener(OnTogglePartsChange);
        m_toggleInitParams.onValueChanged.AddListener(OnToggleInitParamsChange);
        m_toggleMotion.onValueChanged.AddListener(OnToggleMotionChange);
        m_toggleExpression.onValueChanged.AddListener(OnToggleExpressionChange);
    }
    #endregion

    #region auto generated events
    private void OnButtonResetCharaAreaClick()
    {
    }
    private void OnButtonMotionClick()
    {
    }
    private void OnButtonSaveClick()
    {
    }
    private void OnTogglePartsChange(bool value)
    {
    }
    private void OnToggleInitParamsChange(bool value)
    {
    }
    private void OnToggleMotionChange(bool value)
    {
    }
    private void OnToggleExpressionChange(bool value)
    {
    }
    #endregion

    private PageNavModificationParts m_pageNavModificationParts;
    private PageNavModificationInitParams m_pageNavModificationInitParams;
    private PageNavModificationMotion m_pageNavModificationMotion;
    private PageNavModificationExpression m_pageNavModificationExpression;

    protected override void OnInit()
    {
        base.OnInit();
        m_pageNavModificationParts = PageNavModificationParts.CreateWidget(m_itemPageModification_Parts.gameObject);
        m_pageNavModificationInitParams = PageNavModificationInitParams.CreateWidget(m_itemPageModification_InitParams.gameObject);
        m_pageNavModificationMotion = PageNavModificationMotion.CreateWidget(m_itemPageModification_Motion.gameObject);
        m_pageNavModificationExpression = PageNavModificationExpression.CreateWidget(m_itemPageModification_Expression.gameObject);

        m_pageNavModificationParts.BindToToggle(m_toggleParts);
        m_pageNavModificationInitParams.BindToToggle(m_toggleInitParams);
        m_pageNavModificationMotion.BindToToggle(m_toggleMotion);
        m_pageNavModificationExpression.BindToToggle(m_toggleExpression);
    }
}

public class PageNavModificationParts : UIPageWidget<PageNavModificationParts>
{

}

public class PageNavModificationInitParams : UIPageWidget<PageNavModificationInitParams>
{

}

public class PageNavModificationMotion : UIPageWidget<PageNavModificationMotion>
{

}

public class PageNavModificationExpression : UIPageWidget<PageNavModificationExpression>
{

}
