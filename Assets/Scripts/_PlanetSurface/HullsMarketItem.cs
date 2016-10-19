using UnityEngine;
using System.Collections;

public class HullsMarketItem : MonoBehaviour {

	public Sprite[] hullIcons;

	private SpriteRenderer render;

	private HullType hullType;

	private HullsMarketCell cell;

	void Awake () {
		if (render == null) render = transform.GetComponent<SpriteRenderer>();
	}

	private void setSprite () {
		switch (hullType) {
			case HullType.LITTLE: render.sprite = hullIcons[0]; break;
			case HullType.NEEDLE: render.sprite = hullIcons[1]; break;
			case HullType.GNOME: render.sprite = hullIcons[2]; break;
			case HullType.CRICKET: render.sprite = hullIcons[3]; break;
			case HullType.ARGO: render.sprite = hullIcons[4]; break;
			case HullType.FALCON: render.sprite = hullIcons[5]; break;
			case HullType.ADVENTURER: render.sprite = hullIcons[6]; break;
			case HullType.CORVETTE: render.sprite = hullIcons[7]; break;
			case HullType.BUFFALO: render.sprite = hullIcons[8]; break;
			case HullType.LEGIONNAIRE: render.sprite = hullIcons[9]; break;
			case HullType.STARWALKER: render.sprite = hullIcons[10]; break;
			case HullType.WARSHIP: render.sprite = hullIcons[11]; break;
			case HullType.ASTERIX: render.sprite = hullIcons[12]; break;
			case HullType.PRIME: render.sprite = hullIcons[13]; break;
			case HullType.TITAN: render.sprite = hullIcons[14]; break;
			case HullType.DREADNAUT: render.sprite = hullIcons[15]; break;
			case HullType.ARMAGEDDON: render.sprite = hullIcons[16]; break;
		}
	}

	public void setHullType (HullType hullType) {
		this.hullType = hullType;
		setSprite ();
	}

	public HullType getHullType () {
		return hullType;
	}

	public void setCell (HullsMarketCell cell) {
		this.cell = cell;
	}

	public HullsMarketCell getCell () {
		return cell;
	}
}