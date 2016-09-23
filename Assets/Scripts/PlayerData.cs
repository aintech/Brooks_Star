using UnityEngine;
using System.Collections;

public class PlayerData : MonoBehaviour {

	public Transform itemPrefab;

	public Sprite[] playerSprites;

	private SpriteRenderer render;

	private EquipmentSlot handWeaponSlot, bodyArmorSlot;

	private Transform trans;

	private EquipmentSlot[] slots;

//	private int currentHealth, currentShield;

	private TextMesh healthValue, armorValue, damageValue;

	private Color32 okColor, badColor = new Color32(255, 0, 0, 255);

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

		TextMesh damageLabel = playerInfo.Find ("Damage Label").GetComponent<TextMesh> ();
		TextMesh healthLabel = playerInfo.Find ("Health Label").GetComponent<TextMesh> ();
		TextMesh armorLabel = playerInfo.Find ("Armor Label").GetComponent<TextMesh> ();
		damageValue = playerInfo.Find ("Damage Value").GetComponent<TextMesh> ();
		healthValue = playerInfo.Find ("Health Value").GetComponent<TextMesh> ();
		armorValue = playerInfo.Find ("Armor Value").GetComponent<TextMesh> ();

		damageLabel.GetComponent<MeshRenderer> ().sortingLayerName = "Inventory";
		healthLabel.GetComponent<MeshRenderer> ().sortingLayerName = "Inventory";
		armorLabel.GetComponent<MeshRenderer> ().sortingLayerName = "Inventory";
		damageValue.GetComponent<MeshRenderer> ().sortingLayerName = "Inventory";
		healthValue.GetComponent<MeshRenderer> ().sortingLayerName = "Inventory";
		armorValue.GetComponent<MeshRenderer> ().sortingLayerName = "Inventory";

		damageLabel.GetComponent<MeshRenderer> ().sortingOrder = 3;
		healthLabel.GetComponent<MeshRenderer> ().sortingOrder = 3;
		armorLabel.GetComponent<MeshRenderer> ().sortingOrder = 3;
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
		healthValue.text = Player.health.ToString() + "/" + Player.maxHealth.ToString();
	}

	public void updateArmorValue () {
		armorValue.text = Player.armor == null? "0": Player.armor.armorClass.ToString();
	}
}