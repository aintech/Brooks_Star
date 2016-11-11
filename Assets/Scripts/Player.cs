using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class Player {
	
	private const int initHealth = 200;//, healthPerEndurance = 10;

	private const int noWeaponDamage = 10;

	public static int health { get; private set; }
	public static int maxHealth { get; private set; }

	public static HandWeaponData weapon { get; private set; }

	public static BodyArmorData armor { get; private set; }

	public static int minDamage { get; private set; }

	public static int maxDamage { get; private set; }

	public static int randomDamage { get { return weapon == null? noWeaponDamage: UnityEngine.Random.Range(minDamage, maxDamage+1); } private set {;} }

	public static Dictionary<PerkType, float> perks { get; private set; }

	public static FightInterface fightInterface;

	public static void init () {
		if (perks == null || perks.Count == 0) {
			perks = new Dictionary<PerkType, float>();
			foreach (PerkType type in Enum.GetValues(typeof(PerkType))) {
				perks.Add(type, 0);
			}
		}
		updateMinMaxDamage();
		health = maxHealth = initHealth;
	}

	public static void equipWeapon (HandWeaponData weapon) {
		Player.weapon = weapon;
		updateMinMaxDamage();
	}

	public static void updateMinMaxDamage () {
		minDamage = weapon == null? noWeaponDamage: weapon.minDamage + Mathf.RoundToInt(weapon.minDamage / 100f * getPerkLevel(PerkType.MARKSMAN) * PerkType.MARKSMAN.getValuePerLevel());
		maxDamage = weapon == null? noWeaponDamage: weapon.maxDamage + Mathf.RoundToInt(weapon.maxDamage / 100f * getPerkLevel(PerkType.MARKSMAN) * PerkType.MARKSMAN.getValuePerLevel());
	}

	public static void equipArmor (BodyArmorData armor) {
		Player.armor = armor;
	}

	public static int hitPlayer (int damageAmount)  {
		if (armor != null && damageAmount < armor.armorClass) {
			return 0;
		} else {
			health -= (damageAmount - (armor == null? 0: armor.armorClass));
			if (fightInterface.gameObject.activeInHierarchy) { fightInterface.updatePlayerBar(); }
			return damageAmount - (armor == null? 0: armor.armorClass);
		}
	}

	public static int heal (int amount) {
		if (amount + health > maxHealth) {
			int heal = maxHealth - health;
			setHealthToMax();
			if (fightInterface.gameObject.activeInHierarchy) { fightInterface.updatePlayerBar(); }
			return heal;
		} else {
			health += amount;
			if (fightInterface.gameObject.activeInHierarchy) { fightInterface.updatePlayerBar(); }
			return amount;
		}
	}

	public static void setHealthToMax () {
		health = maxHealth;
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