using UnityEngine;
using System.Collections;

public class PlayerData : MonoBehaviour {

	public Transform itemPrefab;

	public Sprite[] playerSprites;

	private SpriteRenderer render;

	private EquipmentSlot handWeaponSlot, bodyArmorSlot;

	private Transform trans;

	private EquipmentSlot[] slots;

	private TextMesh healthValue, armorValue, damageValue;

	private bool onPlanetSurface;

	public PlayerData init () {
		trans = transform;

		EquipmentSlot slot;
		for (int i = 0; i < trans.childCount; i++) {
			slot = trans.GetChild(i).GetComponent<EquipmentSlot>();
			if (slot != null) {
				slot.init();
				switch (slot.slotType) {
					case EquipmentSlot.Type.HAND_WEAPON: handWeaponSlot = slot; break;
					case EquipmentSlot.Type.BODY_ARMOR: bodyArmorSlot = slot; break;
					default: Debug.Log("Unknown slot type: " + slot.slotType); break;
				}
			}
		}

		slots = new EquipmentSlot[]{handWeaponSlot, bodyArmorSlot};

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

	public Slot[] getSlots () {
		return slots;
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
		armorValue.text = Player.armor == null? "0": Player.armor.armorClass.ToString();
	}
}