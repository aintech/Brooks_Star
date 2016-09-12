using UnityEngine;
using System.Collections;

public abstract class InventoryContainedScreen : MonoBehaviour, ButtonHolder {

	protected Button inventoryBtn, storageBtn;

	protected Inventory inventory, storage;
	
	protected InventoryItem draggedItem, chosenItem;
	
	protected Transform chosenItemBorder;
	
	protected Vector3 draggedItemPosition = Vector3.zero;

	protected Vector2 dragOffset;

	protected TextMesh itemName, itemLabel_1, itemValue_1, itemLabel_2, itemValue_2, itemEnergyLabel, itemEnergyValue, itemVolumeLabel, itemVolumeValue, itemCostLabel, itemCostValue;

	protected void innerInit() {
		Transform itemInformation = transform.Find("Item Information").transform;
		
		itemName = itemInformation.Find("ItemName").GetComponent<TextMesh> ();
		itemLabel_1 = itemInformation.Find("ItemLabel_1").GetComponent<TextMesh> ();
		itemValue_1 = itemInformation.Find("ItemValue_1").GetComponent<TextMesh> ();
		itemLabel_2 = itemInformation.Find("ItemLabel_2").GetComponent<TextMesh> ();
		itemValue_2 = itemInformation.Find("ItemValue_2").GetComponent<TextMesh> ();
		itemEnergyLabel = itemInformation.Find("ItemEnergyLabel").GetComponent<TextMesh> ();
		itemEnergyValue = itemInformation.Find("ItemEnergyValue").GetComponent<TextMesh> ();
		itemVolumeLabel = itemInformation.Find("ItemVolumeLabel").GetComponent<TextMesh> ();
		itemVolumeValue = itemInformation.Find("ItemVolumeValue").GetComponent<TextMesh> ();
		itemCostLabel = itemInformation.Find("ItemCostLabel").GetComponent<TextMesh> ();
		itemCostValue = itemInformation.Find("ItemCostValue").GetComponent<TextMesh> ();
		
		itemName.GetComponent<MeshRenderer> ().sortingLayerName = "Inventory";
		itemLabel_1.GetComponent<MeshRenderer> ().sortingLayerName = "Inventory";
		itemValue_1.GetComponent<MeshRenderer> ().sortingLayerName = "Inventory";
		itemLabel_2.GetComponent<MeshRenderer> ().sortingLayerName = "Inventory";
		itemValue_2.GetComponent<MeshRenderer> ().sortingLayerName = "Inventory";
		itemEnergyLabel.GetComponent<MeshRenderer> ().sortingLayerName = "Inventory";
		itemEnergyValue.GetComponent<MeshRenderer> ().sortingLayerName = "Inventory";
		itemVolumeLabel.GetComponent<MeshRenderer> ().sortingLayerName = "Inventory";
		itemVolumeValue.GetComponent<MeshRenderer> ().sortingLayerName = "Inventory";
		itemCostLabel.GetComponent<MeshRenderer> ().sortingLayerName = "Inventory";
		itemCostValue.GetComponent<MeshRenderer> ().sortingLayerName = "Inventory";

		itemName.GetComponent<MeshRenderer> ().sortingOrder = 3;
		itemLabel_1.GetComponent<MeshRenderer> ().sortingOrder = 3;
		itemValue_1.GetComponent<MeshRenderer> ().sortingOrder = 3;
		itemLabel_2.GetComponent<MeshRenderer> ().sortingOrder = 3;
		itemValue_2.GetComponent<MeshRenderer> ().sortingOrder = 3;
		itemEnergyLabel.GetComponent<MeshRenderer> ().sortingOrder = 3;
		itemEnergyValue.GetComponent<MeshRenderer> ().sortingOrder = 3;
		itemVolumeLabel.GetComponent<MeshRenderer> ().sortingOrder = 3;
		itemVolumeValue.GetComponent<MeshRenderer> ().sortingOrder = 3;
		itemCostLabel.GetComponent<MeshRenderer> ().sortingOrder = 3;
		itemCostValue.GetComponent<MeshRenderer> ().sortingOrder = 3;
		
		chosenItemBorder = transform.Find ("Chosen Item Border").transform;

		inventoryBtn = transform.Find("Inventory Button") == null? null: transform.Find("Inventory Button").GetComponent<Button>().init();
		storageBtn = transform.Find("Storage Button") == null? null: transform.Find("Storage Button").GetComponent<Button>().init();
	}

	void Update () {
		if (Input.GetMouseButtonDown(0) && Utils.hit != null) {
			if (Utils.hit.name.Equals("Cell")) {
				InventoryItem item = Utils.hit.transform.GetComponent<InventoryCell>().getItem();
				if (item != null) {
					if (!item.transform.parent.name.Equals("Ship Inventory")) {
						draggedItem = Utils.hit.transform.GetComponent<InventoryCell>().takeItem();
						draggedItem.GetComponent<Renderer>().sortingOrder = 4;
					}
					choseItem(item);
					chosenItemBorder.transform.position = item.transform.position;
					chosenItemBorder.gameObject.SetActive(true);
				}
			} else if (Utils.hit.name.Contains(" Slot")) {
				InventoryItem item = Utils.hit.transform.GetComponent<HullSlot>().getItem();
				if (item != null) {
					choseDraggedItemFromSlot(Utils.hit.transform.GetComponent<HullSlot>());
					chosenItemBorder.transform.position = item.transform.position;
					choseItem(item);
					chosenItemBorder.gameObject.SetActive(true);
				}
			} else {
//				checkBtnPress (Utils.hit.name);
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
	
	virtual protected void choseDraggedItemFromSlot (HullSlot slot) {
		draggedItem = slot.takeItem();
		draggedItem.GetComponent<Renderer>().sortingOrder = 4;
	}

	virtual protected void choseItem (InventoryItem item) {
		chosenItem = item;
		itemName.text = item.getItemName ();
		
		switch (item.getItemQuality()) {
			case InventoryItem.Quality.Normal: itemName.color = new Color(Color.white.r, Color.white.g, Color.white.b, 1); break;
			case InventoryItem.Quality.Superior: itemName.color = new Color(Color.yellow.r, Color.yellow.g, Color.yellow.b, 1); break;
			case InventoryItem.Quality.Unique: itemName.color = new Color(Color.blue.r, Color.blue.g, Color.blue.b, 1); break;
		}
		
		switch (item.getItemType()) {
			case InventoryItem.Type.Weapon:
				itemLabel_1.text = "Урон";
			itemValue_1.text = ((InventoryItem.WeaponData)item.getItemData()).getMinDamage().ToString() + "-" +
							   ((InventoryItem.WeaponData)item.getItemData()).getMaxDamage().ToString() + " ед.";
				itemLabel_2.text = "Перезарядка";
				itemValue_2.text = ((InventoryItem.WeaponData)item.getItemData()).getReloadTime().ToString("F2") + " c.";
				break;
			case InventoryItem.Type.Engine:
				itemLabel_1.text = "Мощность";
				itemValue_1.text = (((InventoryItem.EngineData)item.getItemData()).getPower() * 1000).ToString("F1") + " ед.";
				itemLabel_2.text = "";
				itemValue_2.text = "";
				break;
			case InventoryItem.Type.Armor:
				itemLabel_1.text = "Броня";
				itemValue_1.text = ((InventoryItem.ArmorData)item.getItemData()).getArmorClass().ToString() + " ед.";
				itemLabel_2.text = "";
				itemValue_2.text = "";
				break;
			case InventoryItem.Type.Generator:
				itemLabel_1.text = "Мощность";
				itemValue_1.text = ((InventoryItem.GeneratorData)item.getItemData()).getMaxEnergy().ToString() + " ед.";
				itemLabel_2.text = "";
				itemValue_2.text = "";
				break;
			case InventoryItem.Type.Radar:
				itemLabel_1.text = "Дальность";
				itemValue_1.text = ((InventoryItem.RadarData)item.getItemData()).getRange().ToString() + " ед.";
				itemLabel_2.text = "";
				itemValue_2.text = "";
				break;
			case InventoryItem.Type.Shield:
				itemLabel_1.text = "Защита";
				itemValue_1.text = ((InventoryItem.ShieldData)item.getItemData()).getShieldLevel().ToString() + " ед.";
				itemLabel_2.text = "Перезаряд";
				itemValue_2.text = ((InventoryItem.ShieldData)item.getItemData()).getRechargeSpeed().ToString() + " ед/c.";
				break;
			case InventoryItem.Type.RepairDroid:
				itemLabel_1.text = "Ремонт";
				itemValue_1.text = ((InventoryItem.RepairDroidData)item.getItemData()).getRepairSpeed().ToString() + " ед/с.";
				itemLabel_2.text = "";
				itemValue_2.text = "";
				break;
			case InventoryItem.Type.Harvester:
				itemLabel_1.text = "Поиск";
				itemValue_1.text = ((InventoryItem.HarvesterData)item.getItemData()).getHarvestTime().ToString() + " с.";
				itemLabel_2.text = "";
				itemValue_2.text = "";
				break;
			default:
				itemLabel_1.text = "";
				itemValue_1.text = "";
				itemLabel_2.text = "";
				itemValue_2.text = "";
				break;
		}
		
		itemEnergyLabel.text = "Питание";
		itemEnergyValue.text = item.getEnergyNeeded().ToString() + "E";
		itemVolumeLabel.text = "Объем";
		itemVolumeValue.text = item.getVolume().ToString("F1") + "V";
		itemCostLabel.text = "Стоимость";
		itemCostValue.text = "$ " + item.getCost();
		
		dragOffset.Set(Utils.mousePos.x - item.transform.position.x, Utils.mousePos.y - item.transform.position.y);
	}
	
	private void dropItem () {
		checkItemDrop ();
		draggedItem.GetComponent<Renderer>().sortingOrder = 3;
		if(chosenItem != null) chosenItemBorder.position = chosenItem.transform.position;
		draggedItem = null;
		afterItemDrop ();
	}

	virtual protected void checkItemDrop () {}
	virtual protected void afterItemDrop () {}

	protected void hideItemInfo () {
		itemName.text = "";
		itemLabel_1.text = "";
		itemValue_1.text = "";
		itemLabel_2.text = "";
		itemValue_2.text = "";
		itemEnergyLabel.text = "";
		itemEnergyValue.text = "";
		itemVolumeLabel.text = "";
		itemVolumeValue.text = "";
		itemCostLabel.text = "";
		itemCostValue.text = "";
		chosenItemBorder.gameObject.SetActive (false);
		chosenItem = null;
	}
	
	public InventoryItem getChosenItem () {
		return chosenItem;
	}

	public void updateChosenItemBorder () {
		if (chosenItem != null) {
			if (chosenItem.getCell() == null && chosenItem.getHullSlot() == null) {
				chosenItemBorder.gameObject.SetActive(false);
			} else {
				chosenItemBorder.transform.position = chosenItem.transform.position;
				chosenItemBorder.gameObject.SetActive (true);
			}
		} else {
			chosenItemBorder.gameObject.SetActive (false);
		}
	}
	
	public void updateChosenItemBorder (bool hideBorder) {
		if (hideBorder) chosenItemBorder.gameObject.SetActive (false);
		else chosenItemBorder.gameObject.SetActive (true);
		if (chosenItem != null) chosenItemBorder.position = chosenItem.transform.position;
	}
}