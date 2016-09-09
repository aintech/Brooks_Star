using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EquipmentsMarket : InventoryContainedScreen {

	public Transform inventoryItemPrefab;

	public Sprite shipBtnSprite, shipBtnSpriteDisabled, marketBtnSprite, marketBtnSpriteDisabled, buybackBtnSprite, buybackBtnSpriteDisabled;
	
	private Inventory marketInv, shipInv, buybackInv;

	private ShipData shipData;

	private SpriteRenderer shipBtnRender, marketBtnRender, buybackBtnRender;

	private bool shipInvFilled;

	public void init (MarketScreen marketScreen, Inventory inventory, Inventory storage, Inventory marketInv, Inventory shipInv, Inventory buybackInv, ShipData shipData) {
		this.inventory = inventory;
		this.storage = storage;
		this.marketInv = marketInv;
		this.shipInv = shipInv;
		this.buybackInv = buybackInv;
		this.shipData = shipData;

		innerInit();
//		initInvetoryAndStorageBtns ();
		shipBtnRender = transform.FindChild ("Ship Btn").GetComponent<SpriteRenderer> ();
		marketBtnRender = transform.FindChild ("Market Btn").GetComponent<SpriteRenderer> ();
		buybackBtnRender = transform.FindChild ("Buyback Btn").GetComponent<SpriteRenderer> ();
	}
	
	public void showScreen () {
		inventory.setContainerScreen(this);
		storage.setContainerScreen(this);
		marketInv.setContainerScreen(this);
		shipInv.setContainerScreen(this);
		buybackInv.setContainerScreen(this);

		inventory.setInventoryToBegin ();
		storage.setInventoryToBegin ();
		marketInv.setInventoryToBegin ();
		buybackInv.setInventoryToBegin ();

		inventoryBtnRender.gameObject.SetActive (true);
		storageBtnRender.gameObject.SetActive (true);
		marketBtnRender.gameObject.SetActive (true);
		shipBtnRender.gameObject.SetActive (true);
		buybackBtnRender.gameObject.SetActive (true);

		setInventoryActive ();
		setMarketInvActive ();

		gameObject.SetActive (true);
	}

	override protected void checkBtnPress (string colliderName) {
		switch (colliderName) {
			case "Inventory Btn": setInventoryActive (); break;
			case "Storage Btn": setStorageActive (); break;
			case "Ship Btn": setShipInvActive (); break;
			case "Market Btn": setMarketInvActive(); break;
			case "Buyback Btn": setBuybackInvAvtive(); break;
		}
	}

	override protected void checkItemDrop () {
		if (hit.collider != null && hit.collider.name.Equals("Cell")) {
			InventoryCell cell = hit.collider.transform.GetComponent<InventoryCell>();
			Inventory sourceInv = draggedItem.transform.parent.GetComponent<Inventory>();
			Inventory targetInv = cell.transform.parent.GetComponent<Inventory>();
			
			if (sourceInv != targetInv && (sourceInv == inventory || sourceInv == storage || sourceInv == shipInv) && (targetInv == marketInv || targetInv == buybackInv)) {
				targetInv.sellItemToTrader(draggedItem, buybackInv);
				hideItemInfo(null);
			} else {
				targetInv.addItemToCell(draggedItem, cell);
			}
		} else {
			draggedItem.returnToParentInventory();
		}
	}

	private void setInventoryActive () {
		inventory.gameObject.SetActive (true);
		storage.gameObject.SetActive (false);
		shipInv.gameObject.SetActive (false);
		inventoryBtnRender.sprite = inventoryBtnSprite;
		storageBtnRender.sprite = storageBtnSpriteDisabled;
		shipBtnRender.sprite = shipBtnSpriteDisabled;
		hideItemInfo (inventory);
	}
	
	private void setStorageActive () {
		storage.gameObject.SetActive (true);
		inventory.gameObject.SetActive (false);
		shipInv.gameObject.SetActive (false);
		storageBtnRender.sprite = storageBtnSprite;
		inventoryBtnRender.sprite = inventoryBtnSpriteDisabled;
		shipBtnRender.sprite = shipBtnSpriteDisabled;
		hideItemInfo (storage);
	}
	
	private void setShipInvActive () {
		shipInv.gameObject.SetActive (true);
		inventory.gameObject.SetActive (false);
		storage.gameObject.SetActive (false);
		shipBtnRender.sprite = shipBtnSprite;
		inventoryBtnRender.sprite = inventoryBtnSpriteDisabled;
		storageBtnRender.sprite = storageBtnSpriteDisabled;
		hideItemInfo (shipInv);

		if (!shipInvFilled) {
			fillShipInventory ();
			shipInv.sortInventory();
		}
	}

	public void updateShipInventory () {
		clearShipInventory();
		fillShipInventory();
	}

	private void fillShipInventory () {
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

				shipInv.addItemToCell(item, null);
			}
		}
		shipInvFilled = true;
	}

	private void clearShipInventory () {
		foreach (KeyValuePair<int, InventoryItem> pair in shipInv.getItems()) {
			Destroy(pair.Value.gameObject);
		}
		shipInv.getItems().Clear();
		shipInv.setInventoryToBegin ();
		shipInvFilled = false;
	}

	private void setMarketInvActive () {
		marketInv.gameObject.SetActive (true);
		buybackInv.gameObject.SetActive (false);
		marketBtnRender.sprite = marketBtnSprite;
		buybackBtnRender.sprite = buybackBtnSpriteDisabled;
		hideItemInfo (marketInv);
	}
	
	private void setBuybackInvAvtive () {
		marketInv.gameObject.SetActive (false);
		buybackInv.gameObject.SetActive (true);
		marketBtnRender.sprite = marketBtnSpriteDisabled;
		buybackBtnRender.sprite = buybackBtnSprite;
		hideItemInfo (buybackInv);
	}
	
	protected void hideItemInfo (Inventory activeInventory) {
		if (chosenItem != null && activeInventory != null) {
			Inventory chosenItemInvetnory = chosenItem.transform.parent.GetComponent<Inventory> ();
			
			if ((chosenItemInvetnory == marketInv || chosenItemInvetnory == buybackInv) && 
			    (activeInventory == inventory || activeInventory == storage || activeInventory == shipInv)) 
			{
				return;
			}
			
			if ((chosenItemInvetnory == inventory || chosenItemInvetnory == storage || chosenItemInvetnory == shipInv) &&
			    (activeInventory == marketInv || activeInventory == buybackInv))
			{
				return;
			}
		}
		base.hideItemInfo ();
	}

	public void closeScreen () {
		inventoryBtnRender.gameObject.SetActive (false);
		storageBtnRender.gameObject.SetActive (false);
		shipBtnRender.gameObject.SetActive (false);
		marketBtnRender.gameObject.SetActive (false);
		buybackBtnRender.gameObject.SetActive (false);
		clearShipInventory();

		if (inventory != null) {
			if (draggedItem != null) {
				draggedItem.returnToParentInventory();
				draggedItem = null;
			}
			hideItemInfo(null);
			chosenItem = null;

			inventory.gameObject.SetActive (false);
			storage.gameObject.SetActive (false);
			shipInv.gameObject.SetActive (false);
			marketInv.gameObject.SetActive (false);
			buybackInv.gameObject.SetActive (false);

			gameObject.SetActive (false);
		}
	}
}