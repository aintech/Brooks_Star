using UnityEngine;
using System.Collections;

public class LootContainer : MonoBehaviour {

	private Transform trans;

	private Color32 color = new Color32(255, 255, 255, 255);

	private SpriteRenderer render;

	private float disapearTimer, disapearInterval = 30;

	public bool onScene;

	private BoxCollider2D coll;

	private bool appear, disappear;

	private LootDisplay lootDisplay;

	public ItemData[] loot { get; private set; }

	public LootContainer init (LootDisplay lootDisplay) {
		this.lootDisplay = lootDisplay;
		trans = transform;
		render = GetComponent<SpriteRenderer>();
		coll = GetComponent<BoxCollider2D>();
		loot = new ItemData[6];
		return this;
	}

	void Update () {
		if (StarSystem.gamePaused) { return; }

		if (onScene) {
			if (appear) {
				color.a += 2;
				if (color.a > 250) {
					appear = false;
					updateDisapear();
				}
				render.color = color;
			} else {
				if (!disappear && disapearTimer < Time.time) {
					disappear = true;
				}
				if (disappear) {
					color.a--;
					if (color.a <= 10) {
						hideDrop();
					}
					render.color = color;
				}
				if (Input.GetMouseButtonDown(0) && Utils.hit != null && Utils.hit == coll) {
					lootDisplay.showDisplay(this);
				}
			}
		}
	}

	public void initDrop (Ship ship) {
		color.a = 0;
		render.color = color;
		onScene = true;
		appear = true;
		disappear = false;
		trans.position = ship.transform.position;
		calculateDrop(ship);
		gameObject.SetActive(true);
	}

	private void calculateDrop (Ship ship)  {
		loot[0] = ItemFactory.createWeaponData(WeaponType.BLASTER);
		loot[1] = ItemFactory.createGoodsData(Random.Range(5, 20));
	}

	public void updateDisapear () {
		disapearTimer = Time.time + disapearInterval;
		color.a = 255;
		render.color = color;
	}

	public void hideDrop () {
		onScene = false;
		for (int i = 0; i < loot.Length; i++) {
			loot[i] = null;
		}
		gameObject.SetActive(false);
	}
}