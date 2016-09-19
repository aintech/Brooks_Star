﻿using UnityEngine;
using System.Collections;

public class Item : MonoBehaviour {

	public Sprite[] weaponSprites, engineSprites, armorSprites, generatorSprites, radarSprites, shieldSprites, repairDroidSprites, harvesterSprites;

	private SpriteRenderer render;

    public InventoryCell cell;

	public HullSlot hullSlot;
    
	public ItemData itemData { get; private set; }

	public Item init (ItemData itemData) {
		this.itemData = itemData;
		if (render == null) render = transform.GetComponent<SpriteRenderer>();
		switch (itemData.itemType) {
			case ItemData.Type.WEAPON:
				WeaponType weaponType = ((WeaponData)itemData).type;
				switch (weaponType) {
					case WeaponType.Blaster: render.sprite = weaponSprites[0]; break;
					case WeaponType.Plasmer: render.sprite = weaponSprites[1]; break;
					case WeaponType.Charger: render.sprite = weaponSprites[2]; break;
					case WeaponType.Emitter: render.sprite = weaponSprites[3]; break;
					case WeaponType.Waver: render.sprite = weaponSprites[4]; break;
					case WeaponType.Launcher: render.sprite = weaponSprites[5]; break;
					case WeaponType.Suppressor: render.sprite = weaponSprites[6]; break;
					default: Debug.Log("Неизвестный тип оружия"); break;
				} break;
			case ItemData.Type.ENGINE:
				EngineType engineType = ((EngineData)itemData).type;
				switch (engineType) {
					case EngineType.Force: render.sprite = engineSprites[0]; break;
					case EngineType.Gradual: render.sprite = engineSprites[1]; break;
					case EngineType.Proton: render.sprite = engineSprites[2]; break;
					case EngineType.Allur: render.sprite = engineSprites[3]; break;
					case EngineType.Quazar: render.sprite = engineSprites[4]; break;
					default: Debug.Log("Неизвестный тип двигателя"); break;
				} break;
			case ItemData.Type.ARMOR:
				ArmorType armorType = ((ArmorData)itemData).type;
				switch (armorType) {
					case ArmorType.Steel: render.sprite = armorSprites[0]; break;
					case ArmorType.HardenedSteel: render.sprite = armorSprites[1]; break;
					case ArmorType.Titan: render.sprite = armorSprites[2]; break;
					case ArmorType.Astron: render.sprite = armorSprites[3]; break;
					case ArmorType.Adamant: render.sprite = armorSprites[4]; break;
					default: Debug.Log("Неизвестный тип брони"); break;
				} break;
			case ItemData.Type.GENERATOR:
				GeneratorType generatorType = ((GeneratorData)itemData).type;
				switch (generatorType) {
					case GeneratorType.Atomic: render.sprite = generatorSprites[0]; break;
					case GeneratorType.Plasma: render.sprite = generatorSprites[1]; break;
					case GeneratorType.Multyphase: render.sprite = generatorSprites[2]; break;
					case GeneratorType.Tunnel: render.sprite = generatorSprites[3]; break;
					default: Debug.Log("Неизвестный тип генератора"); break;
				} break;
			case ItemData.Type.RADAR:
				RadarType radarType = ((RadarData)itemData).type;
				switch (radarType) {
					case RadarType.Sequester: render.sprite = radarSprites[0]; break;
					case RadarType.Planar: render.sprite = radarSprites[1]; break;
					case RadarType.Matrix: render.sprite = radarSprites[2]; break;
					case RadarType.PatanCorsac: render.sprite = radarSprites[3]; break;
					case RadarType.Snake: render.sprite = radarSprites[4]; break;
					case RadarType.Astral: render.sprite = radarSprites[5]; break;
					default: Debug.Log("Неизвестный тип радара"); break;
				} break;
			case ItemData.Type.SHIELD:
				ShieldType shieldType = ((ShieldData)itemData).type;
				switch (shieldType) {
					case ShieldType.Block: render.sprite = shieldSprites[0]; break;
					case ShieldType.Quadratic: render.sprite = shieldSprites[1]; break;
					case ShieldType.Cell: render.sprite = shieldSprites[2]; break;
					case ShieldType.Phase: render.sprite = shieldSprites[3]; break;
					default: Debug.Log("Неизвестный тип щита"); break;
				} break;
			case ItemData.Type.REPAIR_DROID:
				RepairDroidType droidType = ((RepairDroidData)itemData).type;
				switch (droidType) {
					case RepairDroidType.Rail: render.sprite = repairDroidSprites[0]; break;
					case RepairDroidType.Channel: render.sprite = repairDroidSprites[1]; break;
					case RepairDroidType.Biphasic: render.sprite = repairDroidSprites[2]; break;
					case RepairDroidType.Thread: render.sprite = repairDroidSprites[3]; break;
					default: Debug.Log("Неизвестный тип ремонтного робота"); break;
				} break;
			case ItemData.Type.HARVESTER:
				HarvesterType harvesterType = ((HarvesterData)itemData).type;
				switch (harvesterType) {
					case HarvesterType.Mechanical: render.sprite = harvesterSprites[0]; break;
					case HarvesterType.Plasmatic: render.sprite = harvesterSprites[1]; break;
					case HarvesterType.Generative: render.sprite = harvesterSprites[2]; break;
					default: Debug.Log("Неизвестный тип сборщика"); break;
				} break;
		}
		return this;
	}

	public ItemData.Kind getItemKind () {
		return  itemData.kind;
	}

	public void returnToParentInventory () {
		Inventory inventory = transform.parent.GetComponent<Inventory> ();
		inventory.addItemToCell (this, cell);
	}

	public float getVolume () {
		return itemData.volume;
	}

	public int getCost () {
		return itemData.cost;
	}

	public int getEnergyNeeded () {
		return itemData.energyNeeded;
	}

	public ItemData.Type getItemType () {
		return itemData.itemType;
	}

	public ItemData.Quality getItemQuality () {
		return itemData.quality;
	}

	public float getItemLevel () {
		return itemData.level;
	}

	public string getItemName () {
		return itemData.name;
	}

	public string getItemDescription () {
		return itemData.description;
	}
}