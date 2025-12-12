

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

internal class PageNavInstructions : UIPageWidget<PageNavInstructions>
{
    #region auto generated members
    private GameObject m_goLeft;
    private Transform m_tfTrackHeaderRoot;
    private Button m_btnPlay;
    private Button m_btnPlay2;
    private InputField m_iptField;
    #endregion

    #region auto generated binders
    protected override void CodeGenBindMembers()
    {
        m_goLeft = transform.Find("m_goLeft").gameObject;
        m_tfTrackHeaderRoot = transform.Find("m_goLeft/m_tfTrackHeaderRoot").GetComponent<Transform>();
        m_btnPlay = transform.Find("m_goLeft/m_tfTrackHeaderRoot/m_btnPlay").GetComponent<Button>();
        m_btnPlay2 = transform.Find("m_goLeft/m_tfTrackHeaderRoot/m_btnPlay2").GetComponent<Button>();
        m_iptField = transform.Find("m_goLeft/m_tfTrackHeaderRoot/InputField/m_iptField").GetComponent<InputField>();

        m_btnPlay.onClick.AddListener(OnButtonPlayClick);
        m_btnPlay2.onClick.AddListener(OnButtonPlay2Click);
        m_iptField.onValueChanged.AddListener(OnInputFieldFieldChange);
        m_iptField.onEndEdit.AddListener(OnInputFieldFieldEndEdit);
    }
    #endregion


    #region auto generated events
    private void OnButtonPlayClick()
    {
        Parse(L2DWUtils.CopyBoard);
        Play();
    }
    private void OnButtonPlay2Click()
    {
        Parse(m_iptField.text);
        Play();
    }
    private void OnInputFieldFieldChange(string value)
    {
        
    }
    private void OnInputFieldFieldEndEdit(string value)
    {
        
    }

    #endregion

    private List<CommandInfo> commandInfoes = new ();

    private void Parse(string text)
    {
        commandInfoes.Clear();
        var lines = L2DWUtils.GetLines(text);
        foreach (var line in lines)
        {
            var command = Experiment.Parse(line);
            commandInfoes.Add(command);
        }
    }

    protected override void OnPageShown()
    {
        base.OnPageShown();
        MainControl.Instance.UpdateBeat += Update;
    }

    protected override void OnPageHidden()
    {
        MainControl.Instance.UpdateBeat -= Update;
        Stop();
        base.OnPageHidden();
    }

    private float startTime;
    private float nextActionTime;
    private int index;
    private bool playing;
    private void Play()
    {
        startTime = Time.unscaledTime;
        index = 0;
        playing = true;
        nextActionTime = startTime;
        ShutAllUp();
        UpdateBeat();
    }

    private void ShutAllUp()
    {
        foreach (var target in MainControl.Instance.models)
        {
            if (target is ModelAdjuster modelAdjuster)
            {
                modelAdjuster.ShutUp();
            }
        }
    }

    private void Stop()
    {
        playing = false;
        ShutAllUp();
    }

    private void Update()
    {
        if (playing)
        {
            UpdateBeat();
        }
    }

    private void UpdateBeat()
    {
        if (!playing || nextActionTime > Time.unscaledTime)
            return;

        if (index < commandInfoes.Count)
        {
            var info = commandInfoes[index];
            index++;

            switch (info.command.ToLower())
            {
                case "setfigure":
                {
                    var motion = info.GetParameter("motion");
                    var exp = info.GetParameter("expression");

                    void SetFigureContent(ModelAdjusterBase target)
                    {
                        if (target != null)
                        {
                            if (!string.IsNullOrEmpty(motion))
                            {
                                target.PlayMotion(motion);
                            }
                            if (!string.IsNullOrEmpty(exp))
                            {
                                target.PlayExp(exp);
                            }
                        }
                    }
                    if (info.commandParam == "all")
                    {
                        foreach (var target in MainControl.Instance.models)
                        {
                            SetFigureContent(target);
                        }
                    }
                    else
                    {
                        var target = MainControl.Instance.FindTarget(info.commandParam);
                        SetFigureContent(target);
                    }
                    nextActionTime = Time.unscaledTime;
                    Debug.Log($"setfigure: {info.commandParam}, motion: {motion}, expression: {exp}");
                    break;
                }
                case "wait":
                    int.TryParse(info.commandParam, out var waitTime);
                    nextActionTime = Time.unscaledTime + waitTime * 0.001f;
                    Debug.Log($"wait: {waitTime}");
                break;
                case "say":
                {
                    var content = info.commandParam;
                    var targetId = info.GetParameter("target");
                    var target = MainControl.Instance.FindTarget(targetId);
                    var duration = (Experiment.GetSayDuration(content) + 500) * 0.001f;
                    if (info.HasParameter("time") && float.TryParse(info.GetParameter("time"), out var time))
                    {
                        duration = time * 0.001f;
                    }
                    
                    if (target != null && target is ModelAdjuster modelAdjuster)
                    {
                        modelAdjuster.Speak(Time.time + duration);
                        Debug.Log($"say: {content}, target: {targetId}");
                    }
                    else
                    {
                        Debug.Log($"say: {content}, target: {targetId} not found");
                    }

                    if (info.HasParameter("wait"))
                    {
                        nextActionTime = Time.unscaledTime + duration;
                    }

                    break;
                }
                case "shutup":
                {
                    var targetId = info.GetParameter("target");
                    var target = MainControl.Instance.FindTarget(targetId);
                    if (target != null && target is ModelAdjuster modelAdjuster)
                    {
                        modelAdjuster.ShutUp();
                        Debug.Log($"shutup: {targetId}");
                    }
                    else
                    {
                        Debug.Log($"shutup: {targetId} not found");
                    }
                    break;
                }
            }
            UpdateBeat();
        }
        else
        {
            playing = false;
            Debug.Log("播放结束");
        }
    }
}