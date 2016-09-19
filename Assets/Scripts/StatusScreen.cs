using UnityEngine;
using System.Collections;

public class StatusScreen : InventoryContainedScreen {

	private bool onPlanetSurface;

	private Transform gearSlots, perks;

	private Button gearSlotsBtn, perksBtn, shipBtn, closeBtn;

	private ShipData shipData;

	private StarSystem starSystem;

	public StatusScreen init (bool onPlanetSurface, Inventory inventory, Inventory storage, StarSystem starSystem) {
		this.onPlanetSurface = onPlanetSurface;
		this.starSystem = starSystem;

		shipData = transform.Find("Ship Data").GetComponent<ShipData>().init(onPlanetSurface);

		innerInit(inventory, storage);

		gearSlotsBtn = transform.Find("Gear Slots Button").GetComponent<Button>().init();
		perksBtn = transform.Find("Perks Button").GetComponent<Button>().init();
		shipBtn = transform.Find("Ship Button").GetComponent<Button>().init();
		closeBtn = transform.Find("Close Button").GetComponent<Button>().init();

		perks = transform.Find("Perks");
		gearSlots = transform.Find("Gear Slots");

		transform.Find("BG").gameObject.SetActive(true);

		inventory.setCapacity(shipData.getHullType().getStorageCapacity());

		closeScreen();

		return this;
	}

	public ShipData getShipData () {
		return shipData;
	}

	public void showScreen () {
		//TODO: прятать нижележащие экраны
		if (gameObject.activeInHierarchy) { return; }

		inventory.setContainerScreen(this);
		inventory.setInventoryToBegin ();

		storage.setContainerScreen(this);
		storage.setInventoryToBegin ();
		storage.setPosition(false);

		shipData.updateHullInfo ();

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
		perks.gameObject.SetActive(false);
		inventory.gameObject.SetActive(false);
		gearSlots.gameObject.SetActive(false);
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
		else if (btn == gearSlotsBtn) { setGearSlotsActive(); }
		else if (btn == perksBtn) { setPerksActive(); }
		else if (btn == shipBtn) { setShipDataActive(); }
		else if (btn == closeBtn) { closeScreen(); }
		else { Debug.Log("Unknown button: " + btn.name); }
	}

	private void setPerksActive () {
		perks.gameObject.SetActive(true);
		perksBtn.setActive(false);
		inventory.gameObject.SetActive(false);
		inventoryBtn.setActive(true);
		hideItemInfo(perksBtn);
	}

	private void setInventoryActive () {
		perks.gameObject.SetActive(false);
		perksBtn.setActive(true);
		inventory.gameObject.SetActive(true);
		inventoryBtn.setActive(false);
	}

	private void setStorageActive () {
		storage.gameObject.SetActive(true);
		shipData.gameObject.SetActive (false);
		gearSlots.gameObject.SetActive(false);
		storageBtn.setActive(false);
		shipBtn.setActive(true);
		gearSlotsBtn.setActive(true);
		hideItemInfo(storageBtn);
	}

	private void setShipDataActive () {
		shipData.gameObject.SetActive (true);
		storage.gameObject.SetActive(false);
		gearSlots.gameObject.SetActive(false);
		shipBtn.setActive(false);
		storageBtn.setActive(true);
		gearSlotsBtn.setActive(true);
		hideItemInfo(shipBtn);
	}

	private void setGearSlotsActive () {
		gearSlots.gameObject.SetActive(true);
		storage.gameObject.SetActive(false);
		shipData.gameObject.SetActive (false);
		shipBtn.setActive(true);
		storageBtn.setActive(true);
		gearSlotsBtn.setActive(false);
		hideItemInfo(gearSlotsBtn);
	}

	private void hideItemInfo (Button btn) {
		if (getChosenItem() == null) { return; }
		if (btn == perksBtn && getChosenItem().cell != null && getChosenItem().cell.getInventory().getInventoryType() == Inventory.InventoryType.INVENTORY) {
			hideItemInfo();
		}
		else if (btn == shipBtn && getChosenItem().cell != null && getChosenItem().cell.getInventory().getInventoryType() != Inventory.InventoryType.INVENTORY) {
			hideItemInfo();
		}
		else if (btn == storageBtn && (getChosenItem().hullSlot != null || (getChosenItem().cell != null && getChosenItem().cell.getInventory().getInventoryType() != Inventory.InventoryType.INVENTORY))) {
			hideItemInfo();
		}
		else if (btn == gearSlotsBtn && (getChosenItem().hullSlot != null || (getChosenItem().cell != null && getChosenItem().cell.getInventory().getInventoryType() != Inventory.InventoryType.INVENTORY))) {
			hideItemInfo();
		}
	}

	override protected void checkItemDrop () {
		if (Utils.hit != null && Utils.hit.name.Equals("Cell")) {
			InventoryCell cell = Utils.hit.transform.GetComponent<InventoryCell>();
			Inventory targetInv = cell.transform.parent.GetComponent<Inventory>();
			if (draggedItem.cell == null) {
				shipData.updateHullInfo();
			}
			targetInv.addItemToCell(draggedItem, cell);
		} else if (Utils.hit != null && Utils.hit.name.StartsWith("HullSlot")) {
			HullSlot slot = Utils.hit.transform.GetComponent<HullSlot> ();
			if (slot.slotType != getItemToSlotType (draggedItem.getItemType ())) {
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
		} else if (draggedItem.cell == null) {
			if (inventory.gameObject.activeInHierarchy) {
				inventory.addItemToCell (draggedItem, null);
			} else if (storage.gameObject.activeInHierarchy) {
				storage.addItemToCell (draggedItem, null);
			}
			shipData.updateHullInfo ();
		} else {
			draggedItem.returnToParentInventory();
		}
	}

	private void setItemToSlot (HullSlot slot) {
		if (draggedItem.cell != null) {
			draggedItem.cell.takeItem ();
			draggedItem.cell.transform.parent.GetComponent<Inventory> ().calculateFreeVolume();
		}
		draggedItem.cell = null;
		slot.setItem (draggedItem);
		draggedItem.transform.position = slot.transform.position;
		draggedItem.transform.parent = shipData.transform;
		shipData.updateHullInfo ();
	}

	private HullSlot.Type getItemToSlotType (ItemData.Type itemType) {
		switch (itemType) {
			case ItemData.Type.ARMOR: return HullSlot.Type.ARMOR;
			case ItemData.Type.ENGINE: return HullSlot.Type.ENGINE;
			case ItemData.Type.GENERATOR: return HullSlot.Type.GENERATOR;
			case ItemData.Type.HARVESTER: return HullSlot.Type.HARVESTER;
			case ItemData.Type.RADAR: return HullSlot.Type.RADAR;
			case ItemData.Type.REPAIR_DROID: return HullSlot.Type.REPAIR_DROID;
			case ItemData.Type.SHIELD: return HullSlot.Type.SHIELD;
			case ItemData.Type.WEAPON: return HullSlot.Type.WEAPON;
			default: Debug.Log("Unknown item type: " + itemType); return HullSlot.Type.ARMOR;
		}
	}

	override protected void afterItemDrop () {
		if (draggedItem == null) {
			highlightSlot (false, ItemData.Type.ARMOR);
		}
	}

	override protected void choseItem (Item item) {
		base.choseItem(item);
		if (draggedItem != null) {
			highlightSlot (true, item.getItemType ());
		}
	}

	private void highlightSlot (bool hightlight, ItemData.Type itemType) {
		if (!hightlight) {
			foreach (HullSlot slot in shipData.getSlots()) {
				slot.setSprite(false);
			}
		} else {
			HullSlot.Type slotType = getItemToSlotType (itemType);
			foreach (HullSlot slot in shipData.getSlots()) {
				if (slot.slotType == slotType) {
					slot.setSprite (true);
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