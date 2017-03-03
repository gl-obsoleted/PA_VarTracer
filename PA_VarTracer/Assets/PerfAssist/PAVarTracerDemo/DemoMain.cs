using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DemoMain : MonoBehaviour {
    private UsMain _usmooth;
    public bool LogRemotely = true;
    public bool LogIntoFile = false;
    public bool InGameGui = false;

    void Start()
    {
        _usmooth = new UsMain(LogRemotely, LogIntoFile, InGameGui);
    }

	// Update is called once per frame
	void Update () {

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

        //VarTracerTools.UpdateVariable("CameraV_X", Camera.main.velocity.x);
        //VarTracerTools.UpdateVariable("CameraV_Y", Camera.main.velocity.y);
        //VarTracerTools.UpdateVariable("CameraV_Z", Camera.main.velocity.z);
        //VarTracerTools.UpdateVariable("CameraV_T", Camera.main.velocity.magnitude);


        //VarTracerTools.UpdateVariable("CameraV_X", Camera.main.velocity.x);
        //VarTracerTools.UpdateVariable("CameraV_Y", Camera.main.velocity.y);
        //VarTracerTools.UpdateVariable("CameraV_Z", Camera.main.velocity.z);
        //VarTracerTools.UpdateVariable("CameraV_T", Camera.main.velocity.magnitude);


        //VarTracerTools.UpdateVariable("CameraV_X", Camera.main.velocity.x);
        //VarTracerTools.UpdateVariable("CameraV_Y", Camera.main.velocity.y);
        //VarTracerTools.UpdateVariable("CameraV_Z", Camera.main.velocity.z);
        //VarTracerTools.UpdateVariable("CameraV_T", Camera.main.velocity.magnitude);

        var PlayerObj = GameObject.Find("Player");
        var PlayerScript = PlayerObj.GetComponent("PlayerTest") as PlayerTest;
        VarTracerTools.UpdateVariable("PlayerV_X", PlayerScript.GetVelocity().x);
        VarTracerTools.UpdateVariable("PlayerV_Y", PlayerScript.GetVelocity().y);
        VarTracerTools.UpdateVariable("PlayerV_Z", PlayerScript.GetVelocity().z);
        VarTracerTools.UpdateVariable("PlayerV_T", PlayerScript.GetVelocity().magnitude);
	}

    void OnDestroy()
    {
        if (_usmooth != null)
            _usmooth.Dispose();
    }

    void OnGUI()
    {
        if (_usmooth != null)
            _usmooth.OnGUI();
    }

    void OnLevelWasLoaded()
    {
        if (_usmooth != null)
            _usmooth.OnLevelWasLoaded();
    }

}
