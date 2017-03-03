using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;
using UnityEngine;

public class VarTracerTools: MonoBehaviour
{
    public static void SendJsonMsg(VarTracerJsonType vtjt)
    {
        if (vtjt == null)
            return;
        VarTracerSender.Instance.SendJsonMsg(vtjt);
    }

    public static void DefineVariable(string variableName, string LogicalName)
    {
        VarTracerJsonType vtjt = new VarTracerJsonType();
        vtjt.logicName = LogicalName;
        vtjt.variableName = new string[] { variableName };
        vtjt.variableValue = new float[] { 0 };
        VarTracerSender.Instance.SendJsonMsg(vtjt);
    }

    public static void UpdateVariable(string variableName, float value)
    {
        VarTracerJsonType vtjt = new VarTracerJsonType();
        vtjt.variableName = new string[] { variableName };
        vtjt.variableValue = new float[] { value };
        VarTracerSender.Instance.SendJsonMsg(vtjt);
    }

    public static void DefineEvent(string eventName, string variableBody)
    {
        VarTracerJsonType vtjt = new VarTracerJsonType();
        vtjt.logicName = variableBody;
        vtjt.eventName = new string[] { eventName };
        vtjt.eventDuration = new float[] { -1 };
        vtjt.eventDesc = new string[] {""};
        VarTracerSender.Instance.SendJsonMsg(vtjt);
    }

    public static void SendEvent(string eventName, float duration = 0, string desc = "")
    {
        VarTracerJsonType vtjt = new VarTracerJsonType();
        vtjt.eventName = new string[] { eventName };
        vtjt.eventDuration = new float[] { duration };
        vtjt.eventDesc = new string[] { desc };
        VarTracerSender.Instance.SendJsonMsg(vtjt);
    }

    //public void StartVarTracer()
    //{
    //    VarTracerJsonType vtjt = new VarTracerJsonType();
    //    vtjt.runingState = (int)VarTracerConst.RunningState.RunningState_Start;
    //    SendJsonMsg(vtjt);
    //}

    //public void StopVarTracer()
    //{
    //    VarTracerJsonType vtjt = new VarTracerJsonType();
    //    vtjt.runingState = (int)VarTracerConst.RunningState.RunningState_Pause;
    //    sendMsgList.Add(vtjt);
    //}
}
