using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HangarScreen : InventoryContainedScreen {

	public Sprite hullActiveSprite, storageActiveSprite;
	
	private PlanetScene planetScene;

	private SpriteRenderer hullStorageBtnRender;

	private ShipData shipData;

	private Vector3 storageRightPosition = new Vector3 (4.31f, 0.43f);

	private Vector3 storageLeftPosition = new Vector3 (-4.33f, 0.43f);

	private Vector3 storageBtnRightPosition = new Vector3 (7.05f, 2.84f);

	private Vector3 storageBtnLeftPosition = new Vector3 (-7.04f, 0.47f);

	private Vector3 hullViewPosition = new Vector3 (4.72f, 0.43f);

	override protected void init () {
		base.init ();
		initInvetoryAndStorageBtns ();
		hullStorageBtnRender = transform.FindChild ("Hull Storage Btn").GetComponent<SpriteRenderer> ();
	}

	public void showScreen (PlanetScene planetScene, Inventory inventory, Inventory storage, ShipData shipData) {
		gameObject.SetActive (true);

		this.planetScene = planetScene;
		this.inventory = inventory;
		this.storage = storage;
		this.shipData = shipData;

		inventory.setContainerScreen(this);
		storage.setContainerScreen(this);

		inventory.setInventoryToBegin ();
		storage.setInventoryToBegin ();

		storage.transform.position = storageLeftPosition;
		shipData.transform.position = hullViewPosition;

		shipData.updateHullInfo ();

		showHullScreen ();
	}

	override protected void checkBtnPress (string colliderName) {
		switch (colliderName) {
			case "Close Btn": closeScreen (); break;
			case "Hull Storage Btn": changeScreens (); break;
			case "Inventory Btn": activateInventory (); break;
			case "Storage Btn": activateStorage (); break;
		}
	}

	override protected void checkItemDrop () {
		if (hit.collider != null && hit.collider.name.Equals("Cell")) {
			InventoryCell cell = hit.collider.transform.GetComponent<InventoryCell>();
			Inventory targetInv = cell.transform.parent.GetComponent<Inventory>();
			if (draggedItem.getCell() == null) {
				shipData.updateHullInfo();
			}
			targetInv.addItemToCell(draggedItem, cell);
		} else if (hit.collider != null && hit.collider.name.Contains(" Slot")) {
			HullSlot slot = hit.collider.transform.GetComponent<HullSlot> ();
			if (slot.getHullSlotType() != getHullToItemType (draggedItem.getItemType ())) {
				if (draggedItem.getCell () == null) {
					if (inventory.gameObject.activeInHierarchy) {
						inventory.addItemToCell (draggedItem, draggedItem.getCell ());
					} else if (storage.gameObject.activeInHierarchy) {
						storage.addItemToCell (draggedItem, draggedItem.getCell ());
					}
					shipData.updateHullInfo();
				} else {
					draggedItem.returnToParentInventory ();
				}
			} else if (slot.getItem () == null) {
				setItemToSlot (slot);
			} else if (slot.getItem() != null) {
				InventoryItem currItem = slot.takeItem ();
				if (inventory.gameObject.activeInHierarchy) {
					inventory.addItemToCell (currItem, draggedItem.getCell ());
				} else if (storage.gameObject.activeInHierarchy) {
					storage.addItemToCell (currItem, draggedItem.getCell ());
				}
				shipData.updateHullInfo ();
				draggedItem.setCell (null);
				setItemToSlot (slot);
			}
		} else if (draggedItem.getCell () == null) {
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
		if (draggedItem.getCell () != null) {
			draggedItem.getCell ().takeItem ();
			draggedItem.getCell().transform.parent.GetComponent<Inventory> ().updateVolumeTxt ();
		}
		draggedItem.setCell (null);
		slot.setItem (draggedItem);
		draggedItem.transform.position = slot.transform.position;
		draggedItem.transform.parent = shipData.transform;
		shipData.updateHullInfo ();
	}

	private HullSlot.HullSlotType getHullToItemType (InventoryItem.Type itemType) {
		switch (itemType) {
		case InventoryItem.Type.Armor: return HullSlot.HullSlotType.Armor;
		case InventoryItem.Type.Engine: return HullSlot.HullSlotType.Engine;
		case InventoryItem.Type.Generator: return HullSlot.HullSlotType.Generator;
		case InventoryItem.Type.Harvester: return HullSlot.HullSlotType.Harvester;
		case InventoryItem.Type.Radar: return HullSlot.HullSlotType.Radar;
		case InventoryItem.Type.RepairDroid: return HullSlot.HullSlotType.RepairDroid;
		case InventoryItem.Type.Shield: return HullSlot.HullSlotType.Shield;
		case InventoryItem.Type.Weapon: return HullSlot.HullSlotType.Weapon;
		default: Debug.Log("Неизвестный тип предмета"); return HullSlot.HullSlotType.Armor;
		}
	}

	override protected void afterItemDrop () {
		if (draggedItem == null) {
			highlightSlot (false, InventoryItem.Type.Armor);
		}
	}

	override protected void choseItem (InventoryItem item) {
		base.choseItem(item);
		if (draggedItem != null) {
			highlightSlot (true, item.getItemType ());
		}
	}

	private void highlightSlot (bool hightlight, InventoryItem.Type itemType) {
		if (!hightlight) {
			foreach (HullSlot slot in shipData.getSlots()) {
				slot.setSprite(false);
			}
		} else {
			HullSlot.HullSlotType hullType = getHullToItemType (itemType);
			foreach (HullSlot slot in shipData.getSlots()) {
				if (slot.getHullSlotType() == hullType) {
					slot.setSprite (true);
				}
			}
		}
	}

	private void changeScreens () {
		if (isHullScreenActive ()) {
			showStorageScreen ();
		} else {
			showHullScreen ();
		}
	}

	private void showHullScreen () {
		hullStorageBtnRender.sprite = hullActiveSprite;
		storage.transform.position = storageLeftPosition;
		
		shipData.gameObject.SetActive (true);
		storageBtnRender.transform.position = storageBtnLeftPosition;
		activateInventory ();
	}

	private void showStorageScreen () {
		hullStorageBtnRender.sprite = storageActiveSprite;
		storageBtnRender.sprite = storageBtnSprite;
		storage.transform.position = storageRightPosition;
		
		shipData.gameObject.SetActive (false);
		storageBtnRender.transform.position = storageBtnRightPosition;
		activateInventory ();
	}

	private void activateInventory () {
		if (isHullScreenActive ()) {
			inventory.gameObject.SetActive (true);
			inventoryBtnRender.sprite = inventoryBtnSprite;
			
			storage.gameObject.SetActive (false);
			storageBtnRender.sprite = storageBtnSpriteDisabled;
		} else {
			inventory.gameObject.SetActive (true);
			inventoryBtnRender.sprite = inventoryBtnSprite;
			
			storage.gameObject.SetActive (true);
			storageBtnRender.sprite = storageBtnSprite;
		}
		hideItemInfo (inventory);
	}

	private void activateStorage () {
		if (isHullScreenActive ()) {
			inventory.gameObject.SetActive (false);
			inventoryBtnRender.sprite = inventoryBtnSpriteDisabled;
			
			storage.gameObject.SetActive (true);
			storageBtnRender.sprite = storageBtnSprite;
		} else {
			inventory.gameObject.SetActive (true);
			inventoryBtnRender.sprite = inventoryBtnSprite;
			
			storage.gameObject.SetActive (true);
			storageBtnRender.sprite = storageBtnSprite;
		}
		hideItemInfo (storage);
	}
	
	protected void hideItemInfo (Inventory activeInventory) {
		if (chosenItem != null) {
			ShipData shipData = chosenItem.transform.parent.GetComponent<ShipData> ();
			if (shipData != null && shipData.gameObject.activeInHierarchy) {
				return;
			}
		}
		if (chosenItem != null && activeInventory != null) {
			Inventory chosenItemInvetnory = chosenItem.transform.parent.GetComponent<Inventory> ();
			//if (chosenItemInvetnory == inventory && activeInventory == inventory) return;
			if (chosenItemInvetnory != null && chosenItemInvetnory.gameObject.activeInHierarchy) {
				chosenItemBorder.transform.position = chosenItem.transform.position;
				return;
			}
		}
		base.hideItemInfo ();
	}

	private bool isHullScreenActive () {
		return shipData.gameObject.activeInHierarchy;
	}

	private void closeScreen () {
		hideItemInfo();
		storage.transform.position = storageLeftPosition;
		storageBtnRender.transform.position = storageBtnLeftPosition;
		inventory.gameObject.SetActive (false);
		storage.gameObject.SetActive (false);
		shipData.gameObject.SetActive (false);
		planetScene.setPlanetEnabled(true);
		gameObject.SetActive (false);
	}
}
