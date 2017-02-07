using UnityEngine;
using System.Collections;

public class NewBehaviourScript : MonoBehaviour {

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
            transform.Translate(Vector3.down * Time.deltaTime * 1);

        //按下键盘“W键”
        if(Input.GetKey(KeyCode.W))
            transform.Translate(Vector3.up * Time.deltaTime * 1);

        if (Input.GetKey(KeyCode.A))
        {
            transform.Translate(Vector3.left * Time.deltaTime * 1);
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.Translate(Vector3.right * Time.deltaTime * 1);
        }

        if (Input.GetKeyUp(KeyCode.E))
        {
            GraphItVar.SendEvent("MOVE");
        }

        if (Input.GetKeyUp(KeyCode.R))
        {
            GraphItVar.SendEvent("JUMP");
        }

        if (Input.GetKeyUp(KeyCode.T))
        {
            GraphItVar.SendEvent("ATTACK");
        }
    }
}
