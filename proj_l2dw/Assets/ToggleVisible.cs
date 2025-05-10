using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleVisible : MonoBehaviour
{
    public GameObject[] visibleObjects;
    private Toggle toggle;

    void Awake()
    {
        toggle = GetComponent<Toggle>();
        toggle.onValueChanged.AddListener(OnValueChanged);
    }

    private void OnValueChanged(bool arg0)
    {
        foreach (var obj in visibleObjects)
        {
            obj.SetActive(arg0);
        }
    }

    void Start()
    {
        OnValueChanged(toggle.isOn);
    }
}
