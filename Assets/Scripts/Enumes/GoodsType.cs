using UnityEngine;
using System.Collections;

public enum GoodsType {
	JEWELRY, PRECIOUS_METALS, BOOZE, ELECTRONICS, MEAL
}

public static class GoodsDescriptor {
	public static string name (this GoodsType type) {
		switch (type) {
			case GoodsType.JEWELRY: return "Драгоценности";
			case GoodsType.PRECIOUS_METALS: return "Драгметаллы";
			case GoodsType.BOOZE: return "Выпивка";
			case GoodsType.ELECTRONICS: return "Микросхемы";
			case GoodsType.MEAL: return "Пища";
			default: Debug.Log("Unknown goods type: " + type); return "";
		}
	}

	public static string description (this GoodsType type) {
		return "";
	}

	public static float volume (this GoodsType type) {
		return 0;
	}

	public static int cost (this GoodsType type) {
		switch (type) {
			case GoodsType.MEAL: return 10;
			case GoodsType.BOOZE: return 15;
			case GoodsType.PRECIOUS_METALS: return 20;
			case GoodsType.ELECTRONICS: return 30;
			case GoodsType.JEWELRY: return 50;
			default: Debug.Log("Unknown goods type: " + type); return 0;
		}
	}
}