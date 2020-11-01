using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class AuthorizationRequest : MonoBehaviour
{
	public AuthorizationRequestGet Result { get; private set; }
	public string ResultLog { get; private set; }
	public int ResponseCode { get; private set; }

	public IEnumerator SendRequest(string email, string password)
	{
		AuthorizationRequestSend sos = new AuthorizationRequestSend(email, password);
		string jsonString = JsonUtility.ToJson(sos);

		string url = Requests.Instance.urlRequestConfig.UrlLogIn;

		UnityWebRequest uwr = new UnityWebRequest(url, "POST");
		byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(jsonString);
		uwr.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);
		uwr.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
		uwr.SetRequestHeader("Content-Type", "application/json");

		Debug.Log(jsonString);
		yield return uwr.SendWebRequest();

		ResponseCode = (int)uwr.responseCode;
		if (uwr.isNetworkError)
		{
			Debug.LogError(uwr.error);
			ResultLog = uwr.error;
		}
		else if(ResponseCode == Requests.RESPONSE_CODE_SUCCESS)
		{
			Debug.Log(uwr.downloadHandler.text);
			ResultLog = uwr.downloadHandler.text;
			Result = JsonUtility.FromJson<AuthorizationRequestGet>(uwr.downloadHandler.text);

			Prefs.EmailUser = Result.email;
			Requests.ApiKey = Result.apiKey;
			Prefs.DateCreatedUser = DateTime.Parse(Result.created);
		}
	}

	[Serializable]
	private struct AuthorizationRequestSend
	{
		public string email;
		public string password;
		public AuthorizationRequestSend(string email, string password)
		{
			this.email = email;
			this.password = password;
		}
	}

	[Serializable]
	public struct AuthorizationRequestGet
	{
		public string firstName;
		public string secondName;
		public string patronymic;
		public string professionTitle;
		public string email;
		public string created;
		public string apiKey;
	}
}
