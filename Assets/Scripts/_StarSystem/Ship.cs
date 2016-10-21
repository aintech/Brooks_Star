using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class Ship : MonoBehaviour {
	
	public Transform blasterPrefab, plasmerPrefab, chargerPrefab, emitterPrefab, waverPrefab, launcherPrefab, suppressorPrefab;

	public Sprite[] engineSprites;

	private BoxCollider2D shipCollider;

	private SpriteRenderer hullRender, engineRender;

	public int armor { get; protected set; }
	public int health { get; protected set; }
	public int shield { get; protected set; }
	public int fullShield { get; protected set; }
	public int fullHealth { get; protected set; }

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

	protected int shieldRechargeValue;

	protected float repairValue;

	private float afterDamageInterval = 5, recoverTime, repairAccumulation;

	private bool rechargeShield, repairHull;

//	private int repairCounter, toNextRepair = 100;

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

	void Update () {
		if (StarSystem.gamePaused) { return; }

		if (repairHull && recoverTime <= Time.time) {
			repairAccumulation += repairValue;
			if (repairAccumulation >= 5) {
				repairAccumulation -= 5;
				health += 5;
				if (health >= fullHealth) {
					health = fullHealth;
					repairHull = false;
				}
				updateHealthAndShieldInfo();
			}
		}
		if (rechargeShield && recoverTime <= Time.time) {
			shield += shieldRechargeValue;
			if (shield >= fullShield) {
				shield = fullShield;
				rechargeShield = false;
			}
			updateHealthAndShieldInfo();
		}
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
		hullRender.sprite = ImagesProvider.getHullSprite(hullType);
	}

	private void resizeCollider () {
		Vector3 collSize;
		colliderMap.TryGetValue(hullType, out collSize);
		shipCollider.offset = new Vector2(0, collSize.x);
		shipCollider.size = new Vector2(collSize.y, collSize.z);
	}

	protected void setEngineSprite () {
		if (engineRender == null) getEngineRender();
		switch (engine.engineType) {
			case EngineType.FORCE: engineRender.sprite = engineSprites[0]; break;
			case EngineType.GRADUAL: engineRender.sprite = engineSprites[1]; break;
			case EngineType.PROTON: engineRender.sprite = engineSprites[2]; break;
			case EngineType.ALLUR: engineRender.sprite = engineSprites[3]; break;
			case EngineType.QUAZAR: engineRender.sprite = engineSprites[4]; break;
			default: Debug.Log("Неизвестный тип двигателя"); break;
		}
		engine.transform.position = transform.position + engineMap[hullType];
		transform.Find("Main Exhaust").localPosition = engineMap[hullType] - mainExhaustOffset;
		transform.Find("Left Exhaust").localPosition = new Vector3(exhaustMap[hullType].x, exhaustMap[hullType].y);
		transform.Find("Right Exhaust").localPosition = new Vector3(-exhaustMap[hullType].x, exhaustMap[hullType].y);
		transform.Find("Front Exhaust").localPosition = new Vector3(0, exhaustMap[hullType].z);
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

	public void damageShip (WeaponType type, Vector3 weaponPosition, int minDamage, int maxDamage) {
		if (type == WeaponType.BLASTER) {
			int damage = Random.Range(minDamage, maxDamage + 1);
			if (shield >= damage) {
				shield -= damage;
				shieldsPool.renderShieldReflection(this, weaponPosition);
			} else {
				int diff = damage - shield;
				shield = 0;
				health -= diff;
				repairHull = health < fullHealth;
			}
		}
		if (shield < fullShield) {
			rechargeShield = true;
			recoverTime = Time.time + afterDamageInterval;
		}
		if (health > 0) {
			updateHealthAndShieldInfo();
		} else {
			disableShip();
		}
	}

	virtual protected void updateHealthAndShieldInfo () {}

	virtual protected void disableShip () {
		health = 0;
		alive = false;
		shipCollider.enabled = false;
		rechargeShield = false;
		repairHull = false;
		updateHealthAndShieldInfo();
		ExplosionsManager.playExplosion(this);
	}

	virtual public void destroyShip () {
		gameObject.SetActive(false);
		destroed = true;
		LootDropper.drop(this);
	}

	public int getShieldRenderOrder () {
		return shieldRenderOrder;
	}

	protected Vector3 getWeaponPosition (int weaponIndex) {
		return weaponSlotsMap[hullType][weaponIndex];
	}

	private static void initializeWeaponSlotsMap () {
		weaponSlotsMap = new Dictionary<HullType, List<Vector3>>();
		int zOffset = 0;//StarField.zOffset
		List<Vector3> list = new List<Vector3>();
		list.Add(new Vector3(-0.54f, -0.43f, zOffset));
		list.Add(new Vector3(0.54f, -0.43f, zOffset));
		weaponSlotsMap.Add(HullType.ADVENTURER, list);
		
		list = new List<Vector3>();
		list.Add(new Vector3(0, -0.48f, zOffset));
		weaponSlotsMap.Add(HullType.ARGO, list);
		
		list = new List<Vector3>();
		list.Add(new Vector3(-0.785f, 0.54f, zOffset));
		list.Add(new Vector3(0.785f, 0.54f, zOffset));
		list.Add(new Vector3(0, -0.11f, zOffset));
		list.Add(new Vector3(-0.69f, -0.89f, zOffset));
		list.Add(new Vector3(0.69f, -0.89f, zOffset));
		weaponSlotsMap.Add(HullType.ARMAGEDDON, list);
		
		list = new List<Vector3>();
		list.Add(new Vector3(-0.61f, 0.225f, zOffset));
		list.Add(new Vector3(0.61f, 0.225f, zOffset));
		list.Add(new Vector3(0, -0.69f, zOffset));
		weaponSlotsMap.Add(HullType.ASTERIX, list);
		
		list = new List<Vector3>();
		list.Add(new Vector3(-0.57f, -0.37f, zOffset));
		list.Add(new Vector3(0.57f, -0.37f, zOffset));
		weaponSlotsMap.Add(HullType.BUFFALO, list);
		
		list = new List<Vector3>();
		list.Add(new Vector3(-0.59f, -0.3f, zOffset));
		list.Add(new Vector3(0.59f, -0.3f, zOffset));
		weaponSlotsMap.Add(HullType.CORVETTE, list);
		
		list = new List<Vector3>();
		list.Add(new Vector3(0, -0.39f, zOffset));
		weaponSlotsMap.Add(HullType.CRICKET, list);
		
		list = new List<Vector3>();
		list.Add(new Vector3(-0.775f, 0.24f, zOffset));
		list.Add(new Vector3(0.775f, 0.24f, zOffset));
		list.Add(new Vector3(-0.765f, -0.9f, zOffset));
		list.Add(new Vector3(0.765f, -0.9f, zOffset));
		weaponSlotsMap.Add(HullType.DREADNAUT, list);
		
		list = new List<Vector3>();
		list.Add(new Vector3(0, -0.545f, zOffset));
		weaponSlotsMap.Add(HullType.FALCON, list);
		
		list = new List<Vector3>();
		list.Add(new Vector3(-0.575f, -0.425f, zOffset));
		list.Add(new Vector3(0.575f, -0.425f, zOffset));
		weaponSlotsMap.Add(HullType.LEGIONNAIRE, list);
		
		list = new List<Vector3>();
		list.Add(new Vector3(-0.64f, 0.26f, zOffset));
		list.Add(new Vector3(0.64f, 0.26f, zOffset));
		list.Add(new Vector3(0, -0.64f, zOffset));
		weaponSlotsMap.Add(HullType.PRIME, list);
		
		list = new List<Vector3>();
		list.Add(new Vector3(-0.575f, 0.14f, zOffset));
		list.Add(new Vector3(0.575f, 0.14f, zOffset));
		list.Add(new Vector3(0, -0.8f, zOffset));
		weaponSlotsMap.Add(HullType.STARWALKER, list);
		
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
		weaponSlotsMap.Add(HullType.WARSHIP, list);
	}

	private void initializeEngineMap () {
		engineMap = new Dictionary<HullType, Vector3>();
		engineMap.Add(HullType.LITTLE, new Vector3(0, -0.42f, 0));
		engineMap.Add(HullType.NEEDLE, new Vector3(0, -0.57f, 0));
		engineMap.Add(HullType.GNOME, new Vector3(0, -0.61f, 0));
		engineMap.Add(HullType.CRICKET, new Vector3(0, -0.725f, 0));
		engineMap.Add(HullType.ARGO, new Vector3(0, -0.805f, 0));
		engineMap.Add(HullType.FALCON, new Vector3(0, -0.99f, 0));
		engineMap.Add(HullType.ADVENTURER, new Vector3(0, -0.975f, 0));
		engineMap.Add(HullType.CORVETTE, new Vector3(0, -0.985f, 0));
		engineMap.Add(HullType.BUFFALO, new Vector3(0, -1.035f, 0));
		engineMap.Add(HullType.LEGIONNAIRE, new Vector3(0, -1.08f, 0));
		engineMap.Add(HullType.STARWALKER, new Vector3(0, -1.1f, 0));
		engineMap.Add(HullType.WARSHIP, new Vector3(0, -1.135f, 0));
		engineMap.Add(HullType.ASTERIX, new Vector3(0, -1.15f, 0));
		engineMap.Add(HullType.PRIME, new Vector3(0, -1.11f, 0));
		engineMap.Add(HullType.TITAN, new Vector3(0, -1.11f, 0));
		engineMap.Add(HullType.DREADNAUT, new Vector3(0, -1.18f, 0));
		engineMap.Add(HullType.ARMAGEDDON, new Vector3(0, -1.33f, 0));
	}

	private void initializeColliderMap () {
		colliderMap = new Dictionary<HullType, Vector3>();
		colliderMap.Add(HullType.LITTLE, new Vector3(-0.07f, 0.6f, 1));
		colliderMap.Add(HullType.NEEDLE, new Vector3(-0.06f, 0.5f, 1.3f));
		colliderMap.Add(HullType.GNOME, new Vector3(-0.065f, 0.55f, 1.37f));
		colliderMap.Add(HullType.CRICKET, new Vector3(-0.07f, 0.7f, 1.6f));
		colliderMap.Add(HullType.ARGO, new Vector3(-0.03f, 0.7f, 1.8f));
		colliderMap.Add(HullType.FALCON, new Vector3(-0.06f, 0.8f, 2.15f));
		colliderMap.Add(HullType.ADVENTURER, new Vector3(-0.06f, 0.9f, 2.15f));
		colliderMap.Add(HullType.CORVETTE, new Vector3(-0.06f, 1, 2.15f));
		colliderMap.Add(HullType.BUFFALO, new Vector3(-0.067f, 0.9f, 2.23f));
		colliderMap.Add(HullType.LEGIONNAIRE, new Vector3(-0.07f, 0.9f, 2.32f));
		colliderMap.Add(HullType.STARWALKER, new Vector3(-0.07f, 0.95f, 2.32f));
		colliderMap.Add(HullType.WARSHIP, new Vector3(-0.07f, 0.9f, 2.42f));
		colliderMap.Add(HullType.ASTERIX, new Vector3(-0.07f, 1, 2.46f));
		colliderMap.Add(HullType.PRIME, new Vector3(-0.06f, 1.2f, 2.4f));
		colliderMap.Add(HullType.TITAN, new Vector3(-0.06f, 1.4f, 2.4f));
		colliderMap.Add(HullType.DREADNAUT, new Vector3(-0.04f, 1.8f, 2.58f));
		colliderMap.Add(HullType.ARMAGEDDON, new Vector3(-0.07f, 1.8f, 2.8f));
	}

	private void initializeExhaustMap () {
		exhaustMap = new Dictionary<HullType, Vector3>();
		exhaustMap.Add(HullType.LITTLE, new Vector3(-.41f, .17f, .6f));
		exhaustMap.Add(HullType.NEEDLE, new Vector3(-.41f, -.06f, .74f));
		exhaustMap.Add(HullType.GNOME, new Vector3(-.4f, .24f, .78f));
		exhaustMap.Add(HullType.CRICKET, new Vector3(-.43f, .22f, .9f));
		exhaustMap.Add(HullType.ARGO, new Vector3(-.45f, .34f, 1.02f));
		exhaustMap.Add(HullType.FALCON, new Vector3(-.62f, .22f, .7f));
		exhaustMap.Add(HullType.ADVENTURER, new Vector3(-.6f, .42f, 1.16f));
		exhaustMap.Add(HullType.CORVETTE, new Vector3(-.47f, .57f, 1.17f));
		exhaustMap.Add(HullType.BUFFALO, new Vector3(-.57f, .6f, 1.02f));
		exhaustMap.Add(HullType.LEGIONNAIRE, new Vector3(-.52f, .58f, 1.25f));
		exhaustMap.Add(HullType.STARWALKER, new Vector3(-.51f, .61f, 1.25f));
		exhaustMap.Add(HullType.WARSHIP, new Vector3(-.58f, .54f, 1.3f));
		exhaustMap.Add(HullType.ASTERIX, new Vector3(-.45f, .6f, 1.31f));
		exhaustMap.Add(HullType.PRIME, new Vector3(-.57f, .8f, 1.3f));
		exhaustMap.Add(HullType.TITAN, new Vector3(-.78f, .72f, 1.3f));
		exhaustMap.Add(HullType.DREADNAUT, new Vector3(-.92f, .6f, 1.41f));
		exhaustMap.Add(HullType.ARMAGEDDON, new Vector3(-1.06f, .83f, 1.47f));
	}

	public abstract bool isPlayerShip ();
}