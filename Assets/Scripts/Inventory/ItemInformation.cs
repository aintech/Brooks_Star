using UnityEngine;
using System.Collections;

public class ItemInformation : MonoBehaviour {

	private TextMesh itemName, itemLabel_1, itemValue_1, itemLabel_2, itemValue_2, itemEnergyLabel, itemEnergyValue, itemVolumeLabel, itemVolumeValue, itemCostLabel, itemCostValue;

	public ItemInformation init () {
		itemName = transform.Find("ItemName").GetComponent<TextMesh> ();
		itemLabel_1 = transform.Find("ItemLabel_1").GetComponent<TextMesh> ();
		itemValue_1 = transform.Find("ItemValue_1").GetComponent<TextMesh> ();
		itemLabel_2 = transform.Find("ItemLabel_2").GetComponent<TextMesh> ();
		itemValue_2 = transform.Find("ItemValue_2").GetComponent<TextMesh> ();
		itemEnergyLabel = transform.Find("ItemEnergyLabel").GetComponent<TextMesh> ();
		itemEnergyValue = transform.Find("ItemEnergyValue").GetComponent<TextMesh> ();
		itemVolumeLabel = transform.Find("ItemVolumeLabel").GetComponent<TextMesh> ();
		itemVolumeValue = transform.Find("ItemVolumeValue").GetComponent<TextMesh> ();
		itemCostLabel = transform.Find("ItemCostLabel").GetComponent<TextMesh> ();
		itemCostValue = transform.Find("ItemCostValue").GetComponent<TextMesh> ();

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

		gameObject.SetActive(true);

		return this;
	}

	public void showItemInfo (Item item) {
		itemName.text = item.getItemName ();

		switch (item.getItemQuality()) {
			case Item.Quality.NORMAL: itemName.color = new Color(Color.white.r, Color.white.g, Color.white.b, 1); break;
			case Item.Quality.SUPERIOR: itemName.color = new Color(Color.yellow.r, Color.yellow.g, Color.yellow.b, 1); break;
			case Item.Quality.UNIQUE: itemName.color = new Color(Color.blue.r, Color.blue.g, Color.blue.b, 1); break;
		}

		switch (item.getItemType()) {
			case Item.Type.WEAPON:
				itemLabel_1.text = "Урон";
				itemValue_1.text = ((Item.WeaponData)item.getItemData()).getMinDamage().ToString() + "-" +
					((Item.WeaponData)item.getItemData()).getMaxDamage().ToString() + " ед.";
				itemLabel_2.text = "Перезарядка";
				itemValue_2.text = ((Item.WeaponData)item.getItemData()).getReloadTime().ToString("F2") + " c.";
				break;
			case Item.Type.ENGINE:
				itemLabel_1.text = "Мощность";
				itemValue_1.text = (((Item.EngineData)item.getItemData()).getPower() * 1000).ToString("F1") + " ед.";
				itemLabel_2.text = "";
				itemValue_2.text = "";
				break;
			case Item.Type.ARMOR:
				itemLabel_1.text = "Броня";
				itemValue_1.text = ((Item.ArmorData)item.getItemData()).getArmorClass().ToString() + " ед.";
				itemLabel_2.text = "";
				itemValue_2.text = "";
				break;
			case Item.Type.GENERATOR:
				itemLabel_1.text = "Мощность";
				itemValue_1.text = ((Item.GeneratorData)item.getItemData()).getMaxEnergy().ToString() + " ед.";
				itemLabel_2.text = "";
				itemValue_2.text = "";
				break;
			case Item.Type.RADAR:
				itemLabel_1.text = "Дальность";
				itemValue_1.text = ((Item.RadarData)item.getItemData()).getRange().ToString() + " ед.";
				itemLabel_2.text = "";
				itemValue_2.text = "";
				break;
			case Item.Type.SHIELD:
				itemLabel_1.text = "Защита";
				itemValue_1.text = ((Item.ShieldData)item.getItemData()).getShieldLevel().ToString() + " ед.";
				itemLabel_2.text = "Перезаряд";
				itemValue_2.text = ((Item.ShieldData)item.getItemData()).getRechargeSpeed().ToString() + " ед/c.";
				break;
			case Item.Type.REPAIR_DROID:
				itemLabel_1.text = "Ремонт";
				itemValue_1.text = ((Item.RepairDroidData)item.getItemData()).getRepairSpeed().ToString() + " ед/с.";
				itemLabel_2.text = "";
				itemValue_2.text = "";
				break;
			case Item.Type.HARVESTER:
				itemLabel_1.text = "Поиск";
				itemValue_1.text = ((Item.HarvesterData)item.getItemData()).getHarvestTime().ToString() + " с.";
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
	}

	public void clearInfo () {
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
	}
}