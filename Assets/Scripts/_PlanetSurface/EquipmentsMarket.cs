using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EquipmentsMarket : InventoryContainedScreen {

	public Sprite buyBG, sellBG;

	private SpriteRenderer bgRender;

	public Transform inventoryItemPrefab;

	private Inventory market;

	private Button buyBtn, sellBtn, closeBtn;

	private MarketScreen marketScreen;

	private ItemDescriptor itemDescriptor;

	private TextMesh actionMsg, cashValue;

	public void init (MarketScreen marketScreen, Inventory inventory, ItemDescriptor itemDescriptor) {
		this.marketScreen = marketScreen;
		this.itemDescriptor = itemDescriptor;

		market = transform.Find("Market").GetComponent<Inventory>().init(Inventory.InventoryType.MARKET);

		bgRender = transform.Find("Background").GetComponent<SpriteRenderer>();

		innerInit(market);

		buyBtn = transform.Find ("Buy Button").GetComponent<Button> ().init();
		sellBtn = transform.Find ("Sell Button").GetComponent<Button> ().init();
		closeBtn = transform.Find("Close Button").GetComponent<Button>().init();

		actionMsg = transform.Find("Action Description").GetComponent<TextMesh>();
		cashValue = transform.Find("Cash Value").GetComponent<TextMesh>();
		MeshRenderer mesh = actionMsg.GetComponent<MeshRenderer>();
		mesh.sortingOrder = 1;
		mesh = cashValue.GetComponent<MeshRenderer>();
		mesh.sortingOrder = 1;

		gameObject.SetActive(false);
	}
	
	public void showScreen () {
		UserInterface.showInterface = false;
		itemDescriptor.setEnabled(market);

		inventory.setContainerScreen(this, 10);
		market.setContainerScreen(this, 10);

		inventory.setInventoryToBegin ();
		market.setInventoryToBegin ();

//		setInventoryActive ();
		setBuyActive ();

		updateCashTxt();

		gameObject.SetActive (true);
	}

	override protected void checkBtnPress (Button btn) {
		if (btn == buyBtn) { setBuyActive(); }
		else if (btn == sellBtn) { setSellActive(); }
		else if (btn == closeBtn) { closeScreen(); }
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
		actionMsg.text = "<color=orange>Покупка</color> - правая кнопка мыши.";
		itemDescriptor.setInventoryType(ItemDescriptor.Type.MARKET_BUY);
		buyBtn.setActive(false);
		sellBtn.setActive(true);
		inventory.gameObject.SetActive(false);
		market.gameObject.SetActive(true);
	}
	
	private void setSellActive () {
		bgRender.sprite = sellBG;
		actionMsg.text = "<color=orange>Продажа</color> - правая кнопка мыши.";
		itemDescriptor.setInventoryType(ItemDescriptor.Type.MARKET_SELL);
		buyBtn.setActive(true);
		sellBtn.setActive(false);
		inventory.gameObject.SetActive(true);
		market.gameObject.SetActive(false);
	}

//	protected void hideItemInfo (Inventory activeInventory) {
//		if (chosenItem != null && activeInventory != null) {
//			Inventory chosenItemInvetnory = chosenItem.transform.parent.GetComponent<Inventory> ();
//			
//			if ((chosenItemInvetnory == market || chosenItemInvetnory == buyback) && 
//				(activeInventory == inventory || activeInventory == storage)) 
//			{
//				return;
//			}
//			
//			if ((chosenItemInvetnory == inventory || chosenItemInvetnory == storage) &&
//			    (activeInventory == market || activeInventory == buyback))
//			{
//				return;
//			}
//		}
//		base.hideItemInfo ();
//	}

	public Inventory getMarket () {
		return market;
	}

	private void updateCashTxt () {
		cashValue.text = Vars.cash.ToString() + "$";
	}

	public void closeScreen () {
		chosenItem = null;
		inventory.gameObject.SetActive(false);
		market.gameObject.SetActive(false);
		gameObject.SetActive(false);
		UserInterface.showInterface = true;
		itemDescriptor.setEnabled(null);
		marketScreen.setVisible(true);
//		if (inventory != null) {
//			if (draggedItem != null) {
//				draggedItem.returnToParent();
//				draggedItem = null;
//			}
//			hideItemInfo(null);
//			chosenItem = null;
//
//			inventory.gameObject.SetActive (false);
//			storage.gameObject.SetActive (false);
//			market.gameObject.SetActive (false);
//			buyback.gameObject.SetActive (false);
//
//			gameObject.SetActive (false);
//		}
	}
}