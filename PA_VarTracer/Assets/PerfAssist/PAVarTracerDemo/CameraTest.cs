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
            VarTracerTools.Instance.SendEvent("MOVE");
        }

        if (Input.GetKeyUp(KeyCode.Z))
        {
            VarTracerTools.Instance.UpdateVariable("NpcV_X", 0);
        }

        if (Input.GetKeyUp(KeyCode.X))
        {
            VarTracerTools.Instance.UpdateVariable("NpcV_X", 5);
        }

        if (Input.GetKeyUp(KeyCode.C))
        {
            VarTracerTools.Instance.UpdateVariable("NpcV_X", 10);
        }

        if (Input.GetKeyUp(KeyCode.V))
        {
            VarTracerTools.Instance.UpdateVariable("NpcV_X", -10);
        }

        if (Input.GetKeyUp(KeyCode.U))
        {
            VarTracerJsonType vtjt = new VarTracerJsonType();
            vtjt.logicName = "Npc";
            vtjt.eventName = new string[] { "NPC_MOVE" };
            vtjt.eventDuration = new float[] { 0 };
            vtjt.eventDesc = new string[] { "NPC" };
            VarTracerTools.Instance.SendJsonMsg(vtjt);

           // VarTracerTools.SendEvent("NPC_MOVE");
        }

        if (Input.GetKeyUp(KeyCode.I))
        {
            VarTracerJsonType vtjt = new VarTracerJsonType();
            vtjt.logicName = "Npc";
            vtjt.eventName = new string[] { "NPC_MOVE" };
            vtjt.eventDuration = new float[] { 1 };
            vtjt.eventDesc = new string[] { "NPC" };
            VarTracerTools.Instance.SendJsonMsg(vtjt);
            //VarTracerTools.SendEvent("NPC_MOVE", 1);
        }

        if (Input.GetKeyUp(KeyCode.O))
        {
            VarTracerTools.Instance.SendEvent("NPC_MOVE", 2, "desc");
        }

        if (Input.GetKeyUp(KeyCode.J))
        {
            //Json测试格式
            //{
            //"logicName":"Npc",
            //"variableName":["NpcV_X","NpcV_Y","NpcV_Z","NpcV_T"],
            //"variableValue":[1,2,3,4],
            //"eventName":["NPC_MOVE"],
            //"eventDuration":[1.2],
            //"eventDesc":["desc"]
            //}

            VarTracerJsonType vtjt = new VarTracerJsonType();
            vtjt.logicName = "JSON";
            vtjt.variableName = new string[] { "JSON_X", "JSON_Y", "JSON_Z", "JSON_T" };
            vtjt.variableValue = new float[] { 1.0f, 2.0f, 3.0f, 4.0f };
            vtjt.eventName = new string[] { "JSON_EVENT" };
            vtjt.eventDuration = new float[] { 0.5f };
            vtjt.eventDesc = new string[] { "JSON" };
            VarTracerTools.Instance.SendJsonMsg(vtjt);
        }

        if (Input.GetKeyUp(KeyCode.T))
        {
            //UnityEngine.Debug.LogFormat("preCount{0} ={1}", VarTracerTools.reciveCount + VarTracerTools.sendMsgTempList.Count, VarTracerTools.sendCount);
        }

        if (Input.GetKeyUp(KeyCode.K))
        {

            VarTracerJsonType vtjt = new VarTracerJsonType();
            vtjt.runingState = (int)VarTracerConst.RunningState.RunningState_Start;
            VarTracerTools.Instance.SendJsonMsg(vtjt);

        }

        if (Input.GetKeyUp(KeyCode.L))
        {
            VarTracerJsonType vtjt = new VarTracerJsonType();
            vtjt.runingState = (int)VarTracerConst.RunningState.RunningState_Pause;
            VarTracerTools.Instance.SendJsonMsg(vtjt);
        }
    }
}
