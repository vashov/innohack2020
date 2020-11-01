using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "UrlRequestConfig", menuName = "UrlRequestConfig")]
public class UrlRequestConfig : ScriptableObject
{
	[SerializeField] private string urlSOS;
	[SerializeField] private string urlLogIn;
	[SerializeField] private string urlGPS;
	[SerializeField] private string urlWork;
	[SerializeField] private string urlDataObject;
	[SerializeField] private string urlRegistration;

	[Header("URL для пинга")]
	[SerializeField] private string urlTest;

	public string UrlSOS => urlSOS;
	public string UrlLogIn => urlLogIn;
	public string UrlGPS => urlGPS;
	public string UrlWork => urlWork;
	public string UrlDataObject => urlDataObject;
	public string UrlRegistration => urlRegistration;
	public string UrlTest => urlTest;
}
