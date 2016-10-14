using UnityEngine;
using System.Collections;

public abstract class InventoryContainedScreen : MonoBehaviour, ButtonHolder {

	protected Inventory inventory;
	
	protected Item draggedItem, chosenItem;
	
	private Transform chosenItemBorder;
	
	private Vector3 draggedItemPosition = Vector3.zero;

	private Vector2 dragOffset;

	private TextMesh cashValue;

	protected void innerInit(Inventory inventory, string layerName) {
		this.inventory = inventory;
		chosenItemBorder = transform.Find ("Chosen Item Border");
		cashValue = transform.Find("Cash Value").GetComponent<TextMesh>();
		MeshRenderer mesh = cashValue.GetComponent<MeshRenderer>();
		mesh.sortingLayerName = layerName;
		mesh.sortingOrder = 1;
		setCashTxtActive(true);
	}

	void Update () {
		if (inventory.getInventoryType() == Inventory.InventoryType.MARKET) { return; }
		if (Input.GetMouseButtonDown(0) && Utils.hit != null) {
			if (Utils.hit.name.Equals("Cell")) {
				Item item = Utils.hit.transform.GetComponent<InventoryCell>().item;
				if (item != null) {
					draggedItem = Utils.hit.transform.GetComponent<InventoryCell>().takeItem();
					draggedItem.GetComponent<Renderer>().sortingOrder = 4;
					choseItem(item);
					chosenItemBorder.transform.position = item.transform.position;
					chosenItemBorder.gameObject.SetActive(true);
				}
			} else if (Utils.hit.name.StartsWith("HullSlot")) {
				Item item = Utils.hit.transform.GetComponent<HullSlot>().item;
				if (item != null) {
					chooseDraggedItemFromSlot(Utils.hit.transform.GetComponent<HullSlot>());
					chosenItemBorder.transform.position = item.transform.position;
					choseItem(item);
					chosenItemBorder.gameObject.SetActive(true);
				}
			} else if (Utils.hit.name.StartsWith("EquipmentSlot")) {
				Item item = Utils.hit.transform.GetComponent<EquipmentSlot>().item;
				if (item != null) {
					chooseDraggedItemFromSlot(Utils.hit.transform.GetComponent<EquipmentSlot>());
					chosenItemBorder.transform.position = item.transform.position;
					choseItem(item);
					chosenItemBorder.gameObject.SetActive(true);
				}
			}
		}
		if (draggedItem != null) {
			draggedItemPosition.Set(Utils.mousePos.x - dragOffset.x, Utils.mousePos.y - dragOffset.y, 0);
			draggedItem.transform.position = draggedItemPosition;
			chosenItemBorder.position = chosenItem.transform.position;
			if (Input.GetMouseButtonUp(0)) dropItem ();
		}
	}

	public void fireClickButton (Button btn) {
		checkBtnPress(btn);
	}

	abstract protected void checkBtnPress (Button btn);
	
	virtual protected void chooseDraggedItemFromSlot (Slot slot) {
		draggedItem = slot.takeItem();
		draggedItem.GetComponent<Renderer>().sortingOrder = 4;
	}

	virtual protected void choseItem (Item item) {
		chosenItem = item;
		dragOffset.Set(Utils.mousePos.x - item.transform.position.x, Utils.mousePos.y - item.transform.position.y);
	}
	
	private void dropItem () {
		checkItemDrop ();
		draggedItem.changeSortOrder(4);
		if(chosenItem != null) chosenItemBorder.position = chosenItem.transform.position;
		draggedItem = null;
		afterItemDrop ();
	}

	virtual protected void checkItemDrop () {}
	virtual protected void afterItemDrop () {}

	protected void hideItemInfo () {
		chosenItemBorder.gameObject.SetActive (false);
		chosenItem = null;
	}
	
	public Item getChosenItem () {
		return chosenItem;
	}

	public void updateChosenItemBorder () {
		if (chosenItem != null) {
			if (chosenItem.cell == null && chosenItem.slot == null) {
				chosenItemBorder.gameObject.SetActive(false);
			} else {
				chosenItemBorder.transform.position = chosenItem.transform.position;
				chosenItemBorder.gameObject.SetActive (true);
			}
		} else {
			chosenItemBorder.gameObject.SetActive (false);
		}
	}

	public void setCashTxtActive (bool asActive) {
		cashValue.gameObject.SetActive(asActive);
	}

	public void updateCashTxt () {
		cashValue.text = Vars.cash.ToString() + "$";
	}

	public void updateChosenItemBorder (bool hideBorder) {
		if (hideBorder) chosenItemBorder.gameObject.SetActive (false);
		else chosenItemBorder.gameObject.SetActive (true);
		if (chosenItem != null) chosenItemBorder.position = chosenItem.transform.position;
	}
}