using UnityEngine;
using System.Collections;

public class FightInterface : MonoBehaviour {

	private Transform healthBar;

	private Enemy enemy;

	private Vector3 scale;

	private Vector3 armorScaleOne = new Vector3(.13f, .13f, 1), armorScaleDouble = new Vector3(.1f, .1f, 1);

	private float enemyMax;

	private StrokeText enemyArmorValue;

	public void init () {
		healthBar = transform.Find("EnemyHealthBar").Find("Bar");
		enemyArmorValue = transform.Find("Enemy Armor Value").GetComponent<StrokeText>().init("default", 5);
		scale = healthBar.localScale;
		gameObject.SetActive(true);
	}

	public void setEnemy (Enemy enemy) {
		this.enemy = enemy;
		enemyMax = enemy.health;
		updateEnemyBar();
		updateEnemyArmor();
	}

	public void updateEnemyBar () {
		scale.y = Mathf.Max(1, enemy.health) / enemyMax;
		healthBar.localScale = scale;
	}

	public void updateEnemyArmor () {
		enemyArmorValue.setText(enemy.armor.ToString());
		enemyArmorValue.transform.localScale = enemy.armor < 10? armorScaleOne: armorScaleDouble;
	}
}