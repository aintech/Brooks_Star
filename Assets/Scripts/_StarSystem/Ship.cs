using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class Ship : MonoBehaviour {
	
	public Transform blasterPrefab, plasmerPrefab, chargerPrefab, emitterPrefab, waverPrefab, launcherPrefab, suppressorPrefab;

	public Sprite[] hullSprites;

	public Sprite[] engineSprites;

	private Transform mainExhaust, leftExhaust, rightExhaust;

	private BoxCollider2D shipCollider;

	private SpriteRenderer hullRender, engineRender;

	protected int armor, health, shield, fullShield, fullHealth;

	public float radarRange { get; protected set; }

	public Engine engine { get; private set; }
	
	protected Weapon weapon_1, weapon_2, weapon_3, weapon_4, weapon_5;

	protected Weapon[] weapons;

	protected int shieldRenderOrder = 3;

	private static Dictionary<HullType, List<Vector3>> weaponSlotsMap;

	private static Dictionary<HullType, Vector3> engineMap;

	private static Dictionary<HullType, Vector3> colliderMap;

	private static Dictionary<HullType, Vector3> exhaustMap;

	private Vector3 mainExhaustOffset = new Vector3(0, .2f, 0);

	private HullType hullType;

	protected ShieldsPool shieldsPool;

	public ShipController controller { get; private set; }

	public bool alive { get; private set; }

	public bool destroed { get; private set; }

	protected void initInner () {
		if (weaponSlotsMap == null) {
			initializeWeaponSlotsMap();
			initializeEngineMap();
			initializeColliderMap();
			initializeExhaustMap();
		}

		weapons = new Weapon[5];
		weapons[0] = weapon_1;
		weapons[1] = weapon_2;
		weapons[2] = weapon_3;
		weapons[3] = weapon_4;
		weapons[4] = weapon_5;

		engine = transform.FindChild("Engine").GetComponent<Engine>();
		shieldsPool = GameObject.Find("ShieldsPool").GetComponent<ShieldsPool>();
		shipCollider = transform.GetComponent<BoxCollider2D>();
		controller = transform.GetComponent<ShipController>();

		destroed = false;
		alive = true;
		shipCollider.enabled = true;
	}

	protected void setHullType (HullType hullType) {
		this.hullType = hullType;
		setHullSprite();
		resizeCollider();
	}

	public HullType getHullType () {
		return hullType;
	}

	private void setHullSprite () {
		if (hullRender == null) getHullRender();
		switch (hullType) {
			case HullType.Little: hullRender.sprite = hullSprites[0]; break;
			case HullType.Needle: hullRender.sprite = hullSprites[1]; break;
			case HullType.Gnome: hullRender.sprite = hullSprites[2]; break;
			case HullType.Cricket: hullRender.sprite = hullSprites[3]; break;
			case HullType.Argo: hullRender.sprite = hullSprites[4]; break;
			case HullType.Falcon: hullRender.sprite = hullSprites[5]; break;
			case HullType.Adventurer: hullRender.sprite = hullSprites[6]; break;
			case HullType.Corvette: hullRender.sprite = hullSprites[7]; break;
			case HullType.Buffalo: hullRender.sprite = hullSprites[8]; break;
			case HullType.Legionnaire: hullRender.sprite = hullSprites[9]; break;
			case HullType.StarWalker: hullRender.sprite = hullSprites[10]; break;
			case HullType.Warship: hullRender.sprite = hullSprites[11]; break;
			case HullType.Asterix: hullRender.sprite = hullSprites[12]; break;
			case HullType.Prime: hullRender.sprite = hullSprites[13]; break;
			case HullType.TITAN: hullRender.sprite = hullSprites[14]; break;
			case HullType.Dreadnaut: hullRender.sprite = hullSprites[15]; break;
			case HullType.Armageddon: hullRender.sprite = hullSprites[16]; break;
			default: Debug.Log("Неизвестный тип корпуса"); break;
		}
	}

	private void resizeCollider () {
		Vector3 collSize;
		colliderMap.TryGetValue(hullType, out collSize);
		shipCollider.offset = new Vector2(0, collSize.x);
		shipCollider.size = new Vector2(collSize.y, collSize.z);
	}

	protected void setEngineSprite () {
		if (engineRender == null) getEngineRender();
		switch (engine.getEngineType()) {
			case EngineType.FORCE: engineRender.sprite = engineSprites[0]; break;
			case EngineType.GRADUAL: engineRender.sprite = engineSprites[1]; break;
			case EngineType.PROTON: engineRender.sprite = engineSprites[2]; break;
			case EngineType.ALLUR: engineRender.sprite = engineSprites[3]; break;
			case EngineType.QUAZAR: engineRender.sprite = engineSprites[4]; break;
			default: Debug.Log("Неизвестный тип двигателя"); break;
		}
		engine.transform.position = transform.position + engineMap[hullType];
		transform.Find("Main Exhaust").localPosition = engineMap[hullType] - mainExhaustOffset;
		transform.Find("Left Exhaust").localPosition = exhaustMap[hullType];
		transform.Find("Right Exhaust").localPosition = new Vector3(exhaustMap[hullType].x * -1, exhaustMap[hullType].y);
	}

	protected SpriteRenderer getHullRender () {
		if (hullRender == null) {
			hullRender = transform.FindChild("Hull Render").GetComponent<SpriteRenderer>();
		}
		return hullRender;
	}

	protected SpriteRenderer getEngineRender () {
		if (engineRender == null) {
			engineRender = transform.FindChild("Engine").GetComponent<SpriteRenderer>();
		}
		return engineRender;
	}

	public int getHealth () {
		return health;
	}
	
	public int getFullHealth () {
		return fullHealth;
	}

	public int getArmor () {
		return armor;
	}

	public int getShield () {
		return shield;
	}

	public int getFullShield () {
		return fullShield;
	}

	public void damageShip (WeaponType type, Vector3 weaponPosition, int minDamage, int maxDamage) {
		//FOR TEST
		if (isPlayerShip()) { return; }

		if (type == WeaponType.BLASTER) {
			int damage = Random.Range(minDamage, maxDamage + 1);
			if (getShield() >= damage) {
				shield = getShield() - damage;
				shieldsPool.renderShieldReflection(this, weaponPosition);
			} else {
				int diff = damage - shield;
				shield = 0;
				health -= diff;
			}
		}
		if (getHealth() > 0) {
			updateHealthAndShieldInfo();
		} else {
			disableShip();
		}
	}

	virtual protected void updateHealthAndShieldInfo () {}

	virtual protected void disableShip () {
		alive = false;
		shipCollider.enabled = false;
		ExplosionsManager.playExplosion(this);
	}

	virtual public void destroyShip () {
		gameObject.SetActive(false);
		destroed = true;
	}

	public int getShieldRenderOrder () {
		return shieldRenderOrder;
	}

	protected Vector3 getWeaponPosition (int weaponIndex) {
		return weaponSlotsMap[hullType][weaponIndex];
	}

	private static void initializeWeaponSlotsMap () {
		weaponSlotsMap = new Dictionary<HullType, List<Vector3>>();
		int zOffset = StarField.zOffset;
		List<Vector3> list = new List<Vector3>();
		list.Add(new Vector3(-0.54f, -0.43f, zOffset));
		list.Add(new Vector3(0.54f, -0.43f, zOffset));
		weaponSlotsMap.Add(HullType.Adventurer, list);
		
		list = new List<Vector3>();
		list.Add(new Vector3(0, -0.48f, zOffset));
		weaponSlotsMap.Add(HullType.Argo, list);
		
		list = new List<Vector3>();
		list.Add(new Vector3(-0.785f, 0.54f, zOffset));
		list.Add(new Vector3(0.785f, 0.54f, zOffset));
		list.Add(new Vector3(0, -0.11f, zOffset));
		list.Add(new Vector3(-0.69f, -0.89f, zOffset));
		list.Add(new Vector3(0.69f, -0.89f, zOffset));
		weaponSlotsMap.Add(HullType.Armageddon, list);
		
		list = new List<Vector3>();
		list.Add(new Vector3(-0.61f, 0.225f, zOffset));
		list.Add(new Vector3(0.61f, 0.225f, zOffset));
		list.Add(new Vector3(0, -0.69f, zOffset));
		weaponSlotsMap.Add(HullType.Asterix, list);
		
		list = new List<Vector3>();
		list.Add(new Vector3(-0.57f, -0.37f, zOffset));
		list.Add(new Vector3(0.57f, -0.37f, zOffset));
		weaponSlotsMap.Add(HullType.Buffalo, list);
		
		list = new List<Vector3>();
		list.Add(new Vector3(-0.59f, -0.3f, zOffset));
		list.Add(new Vector3(0.59f, -0.3f, zOffset));
		weaponSlotsMap.Add(HullType.Corvette, list);
		
		list = new List<Vector3>();
		list.Add(new Vector3(0, -0.39f, zOffset));
		weaponSlotsMap.Add(HullType.Cricket, list);
		
		list = new List<Vector3>();
		list.Add(new Vector3(-0.775f, 0.24f, zOffset));
		list.Add(new Vector3(0.775f, 0.24f, zOffset));
		list.Add(new Vector3(-0.765f, -0.9f, zOffset));
		list.Add(new Vector3(0.765f, -0.9f, zOffset));
		weaponSlotsMap.Add(HullType.Dreadnaut, list);
		
		list = new List<Vector3>();
		list.Add(new Vector3(0, -0.545f, zOffset));
		weaponSlotsMap.Add(HullType.Falcon, list);
		
		list = new List<Vector3>();
		list.Add(new Vector3(-0.575f, -0.425f, zOffset));
		list.Add(new Vector3(0.575f, -0.425f, zOffset));
		weaponSlotsMap.Add(HullType.Legionnaire, list);
		
		list = new List<Vector3>();
		list.Add(new Vector3(-0.64f, 0.26f, zOffset));
		list.Add(new Vector3(0.64f, 0.26f, zOffset));
		list.Add(new Vector3(0, -0.64f, zOffset));
		weaponSlotsMap.Add(HullType.Prime, list);
		
		list = new List<Vector3>();
		list.Add(new Vector3(-0.575f, 0.14f, zOffset));
		list.Add(new Vector3(0.575f, 0.14f, zOffset));
		list.Add(new Vector3(0, -0.8f, zOffset));
		weaponSlotsMap.Add(HullType.StarWalker, list);
		
		list = new List<Vector3>();
		list.Add(new Vector3(-0.705f, 0.24f, zOffset));
		list.Add(new Vector3(0.705f, 0.24f, zOffset));
		list.Add(new Vector3(-0.62f, -0.92f, zOffset));
		list.Add(new Vector3(0.62f, -0.92f, zOffset));
		weaponSlotsMap.Add(HullType.TITAN, list);
		
		list = new List<Vector3>();
		list.Add(new Vector3(-0.575f, 0.165f, zOffset));
		list.Add(new Vector3(0.575f, 0.165f, zOffset));
		list.Add(new Vector3(0, -0.735f, zOffset));
		weaponSlotsMap.Add(HullType.Warship, list);
	}

	private void initializeEngineMap () {
		engineMap = new Dictionary<HullType, Vector3>();
		engineMap.Add(HullType.Little, new Vector3(0, -0.42f, 0));
		engineMap.Add(HullType.Needle, new Vector3(0, -0.57f, 0));
		engineMap.Add(HullType.Gnome, new Vector3(0, -0.61f, 0));
		engineMap.Add(HullType.Cricket, new Vector3(0, -0.725f, 0));
		engineMap.Add(HullType.Argo, new Vector3(0, -0.805f, 0));
		engineMap.Add(HullType.Falcon, new Vector3(0, -0.99f, 0));
		engineMap.Add(HullType.Adventurer, new Vector3(0, -0.975f, 0));
		engineMap.Add(HullType.Corvette, new Vector3(0, -0.985f, 0));
		engineMap.Add(HullType.Buffalo, new Vector3(0, -1.035f, 0));
		engineMap.Add(HullType.Legionnaire, new Vector3(0, -1.08f, 0));
		engineMap.Add(HullType.StarWalker, new Vector3(0, -1.1f, 0));
		engineMap.Add(HullType.Warship, new Vector3(0, -1.135f, 0));
		engineMap.Add(HullType.Asterix, new Vector3(0, -1.15f, 0));
		engineMap.Add(HullType.Prime, new Vector3(0, -1.11f, 0));
		engineMap.Add(HullType.TITAN, new Vector3(0, -1.11f, 0));
		engineMap.Add(HullType.Dreadnaut, new Vector3(0, -1.18f, 0));
		engineMap.Add(HullType.Armageddon, new Vector3(0, -1.33f, 0));
	}

	private void initializeColliderMap () {
		colliderMap = new Dictionary<HullType, Vector3>();
		colliderMap.Add(HullType.Little, new Vector3(-0.07f, 0.6f, 1));
		colliderMap.Add(HullType.Needle, new Vector3(-0.06f, 0.5f, 1.3f));
		colliderMap.Add(HullType.Gnome, new Vector3(-0.065f, 0.55f, 1.37f));
		colliderMap.Add(HullType.Cricket, new Vector3(-0.07f, 0.7f, 1.6f));
		colliderMap.Add(HullType.Argo, new Vector3(-0.03f, 0.7f, 1.8f));
		colliderMap.Add(HullType.Falcon, new Vector3(-0.06f, 0.8f, 2.15f));
		colliderMap.Add(HullType.Adventurer, new Vector3(-0.06f, 0.9f, 2.15f));
		colliderMap.Add(HullType.Corvette, new Vector3(-0.06f, 1, 2.15f));
		colliderMap.Add(HullType.Buffalo, new Vector3(-0.067f, 0.9f, 2.23f));
		colliderMap.Add(HullType.Legionnaire, new Vector3(-0.07f, 0.9f, 2.32f));
		colliderMap.Add(HullType.StarWalker, new Vector3(-0.07f, 0.95f, 2.32f));
		colliderMap.Add(HullType.Warship, new Vector3(-0.07f, 0.9f, 2.42f));
		colliderMap.Add(HullType.Asterix, new Vector3(-0.07f, 1, 2.46f));
		colliderMap.Add(HullType.Prime, new Vector3(-0.06f, 1.2f, 2.4f));
		colliderMap.Add(HullType.TITAN, new Vector3(-0.06f, 1.4f, 2.4f));
		colliderMap.Add(HullType.Dreadnaut, new Vector3(-0.04f, 1.8f, 2.58f));
		colliderMap.Add(HullType.Armageddon, new Vector3(-0.07f, 1.8f, 2.8f));
	}

	private void initializeExhaustMap () {
		exhaustMap = new Dictionary<HullType, Vector3>();
		exhaustMap.Add(HullType.Little, new Vector3(-.41f, .17f));
		exhaustMap.Add(HullType.Needle, new Vector3(-.41f, -.06f));
		exhaustMap.Add(HullType.Gnome, new Vector3(-.4f, .24f));
		exhaustMap.Add(HullType.Cricket, new Vector3(-.43f, .22f));
		exhaustMap.Add(HullType.Argo, new Vector3(-.45f, .34f));
		exhaustMap.Add(HullType.Falcon, new Vector3(-.62f, .22f));
		exhaustMap.Add(HullType.Adventurer, new Vector3(-.6f, .42f));
		exhaustMap.Add(HullType.Corvette, new Vector3(-.47f, .57f));
		exhaustMap.Add(HullType.Buffalo, new Vector3(-.57f, .6f));
		exhaustMap.Add(HullType.Legionnaire, new Vector3(-.52f, .58f));
		exhaustMap.Add(HullType.StarWalker, new Vector3(-.51f, .61f));
		exhaustMap.Add(HullType.Warship, new Vector3(-.58f, .54f));
		exhaustMap.Add(HullType.Asterix, new Vector3(-.45f, .6f));
		exhaustMap.Add(HullType.Prime, new Vector3(-.57f, .8f));
		exhaustMap.Add(HullType.TITAN, new Vector3(-.78f, .72f));
		exhaustMap.Add(HullType.Dreadnaut, new Vector3(-.92f, .6f));
		exhaustMap.Add(HullType.Armageddon, new Vector3(-1.06f, .83f));
	}

	public abstract bool isPlayerShip ();
}