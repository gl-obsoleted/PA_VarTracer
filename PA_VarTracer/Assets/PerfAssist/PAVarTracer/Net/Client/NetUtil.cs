using System.Net.Sockets;
using UnityEngine;
namespace VariableTracer
{
    public delegate void NetLogHandler(string fmt, params object[] args);

    public static class NetUtil
    {
        public static NetLogHandler LogHandler { get; set; }
        public static NetLogHandler LogErrorHandler { get; set; }

        public static void Log(string fmt, params object[] args)
        {
            if (LogHandler != null)
                LogHandler(fmt, args);
        }

        public static void LogError(string fmt, params object[] args)
        {
            if (LogErrorHandler != null)
                LogErrorHandler(fmt, args);
        }

        public static bool ReadStreamData(TcpClient client, ref byte[] buffer)
        {
            if (client == null || !client.GetStream().CanRead 
                || buffer == null || buffer.Length == 0)
            {
                Debug.LogErrorFormat("client = {0},ava ={1} ,canRead ={2},buffer ={3},bufferLen ={4}", client == null, client.Available, !client.GetStream().CanRead
                    , buffer == null, buffer.Length == 0);
                return false;
            }

            int target = buffer.Length;
            int len = 0;
            do
            {
                len = client.GetStream().Read(buffer, len, target);
                if (len < 0)
                {
                    Debug.LogError("len <0");
                    return false;
                }
                    

                target -= len;
            }
            while (target > 0);

            return true;
        }

    }
}