using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ShieldsPool : MonoBehaviour {

	public Transform shieldRenderPrefab;

	private static List<Transform> shieldsList = new List<Transform>();

	private SpriteRenderer render;

	public void renderShieldReflection (Ship ship, Vector3 weaponPosition) {
		Transform shield = null;

		bool found = false;
		foreach (Transform shld in shieldsList) {
			if (!shld.gameObject.activeInHierarchy) {
				shield = shld;
				found = true;
				break;
			}
		}
		if (!found) {
			shield = Instantiate<Transform>(shieldRenderPrefab);
			shieldsList.Add(shield);
		}

		if (ship.isAPlayerShip()) {
			shield.GetComponent<SpriteRenderer>().sortingLayerName = "Player";
		} else {
			shield.GetComponent<SpriteRenderer>().sortingLayerName = "Enemy";
		}

		shield.GetComponent<SpriteRenderer>().sortingOrder = ship.getShieldRenderOrder();
		shield.transform.position = ship.transform.position;
		shield.SetParent(ship.transform);

		Vector3 diff = weaponPosition - shield.transform.position;
		diff.Normalize();
		float rot_z = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
		shield.transform.rotation = Quaternion.Euler(0f, 0f, rot_z - 90);

		shield.gameObject.SetActive(true);
	}

	void FixedUpdate () {
		foreach (Transform shield in shieldsList) {
			if (shield.gameObject.activeInHierarchy) {
				render = shield.GetComponent<SpriteRenderer>();
				if (render.color.a <= 0) {
					render.color = new Color(1, 1, 1, 1);
					shield.transform.rotation = new Quaternion();
					shield.gameObject.SetActive(false);
				} else {
					render.color = new Color(1, 1, 1, render.color.a - .2f);
				}
			}
		}
	}
}