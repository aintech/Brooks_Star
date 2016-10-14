using UnityEngine;
using System.Collections;

public abstract class ItemData {
	public ItemType itemType { get; protected set; }
	public string name { get; protected set; }
	public string description { get; protected set; }
	public float volume { get; protected set; }
	public int quantity = 1;

	public ItemQuality quality { get; private set; }
	public float level { get; private set; }

	public int cost { get; private set; }
	public int energyNeeded { get; private set; }

	protected ItemData (ItemQuality quality, float level) {
		this.quality = quality;
		this.level = level;
	}

	public void initCommons (int cost, int energyNeeded) {
		this.cost = cost;
		this.energyNeeded = energyNeeded;
	}
}

public class GoodsData : ItemData {
	public GoodsType type { get; private set; }
	public GoodsData (GoodsType type, int quantity) : base(ItemQuality.COMMON, 1) {
		this.type = type;
		this.quantity = quantity;
		this.name = type.name();
		this.description = type.description();
		this.volume = type.volume();
		this.itemType = ItemType.GOODS;
	}
}

public class HandWeaponData : ItemData {
	public HandWeaponType type { get; private set; }
	public int minDamage { get; private set; }
	public int maxDamage { get; private set;}

	public HandWeaponData (ItemQuality quality, float level, HandWeaponType type, int minDamage, int maxDamage) : base (quality, level) {
		this.type = type;
		this.minDamage = minDamage;
		this.maxDamage = maxDamage;

		this.name = type.getName();
		this.description = type.getDescription();
		this.volume = type.getVolume();

		this.itemType = ItemType.HAND_WEAPON;
	}
}

public class BodyArmorData : ItemData {
	public BodyArmorType type { get; private set; }
	public int armorClass { get; private set; }

	public BodyArmorData (ItemQuality quality, float level, BodyArmorType type, int armorClass) : base(quality, level) {
		this.type = type;
		this.armorClass = armorClass;

		this.name = type.getName();
		this.description = type.getDescription();
		this.volume = type.getVolume();

		this.itemType = ItemType.BODY_ARMOR;
	}
}

public class WeaponData : ItemData {
	public WeaponType type { get; private set; }
	public int minDamage { get; private set; }
	public int maxDamage { get; private set; }
	public float reloadTime { get; private set; }

	public WeaponData (ItemQuality quality, float level, WeaponType type, int minDamage, int maxDamage, float reloadTime) : base(quality, level) {
		this.type = type;
		this.minDamage = minDamage;
		this.maxDamage = maxDamage;
		this.reloadTime = reloadTime;

		this.name = type.getName();
		this.description = type.getDescription();
		this.volume = type.getVolume();

		this.itemType = ItemType.WEAPON;
	}
}

public class EngineData : ItemData {
	public EngineType type { get; private set; }
	public float power { get; private set; }

	public EngineData (ItemQuality quality, float level, EngineType type, float power) : base(quality, level) {
		this.type = type;
		this.power = power;

		this.name = type.getName();
		this.description = type.getDescription();
		this.volume = type.getVolume();

		this.itemType = ItemType.ENGINE;
	}
}

public class ArmorData : ItemData {
	public ArmorType type { get; private set; }
	public int armorClass { get; private set; }

	public ArmorData (ItemQuality quality, float level, ArmorType type, int armorClass) : base(quality, level) {
		this.type = type;
		this.armorClass = armorClass;

		this.name = type.getName();
		this.description = type.getDescription();
		this.volume = type.getVolume();

		this.itemType = ItemType.ARMOR;
	}
}

public class GeneratorData : ItemData {
	public GeneratorType type { get; private set; }
	public int maxEnergy { get; private set; }

	public GeneratorData (ItemQuality quality, float level, GeneratorType type, int maxEnergy) : base(quality, level) {
		this.type = type;
		this.maxEnergy = maxEnergy;

		this.name = type.getName();
		this.description = type.getDescription();
		this.volume = type.getVolume();

		this.itemType = ItemType.GENERATOR;
	}
}

public class RadarData : ItemData {
	public RadarType type { get; private set; }
	public int range { get; private set; }

	public RadarData (ItemQuality quality, float level, RadarType type, int range) : base(quality, level) {
		this.type = type;
		this.range = range;

		this.name = type.getName();
		this.description = type.getDescription();
		this.volume = type.getVolume();

		this.itemType = ItemType.RADAR;
	}
}

public class ShieldData : ItemData {
	public ShieldType type { get; private set; }
	public int shieldLevel { get; private set; }
	public int rechargeSpeed { get; private set; }

	public ShieldData (ItemQuality quality, float level, ShieldType type, int shieldLevel, int rechargeSpeed) : base(quality, level) {
		this.type = type;
		this.shieldLevel = shieldLevel;
		this.rechargeSpeed = rechargeSpeed;

		this.name = type.getName();
		this.description = type.getDescription();
		this.volume = type.getVolume();

		this.itemType = ItemType.SHIELD;
	}
}

public class RepairDroidData : ItemData {
	public RepairDroidType type { get; private set; }
	public int repairSpeed { get; private set; }

	public RepairDroidData (ItemQuality quality, float level, RepairDroidType type, int repairSpeed) : base(quality, level) {
		this.type = type;
		this.repairSpeed = repairSpeed;

		this.name = type.getName();
		this.description = type.getDescription();
		this.volume = type.getVolume();

		this.itemType = ItemType.REPAIR_DROID;
	}
}

public class HarvesterData : ItemData {
	public HarvesterType type { get; private set; }
	public int harvestTime { get; private set; }

	public HarvesterData (ItemQuality quality, float level, HarvesterType type, int harvestTime) : base(quality, level) {
		this.type = type;
		this.harvestTime = harvestTime;

		this.name = type.getName();
		this.description = type.getDescription();
		this.volume = type.getVolume();

		this.itemType = ItemType.HARVESTER;
	}
}