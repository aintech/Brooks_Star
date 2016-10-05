﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class Vars {

	public static bool EROTIC = true;

	public static StarSystemType starSystemType = StarSystemType.ALURIA;

	public static PlanetType planetType = PlanetType.CORAS;

	public static HullType shipHullType = HullType.Corvette;

	public static Dictionary<KeyValuePair<HullSlot.Type, int>, ItemData> shipHullSlotsMap = new Dictionary<KeyValuePair<HullSlot.Type, int>, ItemData> ();

	public static int shipCurrentHealth = -1;

	public static int cash = 90000;

	public static Dictionary<int, ItemData> inventory = new Dictionary<int, ItemData>(),
											marketCORAS = new Dictionary<int, ItemData> ();

	public static int freeSortingOrder = 0;

	public static List<EnemyShip> enemyShipsPool = new List<EnemyShip>();

	public static UserInterface userInterface;
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
	- магазин корпусов - широкий ангар в котором стоят корабли, он прокручивается по горизонтали как бекграунд поанеты и при наведении мыши на корабль появляет информация о его корпусе
*/