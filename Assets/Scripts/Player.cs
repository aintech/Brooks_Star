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

	public static Dictionary<PerkType, float> perks { get; private set; }

	public static void init () {
		if (perks == null || perks.Count == 0) {
			perks = new Dictionary<PerkType, float>();
			foreach (PerkType type in Enum.GetValues(typeof(PerkType))) {
				perks.Add(type, 0);
			}
		}
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

	public static void updatePerk (PerkType type, float value) {
		int prevValue = (int)perks[type];
		perks[type] += (value / Mathf.Max(perks[type], 1f));
//		Messenger.showMessage("Навык " + type.getName() + " = " + (int)((perks[type] - (int)perks[type]) * 100) + "%");
		if ((int)perks[type] > prevValue) {
			Messenger.showMessage("Навык " + type.getName() + " увеличен до " + getPerkLevel(type));
		}
	}

	public static int getPerkLevel (PerkType type) {
		return (int)perks[type];
	}

	public static float getPerkExp (PerkType type) {
		return perks[type] - (int)perks[type];
	}
}