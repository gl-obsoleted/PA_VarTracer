using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;
using UnityEngine;

public class VarTracerTools : MonoBehaviour
{
    public static VarTracerTools mInstance = null;

    static List<VarTracerJsonType> sendMsgList = new List<VarTracerJsonType>();
    public static List<VarTracerJsonType> sendMsgTempList = new List<VarTracerJsonType>();

    readonly static object _locker = new object();

    public static EventWaitHandle _wh = new AutoResetEvent(false);
    
    Thread m_MsgThread;

    private float m_lastHandleJsonTime;
    void Start()
    {
        m_MsgThread = new Thread(new ThreadStart(SendMsgAsyn));
        m_MsgThread.Start();
    }

    public static VarTracerTools Instance
    {
        get
        {
#if UNITY_EDITOR
            if (mInstance == null)
            {
                if (UsNet.Instance == null && UsNet.Instance.CmdExecutor == null)
                {
                    UnityEngine.Debug.LogError("UsNet not available");
                    return null;
                }

                GameObject go = new GameObject("VarTracerTools");
                go.hideFlags = HideFlags.HideAndDontSave;
                mInstance = go.AddComponent<VarTracerTools>();
            }
            return mInstance;
#else
            return null;
#endif
        }
    }

    void Update()
    {
        if (Time.realtimeSinceStartup - m_lastHandleJsonTime >= VarTracerConst.SEND_MSG_INTERVAL)
        {
            m_lastHandleJsonTime = Time.realtimeSinceStartup;
            if (sendMsgList.Count > 0 && sendMsgTempList.Count == 0)
                _wh.Set();
        }
    }

    private void SendMsgAsyn()
    {
        while (true)
        {
            if (sendMsgList.Count>0 )
            {
                lock (_locker)
                {
                    sendMsgTempList.Clear();
                    sendMsgTempList.AddRange(sendMsgList.ToArray());
                    sendMsgList.Clear();
                }

                foreach (var vtjt in sendMsgTempList)
                {
                    UsCmd pkt = new UsCmd();
                    pkt.WriteNetCmd(eNetCmd.SV_VarTracerJsonParameter);
                    pkt.WriteString(JsonUtility.ToJson(vtjt));
                    UsNet.Instance.SendCommand(pkt);
                }

                sendMsgTempList.Clear();
                _wh.WaitOne();
            }
        }
    }

    public void SendJsonMsg(VarTracerJsonType vtjt)
    {
        if (vtjt.timeStamp == 0)
            vtjt.timeStamp = VarTracerUtils.GetTimeStamp();
        lock (_locker)
        {
            sendMsgList.Add(vtjt);
        }

        //if (Monitor.TryEnter(_locker))
        //{
        //    lock (_locker)
        //    {
        //        sendMsgList.AddRange(sendMsgTemp.ToArray());
        //        sendMsgTemp.Clear();
        //        sendMsgList.Add(vtjt);
        //    }
        //    //_wh.Set();
        //    Monitor.Exit(_locker);
        //}
        //else
        //{
        //    sendMsgTemp.Add(vtjt);
        //}
    }

    public void DefineVariable(string variableName, string LogicalName)
    {
        VarTracerJsonType vtjt = new VarTracerJsonType();
        vtjt.logicName = LogicalName;
        vtjt.variableName = new string[] { variableName };
        vtjt.variableValue = new float[] { 0 };
        //vtjt.timeStamp = VarTracerUtils.GetTimeStamp();
        SendJsonMsg(vtjt);
    }

    public void UpdateVariable(string variableName, float value)
    {
        VarTracerJsonType vtjt = new VarTracerJsonType();
        vtjt.variableName = new string[] { variableName };
        vtjt.variableValue = new float[] { value };
        //vtjt.timeStamp = VarTracerUtils.GetTimeStamp();
        SendJsonMsg(vtjt);
    }

    public void DefineEvent(string eventName, string variableBody)
    {
        VarTracerJsonType vtjt = new VarTracerJsonType();
        vtjt.logicName = variableBody;
        vtjt.eventName = new string[] { eventName };
        vtjt.eventDuration = new float[] { -1 };
        vtjt.eventDesc = new string[] {""};
        //vtjt.timeStamp = VarTracerUtils.GetTimeStamp();
        SendJsonMsg(vtjt);
    }

    public void SendEvent(string eventName, float duration = 0, string desc = "")
    {
        VarTracerJsonType vtjt = new VarTracerJsonType();
        vtjt.eventName = new string[] { eventName };
        vtjt.eventDuration = new float[] { duration };
        vtjt.eventDesc = new string[] { desc };
        //vtjt.timeStamp = VarTracerUtils.GetTimeStamp();
        SendJsonMsg(vtjt);
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
