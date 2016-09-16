using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Inventory : MonoBehaviour, ButtonHolder {

	public Transform inventoryItemPrefab;

    private InventoryType inventoryType;

	private InventoryContainedScreen containerScreen;

	private float capacity = -1, freeVolume;

	private InventoryCell[] cells;
	
	private Dictionary<int, Item> items = new Dictionary<int, Item>();

	private Button upBtn, downBtn, sortBtn;

	private int offset = 0;

	private int offsetStep = 4;

	private bool scrollableUp, scrollableDown;

	private string volumeTxt = "";

	private TextMesh volumeMesh;

	private Vector3 normalScale = new Vector3(.08f, .1f, 1), decimalScale = new Vector3(.065f, .1f, 1);
    
	private Vector3 leftPosition = new Vector3(-4.5f, 0, 0), rightPosition = new Vector3(4.5f, 0, 0);

	private Collider2D inventoryColl;

	public Inventory init (InventoryType inventoryType) {
        this.inventoryType = inventoryType;

		cells = transform.GetComponentsInChildren<InventoryCell> ();

		inventoryColl = transform.GetComponent<Collider2D>();

        foreach (InventoryCell cell in cells) {
            cell.init(this);
        }

		upBtn = transform.FindChild ("Up Button").GetComponent<Button> ().init();
		downBtn = transform.FindChild ("Down Button").GetComponent<Button> ().init();
		sortBtn = transform.FindChild("Sort Button").GetComponent<Button>().init();

		volumeMesh = transform.FindChild ("VolumeTxt").GetComponent<TextMesh> ();
		MeshRenderer meshRend = transform.FindChild ("VolumeTxt").GetComponent<MeshRenderer> ();
		meshRend.sortingLayerName = "Inventory";
		meshRend.sortingOrder = 3;

		if (inventoryType == InventoryType.INVENTORY) {
			transform.Find("VolumeBG").gameObject.SetActive(false);
			volumeMesh.gameObject.SetActive(false);
		}

		checkButtons ();

		gameObject.SetActive(false);

		return this;
	}

	public void setPosition (bool left) {
		if ((left && transform.localPosition.x > 0) || (!left && transform.localPosition.x < 0)) {
			transform.localPosition = transform.localPosition * -1;
			upBtn.transform.localPosition = new Vector2(upBtn.transform.localPosition.x * -1, upBtn.transform.localPosition.y);
			downBtn.transform.localPosition = new Vector2(downBtn.transform.localPosition.x * -1, downBtn.transform.localPosition.y);
			sortBtn.transform.localPosition = new Vector2(sortBtn.transform.localPosition.x * -1, sortBtn.transform.localPosition.y);
		}
	}

	public void setContainerScreen (InventoryContainedScreen containerScreen) {
		this.containerScreen = containerScreen;
	}

	void Update () {
		if (Input.GetAxis("Mouse ScrollWheel") > 0 && Utils.hit != null) {
			if (Utils.hit == inventoryColl) {
				scroll(true);
			} else if (Utils.hit.name.Equals("Cell") && Utils.hit.GetComponent<InventoryCell>().getInventory() == this) {
				scroll(true);
			}
		} else if (Input.GetAxis("Mouse ScrollWheel") < 0 && Utils.hit != null) {
			if (Utils.hit == inventoryColl) {
				scroll(false);
			} else if (Utils.hit.name.Equals("Cell") && Utils.hit.GetComponent<InventoryCell>().getInventory() == this) {
				scroll(false);
			}
		}
	}

	public void fireClickButton (Button btn) {
		if (btn == upBtn) { scroll(true); }
		else if (btn == downBtn) { scroll(false); }
		else if (btn == sortBtn) {
			sortInventory();
			containerScreen.updateChosenItemBorder();
		} else { Debug.Log("Unknown button: " + btn.name); }
	}

	private void scroll (bool up) {
		offset += (up && scrollableUp)? -offsetStep: (!up && scrollableDown)? offsetStep: 0;
		refreshInventory ();
		Item chosenItem = containerScreen.getChosenItem ();
		if (chosenItem != null && chosenItem.transform.parent == this.transform) {
			foreach (InventoryCell cell in cells) {
				if (cell.getItem() == chosenItem) {
					containerScreen.updateChosenItemBorder (false);
					return;
				}
			}
			containerScreen.updateChosenItemBorder (true);
		}
	}

	public void setInventoryToBegin () {
		offset = 0;
		refreshInventory ();
	}

	private void refreshInventory () {
		foreach (InventoryCell cell in cells) {
			cell.setItem (null);
		}

		foreach (KeyValuePair<int, Item> pair in getItems ()) {
			Item item = pair.Value;
			item.setCell(null);
			if (pair.Key >= offset && pair.Key < (cells.Length + offset)) {
				InventoryCell cell = getCell (pair.Key - offset);
				cell.setItem (item);
				item.setCell (cell);
				item.transform.position = cell.transform.position;
				item.gameObject.SetActive (true);
			} else {
				item.gameObject.SetActive (false);
			}
			item.transform.SetParent(transform);
		}

		calculateFreeVolume();
		checkButtons ();
	}

	public void loadItems (Dictionary<int, ItemData> newItems) {
		clearInventory();
		foreach (KeyValuePair<int, ItemData> pair in newItems) {
			items.Add(pair.Key, Instantiate<Transform>(inventoryItemPrefab).GetComponent<Item>().init(pair.Value));
		}
		refreshInventory ();
	}

	public void sellItemToTrader (Item item, Inventory buybackInventory) {
		Inventory source = item.getCell ().transform.parent.GetComponent<Inventory> ();
		if (source != null) {
			source.calculateFreeVolume();
		}
		buybackInventory.addItemToFirstFreePosition (item, true);
		Vars.cash += item.getCost ();
	}

	public void addItemToCell (Item item, InventoryCell cell) {
		if (cell == null) {
			addItemToFirstFreePosition (item, true);
			return;
		}

		Inventory source = item.getCell() == null? null: item.getCell().getInventory();

		if (source != this) {
			if (inventoryType == InventoryType.BUYBACK) {
				item.returnToParentInventory ();
				return;
			} else if (inventoryType == InventoryType.INVENTORY) {
				if (getFreeVolume() < item.getVolume()) {
					item.returnToParentInventory();
					Messenger.showMessage("Объёма инвентаря не достаточно для добавления предмета");
					return;
				} else if (source != null && (source.inventoryType == InventoryType.MARKET || source.inventoryType == InventoryType.BUYBACK)) {
					if (!buyItem (item)) {
						item.returnToParentInventory ();
						return;
					}
				}
			}
		}

		if (inventoryType == InventoryType.BUYBACK) {
			addItemToFirstFreePosition(item, true);
			return;
		}
		if (source != null && source.inventoryType == InventoryType.INVENTORY) { source.calculateFreeVolume(); }

		InventoryCell prevCell = item.getCell();
		Item prevItem = null;

		if (cell.getItem () != null) prevItem = cell.takeItem ();

		items.Add (cell.index + offset, item);

		if (prevItem != null) {
			if (source == this) addItemToCell (prevItem, prevCell);
			else addItemToFirstFreePosition (prevItem, false);
		}

		refreshInventory();
	}

	private void addItemToFirstFreePosition (Item item, bool refresh) {
		int newIndex = getMinFreeItemIndex ();
		getItems ().Add (newIndex, item);
		if (refresh) refreshInventory ();
	}

	private int getMinFreeItemIndex () {
		int maxIndex = getMaximumItemIndex();
		Item item = null;
		for (int i = 0; i <= maxIndex; i++) {
			items.TryGetValue(i, out item);
			if (item == null) return i;
		}
		return ++maxIndex;
	}
	
	private int getMaximumItemIndex () {
		int index = 0;
		foreach (KeyValuePair<int, Item> pair in items) {
			if (pair.Key > index) {
				index = pair.Key;
			}
		}
		return index;
	}

	private bool buyItem (Item item) {
		if (Vars.cash >= item.getCost()) {
			if (getCapacity() > 0.0 && (getFreeVolume() - item.getVolume() < 0.0)) {
				return false;
			}
		} else { return false; }
		Vars.cash -= item.getCost ();

		return true;
	}

	private void checkButtons () {
		scrollableUp = offset != 0;
		upBtn.setActive(scrollableUp);

		scrollableDown = getMaximumItemIndex () >= (cells.Length + offset - offsetStep);
		downBtn.setActive(scrollableDown);
	}

	public void calculateFreeVolume () {
		if (inventoryType != InventoryType.INVENTORY) { return; }
		freeVolume = getCapacity ();
		foreach (KeyValuePair<int, Item> pair in getItems()) {
			freeVolume -= pair.Value.getVolume();	
		}
		updateVolumeTxt();
	}

	public float getFreeVolume () {
		return freeVolume;
	}

	private InventoryCell getCell (int index) {
		foreach (InventoryCell cell in cells) {
			if (cell.index == index) {
				return cell;
			}
		}
		return null;
	}

	public void fillWithRandomItems () {
		fillWithRandomItems (Random.Range(10, 50), null);
	}

	public void fillWithRandomItems (int count, string label) {
		clearInventory();
		for (int i = 0; i < count; i++) {
			ItemData data = null;
			switch (Mathf.RoundToInt (Random.value * 10)) {
				case 1: case 2: case 3: data = ItemFactory.createItemData(ItemData.Type.WEAPON); break;
				case 4: data = ItemFactory.createItemData(ItemData.Type.ENGINE); break;
				case 5: data = ItemFactory.createItemData(ItemData.Type.ARMOR); break;
				case 6: data = ItemFactory.createItemData(ItemData.Type.GENERATOR); break;
				case 7: data = ItemFactory.createItemData(ItemData.Type.RADAR); break;
				case 8: data = ItemFactory.createItemData(ItemData.Type.SHIELD); break;
				case 9: data = ItemFactory.createItemData(ItemData.Type.REPAIR_DROID); break;
			}
			if (data != null) {
				Item item = Instantiate<Transform>(inventoryItemPrefab).GetComponent<Item>().init(data);
				item.transform.SetParent(transform);
				if (label != null) { item.name = label; }
				addItemToFirstFreePosition(item, false);
			}
		}

		refreshInventory ();
		sortInventory();
	}

	private void updateVolumeTxt () {
		if (inventoryType != InventoryType.INVENTORY) { return; }
		if (freeVolume >= 100) {
			volumeTxt = "99";
		} else if (freeVolume < 0) {
			volumeTxt = "0";
		} else {
			volumeTxt = string.Format(freeVolume < 10? "{0:F1}": "{0:D}", freeVolume.ToString());
		}

		volumeMesh.text = volumeTxt;
		volumeMesh.transform.localScale = freeVolume < 10? decimalScale: normalScale;
	}

	public Dictionary<int, Item> getItems () {
		return items;
	}

	public Item takeLastItem () {
		int index = getMaximumItemIndex();
		if (getCell(index) != null && getCell(index).getItem() != null) {
			return getCell(index).takeItem();
		}
		Item item;
		getItems().TryGetValue(index, out item);
		getItems().Remove(index);
		return item;
	}

	public void sortInventory () {
		List<Item> weapons = new List<Item>();
		List<Item> engines = new List<Item>();
		List<Item> armors = new List<Item>();
		List<Item> generators = new List<Item>();
		List<Item> radars = new List<Item>();
		List<Item> shields = new List<Item>();
		List<Item> repairDroids = new List<Item>();
		List<Item> harvesters = new List<Item>();

		foreach(KeyValuePair<int, Item> pair in getItems()) {
			switch (pair.Value.getItemType()) {
				case ItemData.Type.WEAPON: weapons.Add(pair.Value); break;
				case ItemData.Type.ENGINE: engines.Add(pair.Value); break;
				case ItemData.Type.ARMOR: armors.Add(pair.Value); break;
				case ItemData.Type.GENERATOR: generators.Add(pair.Value); break;
				case ItemData.Type.RADAR: radars.Add(pair.Value); break;
				case ItemData.Type.SHIELD: shields.Add(pair.Value); break;
				case ItemData.Type.REPAIR_DROID: repairDroids.Add(pair.Value); break;
				case ItemData.Type.HARVESTER: harvesters.Add(pair.Value); break;
			}
		}

		getItems().Clear();

		weapons = sortList(weapons, ItemData.Type.WEAPON);
		engines = sortList(engines, ItemData.Type.ENGINE);
		armors = sortList(armors, ItemData.Type.ARMOR);
		generators = sortList(generators, ItemData.Type.GENERATOR);
		radars = sortList(radars, ItemData.Type.RADAR);
		shields = sortList(shields, ItemData.Type.SHIELD);
		repairDroids = sortList(repairDroids, ItemData.Type.REPAIR_DROID);
		harvesters = sortList(harvesters, ItemData.Type.HARVESTER);

		int counter = 0;
		counter = addSortToItems(weapons, counter);
		counter = addSortToItems(engines, counter);
		counter = addSortToItems(armors, counter);
		counter = addSortToItems(generators, counter);
		counter = addSortToItems(radars, counter);
		counter = addSortToItems(shields, counter);
		counter = addSortToItems(repairDroids, counter);
		counter = addSortToItems(harvesters, counter);

		refreshInventory ();
	}

	private List<Item> sortList (List<Item> list, ItemData.Type type) {
		SortedDictionary<int, Item> weights = new SortedDictionary<int, Item>();
		int weight = 0;
		foreach (Item item in list) {
			if (type == ItemData.Type.WEAPON) {
				WeaponData data = (WeaponData) item.getItemData();
				switch (data.type) {
					case WeaponType.Blaster: weight = 1000000; break;
					case WeaponType.Plasmer: weight = 2000000; break;
					case WeaponType.Charger: weight = 3000000; break;
					case WeaponType.Emitter: weight = 4000000; break;
					case WeaponType.Waver: weight = 5000000; break;
					case WeaponType.Launcher: weight = 6000000; break;
					case WeaponType.Suppressor: weight = 7000000; break;
					default: Debug.Log("Неизвестный тип оружия"); break;
				}
				weight += item.getCost();
			} else if (type == ItemData.Type.ENGINE) {
				EngineData data = (EngineData) item.getItemData();
				weight = Mathf.RoundToInt(data.power * 1000);
			} else if (type == ItemData.Type.ARMOR) {
				ArmorData data = (ArmorData) item.getItemData();
				weight = data.armorClass * 1000;
			} else if (type == ItemData.Type.GENERATOR) {
				GeneratorData data = (GeneratorData) item.getItemData();
				weight = data.maxEnergy;
			} else if (type == ItemData.Type.RADAR) {
				RadarData data = (RadarData) item.getItemData();
				weight = data.range;
			} else if (type == ItemData.Type.SHIELD) {
				ShieldData data = (ShieldData) item.getItemData();
				weight = data.shieldLevel;
			} else if (type == ItemData.Type.REPAIR_DROID) {
				RepairDroidData data = (RepairDroidData) item.getItemData();
				weight = data.repairSpeed;
			} else if (type == ItemData.Type.HARVESTER) {
				HarvesterData data = (HarvesterData) item.getItemData();
				weight = 1000000 - data.harvestTime;
			}
			while(weights.ContainsKey(weight)) {
				weight++;
			}
			weights.Add(weight, item);
		}

		list.Clear();

		foreach (KeyValuePair<int, Item> pair in weights) {
			list.Add(pair.Value);
		}
		list.Reverse();

		return list;
	}

	private int addSortToItems (List<Item> list, int count) {
		for (int i = 0; i < list.Count; i++) {
			getItems().Add(i + count, list[i]);
		}
		return count + list.Count;
	}

	public InventoryCell[] getCells () {
		return cells;
	}

	public void setCapacity (float capacity) {
		this.capacity = capacity;
	}
	
	public float getCapacity () {
		return capacity;
	}

	public int getOffset () {
		return offset;
	}

	private void clearInventory () {
		Dictionary<int, Item> spare = new Dictionary<int, Item>(getItems());
		foreach (KeyValuePair<int, Item> pair in spare) {
			if (pair.Value.getCell() != null) {
				pair.Value.getCell().takeItem();
			}
			Destroy(pair.Value.gameObject);
		}
		getItems().Clear();
	}

	public void sendToVars () {
		Dictionary<int, ItemData> inventoryToSend = null;
		switch (inventoryType) {
			case InventoryType.INVENTORY: inventoryToSend = Vars.inventory; break;
			case InventoryType.STORAGE: inventoryToSend = Vars.storage; break;
			case InventoryType.MARKET:
				switch (Vars.planetType) {
					case PlanetType.CORAS: inventoryToSend = Vars.marketCORAS; break;
				}
				break;
		}
		if (inventoryToSend == null) {
			Debug.Log("Unmapped inventory to send");
		} else {
			inventoryToSend.Clear();
			foreach (KeyValuePair<int, Item> pair in items) {
				inventoryToSend.Add(pair.Key, pair.Value.getItemData());
			}
			clearInventory();
		}
	}

	public void initFromVars () {
		switch (inventoryType) {
			case InventoryType.INVENTORY: loadItems(Vars.inventory); break;
		}
	}

    public InventoryType getInventoryType () {
        return inventoryType;
    }

    public enum InventoryType {
        INVENTORY, STORAGE, MARKET, BUYBACK
    }
}