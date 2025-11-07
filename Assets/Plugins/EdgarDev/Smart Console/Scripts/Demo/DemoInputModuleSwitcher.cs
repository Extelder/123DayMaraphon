using UnityEngine;

namespace ED.SC.Demo
{
	[ExecuteInEditMode]
	public class DemoInputModuleSwitcher : MonoBehaviour
	{
		private void OnEnable()
		{
			TrySwitchEventSystem();
		}

		private void TrySwitchEventSystem()
		{
#if UNITY_EDITOR && ENABLE_INPUT_SYSTEM && __INPUTSYSTEM__
			var module = GetComponent<UnityEngine.EventSystems.StandaloneInputModule>();

			if (module != null)
			{
				module.gameObject.AddComponent<UnityEngine.InputSystem.UI.InputSystemUIInputModule>();
				DestroyImmediate(module);
			}
#endif
		}
	}
}