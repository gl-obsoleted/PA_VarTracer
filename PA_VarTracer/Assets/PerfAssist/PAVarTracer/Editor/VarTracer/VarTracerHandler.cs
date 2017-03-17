using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
namespace VariableTracer
{
    public class VarTracerHandler
    {
        public static void DefineVariable(string variableName, string groupName)
        {
#if UNITY_EDITOR
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
            if (!VarTracerWindow.isVarTracerStart())
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

        public static void DefineEvent(string eventName, string groupName)
        {
#if UNITY_EDITOR
            if (string.IsNullOrEmpty(eventName))
                return;

            if (!VarTracer.Instance.groups.ContainsKey(groupName))
            {
                var body = new VarTracerGroup(groupName);
                VarTracer.Instance.groups[groupName] = body;
            }

            if (!VarTracer.Instance.groups[groupName].EventInfos.ContainsKey(eventName))
                VarTracer.Instance.groups[groupName].RegistEvent(eventName);
#endif
        }

        public static void SendEvent(string groupName , long timeStamp, string eventName, float duration = 0)
        {
            if (!VarTracerWindow.isVarTracerStart())
                return;

            if (!VarTracer.Instance.groups.ContainsKey(groupName))
            {
                var body = new VarTracerGroup(groupName);
                VarTracer.Instance.groups[groupName] = body;
            }
            
            var eventInfo = VarTracer.Instance.groups[groupName].EventInfos;
            if(!eventInfo.ContainsKey(eventName))
                VarTracer.Instance.groups[groupName].RegistEvent(eventName);
            
            eventInfo[eventName].EventDataList.Add(new EventData(timeStamp, eventName, duration));
        }

        public static void StartVarTracer()
        {
            VarTracerWindow.StartVarTracer();
        }

        public static void StopVarTracer()
        {
            VarTracerWindow.StopVarTracer();
        }
    }

}
