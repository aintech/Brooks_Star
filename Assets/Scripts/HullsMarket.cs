using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class HullsMarket : MonoBehaviour {

	public Transform hullsMarketItemPrefab;

	private ShipData shipData;

	public Sprite moveUpBtnSprite, moveDownBtnSprite, moveUpBtnSpriteDisabled, moveDownBtnSpriteDisabled, buyBtnSprite, buyBtnSpriteDisabled;

	public Sprite[] hullImages;

	private HullsMarketCell[] cells;

	private Dictionary<int, HullsMarketItem> items = new Dictionary<int, HullsMarketItem>();
	
	private SpriteRenderer moveUpBtnRender, moveDownBtnRender, buyHullBtnRender, chosenHullBorder, hullImage;
	
	private BoxCollider2D moveUpBtn, moveDownBtn, buyHullBtn;
	
	private int offset = 0;
	
	private int offsetStep = 2;

	private bool scrollableUp, scrollableDown;

	private HullsMarketItem chosenHull;

	private TextMesh hullName, hullCost, hullHealth, hullStorage,
					 weaponSlots, armorSlots, shieldSlots,
					 generatorSlots, repairDroidSlots, harvesterSlots;

	private Color blueColor = new Color(0.86f, 1, 1);

	private Color redColor = new Color(1, 0, 0);

	private Color greenColor = new Color(0, 1, 0); 

	private Inventory inventory, storage;

	public void init (Inventory inventory, Inventory storage, ShipData shipData) {
		this.inventory = inventory;
		this.storage = storage;
		this.shipData = shipData;

		cells = transform.GetComponentsInChildren<HullsMarketCell> ();

		moveUpBtn = transform.FindChild ("MoveUpBtn").GetComponent<BoxCollider2D> ();
		moveDownBtn = transform.FindChild ("MoveDownBtn").GetComponent<BoxCollider2D> ();
		buyHullBtn = transform.FindChild ("Buy Hull Btn").GetComponent<BoxCollider2D> ();

		moveUpBtnRender = transform.FindChild ("MoveUpBtn").GetComponent<SpriteRenderer>();
		moveDownBtnRender = transform.FindChild ("MoveDownBtn").GetComponent<SpriteRenderer>();
		buyHullBtnRender = transform.FindChild ("Buy Hull Btn").GetComponent<SpriteRenderer>();
		chosenHullBorder = transform.FindChild ("Chosen Item Border").GetComponent<SpriteRenderer>();
		hullImage = transform.FindChild ("Hull Image").GetComponent<SpriteRenderer>();

		Transform hullInformation = transform.FindChild("Hull Information");
		hullName = hullInformation.FindChild("Hull Name").GetComponent<TextMesh>();
		hullCost = hullInformation.FindChild("Hull Cost Value").GetComponent<TextMesh>();
		hullHealth = hullInformation.FindChild("Hull Health Value").GetComponent<TextMesh>();
		hullStorage = hullInformation.FindChild ("Hull Storage Value").GetComponent<TextMesh> ();

		weaponSlots = hullInformation.FindChild("Hull Weapon Slots").GetComponent<TextMesh>();
		armorSlots = hullInformation.FindChild("Hull Armor Slots").GetComponent<TextMesh>();
		shieldSlots = hullInformation.FindChild("Hull Shield Slots").GetComponent<TextMesh>();
		generatorSlots = hullInformation.FindChild("Hull Generator Slots").GetComponent<TextMesh>();
		repairDroidSlots = hullInformation.FindChild("Hull RepairDroid Slots").GetComponent<TextMesh>();
		harvesterSlots = hullInformation.FindChild("Hull Harvester Slots").GetComponent<TextMesh>();
		
		hullInformation.FindChild("Hull Cost Label").GetComponent<MeshRenderer>().sortingOrder = 1;
		hullInformation.FindChild("Hull Health Label").GetComponent<MeshRenderer>().sortingOrder = 1;
		hullInformation.FindChild ("Hull Storage Label").GetComponent<MeshRenderer> ().sortingOrder = 1;

		hullName.GetComponent<MeshRenderer>().sortingOrder = 1;
		hullCost.GetComponent<MeshRenderer>().sortingOrder = 1;
		hullHealth.GetComponent<MeshRenderer>().sortingOrder = 1;
		hullStorage.GetComponent<MeshRenderer> ().sortingOrder = 1;

		weaponSlots.GetComponent<MeshRenderer>().sortingOrder = 1;
		armorSlots.GetComponent<MeshRenderer>().sortingOrder = 1;
		shieldSlots.GetComponent<MeshRenderer>().sortingOrder = 1;
		generatorSlots.GetComponent<MeshRenderer>().sortingOrder = 1;
		repairDroidSlots.GetComponent<MeshRenderer>().sortingOrder = 1;
		harvesterSlots.GetComponent<MeshRenderer>().sortingOrder = 1;

		checkButtons ();
	}

	public void showScreen () {
		gameObject.SetActive (true);
	}

	void Update () {
		if (Input.GetMouseButtonDown (0) && Utils.hit != null) {
			if (Utils.hit == moveUpBtn) {
				if (scrollableUp) {
					offset -= offsetStep;
					afterScroll ();
				}
			} else if (Utils.hit == moveDownBtn) {
				if (scrollableDown) {
					offset += offsetStep;
					afterScroll ();
				}
			} else if (Utils.hit == buyHullBtn) {
				buyHull ();
			} else if (Utils.hit.name.Equals("Cell")) {
				selectCell ((HullsMarketCell)Utils.hit.gameObject.GetComponent<HullsMarketCell>());
			}
		}
	}

	private void buyHull () {
		int cost = shipData.getHullType ().getCost () - chosenHull.getHullType ().getCost ();

		if (Variables.cash + cost < 0) {
			Messenger.showMessage("Не хватает кредитов на замену корпуса");
			return;
		} else {
			Variables.cash += cost;
		}

		HullType oldHullType = shipData.getHullType();

		inventory.setCapacity(chosenHull.getHullType().getStorageCapacity());

		shipData.setHullType(chosenHull.getHullType(), chosenHull.getHullType().getMaxHealth());
		checkInventoryCapacity ();
		updateHullSlots();

		chosenHull.setHullType(oldHullType);
		selectCell(chosenHull.getCell());

		transform.parent.GetComponent<MarketScreen>().transform.FindChild("EquipmentsMarket").GetComponent<EquipmentsMarket>().updateShipInventory();
	}

	private void checkInventoryCapacity () {
		if (inventory.getFreeVolume() < 0) {
			while (true) {
				storage.addItemToCell(inventory.takeLastItem(), null);
				if (inventory.getFreeVolume() >= 0) break;
			}
			Messenger.showMessage("Часть оборудования перемещена из инвентаря в хранилище");
		}
	}

	private void updateHullSlots () {
		shipData.arrangeItemsToSlots();
		bool addedToInv = false, addedToStorage = false;
		foreach (HullSlot slot in shipData.getSlots()) {
			if (!slot.isSlotAvailable() && slot.getItem() != null) {
				if (inventory.getFreeVolume() >= slot.getItem().getVolume()) {
					inventory.addItemToCell(slot.takeItem(), null);
					addedToInv = true;
				} else {
					storage.addItemToCell(slot.takeItem(), null);
					addedToStorage = true;
				}
			}
		}
		if (addedToInv && addedToStorage) {
			Messenger.showMessage("Часть оборудования снята с корабля и перемещена в инвентарь и хранилище");
		} else if (addedToInv) {
			Messenger.showMessage("Часть оборудования снята с корабля и перемещена в инвентарь");
		} else if (addedToStorage) {
			Messenger.showMessage("Часть оборудования снята с корабля и перемещена в хранилище");
		}
	}

	private void refreshMarket () {
		foreach (HullsMarketCell cell in cells) {
			cell.setItem (null);
		}
		
		foreach (KeyValuePair<int, HullsMarketItem> pair in getItems ()) {
			HullsMarketItem item = pair.Value;
			item.setCell(null);
			if (pair.Key >= offset && pair.Key < (cells.Length + offset)) {
				HullsMarketCell cell = getCell (pair.Key - offset);
				cell.setItem (item);
				item.setCell (cell);
				item.transform.position = cell.transform.position;
				item.gameObject.SetActive (true);
			} else {
				item.gameObject.SetActive (false);
			}
			item.transform.SetParent(transform);
		}

		checkButtons ();
	}

	private void afterScroll () {
		refreshMarket ();
		if (chosenHull.getCell() == null) {
			chosenHullBorder.gameObject.SetActive (false);
		} else {
			chosenHullBorder.gameObject.SetActive (true);
			chosenHullBorder.transform.position = chosenHull.transform.position;
		}
	}
	
	private int getMaximumItemIndex () {
		int index = 0;
		foreach (KeyValuePair<int, HullsMarketItem> pair in getItems()) {
			if (pair.Key > index) {
				index = pair.Key;
			}
		}
		return index;
	}

	private void checkButtons () {
		if (offset != 0) {
			scrollableUp = true;
			moveUpBtnRender.sprite = moveUpBtnSprite;
		} else {
			scrollableUp = false;
			moveUpBtnRender.sprite = moveUpBtnSpriteDisabled;
		}
		if (getMaximumItemIndex () >= (cells.Length + offset)) {
			scrollableDown = true;
			moveDownBtnRender.sprite = moveDownBtnSprite;
		} else {
			scrollableDown = false;
			moveDownBtnRender.sprite = moveDownBtnSpriteDisabled;
		}
	}
	
	private HullsMarketCell getCell (int index) {
		foreach (HullsMarketCell cell in cells) {
			if (cell.index == index) {
				return cell;
			}
		}
		return null;
	}
	
	public void fillWithRandomHulls (int count, string label) {
		int index = 0;
		for (int i = 0; i < count; i++) {
			Transform itemTrans = Instantiate(hullsMarketItemPrefab)  as Transform;
			HullsMarketItem item = itemTrans.GetComponent<HullsMarketItem>();
			itemTrans.SetParent(transform);
			if (label != null) { item.name = label; }
			int rand = Mathf.RoundToInt (UnityEngine.Random.value * Enum.GetNames(typeof(HullType)).Length);

			item.setHullType (rand == 0? HullType.Little:
			                  rand == 1? HullType.Needle:
			                  rand == 2? HullType.Gnome:
			                  rand == 3? HullType.Cricket:
			                  rand == 4? HullType.Argo:
			                  rand == 5? HullType.Falcon:
			                  rand == 6? HullType.Adventurer:
			                  rand == 7? HullType.Corvette:
			                  rand == 8? HullType.Buffalo:
			                  rand == 9? HullType.Legionnaire:
			                  rand == 10? HullType.StarWalker:
			                  rand == 11? HullType.Warship:
			                  rand == 12? HullType.Asterix:
			                  rand == 13? HullType.Prime:
			                  rand == 14? HullType.Titan:
			                  rand == 15? HullType.Dreadnaut:
			                  HullType.Armageddon);

			if (index < 6) {
				item.transform.position = getCell (index).transform.position;
			} else {
				item.gameObject.SetActive (false);
			}
			getItems().Add(index, item);
			index++;
		}

		refreshMarket();
		selectCell (getCell (0));
	}

	public void selectCell (HullsMarketCell cell) {
		chosenHull = cell.getItem ();
		chosenHullBorder.transform.position = cell.transform.position;
		chosenHullBorder.gameObject.SetActive (true);

		switch (chosenHull.getHullType ()) {
			case HullType.Little: hullImage.sprite = hullImages[0]; break;
			case HullType.Needle: hullImage.sprite = hullImages[1]; break;
			case HullType.Gnome: hullImage.sprite = hullImages[2]; break;
			case HullType.Cricket: hullImage.sprite = hullImages[3]; break;
			case HullType.Argo: hullImage.sprite = hullImages[4]; break;
			case HullType.Falcon: hullImage.sprite = hullImages[5]; break;
			case HullType.Adventurer: hullImage.sprite = hullImages[6]; break;
			case HullType.Corvette: hullImage.sprite = hullImages[7]; break;
			case HullType.Buffalo: hullImage.sprite = hullImages[8]; break;
			case HullType.Legionnaire: hullImage.sprite = hullImages[9]; break;
			case HullType.StarWalker: hullImage.sprite = hullImages[10]; break;
			case HullType.Warship: hullImage.sprite = hullImages[11]; break;
			case HullType.Asterix: hullImage.sprite = hullImages[12]; break;
			case HullType.Prime: hullImage.sprite = hullImages[13]; break;
			case HullType.Titan: hullImage.sprite = hullImages[14]; break;
			case HullType.Dreadnaut: hullImage.sprite = hullImages[15]; break;
			case HullType.Armageddon: hullImage.sprite = hullImages[16]; break;
		}

		hullName.text = chosenHull.getHullType ().getName();
		int cost = shipData.getHullType ().getCost () - chosenHull.getHullType ().getCost ();
		int health = chosenHull.getHullType ().getMaxHealth () - shipData.getHullType ().getMaxHealth ();
		int store = chosenHull.getHullType ().getStorageCapacity () - shipData.getHullType ().getStorageCapacity ();

		hullCost.text = "$ " + (cost > 0? "+": "") + cost;
		hullHealth.text = (health > 0? "+": "") + health;
		hullStorage.text = (store > 0? "+": "") + store;

		if (health < 0) {
			hullHealth.color = redColor;	
		} else {
			hullHealth.color = blueColor;
		}
		if (store < 0) {
			hullStorage.color = redColor;
		} else {
			hullStorage.color = blueColor;
		}

		int weapons = chosenHull.getHullType ().getWeaponSlots () - shipData.getHullType ().getWeaponSlots ();
		int armors = chosenHull.getHullType().getArmorSlots() - shipData.getHullType().getArmorSlots();
		int shields = chosenHull.getHullType().getArmorSlots() - shipData.getHullType().getArmorSlots();
		int generators = chosenHull.getHullType().getArmorSlots() - shipData.getHullType().getArmorSlots();
		int repairDroids = chosenHull.getHullType().getArmorSlots() - shipData.getHullType().getArmorSlots();
		int harvesters = chosenHull.getHullType().getArmorSlots() - shipData.getHullType().getArmorSlots();

		weaponSlots.text = chosenHull.getHullType().getWeaponSlots().ToString();
		armorSlots.text = chosenHull.getHullType().getArmorSlots().ToString();
		shieldSlots.text = chosenHull.getHullType().getShieldSlots().ToString();
		generatorSlots.text = chosenHull.getHullType().getGeneratorSlots().ToString();
		repairDroidSlots.text = chosenHull.getHullType().getRepairDroidSlots().ToString();
		harvesterSlots.text = chosenHull.getHullType().getHarvesterSlots().ToString();

		weaponSlots.color = weapons > 0 ? greenColor : weapons < 0 ? redColor : blueColor;
		armorSlots.color = armors > 0 ? greenColor : armors < 0 ? redColor : blueColor;
		shieldSlots.color = shields > 0 ? greenColor : shields < 0 ? redColor : blueColor;
		generatorSlots.color = generators > 0 ? greenColor : generators < 0 ? redColor : blueColor;
		repairDroidSlots.color = repairDroids > 0 ? greenColor : repairDroids < 0 ? redColor : blueColor;
		harvesterSlots.color = harvesters > 0 ? greenColor : harvesters < 0 ? redColor : blueColor;

		if (Variables.cash + cost < 0) {
			buyHullBtnRender.sprite = buyBtnSpriteDisabled;
		} else {
			buyHullBtnRender.sprite = buyBtnSprite;
		}
	}

	public Dictionary<int, HullsMarketItem> getItems () {
		return items;
	}
	
	public HullsMarketCell[] getCells () {
		return cells;
	}

	public int getOffset () {
		return offset;
	}
}