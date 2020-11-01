using UnityEngine;

public class Requests : MonoBehaviour
{
	public const int RESPONSE_CODE_SUCCESS = 200;
	public const int RESPONSE_CODE_BAD_REQUEST = 400;
	public const int RESPONSE_CODE_FORBIDEN = 403;
	public const int RESPONSE_CODE_BAD_GATEAWAY = 502;
	public static string ApiKey { get; set; }

	public static Requests Instance { get; private set; }
	public UrlRequestConfig urlRequestConfig { get; private set; }
	public SosRequest sosRequest { get; private set; }
	public AuthorizationRequest authorizationRequest { get; private set; }
	public RegistrationRequest registrationRequest { get; private set; }
	public WorkRequest workRequest { get; private set; }
	public DataConstructionsRequest dataConstructionsRequest { get; private set; }

	private void Awake()
	{
		if(Instance == null)
		{
			Instance = this;
		}
		else
		{
			Destroy(this);
		}

		urlRequestConfig = Resources.Load<UrlRequestConfig>("UrlRequestConfig");
		sosRequest = GetComponent<SosRequest>();
		authorizationRequest = GetComponent<AuthorizationRequest>();
		registrationRequest = GetComponent<RegistrationRequest>();
		workRequest = GetComponent<WorkRequest>();
		dataConstructionsRequest = GetComponent<DataConstructionsRequest>();
	}
}
