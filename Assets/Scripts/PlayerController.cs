using System;
using System.Collections;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : NetworkBehaviour
{
	public float moveSpeed = 5f;
	private Camera playerCam;
	public Slider healthSlider;
	NetworkVariable<int> healthVar = new NetworkVariable<int>(100, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);
	public Rigidbody rb;
	private void Start()
	{
		if (IsOwner)
		{
			playerCam = Camera.main;
			playerCam.transform.SetParent(transform);
			playerCam.transform.localPosition = new Vector3(0, 10, -10);
			playerCam.transform.localEulerAngles = new Vector3(45, 0, 0);
			healthSlider = GameObject.Find("PlayerHealth").GetComponent<Slider>();
			healthVar.OnValueChanged += OnHealthChanged;
		}
	}


	//only executed for owner
	private void OnHealthChanged(int previousValue, int newValue)
	{
		healthSlider.value = newValue;
		if (newValue <= 0)
		{
			DieAsOwner();

		}
		
	}


	private void Update()
	{
		if (!IsOwner) return;

		float h = Input.GetAxis("Horizontal");
		float v = Input.GetAxis("Vertical");

		MoveServerRpc(h, v);
	}


	//only host is gonna receive this call for every player  so guests will never get this
	public void TakeDamage(int damage)
	{
		if (IsServer)
		{
			print(healthVar.Value);
			healthVar.Value -= damage;
			if(healthVar.Value <= 0 && !IsOwner)
			{
				gameObject.SetActive(false);
				MonoBehaviour camMono = Camera.main.GetComponent<MonoBehaviour>();
				//Use it to start your coroutine function
				camMono.StartCoroutine(DespawnLater());
			}
		}

	}

	IEnumerator DespawnLater()
	{
		yield return new WaitForSeconds(2f);
		NetworkObject.Despawn();
	}

	[ServerRpc]
	void MoveServerRpc(float h, float v)
	{
		rb.velocity = moveSpeed * new Vector3(h, 0, v);
		//transform.Translate(moveSpeed * Time.deltaTime * new Vector3(h, 0, v));
	}

	private void DieAsOwner()
	{
		playerCam.transform.SetParent(null);
		gameObject.SetActive(false);

	}
}
