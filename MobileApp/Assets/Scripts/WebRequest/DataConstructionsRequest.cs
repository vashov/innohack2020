using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class DataConstructionsRequest : MonoBehaviour
{
	public DataConstructionRequestGet[] ResultArray { get; private set; }
	public string ResultLog { get; private set; }
	public int ResponseCode { get; private set; }

	public IEnumerator SendRequest()
	{
		string url = Requests.Instance.urlRequestConfig.UrlDataObject;

		UnityWebRequest uwr = new UnityWebRequest(url, "GET");
		uwr.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
		uwr.SetRequestHeader("Content-Type", "application/json");
		uwr.SetRequestHeader("api-key", Requests.ApiKey);
		yield return uwr.SendWebRequest();

		ResponseCode = (int)uwr.responseCode;
		if (uwr.isNetworkError)
		{
			Debug.LogError(uwr.error);
			ResultLog = uwr.error;
		}
		else if (ResponseCode == Requests.RESPONSE_CODE_SUCCESS)
		{
			Debug.Log(uwr.downloadHandler.text);
			ResultLog = uwr.downloadHandler.text;
			string addedHelpJson = "{\"Items\":" + uwr.downloadHandler.text + "}";
			ResultArray = JsonHelper.FromJson<DataConstructionRequestGet>(addedHelpJson);
		}
	}

	[Serializable]
	public struct DataConstructionRequestGet
	{
		public int id;
		public string name;
	}
}
