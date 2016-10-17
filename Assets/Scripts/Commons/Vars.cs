using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class Vars {

	public static bool EROTIC = false;

	public static StarSystemType starSystemType = StarSystemType.ALURIA;

	public static PlanetType planetType = PlanetType.PARPARIS;

	public static HullType shipHullType = HullType.Corvette;

	public static Dictionary<KeyValuePair<Slot.Type, int>, ItemData> shipHullSlotsMap = new Dictionary<KeyValuePair<HullSlot.Type, int>, ItemData> ();

	public static int shipCurrentHealth = -1;

	public static int cash = 90000;

	public static Dictionary<int, ItemData> inventory = new Dictionary<int, ItemData>(),
											market_parparis = new Dictionary<int, ItemData> (),
											market_terana = new Dictionary<int, ItemData>();

	public static Dictionary<Slot.Type, ItemData> equipmentMap = new Dictionary<Slot.Type, ItemData>();

	public static int freeSortingOrder = 0;

	public static List<EnemyShip> enemyShipsPool = new List<EnemyShip>();

	public static UserInterface userInterface;

//	public static List<PlanetType> exploredPlanets = new List<PlanetType>();
//
//	public static Dictionary<PlanetType, int> planetProbes = new Dictionary<PlanetType, int>();
//
//	public static Dictionary<PlanetType, float> planetExploredPercent = new Dictionary<PlanetType, float>();
//
//	public static int probesCount = 100;
}
/* FIX:
	- переделать зум в космосе, чтобы колёсиком мыши не увеличивалось и не уменьшалось звёздное поле, а только корабли и планеты
	- настроить нормальный расчёт цен для предметов
*/
/* TODO:
	- планета (или астероид) сначала исследуется (используя навык "Геолог"), когда выясняется какие минералы есть - на неё засылается харвестер, который после сбора возвращается на указанную планету
	- В инвенторе три кнопки - отображать только товары, снаряжение или оборудование
	- В магазине кораблей - либо сделать, чтобы корпуса не повторялись, либо сделать характеристики корпуса рандомными
	- Если сажаться на планету без щитов - корабль получает повреждения
	- Анимацию выхлопа для двигателей
	- Для ремонта корабля дроидом необходима такая штука как scrapMetal - выбивается из врагов и покупается в магазинах
	- Когда покидаем планету переносим предметы из buybackInventory в marketInventory или просто избавиться от buybackInventory
	- На планете есть медцентры в которых можно баффить персонажа
*/
/* IDEAS:
	- На исследование планеты тратяться зонды и исследование планеты занимает время, чем больше зондов, тем быстрее исследуется планета
	или это миниигра, если проиграл - зонд утерян, выиграл - планета исследована, на необитаемых планетах можно оставлять харвестры для сбора ресурсов
	- Если планета исследована и она обитаема - приземляемся и проводим разведку, чтобы найти противников
	- магазин корпусов - широкий ангар в котором стоят корабли, он прокручивается по горизонтали как бекграунд поанеты и при наведении мыши на корабль появляет информация о его корпусе
*/