﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EquipmentsMarket : InventoryContainedScreen, Closeable {

	public Sprite buyBG, sellBG;

	private SpriteRenderer bgRender;

	private Inventory playerInventory, sellMarket;

	public Inventory buyMarket { get; private set; }

	private Button buyBtn, sellBtn, closeBtn;

	private PlanetSurface planetSurface;

	private ItemDescriptor itemDescriptor;

	private TextMesh actionMsg;

	private QuantityPopup popup;

	public EquipmentsMarket init (PlanetSurface planetSurface, Inventory playerInventory, ItemDescriptor itemDescriptor) {
		this.planetSurface = planetSurface;
		this.playerInventory = playerInventory;
		this.itemDescriptor = itemDescriptor;

		buyMarket = transform.Find("Buy Market").GetComponent<Inventory>().init(Inventory.InventoryType.MARKET);
		sellMarket = transform.Find("Sell Market").GetComponent<Inventory>().init(Inventory.InventoryType.MARKET);

		bgRender = transform.Find("Background").GetComponent<SpriteRenderer>();

		buyBtn = transform.Find ("Buy Button").GetComponent<Button> ().init();
		sellBtn = transform.Find ("Sell Button").GetComponent<Button> ().init();
		closeBtn = transform.Find("Close Button").GetComponent<Button>().init();

		actionMsg = transform.Find("Action Description").GetComponent<TextMesh>();
		MeshRenderer mesh = actionMsg.GetComponent<MeshRenderer>();
		mesh.sortingOrder = 1;

		for (int i = 0; i < transform.childCount; i++) {
			transform.GetChild(i).gameObject.SetActive(true);
		}

		popup = transform.Find("Popup").GetComponent<QuantityPopup>().init(this);
		popup.adjustPosition(Vector3.zero);

		gameObject.SetActive(false);

		return this;
	}

	public void askToBuy (Item item) {
		if (item == null) { return; }
		if (buyMarket.gameObject == null) { return; }

		if (item.quantity == 1) { buyItem(item, 1); }
		else { popup.show(item, true); }
	}

	public void askToSell (Item item) {
		if (item == null) { return; }
		if (sellMarket.gameObject == null) { return; }

		if (item.quantity == 1) { sellItem(item, 1); }
		else { popup.show(item, false); }
	}

	public void buyItem (Item item, int quantity) {
		if (Vars.cash < (item.cost * quantity)) { Messenger.notEnoughtCash(item.itemName, quantity); return; }
		if (item.volume > .001f && (playerInventory.getFreeVolume() - (item.volume * quantity)) < 0) { Messenger.showMessage("Недостаточно места в инвентаре."); return; }
		Vars.cash -= (item.cost * quantity);
		item.cell.inventory.containerScreen.updateCashTxt();

		if (item.quantity == quantity) {
			playerInventory.addItemToCell(item.cell.takeItem(), null);
		} else {
			Item buyed = Instantiate<Transform>(ItemFactory.itemPrefab).GetComponent<Item>();
			buyed.init(DataCopier.copy(item.itemData));
			buyed.quantity = quantity;
			playerInventory.addItemToCell(buyed, null);
			item.quantity -= quantity;
		}
	}

	public void sellItem (Item item, int quantity) {
		Vars.cash += (item.cost * quantity);
		item.cell.inventory.containerScreen.updateCashTxt();

		if (item.quantity == quantity) {
			item.cell.takeItem().destroy();
		} else {
			item.quantity -= quantity;
		}
	}

	public void showScreen () {
		UserInterface.showInterface = false;

		buyMarket.setContainerScreen(this, 10);
		sellMarket.setContainerScreen(this, 10);

		buyMarket.setInventoryToBegin ();
		sellMarket.setInventoryToBegin ();

		buyMarket.sortInventory();
		sellMarket.setItemsFromOtherInventory(playerInventory);

		setBuyActive ();

		updateCashTxt();

		InputProcessor.add(this);

		gameObject.SetActive (true);
	}

	override protected void checkBtnPress (Button btn) {
		if (btn == buyBtn) { setBuyActive(); }
		else if (btn == sellBtn) { setSellActive(); }
		else if (btn == closeBtn) { close(false); }
		else { Debug.Log("Unknown btn: " + btn.name); }
	}

//	override protected void checkItemDrop () {
//		if (Utils.hit != null && Utils.hit.name.Equals("Cell")) {
//			InventoryCell cell = Utils.hit.transform.GetComponent<InventoryCell>();
//			Inventory source = draggedItem.cell.getInventory();
//			Inventory target = cell.getInventory();
//			
//			if (source != target && (source == inventory || source == storage) && target == market) {
//				target.sellItemToTrader(draggedItem, buyback);
//				hideItemInfo(null);
//			} else {
//				target.addItemToCell(draggedItem, cell);
//			}
//		} else {
//			draggedItem.returnToParent();
//		}
//	}

	private void setBuyActive () {
		bgRender.sprite = buyBG;
		innerInit(buyMarket, "default");
		actionMsg.text = "<color=orange>Покупка</color> - правая кнопка мыши.";
		itemDescriptor.setEnabled(ItemDescriptor.Type.MARKET_BUY, this);
		buyBtn.setActive(false);
		sellBtn.setActive(true);
		buyMarket.refreshInventory();
		sellMarket.gameObject.SetActive(false);
		buyMarket.gameObject.SetActive(true);
	}
	
	private void setSellActive () {
		bgRender.sprite = sellBG;
		innerInit(sellMarket, "default");
		actionMsg.text = "<color=orange>Продажа</color> - правая кнопка мыши.";
		itemDescriptor.setEnabled(ItemDescriptor.Type.MARKET_SELL, this);
		buyBtn.setActive(true);
		sellBtn.setActive(false);
		sellMarket.refreshInventory();
		buyMarket.gameObject.SetActive(false);
		sellMarket.gameObject.SetActive(true);
	}

	public void close (bool byInputProcessor) {
		chosenItem = null;
		gameObject.SetActive(false);
		playerInventory.setItemsFromOtherInventory(sellMarket);
		UserInterface.showInterface = true;
		itemDescriptor.setDisabled();
		planetSurface.setVisible(true);
		if (!byInputProcessor) { InputProcessor.removeLast(); }
	}
}