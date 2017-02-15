using UnityEngine;
using System.Collections;
using SLQJ;

public partial class ControlMove : MonoBehaviour
{
    public float movespeed = 10f;

    private Transform transf;
    private float horizontalDirection;
    private float verticalDirection;

	// Use this for initialization
	void Start ()
    {
        transf = this.transform;
        string messName = GETNAME(new { this.movespeed });
        NotificationManager.Instance.Subscribe(messName, changeVarible);
    }
	
	// Update is called once per frame
	void Update ()
    {
        horizontalDirection = 0;
        verticalDirection = 0;
        if (Input.GetKey(KeyCode.A))
        {
            horizontalDirection = -1;
        }

        if (Input.GetKey(KeyCode.D))
        {
            horizontalDirection = 1;
        }

        if (Input.GetKey(KeyCode.S))
        {
            verticalDirection = -1;
        }

        if (Input.GetKey(KeyCode.W))
        {
            verticalDirection = 1;
        }

        transf.position += transf.right * Time.deltaTime * movespeed * horizontalDirection;
        transf.position += transf.up * Time.deltaTime * movespeed * verticalDirection;
    }
}

public partial class ControlMove
{
    void changeVarible(MessageObject ojb)
    {
        float speed = (float)ojb.MsgValue;
        movespeed = speed;
        Debug.Log("current cube move speed " + movespeed);
    }

    public static string GETNAME<T>(T myInput) where T : class
    {
        if (myInput == null)
            return string.Empty;

        return typeof(T).GetProperties()[0].Name;
    }
}