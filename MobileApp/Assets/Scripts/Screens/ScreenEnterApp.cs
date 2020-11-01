using UnityEngine;
using UnityEngine.UI;

public class ScreenEnterApp : MonoBehaviour
{
	[Header("Экраны")]
	[SerializeField] private ScreenRegistration screenRegistration;
	[SerializeField] private ScreenAuthorization screenAuthorization;
	[SerializeField] private ScreenMainMenu screenMainMenu;

	[Header("Кнопки")]
	[SerializeField] private Button buttonRegistration;
	[SerializeField] private Button buttonAuthorization;

	private void Awake()
	{
		buttonRegistration.onClick.AddListener(OnClickButtonRegistration);
		buttonAuthorization.onClick.AddListener(OnClickButtonAuthorization);

		if (!string.IsNullOrEmpty(Requests.ApiKey))
		{
			ShowNextScreen(screenMainMenu.gameObject);
			Hide();
		}
	}

	private void OnClickButtonRegistration()
	{
		Application.OpenURL(Requests.Instance.urlRequestConfig.UrlRegistration);
	}

	private void OnClickButtonAuthorization()
	{
		ShowNextScreen(screenAuthorization.gameObject);
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
}
