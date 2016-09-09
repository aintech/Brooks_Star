using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Inventory : MonoBehaviour {

	public Transform inventoryItemPrefab;

	private InventoryContainedScreen containerScreen;

	public Sprite moveUpBtnSprite, moveDownBtnSprite, moveUpBtnSpriteDisabled, moveDownBtnSpriteDisabled;

	private float capacity;

	private InventoryCell[] cells;
	
	private Dictionary<int, InventoryItem> items = new Dictionary<int, InventoryItem>();

	private SpriteRenderer moveUpBtnRender, moveDownBtnRender;

	private BoxCollider2D moveUpBtn, moveDownBtn, sortBtn;

	private int offset = 0;

	private int offsetStep = 4;

	private bool scrollableUp, scrollableDown;

	private string volumeTxt = "";

	private TextMesh volumeMesh;

	public Inventory init () {
		cells = transform.GetComponentsInChildren<InventoryCell> ();
		moveUpBtn = transform.FindChild ("MoveUpBtn").GetComponent<BoxCollider2D> ();
		moveDownBtn = transform.FindChild ("MoveDownBtn").GetComponent<BoxCollider2D> ();
		sortBtn = transform.FindChild("SortBtn").GetComponent<BoxCollider2D>();
		moveUpBtnRender = transform.FindChild ("MoveUpBtn").GetComponent<SpriteRenderer>();
		moveDownBtnRender = transform.FindChild ("MoveDownBtn").GetComponent<SpriteRenderer>();

		volumeMesh = transform.FindChild ("VolumeTxt").GetComponent<TextMesh> ();
		MeshRenderer meshRend = transform.FindChild ("VolumeTxt").GetComponent<MeshRenderer> ();
		meshRend.sortingLayerName = "Inventory";
		meshRend.sortingOrder = 3;

		checkButtons ();

		gameObject.SetActive(false);

		return this;
	}

	public void setContainerScreen (InventoryContainedScreen containerScreen) {
		this.containerScreen = containerScreen;
	}

	void Update () {
		if (Input.GetMouseButtonDown (0) && Utils.hit != null) {
			if (Utils.hit == moveUpBtn) {
				if (scrollableUp) {
					offset -= offsetStep;
					afterScroll ();
				}
			} else if (Utils.hit == moveDownBtn) {
				if (scrollableDown) {
					offset += offsetStep;
					afterScroll ();
				}
			} else if (Utils.hit == sortBtn) {
				sortInventory();
				containerScreen.updateChosenItemBorder();
			}
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

		updateVolumeTxt ();
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
			source.updateVolumeTxt ();	
		}
		buybackInventory.addItemToFirstFreePosition (item, true);
		Variables.cash += item.getCost ();
	}

	public void addItemToCell (InventoryItem item, InventoryCell cell) {
		if (cell == null) {
			addItemToFirstFreePosition (item, true);
			return;
		}

		Inventory source = item.transform.parent.GetComponent<Inventory> ();

		if (source != this) {
			if (this.name.Equals ("Ship Inventory")) {
				item.returnToParentInventory ();
				return;
			} else if (this.name.Equals("Inventory")) {
				if (getFreeVolume() < item.getVolume()) {
					item.returnToParentInventory();
					Messenger.showMessage("Объёма багажника не достаточно для добавления предмета");
					return;
				}
			} else if (source != null && (source.name.Equals ("Market Inventory") || source.name.Equals ("Buyback Inventory"))) {
				if (!buyItem (item)) {
					item.returnToParentInventory ();
					return;
				}
			}
		}

		if (this.name.Equals("Buyback Inventory")) {
			addItemToFirstFreePosition(item, true);
			return;
		}
		if (source != null && source.name.Equals("Inventory")) source.updateVolumeTxt();

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
		if (Variables.cash >= item.getCost()) {
			if (getCapacity() > 0.0 && (getFreeVolume() - item.getVolume() < 0.0)) {
				return false;
			}
		} else {
			return false;
		}
		Variables.cash -= item.getCost ();
		return true;
	}

	private void afterScroll () {
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
	
	private void checkButtons () {
		if (offset != 0) {
			scrollableUp = true;
			moveUpBtnRender.sprite = moveUpBtnSprite;
		} else {
			scrollableUp = false;
			moveUpBtnRender.sprite = moveUpBtnSpriteDisabled;
		}
		if (getMaximumItemIndex () >= (cells.Length + offset - offsetStep)) {
			scrollableDown = true;
			moveDownBtnRender.sprite = moveDownBtnSprite;
		} else {
			scrollableDown = false;
			moveDownBtnRender.sprite = moveDownBtnSpriteDisabled;
		}
	}

	public float getFreeVolume () {
		float freeVolume = getCapacity ();
		foreach (KeyValuePair<int, InventoryItem> pair in getItems()) {
			freeVolume -= pair.Value.getVolume();	
		}
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
			ItemFactory.createItemData (item, rand <= 3? InventoryItem.Type.Weapon:
			                            	  rand == 4? InventoryItem.Type.Engine:
			                            	  rand == 5? InventoryItem.Type.Armor:
			                            	  rand == 6? InventoryItem.Type.Generator:
			                            	  rand == 7? InventoryItem.Type.Radar:
			                            	  rand == 8? InventoryItem.Type.Shield:
			                            	  rand == 9? InventoryItem.Type.RepairDroid:
			                            	  InventoryItem.Type.Harvester);

			addItemToFirstFreePosition(item, false);
		}

		refreshInventory ();
		sortInventory();
	}

	public void updateVolumeTxt () {
		volumeTxt = getFreeVolume().ToString ();
		if (volumeTxt.Contains (".")) {
			volumeTxt = volumeTxt.Remove(volumeTxt.IndexOf("."));	
		}
		volumeMesh.text = volumeTxt;
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
				case InventoryItem.Type.Weapon: weapons.Add(pair.Value); break;
				case InventoryItem.Type.Engine: engines.Add(pair.Value); break;
				case InventoryItem.Type.Armor: armors.Add(pair.Value); break;
				case InventoryItem.Type.Generator: generators.Add(pair.Value); break;
				case InventoryItem.Type.Radar: radars.Add(pair.Value); break;
				case InventoryItem.Type.Shield: shields.Add(pair.Value); break;
				case InventoryItem.Type.RepairDroid: repairDroids.Add(pair.Value); break;
				case InventoryItem.Type.Harvester: harvesters.Add(pair.Value); break;
			}
		}

		getItems().Clear();

		weapons = sortList(weapons, InventoryItem.Type.Weapon);
		engines = sortList(engines, InventoryItem.Type.Engine);
		armors = sortList(armors, InventoryItem.Type.Armor);
		generators = sortList(generators, InventoryItem.Type.Generator);
		radars = sortList(radars, InventoryItem.Type.Radar);
		shields = sortList(shields, InventoryItem.Type.Shield);
		repairDroids = sortList(repairDroids, InventoryItem.Type.RepairDroid);
		harvesters = sortList(harvesters, InventoryItem.Type.Harvester);

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
			if (type == InventoryItem.Type.Weapon) {
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
			} else if (type == InventoryItem.Type.Engine) {
				InventoryItem.EngineData data = (InventoryItem.EngineData) item.getItemData();
				weight = Mathf.RoundToInt(data.getPower() * 1000);
			} else if (type == InventoryItem.Type.Armor) {
				InventoryItem.ArmorData data = (InventoryItem.ArmorData) item.getItemData();
				weight = data.getArmorClass() * 1000;
			} else if (type == InventoryItem.Type.Generator) {
				InventoryItem.GeneratorData data = (InventoryItem.GeneratorData) item.getItemData();
				weight = data.getMaxEnergy();
			} else if (type == InventoryItem.Type.Radar) {
				InventoryItem.RadarData data = (InventoryItem.RadarData) item.getItemData();
				weight = data.getRange();
			} else if (type == InventoryItem.Type.Shield) {
				InventoryItem.ShieldData data = (InventoryItem.ShieldData) item.getItemData();
				weight = data.getShieldLevel();
			} else if (type == InventoryItem.Type.RepairDroid) {
				InventoryItem.RepairDroidData data = (InventoryItem.RepairDroidData) item.getItemData();
				weight = data.getRepairSpeed();
			} else if (type == InventoryItem.Type.Harvester) {
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
}