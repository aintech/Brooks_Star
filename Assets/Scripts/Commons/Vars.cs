using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public static class Vars {

	private static bool initialized;

	public static bool EROTIC = false;

	public static bool inSpace;

//	Colonized
	public static StarSystemType starSystemType = StarSystemType.ALURIA;
	public static PlanetType planetType = PlanetType.PARPARIS;

//	Populated
//	public static StarSystemType starSystemType = StarSystemType.CRITA;
//	public static PlanetType planetType = PlanetType.PALETTE;

	public static HullType shipHullType = HullType.CORVETTE;

	public static Dictionary<KeyValuePair<Slot.Type, int>, ItemData> shipHullSlotsMap = new Dictionary<KeyValuePair<HullSlot.Type, int>, ItemData> ();

	public static int shipCurrentHealth = -1;

	public static int cash = 90000;

	public static Dictionary<int, ItemData> inventory = new Dictionary<int, ItemData>();

	public static Dictionary<PlanetType, Dictionary<int, ItemData>> markets = new Dictionary<PlanetType, Dictionary<int, ItemData>>();

	public static Dictionary<int, EnemyType> capturedEnemies = new Dictionary<int, EnemyType>();

	public static Dictionary<Slot.Type, ItemData> equipmentMap = new Dictionary<Slot.Type, ItemData>();

	public static Dictionary<int, ItemData> supplyMap = new Dictionary<int, ItemData>();

	public static int freeSortingOrder = 0;

	public static List<EnemyShip> enemyShipsPool = new List<EnemyShip>();

	public static UserInterface userInterface;

	public static List<Bounty> bounties = new List<Bounty> ();

	public static Story.Chapter chapter = Story.Chapter.NONE;

//	public static List<PlanetType> exploredPlanets = new List<PlanetType>();
//
//	public static Dictionary<PlanetType, int> planetProbes = new Dictionary<PlanetType, int>();
//
//	public static Dictionary<PlanetType, float> planetExploredPercent = new Dictionary<PlanetType, float>();
//
//	public static int probesCount = 100;

	public static void initVars () {
		if (initialized) { return; }
		foreach (PlanetType planet in Enum.GetValues(typeof(PlanetType))) {
			if (planet.isColonized()) {
				markets.Add(planet, new Dictionary<int, ItemData>());
			}
		}
		initialized = true;
	}
}

/* TODO:
	- добавить ещё один слой звёзд starField
	- переделать зум в космосе, чтобы колёсиком мыши не увеличивалось и не уменьшалось звёздное поле, а только корабли и планеты
	- настроить нормальный расчёт цен для предметов
	- добавить объём для типа GOODS
	- планета (или астероид) сначала исследуется (используя навык "Геолог"), когда выясняется какие минералы есть - на неё засылается харвестер, который после сбора возвращается на указанную планету
	- В инвенторе три кнопки - отображать только товары, снаряжение или оборудование
	- В магазине кораблей - либо сделать, чтобы корпуса не повторялись, либо сделать характеристики корпуса рандомными
	- Если сажаться на планету без щитов - корабль получает повреждения
	- Для ремонта корабля дроидом необходима такая штука как scrapMetal - выбивается из врагов и покупается в магазинах
	- На планете есть медцентры в которых можно баффить персонажа
	- На исследование планеты тратяться зонды и исследование планеты занимает время, чем больше зондов, тем быстрее исследуется планета
	или это миниигра, если проиграл - зонд утерян, выиграл - планета исследована, на необитаемых планетах можно оставлять харвестры для сбора ресурсов
	- магазин корпусов - широкий ангар в котором стоят корабли, он прокручивается по горизонтали как бекграунд поанеты и при наведении мыши на корабль появляет информация о его корпусе
*/