/*!lic_info

The MIT License (MIT)

Copyright (c) 2015 SeaSunOpenSource

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.

*/

﻿using UnityEngine;
using System.Collections;
using System;

using System.Collections.Generic;
namespace VariableTracer
{
    public class UsMain_NetHandlers
    {
        public static UsMain_NetHandlers Instance;

        public UsMain_NetHandlers(UsCmdParsing exec)
        {
            exec.RegisterHandler(eNetCmd.CL_Handshake, NetHandle_Handshake);
            exec.RegisterHandler(eNetCmd.CL_KeepAlive, NetHandle_KeepAlive);
        }

        private bool NetHandle_Handshake(eNetCmd cmd, UsCmd c)
        {
            Debug.Log("executing handshake.");
            UsCmd reply = new UsCmd();
            reply.WriteNetCmd(eNetCmd.SV_HandshakeResponse);
            UsNet.Instance.SendCommand(reply);
            return true;
        }

        private bool NetHandle_KeepAlive(eNetCmd cmd, UsCmd c)
        {
            UsCmd reply = new UsCmd();
            reply.WriteNetCmd(eNetCmd.SV_KeepAliveResponse);
            UsNet.Instance.SendCommand(reply);
            return true;
        }
    }
}