using UnityEngine;
using System.Collections;

public enum RadarType {
	Sequester,
	Planar,
	Matrix,
	PatanCorsac,
	Snake,
	Astral
}

public static class RadarDescriptor {
	public static string getName (this RadarType type) {
		switch(type) {
		case RadarType.Sequester: return "Секвесторный\nрадар";
		case RadarType.Planar: return "Планарный\nрадар";
		case RadarType.Matrix: return "Матричный\nрадар";
		case RadarType.PatanCorsac: return "Радар\nПатан-Корсака";
		case RadarType.Snake: return "Змеевой\nрадар";
		case RadarType.Astral: return "Астральный\nрадар";
		default: return "Неизвестный тип радара";
		}
	}
	
	public static int getRange (this RadarType type) {
		switch(type) {
		case RadarType.Sequester: return 100;
		case RadarType.Planar: return 150;
		case RadarType.Matrix: return 200;
		case RadarType.PatanCorsac: return 300;
		case RadarType.Snake: return 400;
		case RadarType.Astral: return 600;
		default: return 0;
		}
	}

	public static float getVolume (this RadarType type) {
		switch(type) {
		case RadarType.Sequester: return 1;
		case RadarType.Planar: return 1;
		case RadarType.Matrix: return 1;
		case RadarType.PatanCorsac: return 1;
		case RadarType.Snake: return 1;
		case RadarType.Astral: return 1;
		default: return 0;
		}
	}

	public static int getCost (this RadarType type) {
		switch(type) {
		case RadarType.Sequester: return 100;
		case RadarType.Planar: return 200;
		case RadarType.Matrix: return 300;
		case RadarType.PatanCorsac: return 400;
		case RadarType.Snake: return 500;
		case RadarType.Astral: return 600;
		default: return 0;
		}
	}
	
	public static int getEnergyNeeded (this RadarType type) {
		switch(type) {
		case RadarType.Sequester: return 10;
		case RadarType.Planar: return 20;
		case RadarType.Matrix: return 30;
		case RadarType.PatanCorsac: return 40;
		case RadarType.Snake: return 50;
		case RadarType.Astral: return 60;
		default: return 0;
		}
	}

	public static string getDescription (this RadarType type) {
		switch(type) {
		case RadarType.Sequester: return "Радар секвесторного\nтипа";
		case RadarType.Planar: return "Радар планарного\nтипа";
		case RadarType.Matrix: return "Радар матричного\nтипа";
		case RadarType.PatanCorsac: return "Радар типа\nПатан-Корсака";
		case RadarType.Snake: return "Радар змеевого\nтипа";
		case RadarType.Astral: return "Радар астрального\nтипа";
		default: return "Неизвестный тип радара";
		}
	}
}