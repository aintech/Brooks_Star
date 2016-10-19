using UnityEngine;
using System.Collections;

public class LootDisplay : MonoBehaviour, ButtonHolder {

	private Button takeAllBtn, closeBtn;

	private LootContainer container;

	private Transform cameraTrans;

	private BoxCollider2D[] colls;

	private SpriteRenderer[] renders;

	private Vector3 pos;

	private Inventory inventory;

	public LootDisplay init (Inventory inventory) {
		this.inventory = inventory;
		cameraTrans = Camera.main.transform;
		takeAllBtn = transform.Find("Take All Button").GetComponent<Button>().init();
		closeBtn = transform.Find("Close Button").GetComponent<Button>().init();

		Transform holder = transform.Find("Holder");
		colls = new BoxCollider2D[holder.childCount];
		renders = new SpriteRenderer[holder.childCount];

		int index;
		Transform slot;
		char[] splitChar = new char[]{' '};
		for (int i = 0; i < holder.childCount; i++) {
			slot = holder.GetChild(i);
			index = int.Parse(slot.name.Split(splitChar, 2)[0]);
			colls[index] = slot.GetComponent<BoxCollider2D>();
			renders[index] = slot.GetComponent<SpriteRenderer>();
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
		if (Input.GetMouseButtonDown(0) && Utils.hit != null) {
			for (int i = 0; i < colls.Length; i++) {
				if (colls[i] == Utils.hit) { takeItem(i); break; }
				//Мешается коллайдер корабля - он висит выше кнопок дисплея
			}
		}
	}

	public void showDisplay (LootContainer container) {
		this.container = container;
		for (int i = 0; i < container.loot.Length; i++) {
			if (container.loot[i] != null) {
				renders[i].sprite = ImagesProvider.getItemSprite(container.loot[i]);
			}
		}
		StarSystem.setGamePause(true);
		Vars.userInterface.setEnabled(false);
		pos.Set(cameraTrans.position.x, cameraTrans.position.y, transform.position.z);
		transform.position = pos;
		gameObject.SetActive(true);
	}

	private void closeDisplay (bool disposeContainer) {
		gameObject.SetActive(false);
		if (disposeContainer) {
			container.hideDrop();
		} else {
			container.updateDisapear();
		}
		StarSystem.setGamePause(false);
		Vars.userInterface.setEnabled(true);
	}

	private void takeItem (int index) {
		if (container.loot[index] == null) { return; }

		ItemData data = container.loot[index];

		if (data.itemType == ItemType.GOODS) {
			
		} else {
			if (data.volume >= inventory.getFreeVolume()) {
				inventory.addItemToCell(Instantiate<Transform>(ItemFactory.itemPrefab).GetComponent<Item>().init(data), null);
			} else {
				Messenger.showMessage("Объёма инвентаря не достаточно для добавления предмета");
				return;
			}
		}

		bool allTaken = true;
		for (int i = 0; i < container.loot.Length; i++) {
			if (container.loot[i] != null) { allTaken = false; break; }
		}
		if (allTaken) { closeDisplay(true); }

		renders[index].sprite = null;
		container.loot[index] = null;
	}

	private void takeAll () {
		
	}

	public void fireClickButton (Button btn) {
		if (btn == takeAllBtn) { takeAll(); }
		else if (btn == closeBtn) { closeDisplay(false); }
	}
}