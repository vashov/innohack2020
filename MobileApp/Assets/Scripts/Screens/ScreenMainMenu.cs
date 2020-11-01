using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static DataConstructionsRequest;

public class ScreenMainMenu : MonoBehaviour
{
	[Header("Экраны")]
	[SerializeField] private ScreenEnterApp screenEnterApp;

	[Header("Алерты")]
	[SerializeField] private TextMeshProUGUI alertAttention;

	[Header("Поля для заполнния")]
	[SerializeField] private TMP_Dropdown dropdownConstructions;

	[Header("Кнопки")]
	[SerializeField] private Button buttonBack;
	[SerializeField] private Button buttonSos;
	[SerializeField] private Button buttonStartWork;
	[SerializeField] private Button buttonStopWork;

	[Header("Инфо блок")]
	[SerializeField] private TextMeshProUGUI textNameUser;
	[SerializeField] private TextMeshProUGUI textCareerUser;

	[Header("Подсветка алертов")]
	[SerializeField] private Color colorAlertSuccess;
	[SerializeField] private Color colorTextAlertSuccess;
	[SerializeField] private Color colorAlertFailed;
	[SerializeField] private Color colorTextAlertFailed;

	private Coroutine alertCorutine;
	private Dictionary<string, int> constructions = new Dictionary<string, int>();


	private void Awake()
	{
		buttonBack.onClick.AddListener(OnClickButtonBack);
		buttonSos.onClick.AddListener(OnClickButtonSos);
		buttonStartWork.onClick.AddListener(OnClickButtonStartWork);
		buttonStopWork.onClick.AddListener(OnClickButtonStopWork);
	}

	private void OnEnable()
	{
		HideAttention();
		SetButtonWork(Prefs.IsUserWork);
		StartCoroutine(SendRequestDataConstructions());
	}

	public void SetInfouser(string userFirstName, string userSecondName, string userThirdName, string professionTitle)
	{
		textNameUser.text = userFirstName + " " + userSecondName + " " + userThirdName;
		textCareerUser.text = professionTitle;
	}

	private IEnumerator SendRequestDataConstructions()
	{
		DataConstructionsRequest dataConstructions = Requests.Instance.dataConstructionsRequest;
		yield return StartCoroutine(dataConstructions.SendRequest());

		switch (dataConstructions.ResponseCode)
		{
			case Requests.RESPONSE_CODE_SUCCESS:
				FillDropdownConstructions(dataConstructions.ResultArray);
				Debug.Log("База загружена");
				break;
			case Requests.RESPONSE_CODE_BAD_REQUEST:
				ShowAttention("! ошибка 400 !");
				Debug.LogError("ошибка 400");
				break;
			case Requests.RESPONSE_CODE_BAD_GATEAWAY:
				ShowAttention("! сервер неактивен !");
				Debug.LogError("сервер неактивен");
				break;
		}
	}

	private void FillDropdownConstructions(DataConstructionRequestGet[] arrayConstructions)
	{
		constructions = new Dictionary<string, int>();
		for (int i = 0; i < arrayConstructions.Length; i++)
		{
			for (int j = i + 1; j < arrayConstructions.Length - 1; j++)
			{
				if(arrayConstructions[i].id < arrayConstructions[j + 1].id)
				{
					DataConstructionRequestGet tmp = arrayConstructions[i];
					arrayConstructions[i] = arrayConstructions[j + 1];
					arrayConstructions[j + 1] = tmp;
				}
			}
		}

		List<TMP_Dropdown.OptionData> listOptions = new List<TMP_Dropdown.OptionData>();
		for (int i = 0; i < arrayConstructions.Length; i++)
		{
			TMP_Dropdown.OptionData newItem = new TMP_Dropdown.OptionData();
			newItem.text = arrayConstructions[i].name;
			listOptions.Add(newItem);
			constructions.Add(arrayConstructions[i].name, arrayConstructions[i].id);
		}

		dropdownConstructions.ClearOptions();
		dropdownConstructions.AddOptions(listOptions);
	}

	private void OnClickButtonBack()
	{
		ShowNextScreen(screenEnterApp.gameObject);
		Hide();
	}

	private void OnClickButtonSos()
	{
		StartCoroutine(SendRequestSOS());
	}

	private IEnumerator SendRequestSOS()
	{
		SosRequest sos = Requests.Instance.sosRequest;
		if (!constructions.ContainsKey(dropdownConstructions.captionText.text))
		{
			ShowAttention("! объект не найден !");
			yield break;
		}

		yield return StartCoroutine(sos.SendRequest(constructions[dropdownConstructions.captionText.text]));

		switch (sos.ResponseCode)
		{
			case Requests.RESPONSE_CODE_SUCCESS:
				ShowAttention("! сигнал отправлен диспетчеру !", true);
				Debug.Log("сигнал отправлен диспетчеру");
				break;
			case Requests.RESPONSE_CODE_BAD_GATEAWAY:
				ShowAttention("! сервер неактивен !");
				Debug.LogError("сервер неактивен");
				break;
		}
	}

	private void OnClickButtonStartWork()
	{
		StartCoroutine(SendRequestStartWork());
	}

	private IEnumerator SendRequestStartWork()
	{
		WorkRequest work = Requests.Instance.workRequest;
		if (!constructions.ContainsKey(dropdownConstructions.captionText.text))
		{
			ShowAttention("! объект не найден !");
			yield break;
		}
		int constructionId = constructions[dropdownConstructions.captionText.text];

		yield return StartCoroutine(work.SendRequest(WorkRequest.StatusWork.START_WORK, constructionId));

		switch (work.ResponseCode)
		{
			case Requests.RESPONSE_CODE_SUCCESS:
				StartWork(constructionId);
				ShowAttention("! смена началась !", true);
				Debug.Log("смена началась");
				break;
			case Requests.RESPONSE_CODE_BAD_REQUEST:
				StartWork(constructionId);
				ShowAttention("! вы уже работаете !");
				break;
			case Requests.RESPONSE_CODE_BAD_GATEAWAY:
				ShowAttention("! сервер неактивен !");
				break;
		}
	}

	private void StartWork(int constructionId)
	{
		GeneralJava.work.Start(constructionId);
		dropdownConstructions.interactable = false;
		Prefs.IsUserWork = true;
		Prefs.DateStartWork = DateTime.Now;
		SetButtonWork(true);
	}

	private void OnClickButtonStopWork()
	{
		StartCoroutine(SendRequestStopWork());
	}

	private IEnumerator SendRequestStopWork()
	{
		WorkRequest work = Requests.Instance.workRequest;

		if (!constructions.ContainsKey(dropdownConstructions.captionText.text))
		{
			ShowAttention("! объект не найден !");
			yield break;
		}
		int constructionId = constructions[dropdownConstructions.captionText.text];

		yield return StartCoroutine(work.SendRequest(WorkRequest.StatusWork.STOP_WORK, constructionId));

		switch (work.ResponseCode)
		{
			case Requests.RESPONSE_CODE_SUCCESS:
				StopWork(constructionId);
				ShowAttention("! смена закончилась !", true);
				break;
			case Requests.RESPONSE_CODE_BAD_REQUEST:
				StopWork(constructionId);
				ShowAttention("! вы уже закончили работать !");
				break;
			case Requests.RESPONSE_CODE_BAD_GATEAWAY:
				ShowAttention("! сервер неактивен !");
				break;
		}
	}

	private void StopWork(int constructionId)
	{
		GeneralJava.work.Stop(constructionId);
		dropdownConstructions.interactable = true;
		Prefs.IsUserWork = false;
		Prefs.DateStopWork = DateTime.Now;
		Prefs.LastTimeWork = (float)(Prefs.DateStopWork - Prefs.DateStartWork).TotalHours;
		SetButtonWork(false);
	}

	private void SetButtonWork(bool isStart)
	{
		buttonStartWork.gameObject.SetActive(!isStart);
		buttonStopWork.gameObject.SetActive(isStart);
		buttonBack.gameObject.SetActive(!isStart);
		dropdownConstructions.interactable = !isStart;
	}

	private void Hide()
	{
		this.gameObject.SetActive(false);
	}

	private void ShowNextScreen(GameObject nextScreen)
	{
		nextScreen.SetActive(true);
	}

	private void ShowAttention(string attentionText, bool isSuccess = false)
	{
		if(alertCorutine != null) StopCoroutine(alertCorutine);

		alertAttention.transform.parent.GetComponent<Image>().color = isSuccess ? colorAlertSuccess : colorAlertFailed;
		alertAttention.color = isSuccess ? colorTextAlertSuccess : colorTextAlertFailed;
		alertAttention.text = attentionText;
		alertAttention.transform.parent.gameObject.SetActive(true);
		alertCorutine = StartCoroutine(LateHideAttention());
	}

	private IEnumerator LateHideAttention()
	{
		yield return new WaitForSeconds(5f);
		HideAttention();
	}

	private void HideAttention()
	{
		alertAttention.transform.parent.gameObject.SetActive(false);
	}
}
