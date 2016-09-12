using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class Vars {
	
	public static PlanetType planetType = PlanetType.CORAS;

	public static HullType shipHullType = HullType.Corvette;

	public static Dictionary<string, InventoryItem> shipHullSlotsMap = new Dictionary<string, InventoryItem> ();

	public static int shipCurrentHealth = -1;

	public static int cash = 90000;

	public static Dictionary<int, InventoryItem> inventory = new Dictionary<int, InventoryItem>();

	public static Dictionary<int, InventoryItem> storage = new Dictionary<int, InventoryItem>();

	public static Dictionary<int, InventoryItem> marketCORAS = new Dictionary<int, InventoryItem> ();

	public static int freeSortingOrder = 0;

	public static List<EnemyShip> enemyShipsPool = new List<EnemyShip>();

	public static UserInterface userInterface;
}


//TODO: В инвенторе три кнопки - отображать только товары, снаряжение или оборудование

//Анимацию выхлопа для двигателей
//Для ремонта корабля дроидом необходима такая штука как scrapMetal - выбивается из врагов и покупается в магазинах
//Есть такая штука как ScrollRet - можно попробовать через нее сделать инвентарь с прокруткой
//Когда покидаем планету переносим предметы из buybackInventory в marketInventory или просто избавиться от buybackInventory
//Придумать что-то с прокруткой инвентаря