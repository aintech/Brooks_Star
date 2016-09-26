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

	private ItemHolder holder, tempHolder;

	private Item item;

	private TextMesh qualityValue, nameValue, value1, value2, value3, value4, value5;

	private Vector2 pos = Vector2.zero;

	private Color32 goodColor = new Color32(176, 195, 217, 255),
					superiorColor = new Color32(94, 152, 217, 255),
					rareColor = new Color32(136, 71, 255, 255),
					uniqueColor = new Color32(173, 229, 92, 255),
					artefactColor = new Color32(235, 75, 75, 255);

	private Vector3 scale = Vector3.one;

	private Inventory targetInventory;

//	private Vector2[] positions = new Vector2[] { new Vector2(.5f, -.57f), new Vector2(.5f, -.95f), new Vector2(0, -1.33f), new Vector2(0, -1.71f), new Vector2(0, -2.09f) };

//	private Vector2[,] positions = new Vector2[,]{
//		{new Vector2(.15f, -.57f), new Vector2(.297f, -.57f), new Vector2(.295f, -.76f)},
//		{new Vector2(.15f, -.95f), new Vector2(.297f, -.95f), new Vector2(.295f, -1.14f)},
//		{new Vector2(.15f, -1.33f), new Vector2(.297f, -1.33f), new Vector2(.295f, -1.52f)},
//		{new Vector2(.15f, -1.71f), new Vector2(.297f, -1.71f), new Vector2(.295f, -1.9f)},
//		{new Vector2(.15f, -2.09f), new Vector2(.297f, -2.09f), new Vector2(.295f, -2.28f)},
//		{new Vector2(.15f, -2.47f), new Vector2(.297f, -2.47f), new Vector2(.295f, -2.66f)}
//	}; 

	private Type inventoryType;

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

		MeshRenderer mesh;
		for (int i = 0; i < trans.childCount; i++) {
			mesh = trans.GetChild (i).GetComponent<MeshRenderer> ();
			if (mesh != null) {
				mesh.sortingLayerName = "User Interface";
				mesh.sortingOrder = 9;
			}
		}

		hide ();

		return this;
	}

	public void setEnabled (Inventory inventory) {
		this.targetInventory = inventory;
		enabled = inventory != null;
	}

	public void setInventoryType (Type type) {
		this.inventoryType = type;
	}

	void Update () {
		if (onScreen) {
			if (Utils.hit == null) {
				hide ();
			} else if (Utils.hit != null) {
				tempHolder = Utils.hit.GetComponent<ItemHolder> ();
				if (tempHolder == null || tempHolder.item == null) {
					hide ();
				} else if (tempHolder != holder || tempHolder.item != item) {
					showDescription (tempHolder);
				}
			}
			pos = Utils.mousePos;
			//			if (pos.x > maxX) {
			//				pos.x = maxX;
			//			}
			//			if (pos.y < minY) {
			//				pos.y = minY;
			//			}
			trans.localPosition = pos;
		} else {
			if (Utils.hit != null) {
				holder = Utils.hit.GetComponent<ItemHolder>();
				if (holder != null && holder.item != null) {
					showDescription(holder);
				}
			}
		}
	}

	//	public void sellItemToTrader (Item item, Inventory buybackInventory) {
	//		Inventory source = item.cell.transform.parent.GetComponent<Inventory> ();
	//		if (source != null) {
	//			source.calculateFreeVolume();
	//		}
	//		buybackInventory.addItemToFirstFreePosition (item, true);
	//		Vars.cash += item.getCost ();
	//	}

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

	private void setCost(int index, int cost) {
		string text = (inventoryType == Type.MARKET_BUY? "Купить за": inventoryType == Type.MARKET_SELL? "Продать за": "Стоимость:")  + " <color=yellow>" + cost + "$</color>";
		scale.x = text.Length - 22.5f;
		switch (index) {
			case 1: value1.text = text; bg1.localScale = scale; break;
			case 2: value2.text = text; bg2.localScale = scale; break;
			case 3: value3.text = text; bg3.localScale = scale; break;
			case 4: value4.text = text; bg4.localScale = scale; break;
			case 5: value5.text = text; bg5.localScale = scale; break;
			default: Debug.Log("Unknown index: " + index); break;
		}
	}

	private void showTexts (ItemData data) {
		if (data.quality != ItemQuality.COMMON) {
			qualityPre.gameObject.SetActive (true);
			qualityBG.gameObject.SetActive (true);
			qualityValue.gameObject.SetActive (true);

			qualityValue.text = item.getItemQuality().getName();
			scale.x = qualityValue.text.Length + 1;
			qualityBG.localScale = scale;
			qualityValue.color = (data.quality == ItemQuality.ARTEFACT? artefactColor:
								  data.quality == ItemQuality.UNIQUE? uniqueColor: 
								  data.quality == ItemQuality.RARE? rareColor:
								  data.quality == ItemQuality.SUPERIOR? superiorColor:
								  goodColor);
		}

		namePre.gameObject.SetActive (true);
		nameBG.gameObject.SetActive (true);
		nameValue.gameObject.SetActive (true);

		nameValue.text = data.name.Replace('\n', ' ');
		scale.x = nameValue.text.Length + 1;
		nameBG.localScale = scale;

		switch (data.itemType) {
			case ItemType.HAND_WEAPON:
				pre1.gameObject.SetActive (true);
				bg1.gameObject.SetActive (true);
				value1.gameObject.SetActive (true);
				pre2.gameObject.SetActive (true);
				bg2.gameObject.SetActive (true);
				value2.gameObject.SetActive (true);

				HandWeaponData hwd = (HandWeaponData)data;
				value1.text = "Урон: <color=orange>" + hwd.minDamage + " - " + hwd.maxDamage + "</color>";
				scale.x = value1.text.Length - 24;// + 1 - (количество спецсимволов)
				bg1.localScale = scale;

				setCost(2, data.cost);
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
				scale.x = value1.text.Length - 22;// + 1 - (количество спецсимволов)
				bg1.localScale = scale;

				setCost(2, data.cost);
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

				value2.text = "Перезарядка: <color=orange>" + wd.reloadTime.ToString("0.00") + "</color>";
				scale.x = value2.text.Length - 22.5F;
				bg2.localScale = scale;

				value3.text = "Питание: <color=cyan>" + data.energyNeeded + "</color>";
				scale.x = value3.text.Length - 20.5f;
				bg3.localScale = scale;

				value4.text = "Объём: <color=orange>" + data.volume.ToString("0.0") + "</color>";
				scale.x = value4.text.Length - 22.5F;
				bg4.localScale = scale;

				setCost(5, data.cost);
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

				value2.text = "Питание: <color=cyan>" + data.energyNeeded + "</color>";
				scale.x = value2.text.Length - 20.5f;
				bg2.localScale = scale;

				value3.text = "Объём: <color=orange>" + data.volume.ToString("0.0") + "</color>";
				scale.x = value3.text.Length - 22.5F;
				bg3.localScale = scale;

				setCost(4, data.cost);
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

				value2.text = "Объём: <color=orange>" + data.volume.ToString("0.0") + "</color>";
				scale.x = value2.text.Length - 22.5F;
				bg2.localScale = scale;

				setCost(3, data.cost);
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

				value2.text = "Объём: <color=orange>" + data.volume.ToString("0.0") + "</color>";
				scale.x = value2.text.Length - 22.5F;
				bg2.localScale = scale;

				setCost(3, data.cost);
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

				value2.text = "Питание: <color=cyan>" + data.energyNeeded + "</color>";
				scale.x = value2.text.Length - 20.5f;
				bg2.localScale = scale;

				value3.text = "Объём: <color=orange>" + data.volume.ToString("0.0") + "</color>";
				scale.x = value3.text.Length - 22.5F;
				bg3.localScale = scale;

				setCost(4, data.cost);
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

				value2.text = "Перезарядка: <color=orange>" + sd.rechargeSpeed + "</color>";
				scale.x = value2.text.Length - 22.5F;
				bg2.localScale = scale;

				value3.text = "Питание: <color=cyan>" + data.energyNeeded + "</color>";
				scale.x = value3.text.Length - 20.5f;
				bg3.localScale = scale;

				value4.text = "Объём: <color=orange>" + data.volume.ToString("0.0") + "</color>";
				scale.x = value4.text.Length - 22.5F;
				bg4.localScale = scale;

				setCost(5, data.cost);
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

				value2.text = "Питание: <color=cyan>" + data.energyNeeded + "</color>";
				scale.x = value2.text.Length - 20.5f;
				bg2.localScale = scale;

				value3.text = "Объём: <color=orange>" + data.volume.ToString("0.0") + "</color>";
				scale.x = value3.text.Length - 22.5F;
				bg3.localScale = scale;

				setCost(4, data.cost);
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

				value2.text = "Объём: <color=orange>" + data.volume.ToString("0.0") + "</color>";
				scale.x = value2.text.Length - 22.5F;
				bg2.localScale = scale;

				setCost(3, data.cost);
				break;
		}
	}

	private void hide () {
		onScreen = false;
		trans.gameObject.SetActive (false);
	}

//    public void showDescription(Buff buff) {
//        this.buff = buff;
//        BuffType buffType = buff.getBuffType();
//        nameValue.text = buffType.getName();
//
//        stat_2.text = "";
//		stat_1.text = buffType.getEffectName().Replace("?", buff.getEffectValue().ToString());
//        
//        body.localPosition = bodyPosStat_1;
//        description.transform.localPosition = descriptPos_1;
//
//        quality.gameObject.SetActive(false);
//
//		description.text = buff.getDescription();
//		costText.text = buff.getCost().ToString();
//		stretchBody();
//		setActionMsgForBuff(buff);
//        onScreen = true;
//        Update();
//        gameObject.SetActive(true);
//    }

	public void hideActionMsg () {
//		actionMsgTrans.gameObject.SetActive(false);
//		errorMsgTrans.gameObject.SetActive(false);
//		costTrans.localPosition = costPos_1;
	}

	public enum Type {
		INVENTORY, MARKET_BUY, MARKET_SELL
	}

//    private void stretchBody () {
//		int descriptLines = item == null? buff.getDescriptionLinesCount(): item.getDescriptionLinesCount();
//        stretchScale.y = descriptLines + 1;
//        stretch.localScale = stretchScale;
//		footerPos.y = footerInitY - (footerStep * descriptLines);
//		footer.localPosition = footerPos;
//		minY = containerOffset - body.localPosition.y - footer.localPosition.y + FOOTER_SIZE - SCREEN_HALF_HEIGHT;
//	}

//	private void setActionMsg (ItemHolder holder) {
//		if (shopDescriptor && holder == null) {//Подразумевает, что мы находимся в магазине, и смотрим на полки (у них нет холдера)
//			if (item.getCost() <= Vars.gold) {
//                adjustFooter("Купить", false);
//            } else {
//                adjustFooter("Не хватает монет", true);
//            }
//		} else if (shopDescriptor) {
//            adjustFooter("Продать", false);
//		} else if (workbenchDescriptor) {
//			hideActionMsg();
//		} else {
//			switch (holder.getHolderType()) {
//				case ItemHolderType.INVENTORY:
//				case ItemHolderType.POTION_BAG:
//					if (holder.getItem().getItemType() == ItemType.POTION) {
//						tempPotion = (PotionItem)holder.getItem();
//                        if ((tempPotion.getPotionType() == PotionType.HEALTH && Hero.getHealth() == Hero.getMaxHealth())) {
//                            adjustFooter("Здоровье макс.", true);
//                        } else if (Hero.isPotionAlreadyDrinked(tempPotion.getPotionType())) {
//                            adjustFooter("Нельзя выпить", true);
//                        } else {
//							adjustFooter("Выпить", false);
//						}
//					} else if (holder.getItem().getItemType() == ItemType.MATERIAL) {
//						hideActionMsg();
//					} else if (holder.getItem().getItemType() == ItemType.WEAPON) {
//                    	adjustFooter("Взять", false);
//					} else {
//                        adjustFooter("Надеть", false);
//					}
//					break;
//				case ItemHolderType.EQUIPMENT: adjustFooter("Снять", false); break;
//			}
//		}
//	}

//	private void setActionMsgForBuff (Buff buff) {
//		if (buff.isUsed()) {
//			errorMsgTrans.gameObject.SetActive(false);
//			actionMsgTrans.gameObject.SetActive(false);
//			costTrans.localPosition = costPos_1;
//		} else {
//			if (!buff.isHavingTargetItem()) {
//				errorMsg.text = (buff.getBuffItemType() == BuffItemType.SHIELD)? "Надень щит":
//								(buff.getBuffItemType() == BuffItemType.ARMOR)? "Надень броню":
//								(buff.getBuffItemType() == BuffItemType.WEAPON)? "Возьми оружие": "Я ХЗ что происходит(";
//				costTrans.localPosition = costPos_2;
//				errorMsgTrans.gameObject.SetActive(true);
//				actionMsgTrans.gameObject.SetActive(false);
//				minY += .5f;
//			} else if (Vars.gold >= buff.getCost()) {
//				actionMsg.text = "Чары";
//				costTrans.localPosition = costPos_1;
//				errorMsgTrans.gameObject.SetActive(false);
//				actionMsgTrans.gameObject.SetActive(true);
//			} else {
//				errorMsg.text = "Не хватает монет";
//				costTrans.localPosition = costPos_2;
//				actionMsgTrans.gameObject.SetActive(false);
//				errorMsgTrans.gameObject.SetActive(true);
//				minY += .5f;
//			}
//		}
//	}

//    private void adjustFooter (string message, bool isError) {
//        if (isError) {
//            errorMsgTrans.gameObject.SetActive(true);
//            errorMsg.text = message;
//            actionMsgTrans.gameObject.SetActive(false);
//            minY += .5f;
//        } else {
//            actionMsgTrans.gameObject.SetActive(true);
//            actionMsg.text = message;
//            errorMsgTrans.gameObject.SetActive(false);
//        }
//        costTrans.localPosition = isError ? costPos_2 : costPos_1;
//    }
//
//	public void hideDescription () {
//        item = null;
//        buff = null;
//		gameObject.SetActive(false);
//		onScreen = false;
//	}

//    public void setShopDescriptor (bool shopDescriptor) {
//        this.shopDescriptor = shopDescriptor;
//    }
}