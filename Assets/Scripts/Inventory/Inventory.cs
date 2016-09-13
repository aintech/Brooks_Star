using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Inventory : MonoBehaviour, ButtonHolder {

	public Transform inventoryItemPrefab;

    private InventoryType inventoryType;

	private InventoryContainedScreen containerScreen;

	private float capacity = -1, freeVolume;

	private InventoryCell[] cells;
	
	private Dictionary<int, InventoryItem> items = new Dictionary<int, InventoryItem>();

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
		InventoryItem chosenItem = containerScreen.getChosenItem ();
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

		foreach (KeyValuePair<int, InventoryItem> pair in getItems ()) {
			InventoryItem item = pair.Value;
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

	public void loadItems (Dictionary<int, InventoryItem> items) {
		Dictionary<int, InventoryItem> tempDic = new Dictionary<int, InventoryItem> ();
		foreach (KeyValuePair<int, InventoryItem> pair in items) {
			InventoryItem item = (Instantiate(inventoryItemPrefab) as Transform).GetComponent<InventoryItem>();
			item.initialaizeFromSource (pair.Value);
			tempDic.Add(pair.Key, item);
		}
		this.items.Clear ();
		this.items = tempDic;
		refreshInventory ();
	}

	public void sellItemToTrader (InventoryItem item, Inventory buybackInventory) {
		Inventory source = item.getCell ().transform.parent.GetComponent<Inventory> ();
		if (source != null) {
			source.calculateFreeVolume();
		}
		buybackInventory.addItemToFirstFreePosition (item, true);
		Vars.cash += item.getCost ();
		Vars.userInterface.updateCash();
	}

	public void addItemToCell (InventoryItem item, InventoryCell cell) {
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
		InventoryItem prevItem = null;

		if (cell.getItem () != null) prevItem = cell.takeItem ();

		items.Add (cell.index + offset, item);

		if (prevItem != null) {
			if (source == this) addItemToCell (prevItem, prevCell);
			else addItemToFirstFreePosition (prevItem, false);
		}

		refreshInventory();
	}

	private void addItemToFirstFreePosition (InventoryItem item, bool refresh) {
		int newIndex = getMinFreeItemIndex ();
		getItems ().Add (newIndex, item);
		if (refresh) refreshInventory ();
	}

	private int getMinFreeItemIndex () {
		int maxIndex = getMaximumItemIndex();
		InventoryItem item = null;
		for (int i = 0; i <= maxIndex; i++) {
			items.TryGetValue(i, out item);
			if (item == null) return i;
		}
		return ++maxIndex;
	}
	
	private int getMaximumItemIndex () {
		int index = 0;
		foreach (KeyValuePair<int, InventoryItem> pair in items) {
			if (pair.Key > index) {
				index = pair.Key;
			}
		}
		return index;
	}

	private bool buyItem (InventoryItem item) {
		if (Vars.cash >= item.getCost()) {
			if (getCapacity() > 0.0 && (getFreeVolume() - item.getVolume() < 0.0)) {
				return false;
			}
		} else { return false; }
		Vars.cash -= item.getCost ();
		Vars.userInterface.updateCash();

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
		foreach (KeyValuePair<int, InventoryItem> pair in getItems()) {
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

	public void fillWithRandomItems (int count, string label) {
		for (int i = 0; i < count; i++) {
			InventoryItem item = Instantiate<Transform>(inventoryItemPrefab).GetComponent<InventoryItem>();
			item.transform.SetParent(transform);
			if (label != null) { item.name = label; }
			int rand = Mathf.RoundToInt (Random.value * 10);
			ItemFactory.createItemData (item, rand <= 3? InventoryItem.Type.WEAPON:
			                            	  rand == 4? InventoryItem.Type.ENGINE:
			                            	  rand == 5? InventoryItem.Type.ARMOR:
			                            	  rand == 6? InventoryItem.Type.GENERATOR:
			                            	  rand == 7? InventoryItem.Type.RADAR:
			                            	  rand == 8? InventoryItem.Type.SHIELD:
			                            	  rand == 9? InventoryItem.Type.REPAIR_DROID:
											  InventoryItem.Type.HARVESTER);

			addItemToFirstFreePosition(item, false);
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

	public Dictionary<int, InventoryItem> getItems () {
		return items;
	}

	public InventoryItem takeLastItem () {
		int index = getMaximumItemIndex();
		if (getCell(index) != null && getCell(index).getItem() != null) {
			return getCell(index).takeItem();
		}
		InventoryItem item;
		getItems().TryGetValue(index, out item);
		getItems().Remove(index);
		return item;
	}

	public void sortInventory () {
		List<InventoryItem> weapons = new List<InventoryItem>();
		List<InventoryItem> engines = new List<InventoryItem>();
		List<InventoryItem> armors = new List<InventoryItem>();
		List<InventoryItem> generators = new List<InventoryItem>();
		List<InventoryItem> radars = new List<InventoryItem>();
		List<InventoryItem> shields = new List<InventoryItem>();
		List<InventoryItem> repairDroids = new List<InventoryItem>();
		List<InventoryItem> harvesters = new List<InventoryItem>();

		foreach(KeyValuePair<int, InventoryItem> pair in getItems()) {
			switch (pair.Value.getItemType()) {
				case InventoryItem.Type.WEAPON: weapons.Add(pair.Value); break;
				case InventoryItem.Type.ENGINE: engines.Add(pair.Value); break;
				case InventoryItem.Type.ARMOR: armors.Add(pair.Value); break;
				case InventoryItem.Type.GENERATOR: generators.Add(pair.Value); break;
				case InventoryItem.Type.RADAR: radars.Add(pair.Value); break;
				case InventoryItem.Type.SHIELD: shields.Add(pair.Value); break;
				case InventoryItem.Type.REPAIR_DROID: repairDroids.Add(pair.Value); break;
				case InventoryItem.Type.HARVESTER: harvesters.Add(pair.Value); break;
			}
		}

		getItems().Clear();

		weapons = sortList(weapons, InventoryItem.Type.WEAPON);
		engines = sortList(engines, InventoryItem.Type.ENGINE);
		armors = sortList(armors, InventoryItem.Type.ARMOR);
		generators = sortList(generators, InventoryItem.Type.GENERATOR);
		radars = sortList(radars, InventoryItem.Type.RADAR);
		shields = sortList(shields, InventoryItem.Type.SHIELD);
		repairDroids = sortList(repairDroids, InventoryItem.Type.REPAIR_DROID);
		harvesters = sortList(harvesters, InventoryItem.Type.HARVESTER);

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

	private List<InventoryItem> sortList (List<InventoryItem> list, InventoryItem.Type type) {
		SortedDictionary<int, InventoryItem> weights = new SortedDictionary<int, InventoryItem>();
		int weight = 0;
		foreach (InventoryItem item in list) {
			if (type == InventoryItem.Type.WEAPON) {
				InventoryItem.WeaponData data = (InventoryItem.WeaponData) item.getItemData();
				switch (data.getType()) {
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
			} else if (type == InventoryItem.Type.ENGINE) {
				InventoryItem.EngineData data = (InventoryItem.EngineData) item.getItemData();
				weight = Mathf.RoundToInt(data.getPower() * 1000);
			} else if (type == InventoryItem.Type.ARMOR) {
				InventoryItem.ArmorData data = (InventoryItem.ArmorData) item.getItemData();
				weight = data.getArmorClass() * 1000;
			} else if (type == InventoryItem.Type.GENERATOR) {
				InventoryItem.GeneratorData data = (InventoryItem.GeneratorData) item.getItemData();
				weight = data.getMaxEnergy();
			} else if (type == InventoryItem.Type.RADAR) {
				InventoryItem.RadarData data = (InventoryItem.RadarData) item.getItemData();
				weight = data.getRange();
			} else if (type == InventoryItem.Type.SHIELD) {
				InventoryItem.ShieldData data = (InventoryItem.ShieldData) item.getItemData();
				weight = data.getShieldLevel();
			} else if (type == InventoryItem.Type.REPAIR_DROID) {
				InventoryItem.RepairDroidData data = (InventoryItem.RepairDroidData) item.getItemData();
				weight = data.getRepairSpeed();
			} else if (type == InventoryItem.Type.HARVESTER) {
				InventoryItem.HarvesterData data = (InventoryItem.HarvesterData) item.getItemData();
				weight = 1000000 - data.getHarvestTime();
			}
			while(weights.ContainsKey(weight)) {
				weight++;
			}
			weights.Add(weight, item);
		}

		list.Clear();

		foreach (KeyValuePair<int, InventoryItem> pair in weights) {
			list.Add(pair.Value);
		}
		list.Reverse();

		return list;
	}

	private int addSortToItems (List<InventoryItem> list, int count) {
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

    public InventoryType getInventoryType () {
        return inventoryType;
    }

    public enum InventoryType {
        INVENTORY, STORAGE, MARKET, INUSE, BUYBACK
    }
}