using UnityEngine;
using System.Collections;

public enum ShieldType {
	BLOCK,
	QUADRATIC,
	CELL,
	PHASE
}

public static class ShieldDescriptor {
	public static string name (this ShieldType type) {
		switch(type) {
		case ShieldType.BLOCK: return "Блочный щит";
		case ShieldType.QUADRATIC: return "Квадратичный щит";
		case ShieldType.CELL: return "Ячеестый щит";
		case ShieldType.PHASE: return "Фазовый щит";
		default: return "Неизвестный тип щита";
		}
	}
	
	public static int shieldProtection (this ShieldType type) {
		switch(type) {
		case ShieldType.BLOCK: return 100;
		case ShieldType.QUADRATIC: return 140;
		case ShieldType.CELL: return 200;
		case ShieldType.PHASE: return 300;
		default: return 0;
		}
	}
	
	public static int rechargeSpeed (this ShieldType type) {
		switch(type) {
			case ShieldType.BLOCK: return 1;
			case ShieldType.QUADRATIC: return 2;
			case ShieldType.CELL: return 3;
			case ShieldType.PHASE: return 4;
			default: return 0;
		}
	}

	public static float volume (this ShieldType type) {
		switch(type) {
		case ShieldType.BLOCK: return 1;
		case ShieldType.QUADRATIC: return 1;
		case ShieldType.CELL: return 1;
		case ShieldType.PHASE: return 1;
		default: return 0;
		}
	}

	public static int cost (this ShieldType type) {
		switch(type) {
			case ShieldType.BLOCK: return 100;
			case ShieldType.QUADRATIC: return 200;
			case ShieldType.CELL: return 300;
			case ShieldType.PHASE: return 400;
			default: return 0;
		}
	}
	
	public static int energyNeeded (this ShieldType type) {
		switch(type) {
		case ShieldType.BLOCK: return 10;
		case ShieldType.QUADRATIC: return 20;
		case ShieldType.CELL: return 30;
		case ShieldType.PHASE: return 40;
		default: return 0;
		}
	}

	public static string description (this ShieldType type) {
		switch(type) {
		case ShieldType.BLOCK: return "Щит блочного типа";
		case ShieldType.QUADRATIC: return "Щит квадратичного типа";
		case ShieldType.CELL: return "Щит ячеестого типа";
		case ShieldType.PHASE: return "Щит фазового типа";
		default: return "Неизвестный тип щита";
		}
	}
}