using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneralJava : MonoBehaviour
{
	const string pluginName = "com.example.unity.LDTPlugin";
	private static AndroidJavaClass _pluginClass;
	private static AndroidJavaObject _pluginInstance;
	public static StartWorkJava work { get; private set; }
	public static PermissionRequestJava permission { get; private set; }

	private void Start()
	{
		work = new StartWorkJava();
		permission = new PermissionRequestJava();
	}

	public static AndroidJavaClass PluginClass
	{
		get
		{
			if (_pluginClass == null)
			{
				_pluginClass = new AndroidJavaClass(pluginName);
				AndroidJavaClass playerClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
				AndroidJavaObject activity = playerClass.GetStatic<AndroidJavaObject>("currentActivity");
				_pluginClass.SetStatic<AndroidJavaObject>("mainActivity", activity);
			}
			return _pluginClass;
		}
	}
	public static AndroidJavaObject PluginInstance
	{
		get
		{
			if (_pluginInstance == null)
			{
				_pluginInstance = PluginClass.CallStatic<AndroidJavaObject>("getInstance");

			}
			return _pluginInstance;

		}
	}
}
