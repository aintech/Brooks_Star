using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LootContainer : MonoBehaviour {

	private Transform trans;

	private Color32 color = new Color32(255, 255, 255, 255);

	private SpriteRenderer render;

	private float disapearTimer, disapearInterval = 30;

	public bool onScene;

	private BoxCollider2D coll;

	private bool appear, disappear;

	private LootDisplay lootDisplay;

	public List<Item> loot { get; private set; }

	public LootContainer init (LootDisplay lootDisplay) {
		this.lootDisplay = lootDisplay;
		trans = transform;
		render = GetComponent<SpriteRenderer>();
		coll = GetComponent<BoxCollider2D>();
		loot = new List<Item>();
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
		if (calculateDrop(ship)) {
			color.a = 0;
			render.color = color;
			onScene = true;
			appear = true;
			disappear = false;
			trans.position = ship.transform.position;
			gameObject.SetActive(true);
		} else {
			hideDrop();
		}
	}

	private bool calculateDrop (Ship ship)  {
		if (Random.value <= .5f) {
			ItemType dropType = ItemTypeDescriptor.dropables()[Random.Range(0, ItemTypeDescriptor.dropables().Length)];
			loot.Add(Instantiate<Transform>(ItemFactory.itemPrefab).GetComponent<Item>().init(ItemFactory.createItemData(dropType)));
		}
		if (Random.value <= .75f) {
			loot.Add(Instantiate<Transform>(ItemFactory.itemPrefab).GetComponent<Item>().init(ItemFactory.createGoodsData(Random.Range(5, 20))));
		}
		if (Random.value <= .25f) {
			loot.Add(Instantiate<Transform>(ItemFactory.itemPrefab).GetComponent<Item>().init(ItemFactory.createGoodsData(Random.Range(5, 20))));
		}
		foreach (Item item in loot) {
			item.gameObject.SetActive(false);
		}
		return loot.Count > 0;
	}

	public void updateDisapear () {
		disapearTimer = Time.time + disapearInterval;
		color.a = 255;
		render.color = color;
	}

	public void hideDrop () {
		onScene = false;
		loot.Clear();
		if (gameObject != null) { gameObject.SetActive(false); }
	}
}