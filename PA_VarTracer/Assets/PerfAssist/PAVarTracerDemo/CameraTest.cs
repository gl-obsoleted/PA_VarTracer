using UnityEngine;
using System.Collections;
using System.Threading;

public class CameraTest : MonoBehaviour {
    // Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
        CheckInput();
	}

    void CheckInput()
    {
        if (Input.GetKey(KeyCode.S))
            transform.Translate(Vector3.down * Time.deltaTime * (1 + UnityEngine.Random.value * 0.2f));

        //按下键盘“W键”
        if (Input.GetKey(KeyCode.W))
            transform.Translate(Vector3.up * Time.deltaTime * (1 + UnityEngine.Random.value * 0.5f));

        if (Input.GetKey(KeyCode.A))
        {
            transform.Translate(Vector3.left * Time.deltaTime * (1 + UnityEngine.Random.value * 0.7f));
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.Translate(Vector3.right * Time.deltaTime * (1 + UnityEngine.Random.value * 0.8f));
        }

        if (Input.GetKey(KeyCode.F))
        {
            transform.Translate(Vector3.forward * Time.deltaTime * (1 + UnityEngine.Random.value * 1.5f));
        }

        if (Input.GetKey(KeyCode.B))
        {
            transform.Translate(Vector3.back * Time.deltaTime * (1 + UnityEngine.Random.value * 0.1f));
        }

        if (Input.GetKeyUp(KeyCode.E))
        {
            VarTracerTools.SendEvent("MOVE");
        }

        if (Input.GetKeyUp(KeyCode.Z))
        {
            VarTracerTools.UpdateVariable("NpcV_X", 0);
        }

        if (Input.GetKeyUp(KeyCode.X))
        {
            VarTracerTools.UpdateVariable("NpcV_X", 5);
        }

        if (Input.GetKeyUp(KeyCode.C))
        {
            VarTracerTools.UpdateVariable("NpcV_X", 10);
        }

        if (Input.GetKeyUp(KeyCode.V))
        {
            VarTracerTools.UpdateVariable("NpcV_X", -10);
        }

        if (Input.GetKeyUp(KeyCode.O))
        {
            VarTracerTools.SendEvent("NPC_MOVE", 2, "desc");
        }

        if (Input.GetKeyUp(KeyCode.J))
        {
            VariableParm vp ;
            vp.VariableName = "ValueName";
            vp.VariableValue = 1.0f;

            EventParm ep;
            ep.EventName = "EventName";
            ep.EventDuration = 1.5f;
            ep.EventDesc = "EventDesc";

            VarTracerTools.SendGroupLoop(
                new Group("Test", new VariableParm[] { vp })
            );
       
        }
    }
}
