using UnityEngine;
using System.Collections;

public enum EngineType {
	Force,
	Gradual,
	Proton,
	Allur,
	Quazar
}

public static class EngineDescriptor {
	public static string getName (this EngineType type) {
		switch(type) {
		case EngineType.Force: return "Силовой\nдвигатель";
		case EngineType.Gradual: return "Поступательный\nдвигатель";
		case EngineType.Proton: return "Протонный\nдвигатель";
		case EngineType.Allur: return "Аллюровый\nдвигатель";
		case EngineType.Quazar: return "Квазарный\nдвигатель";
		default: return "Неизвестный тип двигателя";
		}
	}

	public static float getMainPower (this EngineType type) {
		switch(type) {
			case EngineType.Force: return 0.04f;
			case EngineType.Gradual: return 0.044f;
			case EngineType.Proton: return 0.05f;
			case EngineType.Allur: return 0.058f;
			case EngineType.Quazar: return 0.068f;
			default: return 0;
		}
	}

	public static float getRotatePower (this EngineType type) {
		switch(type) {
		case EngineType.Force: return 3;
		case EngineType.Gradual: return 3.5f;
		case EngineType.Proton: return 4;
		case EngineType.Allur: return 4.5f;
		case EngineType.Quazar: return 5;
		default: return 0;
		}
	}

	public static float getVolume (this EngineType type) {
		switch(type) {
		case EngineType.Force: return 2;
		case EngineType.Gradual: return 2;
		case EngineType.Proton: return 2;
		case EngineType.Allur: return 2;
		case EngineType.Quazar: return 2;
		default: return 0;
		}
	}

	public static int getCost (this EngineType type) {
		switch(type) {
		case EngineType.Force: return 100;
		case EngineType.Gradual: return 200;
		case EngineType.Proton: return 300;
		case EngineType.Allur: return 400;
		case EngineType.Quazar: return 500;
		default: return 0;
		}
	}

	public static string getDescription (this EngineType type) {
		switch(type) {
		case EngineType.Force: return "Двигатель на силовой\nтяге";
		case EngineType.Gradual: return "Двигатель на поступа-\nтельной тяге";
		case EngineType.Proton: return "Двигатель на протон-\nной тяге";
		case EngineType.Allur: return "Двигатель на аллюро-\nвой тяге";
		case EngineType.Quazar: return "Двигатель на квазар-\nной тяге";
		default: return "Неизвестный тип двигателя";
		}
	}

	public static int getEnergyNeeded (this EngineType type) {
		switch(type) {
		case EngineType.Force: return 10;
		case EngineType.Gradual: return 20;
		case EngineType.Proton: return 30;
		case EngineType.Allur: return 40;
		case EngineType.Quazar: return 50;
		default: return 0;
		}
	}
}