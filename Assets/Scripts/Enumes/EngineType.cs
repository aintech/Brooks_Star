using UnityEngine;
using System.Collections;

public enum EngineType {
	FORCE,
	GRADUAL,
	PROTON,
	ALLUR,
	QUAZAR
}

public static class EngineDescriptor {
	public static string getName (this EngineType type) {
		switch(type) {
		case EngineType.FORCE: return "Силовой двигатель";
		case EngineType.GRADUAL: return "Поступательный двигатель";
		case EngineType.PROTON: return "Протонный двигатель";
		case EngineType.ALLUR: return "Аллюровый двигатель";
		case EngineType.QUAZAR: return "Квазарный двигатель";
		default: return "Неизвестный тип двигателя";
		}
	}

	public static float getMainPower (this EngineType type) {
		switch(type) {
			case EngineType.FORCE: return 0.04f;
			case EngineType.GRADUAL: return 0.044f;
			case EngineType.PROTON: return 0.05f;
			case EngineType.ALLUR: return 0.058f;
			case EngineType.QUAZAR: return 0.068f;
			default: return 0;
		}
	}

	public static float getRotatePower (this EngineType type) {
		switch(type) {
		case EngineType.FORCE: return 3;
		case EngineType.GRADUAL: return 3.5f;
		case EngineType.PROTON: return 4;
		case EngineType.ALLUR: return 4.5f;
		case EngineType.QUAZAR: return 5;
		default: return 0;
		}
	}

	public static float getVolume (this EngineType type) {
		switch(type) {
		case EngineType.FORCE: return 2;
		case EngineType.GRADUAL: return 2;
		case EngineType.PROTON: return 2;
		case EngineType.ALLUR: return 2;
		case EngineType.QUAZAR: return 2;
		default: return 0;
		}
	}

	public static int getCost (this EngineType type) {
		switch(type) {
		case EngineType.FORCE: return 100;
		case EngineType.GRADUAL: return 200;
		case EngineType.PROTON: return 300;
		case EngineType.ALLUR: return 400;
		case EngineType.QUAZAR: return 500;
		default: return 0;
		}
	}

	public static string getDescription (this EngineType type) {
		switch(type) {
		case EngineType.FORCE: return "Двигатель на силовой\nтяге";
		case EngineType.GRADUAL: return "Двигатель на поступа-\nтельной тяге";
		case EngineType.PROTON: return "Двигатель на протон-\nной тяге";
		case EngineType.ALLUR: return "Двигатель на аллюро-\nвой тяге";
		case EngineType.QUAZAR: return "Двигатель на квазар-\nной тяге";
		default: return "Неизвестный тип двигателя";
		}
	}

	public static int getEnergyNeeded (this EngineType type) {
		switch(type) {
		case EngineType.FORCE: return 10;
		case EngineType.GRADUAL: return 20;
		case EngineType.PROTON: return 30;
		case EngineType.ALLUR: return 40;
		case EngineType.QUAZAR: return 50;
		default: return 0;
		}
	}
}