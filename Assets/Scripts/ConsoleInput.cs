using UnityEngine;
using System;
using System.Collections;
using System.Runtime.InteropServices;
using System.IO;

namespace ConsoleTestWindows
{
	public class ConsoleInput
	{
		//public delegate void InputText( string strInput );
		public event System.Action<string> OnInputText;
		public string inputString;

		public void ClearLine()
		{
            //System.Text.Encoding test = Console.InputEncoding;
            Console.CursorLeft = 0;
			Console.Write( new String( ' ', Console.BufferWidth ) );
			Console.CursorTop--;
			Console.CursorLeft = 0;
		}

		public void RedrawInputLine()
		{
			if ( Console.CursorLeft > 0 )
				ClearLine();
			if ( inputString.Length == 0 ) return;

			System.Console.ForegroundColor = ConsoleColor.Green;
            try
            {
                System.Console.Write(inputString);
            }
            catch (System.Exception ex)
            {
                //System.Console.WriteLine("Error: " + ex.Message);
            }
		}

		internal void OnBackspace()
		{
            int inputLength = inputString.Length;
            if (inputLength  <= 0 ) return;
            if (inputLength == 1)
            {
                inputString = "";
            }
            else
            {
                inputString = inputString.Substring(0, inputLength - 1);
            }
			RedrawInputLine();
		}

		internal void OnEscape()
		{
			ClearLine();
			inputString = "";
		}

		internal void OnEnter()
		{
			ClearLine();
            DefaultCommand(inputString);
            System.Console.ForegroundColor = ConsoleColor.Green;
            try
            {
                System.Console.WriteLine("> " + inputString);
            }
            catch (System.Exception ex)
            {
                //Console.WriteLine(ex.Message);
            }			

			var strtext = inputString;
			inputString = "";

			if ( OnInputText != null )
			{
				OnInputText( strtext );
			}
		}

		public void Update()
		{
			if ( !Console.KeyAvailable ) return;
			var key = Console.ReadKey();

			if ( key.Key == ConsoleKey.Enter )
			{
				OnEnter();
				return;
			}

			if ( key.Key == ConsoleKey.Backspace )
			{
				OnBackspace();
				return;
			}

			if ( key.Key == ConsoleKey.Escape )
			{
				OnEscape();
				return;
			}

			if ( key.KeyChar != '\u0000' )
			{
				inputString += key.KeyChar;
				RedrawInputLine();
				return;
			}
		}

        /// <summary>
        /// add command line 
        /// </summary>
        /// <param name="inputStr"></param>
        internal void DefaultCommand(string inputStr)
        {
            if (inputStr.Length == 0) return;

            switch (inputStr.ToLower())
            {
                case "clear":
                    System.Console.Clear();
                    break;
                case "exit":
                    ServerConsole.SetIshowWindow(false);
                    break;
                default:
                    break;
            }

        }
	}
}