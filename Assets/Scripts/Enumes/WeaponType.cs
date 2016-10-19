using UnityEngine;
using System.Collections;

public enum WeaponType {
	BLASTER,
	PLASMER,
	CHARGER,
	EMITTER,
	WAVER,
	LAUNCHER,
	SUPPRESSOR
}

public static class WeaponDescriptor {
	public static string name (this WeaponType type) {
		switch(type) {
			case WeaponType.BLASTER: return "Бластер";
			case WeaponType.PLASMER: return "Плазмер";
			case WeaponType.CHARGER: return "Разрядник";
			case WeaponType.EMITTER: return "Излучатель";
			case WeaponType.WAVER: return "Волноход";
			case WeaponType.LAUNCHER: return "Ракетница";
			case WeaponType.SUPPRESSOR: return "Подавитель";
			default: return "Неизвестная категория оружия";
		}
	}

	public static int damage (this WeaponType type) {
		switch(type) {
			case WeaponType.BLASTER: return 50;
			case WeaponType.PLASMER: return 10;
			case WeaponType.CHARGER: return 10;
			case WeaponType.EMITTER: return 10;
			case WeaponType.WAVER: return 10;
			case WeaponType.LAUNCHER: return 10;
			case WeaponType.SUPPRESSOR: return 0;
			default: return 0;
		}
	}
	
	public static int damageRange (this WeaponType type) {
		switch(type) {
			case WeaponType.BLASTER: return 20;
			case WeaponType.PLASMER: return 3;
			case WeaponType.CHARGER: return 3;
			case WeaponType.EMITTER: return 3;
			case WeaponType.WAVER: return 3;
			case WeaponType.LAUNCHER: return 3;
			case WeaponType.SUPPRESSOR: return 0;
			default: return 0;
		}
	}

	public static float reloadTime (this WeaponType type) {
		switch(type) {
			case WeaponType.BLASTER: return 1;
			case WeaponType.PLASMER: return 2;
			case WeaponType.CHARGER: return 2;
			case WeaponType.EMITTER: return 2;
			case WeaponType.WAVER: return 2;
			case WeaponType.LAUNCHER: return 2;
			case WeaponType.SUPPRESSOR: return 2;
			default: return 0;
		}
	}
	
	public static int range (this WeaponType type) {
		switch(type) {
			case WeaponType.BLASTER: return 15;
			case WeaponType.PLASMER: return 15;
			case WeaponType.CHARGER: return 15;
			case WeaponType.EMITTER: return 15;
			case WeaponType.WAVER: return 15;
			case WeaponType.LAUNCHER: return 15;
			case WeaponType.SUPPRESSOR: return 15;
			default: return 0;
		}
	}

	public static float volume (this WeaponType type) {
		switch(type) {
			case WeaponType.BLASTER: return 2;
			case WeaponType.PLASMER: return 2;
			case WeaponType.CHARGER: return 2;
			case WeaponType.EMITTER: return 2;
			case WeaponType.WAVER: return 2;
			case WeaponType.LAUNCHER: return 2;
			case WeaponType.SUPPRESSOR: return 2;
			default: return 0;
		}
	}

	public static int cost (this WeaponType type) {
		switch(type) {
			case WeaponType.BLASTER: return 100;
			case WeaponType.PLASMER: return 200;
			case WeaponType.CHARGER: return 300;
			case WeaponType.EMITTER: return 400;
			case WeaponType.WAVER: return 500;
			case WeaponType.LAUNCHER: return 600;
			case WeaponType.SUPPRESSOR: return 700;
			default: return 0;
		}
	}

	public static string description (this WeaponType type) {
		switch(type) {
		case WeaponType.BLASTER:
			return "Стреляет заряжен-\nными частицами";
		case WeaponType.PLASMER:
			return "Стреляет сгустками\nплазмы";
		case WeaponType.CHARGER:
			return "Создает электрический 'луч'";
		case WeaponType.EMITTER:
			return "Луч антиматерии, пронзающий всё на своём пути";
		case WeaponType.WAVER:
			return "Стреляет полукругом энергии с затуханием (навроде дробовика)";
		case WeaponType.LAUNCHER:
			return "Запускает небольшие сгустки энергии взрывающиеся при соприкосновении";
		case WeaponType.SUPPRESSOR:
			return "Снижает скорость движения и атаки врага";
		default: return "Неизвестная категория оружия";
		}
	}

	public static string fullDescription (this WeaponType type) {
		switch(type) {
		case WeaponType.BLASTER:
			return "Стреляет заряженными частицами.\n" +
				"Стандартное оружие заменившее огнестрельное, встречается повсеместно и в " +
					"совершенно разных исполнениях, его можно обнаружить и на боевом корабле и в " +
					"сумочке пожилой леди.";
		case WeaponType.PLASMER:
			return "Стреляет сгустками плазмы.";
		case WeaponType.CHARGER:
			return "Создает электрический 'луч'.";
		case WeaponType.EMITTER:
			return "Луч антиматерии, пронзающий всё на своём пути";
		case WeaponType.WAVER:
			return "Стреляет полукругом энергии с затуханием (навроде дробовика)";
		case WeaponType.LAUNCHER:
			return "Запускает небольшие сгустки энергии взрывающиеся при соприкосновении";
		case WeaponType.SUPPRESSOR:
			return "Снижает скорость движения и атаки врага";
		default: return "Неизвестная категория оружия";
		}
	}

	public static int energyNeeded (this WeaponType type) {
		switch(type) {
		case WeaponType.BLASTER: return 10;
		case WeaponType.PLASMER: return 20;
		case WeaponType.CHARGER: return 30;
		case WeaponType.EMITTER: return 40;
		case WeaponType.WAVER: return 50;
		case WeaponType.LAUNCHER: return 60;
		case WeaponType.SUPPRESSOR: return 70;
		default: return 0;
		}
	}
}