using UnityEngine;
using System.Collections;

public class FightInterface : MonoBehaviour {

	private Transform healthBar;

	private Enemy enemy;

	private Vector3 scale;

	private float enemyMax;

	public void init () {
		healthBar = transform.Find("EnemyHealthBar").Find("Bar");
		scale = healthBar.localScale;
		gameObject.SetActive(true);
	}

	public void setEnemy (Enemy enemy) {
		this.enemy = enemy;
		enemyMax = enemy.health;
		updateEnemyBar();
	}

	public void updateEnemyBar () {
		scale.y = Mathf.Max(1, enemy.health) / enemyMax;
		healthBar.localScale = scale;
	}
}