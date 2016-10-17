using UnityEngine;
using System.Collections;

public class Blaster : Weapon {
	
	private Vector3 mouseToWorldPosition, direct;

	private RaycastHit2D hit;

	override protected void makeAShot () {
		base.makeAShot();
		if (isAPlayerWeapon) {
			mouseToWorldPosition = Camera.main.ScreenToWorldPoint (Input.mousePosition);
			direct = (mouseToWorldPosition - trans.position).normalized;
			hit = Physics2D.Raycast(trans.position, direct, WeaponType.BLASTER.range(), 1 << (isAPlayerWeapon? enemyLayer: playerLayer));
			
			if (hit.collider != null && hit.collider.GetComponent<Ship>() != null) {
				hit.collider.GetComponent<Ship>().damageShip(getWeaponType(), trans.position, getMinDamage(), getMaxDamage());
			}
		} else {
			playerTrans.GetComponent<Ship>().damageShip(getWeaponType(), trans.position, getMinDamage(), getMaxDamage());
		}
	}
}