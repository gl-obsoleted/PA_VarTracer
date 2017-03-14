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

    public void SendVarTracerCmd()
    {
        int byteSize =0,groupCount =0;
        //Cmd Count 
        byteSize += VarTracerConst.ByteSize_Int;
        //group Count
        byteSize += VarTracerConst.ByteSize_Int;
        foreach (var package in _cmdCacher.GroupCmdPackage)
        {
            if (package.Value.IsUse())
            {
                byteSize +=Encoding.Default.GetByteCount(package.Key);
                groupCount++;

                //variable count
                byteSize += VarTracerConst.ByteSize_Int;
                foreach(var list in package.Value.VariableDict.Values){
                    if(list.IsUse())
                    {
                        //cacheList count
                        byteSize += VarTracerConst.ByteSize_Int;
                        
                        //use VariableParm size
                        byteSize += list.UseIndex * VarTracerConst.ByteSize_VariableParm;    
                    }
                }

                //event count
                byteSize += VarTracerConst.ByteSize_Int;
                foreach (var list in package.Value.EventDict.Values)
                {
                    if (list.IsUse())
                    {
                        //cacheList count
                        byteSize += VarTracerConst.ByteSize_Int;

                        //use EventParm size
                        byteSize += list.UseIndex * VarTracerConst.ByteSize_EventParm;
                    }
                }
            }
        }

        byte[] b = new Byte[byteSize];
        //var test = byteSize;
        //UsCmd pkt = new UsCmd();
        //pkt.WriteNetCmd(eNetCmd.SV_VarTracerJsonParameter);
        //pkt.WriteString(JsonUtility.ToJson(vtjt));
        //UsNet.Instance.SendCommand(pkt);

        //_cmdCacher.GroupCmdPackage
        //string vtjtJson = JsonUtility.ToJson(_cmdCacher.GroupCmdPackage);
        _cmdCacher.Clear();
    }

    void Update()
    {
        SendVarTracerCmd();
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
