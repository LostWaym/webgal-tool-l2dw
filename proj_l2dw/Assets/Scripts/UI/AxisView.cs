using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AxisView : MonoBehaviour
{
    public GameObject activeX, activeY;
    public GameObject inactiveX, inactiveY;
    
    public void SetXAxis(bool deactive)
    {
        activeX.SetActive(!deactive);
        inactiveX.SetActive(deactive);
    }
    
    public void SetYAxis(bool deactive)
    {
        activeY.SetActive(!deactive);
        inactiveY.SetActive(deactive);
    }
}
