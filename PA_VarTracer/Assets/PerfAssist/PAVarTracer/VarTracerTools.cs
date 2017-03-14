using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;
using UnityEngine;

//Demo
//VarTracerTools.DefineEvent("ATTACK", "Camera");
//VarTracerTools.UpdateVariable("CameraV_X", Camera.main.velocity.x);
//VarTracerTools.SendEvent("JUMP", 1.0f, "PLAYER JUMP");
//VarTracerTools.SendEvent("ATTACK");
//VarTracerTools.DefineEvent("ATTACK", "Camera");

//{
//    VariableParm vp ;
//    vp.VariableName = "ValueName";
//    vp.VariableValue = 1.0f;

//    EventParm ep;
//    ep.EventName = "EventName";
//    ep.EventDuration = 1.5f;
//    ep.EventDesc = "EventDesc";

//    VarTracerTools.SendGroup(
//        new Group("Test", new VariableParm[] { vp }, new EventParm[] { ep })
//    );
//}

public class VarTracerTools
{

    public static void UpdateVariable(string variableName, float value)
    {
        UpdateVariable(VarTracerConst.DEFAULT_GROUP_NAME,variableName,value);
    }

    public static void UpdateVariable(string groupName,string variableName, float value)
    {
        VarTracerSender.Instance.CmdCacher.SendVariable(groupName,variableName,value);
    }

    public static void SendEvent(string eventName, float duration = 0)
    {
        SendEvent(VarTracerConst.DEFAULT_GROUP_NAME, eventName,duration);
    }

    public static void SendEvent(string groupName, string eventName, float duration = 0)
    {
        VarTracerSender.Instance.CmdCacher.SendEvent(groupName, eventName, duration);
    }

    //public static void SendGroup(VarGroup vjp)
    //{
    //    if (vjp == null)
    //        return;

    //    NamePackage vtjt = new NamePackage();
    //    vtjt.logicName = vjp.Name;
    //    vtjt.runingState = vjp.RuningState;

    //    if(vjp.VarItems !=null)
    //    {
    //        int count = vjp.VarItems.Length;
    //        vtjt.variableName  = new string [count];
    //        vtjt.variableValue = new float[count];
    //        for (int i = 0; i < count; i++)
    //        {
    //            vtjt.variableName[i] = vjp.VarItems[i].VariableName;
    //            vtjt.variableValue[i] = vjp.VarItems[i].VariableValue;
    //        }
    //    }

    //    if(vjp.EventItems !=null)
    //    {
    //        int count = vjp.EventItems.Length;
    //        vtjt.eventName = new string[count];
    //        vtjt.eventDuration = new float[count];
    //        for (int i = 0; i < count; i++)
    //        {
    //            vtjt.eventName[i] = vjp.EventItems[i].EventName;
    //            vtjt.eventDuration[i] = vjp.EventItems[i].EventDuration;
    //        }
    //    }
    //    VarTracerSender.Instance.SendJsonMsg(vtjt);
    //}
}
