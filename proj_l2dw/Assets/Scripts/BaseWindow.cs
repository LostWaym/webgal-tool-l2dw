

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
}

