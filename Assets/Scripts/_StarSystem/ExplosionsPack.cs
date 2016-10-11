using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ExplosionsPack : MonoBehaviour {

	public bool onScene { get; private set; }

	private Explosion finalExplosion;

	private List<Explosion> explosions = new List<Explosion>();

	private int counter;

	private int toNextTimer;

	private Ship ship;

	private const int minOffset = 10, maxOffset = 30;

	private float explRad;

	public void init () {
		Explosion explosion;
		for (int i = 0; i < transform.childCount; i++) {
			explosion = transform.GetChild (i).GetComponent<Explosion> ().init(this);
			if (explosion.isFinal) {
				finalExplosion = explosion;
			} else {
				explosions.Add (explosion);
			}
		}
	}

	void Update () {
		if (toNextTimer > 0) {
			toNextTimer--;
		} else if (toNextTimer != -100) {
			if (counter == 0) {
				toNextTimer = -100;
				finalExplosion.play(ship.transform.position);
				ship.destroyShip ();
			} else {
				playNext ();
				toNextTimer = Random.Range (minOffset, maxOffset);
			}
		}
	}

	private void playNext () {
		foreach (Explosion expl in explosions) {
			if (!expl.onScreen) {
				expl.play (new Vector3(ship.transform.position.x + Random.Range(-explRad, explRad), ship.transform.position.y + Random.Range(-explRad, explRad), ship.transform.position.z));
				counter--;
				return;
			}
		}
	}

	public void play (Ship ship) {
		this.ship = ship;
		explRad = (ship.getHullType().getHullClass() + 1) * .2f;
		counter = ship.getHullType().getHullClass() + 1;
		toNextTimer = Random.Range (minOffset, maxOffset);
		onScene = true;
		gameObject.SetActive(true);
	}

	public void finishPack () {
		gameObject.SetActive (false);
		onScene = false;
	}
}