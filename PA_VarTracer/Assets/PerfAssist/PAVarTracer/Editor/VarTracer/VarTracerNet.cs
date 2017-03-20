using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using System.Text.RegularExpressions;
using System.Threading;

namespace VariableTracer
{
    public class VarTracerNet
    {
        public static VarTracerNet mInstance = null;
        private float m_lastHandleJsonTime;

        private long m_startTimeStamp;
        public long StartTimeStamp
        {
            get { return m_startTimeStamp; }
            set { m_startTimeStamp = value; }
        }

        private long m_netDeltaTime;
        public long NetDeltaTime
        {
            get { return m_netDeltaTime; }
            set { m_netDeltaTime = value; }
        }

        List<string> vartracerJsonMsgList = new List<string>();
        public List<string> VartracerJsonMsgList
        {
            get { return vartracerJsonMsgList; }
        }

        public void Upate()
        {
        }

        public int GetCurrentFrameFromTimestamp(long timeStamp)
        {
            int currentFrame = (int)((timeStamp - m_startTimeStamp) / 1000.0f * VarTracerConst.FPS);
            return currentFrame;
        }

        public bool Handle_VarTracerInfo(eNetCmd cmd, UsCmd c)
        {
            int groupCount = c.ReadInt32();
            //NetUtil.Log("read group count: {0}.", groupCount);

            for (int i = 0; i < groupCount; i++)
            {
                var groupName = c.ReadString();
                //NetUtil.Log("read group Name: {0}.", groupName);
                var variableCount = c.ReadInt32();
                //NetUtil.Log("read var count : {0}.", variableCount);
                for (int j = 0; j < variableCount; j++)
                {
                    var variableName = c.ReadString();
                    //NetUtil.Log("read variableName: {0}.", variableName);
                    var sessionCount = c.ReadInt32();
                    //NetUtil.Log("read sessionCount: {0}.", sessionCount);
                    for (int k = 0; k < sessionCount; k++)
                    {
                        long stamp = c.ReadLong();
                        if (VarTracerNet.Instance.StartTimeStamp == 0)
                        {
                            VarTracerNet.Instance.StartTimeStamp = VarTracerUtils.GetTimeStamp();
                            VarTracerNet.Instance.NetDeltaTime = VarTracerNet.Instance.StartTimeStamp - stamp;
                        }
                        stamp += VarTracerNet.Instance.NetDeltaTime;

                        //NetUtil.Log("read stamp: {0}.", stamp);
                        float value = c.ReadFloat();
                        //NetUtil.Log("read value: {0}.", value);
                        VarTracerHandler.UpdateVariable(groupName, variableName, stamp,value);
                    }
                }

                var eventCount = c.ReadInt32();
                for (int j = 0; j < eventCount; j++)
                {
                    var eventName = c.ReadString();
                    var sessionCount = c.ReadInt32();
                    for (int k = 0; k < sessionCount; k++)
                    {
                        long stamp = c.ReadLong();
                        float duration = c.ReadFloat();
                        VarTracerHandler.SendEvent(groupName, stamp, eventName, duration);
                    }
                }
            }

            return true;
        }

        public static VarTracerNet Instance
        {
            get
            {
#if UNITY_EDITOR
                if (mInstance == null)
                {
                    mInstance = new VarTracerNet();
                }
                return mInstance;
#else
            return null;
#endif
            }
        }
    }

}
