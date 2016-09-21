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
			case ItemData.Quality.NORMAL: itemName.color = new Color(Color.white.r, Color.white.g, Color.white.b, 1); break;
			case ItemData.Quality.SUPERIOR: itemName.color = new Color(Color.yellow.r, Color.yellow.g, Color.yellow.b, 1); break;
			case ItemData.Quality.UNIQUE: itemName.color = new Color(Color.blue.r, Color.blue.g, Color.blue.b, 1); break;
		}

		switch (item.getItemType()) {
			case ItemType.HAND_WEAPON:
				itemLabel_1.text = "Урон";
				itemValue_1.text = ((HandWeaponData)item.itemData).minDamage.ToString() + "-" +
					((HandWeaponData)item.itemData).maxDamage.ToString() + " ед.";
				itemLabel_2.text = "";
				itemValue_2.text = "";
				break;
			case ItemType.BODY_ARMOR:
				itemLabel_1.text = "Броня";
				itemValue_1.text = ((BodyArmorData)item.itemData).armorClass.ToString() + " ед.";
				itemLabel_2.text = "";
				itemValue_2.text = "";
				break;
			case ItemType.WEAPON:
				itemLabel_1.text = "Урон";
				itemValue_1.text = ((WeaponData)item.itemData).minDamage.ToString() + "-" +
					((WeaponData)item.itemData).maxDamage.ToString() + " ед.";
				itemLabel_2.text = "Перезарядка";
				itemValue_2.text = ((WeaponData)item.itemData).reloadTime.ToString("F2") + " c.";
				break;
			case ItemType.ENGINE:
				itemLabel_1.text = "Мощность";
				itemValue_1.text = (((EngineData)item.itemData).power * 1000).ToString("F1") + " ед.";
				itemLabel_2.text = "";
				itemValue_2.text = "";
				break;
			case ItemType.ARMOR:
				itemLabel_1.text = "Броня";
				itemValue_1.text = ((ArmorData)item.itemData).armorClass.ToString() + " ед.";
				itemLabel_2.text = "";
				itemValue_2.text = "";
				break;
			case ItemType.GENERATOR:
				itemLabel_1.text = "Мощность";
				itemValue_1.text = ((GeneratorData)item.itemData).maxEnergy.ToString() + " ед.";
				itemLabel_2.text = "";
				itemValue_2.text = "";
				break;
			case ItemType.RADAR:
				itemLabel_1.text = "Дальность";
				itemValue_1.text = ((RadarData)item.itemData).range.ToString() + " ед.";
				itemLabel_2.text = "";
				itemValue_2.text = "";
				break;
			case ItemType.SHIELD:
				itemLabel_1.text = "Защита";
				itemValue_1.text = ((ShieldData)item.itemData).shieldLevel.ToString() + " ед.";
				itemLabel_2.text = "Перезаряд";
				itemValue_2.text = ((ShieldData)item.itemData).rechargeSpeed.ToString() + " ед/c.";
				break;
			case ItemType.REPAIR_DROID:
				itemLabel_1.text = "Ремонт";
				itemValue_1.text = ((RepairDroidData)item.itemData).repairSpeed.ToString() + " ед/с.";
				itemLabel_2.text = "";
				itemValue_2.text = "";
				break;
			case ItemType.HARVESTER:
				itemLabel_1.text = "Поиск";
				itemValue_1.text = ((HarvesterData)item.itemData).harvestTime.ToString() + " с.";
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

		itemEnergyLabel.text = item.getEnergyNeeded() > 0? "Питание": "";
		itemEnergyValue.text = item.getEnergyNeeded() > 0? item.getEnergyNeeded().ToString() + "E": "";
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