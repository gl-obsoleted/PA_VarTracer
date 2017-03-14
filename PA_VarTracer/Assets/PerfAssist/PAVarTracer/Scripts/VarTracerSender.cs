using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;

public class VarTracerSender : MonoBehaviour
{
    public static VarTracerSender mInstance;
    private VarTracerCmdCacher _cmdCacher = new VarTracerCmdCacher();
    public VarTracerCmdCacher CmdCacher
    {
        get { return _cmdCacher; }
    }

    void Start()
    {
    }
    
    public void Destroy()
    {
        mInstance = null;
    }

    void Update()
    {
        _cmdCacher.Clear();
        //_sendNetCmdWatch.Reset();
        //var msg = _sendMsgList.Dequeue();
        //_sendNetCmdWatch.Start();
        //byte[] b = new Byte[1024 * 1024];

        //string vtjtJson = JsonUtility.ToJson(msg);
        //if (_sendNetCmdWatch.ElapsedMilliseconds > 0)
        //    UnityEngine.Debug.LogFormat("json time = {0}", _sendNetCmdWatch.ElapsedMilliseconds);

        //while (_sendMsgList.Count > 0)
        //{
        //    _sendNetCmdWatch.Reset();
        //    var msg = _sendMsgList.Dequeue();
        //    _sendNetCmdWatch.Start();
        //    string vtjtJson = JsonUtility.ToJson(msg);
        //    if (_sendNetCmdWatch.ElapsedMilliseconds > 0)
        //        UnityEngine.Debug.LogFormat("json time = {0}", _sendNetCmdWatch.ElapsedMilliseconds);
        //    _sendNetCmdWatch.Reset();
        //    byte[] b = new Byte[Encoding.Default.GetBytes(vtjtJson).Length+4];
        //    _sendNetCmdWatch.Start();
        //    //byte[] b = new Byte[50];
        //    UsCmd pkt = new UsCmd(b);
        //    //_sendNetCmdWatch.Stop();
        //    if (_sendNetCmdWatch.ElapsedMilliseconds > 0)
        //        UnityEngine.Debug.LogFormat("init time = {0}", _sendNetCmdWatch.ElapsedMilliseconds);
        //    _sendNetCmdWatch.Reset();

        //    //UsCmd pkt = new UsCmd();
        //    _sendNetCmdWatch.Start();
        //    pkt.WriteNetCmd(eNetCmd.SV_VarTracerJsonParameter);
        //    pkt.WriteString(vtjtJson);
        //    //pkt.WriteString("xxxxxxxffffxx");
        //    if (_sendNetCmdWatch.ElapsedMilliseconds > 0)
        //        UnityEngine.Debug.LogFormat("write time = {0}", _sendNetCmdWatch.ElapsedMilliseconds);
        //    _sendNetCmdWatch.Reset();

        //    //pkt.WriteString(vtjtJson);
        //    _sendNetCmdWatch.Start();
        //    UsNet.Instance.SendCommand(pkt);
        //    if (_sendNetCmdWatch.ElapsedMilliseconds > 0)
        //        //UnityEngine.Debug.LogFormat("send time = {0}", _sendNetCmdWatch.ElapsedMilliseconds);
        //    _sendNetCmdWatch.Reset();
        //}
        //_sendNetCmdWatch.Reset();
    }

    public static VarTracerSender Instance
    {
        get
        {
            if (mInstance == null)
            {
                if (UsNet.Instance == null && UsNet.Instance.CmdExecutor == null)
                {
                    UnityEngine.Debug.LogError("UsNet not available");
                    return null;
                }

                GameObject go = new GameObject("VarTracerSender");
                go.hideFlags = HideFlags.HideAndDontSave;
                mInstance = go.AddComponent<VarTracerSender>();
            }
            return mInstance;
        }
    }
}
