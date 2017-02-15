#define UNITY_EDITOR_WIN
#define UNITY_STANDALONE_WIN
using UnityEngine;
using System.Collections;
using SLQJ;

public class ServerConsole : MonoBehaviour
{
#if UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN

    private ConsoleTestWindows.ConsoleWindow console = new ConsoleTestWindows.ConsoleWindow();
    private ConsoleTestWindows.ConsoleInput input = new ConsoleTestWindows.ConsoleInput();

    private static bool ishowWindow = false;
    private bool oldWindowState = false;
	//
	// Create console window, register callbacks
	//
	void Awake() 
	{
		DontDestroyOnLoad( gameObject );
		Debug.Log( "Console Started" );
	}
 
	//
	// Text has been entered into the console
	// Run it as a console command
	//
	void OnInputText( string obj )
	{
        this.ConsolePrint(obj);
        int subLen = obj.IndexOf(' ');
        if (subLen < 0) return;
        if (obj.Substring(0, subLen) ==  "movespeed")
        {
            string getvaluve = obj.Substring(obj.LastIndexOf(' '));
            float speed = -1;
            float.TryParse(getvaluve, out speed);
            NotificationManager.Instance.Notify("movespeed", speed);
        }
	}
 
	//
	// Debug.Log* callback
	//
	void HandleLog( string message, string stackTrace, LogType type )
	{
        if (type == LogType.Warning)
            System.Console.ForegroundColor = System.ConsoleColor.Yellow;
        else if (type == LogType.Error)
            System.Console.ForegroundColor = System.ConsoleColor.Red;
        else
            System.Console.ForegroundColor = System.ConsoleColor.White;
 
		// We're half way through typing something, so clear this line ..
        if (System.Console.CursorLeft != 0)
			input.ClearLine();
 
		System.Console.WriteLine( message );
 
		// If we were typing something re-add it.
		input.RedrawInputLine();
	}

    //
    // Update the input every frame
    // This gets new key input and calls the OnInputText callback
    //
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab) || Input.GetKeyDown(KeyCode.BackQuote))
        {
            ishowWindow = !ishowWindow;
            if (ishowWindow)
            {
                console = new ConsoleTestWindows.ConsoleWindow();
                input = new ConsoleTestWindows.ConsoleInput();
                console.Initialize();
                console.SetTitle("Test command");
                input.OnInputText += OnInputText;
                Application.logMessageReceived += HandleLog;
            }
            else
            {
                CloseConsoleWindow();
            }
            oldWindowState = ishowWindow;
        }
        // input update
        if (ishowWindow && null != input)
        {
            input.Update();
        }

        if (ishowWindow != oldWindowState && !ishowWindow)
        {
            CloseConsoleWindow();
        }
        oldWindowState = ishowWindow;
    }
 
	//
	// It's important to call console.ShutDown in OnDestroy
	// because compiling will error out in the editor if you don't
	// because we redirected output. This sets it back to normal.
	//
	void OnDestroy()
	{
        CloseConsoleWindow();
    }

    void CloseConsoleWindow()
    {
        if (console != null)
        {
            console.Shutdown();
            console = null;
            input = null;
        }
    }
    // control by other .
    public static void SetIshowWindow(bool flag)
    {   
       ishowWindow = flag;
    }

#endif
}

public static class ExtendDebugClass
{
    public static void ConsolePrint(this MonoBehaviour mono, string message)
    {
        if (message.Length < 0) return;
        System.Console.ForegroundColor = System.ConsoleColor.Magenta;
        System.Console.WriteLine(message);
    }
}
