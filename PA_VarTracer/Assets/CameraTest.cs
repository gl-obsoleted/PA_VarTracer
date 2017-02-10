using UnityEngine;
using System.Collections;

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
            VarTracerTool.SendEvent("MOVE");
        }

    }
}
