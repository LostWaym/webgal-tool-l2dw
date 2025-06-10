using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class AnimInstructionUI : BaseWindow<AnimInstructionUI>
{
    #region auto generated members
    private Button m_btnCancel;
    private InputField m_iptFormatText;
    private Button m_btnCall;
    private Button m_btnLearn;
    #endregion

    #region auto generated binders
    protected override void CodeGenBindMembers()
    {
        m_btnCancel = transform.Find("bg/GameObject/m_btnCancel").GetComponent<Button>();
        m_iptFormatText = transform.Find("bg/GameObject/立绘指令模板/m_iptFormatText").GetComponent<InputField>();
        m_btnCall = transform.Find("bg/GameObject/m_btnCall").GetComponent<Button>();
        m_btnLearn = transform.Find("bg/GameObject/m_btnLearn").GetComponent<Button>();

        m_btnCancel.onClick.AddListener(OnButtonCancelClick);
        m_iptFormatText.onValueChanged.AddListener(OnInputFieldFormatTextChange);
        m_iptFormatText.onEndEdit.AddListener(OnInputFieldFormatTextEndEdit);
        m_btnCall.onClick.AddListener(OnButtonCallClick);
        m_btnLearn.onClick.AddListener(OnButtonLearnClick);
    }
    #endregion


    #region auto generated events
    private void OnButtonCancelClick()
    {
        Close();
    }
    private void OnInputFieldFormatTextChange(string value)
    {
        Debug.Log("OnInputFieldFormatTextChange");
    }
    private void OnInputFieldFormatTextEndEdit(string value)
    {
        Debug.Log("OnInputFieldFormatTextEndEdit");
    }
    private void OnButtonCallClick()
    {
        Cast();
        m_pageNavMotion.RefreshAll();
        Close();
    }

    private void OnButtonLearnClick()
    {
        Application.OpenURL("https://docs.qq.com/sheet/DWFBuZEVCSEhtU0NB?tab=15es20");
    }
    #endregion

    private PageNavMotion m_pageNavMotion;
    
    public void SetPageNavMotion(PageNavMotion pageNavMotion)
    {
        Show();
        m_pageNavMotion = pageNavMotion;
    }

    public void Cast()
    {
        var lines = m_iptFormatText.text.Replace("\r", "").Split('\n');
        foreach (var line in lines)
        {
            CastSingleLine(line);
        }
    }

    private const string INST_SET_KEY_FRAMES = "SetKeyFrames";
    public void CastSingleLine(string line)
    {
        var parts = line.Split(' ');
        var paramName = parts[0];
        var param = parts.Skip(1).ToArray();
        switch (paramName)
        {
            case INST_SET_KEY_FRAMES:
                SetKeyFrames(param);
                break;
        }
    }

    private void SetKeyFrames(string[] param)
    {
        string paramName = param[0];
        int startFrameIndex = int.Parse(param[1]) - 1;
        float[] values = param[2].Split(',').Select(p => float.Parse(p)).ToArray();

        var track = m_pageNavMotion.MotionData.TryGetTrack(paramName, false);
        if (track == null)
            return;
        
        for (int i = 0; i < values.Length; i++)
        {
            track.keyFrames[startFrameIndex + i] = values[i];
        }

        if(!track.HasKeyFrame(0))
        {
            var target = MainControl.Instance.curTarget;
            var defValue = target.GetEmotionEditorList().paramDefDict.TryGetValue(paramName, out var paramDef) ? paramDef : values[0];
            track.keyFrames[0] = defValue;
        }

        m_pageNavMotion.MotionData.BakeFrames(paramName);
    }
}
