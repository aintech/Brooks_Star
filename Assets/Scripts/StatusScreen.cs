using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StatusScreen : InventoryContainedScreen {

	public Sprite equipmentBG, shipBG, perksBG;

	private bool onPlanetSurface;

	private PerksView perksView;

	private Button playerBtn, perksBtn, shipBtn, repairBtn, closeBtn;

	private ShipData shipData;

	private PlayerData playerData;

	private StarSystem starSystem;

	private SpriteRenderer background;

	private ItemDescriptor itemDescriptor;

	public CameraController cameraController;

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
		closeBtn = transform.Find("Close Button").GetComponent<Button>().init();

		perksView = transform.Find("Perks View").GetComponent<PerksView>().init();

		background = transform.Find("Background").GetComponent<SpriteRenderer>();
		background.gameObject.SetActive(true);

		inventory.setCapacity(shipData.hullType.getStorageCapacity());

		closeScreen();

		return this;
	}

	public Inventory getInventory () {
		return inventory;
	}

	public ShipData getShipData () {
		return shipData;
	}

	public void showScreen () {
		if (gameObject.activeInHierarchy) { return; }

		UserInterface.showInterface = false;
		itemDescriptor.setEnabled(inventory);
		itemDescriptor.setInventoryType(ItemDescriptor.Type.INVENTORY);

		inventory.setContainerScreen(this, 6);
		inventory.setInventoryToBegin ();

		perksView.updatePerks();

		Player.updateMinMaxDamage();

		shipData.updateHullInfo ();
		playerData.updatePlayerInfo();

		if (onPlanetSurface) {
			transform.position = Vector3.zero;
			PlanetSurface.topHideable.setVisible(false);
		} else {
			transform.position = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, transform.position.z);
			cameraController.setCameraSizeToDefault();
			inventory.transform.parent.position = transform.position;
			foreach (Planet planet in starSystem.getPlanets()) {
				planet.setShipIsNear(false);
			}
			StarSystem.setGamePause(true);
			itemDescriptor.setSpaceOffset(transform.localPosition);
//			itemDescriptor.transform.localPosition = transform.localPosition;
		}

		if (shipData.repairCost > 0) {
			repairBtn.setText(shipData.repairCost.ToString() + "$");
			repairBtn.setVisible(true);
		} else {
			repairBtn.setVisible(false);
		}

		show (ItemKind.EQUIPMENTS);
//		setInventoryActive();
//		setShipDataActive();
		updateCashTxt();

		gameObject.SetActive(true);
	}

	public void closeScreen () {
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
		itemDescriptor.setEnabled(null);
		itemDescriptor.setAsPerkDescriptor(false);
	}

	private void show (ItemKind kind) {
		background.sprite = kind == ItemKind.SHIP_EQUIPMENT? shipBG: kind == ItemKind.EQUIPMENTS? equipmentBG: perksBG;

		setCashTxtActive (kind == ItemKind.EQUIPMENTS || kind == ItemKind.SHIP_EQUIPMENT);

		repairBtn.gameObject.SetActive(onPlanetSurface && kind == ItemKind.SHIP_EQUIPMENT);

		itemDescriptor.setAsPerkDescriptor(kind != ItemKind.EQUIPMENTS && kind != ItemKind.SHIP_EQUIPMENT);

		shipData.gameObject.SetActive(kind == ItemKind.SHIP_EQUIPMENT);
		playerData.gameObject.SetActive(kind == ItemKind.EQUIPMENTS);
		inventory.gameObject.SetActive(kind == ItemKind.SHIP_EQUIPMENT || kind == ItemKind.EQUIPMENTS || kind == ItemKind.GOOD);
		perksView.gameObject.SetActive(kind == ItemKind.NONE);

		playerBtn.setActive (kind != ItemKind.EQUIPMENTS);
		shipBtn.setActive (kind != ItemKind.SHIP_EQUIPMENT);
		perksBtn.setActive (kind != ItemKind.NONE);

		hideItemInfo();
	}

	private void repairShip () {
		shipData.repairShip(false);
		repairBtn.setText("");
		repairBtn.setVisible(false);
	}

	protected override void checkBtnPress (Button btn) {
//		if (btn == inventoryBtn) { setInventoryActive(); }
//		else if (btn == storageBtn) { setStorageActive(); }
		if (btn == playerBtn) { show(ItemKind.EQUIPMENTS); }
		else if (btn == perksBtn) { show(ItemKind.NONE); }
		else if (btn == shipBtn) { show(ItemKind.SHIP_EQUIPMENT); }
		else if (btn == repairBtn) { repairShip (); }
		else if (btn == closeBtn) { closeScreen(); }
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

		if (btn == perksBtn && getChosenItem().cell != null && getChosenItem().cell.inventory.getInventoryType() == Inventory.InventoryType.INVENTORY) {
			hideItemInfo();
		}
		else if (btn == shipBtn && getChosenItem().cell != null && getChosenItem().cell.inventory.getInventoryType() != Inventory.InventoryType.INVENTORY) {
			hideItemInfo();
		}
		else if (btn ==playerBtn && (getChosenItem().slot != null || (getChosenItem().cell != null && getChosenItem().cell.inventory.getInventoryType() != Inventory.InventoryType.INVENTORY))) {
			hideItemInfo();
		}
	}

	override protected void checkItemDrop () {
		if (Utils.hit != null && Utils.hit.name.Equals("Cell")) {
			InventoryCell cell = Utils.hit.transform.GetComponent<InventoryCell>();
			Inventory targetInv = cell.inventory;
			if (draggedItem.cell == null) {
				switch (draggedItem.getItemType().getKind()) {
					case ItemKind.SHIP_EQUIPMENT: shipData.updateHullInfo(); break;
					case ItemKind.EQUIPMENTS: playerData.updatePlayerInfo(); break;
				}
			}
			targetInv.addItemToCell(draggedItem, cell);
		} else if (Utils.hit != null && Utils.hit.name.StartsWith("HullSlot")) {
			HullSlot slot = Utils.hit.transform.GetComponent<HullSlot> ();
			if (slot.slotType != getItemToHullSlotType (draggedItem.getItemType ())) {
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
			EquipmentSlot slot = Utils.hit.transform.GetComponent<EquipmentSlot> ();
			if (slot.slotType != getItemToEquipmentSlotType (draggedItem.getItemType ())) {
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
		} else if (draggedItem.cell == null && draggedItem.slot == null) {
			if (inventory.gameObject.activeInHierarchy) {
				inventory.addItemToCell (draggedItem, null);
			}
			switch (draggedItem.getItemType().getKind()) {
				case ItemKind.SHIP_EQUIPMENT: shipData.updateHullInfo(); break;
				case ItemKind.EQUIPMENTS: playerData.updatePlayerInfo(); break;
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
		} else if (slot.kind == ItemKind.EQUIPMENTS) {
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
			highlightSlot (false, ItemType.MINERAL);
		}
	}

	override protected void choseItem (Item item) {
		base.choseItem(item);
		if (draggedItem != null) {
			highlightSlot (true, item.getItemType ());
		}
//		perksView.hideInfo();
	}

	private void highlightSlot (bool hightlight, ItemType itemType) {
		if (!hightlight) {
			if (itemType.getKind() == ItemKind.SHIP_EQUIPMENT) {
				foreach (Slot slot in shipData.getSlots()) { slot.setActive(false); }
			} else if (itemType.getKind() == ItemKind.EQUIPMENTS) {
				foreach (Slot slot in playerData.getSlots()) { slot.setActive(false); }
			} else {
				foreach (Slot slot in shipData.getSlots()) { slot.setActive(false); }
				foreach (Slot slot in playerData.getSlots()) { slot.setActive(false); }
			}
		} else {
			if (itemType.getKind() == ItemKind.SHIP_EQUIPMENT) {
				HullSlot.Type slotType = getItemToHullSlotType (itemType);
				foreach (HullSlot slot in shipData.getSlots()) {
					if (slot.slotType == slotType) {
						slot.setActive (true);
					}
				}
			} else {
				EquipmentSlot.Type slotType = getItemToEquipmentSlotType (itemType);
				foreach (EquipmentSlot slot in playerData.getSlots()) {
					if (slot.slotType == slotType) {
						slot.setActive (true);
					}
				}
			}
		}
	}

	public void sendToVars () {
		shipData.sendToVars ();
		inventory.sendToVars();
	}

	public void initFromVars () {
		shipData.initializeFromVars();
		inventory.initFromVars();
	}
}