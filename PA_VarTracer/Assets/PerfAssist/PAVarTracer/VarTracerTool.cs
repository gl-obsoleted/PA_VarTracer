using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class VarTracerTool
{
    public static void DefineVariable(string variableName, string variableBody, Color color)
    {
#if UNITY_EDITOR
        foreach (var varBody in VarTracer.Instance.VariableBodys.Values)
        {
            if (varBody.VariableDict.ContainsKey(variableName))
            {
                Debug.LogFormat("variableName {0} ,Already Exsit!", variableName);
                return;
            }
        }

        if (!VarTracer.Instance.VariableBodys.ContainsKey(variableBody))
        {
            var body = new GraphItVariableBody(variableBody);
            body.VariableDict[variableName] = new GraphItVariable(variableName,variableBody);
            VarTracer.Instance.VariableBodys[variableBody] = body;
        }

        var variableDict = VarTracer.Instance.VariableBodys[variableBody].VariableDict;
        if (!variableDict.ContainsKey(variableName))
        {
            variableDict[variableName] = new GraphItVariable(variableName,variableBody);
        }
        variableDict[variableName].Color = color;
#endif
    }

    public static void UpdateVariable(string variableName, float value)
    {
#if UNITY_EDITOR
        foreach (var VarBody in VarTracer.Instance.VariableBodys.Values)
        {
            if (VarBody.VariableDict.ContainsKey(variableName))
            {
                var var = VarBody.VariableDict[variableName];
                var.InsertValue(value);
            }
        }
#endif
    }

    public static void DefineEvent(string eventName, string variableBody,Color color)
    {
#if UNITY_EDITOR
        if (string.IsNullOrEmpty(eventName))
            return;

        if (!VarTracer.Instance.VariableBodys.ContainsKey(variableBody))
        {
            var body = new GraphItVariableBody(variableBody);
            VarTracer.Instance.VariableBodys[variableBody] = body;
        }

        foreach (var varBody in VarTracer.Instance.VariableBodys)
        {
            foreach (var eName in  varBody.Value.EventInfos.Keys)
            {
                if(eventName.Equals(eName))
                {
                    Debug.LogErrorFormat("Define Event Name Already Exist!");
                    return;
                }
            }

        }
        VarTracer.Instance.VariableBodys[variableBody].RegistEvent(eventName,color);
#endif
    }

    public static void SendEvent(string eventName)
    {
        foreach (var varBody in VarTracer.Instance.VariableBodys)
        {
            foreach (var eName in varBody.Value.EventInfos.Keys)
            {
                if (eventName.Equals(eName))
                {
                    List<EventData> listEvent;
                    varBody.Value.EventInfos.TryGetValue(eventName, out listEvent);
                    listEvent.Add(new EventData(VarTracer.m_wholeFrameIndex, eventName));
                    break;
                }
            }
        }
    }
}
