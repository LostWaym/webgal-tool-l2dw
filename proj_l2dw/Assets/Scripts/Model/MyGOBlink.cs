using System;
using live2d;

public class MyGOBlink
{
    private enum EYE_STATE
    {
        STATE_FIRST,
        STATE_INTERVAL,
        STATE_CLOSING,
        STATE_CLOSED,
        STATE_OPENING
    }

    private long nextBlinkTime;

    private long stateStartTime;

    private EYE_STATE eyeState;

    private bool closeIfZero;

    private string eyeID_L;

    private string eyeID_R;

    private int blinkIntervalMsec;

    private int closingMotionMsec;

    private int closedMotionMsec;

    private int openingMotionMsec;

    public MyGOBlink()
    {
        eyeState = EYE_STATE.STATE_FIRST;
        blinkIntervalMsec = 4000;
        closingMotionMsec = 100;
        closedMotionMsec = 50;
        openingMotionMsec = 150;
        closeIfZero = true;
        eyeID_L = "PARAM_EYE_L_OPEN";
        eyeID_R = "PARAM_EYE_R_OPEN";
    }

    public long calcNextBlink()
    {
        long userTimeMSec = UtSystem.getUserTimeMSec();
        double num = new Random().NextDouble();
        return (long)((double)userTimeMSec + num * (double)(2 * blinkIntervalMsec - 1));
    }

    public void setInterval(int blinkIntervalMsec)
    {
        this.blinkIntervalMsec = blinkIntervalMsec;
    }

    public void setEyeMotion(int closingMotionMsec, int closedMotionMsec, int openingMotionMsec)
    {
        this.closingMotionMsec = closingMotionMsec;
        this.closedMotionMsec = closedMotionMsec;
        this.openingMotionMsec = openingMotionMsec;
    }

    public float l, r;
    public void setParam(ALive2DModel model)
    {
        long userTimeMSec = UtSystem.getUserTimeMSec();
        float num = 0f;
        float num2;
        switch (eyeState)
        {
            case EYE_STATE.STATE_CLOSING:
                num = (float)(userTimeMSec - stateStartTime) / (float)closingMotionMsec;
                if (num >= 1f)
                {
                    num = 1f;
                    eyeState = EYE_STATE.STATE_CLOSED;
                    stateStartTime = userTimeMSec;
                }

                num2 = 1f - num;
                break;
            case EYE_STATE.STATE_CLOSED:
                num = (float)(userTimeMSec - stateStartTime) / (float)closedMotionMsec;
                if (num >= 1f)
                {
                    eyeState = EYE_STATE.STATE_OPENING;
                    stateStartTime = userTimeMSec;
                }

                num2 = 0f;
                break;
            case EYE_STATE.STATE_OPENING:
                num = (float)(userTimeMSec - stateStartTime) / (float)openingMotionMsec;
                if (num >= 1f)
                {
                    num = 1f;
                    eyeState = EYE_STATE.STATE_INTERVAL;
                    nextBlinkTime = calcNextBlink();
                }

                num2 = num;
                break;
            case EYE_STATE.STATE_INTERVAL:
                if (nextBlinkTime < userTimeMSec)
                {
                    eyeState = EYE_STATE.STATE_CLOSING;
                    stateStartTime = userTimeMSec;
                }

                num2 = 1f;
                break;
            default:
                eyeState = EYE_STATE.STATE_INTERVAL;
                nextBlinkTime = calcNextBlink();
                num2 = 1f;
                break;
        }

        if (!closeIfZero)
        {
            num2 = 0f - num2;
        }

        l = model.getParamFloat(eyeID_L);
        r = model.getParamFloat(eyeID_R);

        model.setParamFloat(eyeID_L, l * num2);
        model.setParamFloat(eyeID_R, r * num2);
    }

    public void Revert(ALive2DModel model)
    {
        model.setParamFloat(eyeID_L, l);
        model.setParamFloat(eyeID_R, r);
    }
}