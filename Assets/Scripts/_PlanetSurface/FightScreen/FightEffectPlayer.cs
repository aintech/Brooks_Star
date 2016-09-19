using UnityEngine;
using System.Collections;

public class FightEffectPlayer : MonoBehaviour {

	public Sprite //armorLightSprite, armorModerateSprite, armorHeavySprite, armorUltraSprite,
				  //medkitSmallSprite, medkitMediumSprite, medkitLargeSprite, medkitSuperiorSprite,
				  damageSprite;

	private Transform bg, effectImage;

	private SpriteRenderer effectRender;

	private TextMesh effectTxt, effectTxtBG;

	private Vector3 scale = Vector3.one;

	private float scaleSpeed = .05f;

	private bool scaling, scaleBack;

	private bool effectIsPlaying;

	private Color red = new Color(1, 0, 0, 1), alfa = new Color(1, 1, 1, 1);//, green = new Color(0, 1, 0, 1), blue = new Color(0, 0, 1, 1)

    private Quaternion idleRot = new Quaternion();

	private Vector3 bgRotSpeed = new Vector3(0, 0, -2);

	private float playStart, playTime = .5f;

	private bool damageEffect;

	private float damageAppearTime, damageEffectDuration = .2f;

	public FightEffectPlayer init () {
		bg = transform.FindChild("BG");
		bg.GetComponent<SpriteRenderer>().enabled = true;
		effectImage = transform.FindChild("EffectImage");
		effectRender = effectImage.GetComponent<SpriteRenderer>();
		effectTxt = transform.FindChild("ValueTxt").GetComponent<TextMesh>();
		effectTxtBG = transform.FindChild("ValueTxtBG").GetComponent<TextMesh>();
		MeshRenderer rend = effectTxt.transform.GetComponent<MeshRenderer>();
		rend.sortingLayerName = "FightEffectLayer";
		rend.sortingOrder = 4;
		rend = effectTxtBG.transform.GetComponent<MeshRenderer>();
		rend.sortingLayerName = "FightEffectLayer";
		rend.sortingOrder = 3;
		bg.gameObject.SetActive(false);
		effectTxt.gameObject.SetActive(false);

		gameObject.SetActive(true);

		return this;
	}

	void Update () {
		if (effectIsPlaying) {
			if (damageEffect) {
				if (!scaleBack) {
					scaleBack = true;
					damageAppearTime = Time.time + damageEffectDuration;
				} else {
					if (damageAppearTime < Time.time) {
						alfa.a -= .03f;
						if (alfa.a <= 0) {
							alfa.a = 1;
							endPlay();
						}
					}
				}
				effectRender.color = alfa;
			} else {
				bg.Rotate(bgRotSpeed);
				if (scaling && scale.x >= 1) {
					scaling = false;
					playStart = Time.time;
				}
				if (scaling) {
					scale.x += scaleSpeed;
					scale.y += scaleSpeed;
					effectImage.localScale = scale;
				} else if (!scaleBack) {
					if (Time.time > playStart + playTime) {
						scaleBack = true;
					}
				} else {
					if (scale.x <= 0) {
						endPlay();
					}
					scale.x -= scaleSpeed * 2;
					scale.y -= scaleSpeed * 2;
					effectImage.localScale = scale;
				}
			}
		}
	}

	/*
	 * Вместо медкита и армора - эффект от разных зелий
	 */

//	public void playMedkitEffect (MedkitElementType type, int value) {
//		switch(type) {
//			case MedkitElementType.SMALL: effectRender.sprite = medkitSmallSprite; break;
//			case MedkitElementType.MEDIUM: effectRender.sprite = medkitMediumSprite; break;
//			case MedkitElementType.LARGE: effectRender.sprite = medkitLargeSprite; break;
//			case MedkitElementType.SUPERIOR: effectRender.sprite = medkitSuperiorSprite; break;
//		}
//		effectTxt.color = green;
//		effectTxt.text = "+" + value;
//		effectSetup();
//	}
//
//	public void playArmorEffect (ArmorElementType type, int value) {
//		switch (type) {
//			case ArmorElementType.LIGHT: effectRender.sprite = armorLightSprite; break;
//			case ArmorElementType.MODERATE: effectRender.sprite = armorModerateSprite; break;
//			case ArmorElementType.HEAVY: effectRender.sprite = armorHeavySprite; break;
//			case ArmorElementType.ULTRA: effectRender.sprite = armorUltraSprite; break;
//		}
//
//		effectTxt.color = blue;
//		effectTxt.text = "+" + value;
//		effectSetup();
//	}

	private void effectSetup () {
		if (!damageEffect) {
			scaling = true;
			
			scale.x = .1f;
			scale.y = .1f;
			
			effectImage.localScale = scale;
		}
		effectIsPlaying = true;
		effectTxtBG.text = effectTxt.text;

		bg.gameObject.SetActive(!damageEffect);
		effectImage.gameObject.SetActive(true);
		effectTxt.gameObject.SetActive(true);
		effectTxtBG.gameObject.SetActive(true);
	}

	public void playEffect (FightEffectType type, int value) {
		if (type == FightEffectType.DAMAGE) {
			damageEffect = true;
			effectRender.sprite = damageSprite;
			effectTxt.color = red;
			effectTxt.text = "-" + value;
		}
		effectSetup();
	}

	private void endPlay () {
		effectIsPlaying = false;
		damageEffect = false;
		scaleBack = false;
		scale.x = scale.y = 1;
		effectImage.localScale = scale;
		bg.localRotation = idleRot;
		bg.gameObject.SetActive(false);
		effectImage.gameObject.SetActive(false);
		effectTxt.gameObject.SetActive(false);
		effectTxtBG.gameObject.SetActive(false);
		FightProcessor.FIGHT_ANIM_PLAYER_DONE = true;
	}
}