using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Enemy : MonoBehaviour {

	private const float damageToPerkMultiplier = .01f;

	private SpriteRenderer render;

	public int health { get; private set; }

	public int maxHealth { get; private set; }

	public int armor { get; private set; }

	public int damage { get; private set; }

	public int dexterity { get; private set; }

	public EnemyType enemyType { get; private set; }

	public void init () {
		render = GetComponent<SpriteRenderer>();
	}

	public void initEnemy (EnemyType enemyType) {
		this.enemyType = enemyType;
		damage = enemyType.damage();
		health = enemyType.health();
		maxHealth = health;
		armor = enemyType.armor();
		dexterity = enemyType.dexterity();
		setSprite();
		gameObject.SetActive(true);
	}

	private void setSprite () {
		render.sprite = Imager.getEnemy(enemyType, (float) health / (float)maxHealth);
	}

	public void playHit () {
//		animInAction = true;
//		endTime = Time.time + duration;
//		setSprite();
	}

	public int hitEnemy (int damageAmount, int iconsCount)  {
		if (damageAmount < armor) {
			return 0;
		} else {
			health -= (damageAmount - armor);
			setSprite();
			Player.updatePerk(PerkType.MARKSMAN, damageAmount * damageToPerkMultiplier);
			return damageAmount - armor;
		}
	}

	public void destroyEnemy () {
		gameObject.SetActive(false);
	}
}