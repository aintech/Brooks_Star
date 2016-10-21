using UnityEngine;
using System.Collections;

public class LootDisplay : MonoBehaviour, ButtonHolder {

	private Button takeAllBtn, closeBtn;

	private LootContainer container;

	private LootSlot[] slots;

	private Transform cameraTrans;

	private Vector3 pos;

	private Inventory inventory;

	private ItemDescriptor itemDescriptor;

	private QuantityPopup popup;

	private LootSlot takeFromSlot;

	public LootDisplay init (Inventory inventory, ItemDescriptor itemDescriptor) {
		this.inventory = inventory;
		this.itemDescriptor = itemDescriptor;
		cameraTrans = Camera.main.transform;
		takeAllBtn = transform.Find("Take All Button").GetComponent<Button>().init();
		closeBtn = transform.Find("Close Button").GetComponent<Button>().init();
		popup = transform.Find("Popup").GetComponent<QuantityPopup>().init(this);

		Transform holder = transform.Find("Holder");
		slots = new LootSlot[holder.childCount];

		int index;
		Transform slot;
		char[] splitChar = new char[]{' '};
		for (int i = 0; i < holder.childCount; i++) {
			slot = holder.GetChild(i);
			index = int.Parse(slot.name.Split(splitChar, 2)[0]);
			slots[index] = slot.GetComponent<LootSlot>();
		}

		takeAllBtn.gameObject.SetActive(true);
		closeBtn.gameObject.SetActive(true);
		holder.gameObject.SetActive(true);
		transform.Find("Background").gameObject.SetActive(true);
		gameObject.SetActive(false);

		pos = transform.position;

		return this;
	}

	void Update () {
		if (popup.onScreen) { return; }
		if (Input.GetKeyDown(KeyCode.Escape)) {
			closeDisplay(false);
		}
		if (Input.GetMouseButtonDown(0) && Utils.hit != null) {
			LootSlot slot = Utils.hit.GetComponent<LootSlot>();
			if (slot != null) {
				takeItem(slot);
			}
		}
	}

	public void showDisplay (LootContainer container) {
		this.container = container;
		for (int i = 0; i < container.loot.Count; i++) {
			slots[i].setItem(container.loot[i]);
		}
		StarSystem.setGamePause(true);
		UserInterface.showInterface = false;
		pos.Set(cameraTrans.position.x, cameraTrans.position.y, transform.position.z);
		transform.position = pos;
		itemDescriptor.setEnabled(null, ItemDescriptor.Type.LOOT, null);
		itemDescriptor.setSpaceOffset(transform.localPosition);
		popup.adjustPosition(transform.position);
		gameObject.SetActive(true);
	}

	private void closeDisplay (bool disposeContainer) {
		gameObject.SetActive(false);
		foreach (LootSlot slot in slots) {
			slot.takeItem();
		}
		if (disposeContainer) {
			container.hideDrop();
		} else {
			container.updateDisapear();
		}
		StarSystem.setGamePause(false);
		UserInterface.showInterface = true;
	}

	private void takeItem (LootSlot slot) {
		if (slot.item == null) { return; }

		ItemData data = slot.item.itemData;

		if (data.itemType == ItemType.GOODS) {
			takeFromSlot = slot;
			itemDescriptor.setDisabled();
			popup.show(slot.item);
		} else {
			if (data.volume <= inventory.getFreeVolume()) {
				container.loot.Remove(slot.item);
				inventory.addItemToCell(slot.takeItem(), null);
			} else {
				Messenger.inventoryCapacityLow(data.name, data.quantity);
				return;
			}
			checkAllTaken();
		}
	}

	public void applyItemTake(int count) {
		if (count > 0) {
			if (takeFromSlot.item.quantity == count) {
				container.loot.Remove(takeFromSlot.item);
				inventory.addItemToCell(takeFromSlot.takeItem(), null);
			} else {
				takeFromSlot.item.quantity -= count;
				Item newItem = Instantiate<Transform>(ItemFactory.itemPrefab).GetComponent<Item>().init(DataCopier.copy(takeFromSlot.item.itemData));
				newItem.quantity = count;
				inventory.addItemToCell(newItem, null);
			}
			checkAllTaken();
		}
		itemDescriptor.setEnabled(null, ItemDescriptor.Type.LOOT, null);
	}

	private void checkAllTaken () {
		for (int i = 0; i < slots.Length; i++) {
			if (slots[i].item != null) { return; }
		}
		closeDisplay(true);
	}

	private void takeAll () {
		Item item;
		for (int i = 0; i < slots.Length; i++) {
			item = slots[i].item;
			if (item == null) { continue; }

			if ((item.quantity * item.itemData.volume) <= inventory.getFreeVolume()) {
				slots[i].takeItem();
				container.loot.Remove(item);
				inventory.addItemToCell(item, null);
			} else {
				Messenger.inventoryCapacityLow(item.itemName, item.quantity);
			}
		}
		checkAllTaken();
	}

	public void fireClickButton (Button btn) {
		if (btn == takeAllBtn) { takeAll(); }
		else if (btn == closeBtn) { closeDisplay(false); }
	}
}