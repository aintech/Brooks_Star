using UnityEngine;
using System.Collections;

public enum CharacterType {
	ALIKA, ROKOT
}

public static class CharacterDescriptor {
	public static string getName (this CharacterType type) {
		switch (type) {
			case CharacterType.ALIKA: return "Алика";
			case CharacterType.ROKOT: return "Рокот";
			default: Debug.Log("Unknown charakter type"); return "";
		}
	}
}