

using live2d;
using UnityEngine;

public class ExpQueueManager
{
    private MygoExp prevExp;
    public MygoExp curExp;
    public MygoExp nextExp;

    private MygoExp RevertTarget => prevExp ?? curExp;

    public void updateParam(Live2DModelUnity model)
    {
        if (nextExp != null)
        {
            if (nextExp == curExp)
            {
                nextExp = null;
            }
            else
            {
                prevExp = curExp;
                curExp = nextExp;
                nextExp = null;
                curExp.Reset();
            }
        }

        prevExp?.Update(Time.deltaTime, true);
        curExp?.Update(Time.deltaTime);
        prevExp?.Apply(model);
        curExp?.Apply(model);
    }

    public void RevertExp(Live2DModelUnity model)
    {
        curExp?.Revert(model);
        prevExp?.Revert(model);
    }

    public void startExp(MygoExp exp, Live2DModelUnity model)
    {
        nextExp = exp;
    }
}