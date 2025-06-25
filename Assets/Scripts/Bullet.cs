using Unity.Netcode;
using UnityEngine;

public class Bullet : NetworkBehaviour
{
	public float speed = 10f;
	public int damage = 10;
	public Rigidbody rb;
	private void Start()
	{
		if (!IsServer)
		{
			Destroy(rb);
		}
	}


	private void OnCollisionEnter(Collision collision)
	{

		if (collision.gameObject.TryGetComponent<PlayerController>(out var player))
		{
			player.TakeDamage(damage);
			GetComponent<NetworkObject>().Despawn();
			print("attempting despawning");
		}
	}
}
