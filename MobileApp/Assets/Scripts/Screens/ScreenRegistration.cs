using System.Collections;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScreenRegistration : MonoBehaviour
{
	[Header("Экраны")]
	[SerializeField] private ScreenEnterApp screenEnterApp;
	[SerializeField] private ScreenMainMenu screenMainMenu;

	[Header("Алерты")]
	[SerializeField] private TextMeshProUGUI alertAttention;

	[Header("Поля для заполнния")]
	[SerializeField] private TMP_InputField inputFieldEmail;
	[SerializeField] private TMP_InputField inputFieldPassword;
	[SerializeField] private Toggle togglePermission;

	[Header("Кнопки")]
	[SerializeField] private Button buttonBack;
	[SerializeField] private Button buttonRegistration;
	[SerializeField] private Button buttonRegistrationCatcher;

	private Regex regex = new Regex(@"^([a-z0-9_-]+\.)*[a-z0-9_-]+@[a-z0-9_-]+(\.[a-z0-9_-]+)*\.[a-z]{2,6}$");

	private void Awake()
	{
		buttonBack.onClick.AddListener(OnClickButtonBack);
		buttonRegistration.onClick.AddListener(OnClickButtonRegistration);
		buttonRegistrationCatcher.onClick.AddListener(OnClickButtonRegistrationCatcher);

		inputFieldEmail.onValueChanged.AddListener(OnValueChangedFieldEmail);
		inputFieldPassword.onValueChanged.AddListener(OnValueChangedFieldPassword);
		togglePermission.onValueChanged.AddListener(OnValueChangedToggle);
	}


	private void OnEnable()
	{
		togglePermission.isOn = false;
		inputFieldEmail.text = "";
		inputFieldPassword.text = "";
		buttonRegistration.interactable = false;
		buttonRegistrationCatcher.gameObject.SetActive(true);

		HideAttention();
	}

	private void OnValueChangedFieldEmail(string value)
	{
		HideAttention();
		SetInteractableRegister();
	}

	private void OnValueChangedFieldPassword(string value)
	{
		HideAttention();
		SetInteractableRegister();
	}

	private void OnValueChangedToggle(bool value)
	{
		HideAttention();
		SetInteractableRegister();
	}

	private void SetInteractableRegister()
	{
		bool isSucces = togglePermission.isOn && ParseEmail(inputFieldEmail.text) && inputFieldPassword.text.Length >= 8;
		if(isSucces)
		{
			HideAttention();
		}

		buttonRegistrationCatcher.gameObject.SetActive(!isSucces);
		buttonRegistration.interactable = isSucces;
	}

	private bool ParseEmail(string email)
	{
		return regex.IsMatch(email);
	}

	private void OnClickButtonBack()
	{
		ShowNextScreen(screenEnterApp.gameObject);
		Hide();
	}

	private void OnClickButtonRegistration()
	{
		if (string.IsNullOrEmpty(inputFieldEmail.text) || string.IsNullOrEmpty(inputFieldPassword.text) || !togglePermission.isOn)
		{
			ShowAttention("! Заполнены не все поля !");
			return;
		}

		StartCoroutine(SendRequestLogIn());
	}

	private void OnClickButtonRegistrationCatcher()
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

		if (inputFieldPassword.text.Length < 8)
		{
			ShowAttention("! пароль слишком короткий !");
			return;
		}

		if (!togglePermission.isOn)
		{
			ShowAttention("! не подтверждено соглашение !");
			return;
		}
	}

	private IEnumerator SendRequestLogIn()
	{
		yield return null;
		ShowNextScreen(screenMainMenu.gameObject);
		Hide();
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
