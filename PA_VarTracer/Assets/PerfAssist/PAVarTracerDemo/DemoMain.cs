using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace VariableTracer
{
    public class DemoMain : MonoBehaviour
    {
        private UsMain _usmooth;
        public float updateInterval = 0.5F;

        private float accum = 0; // FPS accumulated over the interval
        private int frames = 0; // Frames drawn over the interval
        private float timeleft; // Left time for current interval

        void Start()
        {
            _usmooth = new UsMain();
        }

        // Update is called once per frame
        void Update()
        {

            if (_usmooth != null)
                _usmooth.Update();

            //var NpcObj = GameObject.Find("Npc");
            //var NpcScript = NpcObj.GetComponent("NpcTest") as NpcTest;
            ////VarTracerTool.UpdateVariable("NpcV_X", NpcScript.GetVelocity().x);
            //VarTracerTools.UpdateVariable("NpcV_Y", NpcScript.GetVelocity().y);
            //VarTracerTools.UpdateVariable("NpcV_Z", NpcScript.GetVelocity().z);
            //VarTracerTools.UpdateVariable("NpcV_T", NpcScript.GetVelocity().magnitude);

            //VarTracerTools.UpdateVariable("CameraV_X", float.Parse(Camera.main.velocity.x.ToString("F1")));
            //VarTracerTools.UpdateVariable("CameraV_Y", float.Parse(Camera.main.velocity.y.ToString("F1")));
            //VarTracerTools.UpdateVariable("CameraV_Z", float.Parse(Camera.main.velocity.z.ToString("F1")));
            //VarTracerTools.UpdateVariable("CameraV_T", float.Parse(Camera.main.velocity.magnitude.ToString("F1")));

            var PlayerObj = GameObject.Find("Player");
            var PlayerScript = PlayerObj.GetComponent("PlayerTest") as PlayerTest;

            VarTracerTools.UpdateVariable("Player", "PlayerV_X", PlayerScript.GetVelocity().x);
            VarTracerTools.UpdateVariable("Player", "PlayerV_Y", PlayerScript.GetVelocity().y);
            VarTracerTools.UpdateVariable("Player", "PlayerV_Z", PlayerScript.GetVelocity().z);
            VarTracerTools.UpdateVariable("Player", "PlayerV_T", PlayerScript.GetVelocity().magnitude);
            for (int i = 0; i < 250; i++)
            {
                
            }

            timeleft -= Time.deltaTime;
            accum += Time.timeScale / Time.deltaTime;
            ++frames;
            float fps = 0;
            // Interval ended - update GUI text and start new interval
            if (timeleft <= 0.0)
            {
                // display two fractional digits (f2 format)
                fps = accum / frames;
                //string format = System.String.Format("{0:F2} FPS", fps);
                //	DebugConsole.Log(format,level);
                timeleft = updateInterval;
                accum = 0.0F;
                frames = 0;
                VarTracerTools.UpdateVariable("System", "FPS", fps);
                //UnityEngine.Debug.LogFormat("FPS= {0}", fps);
            }
        }

        void OnDestroy()
        {
            if (_usmooth != null)
                _usmooth.Dispose();

            VarTracerSender.Instance.Destroy();
        }
    }

}
