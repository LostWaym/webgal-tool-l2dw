
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class MonoKeyUIStyle : MonoBehaviour
{
    public KeyUIStyle style;
}

[System.Serializable]
public class KeyUIStyle
{
    public KeyUIStyleState[] states;

    public KeyUIStyleState GetState(string name)
    {
        return states.FirstOrDefault(s => s.name == name);
    }

    public void ApplyObject(string name)
    {
        var state = GetState(name);
        if (state != null)
        {
            state.ApplyObject();
        }
    }
}

[System.Serializable]
public class KeyUIStyleState
{
    public string name;
    public GameObject[] activeObjects;
    public GameObject[] inactiveObjects;

    public void ApplyObject()
    {
        foreach (var obj in activeObjects)
        {
            obj.SetActive(true);
        }
        foreach (var obj in inactiveObjects)
        {
            obj.SetActive(false);
        }
    }
}
