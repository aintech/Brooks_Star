using UnityEngine;
using System.Collections;

public class InventoryItem : MonoBehaviour {

	public Sprite[] weaponSprites, engineSprites, armorSprites, generatorSprites, radarSprites, shieldSprites, repairDroidSprites, harvesterSprites;

	private SpriteRenderer render;

	private InventoryCell cell;

	private HullSlot hullSlot;

	private int index;

	private Type itemType;

	private Quality itemQuality;

	private float itemLevel;

	private ItemData itemData;

	private int cost;

	private int energyNeeded;
	
	private float volume;

	private void setSprite (ItemData itemData, Type itemType) {
		this.itemType = itemType;
		if (render == null) render = transform.GetComponent<SpriteRenderer>();
		switch (itemType) {
			case Type.Weapon:
				WeaponType weaponType = ((WeaponData)itemData).getType();
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
			case Type.Engine:
				EngineType engineType = ((EngineData)itemData).getType();
				switch (engineType) {
					case EngineType.Force: render.sprite = engineSprites[0]; break;
					case EngineType.Gradual: render.sprite = engineSprites[1]; break;
					case EngineType.Proton: render.sprite = engineSprites[2]; break;
					case EngineType.Allur: render.sprite = engineSprites[3]; break;
					case EngineType.Quazar: render.sprite = engineSprites[4]; break;
					default: Debug.Log("Неизвестный тип двигателя"); break;
				} break;
			case Type.Armor:
				ArmorType armorType = ((ArmorData)itemData).getType();
				switch (armorType) {
					case ArmorType.Steel: render.sprite = armorSprites[0]; break;
					case ArmorType.HardenedSteel: render.sprite = armorSprites[1]; break;
					case ArmorType.Titan: render.sprite = armorSprites[2]; break;
					case ArmorType.Astron: render.sprite = armorSprites[3]; break;
					case ArmorType.Adamant: render.sprite = armorSprites[4]; break;
					default: Debug.Log("Неизвестный тип брони"); break;
				} break;
			case Type.Generator:
				GeneratorType generatorType = ((GeneratorData)itemData).getType();
				switch (generatorType) {
					case GeneratorType.Atomic: render.sprite = generatorSprites[0]; break;
					case GeneratorType.Plasma: render.sprite = generatorSprites[1]; break;
					case GeneratorType.Multyphase: render.sprite = generatorSprites[2]; break;
					case GeneratorType.Tunnel: render.sprite = generatorSprites[3]; break;
					default: Debug.Log("Неизвестный тип генератора"); break;
				} break;
			case Type.Radar:
				RadarType radarType = ((RadarData)itemData).getType();
				switch (radarType) {
					case RadarType.Sequester: render.sprite = radarSprites[0]; break;
					case RadarType.Planar: render.sprite = radarSprites[1]; break;
					case RadarType.Matrix: render.sprite = radarSprites[2]; break;
					case RadarType.PatanCorsac: render.sprite = radarSprites[3]; break;
					case RadarType.Snake: render.sprite = radarSprites[4]; break;
					case RadarType.Astral: render.sprite = radarSprites[5]; break;
					default: Debug.Log("Неизвестный тип радара"); break;
				} break;
			case Type.Shield:
				ShieldType shieldType = ((ShieldData)itemData).getType();
				switch (shieldType) {
					case ShieldType.Block: render.sprite = shieldSprites[0]; break;
					case ShieldType.Quadratic: render.sprite = shieldSprites[1]; break;
					case ShieldType.Cell: render.sprite = shieldSprites[2]; break;
					case ShieldType.Phase: render.sprite = shieldSprites[3]; break;
					default: Debug.Log("Неизвестный тип щита"); break;
				} break;
			case Type.RepairDroid:
				RepairDroidType droidType = ((RepairDroidData)itemData).getType();
				switch (droidType) {
					case RepairDroidType.Rail: render.sprite = repairDroidSprites[0]; break;
					case RepairDroidType.Channel: render.sprite = repairDroidSprites[1]; break;
					case RepairDroidType.Biphasic: render.sprite = repairDroidSprites[2]; break;
					case RepairDroidType.Thread: render.sprite = repairDroidSprites[3]; break;
					default: Debug.Log("Неизвестный тип ремонтного робота"); break;
				} break;
			case Type.Harvester:
				HarvesterType harvesterType = ((HarvesterData)itemData).getType();
				switch (harvesterType) {
					case HarvesterType.Mechanical: render.sprite = harvesterSprites[0]; break;
					case HarvesterType.Plasmatic: render.sprite = harvesterSprites[1]; break;
					case HarvesterType.Generative: render.sprite = harvesterSprites[2]; break;
					default: Debug.Log("Неизвестный тип сборщика"); break;
				} break;
		}
	}

	public void returnToParentInventory () {
		Inventory inventory = transform.parent.GetComponent<Inventory> ();
		inventory.addItemToCell (this, getCell());
	}

	public InventoryCell getCell() {
		return cell;
	}

	public void setCell(InventoryCell cell) {
		this.cell = cell;
	}

	public HullSlot getHullSlot () {
		return hullSlot;
	}

	public void setHullSlot (HullSlot hullSlot) {
		this.hullSlot = hullSlot;
	}

	public void setVolume (float volume) {
		this.volume = volume;
	}

	public float getVolume () {
		return volume;
	}

	public void setCost (int cost) {
		this.cost = cost;
	} 

	public int getCost () {
		return cost;
	}

	public void setEnergyNeeded (int energyNeeded) {
		this.energyNeeded = energyNeeded;
	}

	public int getEnergyNeeded () {
		return energyNeeded;
	}

	public Type getItemType () {
		return itemType;
	}

	public void setItemQuality (Quality itemQuality) {
		this.itemQuality = itemQuality;
	}

	public Quality getItemQuality () {
		return itemQuality;
	}

	public void setItemLevel (float itemLevel) {
		this.itemLevel = itemLevel;
	}

	public float getItemLevel () {
		return itemLevel;
	}
	
	public void setItemData (ItemData itemData) {
		this.itemData = itemData;
		if (itemData is WeaponData) setSprite (itemData, Type.Weapon);
		else if (itemData is EngineData) setSprite (itemData, Type.Engine);
		else if (itemData is ArmorData) setSprite (itemData, Type.Armor);
		else if (itemData is GeneratorData) setSprite (itemData, Type.Generator);
		else if (itemData is RadarData) setSprite (itemData, Type.Radar);
		else if (itemData is ShieldData) setSprite (itemData, Type.Shield);
		else if (itemData is RepairDroidData) setSprite (itemData, Type.RepairDroid);
		else if (itemData is HarvesterData) setSprite (itemData, Type.Harvester);
	}

	public ItemData getItemData () {
		return itemData;
	}

	public string getItemName () {
		return getItemData ().getItemName ();
	}

	public string getItemDescription () {
		return getItemData ().getItemDescription ();
	}

	public void initialaizeFromSource (InventoryItem source) {
		setItemData (source.getItemData ());
		setItemLevel (source.getItemLevel ());
		setItemQuality (source.getItemQuality ());
		setCost (source.getCost ());
		setEnergyNeeded (source.getEnergyNeeded ());
		setVolume (source.getVolume ());
	}

	public enum Type {
		Weapon, Engine, Armor, Generator, Radar, Shield, RepairDroid, Harvester
	}

	public enum Quality {
		Normal, Superior, Unique//Normal - обычное, Superior - отличное, Unique - уникальное
	}

	public abstract class ItemData {
		public abstract string getItemName ();
		public abstract string getItemDescription ();
	}

	public class WeaponData : ItemData {
		private WeaponType type;
		private int minDamage, maxDamage;
		private float reloadTime;

		public WeaponData (WeaponType type, int minDamage, int maxDamage, float reloadTime) {
			this.type = type;
			this.minDamage = minDamage;
			this.maxDamage = maxDamage;
			this.reloadTime = reloadTime;
		}
		
		public int getMinDamage () { return minDamage; }
		public int getMaxDamage () { return maxDamage; }
		public float getReloadTime () { return reloadTime; }
		public WeaponType getType () { return type; }
		public override string getItemName () { return type.getName(); }
		public override string getItemDescription () { return type.getDescription(); }
	}

	public class EngineData : ItemData {
		private EngineType type;
		private float power;

		public EngineData (EngineType type, float power) {
			this.type = type;
			this.power = power;
		}

		public float getPower () { return power; }
		public EngineType getType () { return type; }
		public override string getItemName () { return type.getName(); }
		public override string getItemDescription () { return type.getDescription(); }
	}
	
	public class ArmorData : ItemData {
		private ArmorType type;
		private int armorClass;

		public ArmorData (ArmorType type, int armorClass) {
			this.type = type;
			this.armorClass = armorClass;
		}

		public int getArmorClass () { return armorClass; }
		public ArmorType getType () { return type; }
		public override string getItemName () { return type.getName(); }
		public override string getItemDescription () { return type.getDescription(); }
	}

	public class GeneratorData : ItemData {
		private GeneratorType type;
		private int maxEnergy;
		
		public GeneratorData (GeneratorType type, int maxEnergy) {
			this.type = type;
			this.maxEnergy = maxEnergy;
		}
		
		public int getMaxEnergy () { return maxEnergy; }
		public GeneratorType getType () { return type; }
		public override string getItemName () { return type.getName(); }
		public override string getItemDescription () { return type.getDescription(); }
	}

	public class RadarData : ItemData {
		private RadarType type;
		private int range;

		public RadarData (RadarType type, int range) {
			this.type = type;
			this.range = range;
		}

		public int getRange () { return range; }
		public RadarType getType () { return type; }
		public override string getItemName () { return type.getName(); }
		public override string getItemDescription () { return type.getDescription(); }
	}
	
	public class ShieldData : ItemData {
		private ShieldType type;
		private int shieldLevel, rechargeSpeed;
		
		public ShieldData (ShieldType type, int shieldLevel, int rechargeSpeed) {
			this.type = type;
			this.shieldLevel = shieldLevel;
			this.rechargeSpeed = rechargeSpeed;
		}

		public int getShieldLevel () { return shieldLevel; }
		public int getRechargeSpeed () { return rechargeSpeed; }
		public ShieldType getType () { return type; }
		public override string getItemName () { return type.getName(); }
		public override string getItemDescription () { return type.getDescription(); }
	}

	public class RepairDroidData : ItemData {
		private RepairDroidType type;
		private int repairSpeed;
		
		public RepairDroidData (RepairDroidType type, int repairSpeed) {
			this.type = type;
			this.repairSpeed = repairSpeed;
		}

		public int getRepairSpeed () { return repairSpeed; }
		public RepairDroidType getType () { return type; }
		public override string getItemName () { return type.getName(); }
		public override string getItemDescription () { return type.getDescription(); }
	}
	
	public class HarvesterData : ItemData {
		private HarvesterType type;
		private int harvestTime;

		public HarvesterData (HarvesterType type, int harvestTime) {
			this.type = type;
			this.harvestTime = harvestTime;
		}

		public int getHarvestTime () { return harvestTime; }
		public HarvesterType getType () { return type; }
		public override string getItemName () { return type.getName(); }
		public override string getItemDescription () { return type.getDescription(); }
	}
}