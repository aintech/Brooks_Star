using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class Player {
	
	private const int initHealth = 1000, healthPerEndurance = 10;

	public static int health { get; private set; }
	public static int maxHealth { get; private set; }

	public static HandWeapon weapon { get; private set; }

	public static BodyArmor armor { get; private set; }

	public static int damage { get { return weapon == null? 20: UnityEngine.Random.Range(weapon.getMinDamage(), weapon.getMaxDamage()+1); } private set {;} }

	public static void init () {
		health = maxHealth = initHealth;
	}

	public static void equipWeapon (HandWeapon weapon) {
		Player.weapon = weapon;
	}

	public static void equipArmor (BodyArmor armor) {
		Player.armor = armor;
	}

	public static int hitPlayer (int damageAmount)  {
		if (armor != null && damageAmount < armor.getArmorClass()) {
			return 0;
		} else {
			health -= (damageAmount - (armor == null? 0: armor.getArmorClass()));
//			UserInterface.updateHealth();
			return damageAmount - (armor == null? 0: armor.getArmorClass());
		}
	}

	public static int healPlayer (int healAmount) {
		if (healAmount + health > maxHealth) {
			int heal = maxHealth - health;
			setHealthToMax();
//			UserInterface.updateHealth();
			return heal;
		} else {
			health += healAmount;
//			UserInterface.updateHealth();
			return healAmount;
		}
	}

	public static void setHealthToMax () {
		health = maxHealth;
//		UserInterface.updateHealth();
	}
}