using UnityEngine;
using System.Collections;

public enum RepairDroidType {
	RAIL,
	CHANNEL,
	BIPHASIC,
	THREAD
}

public static class RepairDroidDescriptor {
	public static string name (this RepairDroidType type) {
		switch(type) {
			case RepairDroidType.RAIL: return "Рельсовый дроид";
			case RepairDroidType.CHANNEL: return "Канальный дроид";
			case RepairDroidType.BIPHASIC: return "Бифазный дроид";
			case RepairDroidType.THREAD: return "Поточный дроид";
			default: return "Неизвестный тип дроида-ремонтника";
		}
	}
	
	public static int repairSpeed (this RepairDroidType type) {
		switch(type) {
			case RepairDroidType.RAIL: return 5;
			case RepairDroidType.CHANNEL: return 10;
			case RepairDroidType.BIPHASIC: return 17;
			case RepairDroidType.THREAD: return 30;
			default: return 0;
		}
	}

	public static float volume (this RepairDroidType type) {
		switch(type) {
			case RepairDroidType.RAIL: return 1;
			case RepairDroidType.CHANNEL: return 1;
			case RepairDroidType.BIPHASIC: return 1;
			case RepairDroidType.THREAD: return 1;
			default: return 0;
		}
	}

	public static int cost (this RepairDroidType type) {
		switch(type) {
			case RepairDroidType.RAIL: return 100;
			case RepairDroidType.CHANNEL: return 200;
			case RepairDroidType.BIPHASIC: return 300;
			case RepairDroidType.THREAD: return 400;
			default: return 0;
		}
	}
	
	public static int energyNeeded (this RepairDroidType type) {
		switch(type) {
			case RepairDroidType.RAIL: return 10;
			case RepairDroidType.CHANNEL: return 20;
			case RepairDroidType.BIPHASIC: return 30;
			case RepairDroidType.THREAD: return 40;
			default: return 0;
		}
	}

	public static string description (this RepairDroidType type) {
		switch(type) {
			case RepairDroidType.RAIL: return "Ремонтный дроид на\nоснове рельс";
			case RepairDroidType.CHANNEL: return "Ремонтный дроид на\nоснове каналов";
			case RepairDroidType.BIPHASIC: return "Ремонтный дроид на\nоснове бифаз";
			case RepairDroidType.THREAD: return "Ремонтный дроид на\nоснове потоков";
			default: return "Неизвестный тип дроида-ремонтника";
		}
	}
}