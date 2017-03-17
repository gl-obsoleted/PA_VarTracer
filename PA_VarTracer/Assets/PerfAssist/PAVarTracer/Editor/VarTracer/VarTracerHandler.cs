using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
namespace VariableTracer
{
    public class VarTracerHandler
    {
        public static void ResoloveJsonMsg(VarTracerJsonType resolved)
        {
            //int variableCount = resolved.variableName.Length;
            //if (variableCount != resolved.variableValue.Length)
            //    Debug.LogErrorFormat("Parameter Resolove Json Error ,variableCount = {0}", variableCount);
            //int eventCount = resolved.eventName.Length;
            //if (eventCount != resolved.eventDuration.Length)
            //    Debug.LogErrorFormat("Parameter Resolove Json Error ,eventCount = {0}", eventCount);

            //long timeStamp = resolved.timeStamp;
            //if (VarTracerNet.Instance.StartTimeStamp == 0)
            //{
            //    VarTracerNet.Instance.StartTimeStamp = VarTracerUtils.GetTimeStamp();
            //    VarTracerNet.Instance.NetDeltaTime = VarTracerNet.Instance.StartTimeStamp - timeStamp;
            //}
            //timeStamp += VarTracerNet.Instance.NetDeltaTime;

            //bool hasLogicalName = !string.IsNullOrEmpty(resolved.logicName);

            //for (int i = 0; i < variableCount; i++)
            //{
            //    if (hasLogicalName)
            //        DefineVariable(resolved.variableName[i], resolved.logicName);
            //    UpdateVariable(timeStamp, resolved.variableName[i], resolved.variableValue[i]);
            //}

            //for (int i = 0; i < eventCount; i++)
            //{
            //    if (hasLogicalName)
            //        DefineEvent(resolved.eventName[i], resolved.logicName);
            //    if (resolved.eventDuration[i] != -1)
            //        SendEvent(timeStamp, resolved.eventName[i], resolved.eventDuration[i]);
            //}

            //if (resolved.runingState == (int)VarTracerConst.RunningState.RunningState_Start)
            //{
            //    StartVarTracer();
            //}
            //else if (resolved.runingState == (int)VarTracerConst.RunningState.RunningState_Pause)
            //{
            //    StopVarTracer();
            //}
        }

        public static void DefineVariable(string variableName, string groupName)
        {
#if UNITY_EDITOR
            foreach (var varBody in VarTracer.Instance.groups.Values)
            {
                if (varBody.VariableDict.ContainsKey(variableName))
                {
                    //Debug.LogFormat("variableName {0} ,Already Exsit!", variableName);
                    return;
                }
            }

            if (!VarTracer.Instance.groups.ContainsKey(groupName))
            {
                var body = new VarTracerGroup(groupName);
                body.VariableDict[variableName] = new VarTracerVariable(variableName, groupName);
                VarTracer.Instance.groups[groupName] = body;
            }

            var variableDict = VarTracer.Instance.groups[groupName].VariableDict;
            if (!variableDict.ContainsKey(variableName))
            {
                variableDict[variableName] = new VarTracerVariable(variableName, groupName);
            }
#endif
        }

        public static void UpdateVariable(string groupName ,string variableName, long timeStamp, float value)
        {
            if (!GraphItWindow.isVarTracerStart())
                return;
            if (!VarTracer.Instance.groups.ContainsKey(groupName))
            {
                var body = new VarTracerGroup(groupName);
                body.VariableDict[variableName] = new VarTracerVariable(variableName, groupName);
                VarTracer.Instance.groups[groupName] = body;
            }

            var variableDict = VarTracer.Instance.groups[groupName].VariableDict;
            if (!variableDict.ContainsKey(variableName))
            {
                variableDict[variableName] = new VarTracerVariable(variableName, groupName);
            }
             variableDict[variableName].InsertValue(new VarDataInfo(value, VarTracerNet.Instance.GetCurrentFrameFromTimestamp(timeStamp)));
        }

        public static void DefineEvent(string eventName, string variableBody)
        {
#if UNITY_EDITOR
            if (string.IsNullOrEmpty(eventName))
                return;

            if (!VarTracer.Instance.groups.ContainsKey(variableBody))
            {
                var body = new VarTracerGroup(variableBody);
                VarTracer.Instance.groups[variableBody] = body;
            }

            foreach (var varBody in VarTracer.Instance.groups)
            {
                foreach (var eName in varBody.Value.EventInfos.Keys)
                {
                    if (eventName.Equals(eName))
                    {
                        //Debug.LogErrorFormat("Define Event Name Already Exist!");
                        return;
                    }
                }
            }
            VarTracer.Instance.groups[variableBody].RegistEvent(eventName);
#endif
        }

        public static void SendEvent(long timeStamp, string eventName, float duration = 0, string desc = "")
        {
            if (!GraphItWindow.isVarTracerStart())
                return;
            foreach (var varBody in VarTracer.Instance.groups)
            {
                foreach (var eName in varBody.Value.EventInfos.Keys)
                {
                    if (eventName.Equals(eName))
                    {
                        varBody.Value.EventInfos[eventName].EventDataList.Add(new EventData(timeStamp, eventName, desc, duration));
                        break;
                    }
                }
            }
        }

        public static void StartVarTracer()
        {
            GraphItWindow.StartVarTracer();
        }

        public static void StopVarTracer()
        {
            GraphItWindow.StopVarTracer();
        }
    }

}
