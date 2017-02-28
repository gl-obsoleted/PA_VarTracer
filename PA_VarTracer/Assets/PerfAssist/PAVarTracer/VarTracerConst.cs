using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;

public class VarTracerConst
{
    public static readonly float HANDLE_JASON_INTERVAL = 0.5f;

    public static readonly float FPS =100.0f;

    public static readonly int   EventButtonHeight = 20;

    public static readonly int   EventStartHigh = 200;

    public static readonly int EventButtonBaseWidth = 70;

    public static int DEFAULT_SAMPLES = 1048;

    public static string NUM_FORMAT_3 = "###,###,##0.###";
    public static string NUM_FORMAT_2 = "###,##0.##";
    public static string NUM_FORMAT_1 = "###,##0.#";

    public static float INSTANT_EVENT_BTN_DURATION = 0.5f;

    public static readonly int Graph_Grid_Row_Num = 8;

    public static readonly string RemoteIPDefaultText = "127.0.0.1";

    public static readonly string LastConnectedIP = "Mem_LastConnectedIP";

    public static readonly string SPRLIT_TAG = "$";

    public static readonly string[] SPRLIT_TAG_ARRAY = new string[] { VarTracerConst.SPRLIT_TAG };

    public enum ActionType
    {
        None,
        DefineVariable,
        UpdateVariable,
        DefineEvent,
        SendEvent,
        All,
    }

    public enum RunningState
    {
        None,
        RunningState_Start,
        RunningState_Pause,
        RunningState_Close,
    }


    public static GUIStyle NameLabel;
    public static GUIStyle SmallLabel;
    public static GUIStyle HoverText;
    public static GUIStyle FracGS;
    public static GUIStyle EventInstantButtonStyle;
    public static GUIStyle EventDurationButtonStyle;

    public static void InitializeStyles()
    {
        if (NameLabel == null)
        {
            NameLabel = new GUIStyle(EditorStyles.whiteBoldLabel);
            NameLabel.normal.textColor = Color.white;
            SmallLabel = new GUIStyle(EditorStyles.whiteLabel);
            SmallLabel.normal.textColor = Color.white;

            HoverText = new GUIStyle(EditorStyles.whiteLabel);
            HoverText.alignment = TextAnchor.UpperRight;
            HoverText.normal.textColor = Color.white;

            FracGS = new GUIStyle(EditorStyles.whiteLabel);
            FracGS.alignment = TextAnchor.LowerLeft;

            EventInstantButtonStyle = new GUIStyle(EditorStyles.whiteBoldLabel);
            EventInstantButtonStyle.normal.background = Resources.Load("instantButton") as Texture2D;
            EventInstantButtonStyle.normal.textColor = Color.white;
            EventInstantButtonStyle.alignment = TextAnchor.MiddleCenter;

            EventDurationButtonStyle = new GUIStyle(EditorStyles.whiteBoldLabel);
            EventDurationButtonStyle.normal.background = Resources.Load("durationButton") as Texture2D;
            EventDurationButtonStyle.normal.textColor = Color.white;
            EventDurationButtonStyle.alignment = TextAnchor.MiddleCenter;
        }
    }


}


