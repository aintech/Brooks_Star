using UnityEngine;
using System.Collections;

public class Perk : MonoBehaviour {

	public Sprite bg, bgActive;

	public PerkType perkType;

	private SpriteRenderer bgRender;

	private Transform expBar;

	private Vector3 barScale = Vector3.one;

	private StrokeText levelText;

	public Perk init () {
		bgRender = transform.GetComponent<SpriteRenderer>();
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

	public void setAsChoosed (bool choosed) {
		bgRender.sprite = choosed? bgActive: bg;
	}
}