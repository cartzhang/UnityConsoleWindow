using UnityEngine;
using System.Collections;
using SLQJ;

public class main : MonoBehaviour {

	// Use this for initialization
	void Awake ()
    {
        NotificationManager.Instance.Init();
    }
	
	// Update is called once per frame
	void Update ()
    {
        NotificationManager.Instance.Update();
    }
}
