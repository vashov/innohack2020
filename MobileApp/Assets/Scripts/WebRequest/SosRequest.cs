using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class SosRequest : MonoBehaviour
{
	private enum TypeSendSOS
	{
		HAND = 1,
		AUTHO = 2
	}

	public int ResponseCode { get; private set; }

	private float latitude = 0f;
	private float longitude = 0f;
	public IEnumerator SendRequest(int idConstruction)
	{
		yield return StartCoroutine(GetGeopostion());
		SOSRequestSend sos = new SOSRequestSend(TypeSendSOS.HAND, DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss"), idConstruction, latitude, longitude);
		string jsonString = "[" + JsonUtility.ToJson(sos) + "]";

		string url = Requests.Instance.urlRequestConfig.UrlSOS;

		UnityWebRequest uwr = new UnityWebRequest(url, "POST");
		byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(jsonString);
		uwr.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);
		uwr.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
		uwr.SetRequestHeader("Content-Type", "application/json");
		uwr.SetRequestHeader("api-key", Requests.ApiKey);

		Debug.Log(jsonString);
		yield return uwr.SendWebRequest();

		ResponseCode = (int)uwr.responseCode;
		if (uwr.isNetworkError)
		{
			Debug.LogError(uwr.error);
		}
		else
		{
			Debug.Log(uwr.downloadHandler.text);
		}
	}

	public IEnumerator GetGeopostion()
	{
		if(!Input.location.isEnabledByUser)
		{
			Debug.LogError("пользователь не включил GPS");
		}

		Input.location.Start();
		int maxWait = 10;
		while(Input.location.status == LocationServiceStatus.Initializing && maxWait > 0)
		{
			yield return new WaitForSeconds(1);
			maxWait -= 1;
		}

		if(maxWait <= 0)
		{
			Debug.LogError("время истекло");
			yield break;
		}

		if(Input.location.status == LocationServiceStatus.Failed)
		{
			Debug.LogError("Расположение не распознано");
			yield break;
		}

		latitude = Input.location.lastData.latitude;
		longitude = Input.location.lastData.longitude;
		yield break;
	}


	[Serializable]
	private struct SOSRequestSend
	{
		public TypeSendSOS type;
		public string date;
		public int constructionId;
		public float latitude;
		public float longitude;
		public SOSRequestSend(TypeSendSOS type, string date, int constructionId, float latitude, float longitude)
		{
			this.type = type;
			this.date = date;
			this.constructionId = constructionId;
			this.latitude = latitude;
			this.longitude = longitude;
		}
	}
}
