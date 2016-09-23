using UnityEngine;
using System.Collections;

public class ItemDescriptor : MonoBehaviour {

	private Transform trans, qualityBG, nameBG;

//	private Transform trans, quality, body, stretch, footer, actionMsgTrans, errorMsgTrans, costTrans;

	private const float SCREEN_HALF_HEIGHT = 5, FOOTER_SIZE = .5f;

	private float containerOffset;

	public bool onScreen { get; private set; }

	public Describable descriable { get; private set; }

//    private Buff buff;

	private TextMesh qualityValue, nameValue;//, stat_1, stat_2, description, costText, actionMsg, errorMsg;

	private float minY, maxX = 4.8f, footerInitY = -1.1f, footerStep = .3f;

	private Vector2 pos = Vector2.zero, footerPos = Vector2.zero, 
					bodyPosStat_1 = Vector2.zero, bodyPosStat_2 = new Vector2(0, -.3f),
					descriptPos_1 = new Vector2(1.7f, -1.35f), descriptPos_2 = new Vector2(1.7f, -1.65f);

    private Vector2 costPos_1 = new Vector2(.88f, -.26f), costPos_2 = new Vector2(.88f, -.73f);

	private Vector3 stretchScale = Vector3.one;

//	private PotionItem tempPotion;

	private bool shopDescriptor;

	public ItemDescriptor init () {
		trans = transform;
		qualityBG = transform.Find("Quality Background");
		nameBG = transform.Find("Name Background");

		qualityValue = transform.Find("Quality Value").GetComponent<TextMesh>();
		nameValue = transform.Find("Name Value").GetComponent<TextMesh>();

//		Transform container = trans.Find("Container");
//		containerOffset = -container.localPosition.y;
//        container.gameObject.SetActive(true);
//		quality = container.Find("Quality");
//		body = container.Find("Body");
//		stretch = body.Find("Stretch");
//		footer = body.Find("Footer");
//		footerPos = footer.localPosition;
//		actionMsgTrans = footer.Find("ActionMsg");
//        costTrans = footer.Find("CostValue");
//        errorMsgTrans = footer.Find("ErrorMsg");

//		stat_1 = body.Find("Stat_1").GetComponent<TextMesh>();
//		stat_2 = body.Find("Stat_2").GetComponent<TextMesh>();
//		description = trans.Find("Description").GetComponent<TextMesh>();
//		actionMsg = actionMsgTrans.Find("ActionText").GetComponent<TextMesh>();
//        costText = costTrans.Find("CostText").GetComponent<TextMesh>();
//        errorMsg = errorMsgTrans.Find("ErrorText").GetComponent<TextMesh>();

        int layer = 9;
		string layerName = "User Interface";
		MeshRenderer mesh = qualityValue.GetComponent<MeshRenderer>();
		mesh.sortingLayerName = "User Interface";
		mesh.sortingOrder = layer;
		mesh = nameValue.GetComponent<MeshRenderer>();
		mesh.sortingLayerName = "User Interface";
		mesh.sortingOrder = layer;
//		mesh = stat_1.GetComponent<MeshRenderer>();
//		mesh.sortingLayerName = "InventoryLayer";
//		mesh.sortingOrder = layer;
//		mesh = stat_2.GetComponent<MeshRenderer>();
//		mesh.sortingLayerName = "InventoryLayer";
//		mesh.sortingOrder = layer;
//		mesh = description.GetComponent<MeshRenderer>();
//		mesh.sortingLayerName = "InventoryLayer";
//		mesh.sortingOrder = layer;
//        mesh = actionMsg.GetComponent<MeshRenderer>();
//        mesh.sortingLayerName = "InventoryLayer";
//        mesh.sortingOrder = layer;
//        mesh = costText.GetComponent<MeshRenderer>();
//		mesh.sortingLayerName = "InventoryLayer";
//		mesh.sortingOrder = layer;
//        mesh = errorMsg.GetComponent<MeshRenderer>();
//        mesh.sortingLayerName = "InventoryLayer";
//        mesh.sortingOrder = layer;
//
//		description.gameObject.SetActive(true);
		qualityBG.gameObject.SetActive(true);
		qualityValue.gameObject.SetActive(true);
		nameBG.gameObject.SetActive(true);
		nameValue.gameObject.SetActive(true);

        gameObject.SetActive(false);

		return this;
	}

	void Update () {
		if (onScreen) {
			if (Utils.hit == null) {
				onScreen = false;
				gameObject.SetActive(false);
			} else if (Utils.hit != null) {
//				HERE проверка, Utils.hit - наш describable или нет
			}
		} else {
			if (Utils.hit != null) {
				descriable = Utils.hit.GetComponent<Describable>();
				if (descriable != null) {
					showDescription();
				}
			}
		}
//		if (!onScreen && Utils.hit != null) {
//			if (Utils.hit.name.Equals("Cell")) {
//				
//			}
//		}
		if (onScreen) {
			pos = Utils.mousePos;
			if (pos.x > maxX) {
				pos.x = maxX;
			}
			if (pos.y < minY) {
				pos.y = minY;
			}
			trans.localPosition = pos;
		}
	}

	public void showDescription () {
		nameValue.text = descriable.getName();

//		stat_2.text = "";
//		if (item is ArmorModifier) {
//			stat_1.text = "Защита=> " + ((ArmorModifier)item).getArmor().ToString();
//		} else if (item is WeaponItem) {
//			stat_1.text = ((WeaponItem)item).getDamage().ToString() + " " + ((WeaponItem)item).getWeaponType().getSpeedType().getSpeedName();
////			stat_2.text = "Урон=> " + ((WeaponItem)item).getDamage().ToString();
////			stat_1.text = "Скорость=> " + ((WeaponItem)item).getWeaponType().getAttackSpeed().ToString();
//		} else if (item is PotionItem) {
//			stat_1.text = ((PotionItem)item).getPotionType().getEffectDescript();
//		} else if (item is MaterialItem) {
//			stat_1.text = "";
//		}
//
//		if (stat_2.text.Equals("")) {
//			body.localPosition = bodyPosStat_1;
//			description.transform.localPosition = descriptPos_1;
//		} else {
//			body.localPosition = bodyPosStat_2;
//			description.transform.localPosition = descriptPos_2;
//		}
//
//		if (item.getItemQuality() != ItemQuality.COMMON) {
//			qualityValue.text = item.getItemQuality().getDescription();
//			qualityValue.color = item.getItemQuality().getColor();
//			quality.gameObject.SetActive(true);
//		} else {
//			quality.gameObject.SetActive(false);
//		}
//
//		description.text = item.getDescription();
//		costText.text = item.getCost().ToString();
//		stretchBody();
//		setActionMsg(item.getHolder());
		onScreen = true;
		Update();
		gameObject.SetActive(true);
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