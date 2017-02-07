using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TestGraph : MonoBehaviour {


	// Use this for initialization
	void Start () {
        Application.runInBackground = true;
        GraphItVar.DefineVisualChannel("A", 200, true, false);
        GraphItVar.DefineVisualChannel("B", 200, true, false);
        GraphItVar.DefineVisualChannel("C", 200, true, false);

        GraphItVar.DefineVariable("NPC","X",Color.green);
        GraphItVar.DefineVariable("NPC", "Y",Color.yellow);
        GraphItVar.DefineVariable("WBO", "Z", Color.cyan);
       // GraphItVar.DefineVariable("NPC", "Z", Color.cyan);

        GraphItVar.AttachVariable("X", "A");
        GraphItVar.AttachVariable("Y", "B");
        GraphItVar.AttachVariable("Z", "B");
        GraphItVar.AttachVariable("X", "C");


        GraphItVar.DefineEvent("NPC", "MOVE");
        GraphItVar.DefineEvent("NPC", "JUMP");
        GraphItVar.DefineEvent("WBO", "ATTACK");

	}
	
	// Update is called once per frame
	void Update () {
        GameObject NpcObj = GameObject.Find("Npc");

        GraphItVar.UpdateVariable("X", NpcObj.transform.position.x);

        GraphItVar.UpdateVariable("Y", NpcObj.transform.position.y);

        GraphItVar.UpdateVariable("Z", NpcObj.transform.position.z);

       // CheckInput();
	}


    void CheckInput()
    {
        if (Input.GetKey(KeyCode.E))
        {
            GraphItVar.SendEvent("MOVE");
        }

    }

}
