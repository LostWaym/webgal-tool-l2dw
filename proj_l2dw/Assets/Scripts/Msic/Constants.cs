

public static class Constants
{
    public static int WebGalWidth
    {
        get
        {
            if(Global.IsSetResolution)
            {
                return Global.NewResolutionWidth;
            }
            return defaultWidth;
        }
    }
    public static int WebGalHeight
    {
        get
        {
            if(Global.IsSetResolution)
            {
                return Global.NewResolutionHeight;
            }
            return defaultHeight;
        }
    }

    public const int defaultWidth = 2560;
    public const int defaultHeight = 1440;
}

