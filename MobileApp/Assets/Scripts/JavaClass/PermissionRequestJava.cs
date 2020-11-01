using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PermissionRequestJava
{
    public void GetPermissionGeometria()
    {
		if (Application.platform == RuntimePlatform.Android)
		{
			if (GeneralJava.PluginInstance.Call<bool>("getPermissionGeometria")) Debug.Log("Success getPermissionGeometria");
			else Debug.LogError("Failed getPermissionGeometria");
		}
	}
}
