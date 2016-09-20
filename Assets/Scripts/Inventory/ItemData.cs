using UnityEngine;
using System.Collections;

public abstract class ItemData {
	public Type itemType { get; protected set; }
	public string name { get; protected set; }
	public string description { get; protected set; }
	public float volume { get; protected set; }
	public Kind kind { get; protected set; }

	public Quality quality { get; private set; }
	public float level { get; private set; }

	public int cost { get; private set; }
	public int energyNeeded { get; private set; }

	protected ItemData (Quality quality, float level) {
		this.quality = quality;
		this.level = level;
	}

	public void initCommons (int cost, int energyNeeded) {
		this.cost = cost;
		this.energyNeeded = energyNeeded;
	}

	public enum Kind {
		GOOD, GEAR, EQUIPMENT
	}

	public enum Type {
		WEAPON, ENGINE, ARMOR, GENERATOR, RADAR, SHIELD, REPAIR_DROID, HARVESTER,
		HAND_WEAPON, BODY_ARMOR,
		MINERAL
	}

	public enum Quality {
		NORMAL, SUPERIOR, UNIQUE//Normal - обычное, Superior - отличное, Unique - уникальное
	}
}

public class HandWeaponData : ItemData {
	public HandWeaponType type { get; private set; }
	public int damage { get; private set; }

	public HandWeaponData (Quality quality, float level, HandWeaponType type, int damage) : base (quality, level) {
		this.type = type;
		this.damage = damage;

		this.name = type.getName();
		this.description = type.getDescription();
		this.volume = type.getVolume();

		this.itemType = Type.HAND_WEAPON;
		this.kind = Kind.GEAR;
	}
}

public class BodyArmorData : ItemData {
	public BodyArmorType type { get; private set; }
	public int armorClass { get; private set; }

	public BodyArmorData (Quality quality, float level, BodyArmorType type, int armorClass) : base(quality, level) {
		this.type = type;
		this.armorClass = armorClass;

		this.name = type.getName();
		this.description = type.getDescription();
		this.volume = type.getVolume();

		this.itemType = Type.BODY_ARMOR;
		this.kind = Kind.GEAR;
	}
}

public class WeaponData : ItemData {
	public WeaponType type { get; private set; }
	public int minDamage { get; private set; }
	public int maxDamage { get; private set; }
	public float reloadTime { get; private set; }

	public WeaponData (Quality quality, float level, WeaponType type, int minDamage, int maxDamage, float reloadTime) : base(quality, level) {
		this.type = type;
		this.minDamage = minDamage;
		this.maxDamage = maxDamage;
		this.reloadTime = reloadTime;

		this.name = type.getName();
		this.description = type.getDescription();
		this.volume = type.getVolume();

		this.itemType = Type.WEAPON;
		this.kind = Kind.EQUIPMENT;
	}
}

public class EngineData : ItemData {
	public EngineType type { get; private set; }
	public float power { get; private set; }

	public EngineData (Quality quality, float level, EngineType type, float power) : base(quality, level) {
		this.type = type;
		this.power = power;

		this.name = type.getName();
		this.description = type.getDescription();
		this.volume = type.getVolume();

		this.itemType = Type.ENGINE;
		this.kind = Kind.EQUIPMENT;
	}
}

public class ArmorData : ItemData {
	public ArmorType type { get; private set; }
	public int armorClass { get; private set; }

	public ArmorData (Quality quality, float level, ArmorType type, int armorClass) : base(quality, level) {
		this.type = type;
		this.armorClass = armorClass;

		this.name = type.getName();
		this.description = type.getDescription();
		this.volume = type.getVolume();

		this.itemType = Type.ARMOR;
		this.kind = Kind.EQUIPMENT;
	}
}

public class GeneratorData : ItemData {
	public GeneratorType type { get; private set; }
	public int maxEnergy { get; private set; }

	public GeneratorData (Quality quality, float level, GeneratorType type, int maxEnergy) : base(quality, level) {
		this.type = type;
		this.maxEnergy = maxEnergy;

		this.name = type.getName();
		this.description = type.getDescription();
		this.volume = type.getVolume();

		this.itemType = Type.GENERATOR;
		this.kind = Kind.EQUIPMENT;
	}
}

public class RadarData : ItemData {
	public RadarType type { get; private set; }
	public int range { get; private set; }

	public RadarData (Quality quality, float level, RadarType type, int range) : base(quality, level) {
		this.type = type;
		this.range = range;

		this.name = type.getName();
		this.description = type.getDescription();
		this.volume = type.getVolume();

		this.itemType = Type.RADAR;
		this.kind = Kind.EQUIPMENT;
	}
}

public class ShieldData : ItemData {
	public ShieldType type { get; private set; }
	public int shieldLevel { get; private set; }
	public int rechargeSpeed { get; private set; }

	public ShieldData (Quality quality, float level, ShieldType type, int shieldLevel, int rechargeSpeed) : base(quality, level) {
		this.type = type;
		this.shieldLevel = shieldLevel;
		this.rechargeSpeed = rechargeSpeed;

		this.name = type.getName();
		this.description = type.getDescription();
		this.volume = type.getVolume();

		this.itemType = Type.SHIELD;
		this.kind = Kind.EQUIPMENT;
	}
}

public class RepairDroidData : ItemData {
	public RepairDroidType type { get; private set; }
	public int repairSpeed { get; private set; }

	public RepairDroidData (Quality quality, float level, RepairDroidType type, int repairSpeed) : base(quality, level) {
		this.type = type;
		this.repairSpeed = repairSpeed;

		this.name = type.getName();
		this.description = type.getDescription();
		this.volume = type.getVolume();

		this.itemType = Type.REPAIR_DROID;
		this.kind = Kind.EQUIPMENT;
	}
}

public class HarvesterData : ItemData {
	public HarvesterType type { get; private set; }
	public int harvestTime { get; private set; }

	public HarvesterData (Quality quality, float level, HarvesterType type, int harvestTime) : base(quality, level) {
		this.type = type;
		this.harvestTime = harvestTime;

		this.name = type.getName();
		this.description = type.getDescription();
		this.volume = type.getVolume();

		this.itemType = Type.HARVESTER;
		this.kind = Kind.EQUIPMENT;
	}
}