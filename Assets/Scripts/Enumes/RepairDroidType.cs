using UnityEngine;
using System.Collections;

public enum RepairDroidType {
	Rail,
	Channel,
	Biphasic,
	Thread
}

public static class RepairDroidDescriptor {
	public static string getName (this RepairDroidType type) {
		switch(type) {
		case RepairDroidType.Rail: return "Рельсовый\nдроид";
		case RepairDroidType.Channel: return "Канальный\nдроид";
		case RepairDroidType.Biphasic: return "Бифазный\nдроид";
		case RepairDroidType.Thread: return "Поточный\nдроид";
		default: return "Неизвестный тип дроида-ремонтника";
		}
	}
	
	public static int getRepairSpeed (this RepairDroidType type) {
		switch(type) {
		case RepairDroidType.Rail: return 2;
		case RepairDroidType.Channel: return 5;
		case RepairDroidType.Biphasic: return 10;
		case RepairDroidType.Thread: return 25;
		default: return 0;
		}
	}

	public static float getVolume (this RepairDroidType type) {
		switch(type) {
		case RepairDroidType.Rail: return 1;
		case RepairDroidType.Channel: return 1;
		case RepairDroidType.Biphasic: return 1;
		case RepairDroidType.Thread: return 1;
		default: return 0;
		}
	}

	public static int getCost (this RepairDroidType type) {
		switch(type) {
		case RepairDroidType.Rail: return 100;
		case RepairDroidType.Channel: return 200;
		case RepairDroidType.Biphasic: return 300;
		case RepairDroidType.Thread: return 400;
		default: return 0;
		}
	}
	
	public static int getEnergyNeeded (this RepairDroidType type) {
		switch(type) {
		case RepairDroidType.Rail: return 10;
		case RepairDroidType.Channel: return 20;
		case RepairDroidType.Biphasic: return 30;
		case RepairDroidType.Thread: return 40;
		default: return 0;
		}
	}

	public static string getDescription (this RepairDroidType type) {
		switch(type) {
		case RepairDroidType.Rail: return "Ремонтный дроид на\nоснове рельс";
		case RepairDroidType.Channel: return "Ремонтный дроид на\nоснове каналов";
		case RepairDroidType.Biphasic: return "Ремонтный дроид на\nоснове бифаз";
		case RepairDroidType.Thread: return "Ремонтный дроид на\nоснове потоков";
		default: return "Неизвестный тип дроида-ремонтника";
		}
	}
}