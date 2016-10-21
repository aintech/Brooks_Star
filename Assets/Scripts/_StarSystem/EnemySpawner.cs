using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour {

	public Transform enemyShipPrefab;

	private Transform playerShip;

	private Minimap minimap;

	public EnemySpawner init (Minimap minimap, Transform playerShip) {
		this.minimap = minimap;
		this.playerShip = playerShip;
		return this;
	}

	public void spawnAnEnemy (int minLevel, int maxLevel, int quantity) {
		bool found = false;
		EnemyShip enemy = null;
//		float bound = Vars.planetType.getDistanceToStar();
		for (int i = 0; i < quantity; i++) {
			found = false;
			enemy = null;
			foreach (EnemyShip ship in Vars.enemyShipsPool) {
				if (ship.destroed) {
					enemy = ship;
					found = true;
					break;
				}
			}
			if (!found) {
				enemy = Instantiate<Transform>(enemyShipPrefab).GetComponent<EnemyShip>();
				Vars.enemyShipsPool.Add(enemy);
			}
			enemy.initRandomShip(Random.Range(minLevel, maxLevel + 1), playerShip.transform);
			enemy.transform.position = playerShip.transform.position + new Vector3(3, 3, 0); //new Vector3(Random.Range(-bound, bound), Random.Range(-bound, bound));
			minimap.addEnemy(enemy.transform);
		}
	}
}