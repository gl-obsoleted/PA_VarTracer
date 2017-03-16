using System;
using UnityEngine;

namespace VariableTracer
{
    public class UsMain : IDisposable
    {
        public const int MAX_CONTENT_LEN = 1024;

        private long _currentTimeInMilliseconds = 0;
        private long _tickNetLast = 0;
        private long _tickNetInterval = 0;

        public UsMain()
        {
            Application.runInBackground = true;

            UsNet.Instance = new UsNet();
            UsMain_NetHandlers.Instance = new UsMain_NetHandlers(UsNet.Instance.CmdExecutor);
        }

        public void Update()
        {
            _currentTimeInMilliseconds = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;

            if (_currentTimeInMilliseconds - _tickNetLast > _tickNetInterval)
            {
                if (UsNet.Instance != null)
                {
                    UsNet.Instance.Update();
                }

                _tickNetLast = _currentTimeInMilliseconds;
            }
        }

        public void Dispose()
        {
            UsNet.Instance.Dispose();
        }
    }
}