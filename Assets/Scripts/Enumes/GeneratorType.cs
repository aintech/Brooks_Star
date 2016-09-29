using UnityEngine;
using System.Collections;

public enum GeneratorType {
	Atomic,
	Plasma,
	Multyphase,
	Tunnel
}

public static class GeneratorDescriptor {
	public static string getName (this GeneratorType type) {
		switch(type) {
		case GeneratorType.Atomic: return "Атомарный генератор";
		case GeneratorType.Plasma: return "Плазменный генератор";
		case GeneratorType.Multyphase: return "Мультифазный генератор";
		case GeneratorType.Tunnel: return "Тунельный генератор";
		default: return "Неизвестный тип генератора";
		}
	}

	public static int getMaxEnergy (this GeneratorType type) {
		switch(type) {
		case GeneratorType.Atomic: return 100;
		case GeneratorType.Plasma: return 200;
		case GeneratorType.Multyphase: return 300;
		case GeneratorType.Tunnel: return 400;
		default: return 0;
		}
	}

	public static float getVolume (this GeneratorType type) {
		switch(type) {
		case GeneratorType.Atomic: return 1;
		case GeneratorType.Plasma: return 1;
		case GeneratorType.Multyphase: return 1;
		case GeneratorType.Tunnel: return 1;
		default: return 0;
		}
	}

	public static int getCost (this GeneratorType type) {
		switch(type) {
		case GeneratorType.Atomic: return 100;
		case GeneratorType.Plasma: return 200;
		case GeneratorType.Multyphase: return 300;
		case GeneratorType.Tunnel: return 400;
		default: return 0;
		}
	}

	public static string getDescription (this GeneratorType type) {
		switch(type) {
		case GeneratorType.Atomic: return "Генератор на атомар-\nном принципе";
		case GeneratorType.Plasma: return "Генератор на плаз-\nменном принципе";
		case GeneratorType.Multyphase: return "Генератор на мульти-\nфазном принципе";
		case GeneratorType.Tunnel: return "Генератор на туне-\nльном принципе";
		default: return "Неизвестный тип генератора";
		}
	}
}