using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : NetworkBehaviour
{
	public float moveSpeed = 5f;
	private Camera playerCam;
	public Slider healthSlider;
	NetworkVariable<int> healthVar = new NetworkVariable<int>(100, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);
	public int health = 100;

	private void Start()
	{
		if (IsOwner)
		{
			playerCam = Camera.main;
			playerCam.transform.SetParent(transform);
			playerCam.transform.localPosition = new Vector3(0, 10, -10);
			playerCam.transform.localEulerAngles = new Vector3(45, 0, 0);
			healthSlider = GameObject.Find("PlayerHealth").GetComponent<Slider>();
		}
	}

	private void Update()
	{
		if (!IsOwner) return;

		float h = Input.GetAxis("Horizontal");
		float v = Input.GetAxis("Vertical");
		
		MoveServerRpc(h, v);

		if (!IsServer)
		{
			if(healthSlider.value != healthVar.Value)
			{
				TakeDamage(healthVar.Value - (int)healthSlider.value);
			}
		}
	}

	public void TakeDamage(int damage)
	{
		if (IsServer)
		{
			healthVar.Value -= damage;
			if(!IsOwner && healthVar.Value <= 0)
			{
				GetComponent<Collider>().enabled = false;
				GetComponent<MeshRenderer>().enabled = false;
				

			}
		}
		if (IsOwner)
		{
			healthSlider.value = healthVar.Value;
			if(healthSlider.value <= 0)
			{
				GetComponent<Collider>().enabled = false;
				GetComponent<MeshRenderer>().enabled = false;
				var rb = GetComponent<Rigidbody>();
				rb.isKinematic = false;
				rb.velocity = Vector3.zero;
			}
		}
	}
	[ServerRpc]
	void MoveServerRpc(float h, float v)
	{

		transform.Translate(new Vector3(h, 0, v) * moveSpeed * Time.deltaTime);
	}
}
