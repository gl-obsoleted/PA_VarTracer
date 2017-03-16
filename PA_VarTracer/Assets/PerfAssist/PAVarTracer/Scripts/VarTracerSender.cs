using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
namespace VariableTracer
{
    public class VarTracerSender : MonoBehaviour
    {
        public static VarTracerSender mInstance;
        private VarTracerCmdCacher _cmdCacher = new VarTracerCmdCacher();
        public VarTracerCmdCacher CmdCacher
        {
            get { return _cmdCacher; }
        }
        private UsCmd _uscmd = new UsCmd(new Byte[VarTracerConst.DefaultCmdByteSize]);
        void Start()
        {
        }

        public void Destroy()
        {
            mInstance = null;
        }

        private bool HasNewVarTracerCmd()
        {
            return _cmdCacher.GetUsedGroupCount() > 0;
        }

        public void SendVarTracerCmd()
        {
            if (!HasNewVarTracerCmd())
                return;
            int newSize = CalculateCmdSize();
            int currentSize = _uscmd.Buffer.Length;
            int preSize = currentSize;
            while (newSize > currentSize)
                currentSize *= 2;

            if (currentSize > preSize)
                _uscmd = new UsCmd(new byte[currentSize]);

            writeNetCmd(_uscmd);

            _cmdCacher.Clear();
        }

        private void writeNetCmd(UsCmd uscmd)
        {
            uscmd.clearUsCmd();
            uscmd.WriteNetCmd(eNetCmd.SV_VarTracerInfo);
            //group count
            uscmd.WriteInt32(_cmdCacher.GetUsedGroupCount());
            foreach (var package in _cmdCacher.GroupCmdPackage)
            {
                if (package.Value.IsUse())
                {
                    //group name
                    uscmd.WriteString(package.Key);

                    //variable count 
                    uscmd.WriteInt32(_cmdCacher.GetUsedVariableCount(package.Value));
                    foreach (var list in package.Value.VariableDict)
                    {
                        if (list.Value.IsUse())
                        {
                            //variable name 
                            uscmd.WriteString(list.Key);
                            //session count
                            uscmd.WriteInt32(list.Value.UseIndex);
                            var cacheList = list.Value;
                            int usedCount = cacheList.UseIndex;
                            for (int i = 0; i < usedCount; i++)
                            {
                                //stamp
                                uscmd.WriteInt32(312);
                                //uscmd.WriteLong(list.Value.VarChacheList[i]._stamp);
                                //value
                                uscmd.WriteFloat(list.Value.VarChacheList[i]._value);
                            }
                        }
                    }

                    //event count 
                    uscmd.WriteInt32(_cmdCacher.GetUsedEventCount(package.Value));
                    foreach (var list in package.Value.EventDict)
                    {
                        if (list.Value.IsUse())
                        {
                            //event name 
                            uscmd.WriteString(list.Key);
                            //session count
                            uscmd.WriteInt32(list.Value.UseIndex);
                            var cacheList = list.Value;
                            int usedCount = cacheList.UseIndex;
                            for (int i = 0; i < usedCount; i++)
                            {
                                //stamp
                                uscmd.WriteLong(list.Value.VarChacheList[i]._stamp);
                                //duration
                                uscmd.WriteFloat(list.Value.VarChacheList[i]._duration);
                            }
                        }
                    }
                }
            }

            UsNet.Instance.SendCommand(uscmd);
        }

        private int CalculateCmdSize()
        {
            int byteSize = 0, groupCount = 0;
            //Cmd Count 
            byteSize += VarTracerConst.ByteSize_Int;
            //group Count
            byteSize += VarTracerConst.ByteSize_Int;
            foreach (var package in _cmdCacher.GroupCmdPackage)
            {
                if (package.Value.IsUse())
                {
                    byteSize += Encoding.Default.GetByteCount(package.Key);
                    groupCount++;

                    //variable count
                    byteSize += VarTracerConst.ByteSize_Int;
                    foreach (var list in package.Value.VariableDict)
                    {
                        if (list.Value.IsUse())
                        {
                            //variable name 
                            byteSize += Encoding.Default.GetByteCount(list.Key);

                            //cacheList count
                            byteSize += VarTracerConst.ByteSize_Int;

                            //use VariableParm size
                            byteSize += list.Value.UseIndex * VarTracerConst.ByteSize_VariableParm;
                        }
                    }

                    //event count
                    byteSize += VarTracerConst.ByteSize_Int;
                    foreach (var list in package.Value.EventDict)
                    {
                        if (list.Value.IsUse())
                        {
                            //event name 
                            byteSize += Encoding.Default.GetByteCount(list.Key);

                            //cacheList count
                            byteSize += VarTracerConst.ByteSize_Int;

                            //use EventParm size
                            byteSize += list.Value.UseIndex * VarTracerConst.ByteSize_EventParm;
                        }
                    }
                }
            }
            return byteSize;
        }

        void Update()
        {
            SendVarTracerCmd();
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
}

