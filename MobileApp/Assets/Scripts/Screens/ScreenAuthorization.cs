using System.Collections;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScreenAuthorization : MonoBehaviour
{
	[Header("Экраны")]
	[SerializeField] private ScreenEnterApp screenEnterApp;
	[SerializeField] private ScreenMainMenu screenMainMenu;

	[Header("Алерты")]
	[SerializeField] private TextMeshProUGUI alertAttention;

	[Header("Поля для заполнния")]
	[SerializeField] private TMP_InputField inputFieldEmail;
	[SerializeField] private TMP_InputField inputFieldPassword;

	[Header("Кнопки")]
	[SerializeField] private Button buttonBack;
	[SerializeField] private Button buttonEnter;
	[SerializeField] private Button buttonEnterCatcher;
	[SerializeField] private bool isDebug = false;

	private Regex regex = new Regex(@"^([a-z0-9_-]+\.)*[a-z0-9_-]+@[a-z0-9_-]+(\.[a-z0-9_-]+)*\.[a-z]{2,6}$");

	private void Awake()
	{
		buttonBack.onClick.AddListener(OnClickButtonBack);
		buttonEnter.onClick.AddListener(OnClickButtonEnter);
		buttonEnterCatcher.onClick.AddListener(OnClickButtonEnterCatcher);

		inputFieldEmail.onValueChanged.AddListener(OnValueChangedFieldEmail);
		inputFieldPassword.onValueChanged.AddListener(OnValueChangedFieldPassword);
	}

	private void OnEnable()
	{
		inputFieldEmail.text = isDebug ? "vashov@gmail.com" : "";
		inputFieldPassword.text = isDebug ? "Qwerty_12345" : "";
		buttonEnter.interactable = isDebug ? true : false;
		buttonEnterCatcher.gameObject.SetActive(isDebug ? false : true);

		HideAttention();
	}

	private void OnValueChangedFieldEmail(string value)
	{
		SetInteractableRegister();
	}

	private void OnValueChangedFieldPassword(string value)
	{
		SetInteractableRegister();
	}

	private bool ParseEmail(string email)
	{
		return regex.IsMatch(email);
	}

	private void SetInteractableRegister()
	{
		bool isSuccess = ParseEmail(inputFieldEmail.text) && inputFieldPassword.text.Length >= 8;
		if(isSuccess)
		{
			HideAttention();
		}

		buttonEnterCatcher.gameObject.SetActive(!isSuccess);
		buttonEnter.interactable = isSuccess;
	}

	private void OnClickButtonBack()
	{
		ShowNextScreen(screenEnterApp.gameObject);
		Hide();
	}

	private void OnClickButtonEnter()
	{
		if(string.IsNullOrEmpty(inputFieldEmail.text) || string.IsNullOrEmpty(inputFieldPassword.text))
		{
			ShowAttention("! Заполнены не все поля !");
			return;
		}

		StartCoroutine(SendRequestLogIn());
	}

	private void OnClickButtonEnterCatcher()
	{
		if (string.IsNullOrEmpty(inputFieldEmail.text))
		{
			ShowAttention("! не указана почта !");
			return;
		}

		if (!ParseEmail(inputFieldEmail.text))
		{
			ShowAttention("! почта неверного формата !");
			return;
		}

		if (string.IsNullOrEmpty(inputFieldPassword.text))
		{
			ShowAttention("! не заполнен пароль !");
			return;
		}
	}

	private IEnumerator SendRequestLogIn()
	{
		AuthorizationRequest authorization = Requests.Instance.authorizationRequest;
		yield return StartCoroutine(authorization.SendRequest(inputFieldEmail.text, inputFieldPassword.text));

		switch (authorization.ResponseCode)
		{
			case Requests.RESPONSE_CODE_SUCCESS:
				GeneralJava.permission.GetPermissionGeometria();

				screenMainMenu.SetInfouser(
					authorization.Result.firstName, 
					authorization.Result.secondName, authorization.Result.patronymic, 
					authorization.Result.professionTitle);

				ShowNextScreen(screenMainMenu.gameObject);
				Hide();
				break;
			case Requests.RESPONSE_CODE_FORBIDEN:
				ShowAttention("! неверный пароль !");
				break;
			case Requests.RESPONSE_CODE_BAD_REQUEST:
				ShowAttention("! пользователя с таким логином нет !");
				break;
			case Requests.RESPONSE_CODE_BAD_GATEAWAY:
				ShowAttention("! сервер неактивен !");
				break;
		}
	}

	private void Hide()
	{
		this.gameObject.SetActive(false);
	}

	private void ShowNextScreen(GameObject nextScreen)
	{
		nextScreen.SetActive(true);
	}

	private void ShowAttention(string attentionText)
	{
		alertAttention.text = attentionText;
		alertAttention.transform.parent.gameObject.SetActive(true);
	}

	private void HideAttention()
	{
		alertAttention.transform.parent.gameObject.SetActive(false);
	}
}
