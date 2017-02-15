using UnityEngine;
using System.Collections;

public class PrintOnConsole : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            this.ConsolePrint("this is print test,not use Debug.Log()");
        }

    }
}
