using UnityEngine;
using System.Collections;

public class EnemyShipController : ShipController {

	private Transform playerShip;

	private float distanceToPlayer, pursueDistance = 5, closestDistance = 3;

//	private Weapon weapon_1, weapon_2, weapon_3, weapon_4, weapon_5;

	private float radarRange;

	private Transform barTrans;

	private Vector3 barOffset = new Vector3(1, 1);

	private Vector2 toPlayerDirection = Vector2.zero, lookVector = Vector2.up;

	private float toPlayerAngle;

	private bool isPursue;

	public void setStuff (Transform playerShip, Transform barTrans, float radarRange, Weapon[] weapons) {
		this.playerShip = playerShip;
		this.barTrans = barTrans;
		this.radarRange = radarRange;
//		weapon_1 = weapons[0];
//		weapon_2 = weapons[1];
//		weapon_3 = weapons[2];
//		weapon_4 = weapons[3];
//		weapon_5 = weapons[4];
		arrangeBarToShip();
	}

	protected override void decideNextMove () {
		distanceToPlayer = Vector2.Distance(trans.position, playerShip.position);

		if (distanceToPlayer <= radarRange) {
			if (isPursue && distanceToPlayer < closestDistance) {
				isPursue = false;
			} else if (!isPursue && distanceToPlayer > pursueDistance) {
				isPursue = true;
			}
			if (isPursue) { moveShip(); }
			else if (accelarate || turnLeft || turnRight) { accelarate = turnLeft = turnRight = false; }

			arrangeBarToShip();
//			float maxRange = 0;
//			if (weapon_1 != null && weapon_1.getWeaponType().getRange() > maxRange) maxRange = weapon_1.getWeaponType().getRange();
//			if (weapon_2 != null && weapon_2.getWeaponType().getRange() > maxRange) maxRange = weapon_2.getWeaponType().getRange();
//			if (weapon_3 != null && weapon_3.getWeaponType().getRange() > maxRange) maxRange = weapon_3.getWeaponType().getRange();
//			if (weapon_4 != null && weapon_4.getWeaponType().getRange() > maxRange) maxRange = weapon_4.getWeaponType().getRange();
//			if (weapon_5 != null && weapon_5.getWeaponType().getRange() > maxRange) maxRange = weapon_5.getWeaponType().getRange();
//
//			moveShip();
		} else if (accelarate || turnLeft || turnRight) { accelarate = turnLeft = turnRight = false; }
	}

	private void moveShip () {
		accelarate = true;
		turnControl();
	}

	private void turnControl () {
		lookVector.x = Mathf.Sin(Mathf.Deg2Rad * trans.rotation.eulerAngles.z);
		lookVector.y = Mathf.Cos(Mathf.Deg2Rad * trans.rotation.eulerAngles.z);
		toPlayerDirection = (playerShip.position - trans.position).normalized;
		toPlayerDirection.x *= -1;
		toPlayerAngle = Vector2.Angle(lookVector, toPlayerDirection);
		if (toPlayerAngle > 50) {
			bool onLeftSide = Vector3.Cross(lookVector, toPlayerDirection).z < 0;
			turnLeft = onLeftSide;
			turnRight = !onLeftSide;
		} else if (toPlayerAngle < 40) {
			turnLeft = turnRight = false;
		}
	}

	private void arrangeBarToShip () {
		barTrans.position = trans.position + barOffset;
	}
}