using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class NetworkManagerUI : MonoBehaviour
{
	[SerializeField] private Button hostButton;
	[SerializeField] private Button clientButton;
	[SerializeField] private Button shutdownButton;

	private void Awake()
	{
		hostButton.onClick.AddListener(() =>
		{
			NetworkManager.Singleton.StartHost();
		});

		clientButton.onClick.AddListener(() =>
		{
			NetworkManager.Singleton.StartClient();
		});

		shutdownButton.onClick.AddListener(() =>
		{
			NetworkManager.Singleton.Shutdown();
			Application.Quit();
		});
	}
}
