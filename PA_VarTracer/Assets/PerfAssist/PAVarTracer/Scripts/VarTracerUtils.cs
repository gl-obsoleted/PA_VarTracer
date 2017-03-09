using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public class VarTracerUtils
{
    private static long stopTimeStamp;
    public static long StopTimeStamp
    {
        get { return VarTracerUtils.stopTimeStamp; }
        set { VarTracerUtils.stopTimeStamp = value; }
    }

    public static long GetTimeStamp()
    {
        TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
        return Convert.ToInt64(ts.TotalMilliseconds);
    }

    public static Color GetColorByIndex(int index)
    {
        switch (index)
        {
            case 0:
                return new Color(0 / 255, 255 / 255, 255 / 255);
            case 1:
                return new Color(227.0f / 255.0f, 207.0f / 255.0f, 87.0f / 255.0f);
            case 2:
                return new Color(0 / 255.0f, 255 / 255.0f, 0 / 255.0f);
            case 3:
                return new Color(209 / 255.0f, 238 / 255.0f, 238 / 255.0f);
            case 4:
                return new Color(227 / 255.0f, 240 / 255.0f, 202 / 255.0f);
            case 5:
                return new Color(255 / 255.0f, 127 / 255.0f, 80 / 255.0f);
            case 6:
                return new Color(176 / 255.0f, 226 / 255.0f, 255 / 255.0f);
            case 7:
                return new Color(255 / 255.0f, 192 / 255.0f, 203 / 255.0f);
            case 8:
                return new Color(244 / 255.0f, 164 / 255.0f, 96 / 255.0f);
            case 9:
                return new Color(218 / 255.0f, 112 / 255.0f, 214 / 255.0f);
            case 10:
                return new Color(210 / 255.0f, 105 / 255.0f, 30 / 255.0f);
            case 11:
                return new Color(252 / 255.0f, 230 / 255.0f, 201 / 255.0f);
            case 12:
                return new Color(192 / 255.0f, 192 / 255.0f, 192 / 255.0f);
            case 13:
                return new Color(255 / 255.0f, 215 / 255.0f, 0);
            case 14:
                return new Color(255 / 255.0f, 97 / 255.0f, 0);
            case 15:
                return new Color(50 / 255.0f, 205 / 255.0f, 50 / 255.0f);
            case 16:
                return new Color(160 / 255.0f, 102 / 255.0f, 211 / 255.0f);
            case 17:
                return new Color(240 / 255.0f, 230 / 255.0f, 140 / 255.0f);
            case 18:
                return new Color(255 / 255.0f, 0, 255 / 255.0f);
            case 19:
                return new Color(0, 199 / 255.0f, 140 / 255.0f);
            case 20:
                return new Color(51 / 255.0f, 161 / 255.0f, 201 / 255.0f);
            default:
                return new Color(255 / 255.0f, 52 / 255.0f, 179 / 255.0f);
        }
    }
}
