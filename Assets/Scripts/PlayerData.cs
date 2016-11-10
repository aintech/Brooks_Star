using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class PlayerData : MonoBehaviour {

	public Sprite nude, underwear, armor;

	private SpriteRenderer render;

	private EquipmentSlot handWeaponSlot, bodyArmorSlot;

	private Transform trans;

	public List<Slot> allSlots { get; private set; }

	public List<EquipmentSlot> equipmentSlots { get; private set; }

	public List<SupplySlot> supplySlots { get; private set; }

	private TextMesh healthValue, armorValue, damageValue;

	private bool onPlanetSurface;

	public PlayerData init () {
		trans = transform;

		allSlots = new List<Slot>();
		equipmentSlots = new List<EquipmentSlot>();
		supplySlots = new List<SupplySlot>();

		Slot slot;
		EquipmentSlot eSlot;
		for (int i = 0; i < trans.childCount; i++) {
			slot = trans.GetChild(i).GetComponent<Slot>();
			if (slot != null) {
				slot.init();
				if (slot.kind == ItemKind.EQUIPMENT) {
					eSlot = (EquipmentSlot)slot;
					switch (eSlot.slotType) {
						case EquipmentSlot.Type.HAND_WEAPON: handWeaponSlot = eSlot; break;
						case EquipmentSlot.Type.BODY_ARMOR: bodyArmorSlot = eSlot; break;
						default: Debug.Log("Unknown slot type: " + eSlot.slotType); break;
					}
					equipmentSlots.Add(eSlot);
				} else if (slot.kind == ItemKind.SUPPLY) {
					supplySlots.Add((SupplySlot)slot);
				}
				allSlots.Add(slot);
			}
		}

		Transform playerInfo = transform.Find ("Player Information");
		playerInfo.gameObject.SetActive(true);

		damageValue = playerInfo.Find ("Damage Value").GetComponent<TextMesh> ();
		healthValue = playerInfo.Find ("Health Value").GetComponent<TextMesh> ();
		armorValue = playerInfo.Find ("Armor Value").GetComponent<TextMesh> ();

		damageValue.GetComponent<MeshRenderer> ().sortingLayerName = "Inventory";
		healthValue.GetComponent<MeshRenderer> ().sortingLayerName = "Inventory";
		armorValue.GetComponent<MeshRenderer> ().sortingLayerName = "Inventory";

		damageValue.GetComponent<MeshRenderer> ().sortingOrder = 3;
		healthValue.GetComponent<MeshRenderer> ().sortingOrder = 3;
		armorValue.GetComponent<MeshRenderer> ().sortingOrder = 3;

		render =  transform.Find("Player Image").GetComponent<SpriteRenderer>();
		render.gameObject.SetActive(true);

		updatePlayerInfo();

		gameObject.SetActive(false);

		return this;
	}

	public void updatePlayerInfo () {
		updateDamageValue();
		updateHealthValue ();
		updateArmorValue ();
	}

	public void updateDamageValue () {
		damageValue.text = Player.minDamage.ToString() + " - " + Player.maxDamage.ToString();
	}

	public void updateHealthValue () {
		healthValue.text = Player.health.ToString() + (Player.health < Player.maxHealth? "/" + Player.maxHealth.ToString(): "");
	}

	public void updateArmorValue () {
		updatePlayerImage();
		armorValue.text = Player.armor == null? "0": Player.armor.armorClass.ToString();
	}

	public void sendToVars () {
		Vars.equipmentMap.Clear();
		Vars.supplyMap.Clear();
		foreach (EquipmentSlot slot in equipmentSlots) {
			if (slot.item != null) {
				Vars.equipmentMap.Add(slot.slotType, slot.item.itemData);
			}
		}
		foreach(SupplySlot slot in supplySlots) {
			if (slot.item != null) {
				Vars.supplyMap.Add(slot.index, slot.item.itemData);
			}
		}
	}

	public void initFromVars () {
		foreach (KeyValuePair<Slot.Type, ItemData> pair in Vars.equipmentMap) {
			Item item = Instantiate<Transform>(ItemFactory.itemPrefab).GetComponent<Item>().init(pair.Value);
			item.GetComponent<SpriteRenderer>().sortingOrder = 3;
			getEquipmentSlot(pair.Key).setItem(item);
		}
		foreach (KeyValuePair<int, ItemData> pair in Vars.supplyMap) {
			Item item = Instantiate<Transform>(ItemFactory.itemPrefab).GetComponent<Item>().init(pair.Value);
			item.GetComponent<SpriteRenderer>().sortingOrder = 3;
			getSupplySlot(pair.Key).setItem(item);
		}
		Vars.equipmentMap.Clear();
		Vars.supplyMap.Clear();
	}

	private EquipmentSlot getEquipmentSlot (Slot.Type type) {
		foreach (EquipmentSlot slot in equipmentSlots) {
			if (slot.slotType == type) { return slot; }
		}
		Debug.Log("Unknown slot type: " + type);
		return null;
	}

	public SupplySlot getSupplySlot (int index) {
		foreach (SupplySlot slot in supplySlots) {
			if (slot.index == index) { return slot; }
		}
		Debug.Log("Unknown slot index: " + index);
		return null;
	}

	public void updatePlayerImage () {
		render.sprite = (bodyArmorSlot.item == null? (Vars.EROTIC? nude: underwear): armor);
	}
}