/* 
  Package Name: Smart Console
  Version: 2.3.10
  Author: EdgarDev
  Unity Asset Profile: https://assetstore.unity.com/publishers/64126
  Date: 2024-01-12
  Script Name: ScreenCommands.cs

  Description:
  This script implements screen related commands.
*/

using System;
using System.IO;
using UnityEngine;

namespace ED.SC.Extra
{
	[CommandGroup("Screen")]
	public static class ScreenCommands
	{
		[Command("set_fullscreen", "Sets the fullscreen state of the application")]
		public static void SetFullScreen(bool value)
		{
			Screen.fullScreen = value;
		}

		[Command("toggle_fullscreen", "Toggles the fullscreen state of the application")]
		public static void ToggleFullScreen()
		{
			SetFullScreen(!Screen.fullScreen);
		}

		[Command("get_screen_resolution", "Gets the current screen resolution")]
		public static void GetScreenResolution()
		{
			SmartConsole.Log($"Current screen resolution is {Screen.width} x {Screen.height}.");
		}

		[Command("set_screen_resolution", "Sets the current screen resolution in the range of supported resolutions")]
		public static void SetScreenResolution(int width, int height)
		{
			Screen.SetResolution(width, height, Screen.fullScreen);
		}

		[Command("set_screen_main_native_resolution", "Sets the current screen resolution to main screen native resolution")]
		public static void SetScreenMainNativeResolution()
		{
			SetScreenResolution(Display.main.systemWidth, Display.main.systemHeight);
		}

		[Command("capture_screenshot", "Captures a screenshot and saves it to the supplied file path as a PNG.")]
		public static void CaptureScreenshot(int resolutionMultiplier = 1)
		{
			string folderName = "Screenshots";
			string fileName = $"screenshot-0.png";
			string filePath = Path.Combine(Application.persistentDataPath, folderName, fileName);

			int count = 1;

			while (File.Exists(filePath))
			{
				fileName = $"screenshot-{count}.png";
				filePath = Path.Combine(Application.persistentDataPath, folderName, fileName);
				count++;
			}

			// Create the directory if it doesn't exist
			Directory.CreateDirectory(Path.GetDirectoryName(filePath));

			try
			{
				// Take the screenshot and save it to the specified file path
				ScreenCapture.CaptureScreenshot(filePath, resolutionMultiplier);
				SmartConsole.Log($"Screenshot saved to {filePath}.\nPath has been copied to clipboard.");
				GUIUtility.systemCopyBuffer = filePath;
			}
			catch (Exception e)
			{
				SmartConsole.LogWarning(e.Message);
			}
		}
	}
}
