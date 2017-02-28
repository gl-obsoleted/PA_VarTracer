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
    
    readonly static object _locker = new object();

    static EventWaitHandle _wh = new AutoResetEvent(false);
    
    Thread m_MsgThread;

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
    }

    private void SendMsgAsyn()
    {
        while(true)
        {
            lock (_locker)
            {

                if (sendMsgList.Count > 0)
                {
                    var vtjt = sendMsgList[0];
                    UsCmd pkt = new UsCmd();
                    pkt.WriteNetCmd(eNetCmd.SV_VarTracerJsonParameter);
                    pkt.WriteString(JsonUtility.ToJson(vtjt));
                    UsNet.Instance.SendCommand(pkt);
                    sendMsgList.RemoveAt(0);
                }
            }
            if (sendMsgList.Count ==0)
                _wh.WaitOne();
        }
    }
    public static void SendJsonMsg(VarTracerJsonType vtjt)
    {
        lock (_locker)
            sendMsgList.Add(vtjt);  // 向队列中插入任务 
        _wh.Set();  // 给工作线程发信号
    }

    //public static void SendJsonMsg(string json)
    //{
    //    if (string.IsNullOrEmpty(json))
    //        return;
    //    UsCmd pkt = new UsCmd();
    //    pkt.WriteNetCmd(eNetCmd.SV_VarTracerJsonParameter);
    //    pkt.WriteString(json);
    //    //pkt.WriteString(VarTracerUtils.GetTimeStamp().ToString());
    //    UsNet.Instance.SendCommand(pkt);
    //}

    public static void DefineVariable(string variableName, string LogicalName)
    {
        VarTracerJsonType vtjt = new VarTracerJsonType();
        vtjt.logicName = LogicalName;
        vtjt.variableName = new string[] { variableName };
        vtjt.variableValue = new float[] { 0 };
        vtjt.timeStamp = VarTracerUtils.GetTimeStamp();
        SendJsonMsg(vtjt);
    }
    public void UpdateVariable(string variableName, float value)
    {
        VarTracerJsonType vtjt = new VarTracerJsonType();
        vtjt.variableName = new string[] { variableName };
        vtjt.variableValue = new float[] { value };
        vtjt.timeStamp = VarTracerUtils.GetTimeStamp();
        SendJsonMsg(vtjt);
    }
    public static void DefineEvent(string eventName, string variableBody)
    {
        VarTracerJsonType vtjt = new VarTracerJsonType();
        vtjt.logicName = variableBody;
        vtjt.eventName = new string[] { eventName };
        vtjt.eventDuration = new float[] { -1 };
        vtjt.eventDesc = new string[] {""};
        vtjt.timeStamp = VarTracerUtils.GetTimeStamp();
        SendJsonMsg(vtjt);
    }
    public static void SendEvent(string eventName, float duration = 0, string desc = "")
    {
        VarTracerJsonType vtjt = new VarTracerJsonType();
        vtjt.eventName = new string[] { eventName };
        vtjt.eventDuration = new float[] { duration };
        vtjt.eventDesc = new string[] { desc };
        vtjt.timeStamp = VarTracerUtils.GetTimeStamp();
        SendJsonMsg(vtjt);
    }

    public static void StartVarTracer()
    {
        VarTracerJsonType vtjt = new VarTracerJsonType();
        vtjt.runingState = (int)VarTracerConst.RunningState.RunningState_Start;
        SendJsonMsg(vtjt);
    }

    public static void StopVarTracer()
    {
        VarTracerJsonType vtjt = new VarTracerJsonType();
        vtjt.runingState = (int)VarTracerConst.RunningState.RunningState_Pause;
        sendMsgList.Add(vtjt);
    }
}
