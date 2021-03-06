﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StatusScreen : InventoryContainedScreen, Closeable {

	public Sprite equipmentBG, shipBG, perksBG, cabinBG;

	private bool onPlanetSurface;

	private PerksView perksView;

	private Button playerBtn, perksBtn, shipBtn, cabinBtn, repairBtn, closeBtn;

	public ShipData shipData { get; private set; }

	public PlayerData playerData { get; private set; }

	private StarSystem starSystem;

	public Cabin cabin { get; private set; }

	private SpriteRenderer background;

	private ItemDescriptor itemDescriptor;

	[HideInInspector]
	public CameraController cameraController;

	private ScreenType lastOpened = ScreenType.EQUIPMENT;

	public StatusScreen init (StarSystem starSystem, ItemDescriptor itemDescriptor) {
		this.starSystem = starSystem;
		this.itemDescriptor = itemDescriptor;

		onPlanetSurface = starSystem == null;

		shipData = transform.Find("Ship Data").GetComponent<ShipData>().init(onPlanetSurface);
		playerData = transform.Find("Player Data").GetComponent<PlayerData>().init();

		innerInit(transform.Find("Inventory").GetComponent<Inventory>().init(Inventory.InventoryType.INVENTORY), "Inventory");

		playerBtn = transform.Find("Player Button").GetComponent<Button>().init();
		perksBtn = transform.Find("Perks Button").GetComponent<Button>().init();
		shipBtn = transform.Find("Ship Button").GetComponent<Button>().init();
		repairBtn = transform.Find ("Repair Button").GetComponent<Button> ().init (true);
		cabinBtn = transform.Find("Cabin Button").GetComponent<Button>().init();
		closeBtn = transform.Find("Close Button").GetComponent<Button>().init();

		perksView = transform.Find("Perks View").GetComponent<PerksView>().init();
		cabin = transform.Find("Cabin").GetComponent<Cabin>().init(this);

		background = transform.Find("Background").GetComponent<SpriteRenderer>();
		background.gameObject.SetActive(true);

		inventory.setCapacity(shipData.hullType.getStorageCapacity());

		close(false);

		return this;
	}

	public void showScreen () {
		if (gameObject.activeInHierarchy) { return; }

		UserInterface.showInterface = false;
		itemDescriptor.setEnabled(ItemDescriptor.Type.INVENTORY, this);

		inventory.setContainerScreen(this, 6);
		inventory.setInventoryToBegin ();

		perksView.updatePerks();

		Player.updateMinMaxDamage();

		shipData.updateHullInfo ();
		playerData.updatePlayerInfo();

		if (onPlanetSurface) {
			transform.position = Vector3.zero;
			PlanetSurface.topHideable.setVisible(false);
			repairBtn.setText(shipData.repairCost.ToString() + "$");
		} else {
			transform.position = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, transform.position.z);
			cameraController.setCameraSizeToDefault();
			inventory.transform.parent.position = transform.position;
			foreach (Planet planet in starSystem.getPlanets()) {
				planet.setShipIsNear(false);
			}
			StarSystem.setGamePause(true);
			itemDescriptor.setSpaceOffset(transform.localPosition);
		}

		show (lastOpened);

		updateCashTxt();

		InputProcessor.add(this);

		gameObject.SetActive(true);
	}

	public void close (bool byInputProcessor) {
		hideItemInfo();
		//		perksView.hideInfo();
		perksView.gameObject.SetActive(false);
		inventory.gameObject.SetActive(false);
		playerData.gameObject.SetActive(false);
		shipData.gameObject.SetActive(false);
		gameObject.SetActive(false);

		if (onPlanetSurface) {
			PlanetSurface.topHideable.setVisible(true);
		}
		else { StarSystem.setGamePause(false); }

		UserInterface.showInterface = true;
		itemDescriptor.setDisabled();

		if (!byInputProcessor) { InputProcessor.removeLast(); }
	}

	private void show (ScreenType type) {
		background.sprite = type == ScreenType.SHIP? shipBG: type == ScreenType.EQUIPMENT? equipmentBG: type == ScreenType.PERKS? perksBG: cabinBG;

		setCashTxtActive (type == ScreenType.SHIP || type == ScreenType.EQUIPMENT);

		repairBtn.setVisible(onPlanetSurface && type == ScreenType.SHIP && shipData.repairCost > 0);

		itemDescriptor.setAsPerkDescriptor(type == ScreenType.PERKS);

		shipData.gameObject.SetActive(type == ScreenType.SHIP);
		playerData.gameObject.SetActive(type == ScreenType.EQUIPMENT);
		inventory.gameObject.SetActive(type == ScreenType.EQUIPMENT || type == ScreenType.SHIP);
		perksView.gameObject.SetActive(type == ScreenType.PERKS);
		cabin.gameObject.SetActive(type == ScreenType.CABIN);

		playerBtn.setActive (type != ScreenType.EQUIPMENT);
		shipBtn.setActive (type != ScreenType.SHIP);
		perksBtn.setActive (type != ScreenType.PERKS);
		cabinBtn.setActive (type != ScreenType.CABIN);

		hideItemInfo();

		lastOpened = type;
	}

	private void repairShip () {
		shipData.repairShip(false);
		repairBtn.setText("");
		repairBtn.setVisible(false);
	}

	protected override void checkBtnPress (Button btn) {
		if (btn == playerBtn) { show(ScreenType.EQUIPMENT); }
		else if (btn == perksBtn) { show(ScreenType.PERKS); }
		else if (btn == shipBtn) { show(ScreenType.SHIP); }
		else if (btn == cabinBtn) { show(ScreenType.CABIN); }
		else if (btn == repairBtn) { repairShip (); }
		else if (btn == closeBtn) { close(false); }
		else { Debug.Log("Unknown button: " + btn.name); }
	}

//	private void setPerksActive () {
//		perksView.gameObject.SetActive(true);
//		perksBtn.setActive(false);
//		inventory.gameObject.SetActive(false);
//		inventoryBtn.setActive(true);
//		hideItemInfo(perksBtn);
//	}
//
//	private void setInventoryActive () {
//		perksView.gameObject.SetActive(false);
//		perksBtn.setActive(true);
//		inventory.gameObject.SetActive(true);
//		inventoryBtn.setActive(false);
//	}
//
//	private void setStorageActive () {
//		storage.gameObject.SetActive(true);
//		shipData.gameObject.SetActive (false);
//		playerData.gameObject.SetActive(false);
//		storageBtn.setActive(false);
//		shipBtn.setActive(true);
//		playerBtn.setActive(true);
//		hideItemInfo(storageBtn);
//	}
//
//	private void setShipDataActive () {
//		shipData.gameObject.SetActive (true);
//		storage.gameObject.SetActive(false);
//		playerData.gameObject.SetActive(false);
//		shipBtn.setActive(false);
//		storageBtn.setActive(true);
//		playerBtn.setActive(true);
//		hideItemInfo(shipBtn);
//	}
//
//	private void setPlayerDataActive () {
//		playerData.updatePlayerInfo();
//		playerData.gameObject.SetActive(true);
//		storage.gameObject.SetActive(false);
//		shipData.gameObject.SetActive (false);
//		shipBtn.setActive(true);
//		storageBtn.setActive(true);
//		playerBtn.setActive(false);
//		hideItemInfo(playerBtn);
//	}

	public void hideItemInfo (Button btn) {
		if (getChosenItem() == null) { return; }
		if (btn == null) { hideItemInfo(); }

		if (btn == perksBtn && getChosenItem().cell != null && getChosenItem().cell.inventory.inventoryType == Inventory.InventoryType.INVENTORY) {
			hideItemInfo();
		}
		else if (btn == shipBtn && getChosenItem().cell != null && getChosenItem().cell.inventory.inventoryType != Inventory.InventoryType.INVENTORY) {
			hideItemInfo();
		}
		else if (btn ==playerBtn && (getChosenItem().slot != null || (getChosenItem().cell != null && getChosenItem().cell.inventory.inventoryType != Inventory.InventoryType.INVENTORY))) {
			hideItemInfo();
		}
	}

	override protected void checkItemDrop () {
		if (Utils.hit != null && Utils.hit.name.Equals("Cell")) {
			InventoryCell cell = Utils.hit.GetComponent<InventoryCell>();
			Inventory targetInv = cell.inventory;
			if (draggedItem.cell == null) {
				switch (draggedItem.kind) {
					case ItemKind.SHIP_EQUIPMENT: shipData.updateHullInfo(); break;
					case ItemKind.EQUIPMENT: playerData.updatePlayerInfo(); break;
				}
			}
			targetInv.addItemToCell(draggedItem, cell);
		} else if (Utils.hit != null && Utils.hit.name.StartsWith("HullSlot")) {
			HullSlot slot = Utils.hit.GetComponent<HullSlot> ();
			if (slot.slotType != getItemToHullSlotType (draggedItem.type)) {
				if (draggedItem.cell == null && draggedItem.slot == null) {
					if (inventory.gameObject.activeInHierarchy) {
						inventory.addItemToCell (draggedItem, draggedItem.cell);
					}
					shipData.updateHullInfo();
				} else {
					draggedItem.returnToParent ();
				}
			} else if (slot.item == null) {
				setItemToSlot (slot);
			} else if (slot.item != null) {
				Item currItem = slot.takeItem ();
				if (draggedItem.slot != null && draggedItem.slot.slotType == slot.slotType) {
					draggedItem.slot.setItem(currItem);
				} else if (draggedItem.slot != null) {
					draggedItem.returnToParent();
				} else if (inventory.gameObject.activeInHierarchy) {
					inventory.addItemToCell (currItem, draggedItem.cell);
				}
				shipData.updateHullInfo ();
				setItemToSlot (slot);
			}
		} else if (Utils.hit != null && Utils.hit.name.StartsWith("EquipmentSlot")) {
			EquipmentSlot slot = Utils.hit.GetComponent<EquipmentSlot> ();
			if (slot.slotType != getItemToEquipmentSlotType (draggedItem.type)) {
				if (draggedItem.cell == null && draggedItem.slot == null) {
					if (inventory.gameObject.activeInHierarchy) {
						inventory.addItemToCell (draggedItem, draggedItem.cell);
					}
					playerData.updatePlayerInfo();
				} else {
					draggedItem.returnToParent ();
				}
			} else if (slot.item == null) {
				setItemToSlot (slot);
			} else if (slot.item != null) {
				Item currItem = slot.takeItem ();
				if (draggedItem.slot != null && draggedItem.slot.slotType == slot.slotType) {
					draggedItem.slot.setItem(currItem);
				} else if (draggedItem.slot != null) {
					draggedItem.returnToParent();
				} else if (inventory.gameObject.activeInHierarchy) {
					inventory.addItemToCell (currItem, draggedItem.cell);
				}
				setItemToSlot (slot);
				playerData.updatePlayerInfo ();
			}
		} else if (Utils.hit != null && Utils.hit.name.StartsWith("Supply")) {
			SupplySlot slot = Utils.hit.GetComponent<SupplySlot>();
			if (slot.item == null) {
				setItemToSlot(slot);
			} else {
				Item currItem = slot.takeItem();
				if (draggedItem.slot != null) {
					draggedItem.slot.setItem(currItem);
				} else if (inventory.gameObject.activeInHierarchy) {
					inventory.addItemToCell (currItem, draggedItem.cell);
				}
				setItemToSlot (slot);
			}
		} else if (draggedItem.cell == null && draggedItem.slot == null) {
			if (inventory.gameObject.activeInHierarchy) {
				inventory.addItemToCell (draggedItem, null);
			}
			switch (draggedItem.kind) {
				case ItemKind.SHIP_EQUIPMENT: shipData.updateHullInfo(); break;
				case ItemKind.EQUIPMENT: playerData.updatePlayerInfo(); break;
			}
		} else {
			draggedItem.returnToParent();
		}
	}

	private void setItemToSlot (Slot slot) {
		if (draggedItem.cell != null) {
			draggedItem.cell.inventory.calculateFreeVolume();
		}
		slot.setItem (draggedItem);
		if (slot.kind == ItemKind.SHIP_EQUIPMENT) {
			shipData.updateHullInfo ();
		} else if (slot.kind == ItemKind.EQUIPMENT) {
			playerData.updatePlayerInfo();
		}
	}

	private HullSlot.Type getItemToHullSlotType (ItemType itemType) {
		switch (itemType) {
			case ItemType.ARMOR: return HullSlot.Type.ARMOR;
			case ItemType.ENGINE: return HullSlot.Type.ENGINE;
			case ItemType.GENERATOR: return HullSlot.Type.GENERATOR;
			case ItemType.HARVESTER: return HullSlot.Type.HARVESTER;
			case ItemType.RADAR: return HullSlot.Type.RADAR;
			case ItemType.REPAIR_DROID: return HullSlot.Type.REPAIR_DROID;
			case ItemType.SHIELD: return HullSlot.Type.SHIELD;
			case ItemType.WEAPON: return HullSlot.Type.WEAPON;
			default: Debug.Log("Unknown item type: " + itemType); return HullSlot.Type.NONE;
		}
	}

	private EquipmentSlot.Type getItemToEquipmentSlotType (ItemType itemType) {
		switch (itemType) {
			case ItemType.HAND_WEAPON: return EquipmentSlot.Type.HAND_WEAPON;
			case ItemType.BODY_ARMOR: return EquipmentSlot.Type.BODY_ARMOR;
			default: Debug.Log("Unknown item type: " + itemType); return EquipmentSlot.Type.NONE;
		}
	}

	override protected void afterItemDrop () {
		if (draggedItem == null) {
			highlightSlot (false, ItemType.GOODS);//Goods здесь вместо null, т.к. enum неможет в null
		}
	}

	override protected void choseItem (Item item) {
		base.choseItem(item);
		if (draggedItem != null) {
			highlightSlot (true, item.type);
		}
//		perksView.hideInfo();
	}

	private void highlightSlot (bool hightlight, ItemType itemType) {
		if (!hightlight) {
			if (itemType.kind() == ItemKind.SHIP_EQUIPMENT) {
				foreach (Slot slot in shipData.slots) { slot.setActive(false); }
			} else if (itemType.kind() == ItemKind.EQUIPMENT || itemType.kind() == ItemKind.SUPPLY) {
				foreach (Slot slot in playerData.allSlots) { slot.setActive(false); }
			} else {
				foreach (Slot slot in shipData.slots) { slot.setActive(false); }
				foreach (Slot slot in playerData.allSlots) { slot.setActive(false); }
			}
		} else {
			if (itemType.kind() == ItemKind.SHIP_EQUIPMENT) {
				HullSlot.Type slotType = getItemToHullSlotType (itemType);
				foreach (HullSlot slot in shipData.slots) {
					if (slot.slotType == slotType) {
						slot.setActive (true);
					}
				}
			} else if (itemType.kind() == ItemKind.EQUIPMENT) {
				EquipmentSlot.Type slotType = getItemToEquipmentSlotType (itemType);
				foreach (EquipmentSlot slot in playerData.equipmentSlots) {
					if (slot.slotType == slotType) {
						slot.setActive (true);
					}
				}
			} else if (itemType.kind() == ItemKind.SUPPLY) {
				foreach (Slot slot in playerData.supplySlots) {
					slot.setActive(true);
				}
			}
		}
	}

	public void setButtonsVisible (bool visible) {
		perksBtn.setVisible(visible);
		shipBtn.setVisible(visible);
		cabinBtn.setVisible(visible);
		playerBtn.setVisible(visible);
		closeBtn.setVisible(visible);
	}

	public void sendToVars () {
		shipData.sendToVars ();
		playerData.sendToVars();
		inventory.sendToVars();
		cabin.sendToVars();
	}

	public void initFromVars () {
		shipData.initializeFromVars();
		playerData.initFromVars();
		inventory.initFromVars();
		inventory.setCapacity(shipData.hullType.getStorageCapacity());
		cabin.initFromVars();
	}

	private enum ScreenType {
		PERKS, EQUIPMENT, SHIP, CABIN
	}
}