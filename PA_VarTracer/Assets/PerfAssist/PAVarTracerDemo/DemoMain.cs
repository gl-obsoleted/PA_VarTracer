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

        var NpcObj = GameObject.Find("Npc");
        var NpcScript = NpcObj.GetComponent("NpcTest") as NpcTest;
        ////VarTracerTool.UpdateVariable("NpcV_X", NpcScript.GetVelocity().x);
        VarTracerTools.Instance.UpdateVariable("NpcV_Y", NpcScript.GetVelocity().y);
        VarTracerTools.Instance.UpdateVariable("NpcV_Z", NpcScript.GetVelocity().z);
        VarTracerTools.Instance.UpdateVariable("NpcV_T", NpcScript.GetVelocity().magnitude);

        VarTracerTools.Instance.UpdateVariable("CameraV_X", Camera.main.velocity.x);
        VarTracerTools.Instance.UpdateVariable("CameraV_Y", Camera.main.velocity.y);
        VarTracerTools.Instance.UpdateVariable("CameraV_Z", Camera.main.velocity.z);
        VarTracerTools.Instance.UpdateVariable("CameraV_T", Camera.main.velocity.magnitude);

        var PlayerObj = GameObject.Find("Player");
        var PlayerScript = PlayerObj.GetComponent("PlayerTest") as PlayerTest;
        VarTracerTools.Instance.UpdateVariable("PlayerV_X", PlayerScript.GetVelocity().x);
        VarTracerTools.Instance.UpdateVariable("PlayerV_Y", PlayerScript.GetVelocity().y);
        VarTracerTools.Instance.UpdateVariable("PlayerV_Z", PlayerScript.GetVelocity().z);
        VarTracerTools.Instance.UpdateVariable("PlayerV_T", PlayerScript.GetVelocity().magnitude);
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
