using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class VarTracerJsonType
{
    public string logicName;
    public string[] variableName;
    public float[] variableValue;
    public string[] eventName;
    public float[] eventDuration;
    public string[] eventDesc;
    public string runingState;
}

public class VarTracerHandler
{
    public static void ResoloveJsonMsg(string str)
    {
        if(string.IsNullOrEmpty(str))
            return ;
        var resolved = JsonUtility.FromJson<VarTracerJsonType>(str);
        int variableCount = resolved.variableName.Length;
        if (variableCount != resolved.variableValue.Length)
            Debug.LogErrorFormat("Resolove Json Error ,msg = {0}",str);
        int eventCount = resolved.eventName.Length;
        if (eventCount != resolved.eventDuration.Length || eventCount != resolved.eventDesc.Length)
            Debug.LogErrorFormat("Resolove Json Error ,msg = {0}", str);

        bool hasLogicalName = !string.IsNullOrEmpty(resolved.logicName);

        for (int i = 0; i < variableCount; i++ )
        {
            if (hasLogicalName)
                DefineVariable(resolved.variableName[i], resolved.logicName);
            UpdateVariable(resolved.variableName[i], resolved.variableValue[i]);
        }

        for (int i = 0; i < eventCount; i++)
        {
            if (hasLogicalName)
                DefineEvent(resolved.eventName[i], resolved.logicName);
            if (resolved.eventDuration[i] != -1)
                SendEvent(resolved.eventName[i], resolved.eventDuration[i], resolved.eventDesc[i]);
        }
        
        if(!string.IsNullOrEmpty(resolved.runingState))
        {
            if(resolved.runingState.Equals(VarTracerConst.RunningState_Start))
            {
                StartVarTracer();
            }else if (resolved.runingState.Equals(VarTracerConst.RunningState_Pause))
            {
                StopVarTracer();
            }
        }
    }

    public static void DefineVariable(string variableName, string variableBody)
    {
#if UNITY_EDITOR
        foreach (var varBody in VarTracer.Instance.VariableBodys.Values)
        {
            if (varBody.VariableDict.ContainsKey(variableName))
            {
                //Debug.LogFormat("variableName {0} ,Already Exsit!", variableName);
                return;
            }
        }

        if (!VarTracer.Instance.VariableBodys.ContainsKey(variableBody))
        {
            var body = new VarTracerLogicalBody(variableBody);
            body.VariableDict[variableName] = new VarTracerVariable(variableName,variableBody);
            VarTracer.Instance.VariableBodys[variableBody] = body;
        }

        var variableDict = VarTracer.Instance.VariableBodys[variableBody].VariableDict;
        if (!variableDict.ContainsKey(variableName))
        {
            variableDict[variableName] = new VarTracerVariable(variableName,variableBody);
        }
#endif
    }

    public static void UpdateVariable(string variableName, float value)
    {
        if (!VarTracer.isVarTracerStart())
            return ;
#if UNITY_EDITOR
        foreach (var VarBody in VarTracer.Instance.VariableBodys.Values)
        {
            if (VarBody.VariableDict.ContainsKey(variableName))
            {
                var var = VarBody.VariableDict[variableName];
                var.InsertValue(new VarDataInfo(value,VarTracer.Instance.GetCurrentFrameFromTime()));
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
            var body = new VarTracerLogicalBody(variableBody);
            VarTracer.Instance.VariableBodys[variableBody] = body;
        }

        foreach (var varBody in VarTracer.Instance.VariableBodys)
        {
            foreach (var eName in  varBody.Value.EventInfos.Keys)
            {
                if(eventName.Equals(eName))
                {
                    //Debug.LogErrorFormat("Define Event Name Already Exist!");
                    return;
                }
            }
        }
        VarTracer.Instance.VariableBodys[variableBody].RegistEvent(eventName);
#endif
    }

    public static void SendEvent(string eventName, float duration = 0, string desc = "")
    {
        if (!VarTracer.isVarTracerStart())
            return;
        foreach (var varBody in VarTracer.Instance.VariableBodys)
        {
            foreach (var eName in varBody.Value.EventInfos.Keys)
            {
                if (eventName.Equals(eName))
                {
                    List<EventData> listEvent;
                    varBody.Value.EventInfos.TryGetValue(eventName, out listEvent);
                    listEvent.Add(new EventData(VarTracer.Instance.GetCurrentFrameFromTime(), eventName, desc, duration));
                    break;
                }
            }
        }
    }

    public static void StartVarTracer()
    {
        VarTracer.StartVarTracer();
    }

    public static void StopVarTracer()
    {
        VarTracer.StopVarTracer();
    }
}
