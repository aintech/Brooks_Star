using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class HangarScreen : MonoBehaviour, ButtonHolder {

	private TextMesh cashValue;

	private ShipData shipData;

	public Sprite[] hullImages;

	private Inventory inventory;

	public List<HullDisplay> displays { get; private set; }

	private Button closeBtn;

	private PlanetSurface planetSurface;

	public HangarScreen init (PlanetSurface planetSurface, Inventory inventory, ShipData shipData) {
		this.planetSurface = planetSurface;
		this.inventory = inventory;
		this.shipData = shipData;

		displays = new List<HullDisplay>();
		HullDisplay display;
		for (int i = 0; i < transform.childCount; i++) {
			transform.GetChild(i).gameObject.SetActive(true);
			display = transform.GetChild(i).GetComponent<HullDisplay>();
			if (display != null) { displays.Add(display.init(this, shipData)); }
		}

		cashValue = transform.Find("Cash Value").GetComponent<TextMesh>();
		cashValue.GetComponent<MeshRenderer>().sortingOrder = 1;
		cashValue.text = Vars.cash + "$";

		closeBtn = transform.Find("Close Button").GetComponent<Button>().init();

		refreshMarket();

		gameObject.SetActive(false);

		return this;
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
			case HullType.LITTLE: return hullImages[0];
			case HullType.NEEDLE: return hullImages[1];
			case HullType.GNOME: return hullImages[2];
			case HullType.CRICKET: return hullImages[3];
			case HullType.ARGO: return hullImages[4];
			case HullType.FALCON: return hullImages[5];
			case HullType.ADVENTURER: return hullImages[6];
			case HullType.CORVETTE: return hullImages[7];
			case HullType.BUFFALO: return hullImages[8];
			case HullType.LEGIONNAIRE: return hullImages[9];
			case HullType.STARWALKER: return hullImages[10];
			case HullType.WARSHIP: return hullImages[11];
			case HullType.ASTERIX: return hullImages[12];
			case HullType.PRIME: return hullImages[13];
			case HullType.TITAN: return hullImages[14];
			case HullType.DREADNAUT: return hullImages[15];
			case HullType.ARMAGEDDON: return hullImages[16];
			default: Debug.Log("Unknown hull type: " + type); return null;
		}
	}

	public void showScreen () {
		UserInterface.showInterface = false;
		foreach (HullDisplay dsp in displays) { dsp.updateCost(); }
		gameObject.SetActive (true);
	}

	public void closeScreen () {
		gameObject.SetActive(false);
		planetSurface.setVisible(true);
		UserInterface.showInterface = true;
	}

	public void fireClickButton (Button btn) {
		if (btn == closeBtn) { closeScreen(); }
		else { Debug.Log("Unknown button: " + btn.name); }
	}

	public void buyHull (HullDisplay display) {
		if (Vars.cash + display.cost < 0) {
			Messenger.showMessage("Не хватает кредитов на замену корпуса");
			return;
		} else { Vars.cash += display.cost; }

		HullType oldHullType = shipData.hullType;

		inventory.setCapacity(display.hullType.getStorageCapacity());

		shipData.setHullType(display.hullType, display.hullType.getMaxHealth());
		shipData.repairShip(true);
		updateHullSlots();

		display.setHull(oldHullType, getHullSprite(oldHullType));

		foreach (HullDisplay dsp in displays) { dsp.updateCost(); }

		cashValue.text = Vars.cash + "$";
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
		foreach (HullSlot slot in shipData.slots) {
			if (!slot.isSlotAvailable() && slot.item != null) {
//				if (inventory.getFreeVolume() >= slot.item.volume()) {
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