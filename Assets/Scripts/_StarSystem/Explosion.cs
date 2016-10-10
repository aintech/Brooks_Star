using UnityEngine;
using System.Collections;

public class Explosion : MonoBehaviour {

	public bool onScene { get; private set; }

	private Animator anim;

	public void init () {
		anim = GetComponent<Animator>();
	}

	public void play () {
		onScene = true;
		gameObject.SetActive(true);
	}

	public void stop () {
		onScene = false;
		gameObject.SetActive(false);
	}
}