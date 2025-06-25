using Unity.Netcode;
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


	//only executed for host
	void Shoot()
	{
		GameObject closest = PlayerManager.GetClosestPlayer(transform.position);
		if(closest == null) return;
		Bullet bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);//in future I may add bulletPool but not now
																						   //На що НЕ варто витрачати час:
																						   //Оптимізація під велику кількість об'єктів.
		bullet.NetworkObject.Spawn();
		var v = closest.transform.position - transform.position;
		v.y = 0;
		v = v.normalized * bulletSpeed;
		bullet.rb.AddForce(v, ForceMode.VelocityChange);
	}
}
	