using UnityEngine;
using System.Collections;

public class Tes : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () 
    {
        if (Input.GetKeyDown(KeyCode.Comma))
        {
            Debug.Log("this is debug log");
            this.ConsolePrint("this is system console write line");
        }
	
	}
}
