using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EquipmentsMarket : InventoryContainedScreen {

	public Transform inventoryItemPrefab;

	private Inventory market, inUse, buyback;

	private ShipData shipData;

	private Button marketBtn, inUseBtn, buybackBtn;

	private bool inUseFilled;

	public void init (MarketScreen marketScreen, Inventory inventory, Inventory storage, Inventory market, Inventory inUse, Inventory buyback, ShipData shipData) {
		this.inventory = inventory;
		this.storage = storage;
		this.market = market;
		this.inUse = inUse;
		this.buyback = buyback;
		this.shipData = shipData;

		innerInit();
		marketBtn = transform.FindChild ("Market Button").GetComponent<Button> ().init();
		inUseBtn = transform.FindChild ("InUse Button").GetComponent<Button> ().init();
		buybackBtn = transform.FindChild ("Buyback Button").GetComponent<Button> ().init();
	}
	
	public void showScreen () {
		inventory.setContainerScreen(this);
		storage.setContainerScreen(this);
		market.setContainerScreen(this);
		inUse.setContainerScreen(this);
		buyback.setContainerScreen(this);

		inventory.setInventoryToBegin ();
		storage.setInventoryToBegin ();
		market.setInventoryToBegin ();
		buyback.setInventoryToBegin ();

		inventoryBtn.setEnable(true);
		storageBtn.setEnable(true);
		marketBtn.setEnable(true);
		inUseBtn.setEnable(true);
		buybackBtn.setEnable(true);

		setInventoryActive ();
		setMarketActive ();

		gameObject.SetActive (true);
	}

	override protected void checkBtnPress (Button btn) {
		if (btn == inventoryBtn) { setInventoryActive(); }
		else if (btn == storageBtn) { setStorageActive(); }
		else if (btn == inUseBtn) { setInUseActive(); }
		else if (btn == marketBtn) { setMarketActive(); }
		else if (btn == buybackBtn) { setBuybackActive(); }
		else { Debug.Log("Unknown btn: " + btn.name); }
	}

	override protected void checkItemDrop () {
		if (Utils.hit != null && Utils.hit.name.Equals("Cell")) {
			InventoryCell cell = Utils.hit.transform.GetComponent<InventoryCell>();
			Inventory source = draggedItem.transform.parent.GetComponent<Inventory>();
			Inventory target = cell.transform.parent.GetComponent<Inventory>();
			
			if (source != target && (source == inventory || source == storage) && target == market) {
				target.sellItemToTrader(draggedItem, buyback);
				hideItemInfo(null);
			} else {
				target.addItemToCell(draggedItem, cell);
			}
		} else {
			draggedItem.returnToParentInventory();
		}
	}

	private void setInventoryActive () {
		inventory.gameObject.SetActive (true);
		storage.gameObject.SetActive (false);
		inUse.gameObject.SetActive (false);
		inventoryBtn.setActive(false);
		storageBtn.setActive(true);
		inUseBtn.setActive(true);
		hideItemInfo (inventory);
	}
	
	private void setStorageActive () {
		storage.gameObject.SetActive (true);
		inventory.gameObject.SetActive (false);
		inUse.gameObject.SetActive (false);
		storageBtn.setActive(false);
		inventoryBtn.setActive(true);
		inUseBtn.setActive(true);
		hideItemInfo (storage);
	}
	
	private void setInUseActive () {
		inUse.gameObject.SetActive (true);
		inventory.gameObject.SetActive (false);
		storage.gameObject.SetActive (false);
		inUseBtn.setActive(false);
		inventoryBtn.setActive(true);
		storageBtn.setActive(true);
		hideItemInfo (inUse);

		if (!inUseFilled) {
			fillInUseInventory ();
		}
	}

	public void updateInUseInventory () {
		clearInUseInventory();
		fillInUseInventory();
	}

	private void fillInUseInventory () {
		foreach (HullSlot slot in shipData.getSlots()) {
			if (slot.getItem() != null) {
				InventoryItem sourceItem = slot.getItem ();
				InventoryItem item = (Instantiate(inventoryItemPrefab) as Transform).GetComponent<InventoryItem>();

				item.setItemData(sourceItem.getItemData());
				item.setCost(sourceItem.getCost());
				item.setEnergyNeeded(sourceItem.getEnergyNeeded());
				item.setItemLevel(sourceItem.getItemLevel());
				item.setItemQuality(sourceItem.getItemQuality());
				item.setVolume(sourceItem.getVolume());

				inUse.addItemToCell(item, null);
			}
		}
		inUse.sortInventory();
		inUseFilled = true;
	}

	private void clearInUseInventory () {
		foreach (KeyValuePair<int, InventoryItem> pair in inUse.getItems()) {
			Destroy(pair.Value.gameObject);
		}
		inUse.getItems().Clear();
		inUse.setInventoryToBegin ();
		inUseFilled = false;
	}

	private void setMarketActive () {
		market.gameObject.SetActive (true);
		buyback.gameObject.SetActive (false);
		marketBtn.setActive(false);
		buybackBtn.setActive(true);
		hideItemInfo (market);
	}
	
	private void setBuybackActive () {
		market.gameObject.SetActive (false);
		buyback.gameObject.SetActive (true);
		marketBtn.setActive(true);
		buybackBtn.setActive(false);
		hideItemInfo (buyback);
	}
	
	protected void hideItemInfo (Inventory activeInventory) {
		if (chosenItem != null && activeInventory != null) {
			Inventory chosenItemInvetnory = chosenItem.transform.parent.GetComponent<Inventory> ();
			
			if ((chosenItemInvetnory == market || chosenItemInvetnory == buyback) && 
				(activeInventory == inventory || activeInventory == storage || activeInventory == inUse)) 
			{
				return;
			}
			
			if ((chosenItemInvetnory == inventory || chosenItemInvetnory == storage || chosenItemInvetnory == inUse) &&
			    (activeInventory == market || activeInventory == buyback))
			{
				return;
			}
		}
		base.hideItemInfo ();
	}

	public void closeScreen () {
//		inventoryBtnRender.gameObject.SetActive (false);
//		storageBtnRender.gameObject.SetActive (false);
//		shipBtnRender.gameObject.SetActive (false);
//		marketBtnRender.gameObject.SetActive (false);
//		buybackBtnRender.gameObject.SetActive (false);
		clearInUseInventory();

		if (inventory != null) {
			if (draggedItem != null) {
				draggedItem.returnToParentInventory();
				draggedItem = null;
			}
			hideItemInfo(null);
			chosenItem = null;

			inventory.gameObject.SetActive (false);
			storage.gameObject.SetActive (false);
			inUse.gameObject.SetActive (false);
			market.gameObject.SetActive (false);
			buyback.gameObject.SetActive (false);

			gameObject.SetActive (false);
		}
	}
}