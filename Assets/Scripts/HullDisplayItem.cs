using UnityEngine;
using System.Collections;

public class HullDisplayItem : MonoBehaviour {

	public Sprite[] hullIcons;

	private SpriteRenderer render;

	private HullType hullType;

	private HullDisplayCell cell;

	void Awake () {
		if (render == null) render = transform.GetComponent<SpriteRenderer>();
	}

	private void setSprite () {
		switch (hullType) {
			case HullType.Little: render.sprite = hullIcons[0]; break;
			case HullType.Needle: render.sprite = hullIcons[1]; break;
			case HullType.Gnome: render.sprite = hullIcons[2]; break;
			case HullType.Cricket: render.sprite = hullIcons[3]; break;
			case HullType.Argo: render.sprite = hullIcons[4]; break;
			case HullType.Falcon: render.sprite = hullIcons[5]; break;
			case HullType.Adventurer: render.sprite = hullIcons[6]; break;
			case HullType.Corvette: render.sprite = hullIcons[7]; break;
			case HullType.Buffalo: render.sprite = hullIcons[8]; break;
			case HullType.Legionnaire: render.sprite = hullIcons[9]; break;
			case HullType.StarWalker: render.sprite = hullIcons[10]; break;
			case HullType.Warship: render.sprite = hullIcons[11]; break;
			case HullType.Asterix: render.sprite = hullIcons[12]; break;
			case HullType.Prime: render.sprite = hullIcons[13]; break;
			case HullType.Titan: render.sprite = hullIcons[14]; break;
			case HullType.Dreadnaut: render.sprite = hullIcons[15]; break;
			case HullType.Armageddon: render.sprite = hullIcons[16]; break;
		}
	}

	public void setHullType (HullType hullType) {
		this.hullType = hullType;
		setSprite ();
	}

	public HullType getHullType () {
		return hullType;
	}

	public void setCell (HullDisplayCell cell) {
		this.cell = cell;
	}

	public HullDisplayCell getCell () {
		return cell;
	}
}