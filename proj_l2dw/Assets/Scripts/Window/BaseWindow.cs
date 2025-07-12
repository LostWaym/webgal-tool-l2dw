

using UnityEngine;

public class BaseWindow : MonoBehaviour
{

    protected virtual void Awake()
    {
        CodeGenBindMembers();
        OnInit();
    }

    protected virtual void CodeGenBindMembers()
    {
    }

    protected virtual void OnInit()
    {

    }

    public virtual void Show()
    {
        if (gameObject.activeSelf)
            return;
        gameObject.SetActive(true);
        OnShow();
    }

    public virtual void Close()
    {
        if (!gameObject.activeSelf)
            return;
        
        gameObject.SetActive(false);
        OnClose();
    }

    protected virtual void OnShow()
    {

    }

    protected virtual void OnClose()
    {

    }
}

