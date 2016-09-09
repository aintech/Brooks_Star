using UnityEngine;
using System.Collections;

public enum WeaponType {
	Blaster,
	Plasmer,
	Charger,
	Emitter,
	Waver,
	Launcher,
	Suppressor
}

public static class WeaponDescriptor {
	public static string getName (this WeaponType type) {
		switch(type) {
			case WeaponType.Blaster: return "Бластер";
			case WeaponType.Plasmer: return "Плазмер";
			case WeaponType.Charger: return "Разрядник";
			case WeaponType.Emitter: return "Излучатель";
			case WeaponType.Waver: return "Волноход";
			case WeaponType.Launcher: return "Ракетница";
			case WeaponType.Suppressor: return "Подавитель";
			default: return "Неизвестная категория оружия";
		}
	}

	public static int getDamage (this WeaponType type) {
		switch(type) {
			case WeaponType.Blaster: return 10;
			case WeaponType.Plasmer: return 10;
			case WeaponType.Charger: return 10;
			case WeaponType.Emitter: return 10;
			case WeaponType.Waver: return 10;
			case WeaponType.Launcher: return 10;
			case WeaponType.Suppressor: return 0;
			default: return 0;
		}
	}
	
	public static int getDamageRange (this WeaponType type) {
		switch(type) {
		case WeaponType.Blaster: return 3;
		case WeaponType.Plasmer: return 3;
		case WeaponType.Charger: return 3;
		case WeaponType.Emitter: return 3;
		case WeaponType.Waver: return 3;
		case WeaponType.Launcher: return 3;
		case WeaponType.Suppressor: return 0;
		default: return 0;
		}
	}

	public static float getReloadTime (this WeaponType type) {
		switch(type) {
			case WeaponType.Blaster: return 1;
			case WeaponType.Plasmer: return 2;
			case WeaponType.Charger: return 2;
			case WeaponType.Emitter: return 2;
			case WeaponType.Waver: return 2;
			case WeaponType.Launcher: return 2;
			case WeaponType.Suppressor: return 2;
			default: return 0;
		}
	}
	
	public static int getRange (this WeaponType type) {
		switch(type) {
		case WeaponType.Blaster: return 15;
		case WeaponType.Plasmer: return 15;
		case WeaponType.Charger: return 15;
		case WeaponType.Emitter: return 15;
		case WeaponType.Waver: return 15;
		case WeaponType.Launcher: return 15;
		case WeaponType.Suppressor: return 15;
		default: return 0;
		}
	}

	public static float getVolume (this WeaponType type) {
		switch(type) {
			case WeaponType.Blaster: return 2;
			case WeaponType.Plasmer: return 2;
			case WeaponType.Charger: return 2;
			case WeaponType.Emitter: return 2;
			case WeaponType.Waver: return 2;
			case WeaponType.Launcher: return 2;
			case WeaponType.Suppressor: return 2;
			default: return 0;
		}
	}

	public static int getCost (this WeaponType type) {
		switch(type) {
			case WeaponType.Blaster: return 100;
			case WeaponType.Plasmer: return 200;
			case WeaponType.Charger: return 300;
			case WeaponType.Emitter: return 400;
			case WeaponType.Waver: return 500;
			case WeaponType.Launcher: return 600;
			case WeaponType.Suppressor: return 700;
			default: return 0;
		}
	}

	public static string getDescription (this WeaponType type) {
		switch(type) {
		case WeaponType.Blaster:
			return "Стреляет заряжен-\nными частицами";
		case WeaponType.Plasmer:
			return "Стреляет сгустками\nплазмы";
		case WeaponType.Charger:
			return "Создает электрический 'луч'";
		case WeaponType.Emitter:
			return "Луч антиматерии, пронзающий всё на своём пути";
		case WeaponType.Waver:
			return "Стреляет полукругом энергии с затуханием (навроде дробовика)";
		case WeaponType.Launcher:
			return "Запускает небольшие сгустки энергии взрывающиеся при соприкосновении";
		case WeaponType.Suppressor:
			return "Снижает скорость движения и атаки врага";
		default: return "Неизвестная категория оружия";
		}
	}

	public static string getFullDescription (this WeaponType type) {
		switch(type) {
		case WeaponType.Blaster:
			return "Стреляет заряженными частицами.\n" +
				"Стандартное оружие заменившее огнестрельное, встречается повсеместно и в " +
					"совершенно разных исполнениях, его можно обнаружить и на боевом корабле и в " +
					"сумочке пожилой леди.";
		case WeaponType.Plasmer:
			return "Стреляет сгустками плазмы.";
		case WeaponType.Charger:
			return "Создает электрический 'луч'.";
		case WeaponType.Emitter:
			return "Луч антиматерии, пронзающий всё на своём пути";
		case WeaponType.Waver:
			return "Стреляет полукругом энергии с затуханием (навроде дробовика)";
		case WeaponType.Launcher:
			return "Запускает небольшие сгустки энергии взрывающиеся при соприкосновении";
		case WeaponType.Suppressor:
			return "Снижает скорость движения и атаки врага";
		default: return "Неизвестная категория оружия";
		}
	}

	public static int getEnergyNeeded (this WeaponType type) {
		switch(type) {
		case WeaponType.Blaster: return 10;
		case WeaponType.Plasmer: return 20;
		case WeaponType.Charger: return 30;
		case WeaponType.Emitter: return 40;
		case WeaponType.Waver: return 50;
		case WeaponType.Launcher: return 60;
		case WeaponType.Suppressor: return 70;
		default: return 0;
		}
	}
}