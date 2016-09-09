using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Ship : MonoBehaviour {
	
	public Transform blasterPrefab, plasmerPrefab, chargerPrefab, emitterPrefab, waverPrefab, launcherPrefab, suppressorPrefab;

	public Sprite[] hullSprites;

	public Sprite[] engineSprites;

	private BoxCollider2D shipCollider;

	private SpriteRenderer hullRender, engineRender;

	protected int armor, health, shield, fullShield, fullHealth, radarRange;
	
	protected Engine engine;
	
	protected Weapon weapon_1, weapon_2, weapon_3, weapon_4, weapon_5;

	protected Weapon[] weapons;

	protected int shieldRenderOrder = 3;

	private static Dictionary<HullType, List<Vector3>> weaponSlotsMap;

	private static Dictionary<HullType, Vector3> engineMap;

	private static Dictionary<HullType, Vector3> colliderMap;

	private HullType hullType;

	protected ShieldsPool shieldsPool;

	protected bool playerShip = false;

	void Awake () {
		init ();
	}

	virtual protected void init () {
		if (weaponSlotsMap == null) {
			initializeWeaponSlotsMap();
			initializeEngineMap();
			initializeColliderMap();
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
			case HullType.Titan: hullRender.sprite = hullSprites[14]; break;
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
			case EngineType.Force: engineRender.sprite = engineSprites[0]; break;
			case EngineType.Gradual: engineRender.sprite = engineSprites[1]; break;
			case EngineType.Proton: engineRender.sprite = engineSprites[2]; break;
			case EngineType.Allur: engineRender.sprite = engineSprites[3]; break;
			case EngineType.Quazar: engineRender.sprite = engineSprites[4]; break;
			default: Debug.Log("Неизвестный тип двигателя"); break;
		}
		Vector3 pos;
		engineMap.TryGetValue(hullType, out pos);
		engine.transform.position = transform.position + pos;
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

	public int getRadarRange () {
		return radarRange;
	}

	public int getShield () {
		return shield;
	}

	public int getFullShield () {
		return fullShield;
	}
	//для теста - потом убрать
	public void damageShip (int damage) {
		if (getShield() >= damage) {
			shield = getShield() - damage;
		} else {
			int diff = damage - shield;
			shield = 0;
			health -= diff;
		}
		updateHealthAndShieldInfo();
	}

	public void damageShip (WeaponType type, Vector3 weaponPosition, int minDamage, int maxDamage) {
		if (type == WeaponType.Blaster) {
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
			destroyShip();
		}
	}

	virtual protected void updateHealthAndShieldInfo () {}
	virtual protected void destroyShip () {}

	virtual public void setGamePaused (bool gamePaused) {
		if (weapon_1 != null) { weapon_1.setGamePaused(gamePaused); }
		if (weapon_2 != null) { weapon_2.setGamePaused(gamePaused); }
		if (weapon_3 != null) { weapon_3.setGamePaused(gamePaused); }
		if (weapon_4 != null) { weapon_4.setGamePaused(gamePaused); }
		if (weapon_5 != null) { weapon_5.setGamePaused(gamePaused); }
	}

	public int getShieldRenderOrder () {
		return shieldRenderOrder;
	}

	public bool isAPlayerShip () {
		return playerShip;
	}

	protected Vector3 getWeaponPosition (int weaponIndex) {
		List<Vector3> list = new List<Vector3>();
		weaponSlotsMap.TryGetValue(hullType, out list);
		return list[weaponIndex-1];
	}

	private static void initializeWeaponSlotsMap () {
		weaponSlotsMap = new Dictionary<HullType, List<Vector3>>();
		
		List<Vector3> list = new List<Vector3>();
		list.Add(new Vector3(-0.54f, -0.43f, 0));
		list.Add(new Vector3(0.54f, -0.43f, 0));
		weaponSlotsMap.Add(HullType.Adventurer, list);
		
		list = new List<Vector3>();
		list.Add(new Vector3(0, -0.48f, 0));
		weaponSlotsMap.Add(HullType.Argo, list);
		
		list = new List<Vector3>();
		list.Add(new Vector3(-0.785f, 0.54f, 0));
		list.Add(new Vector3(0.785f, 0.54f, 0));
		list.Add(new Vector3(0, -0.11f, 0));
		list.Add(new Vector3(-0.69f, -0.89f, 0));
		list.Add(new Vector3(0.69f, -0.89f, 0));
		weaponSlotsMap.Add(HullType.Armageddon, list);
		
		list = new List<Vector3>();
		list.Add(new Vector3(-0.61f, 0.225f, 0));
		list.Add(new Vector3(0.61f, 0.225f, 0));
		list.Add(new Vector3(0, -0.69f, 0));
		weaponSlotsMap.Add(HullType.Asterix, list);
		
		list = new List<Vector3>();
		list.Add(new Vector3(-0.57f, -0.37f, 0));
		list.Add(new Vector3(0.57f, -0.37f, 0));
		weaponSlotsMap.Add(HullType.Buffalo, list);
		
		list = new List<Vector3>();
		list.Add(new Vector3(-0.59f, -0.3f, 0));
		list.Add(new Vector3(0.59f, -0.3f, 0));
		weaponSlotsMap.Add(HullType.Corvette, list);
		
		list = new List<Vector3>();
		list.Add(new Vector3(0, -0.39f, 0));
		weaponSlotsMap.Add(HullType.Cricket, list);
		
		list = new List<Vector3>();
		list.Add(new Vector3(-0.775f, 0.24f, 0));
		list.Add(new Vector3(0.775f, 0.24f, 0));
		list.Add(new Vector3(-0.765f, -0.9f, 0));
		list.Add(new Vector3(0.765f, -0.9f, 0));
		weaponSlotsMap.Add(HullType.Dreadnaut, list);
		
		list = new List<Vector3>();
		list.Add(new Vector3(0, -0.545f, 0));
		weaponSlotsMap.Add(HullType.Falcon, list);
		
		list = new List<Vector3>();
		list.Add(new Vector3(-0.575f, -0.425f, 0));
		list.Add(new Vector3(0.575f, -0.425f, 0));
		weaponSlotsMap.Add(HullType.Legionnaire, list);
		
		list = new List<Vector3>();
		list.Add(new Vector3(-0.64f, 0.26f, 0));
		list.Add(new Vector3(0.64f, 0.26f, 0));
		list.Add(new Vector3(0, -0.64f, 0));
		weaponSlotsMap.Add(HullType.Prime, list);
		
		list = new List<Vector3>();
		list.Add(new Vector3(-0.575f, 0.14f, 0));
		list.Add(new Vector3(0.575f, 0.14f, 0));
		list.Add(new Vector3(0, -0.8f, 0));
		weaponSlotsMap.Add(HullType.StarWalker, list);
		
		list = new List<Vector3>();
		list.Add(new Vector3(-0.705f, 0.24f, 0));
		list.Add(new Vector3(0.705f, 0.24f, 0));
		list.Add(new Vector3(-0.62f, -0.92f, 0));
		list.Add(new Vector3(0.62f, -0.92f, 0));
		weaponSlotsMap.Add(HullType.Titan, list);
		
		list = new List<Vector3>();
		list.Add(new Vector3(-0.575f, 0.165f, 0));
		list.Add(new Vector3(0.575f, 0.165f, 0));
		list.Add(new Vector3(0, -0.735f, 0));
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
		engineMap.Add(HullType.Titan, new Vector3(0, -1.11f, 0));
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
		colliderMap.Add(HullType.Titan, new Vector3(-0.06f, 1.4f, 2.4f));
		colliderMap.Add(HullType.Dreadnaut, new Vector3(-0.04f, 1.8f, 2.58f));
		colliderMap.Add(HullType.Armageddon, new Vector3(-0.07f, 1.8f, 2.8f));
	}
}