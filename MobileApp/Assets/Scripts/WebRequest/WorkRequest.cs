using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class WorkRequest : MonoBehaviour
{
	public enum StatusWork
	{
		START_WORK = 1,
		STOP_WORK = 2
	}

	public string ResultLog { get; private set; }
	public int ResponseCode { get; private set; }

	public IEnumerator SendRequest(StatusWork status, int constructionId)
	{
		WorkRequestSend sos = new WorkRequestSend(DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss"), status, constructionId);
		string jsonString = JsonUtility.ToJson(sos);

		string url = Requests.Instance.urlRequestConfig.UrlWork;

		UnityWebRequest uwr = new UnityWebRequest(url, "POST");
		byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(jsonString);
		uwr.uploadHandler = new UploadHandlerRaw(jsonToSend);
		uwr.downloadHandler = new DownloadHandlerBuffer();
		uwr.SetRequestHeader("Content-Type", "application/json");
		uwr.SetRequestHeader("api-key", Requests.ApiKey);

		Debug.Log(jsonString);
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
		}
	}

	[Serializable]
	private struct WorkRequestSend
	{
		public string date;
		public StatusWork type;
		public int constructionId;
		public WorkRequestSend(string date, StatusWork type, int constructionId)
		{
			this.date = date;
			this.type = type;
			this.constructionId = constructionId;
		}
	}
}
