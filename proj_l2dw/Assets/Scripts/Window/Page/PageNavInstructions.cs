

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

internal class PageNavInstructions : UIPageWidget<PageNavInstructions>
{
    #region auto generated members
    private GameObject m_goLeft;
    private Transform m_tfTrackHeaderRoot;
    private Button m_btnPlay;
    #endregion

    #region auto generated binders
    protected override void CodeGenBindMembers()
    {
        m_goLeft = transform.Find("m_goLeft").gameObject;
        m_tfTrackHeaderRoot = transform.Find("m_goLeft/m_tfTrackHeaderRoot").GetComponent<Transform>();
        m_btnPlay = transform.Find("m_goLeft/m_tfTrackHeaderRoot/m_btnPlay").GetComponent<Button>();

        m_btnPlay.onClick.AddListener(OnButtonPlayClick);
    }
    #endregion

    #region auto generated events
    private void OnButtonPlayClick()
    {
        Parse(L2DWUtils.CopyBoard);
        Play();
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
        UpdateBeat();
    }

    private void Stop()
    {
        playing = false;
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
                    var target = MainControl.Instance.FindTarget(info.commandParam);
                    var motion = info.GetParameter("motion");
                    var exp = info.GetParameter("expression");
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
                    if (target != null)
                    {
                        // target.Say(content);
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