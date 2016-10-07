using UnityEngine;
using System.Collections;

public class EnemyShipController : ShipController {

	private Transform playerShip;

	private float distanceToPlayer, pursueDistance = 5, closestDistance = 1;

	private Weapon weapon_1, weapon_2, weapon_3, weapon_4, weapon_5;

	private float radarRange;

	private Transform barTrans;

	private Vector3 barOffset = new Vector3(1, 1);

	private Vector3 toPlayerDirection = Vector3.zero;

	private float toPlayerAngle;

	public void setStuff (Transform playerShip, Transform barTrans, float radarRange, Weapon[] weapons) {
		this.playerShip = playerShip;
		this.barTrans = barTrans;
		this.radarRange = radarRange;
		weapon_1 = weapons[0];
		weapon_2 = weapons[1];
		weapon_3 = weapons[2];
		weapon_4 = weapons[3];
		weapon_5 = weapons[4];
		arrangeBarToShip();
	}

	protected override void decideNextMove () {
		distanceToPlayer = Vector2.Distance(trans.position, playerShip.position);

		if (distanceToPlayer <= radarRange) {
//			if (distanceToPlayer > pursueDistance) {
				moveShip();
//			}

//			float maxRange = 0;
//			if (weapon_1 != null && weapon_1.getWeaponType().getRange() > maxRange) maxRange = weapon_1.getWeaponType().getRange();
//			if (weapon_2 != null && weapon_2.getWeaponType().getRange() > maxRange) maxRange = weapon_2.getWeaponType().getRange();
//			if (weapon_3 != null && weapon_3.getWeaponType().getRange() > maxRange) maxRange = weapon_3.getWeaponType().getRange();
//			if (weapon_4 != null && weapon_4.getWeaponType().getRange() > maxRange) maxRange = weapon_4.getWeaponType().getRange();
//			if (weapon_5 != null && weapon_5.getWeaponType().getRange() > maxRange) maxRange = weapon_5.getWeaponType().getRange();
//
//			moveShip();
		}
	}

	private void moveShip () {
//		toPlayerDirection = trans.position - playerShip.position;
		toPlayerAngle = AngleBetweenVector2(trans.position, playerShip.position);
		Debug.Log(toPlayerAngle);
//		if (toPlayerAngle > 130) {
//			turnLeft = true;
//			if (turnRight) { turnRight = false; }
//		} else if (toPlayerAngle < 100) {
//			turnRight = true;
//			if (turnLeft) { turnLeft = false; }
//		} else {
//			turnRight = turnLeft = false;
//		}
		arrangeBarToShip();
	}

	private float AngleBetweenVector2(Vector2 vec1, Vector2 vec2) {
		Vector2 diference = vec2 - vec1;
		float sign = (vec2.y < vec1.y)? -1.0f : 1.0f;
		return Vector2.Angle(Vector2.right, diference) * sign - 90;
	}

	private void arrangeBarToShip () {
		barTrans.position = trans.position + barOffset;
	}
}