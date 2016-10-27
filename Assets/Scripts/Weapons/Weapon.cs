using UnityEngine;
using System.Collections;

public abstract class Weapon : MonoBehaviour {

	protected Transform trans;

	protected Transform playerTrans;

	private WeaponType weaponType;

	private int minDamage, maxDamage;

	protected Animator anim;

	protected float lastShotTime, reloadTime;

	private Camera mainCamera;

	private float targetToWeaponX, targetToWeaponY, degreeTargetToWeapon;

	private Vector3 weaponRotation = Vector3.zero;

	protected bool isAPlayerWeapon = false;

	private SpriteRenderer render;
	
	protected static int enemyLayer = -1, playerLayer = -1;

	private Ship ship;

	private bool active = true;

	private Quaternion idle = new Quaternion();

	private Vector3 idleVec = new Vector3(0, 0, 180);

	private Vector3 rotVec = Vector3.zero;

	private int normalZ;

	public void init (Ship ship) {
		this.ship = ship;
		if (anim == null) {
			anim = GetComponent<Animator>();
			mainCamera = Camera.main;
			trans = transform;
			render = trans.GetComponent<SpriteRenderer>();
		}
		if (enemyLayer == -1) {
			enemyLayer = LayerMask.NameToLayer("EnemyLayer");
			playerLayer = LayerMask.NameToLayer("PlayerLayer");
		}
		idle.eulerAngles = idleVec;
	}

	private void Update () {
		if (StarSystem.gamePaused || !ship.alive || !active) { return; }

		if (isAPlayerWeapon) {
			if (Input.GetMouseButton(0) && canShoot()) {
				lastShotTime = Time.time;
				makeAShot ();
			}
		} else {
			if (canShoot() && distanceInRange()) {
				lastShotTime = Time.time;
				makeAShot ();
			}
		}
	}

	private bool canShoot () {
		return (lastShotTime + reloadTime) <= Time.time;
	}

	private bool distanceInRange () {
		return Vector2.Distance(playerTrans.position, trans.position) < getWeaponType().range();
	}

	private void FixedUpdate () {
		if (StarSystem.gamePaused || !ship.alive) { return; }

		if (active) {
			weaponLookAtTarget(isAPlayerWeapon? mainCamera.ScreenToWorldPoint(Input.mousePosition): playerTrans.position);
		} else if (normalZ != 180) {
			rotVec = Vector3.Lerp(trans.localRotation.eulerAngles, idleVec, .1f);
			normalZ = (int) rotVec.z;
			idle.eulerAngles = rotVec;
			trans.localRotation = idle;
		}
	}

	private void weaponLookAtTarget (Vector3 target) {
		targetToWeaponX = target.x - trans.position.x;
		targetToWeaponY = target.y - trans.position.y;
		degreeTargetToWeapon = Mathf.Atan2(targetToWeaponY, targetToWeaponX) * Mathf.Rad2Deg;
		degreeTargetToWeapon -= (degreeTargetToWeapon >= 360) ? 270 : 90;
		weaponRotation.z = degreeTargetToWeapon - trans.rotation.eulerAngles.z;
		trans.Rotate(weaponRotation);
	}

	virtual protected void makeAShot () { anim.SetTrigger("shot"); }
	
	public void setAsPlayerWeapon () {
		isAPlayerWeapon = true;
		transform.GetComponent<SpriteRenderer>().sortingLayerName = "Player";
	}

	public void setWeaponType (WeaponType weaponType) { this.weaponType = weaponType; }
	public WeaponType getWeaponType () { return weaponType; }

	public void setDamage (int minDamage, int maxDamage) {
		this.minDamage = minDamage;
		this.maxDamage = maxDamage;
	}
	public int getMinDamage () { return minDamage; }
	public int getMaxDamage () { return maxDamage; }

	public void setReloadTime (float reloadTime) { this.reloadTime = reloadTime; }
	public float getReloadTime () { return reloadTime; }

	public void setPlayerTransform (Transform playerTrans) { this.playerTrans = playerTrans; }

	public void prepareToJump () {
		active = false;
		rotVec = Vector3.zero;
		normalZ = 0;
	}

	public void activateWeapon () {
		active = true;
	}

	public SpriteRenderer getRender () {
		return render;
	}
}