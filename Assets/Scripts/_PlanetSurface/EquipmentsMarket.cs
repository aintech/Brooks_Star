using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EquipmentsMarket : InventoryContainedScreen {

	public Transform inventoryItemPrefab;

	private Inventory market, buyback;

	private Button marketBtn, buybackBtn;

	public void init (MarketScreen marketScreen, Inventory inventory, Inventory storage, Inventory market, Inventory buyback) {
		this.market = market;
		this.buyback = buyback;

		innerInit(inventory, storage);

		marketBtn = transform.FindChild ("Market Button").GetComponent<Button> ().init();
		buybackBtn = transform.FindChild ("Buyback Button").GetComponent<Button> ().init();
	}
	
	public void showScreen () {
		inventory.setContainerScreen(this);
		storage.setContainerScreen(this);
		market.setContainerScreen(this);
		buyback.setContainerScreen(this);

		inventory.setInventoryToBegin ();
		storage.setInventoryToBegin ();
		market.setInventoryToBegin ();
		buyback.setInventoryToBegin ();

		inventoryBtn.setVisible(true);
		storageBtn.setVisible(true);
		marketBtn.setVisible(true);
		buybackBtn.setVisible(true);

		storage.setPosition(true);

		setInventoryActive ();
		setMarketActive ();

		gameObject.SetActive (true);
	}

	override protected void checkBtnPress (Button btn) {
		if (btn == inventoryBtn) { setInventoryActive(); }
		else if (btn == storageBtn) { setStorageActive(); }
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
		inventoryBtn.setActive(false);
		storageBtn.setActive(true);
		hideItemInfo (inventory);
	}
	
	private void setStorageActive () {
		storage.gameObject.SetActive (true);
		inventory.gameObject.SetActive (false);
		storageBtn.setActive(false);
		inventoryBtn.setActive(true);
		hideItemInfo (storage);
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
				(activeInventory == inventory || activeInventory == storage)) 
			{
				return;
			}
			
			if ((chosenItemInvetnory == inventory || chosenItemInvetnory == storage) &&
			    (activeInventory == market || activeInventory == buyback))
			{
				return;
			}
		}
		base.hideItemInfo ();
	}

	public void closeScreen () {
		if (inventory != null) {
			if (draggedItem != null) {
				draggedItem.returnToParentInventory();
				draggedItem = null;
			}
			hideItemInfo(null);
			chosenItem = null;

			inventory.gameObject.SetActive (false);
			storage.gameObject.SetActive (false);
			market.gameObject.SetActive (false);
			buyback.gameObject.SetActive (false);

			gameObject.SetActive (false);
		}
	}
}