using UnityEngine;
using System.Collections;

public class Explosion : MonoBehaviour {

	public bool isFinal;

	public bool onScreen { get; private set; }

	private Animator anim;

	private ExplosionsPack pack;

	public Explosion init (ExplosionsPack pack) {
		this.pack = pack;
		anim = GetComponent<Animator>();
		return this;
	}

	public void play () {
		onScreen = true;
		transform.localPosition = new Vector3(Random.Range (-1f, 1f), Random.Range (-1f, 1f), transform.position.z);
		gameObject.SetActive(true);
	}

	public void stop () {
		onScreen = false;
		pack.downCount ();
		gameObject.SetActive(false);
	}
}