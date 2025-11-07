/*
  Package Name: Smart Console
  Version: 2.3.7
  Author: EdgarDev
  Unity Asset Profile: https://assetstore.unity.com/publishers/64126
  Date: 2023-12-24
  Script Name: UIBackgroundSetter.cs

  Description:
  This script sets background UI following preferences colors.
*/

using UnityEngine;
using UnityEngine.UI;

namespace ED.SC.Components
{
	public class UIBackgroundSetter : MonoBehaviour
	{
		[SerializeField] private SmartConsolePreferences m_Preferences;
		[SerializeField] CanvasGroup m_CanvasGroup;
		[SerializeField] Image[] m_Backgrounds;

		private void Start()
		{
			SetConsoleOpacity(m_Preferences.Opacity);

			foreach (var background in m_Backgrounds)
			{
				background.color = m_Preferences.BackgroundMainColor;
			}
		}

		[Command("set_console_opacity", "Sets console opacity from 0 to 1")]
		public void SetConsoleOpacity(float value)
		{
			m_CanvasGroup.alpha = value;
		}
	}
}