using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartWorkJava
{
	public void Start(int constructionId)
	{
		if (Application.platform == RuntimePlatform.Android)
		{
			string apiKey = Requests.ApiKey;
			GeneralJava.PluginInstance.Call("setApiKey", apiKey);
			GeneralJava.PluginInstance.Call<int>("setIdConstr", constructionId);

			if (GeneralJava.PluginInstance.Call<int>("startAccelerometerService") == 1) Debug.Log("Start AccelerometerService");
			else Debug.LogError("Not Start AccelerometerService");

			if (GeneralJava.PluginInstance.Call<int>("startGPSService") == 1) Debug.Log("Start GPSService");
			else Debug.LogError("Not Start GPSService");
		}
	}

	public void Stop(int constructionId)
	{
		if (Application.platform == RuntimePlatform.Android)
		{
			string apiKey = Requests.ApiKey;
			GeneralJava.PluginInstance.Call("setApiKey", apiKey);
			GeneralJava.PluginInstance.Call<int>("setIdConstr", constructionId);

			if (GeneralJava.PluginInstance.Call<int>("stopAccelerometerService") == 1) Debug.Log("Stop AccelerometerService");
			else Debug.LogError("Not Stop stopAccelerometerService");

			if (GeneralJava.PluginInstance.Call<int>("stopGPSService") == 1) Debug.Log("Stop GPSService");
			else Debug.LogError("Not Stop GPSService");
		}
	}
}
