﻿using UnityEngine;
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
		case RadarType.Sequester: return "Секвесторный радар";
		case RadarType.Planar: return "Планарный радар";
		case RadarType.Matrix: return "Матричный радар";
		case RadarType.PatanCorsac: return "Радар Патан-Корсака";
		case RadarType.Snake: return "Змеевой радар";
		case RadarType.Astral: return "Астральный радар";
		default: return "Неизвестный тип радара";
		}
	}
	
	public static int getRange (this RadarType type) {
		switch(type) {
		case RadarType.Sequester: return 5;
		case RadarType.Planar: return 8;
		case RadarType.Matrix: return 12;
		case RadarType.PatanCorsac: return 20;
		case RadarType.Snake: return 30;
		case RadarType.Astral: return 50;
		default: return 0;
		}
	}

	public static float getVolume (this RadarType type) {
		switch(type) {
			case RadarType.Sequester: return .5f;
			case RadarType.Planar: return .5f;
			case RadarType.Matrix: return .5f;
			case RadarType.PatanCorsac: return .5f;
			case RadarType.Snake: return .5f;
			case RadarType.Astral: return .5f;
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