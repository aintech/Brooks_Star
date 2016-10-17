using UnityEngine;
using System.Collections;

public class ItemDescriptor : MonoBehaviour {

	private Transform 	trans,
						namePre, nameBG,
						qualityPre, qualityBG,
						pre1, bg1,
						pre2, bg2,
						pre3, bg3,
						pre4, bg4,
						pre5, bg5;

	public bool onScreen { get; private set; }

	private bool perkDescriptor;

	private ItemHolder holder, tempHolder;

	private Item item;

	private Perk perk, tempPerk;

	private TextMesh qualityValue, nameValue, value1, value2, value3, value4, value5;

	private MeshRenderer qualityRender, nameRender, value1Render, value2Render,value3Render, value4Render, value5Render;

	private Vector3 pos = Vector3.zero, spaceOffset = Vector3.zero;

	private Color32 goodColor = new Color32(176, 195, 217, 255),
					superiorColor = new Color32(94, 152, 217, 255),
					rareColor = new Color32(136, 71, 255, 255),
					uniqueColor = new Color32(173, 229, 92, 255),
					artefactColor = new Color32(235, 75, 75, 255);

	private Color32 normalDescriptionColor = new Color32(255, 255, 255, 255),
					perkDescriptionColor = new Color32(94, 152, 217, 255);

	private Vector3 scale = Vector3.one;

	private Inventory playerInventory;

	private float minY = -10, maxX = 10, screenWidth;

//	private Vector2[] positions = new Vector2[] { new Vector2(.5f, -.57f), new Vector2(.5f, -.95f), new Vector2(0, -1.33f), new Vector2(0, -1.71f), new Vector2(0, -2.09f) };
//
//	private Vector2[,] positions = new Vector2[,]{
//		{new Vector2(.15f, -.57f), new Vector2(.297f, -.57f), new Vector2(.295f, -.76f)},
//		{new Vector2(.15f, -.95f), new Vector2(.297f, -.95f), new Vector2(.295f, -1.14f)},
//		{new Vector2(.15f, -1.33f), new Vector2(.297f, -1.33f), new Vector2(.295f, -1.52f)},
//		{new Vector2(.15f, -1.71f), new Vector2(.297f, -1.71f), new Vector2(.295f, -1.9f)},
//		{new Vector2(.15f, -2.09f), new Vector2(.297f, -2.09f), new Vector2(.295f, -2.28f)},
//		{new Vector2(.15f, -2.47f), new Vector2(.297f, -2.47f), new Vector2(.295f, -2.66f)}
//	}; 

	private Type inventoryType;

	private EquipmentsMarket market;

	public ItemDescriptor init () {
		trans = transform.Find ("Descriptor");
		qualityPre = trans.Find ("Quality Pre");
		qualityBG = trans.Find("Quality Background");
		namePre = trans.Find ("Name Pre");
		nameBG = trans.Find("Name Background");
		pre1 = trans.Find ("Pre 1");
		bg1 = trans.Find ("Background 1");
		pre2 = trans.Find ("Pre 2");
		bg2 = trans.Find ("Background 2");
		pre3 = trans.Find ("Pre 3");
		bg3 = trans.Find ("Background 3");
		pre4 = trans.Find ("Pre 4");
		bg4 = trans.Find ("Background 4");
		pre5 = trans.Find ("Pre 5");
		bg5 = trans.Find ("Background 5");

		qualityValue = trans.Find("Quality Value").GetComponent<TextMesh>();
		nameValue = trans.Find("Name Value").GetComponent<TextMesh>();
		value1 = trans.Find ("Value 1").GetComponent<TextMesh> ();
		value2 = trans.Find ("Value 2").GetComponent<TextMesh> ();
		value3 = trans.Find ("Value 3").GetComponent<TextMesh> ();
		value4 = trans.Find ("Value 4").GetComponent<TextMesh> ();
		value5 = trans.Find ("Value 5").GetComponent<TextMesh> ();

		qualityRender = qualityValue.GetComponent<MeshRenderer>();
		nameRender = nameValue.GetComponent<MeshRenderer>();
		value1Render = value1.GetComponent<MeshRenderer>();
		value2Render = value2.GetComponent<MeshRenderer>();
		value3Render = value3.GetComponent<MeshRenderer>();
		value4Render = value4.GetComponent<MeshRenderer>();
		value5Render = value5.GetComponent<MeshRenderer>();

		MeshRenderer mesh;
		for (int i = 0; i < trans.childCount; i++) {
			mesh = trans.GetChild (i).GetComponent<MeshRenderer> ();
			if (mesh != null) {
				mesh.sortingLayerName = "User Interface";
				mesh.sortingOrder = 9;
			}
		}
		screenWidth = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height)).x;

		hide ();

		return this;
	}

	public void setSpaceOffset (Vector3 spaceOffset) {
		this.spaceOffset = spaceOffset;
	}

	public void initPlayerInventory (Inventory playerInventory) {
		this.playerInventory = playerInventory;
	}

	public void setAsPerkDescriptor (bool perkDescriptor) {
		this.perkDescriptor = perkDescriptor;
		nameValue.color = perkDescriptor? perkDescriptionColor: normalDescriptionColor;
		if (perkDescriptor) {
			for (int i = 0; i < trans.childCount; i++) {
				trans.GetChild (i).gameObject.SetActive (false);
			}
			namePre.gameObject.SetActive (true);
			nameBG.gameObject.SetActive (true);
			nameValue.gameObject.SetActive (true);
			pre1.gameObject.SetActive (true);
			bg1.gameObject.SetActive (true);
			value1.gameObject.SetActive (true);
		}
		minY = -10;
	}

	public void setDisabled () {
		enabled = false;
		hide();
	}

	public void setEnabled (Inventory inventory, Type type, InventoryContainedScreen container) {
		enabled = true;
		this.inventoryType = type;
		if (type == Type.MARKET_BUY || type == Type.MARKET_SELL) {
			market = (EquipmentsMarket)container;
		}
	}

	void Update () {
		if (Input.GetMouseButtonDown(1) && !perkDescriptor) {
			passRightClick();
		}
		if (onScreen) {
			if (Utils.hit == null) {
				hide ();
			} else if (Utils.hit != null) {
				if (perkDescriptor) {
					tempPerk = Utils.hit.GetComponent<Perk>();
					if (tempPerk == null) {
						hide();
					} else if (tempPerk.perkType != perk.perkType) {
						showDescription(tempPerk);
					}
				} else {
					tempHolder = Utils.hit.GetComponent<ItemHolder> ();
					if (tempHolder == null || tempHolder.item == null) {
						hide ();
					} else if (tempHolder != holder || tempHolder.item != item) {
						showDescription (tempHolder);
					}
				}
			}
			pos = Utils.mousePos;
			if (pos.y < minY + spaceOffset.y) { pos.y = minY + spaceOffset.y; }
			if (pos.x > maxX + spaceOffset.x) { pos.x = maxX + spaceOffset.x; }
			trans.localPosition = pos;
		} else {
			if (Utils.hit != null) {
				if (perkDescriptor) {
					perk = Utils.hit.GetComponent<Perk>();
					if (perk != null) { showDescription(perk); }
				} else {
					holder = Utils.hit.GetComponent<ItemHolder>();
					if (holder != null && holder.item != null) {
						showDescription(holder);
					}
				}
			}
		}
	}

	private void showDescription (Perk perk) {
		this.perk = perk;

		nameValue.text = perk.perkType.getName();
		scale.x = nameValue.text.Length + 1;
		nameBG.localScale = scale;

		value1.text = perk.perkType.getDescription() + " <color=lime>+" + (perk.perkType.getValuePerLevel() * Player.getPerkLevel(perk.perkType)) + "%</color>";
		scale.x = value1.text.Length - 20.5f;
		bg1.localScale = scale;

		maxX = screenWidth - value1Render.bounds.size.x - .5f;

		onScreen = true;
		Update();
		trans.gameObject.SetActive(true);
	}

	private void showDescription (ItemHolder holder) {
		this.holder = holder;
		this.item = holder.item;

		for (int i = 0; i < trans.childCount; i++) {
			trans.GetChild (i).gameObject.SetActive (false);
		}

		showTexts (item.itemData);

		onScreen = true;
		Update();
		trans.gameObject.SetActive(true);
	}

	private void passRightClick () {
		if (inventoryType == Type.MARKET_BUY) { market.askToBuy(item); }
		else if (inventoryType == Type.MARKET_SELL) { market.askToSell(item); }
	}

	private float setCost(int index, ItemData data) {
		string text = (inventoryType == Type.MARKET_BUY? "Купить за": inventoryType == Type.MARKET_SELL? "Продать за": "Стоимость:")  + " <color=yellow>" + data.cost +
			(data.quantity == 1? "$</color>": (" (" + (data.quantity * data.cost) + ")$</color>"));
		scale.x = text.Length - (data.quantity == 1? 22.5f: 24f);
		minY = - 4.7f + (.4f * index);// + transform.localPosition.y;
		switch (index) {
			case 1: value1.text = text; bg1.localScale = scale; return value1Render.bounds.size.x;
			case 2: value2.text = text; bg2.localScale = scale; return value2Render.bounds.size.x;
			case 3: value3.text = text; bg3.localScale = scale; return value3Render.bounds.size.x;
			case 4: value4.text = text; bg4.localScale = scale; return value4Render.bounds.size.x;
			case 5: value5.text = text; bg5.localScale = scale; return value5Render.bounds.size.x;
			default: Debug.Log("Unknown index: " + index); return 0;
		}
	}

	private void showTexts (ItemData data) {
		float maxLenght = 0;

		if (data.quality != ItemQuality.COMMON) {
			qualityPre.gameObject.SetActive (true);
			qualityBG.gameObject.SetActive (true);
			qualityValue.gameObject.SetActive (true);

			qualityValue.text = item.quality().getName();
			scale.x = qualityValue.text.Length + 1;
			qualityBG.localScale = scale;
			qualityValue.color = (data.quality == ItemQuality.ARTEFACT? artefactColor:
								  data.quality == ItemQuality.UNIQUE? uniqueColor: 
								  data.quality == ItemQuality.RARE? rareColor:
								  data.quality == ItemQuality.SUPERIOR? superiorColor:
								  goodColor);
			maxLenght = qualityRender.bounds.size.x;
		}

		namePre.gameObject.SetActive (true);
		nameBG.gameObject.SetActive (true);
		nameValue.gameObject.SetActive (true);

		nameValue.text = data.name;
		scale.x = nameValue.text.Length + 1;
		nameBG.localScale = scale;
		maxLenght = Mathf.Max(maxLenght, nameRender.bounds.size.x);

		switch (data.itemType) {
			case ItemType.GOODS:
				pre1.gameObject.SetActive (true);
				bg1.gameObject.SetActive (true);
				value1.gameObject.SetActive (true);

				maxLenght = Mathf.Max(maxLenght, setCost(1, data));
				break;

			case ItemType.HAND_WEAPON:
				pre1.gameObject.SetActive (true);
				bg1.gameObject.SetActive (true);
				value1.gameObject.SetActive (true);
				pre2.gameObject.SetActive (true);
				bg2.gameObject.SetActive (true);
				value2.gameObject.SetActive (true);

				HandWeaponData hwd = (HandWeaponData)data;
				value1.text = "Урон: <color=orange>" + hwd.minDamage + " - " + hwd.maxDamage + "</color>";
				scale.x = value1.text.Length - 24;// - (количество спецсимволов)
				bg1.localScale = scale;
				maxLenght = Mathf.Max(maxLenght, value1Render.bounds.size.x);

				maxLenght = Mathf.Max(maxLenght, setCost(2, data));
				break;

			case ItemType.BODY_ARMOR:
				pre1.gameObject.SetActive (true);
				bg1.gameObject.SetActive (true);
				value1.gameObject.SetActive (true);
				pre2.gameObject.SetActive (true);
				bg2.gameObject.SetActive (true);
				value2.gameObject.SetActive (true);

				BodyArmorData bad = (BodyArmorData)data;
				value1.text = "Защита: <color=orange>" + bad.armorClass + "</color>";
				scale.x = value1.text.Length - 22;
				bg1.localScale = scale;
				maxLenght = Mathf.Max(maxLenght, value1Render.bounds.size.x);

				maxLenght = Mathf.Max(maxLenght, setCost(2, data));
				break;

			case ItemType.WEAPON:
				pre1.gameObject.SetActive (true);
				bg1.gameObject.SetActive (true);
				value1.gameObject.SetActive (true);
				pre2.gameObject.SetActive (true);
				bg2.gameObject.SetActive (true);
				value2.gameObject.SetActive (true);
				pre3.gameObject.SetActive (true);
				bg3.gameObject.SetActive (true);
				value3.gameObject.SetActive (true);
				pre4.gameObject.SetActive (true);
				bg4.gameObject.SetActive (true);
				value4.gameObject.SetActive (true);
				pre5.gameObject.SetActive (true);
				bg5.gameObject.SetActive (true);
				value5.gameObject.SetActive (true);

				WeaponData wd = (WeaponData)data;
				value1.text = "Урон: <color=orange>" + wd.minDamage + " - " + wd.maxDamage + "</color>";
				scale.x = value1.text.Length - 24;
				bg1.localScale = scale;
				maxLenght = Mathf.Max(maxLenght, value1Render.bounds.size.x);

				value2.text = "Перезарядка: <color=orange>" + wd.reloadTime.ToString("0.00") + "</color>";
				scale.x = value2.text.Length - 22.5F;
				bg2.localScale = scale;
				maxLenght = Mathf.Max(maxLenght, value2Render.bounds.size.x);

				value3.text = "Питание: <color=cyan>" + data.energyNeeded + "</color>";
				scale.x = value3.text.Length - 20.5f;
				bg3.localScale = scale;
				maxLenght = Mathf.Max(maxLenght, value3Render.bounds.size.x);

				value4.text = "Объём: <color=orange>" + data.volume.ToString("0.0") + "</color>";
				scale.x = value4.text.Length - 22.5F;
				bg4.localScale = scale;
				maxLenght = Mathf.Max(maxLenght, value4Render.bounds.size.x);

				maxLenght = Mathf.Max(maxLenght, setCost(5, data));
				break;

			case ItemType.ENGINE:
				pre1.gameObject.SetActive (true);
				bg1.gameObject.SetActive (true);
				value1.gameObject.SetActive (true);
				pre2.gameObject.SetActive (true);
				bg2.gameObject.SetActive (true);
				value2.gameObject.SetActive (true);
				pre3.gameObject.SetActive (true);
				bg3.gameObject.SetActive (true);
				value3.gameObject.SetActive (true);
				pre4.gameObject.SetActive (true);
				bg4.gameObject.SetActive (true);
				value4.gameObject.SetActive (true);

				EngineData ed = (EngineData)data;
				value1.text = "Мощность: <color=orange>" + ((ed.power) * 1000).ToString("0") + "</color>";
				scale.x = value1.text.Length - 22.5f;
				bg1.localScale = scale;
				maxLenght = Mathf.Max(maxLenght, value1Render.bounds.size.x);

				value2.text = "Питание: <color=cyan>" + data.energyNeeded + "</color>";
				scale.x = value2.text.Length - 20.5f;
				bg2.localScale = scale;
				maxLenght = Mathf.Max(maxLenght, value2Render.bounds.size.x);

				value3.text = "Объём: <color=orange>" + data.volume.ToString("0.0") + "</color>";
				scale.x = value3.text.Length - 22.5F;
				bg3.localScale = scale;
				maxLenght = Mathf.Max(maxLenght, value3Render.bounds.size.x);

				maxLenght = Mathf.Max(maxLenght, setCost(4, data));
				break;

			case ItemType.ARMOR:
				pre1.gameObject.SetActive (true);
				bg1.gameObject.SetActive (true);
				value1.gameObject.SetActive (true);
				pre2.gameObject.SetActive (true);
				bg2.gameObject.SetActive (true);
				value2.gameObject.SetActive (true);
				pre3.gameObject.SetActive (true);
				bg3.gameObject.SetActive (true);
				value3.gameObject.SetActive (true);

				ArmorData ad = (ArmorData)data;
				value1.text = "Броня: <color=orange>" + ad.armorClass + "</color>";
				scale.x = value1.text.Length - 22.5f;
				bg1.localScale = scale;
				maxLenght = Mathf.Max(maxLenght, value1Render.bounds.size.x);

				value2.text = "Объём: <color=orange>" + data.volume.ToString("0.0") + "</color>";
				scale.x = value2.text.Length - 22.5F;
				bg2.localScale = scale;
				maxLenght = Mathf.Max(maxLenght, value2Render.bounds.size.x);

				maxLenght = Mathf.Max(maxLenght, setCost(3, data));
				break;

			case ItemType.GENERATOR:
				pre1.gameObject.SetActive (true);
				bg1.gameObject.SetActive (true);
				value1.gameObject.SetActive (true);
				pre2.gameObject.SetActive (true);
				bg2.gameObject.SetActive (true);
				value2.gameObject.SetActive (true);
				pre3.gameObject.SetActive (true);
				bg3.gameObject.SetActive (true);
				value3.gameObject.SetActive (true);

				GeneratorData gd = (GeneratorData)data;
				value1.text = "Мощность: <color=orange>" + gd.maxEnergy + "</color>";
				scale.x = value1.text.Length - 22.5f;
				bg1.localScale = scale;
				maxLenght = Mathf.Max(maxLenght, value1Render.bounds.size.x);

				value2.text = "Объём: <color=orange>" + data.volume.ToString("0.0") + "</color>";
				scale.x = value2.text.Length - 22.5F;
				bg2.localScale = scale;
				maxLenght = Mathf.Max(maxLenght, value2Render.bounds.size.x);

				maxLenght = Mathf.Max(maxLenght, setCost(3, data));
				break;

			case ItemType.RADAR:
				pre1.gameObject.SetActive (true);
				bg1.gameObject.SetActive (true);
				value1.gameObject.SetActive (true);
				pre2.gameObject.SetActive (true);
				bg2.gameObject.SetActive (true);
				value2.gameObject.SetActive (true);
				pre3.gameObject.SetActive (true);
				bg3.gameObject.SetActive (true);
				value3.gameObject.SetActive (true);
				pre4.gameObject.SetActive (true);
				bg4.gameObject.SetActive (true);
				value4.gameObject.SetActive (true);

				RadarData rd = (RadarData)data;
				value1.text = "Дальность: <color=orange>" + rd.range + "</color>";
				scale.x = value1.text.Length - 22.5f;
				bg1.localScale = scale;
				maxLenght = Mathf.Max(maxLenght, value1Render.bounds.size.x);

				value2.text = "Питание: <color=cyan>" + data.energyNeeded + "</color>";
				scale.x = value2.text.Length - 20.5f;
				bg2.localScale = scale;
				maxLenght = Mathf.Max(maxLenght, value2Render.bounds.size.x);

				value3.text = "Объём: <color=orange>" + data.volume.ToString("0.0") + "</color>";
				scale.x = value3.text.Length - 22.5F;
				bg3.localScale = scale;
				maxLenght = Mathf.Max(maxLenght, value3Render.bounds.size.x);

				maxLenght = Mathf.Max(maxLenght, setCost(4, data));
				break;

			case ItemType.SHIELD:
				pre1.gameObject.SetActive (true);
				bg1.gameObject.SetActive (true);
				value1.gameObject.SetActive (true);
				pre2.gameObject.SetActive (true);
				bg2.gameObject.SetActive (true);
				value2.gameObject.SetActive (true);
				pre3.gameObject.SetActive (true);
				bg3.gameObject.SetActive (true);
				value3.gameObject.SetActive (true);
				pre4.gameObject.SetActive (true);
				bg4.gameObject.SetActive (true);
				value4.gameObject.SetActive (true);
				pre5.gameObject.SetActive (true);
				bg5.gameObject.SetActive (true);
				value5.gameObject.SetActive (true);

				ShieldData sd = (ShieldData)data;
				value1.text = "Защита: <color=orange>" + sd.shieldLevel + "</color>";
				scale.x = value1.text.Length - 22.5f;
				bg1.localScale = scale;
				maxLenght = Mathf.Max(maxLenght, value1Render.bounds.size.x);

				value2.text = "Перезарядка: <color=orange>" + sd.rechargeSpeed + "</color>";
				scale.x = value2.text.Length - 22.5F;
				bg2.localScale = scale;
				maxLenght = Mathf.Max(maxLenght, value2Render.bounds.size.x);

				value3.text = "Питание: <color=cyan>" + data.energyNeeded + "</color>";
				scale.x = value3.text.Length - 20.5f;
				bg3.localScale = scale;
				maxLenght = Mathf.Max(maxLenght, value3Render.bounds.size.x);

				value4.text = "Объём: <color=orange>" + data.volume.ToString("0.0") + "</color>";
				scale.x = value4.text.Length - 22.5F;
				bg4.localScale = scale;
				maxLenght = Mathf.Max(maxLenght, value4Render.bounds.size.x);

				maxLenght = Mathf.Max(maxLenght, setCost(5, data));
				break;

			case ItemType.REPAIR_DROID:
				pre1.gameObject.SetActive (true);
				bg1.gameObject.SetActive (true);
				value1.gameObject.SetActive (true);
				pre2.gameObject.SetActive (true);
				bg2.gameObject.SetActive (true);
				value2.gameObject.SetActive (true);
				pre3.gameObject.SetActive (true);
				bg3.gameObject.SetActive (true);
				value3.gameObject.SetActive (true);
				pre4.gameObject.SetActive (true);
				bg4.gameObject.SetActive (true);
				value4.gameObject.SetActive (true);

				RepairDroidData rdd = (RepairDroidData)data;
				value1.text = "Ремонт: <color=orange>" + rdd.repairSpeed + "</color>";
				scale.x = value1.text.Length - 22.5f;
				bg1.localScale = scale;
				maxLenght = Mathf.Max(maxLenght, value1Render.bounds.size.x);

				value2.text = "Питание: <color=cyan>" + data.energyNeeded + "</color>";
				scale.x = value2.text.Length - 20.5f;
				bg2.localScale = scale;
				maxLenght = Mathf.Max(maxLenght, value2Render.bounds.size.x);

				value3.text = "Объём: <color=orange>" + data.volume.ToString("0.0") + "</color>";
				scale.x = value3.text.Length - 22.5F;
				bg3.localScale = scale;
				maxLenght = Mathf.Max(maxLenght, value3Render.bounds.size.x);

				maxLenght = Mathf.Max(maxLenght, setCost(4, data));
				break;

			case ItemType.HARVESTER:
				pre1.gameObject.SetActive (true);
				bg1.gameObject.SetActive (true);
				value1.gameObject.SetActive (true);
				pre2.gameObject.SetActive (true);
				bg2.gameObject.SetActive (true);
				value2.gameObject.SetActive (true);
				pre3.gameObject.SetActive (true);
				bg3.gameObject.SetActive (true);
				value3.gameObject.SetActive (true);

				HarvesterData hd = (HarvesterData)data;
				value1.text = "Поиск: <color=orange>" + hd.harvestTime + "</color>";
				scale.x = value1.text.Length - 22.5f;
				bg1.localScale = scale;
				maxLenght = Mathf.Max(maxLenght, value1Render.bounds.size.x);

				value2.text = "Объём: <color=orange>" + data.volume.ToString("0.0") + "</color>";
				scale.x = value2.text.Length - 22.5F;
				bg2.localScale = scale;
				maxLenght = Mathf.Max(maxLenght, value2Render.bounds.size.x);

				maxLenght = Mathf.Max(maxLenght, setCost(3, data));
				break;
		}

		maxX = screenWidth - maxLenght - .5f;
	}

	private void hide () {
		onScreen = false;
		item = null;
		trans.gameObject.SetActive (false);
	}

	public enum Type {
		NONE, INVENTORY, MARKET_BUY, MARKET_SELL
	}
}