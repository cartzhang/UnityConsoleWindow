using UnityEngine;
using System.Collections;

public class Tes : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () 
    {
        if (Input.GetKey(KeyCode.A))
        {
            Debug.Log("this is debug log");
            System.Console.WriteLine("this is system console write line");
        }
	
	}
}
