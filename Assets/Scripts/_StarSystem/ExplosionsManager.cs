using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ExplosionsManager : MonoBehaviour {

	public Transform explosionPrefab, explosionBigPrefab;

	private static Transform trans;

	private static Transform explosionTrans, explosionBigTrans;

	private static List<Explosion> bigExplosions = new List<Explosion>();

	private static List<Explosion> smallExplosions = new List<Explosion>();

	public void init () {
		trans = transform;
		explosionTrans = explosionPrefab;
		explosionBigTrans = explosionBigPrefab;
	}

	public static void playExplosion (EnemyShip ship) {
		Explosion expl = getExplosion();
		expl.transform.position = ship.transform.position;
		expl.gameObject.SetActive(true);
		expl.play();
	}

	private static Explosion getExplosion () {
		foreach (Explosion expl in bigExplosions) {
			if (!expl.onScene) { return expl; }
		}
		Explosion explosion = Instantiate<Transform>(explosionBigTrans).GetComponent<Explosion>();
		explosion.transform.parent = trans;
		explosion.init();
		bigExplosions.Add(explosion);
		return explosion;
	}
}