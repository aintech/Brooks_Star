using UnityEngine;
using System.Collections;

public enum StatusEffectType {
	BLINDED, PARALIZED, REGENERATION, POISONED, ARMORED, SPEED, HEAL, NONE
}

public static class StatusEffectDescriptor {
	public static string name (this StatusEffectType type) {
		switch (type) {
			case StatusEffectType.BLINDED: return "Ослепление";
			case StatusEffectType.PARALIZED: return "Паралич";
			case StatusEffectType.REGENERATION: return "Регенерация";
			case StatusEffectType.POISONED: return "Отравление";
			case StatusEffectType.ARMORED: return "Защита";
			case StatusEffectType.SPEED: return "Ускорение";
			case StatusEffectType.HEAL: return "Лечение";
			default: Debug.Log("Unknown status effect type: " + type); return "";
		}
	}

	public static bool withoutStatusHolder (this StatusEffectType type) {
		return type == StatusEffectType.HEAL || type == StatusEffectType.NONE;
	}

	public static bool isStatusActiveOnNextTurn (this StatusEffectType type) {
		return type == StatusEffectType.BLINDED || type == StatusEffectType.PARALIZED;
	}
}