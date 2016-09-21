using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StatusScreen : InventoryContainedScreen {

	private bool onPlanetSurface;

	private PerksView perksView;

	private Button playerBtn, perksBtn, shipBtn, closeBtn;

	private ShipData shipData;

	private PlayerData playerData;

	private StarSystem starSystem;

	public StatusScreen init (bool onPlanetSurface, Inventory inventory, Inventory storage, StarSystem starSystem) {
		this.onPlanetSurface = onPlanetSurface;
		this.starSystem = starSystem;

		shipData = transform.Find("Ship Data").GetComponent<ShipData>().init(onPlanetSurface);
		playerData = transform.Find("Player Data").GetComponent<PlayerData>().init();

		innerInit(inventory, storage);

		playerBtn = transform.Find("Player Button").GetComponent<Button>().init();
		perksBtn = transform.Find("Perks Button").GetComponent<Button>().init();
		shipBtn = transform.Find("Ship Button").GetComponent<Button>().init();
		closeBtn = transform.Find("Close Button").GetComponent<Button>().init();

		perksView = transform.Find("Perks View").GetComponent<PerksView>().init(this);

		transform.Find("BG").gameObject.SetActive(true);

		inventory.setCapacity(shipData.getHullType().getStorageCapacity());

		closeScreen();

		return this;
	}

	public ShipData getShipData () {
		return shipData;
	}

	public void showScreen () {
		if (gameObject.activeInHierarchy) { return; }

		inventory.setContainerScreen(this);
		inventory.setInventoryToBegin ();

		storage.setContainerScreen(this);
		storage.setInventoryToBegin ();
		storage.setPosition(false);

		perksView.updatePerks();

		Player.updateMinMaxDamage();

		shipData.updateHullInfo ();
		playerData.updatePlayerInfo();

		if (onPlanetSurface) {
			transform.position = Vector3.zero;
			PlanetSurface.topHideable.setVisible(false);
		} else {
			transform.position = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, transform.position.z);
			inventory.transform.parent.position = transform.position;
			foreach (Planet planet in starSystem.getPlanets()) {
				planet.setShipIsNear(false);
			}
			StarSystem.setGamePause(true);
		}

		setInventoryActive();
		setShipDataActive();

		gameObject.SetActive(true);
	}

	public void closeScreen () {
		hideItemInfo();
		perksView.hideInfo();
		perksView.gameObject.SetActive(false);
		inventory.gameObject.SetActive(false);
		playerData.gameObject.SetActive(false);
		shipData.gameObject.SetActive(false);
		storage.gameObject.SetActive(false);
		gameObject.SetActive(false);

		if (onPlanetSurface) {
			PlanetSurface.topHideable.setVisible(true);
		}
		else { StarSystem.setGamePause(false); }
	}

	protected override void checkBtnPress (Button btn) {
		if (btn == inventoryBtn) { setInventoryActive(); }
		else if (btn == storageBtn) { setStorageActive(); }
		else if (btn == playerBtn) { setPlayerDataActive(); }
		else if (btn == perksBtn) { setPerksActive(); }
		else if (btn == shipBtn) { setShipDataActive(); }
		else if (btn == closeBtn) { closeScreen(); }
		else { Debug.Log("Unknown button: " + btn.name); }
	}

	private void setPerksActive () {
		perksView.gameObject.SetActive(true);
		perksBtn.setActive(false);
		inventory.gameObject.SetActive(false);
		inventoryBtn.setActive(true);
		hideItemInfo(perksBtn);
	}

	private void setInventoryActive () {
		perksView.gameObject.SetActive(false);
		perksBtn.setActive(true);
		inventory.gameObject.SetActive(true);
		inventoryBtn.setActive(false);
	}

	private void setStorageActive () {
		storage.gameObject.SetActive(true);
		shipData.gameObject.SetActive (false);
		playerData.gameObject.SetActive(false);
		storageBtn.setActive(false);
		shipBtn.setActive(true);
		playerBtn.setActive(true);
		hideItemInfo(storageBtn);
	}

	private void setShipDataActive () {
		shipData.gameObject.SetActive (true);
		storage.gameObject.SetActive(false);
		playerData.gameObject.SetActive(false);
		shipBtn.setActive(false);
		storageBtn.setActive(true);
		playerBtn.setActive(true);
		hideItemInfo(shipBtn);
	}

	private void setPlayerDataActive () {
		playerData.updatePlayerInfo();
		playerData.gameObject.SetActive(true);
		storage.gameObject.SetActive(false);
		shipData.gameObject.SetActive (false);
		shipBtn.setActive(true);
		storageBtn.setActive(true);
		playerBtn.setActive(false);
		hideItemInfo(playerBtn);
	}

	public void hideItemInfo (Button btn) {
		if (getChosenItem() == null) { return; }
		if (btn == null) { hideItemInfo(); }

		if (btn == perksBtn && getChosenItem().cell != null && getChosenItem().cell.getInventory().getInventoryType() == Inventory.InventoryType.INVENTORY) {
			hideItemInfo();
		}
		else if (btn == shipBtn && getChosenItem().cell != null && getChosenItem().cell.getInventory().getInventoryType() != Inventory.InventoryType.INVENTORY) {
			hideItemInfo();
		}
		else if (btn == storageBtn && (getChosenItem().slot != null || (getChosenItem().cell != null && getChosenItem().cell.getInventory().getInventoryType() != Inventory.InventoryType.INVENTORY))) {
			hideItemInfo();
		}
		else if (btn ==playerBtn && (getChosenItem().slot != null || (getChosenItem().cell != null && getChosenItem().cell.getInventory().getInventoryType() != Inventory.InventoryType.INVENTORY))) {
			hideItemInfo();
		}
	}

	override protected void checkItemDrop () {
		if (Utils.hit != null && Utils.hit.name.Equals("Cell")) {
			InventoryCell cell = Utils.hit.transform.GetComponent<InventoryCell>();
			Inventory targetInv = cell.transform.parent.GetComponent<Inventory>();
			if (draggedItem.cell == null) {
				switch (draggedItem.getItemType().getKind()) {
					case ItemKind.EQUIPMENT: shipData.updateHullInfo(); break;
					case ItemKind.GEAR: playerData.updatePlayerInfo(); break;
				}
			}
			targetInv.addItemToCell(draggedItem, cell);
		} else if (Utils.hit != null && Utils.hit.name.StartsWith("HullSlot")) {
			HullSlot slot = Utils.hit.transform.GetComponent<HullSlot> ();
			if (slot.slotType != getItemToHullSlotType (draggedItem.getItemType ())) {
				if (draggedItem.cell == null) {
					if (inventory.gameObject.activeInHierarchy) {
						inventory.addItemToCell (draggedItem, draggedItem.cell);
					} else if (storage.gameObject.activeInHierarchy) {
						storage.addItemToCell (draggedItem, draggedItem.cell);
					}
					shipData.updateHullInfo();
				} else {
					draggedItem.returnToParentInventory ();
				}
			} else if (slot.item == null) {
				setItemToSlot (slot);
			} else if (slot.item != null) {
				Item currItem = slot.takeItem ();
				if (inventory.gameObject.activeInHierarchy) {
					inventory.addItemToCell (currItem, draggedItem.cell);
				} else if (storage.gameObject.activeInHierarchy) {
					storage.addItemToCell (currItem, draggedItem.cell);
				}
				shipData.updateHullInfo ();
				draggedItem.cell = null;
				setItemToSlot (slot);
			}
		} else if (Utils.hit != null && Utils.hit.name.StartsWith("GearSlot")) {
			GearSlot slot = Utils.hit.transform.GetComponent<GearSlot> ();
			if (slot.gearType != getItemToGearSlotType (draggedItem.getItemType ())) {
				if (draggedItem.cell == null) {
					if (inventory.gameObject.activeInHierarchy) {
						inventory.addItemToCell (draggedItem, draggedItem.cell);
					} else if (storage.gameObject.activeInHierarchy) {
						storage.addItemToCell (draggedItem, draggedItem.cell);
					}
					playerData.updatePlayerInfo();
				} else {
					draggedItem.returnToParentInventory ();
				}
			} else if (slot.item == null) {
				setItemToSlot (slot);
			} else if (slot.item != null) {
				Item currItem = slot.takeItem ();
				if (inventory.gameObject.activeInHierarchy) {
					inventory.addItemToCell (currItem, draggedItem.cell);
				} else if (storage.gameObject.activeInHierarchy) {
					storage.addItemToCell (currItem, draggedItem.cell);
				}
				draggedItem.cell = null;
				setItemToSlot (slot);
				playerData.updatePlayerInfo ();
			}
		} else if (draggedItem.cell == null) {
			if (inventory.gameObject.activeInHierarchy) {
				inventory.addItemToCell (draggedItem, null);
			} else if (storage.gameObject.activeInHierarchy) {
				storage.addItemToCell (draggedItem, null);
			}
			switch (draggedItem.getItemType().getKind()) {
				case ItemKind.EQUIPMENT: shipData.updateHullInfo(); break;
				case ItemKind.GEAR: playerData.updatePlayerInfo(); break;
			}
		} else {
			draggedItem.returnToParentInventory();
		}
	}

	private void setItemToSlot (Slot slot) {
		if (draggedItem.cell != null) {
			draggedItem.cell.takeItem ();
			draggedItem.cell.getInventory().calculateFreeVolume();
		}
		draggedItem.cell = null;
		slot.setItem (draggedItem);
		draggedItem.transform.position = slot.transform.position;
		if (slot.kind == ItemKind.EQUIPMENT) {
			draggedItem.transform.parent = shipData.transform;
			shipData.updateHullInfo ();
		} else if (slot.kind == ItemKind.GEAR) {
			draggedItem.transform.parent = playerData.transform;
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

	private GearSlot.Type getItemToGearSlotType (ItemType itemType) {
		switch (itemType) {
			case ItemType.HAND_WEAPON: return GearSlot.Type.HAND_WEAPON;
			case ItemType.BODY_ARMOR: return GearSlot.Type.BODY_ARMOR;
			default: Debug.Log("Unknown item type: " + itemType); return GearSlot.Type.NONE;
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
		perksView.hideInfo();
	}

	private void highlightSlot (bool hightlight, ItemType itemType) {
		if (!hightlight) {
			if (itemType.getKind() == ItemKind.EQUIPMENT) {
				foreach (Slot slot in shipData.getSlots()) { slot.setSprite(false); }
			} else if (itemType.getKind() == ItemKind.GEAR) {
				foreach (Slot slot in playerData.getSlots()) { slot.setSprite(false); }
			} else {
				foreach (Slot slot in shipData.getSlots()) { slot.setSprite(false); }
				foreach (Slot slot in playerData.getSlots()) { slot.setSprite(false); }
			}
		} else {
			if (itemType.getKind() == ItemKind.EQUIPMENT) {
				HullSlot.Type slotType = getItemToHullSlotType (itemType);
				foreach (HullSlot slot in shipData.getSlots()) {
					if (slot.slotType == slotType) {
						slot.setSprite (true);
					}
				}
			} else {
				GearSlot.Type gearType = getItemToGearSlotType (itemType);
				foreach (GearSlot slot in playerData.getSlots()) {
					if (slot.gearType == gearType) {
						slot.setSprite (true);
					}
				}
			}
		}
	}

	public void sendToVars () {
		shipData.sendToVars ();
		inventory.sendToVars();
		storage.sendToVars();
	}

	public void initFromVars () {
		shipData.initializeFromVars();
		inventory.initFromVars();
		storage.initFromVars();
	}
}