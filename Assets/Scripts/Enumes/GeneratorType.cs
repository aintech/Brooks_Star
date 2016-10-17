using UnityEngine;
using System.Collections;

public enum GeneratorType {
	ATOMIC,
	PLASMA,
	MULTYPHASE,
	TUNNEL
}

public static class GeneratorDescriptor {
	public static string name (this GeneratorType type) {
		switch(type) {
			case GeneratorType.ATOMIC: return "Атомарный генератор";
			case GeneratorType.PLASMA: return "Плазменный генератор";
			case GeneratorType.MULTYPHASE: return "Мультифазный генератор";
			case GeneratorType.TUNNEL: return "Тунельный генератор";
			default: return "Неизвестный тип генератора";
		}
	}

	public static int maxEnergy (this GeneratorType type) {
		switch(type) {
			case GeneratorType.ATOMIC: return 100;
			case GeneratorType.PLASMA: return 200;
			case GeneratorType.MULTYPHASE: return 300;
			case GeneratorType.TUNNEL: return 400;
			default: return 0;
		}
	}

	public static float volume (this GeneratorType type) {
		switch(type) {
			case GeneratorType.ATOMIC: return 1;
			case GeneratorType.PLASMA: return 1;
			case GeneratorType.MULTYPHASE: return 1;
			case GeneratorType.TUNNEL: return 1;
			default: return 0;
		}
	}

	public static int cost (this GeneratorType type) {
		switch(type) {
			case GeneratorType.ATOMIC: return 100;
			case GeneratorType.PLASMA: return 200;
			case GeneratorType.MULTYPHASE: return 300;
			case GeneratorType.TUNNEL: return 400;
			default: return 0;
		}
	}

	public static string description (this GeneratorType type) {
		switch(type) {
			case GeneratorType.ATOMIC: return "Генератор на атомар-\nном принципе";
			case GeneratorType.PLASMA: return "Генератор на плаз-\nменном принципе";
			case GeneratorType.MULTYPHASE: return "Генератор на мульти-\nфазном принципе";
			case GeneratorType.TUNNEL: return "Генератор на туне-\nльном принципе";
			default: return "Неизвестный тип генератора";
		}
	}
}