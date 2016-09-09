using UnityEngine;
using System.Collections;

public enum ShieldType {
	Block,
	Quadratic,
	Cell,
	Phase
}

public static class ShieldDescriptor {
	public static string getName (this ShieldType type) {
		switch(type) {
		case ShieldType.Block: return "Блочный\nщит";
		case ShieldType.Quadratic: return "Квадратичный\nщит";
		case ShieldType.Cell: return "Ячеестый\nщит";
		case ShieldType.Phase: return "Фазовый\nщит";
		default: return "Неизвестный тип щита";
		}
	}
	
	public static int getShieldProtection (this ShieldType type) {
		switch(type) {
		case ShieldType.Block: return 100;
		case ShieldType.Quadratic: return 150;
		case ShieldType.Cell: return 200;
		case ShieldType.Phase: return 300;
		default: return 0;
		}
	}
	
	public static int getRechargeSpeed (this ShieldType type) {
		switch(type) {
		case ShieldType.Block: return 10;
		case ShieldType.Quadratic: return 15;
		case ShieldType.Cell: return 25;
		case ShieldType.Phase: return 40;
		default: return 0;
		}
	}

	public static float getVolume (this ShieldType type) {
		switch(type) {
		case ShieldType.Block: return 1;
		case ShieldType.Quadratic: return 1;
		case ShieldType.Cell: return 1;
		case ShieldType.Phase: return 1;
		default: return 0;
		}
	}

	public static int getCost (this ShieldType type) {
		switch(type) {
			case ShieldType.Block: return 100;
			case ShieldType.Quadratic: return 200;
			case ShieldType.Cell: return 300;
			case ShieldType.Phase: return 400;
			default: return 0;
		}
	}
	
	public static int getEnergyNeeded (this ShieldType type) {
		switch(type) {
		case ShieldType.Block: return 10;
		case ShieldType.Quadratic: return 20;
		case ShieldType.Cell: return 30;
		case ShieldType.Phase: return 40;
		default: return 0;
		}
	}

	public static string getDescription (this ShieldType type) {
		switch(type) {
		case ShieldType.Block: return "Щит блочного типа";
		case ShieldType.Quadratic: return "Щит квадратичного типа";
		case ShieldType.Cell: return "Щит ячеестого типа";
		case ShieldType.Phase: return "Щит фазового типа";
		default: return "Неизвестный тип щита";
		}
	}
}