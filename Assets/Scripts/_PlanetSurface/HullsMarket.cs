using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class HullsMarket : MonoBehaviour, ButtonHolder {

	private TextMesh cashValue;

	private ShipData shipData;

	public Sprite[] hullImages;

	private Color blueColor = new Color(0.86f, 1, 1);

	private Color redColor = new Color(1, 0, 0);

	private Color greenColor = new Color(0, 1, 0); 

	private Inventory inventory;

	public List<HullDisplay> displays = new List<HullDisplay>();

	private Button closeBtn;

	private MarketScreen marketScreen;

	public void init (MarketScreen marketScreen, Inventory inventory, ShipData shipData) {
		this.marketScreen = marketScreen;
		this.inventory = inventory;
		this.shipData = shipData;

		HullDisplay display;
		for (int i = 0; i < transform.childCount; i++) {
			display = transform.GetChild(i).GetComponent<HullDisplay>();
			if (display != null) { displays.Add(display.init(this, shipData)); }
		}

		closeBtn = transform.Find("Close Button").GetComponent<Button>().init();

		refreshMarket();

		gameObject.SetActive(false);
	}

	private void refreshMarket () {
		Array types = Enum.GetValues(typeof(HullType));
		foreach (HullDisplay display in displays) {
			HullType type = (HullType)types.GetValue(UnityEngine.Random.Range(0, types.Length-1));
			display.setHull(type, getHullSprite(type));
		}
	}

	private Sprite getHullSprite (HullType type) {
		switch (type) {
			case HullType.Little: return hullImages[0];
			case HullType.Needle: return hullImages[1];
			case HullType.Gnome: return hullImages[2];
			case HullType.Cricket: return hullImages[3];
			case HullType.Argo: return hullImages[4];
			case HullType.Falcon: return hullImages[5];
			case HullType.Adventurer: return hullImages[6];
			case HullType.Corvette: return hullImages[7];
			case HullType.Buffalo: return hullImages[8];
			case HullType.Legionnaire: return hullImages[9];
			case HullType.StarWalker: return hullImages[10];
			case HullType.Warship: return hullImages[11];
			case HullType.Asterix: return hullImages[12];
			case HullType.Prime: return hullImages[13];
			case HullType.Titan: return hullImages[14];
			case HullType.Dreadnaut: return hullImages[15];
			case HullType.Armageddon: return hullImages[16];
			default: Debug.Log("Unknown hull type: " + type); return null;
		}
	}

	public void showScreen () {
		UserInterface.showInterface = false;
		gameObject.SetActive (true);
	}

	public void closeScreen () {
		gameObject.SetActive(false);
		marketScreen.setVisible(true);
		UserInterface.showInterface = true;
	}

	public void fireClickButton (Button btn) {
		if (btn == closeBtn) { closeScreen(); }
		else { Debug.Log("Unknown button: " + btn.name); }
	}

	public void buyHull (HullDisplay display) {
		int cost = shipData.getHullType ().getCost () - display.hullType.getCost ();

		if (Vars.cash + cost < 0) {
			Messenger.showMessage("Не хватает кредитов на замену корпуса");
			return;
		} else { Vars.cash += cost; }

		HullType oldHullType = shipData.getHullType();

		inventory.setCapacity(display.hullType.getStorageCapacity());

		shipData.setHullType(display.hullType, display.hullType.getMaxHealth());
		updateHullSlots();

		display.setHull(oldHullType, getHullSprite(oldHullType));

		foreach (HullDisplay dsp in displays) {
			dsp.updateCost();
		}
	}

//	private void checkInventoryCapacity () {
//		if (inventory.getFreeVolume() < 0) {
//			while (true) {
//				storage.addItemToCell(inventory.takeLastItem(), null);
//				if (inventory.getFreeVolume() >= 0) break;
//			}
//			Messenger.showMessage("Часть оборудования перемещена из инвентаря в хранилище");
//		}
//	}

	private void updateHullSlots () {
		shipData.arrangeItemsToSlots();
		bool addedToInv = false;//, addedToStorage = false;
		foreach (HullSlot slot in shipData.getSlots()) {
			if (!slot.isSlotAvailable() && slot.item != null) {
//				if (inventory.getFreeVolume() >= slot.item.getVolume()) {
					inventory.addItemToCell(slot.takeItem(), null);
//					addedToInv = true;
//				} else {
//					storage.addItemToCell(slot.takeItem(), null);
//					addedToStorage = true;
//				}
			}
		}
//		if (addedToInv && addedToStorage) {
//			Messenger.showMessage("Часть оборудования снята с корабля и перемещена в инвентарь и хранилище");
//		} else 
		if (addedToInv) {
			Messenger.showMessage("Часть оборудования снята с корабля и перемещена в инвентарь");
		}
//		else if (addedToStorage) {
//			Messenger.showMessage("Часть оборудования снята с корабля и перемещена в хранилище");
//		}
	}
}