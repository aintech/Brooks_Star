using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Enemy : MonoBehaviour {

	private SpriteRenderer render;

	public int health { get; private set; }

	public int maxHealth { get; private set; }

	public int armor { get; private set; }

	public int damage { get; private set; }

	public int dexterity { get; private set; }

	public EnemyType enemyType { get; private set; }

	private Vector3 initPos = new Vector3(0, 0, 0);

	public void init () {
//		trans = transform;
//		newPos = trans.localPosition;
//		initX = newPos.x;
//		maxX = initX + .16f;
		render = GetComponent<SpriteRenderer>();
	}
	
//	void Update () {
//		if (animInAction) {
//			if (endTime > Time.time) {
//				if (newPos.x < maxX) {
//					newPos.x += .01f;
//				}
//			} else {
//				newPos.x = initX;
//				animInAction = false;
//				setSprite();
//			}
//			trans.localPosition = newPos;
//		}
//	}

	public void initEnemy (EnemyType enemyType) {
		this.enemyType = enemyType;
		damage = enemyType.getDamage();
		health = enemyType.getHealth();
		maxHealth = health;
		armor = enemyType.getArmor();
		dexterity = enemyType.getDexterity();
		setSprite();
		gameObject.SetActive(true);
	}

	private void setSprite () {
		render.sprite = Imager.getEnemy(enemyType, (float) health / (float)maxHealth);
	}

//	public Sprite getRandomWinSprite () {
//		return QuestParser.allImages.Length == 0? null: QuestParser.allImages[Random.Range(0, QuestParser.allImages.Length)];
//	}

//	public int getRankPoints () {
//		return enemyType.getRankPoints() * 10;
//	}

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
			return damageAmount - armor;
		}
	}

	public void destroyEnemy () {
		gameObject.SetActive(false);
	}
}