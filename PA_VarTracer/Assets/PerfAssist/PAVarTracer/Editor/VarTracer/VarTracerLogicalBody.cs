using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace VariableTracer
{
    public class EventInfoData
    {
        List<EventData> eventDataList = new List<EventData>();
        public List<EventData> EventDataList
        {
            get { return eventDataList; }
            set { eventDataList = value; }
        }
        private bool isCutFlag = false;
        public bool IsCutFlag
        {
            get { return isCutFlag; }
            set { isCutFlag = value; }
        }
        private long timeStamp;
        public long TimeStamp
        {
            get { return timeStamp; }
            set { timeStamp = value; }
        }
    }

    public class VarTracerLogicalBody
    {
        private string channelName;
        public string ChannelName
        {
            get { return channelName; }
            set { channelName = value; }
        }

        private string m_variableBodyName;
        public string VariableBodyName
        {
            get { return m_variableBodyName; }
            set { m_variableBodyName = value; }
        }

        public VarTracerLogicalBody(string varBodyName)
        {
            m_variableBodyName = varBodyName;
        }

        private Dictionary<string, VarTracerVariable> m_variableDict = new Dictionary<string, VarTracerVariable>();
        public Dictionary<string, VarTracerVariable> VariableDict
        {
            get { return m_variableDict; }
            set { m_variableDict = value; }
        }

        //eventName  eventData
        Dictionary<string, EventInfoData> eventInfos = new Dictionary<string, EventInfoData>();

        public Dictionary<string, EventInfoData> EventInfos
        {
            get { return eventInfos; }
            set { eventInfos = value; }
        }

        public void RegistEvent(string eventName)
        {
            if (string.IsNullOrEmpty(eventName))
                return;
            if (!eventInfos.ContainsKey(eventName))
                eventInfos[eventName] = new EventInfoData();
        }
    }

}
