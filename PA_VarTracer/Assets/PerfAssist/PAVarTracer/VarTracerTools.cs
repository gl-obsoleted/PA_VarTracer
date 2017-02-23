using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using UnityEngine;

public class VarTracerTools
{
    public static VarTracerTools Instance;

    public VarTracerTools()
    {
        if (UsNet.Instance != null && UsNet.Instance.CmdExecutor != null)
        {
        }
        else
            UnityEngine.Debug.LogError("UsNet not available");
    }

    public static void SendJsonMsg(string json)
    {
        UsCmd pkt = new UsCmd();
        pkt.WriteNetCmd(eNetCmd.SV_VarTracerJsonParameter);
        pkt.WriteString(json);
        UsNet.Instance.SendCommand(pkt);
    }

    public static void DefineVariable(string variableName, string LogicalName)
    {
        VarTracerJsonType vtjt = new VarTracerJsonType();
        vtjt.logicName = LogicalName;
        vtjt.variableName = new string[] { variableName };
        vtjt.variableValue = new float[] { 0 };
        string json = JsonUtility.ToJson(vtjt);
        SendJsonMsg(json);
    }
    public static void UpdateVariable(string variableName, float value)
    {
        VarTracerJsonType vtjt = new VarTracerJsonType();
        vtjt.variableName = new string[] { variableName };
        vtjt.variableValue = new float[] { value };
        string json = JsonUtility.ToJson(vtjt);
        SendJsonMsg(json);
    }
    public static void DefineEvent(string eventName, string variableBody)
    {
        VarTracerJsonType vtjt = new VarTracerJsonType();
        vtjt.eventName = new string[] { eventName };
        vtjt.eventDuration = new float[] { -1 };
        vtjt.eventDesc = new string[] { "" };
        string json = JsonUtility.ToJson(vtjt);
        SendJsonMsg(json);
    }
    public static void SendEvent(string eventName, float duration = 0, string desc = "")
    {
        VarTracerJsonType vtjt = new VarTracerJsonType();
        vtjt.eventName = new string[] { eventName };
        vtjt.eventDuration = new float[] { duration };
        vtjt.eventDesc = new string[] { desc };
        string json = JsonUtility.ToJson(vtjt);
        SendJsonMsg(json);
    }

    public static void StartVarTracer()
    {
        VarTracerJsonType vtjt = new VarTracerJsonType();
        vtjt.runingState = VarTracerConst.RunningState_Start;
        string json = JsonUtility.ToJson(vtjt);
        SendJsonMsg(json);
    }

    public static void StopVarTracer()
    {
        VarTracerJsonType vtjt = new VarTracerJsonType();
        vtjt.runingState = VarTracerConst.RunningState_Pause;
        string json = JsonUtility.ToJson(vtjt);
        SendJsonMsg(json);
    }
}
