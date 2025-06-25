using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyAI : NetworkBehaviour
{
	public Bullet bulletPrefab;
	public float shootInterval = 2f;
	public float bulletSpeed = 10f;
	private void Start()
	{
		if (IsServer)
			InvokeRepeating(nameof(Shoot), 1f, shootInterval);
	}

	void Shoot()
	{
		GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
		if (players.Length == 0) return;

		GameObject closest = players[0];
		float minDist = float.MaxValue;

		foreach (var p in players)
		{
			float dist = Vector3.Distance(transform.position, p.transform.position);
			if (dist < minDist)
			{
				closest = p;
				minDist = dist;
			}
		}

		Bullet bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
		bullet.NetworkObject.Spawn();
		bullet.transform.position = transform.position;
		var v = closest.transform.position - transform.position;
		v.y = 0;
		v = v.normalized * bulletSpeed;
		bullet.rb.AddForce(v, ForceMode.VelocityChange);
	}
}
	