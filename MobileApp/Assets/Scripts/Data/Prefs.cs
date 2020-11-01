using System;
using UnityEngine;

public class Prefs
{
	public static string ApiKey
	{
		get{return PlayerPrefs.GetString("api_key_user", "8e92896f-32aa-4199-ab2b-2678042ea946"); }
		set { PlayerPrefs.SetString("api_key_user", value); }
	}

	public static DateTime DateStartWork
	{
		get { return DateTime.Parse(PlayerPrefs.GetString("date_start_work", DateTime.Now.ToString())); }
		set { PlayerPrefs.SetString("date_start_work", value.ToString()); }
	}

	public static DateTime DateStopWork
	{
		get { return DateTime.Parse(PlayerPrefs.GetString("date_stop_work", DateTime.Now.ToString())); }
		set { PlayerPrefs.SetString("date_stop_work", value.ToString()); }
	}

	public static DateTime DateCreatedUser
	{
		get { return DateTime.Parse(PlayerPrefs.GetString("date_created_user", DateTime.Now.ToString())); }
		set { PlayerPrefs.SetString("date_created_user", value.ToString()); }
	}

	public static float LastTimeWork
	{
		get { return PlayerPrefs.GetFloat("last_time_work", 0f); }
		set { PlayerPrefs.SetFloat("last_time_work", value); }
	}

	public static string NameUser
	{
		get { return PlayerPrefs.GetString("name_user", ""); }
		set { PlayerPrefs.SetString("name_user", value); }
	}

	public static string EmailUser
	{
		get { return PlayerPrefs.GetString("email_user", ""); }
		set { PlayerPrefs.SetString("email_user", value); }
	}

	public static bool IsUserWork
	{
		get { return PlayerPrefs.GetInt("is_user_work", 0) == 1; }
		set { PlayerPrefs.SetInt("is_user_work", value ? 1 : 0); }
	}
}
