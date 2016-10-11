using UnityEngine;
using System.Collections;

public class Explosion : MonoBehaviour {

	public bool isFinal;

	public bool onScreen { get; private set; }

	private ExplosionsPack pack;

	public Explosion init (ExplosionsPack pack) {
		this.pack = pack;
		return this;
	}

	public void play (Vector2 pos) {
		onScreen = true;
		transform.position = pos;
		gameObject.SetActive(true);
	}

	public void stop () {
		onScreen = false;
		if (isFinal) { pack.finishPack(); }
		gameObject.SetActive(false);
	}
}