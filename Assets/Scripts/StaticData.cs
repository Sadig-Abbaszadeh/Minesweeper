using UnityEngine;

public static class StaticData
{
    private static Color darkColor = new Color(0.11f, 0.11f, 0.12f, 1);
    private static Color normalColor = new Color(0.5226f, 0.6165f, 0.7641f, 1);
    private static Color disabledColor = new Color(0.76f, 0.76f, 0.76f, 1);

    public static float camSize { get; set; }
    public static int allowableHeight { get; set; }

    public static int minWidthAndHeight
    {
        get
        {
            return 5;
        }
    }

    public static int minBombPercent
    {
        get
        {
            return 10;
        }
    }

    public static int maxBombPercent
    {
        get
        {
            return 70;
        }
    }

    public static int allowableWidth
    {
        get
        {
            return 15;
        }
    }

    public static Color DarkColor
    {
        get
        {
            return darkColor;
        }
    }

    public static Color NormalColor
    {
        get
        {
            return normalColor;
        }
    }

    public static Color DisabledColor
    {
        get
        {
            return disabledColor;
        }
    }
}