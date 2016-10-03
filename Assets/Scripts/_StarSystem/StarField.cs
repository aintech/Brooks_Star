using UnityEngine;
using System.Collections;

public class StarField : MonoBehaviour {

	private Transform trans;

	private SpriteRenderer render;

	private Vector3 pos;

	public const int zOffset = -2;

	public StarField init () {
		trans = transform;
		render = GetComponent<SpriteRenderer>();
		return this;
	}

	public void initStarField () {
		render.sprite = Imager.getStarSystem(Vars.starSystemType);
	}

	public void adjustStarField (Vector3 pos) {
		this.pos.Set(pos.x * .95f, pos.y * .95f, zOffset);
		trans.localPosition = this.pos;
	}
}