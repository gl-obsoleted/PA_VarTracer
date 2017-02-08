using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TestGraph : MonoBehaviour {


	// Use this for initialization
	void Start () {
        Application.runInBackground = true;

        GraphItVar.DefineVariable("X", "NPC", Color.green);
        GraphItVar.DefineVariable("Y", "NPC", Color.yellow);
        GraphItVar.DefineVariable("Z", "WBO", Color.cyan);
        GraphItVar.DefineVariable("W", "WBO", Color.blue);
       // GraphItVar.DefineVariable("NPC", "Z", Color.cyan);

        GraphItVar.DefineEvent("MOVE","NPC");
        GraphItVar.DefineEvent("JUMP","NPC");
        GraphItVar.DefineEvent("ATTACK","WBO");

	}
	
	// Update is called once per frame
	void Update () {
        GameObject NpcObj = GameObject.Find("Npc");

        GraphItVar.UpdateVariable("X", NpcObj.transform.position.x);

        GraphItVar.UpdateVariable("Y", NpcObj.transform.position.y);

        GraphItVar.UpdateVariable("Z", NpcObj.transform.position.z);
	}


}
