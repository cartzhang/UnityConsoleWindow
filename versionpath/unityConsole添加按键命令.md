2017-02-16 
## 修改说明：

本次修改主要是根据之前使用的体验做了修改。

2017年之前版本，只是在游戏开始时候会自动开启控制台窗口，然后在里面可以输入或代码里调用输出。

2017年2月16号现在版本，通过tab按键或～按键来调用和关闭控制台窗口。
当然主要功能还是没有改变，就是可以输入和输出信息。
命令行窗口上添加了两个命令，一个clear的清屏命令，一个是exit的退出命令行窗口命令。

![0](https://github.com/cartzhang/UnityConsoleWindow/blob/master/image/0.png)

![1](https://github.com/cartzhang/UnityConsoleWindow/blob/master/image/1.png)

## 使用方法

使用方法可以参考CommandDemo场景中的Demo样例。

![2](https://github.com/cartzhang/UnityConsoleWindow/blob/master/image/2.png)

在场景中挂载serverConsole.cs代码，在其他地方要使用log输出或打印的时候，调用

    `this.ConsolePrint("this is print test,not use Debug.Log()");`

就可以了。

参考PrintOnConsole.cs中代码：

    `
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
	`

为什么可以这么写呢？很简单，因为把monoBehaviour类添加了扩展方法。

具体代码：
    `
	public static class ExtendDebugClass
	{
	    public static void ConsolePrint(this MonoBehaviour mono, string message)
	    {
	        if (message.Length < 0) return;
	        System.Console.ForegroundColor = System.ConsoleColor.Magenta;
	        System.Console.WriteLine(message);
	    }
	}
	`

暂时就这么多。

## 关于下一步想法

其实，还有，就是想把这个控制台窗口也做了一个类似修改器，可以修改一些代码里或游戏里的参数。显示里面实现了一个简单的
就是在控制台窗口中输入movespeed 10,输入参数就可以调整里面物体小方块的在按下WASD键时候运行速度了。













