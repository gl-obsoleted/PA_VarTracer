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

    public static void DefineEvent(string eventName, string variableBody)
    {
#if UNITY_EDITOR
        if (string.IsNullOrEmpty(eventName))
            return;
        if (!VarTracer.Instance.VariableBodys.ContainsKey(variableBody))
        {
            var body = new GraphItVariableBody(variableBody);
            VarTracer.Instance.VariableBodys[variableBody] = body;
        }

        VarTracer.Instance.VariableBodys[variableBody].registEvent(eventName);

        if (!VarTracer.Instance.MEventDataDict.ContainsKey(eventName))
            VarTracer.Instance.MEventDataDict.Add(eventName, new List<EventData>());
#endif
    }

    public static void SendEvent(string eventName)
    {
        if (VarTracer.Instance.MEventDataDict.ContainsKey(eventName))
        {
            List<EventData> listEvent;
            VarTracer.Instance.MEventDataDict.TryGetValue(eventName, out listEvent);
            listEvent.Add(new EventData(VarTracer.Instance.m_frameIndex, eventName));
        }

        List<string> needChannel = new List<string>();
#if UNITY_EDITOR
        if (string.IsNullOrEmpty(eventName))
            return;
        foreach (var variableBodys in VarTracer.Instance.VariableBodys.Values)
        {
            if (variableBodys.RegistEventList.ContainsKey(eventName))
            {
                foreach (var var in variableBodys.VariableDict.Values)
                {
                    foreach (var channelName in var.ChannelDict.Keys)
                    {
                        if (!needChannel.Contains(channelName))
                            needChannel.Add(channelName);
                    }
                }
            }
        }

        foreach (string channelName in needChannel)
        {
            VarTracer.SetGraphEvent(channelName, eventName);
        }
#endif
    }
}
