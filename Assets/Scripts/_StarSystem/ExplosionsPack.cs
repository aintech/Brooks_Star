using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ExplosionsPack : MonoBehaviour {

	public bool onScene { get; private set; }

	private Explosion finalExplosion;

	private List<Explosion> explosions = new List<Explosion>(6);

	private int counter;

	private int toNextTimer;

	private Ship ship;

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
		} else {
			if (counter < 1) {
				toNextTimer = -1;
				return;
			}
			playNext ();
			toNextTimer = Random.Range (10, 50);
		}
	}

	private void playNext () {
		foreach (Explosion expl in explosions) {
			if (!expl.onScreen) {
				expl.play ();
				return;
			}
		}
	}

	public void play (Ship ship) {
		this.ship = ship;
		counter = ship.getHullType().getHullClass() + 2;
		toNextTimer = Random.Range (10, 50);
		downCount ();
	}

	public void downCount () {
		counter--;
		if (counter < 0) {
			gameObject.SetActive (false);
			onScene = false;
			ship.destroyShip ();
		} else if (counter == 0) {
			finalExplosion.play ();
		}
	}
}