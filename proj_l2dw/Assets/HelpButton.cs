using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HelpButton : MonoBehaviour
{
    [Multiline(8)]
    public string content;

    void Awake()
    {
        GetComponent<Button>().onClick.AddListener(()=>
        {
            MessageTipWindow.Instance.Show("帮助", content);
        });
    }
}
