using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ExplosionsManager : MonoBehaviour {

	public Transform explosionsPackPrefab;

	private static Transform trans, explosionsPack;

	private static List<ExplosionsPack> explosionsPacks = new List<ExplosionsPack>();

	public ExplosionsManager init () {
		trans = transform;
		explosionsPack = explosionsPackPrefab;
		return this;
	}

	public static void playExplosion (Ship ship) {
		ExplosionsPack pack = getPack();
		pack.play(ship);
	}

	private static ExplosionsPack getPack () {
		foreach (ExplosionsPack ep in explosionsPacks) {
			if (!ep.onScene) { return ep; }
		}
		ExplosionsPack pack = Instantiate<Transform>(explosionsPack).GetComponent<ExplosionsPack>();
		pack.transform.parent = trans;
		pack.init();
		explosionsPacks.Add(pack);
		return pack;
	}

	public static void clear () {
		explosionsPacks.Clear();
	}
}