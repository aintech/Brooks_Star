using UnityEngine;
using System.Collections;

public class Perk : MonoBehaviour {

	public PerkType perkType;

	private Transform expBar;

	private Vector3 barScale = Vector3.one;

	private StrokeText levelText;

	public Perk init () {
		expBar = transform.Find("Exp Bar");
		levelText = transform.Find("Level").GetComponent<StrokeText>().init("Inventory", 6);
		updatePerk();

		return this;
	}

	public void updatePerk () {
		levelText.setText(Player.getPerkLevel(perkType).ToString());
		barScale.y = Player.getPerkExp(perkType) / 1f;
		expBar.localScale = barScale;
	}
}