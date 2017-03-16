using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace VariableTracer
{
    public class EventData
    {
        private long m_timeStamp;
        public long TimeStamp
        {
            get { return m_timeStamp; }
            set { m_timeStamp = value; }
        }

        private string m_eventName;
        public string EventName
        {
            get { return m_eventName; }
            set { m_eventName = value; }
        }
        private int m_eventFrameIndex;

        public int EventFrameIndex
        {
            get { return m_eventFrameIndex; }
            set { m_eventFrameIndex = value; }
        }
        private float m_duration;

        public float Duration
        {
            get { return m_duration; }
            set { m_duration = value; }
        }
        private string m_desc;
        public string Desc
        {
            get { return m_desc; }
            set { m_desc = value; }
        }

        public EventData(long eventTimeStamp, string eventName, string desc = "", float duration = 0)
        {
            m_timeStamp = eventTimeStamp;
            m_eventName = eventName;
            m_duration = duration;
            m_desc = desc;
            m_eventFrameIndex = VarTracerNet.Instance.GetCurrentFrameFromTimestamp(m_timeStamp);
        }
    }
}
