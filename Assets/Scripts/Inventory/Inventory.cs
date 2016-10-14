using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Inventory : MonoBehaviour, ButtonHolder {

	private Transform itemsContainer;

    private InventoryType inventoryType;

	public InventoryContainedScreen containerScreen { get; private set; }

	private float capacity = -1, freeVolume;

	private InventoryCell[] cells;
	
	private Dictionary<int, Item> items = new Dictionary<int, Item>();

	private Button upBtn, downBtn, sortBtn;

	private int offset = 0;

	private int offsetStep;

	private bool scrollableUp, scrollableDown;

	private TextMesh volumeMesh;

	public Inventory init (InventoryType inventoryType) {
		this.inventoryType = inventoryType;

		cells = transform.GetComponentsInChildren<InventoryCell> ();

		itemsContainer = transform.Find("Items Container");

        foreach (InventoryCell cell in cells) {
			cell.init(this, itemsContainer);
        }

		upBtn = transform.FindChild ("Up Button").GetComponent<Button> ().init();
		downBtn = transform.FindChild ("Down Button").GetComponent<Button> ().init();
		sortBtn = transform.FindChild("Sort Button").GetComponent<Button>().init();

		volumeMesh = transform.Find ("VolumeTxt").GetComponent<TextMesh> ();
		MeshRenderer meshRend = volumeMesh.GetComponent<MeshRenderer> ();
		meshRend.sortingLayerName = "Inventory";
		meshRend.sortingOrder = 3;

		sortBtn.gameObject.SetActive(inventoryType == InventoryType.INVENTORY);
		volumeMesh.gameObject.SetActive(inventoryType == InventoryType.INVENTORY);

		checkButtons ();

		gameObject.SetActive(false);

		return this;
	}

	public void setContainerScreen (InventoryContainedScreen containerScreen, int columnsCount) {
		this.containerScreen = containerScreen;
		this.offsetStep = columnsCount;
	}

	void Update () {
		if (Input.GetAxis("Mouse ScrollWheel") > 0 && Utils.hit != null) {
			if (Utils.hit.name.Equals("Cell") && Utils.hit.GetComponent<InventoryCell>().inventory == this) {
				scroll(true);
			}
		} else if (Input.GetAxis("Mouse ScrollWheel") < 0 && Utils.hit != null) {
			if (Utils.hit.name.Equals("Cell") && Utils.hit.GetComponent<InventoryCell>().inventory == this) {
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
		if (chosenItem != null && chosenItem.cell != null && chosenItem.cell.inventory == this) {
			foreach (InventoryCell cell in cells) {
				if (cell.item == chosenItem) {
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

	public void refreshInventory () {
		foreach (InventoryCell cell in cells) {
			cell.setItem (null);
		}

		foreach (KeyValuePair<int, Item> pair in items) {
			Item item = pair.Value;
			if (pair.Key >= offset && pair.Key < (cells.Length + offset)) {
				getCell (pair.Key - offset).setItem (item);
				item.gameObject.SetActive (true);
			} else {
				item.gameObject.SetActive (false);
			}
		}

		calculateFreeVolume();
		checkButtons ();
	}

	public void loadItems (Dictionary<int, ItemData> newItems) {
		clearInventory();
		foreach (KeyValuePair<int, ItemData> pair in newItems) {
			items.Add(pair.Key, Instantiate<Transform>(ItemFactory.itemPrefab).GetComponent<Item>().init(pair.Value, pair.Key));
		}
		refreshInventory ();
	}

	public void addItemToCell (Item item, InventoryCell cell) {
		if (cell == null) {
			addItemToFirstFreePosition (item, true);
			return;
		}

		Inventory source = item.cell == null? null: item.cell.inventory;

		if (source != this) {
			if (inventoryType == InventoryType.INVENTORY && item.slot == null) {
				if (getFreeVolume() < item.getVolume()) {
					item.returnToParent ();
					Messenger.showMessage("Объёма инвентаря не достаточно для добавления предмета");
					return;
				}
			}
		}

		if (source != null && source.inventoryType == InventoryType.INVENTORY) { source.calculateFreeVolume(); }

		InventoryCell prevCell = item.cell;
		Item prevItem = null;

		if (cell.item != null) prevItem = cell.takeItem ();

		item.index = cell.index + offset;
		items.Add (item.index, item);

		if (prevItem != null) {
			if (source == this) addItemToCell (prevItem, prevCell);
			else addItemToFirstFreePosition (prevItem, false);
		}

		refreshInventory();
	}

	private void addItemToFirstFreePosition (Item item, bool refresh) {
		int newIndex = getMinFreeItemIndex ();
		item.index = newIndex;
		items.Add (newIndex, item);
		if (refresh) refreshInventory ();
	}

	private int getMinFreeItemIndex () {
		int maxIndex = getMaximumItemIndex();
		for (int i = 0; i <= maxIndex; i++) {
			if (!items.ContainsKey(i)) { return i; }
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

	private void checkButtons () {
		scrollableUp = offset != 0;
		upBtn.setActive(scrollableUp);

		scrollableDown = getMaximumItemIndex () >= (cells.Length + offset);
		downBtn.setActive(scrollableDown);
	}

	public void calculateFreeVolume () {
		if (inventoryType != InventoryType.INVENTORY) { return; }
		freeVolume = getCapacity ();
		foreach (KeyValuePair<int, Item> pair in items) {
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
		fillWithRandomItems (Random.Range(20, 50), null);
	}

	public void fillWithRandomItems (int count, string label) {
		clearInventory();
		for (int i = 0; i < count; i++) {
			ItemData data = null;
			switch (Mathf.FloorToInt (Random.value * 15)) {
				case 0: case 1: data = ItemFactory.createItemData(ItemType.HAND_WEAPON); break;
				case 2: case 3: data = ItemFactory.createItemData(ItemType.BODY_ARMOR); break;
				case 4: data = ItemFactory.createItemData(ItemType.WEAPON); break;
				case 5: data = ItemFactory.createItemData(ItemType.ENGINE); break;
				case 6: data = ItemFactory.createItemData(ItemType.ARMOR); break;
				case 7: data = ItemFactory.createItemData(ItemType.GENERATOR); break;
				case 8: data = ItemFactory.createItemData(ItemType.RADAR); break;
				case 9: data = ItemFactory.createItemData(ItemType.SHIELD); break;
				case 10: data = ItemFactory.createItemData(ItemType.REPAIR_DROID); break;
				case 11: data = ItemFactory.createItemData (ItemType.HARVESTER); break;
				case 12: case 13: case 14: data = ItemFactory.createItemData(ItemType.GOODS); break;
			}
			Item item = Instantiate<Transform>(ItemFactory.itemPrefab).GetComponent<Item>().init(data);
			item.transform.SetParent(itemsContainer);
			if (label != null) { item.name = label; }
			addItemToFirstFreePosition(item, false);
		}

		refreshInventory ();
		sortInventory();
	}

	private void updateVolumeTxt () {
		if (inventoryType != InventoryType.INVENTORY) { return; }
		volumeMesh.text = "Объём: " + (freeVolume < 0? "<color=red>": "<color=orange>") + freeVolume.ToString("0.0") + "</color>";
	}

	public Dictionary<int, Item> getItems () {
		//По странной причине иногда после боя ячейки теряют свои предметы...
		if (items.Count == 0 && itemsContainer.childCount > 0) {
			Debug.Log("Strange shit happens: " + inventoryType + ": " + transform.name);
			recheckItems();
		}
		return items;
	}

	private void recheckItems () {
		Item item;
		for (int i = 0; i < itemsContainer.childCount; i++) {
			item = itemsContainer.GetChild(i).GetComponent<Item>();
			items.Add(item.index, item);
		}
		refreshInventory();
	}

	public void setItemsFromOtherInventory (Inventory inventory) {
		items = inventory.getItems();
		refreshInventory();
	}

	public Item takeLastItem () {
		int index = getMaximumItemIndex();
		if (getCell(index) != null && getCell(index).item != null) {
			return getCell(index).takeItem();
		}
		Item item = items[index];
		items.Remove(index);
		return item;
	}

	public void sortInventory () {
		List<Item> goods = new List<Item>();
		List<Item> handWeapons = new List<Item>();
		List<Item> bodyArmors = new List<Item>();
		List<Item> weapons = new List<Item>();
		List<Item> engines = new List<Item>();
		List<Item> armors = new List<Item>();
		List<Item> generators = new List<Item>();
		List<Item> radars = new List<Item>();
		List<Item> shields = new List<Item>();
		List<Item> repairDroids = new List<Item>();
		List<Item> harvesters = new List<Item>();

		foreach(KeyValuePair<int, Item> pair in items) {
			switch (pair.Value.getItemType()) {
				case ItemType.GOODS: goods.Add(pair.Value); break;
				case ItemType.HAND_WEAPON: handWeapons.Add(pair.Value); break;
				case ItemType.BODY_ARMOR: bodyArmors.Add(pair.Value); break;
				case ItemType.WEAPON: weapons.Add(pair.Value); break;
				case ItemType.ENGINE: engines.Add(pair.Value); break;
				case ItemType.ARMOR: armors.Add(pair.Value); break;
				case ItemType.GENERATOR: generators.Add(pair.Value); break;
				case ItemType.RADAR: radars.Add(pair.Value); break;
				case ItemType.SHIELD: shields.Add(pair.Value); break;
				case ItemType.REPAIR_DROID: repairDroids.Add(pair.Value); break;
				case ItemType.HARVESTER: harvesters.Add(pair.Value); break;
				default: Debug.Log("Unknown item type: " + pair.Value.getItemType()); break;
			}
		}

		items.Clear();

		goods = sortList(goods, ItemType.GOODS);
		handWeapons = sortList(handWeapons, ItemType.HAND_WEAPON);
		bodyArmors = sortList(bodyArmors, ItemType.BODY_ARMOR);
		weapons = sortList(weapons, ItemType.WEAPON);
		engines = sortList(engines, ItemType.ENGINE);
		armors = sortList(armors, ItemType.ARMOR);
		generators = sortList(generators, ItemType.GENERATOR);
		radars = sortList(radars, ItemType.RADAR);
		shields = sortList(shields, ItemType.SHIELD);
		repairDroids = sortList(repairDroids, ItemType.REPAIR_DROID);
		harvesters = sortList(harvesters, ItemType.HARVESTER);

		int counter = 0;
		counter = addSortToItems(goods, counter);
		counter = addSortToItems(handWeapons, counter);
		counter = addSortToItems(bodyArmors, counter);
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

	private List<Item> sortList (List<Item> list, ItemType type) {
		SortedDictionary<long, Item> weights = new SortedDictionary<long, Item>();
		long weight = 0;
		foreach (Item item in list) {
			if (type == ItemType.WEAPON) {
				WeaponData data = (WeaponData) item.itemData;
				switch (data.type) {
					case WeaponType.BLASTER: weight = 1000000; break;
					case WeaponType.PLASMER: weight = 2000000; break;
					case WeaponType.CHARGER: weight = 3000000; break;
					case WeaponType.EMITTER: weight = 4000000; break;
					case WeaponType.WAVER: weight = 5000000; break;
					case WeaponType.LAUNCHER: weight = 6000000; break;
					case WeaponType.SUPPRESSOR: weight = 7000000; break;
					default: Debug.Log("Неизвестный тип оружия"); break;
				}
				weight += item.getCost();
			} else if (type == ItemType.HAND_WEAPON) {
				HandWeaponData data = (HandWeaponData) item.itemData;
				weight = data.maxDamage * 1000000 + data.cost;
			} else if (type == ItemType.BODY_ARMOR) {
				BodyArmorData data = (BodyArmorData) item.itemData;
				weight = data.armorClass * 1000000 + data.cost;
			} else if (type == ItemType.ENGINE) {
				EngineData data = (EngineData) item.itemData;
				weight = Mathf.RoundToInt(data.power * 1000);
			} else if (type == ItemType.ARMOR) {
				ArmorData data = (ArmorData) item.itemData;
				weight = data.armorClass * 1000000 + data.cost;
			} else if (type == ItemType.GENERATOR) {
				GeneratorData data = (GeneratorData) item.itemData;
				weight = data.maxEnergy * 1000000 + data.cost;
			} else if (type == ItemType.RADAR) {
				RadarData data = (RadarData) item.itemData;
				weight = data.range * 1000000 + data.cost;
			} else if (type == ItemType.SHIELD) {
				ShieldData data = (ShieldData) item.itemData;
				weight = data.shieldLevel * 1000000 + data.cost;
			} else if (type == ItemType.REPAIR_DROID) {
				RepairDroidData data = (RepairDroidData) item.itemData;
				weight = data.repairSpeed * 1000000 + data.cost;
			} else if (type == ItemType.HARVESTER) {
				HarvesterData data = (HarvesterData) item.itemData;
				weight = 1000000 - data.harvestTime;
			} else if (type == ItemType.GOODS) {
				weight = item.itemData.cost;
			} else {
				Debug.Log("Unknown item: " + item.name);
			}
			while(weights.ContainsKey(weight)) {
				weight++;
			}
			weights.Add(weight, item);
		}

		list.Clear();

		foreach (KeyValuePair<long, Item> pair in weights) {
			list.Add(pair.Value);
		}
		list.Reverse();

		return list;
	}

	private int addSortToItems (List<Item> list, int count) {
		for (int i = 0; i < list.Count; i++) {
			items.Add(i + count, list[i]);
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
		Dictionary<int, Item> spare = new Dictionary<int, Item>(items);
		foreach (KeyValuePair<int, Item> pair in spare) {
			if (pair.Value.cell != null) {
				pair.Value.cell.takeItem();
			}
			Destroy(pair.Value.gameObject);
		}

		items.Clear();
	}

	public void sendToVars () {
		Dictionary<int, ItemData> inventoryToSend = null;
		switch (inventoryType) {
			case InventoryType.INVENTORY: inventoryToSend = Vars.inventory; break;
			case InventoryType.MARKET:
				switch (Vars.planetType) {
					case PlanetType.PARPARIS: inventoryToSend = Vars.market_parparis; break;
					case PlanetType.TERANA: inventoryToSend = Vars.market_terana; break;
				}
				break;
		}
		if (inventoryToSend == null) {
			Debug.Log("Unmapped inventory to send: " + inventoryType);
		} else {
			inventoryToSend.Clear();
			foreach (KeyValuePair<int, Item> pair in items) {
				inventoryToSend.Add(pair.Key, pair.Value.itemData);
			}
			clearInventory();
		}
	}

	public void initFromVars () {
		switch (inventoryType) {
			case InventoryType.INVENTORY: loadItems(Vars.inventory); break;
			case InventoryType.MARKET:
				switch (Vars.planetType) {
					case PlanetType.PARPARIS: loadItems(Vars.market_parparis); break;
					case PlanetType.TERANA: loadItems(Vars.market_terana); break;
					default: Debug.Log("Unknown planet market: " + Vars.planetType); break;
				}
				break;
			default: Debug.Log("Unknown inventory type: " + inventoryType); break;
		}
	}

    public InventoryType getInventoryType () {
        return inventoryType;
    }

    public enum InventoryType {
        INVENTORY,MARKET
    }
}