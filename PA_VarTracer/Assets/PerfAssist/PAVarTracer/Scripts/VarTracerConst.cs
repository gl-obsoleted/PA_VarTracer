using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class VarTracerConst
{
    public static readonly float HANDLE_JASON_INTERVAL = 0.001f;
    public static readonly float SEND_MSG_INTERVAL = 0.001f;

    public static readonly float FPS =100.0f;

    public static readonly int   EventButtonHeight = 20;

    public static readonly int   EventStartHigh = 180;

    public static readonly int EventButtonBaseWidth = 70;

    public static readonly int EventButtonFixGap = 30;

    public static int DEFAULT_SAMPLES = 1048;

    public static string NUM_FORMAT_3 = "###,###,##0.###";
    public static string NUM_FORMAT_2 = "###,##0.##";
    public static string NUM_FORMAT_1 = "###,##0.#";

    public static float INSTANT_EVENT_BTN_DURATION = 0.1f;

    public static readonly int Graph_Grid_Row_Num = 8;

    public static readonly string RemoteIPDefaultText = "127.0.0.1";

    public static readonly string LastConnectedIP = "Mem_LastConnectedIP";

    public static readonly string SPRLIT_TAG = "$";

    public static readonly string[] SPRLIT_TAG_ARRAY = new string[] { VarTracerConst.SPRLIT_TAG };


    //varTracerGraphItWindow
    public static float NavigationAreaStartX = 0;
    public static float AttributeAreaStartX = 1220;
    public static float TickMarkAreaStartX = 1180;

    public static float NavigationAreaRWidth = 250;
    //varTracerGraphItWindow


    public static readonly int  DefaultChannelHieght= 200;

    public enum RunningState
    {
        None,
        RunningState_Start,
        RunningState_Pause,
        RunningState_Close,
    }
}


