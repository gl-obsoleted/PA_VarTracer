using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;
using UnityEngine;
//Demo
//VarTracerTools.SendEvent("Group","TestEvent",0.5f);
//VarTracerTools.UpdateVariable("Player", "PlayerV_X", PlayerScript.GetVelocity().x);

//var vps = new VariableParm[100];
//for (int i = 0; i < 100; i++)
//{
//    VariableParm vp;
//    vp.VariableName = string.Format("Var_{0}",i);
//    vp.VariableValue = PlayerScript.GetVelocity().x;
//    vps[i] = vp;
//}

//var eps = new EventParm[2];
//for (int i = 0; i < 2; i++)
//{
//    EventParm ep;
//    ep.EventName = string.Format("Eve_{0}", i);
//    ep.EventDuration = 1;
//    eps[i] = ep;
//}

//VarTracerTools.SendGroup("Group",vps,eps);

namespace VariableTracer
{
    public class VarTracerTools
    {
        public static void UpdateVariable(string variableName, float value)
        {
            UpdateVariable(VarTracerConst.DEFAULT_GROUP_NAME, variableName, value);
        }

        public static void UpdateVariable(string groupName, string variableName, float value)
        {
            if (UsNet.Instance.IsSendAvaiable())
                VarTracerSender.Instance.CmdCacher.SendVariable(groupName, variableName, value);
        }

        public static void SendEvent(string eventName, float duration = 0)
        {
            if (UsNet.Instance.IsSendAvaiable())
                SendEvent(VarTracerConst.DEFAULT_GROUP_NAME, eventName, duration);
        }

        public static void SendEvent(string groupName, string eventName, float duration = 0)
        {
            VarTracerSender.Instance.CmdCacher.SendEvent(groupName, eventName, duration);
        }

        public static void SendGroup(string name, VariableParm[] vp = null)
        {
            SendGroup(name, vp,null);
        }

        public static void SendGroup(string name, EventParm[] ep = null)
        {
            SendGroup(name, null, ep);
        }

        public static void SendGroup(string name, VariableParm[] vp, EventParm[] ep)
        {
            if (string.IsNullOrEmpty(name))
                name = VarTracerConst.DEFAULT_GROUP_NAME;
            if(vp !=null)
            {
                foreach (var variable in vp)
                {
                    if (!string.IsNullOrEmpty(variable.VariableName))
                        UpdateVariable(name, variable.VariableName, variable.VariableValue);
                }
            }

            if(ep != null)
            {
                foreach (var eve in ep)
                {
                    if (!string.IsNullOrEmpty(eve.EventName))
                        SendEvent(name,eve.EventName, eve.EventDuration);
                }
            }
        }
    }
  
}
