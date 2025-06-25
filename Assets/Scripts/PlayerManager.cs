using System.Collections.Generic;
using UnityEngine;

public static class PlayerManager
{
	private static List<GameObject> _players = new List<GameObject>();

	public static void RegisterPlayer(GameObject player)
	{
		if (!_players.Contains(player))
		{
			_players.Add(player);
		}
	}

	public static void UnregisterPlayer(GameObject player)
	{
		if (_players.Contains(player))
		{
			_players.Remove(player);
		}
	}

	public static List<GameObject> GetPlayers()
	{
		return new List<GameObject>(_players); 
	}

	public static GameObject GetClosestPlayer(Vector3 position)
	{
		if (_players.Count == 0) return null;

		GameObject closest = _players[0];
		float minDistance = Vector3.Distance(position, closest.transform.position);

		foreach (var player in _players)
		{
			if (player == null || !player.activeSelf) continue;

			float distance = Vector3.Distance(position, player.transform.position);
			if (distance < minDistance)
			{
				closest = player;
				minDistance = distance;
			}
		}

		return closest;
	}
}