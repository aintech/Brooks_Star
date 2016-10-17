using UnityEngine;
using System.Collections;

public enum RadarType {
	SEQUESTER,
	PLANAR,
	MATRIX,
	PATAN_CORSAC,
	SNAKE,
	ASTRAL
}

public static class RadarDescriptor {
	public static string name (this RadarType type) {
		switch(type) {
		case RadarType.SEQUESTER: return "Секвесторный радар";
		case RadarType.PLANAR: return "Планарный радар";
		case RadarType.MATRIX: return "Матричный радар";
		case RadarType.PATAN_CORSAC: return "Радар Патан-Корсака";
		case RadarType.SNAKE: return "Змеевой радар";
		case RadarType.ASTRAL: return "Астральный радар";
		default: return "Неизвестный тип радара";
		}
	}
	
	public static int range (this RadarType type) {
		switch(type) {
		case RadarType.SEQUESTER: return 5;
		case RadarType.PLANAR: return 8;
		case RadarType.MATRIX: return 12;
		case RadarType.PATAN_CORSAC: return 20;
		case RadarType.SNAKE: return 30;
		case RadarType.ASTRAL: return 50;
		default: return 0;
		}
	}

	public static float volume (this RadarType type) {
		switch(type) {
			case RadarType.SEQUESTER: return .5f;
			case RadarType.PLANAR: return .5f;
			case RadarType.MATRIX: return .5f;
			case RadarType.PATAN_CORSAC: return .5f;
			case RadarType.SNAKE: return .5f;
			case RadarType.ASTRAL: return .5f;
			default: return 0;
		}
	}

	public static int cost (this RadarType type) {
		switch(type) {
		case RadarType.SEQUESTER: return 100;
		case RadarType.PLANAR: return 200;
		case RadarType.MATRIX: return 300;
		case RadarType.PATAN_CORSAC: return 400;
		case RadarType.SNAKE: return 500;
		case RadarType.ASTRAL: return 600;
		default: return 0;
		}
	}
	
	public static int energyNeeded (this RadarType type) {
		switch(type) {
		case RadarType.SEQUESTER: return 10;
		case RadarType.PLANAR: return 20;
		case RadarType.MATRIX: return 30;
		case RadarType.PATAN_CORSAC: return 40;
		case RadarType.SNAKE: return 50;
		case RadarType.ASTRAL: return 60;
		default: return 0;
		}
	}

	public static string description (this RadarType type) {
		switch(type) {
		case RadarType.SEQUESTER: return "Радар секвесторного\nтипа";
		case RadarType.PLANAR: return "Радар планарного\nтипа";
		case RadarType.MATRIX: return "Радар матричного\nтипа";
		case RadarType.PATAN_CORSAC: return "Радар типа\nПатан-Корсака";
		case RadarType.SNAKE: return "Радар змеевого\nтипа";
		case RadarType.ASTRAL: return "Радар астрального\nтипа";
		default: return "Неизвестный тип радара";
		}
	}
}