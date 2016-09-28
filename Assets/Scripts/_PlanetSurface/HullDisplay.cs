using UnityEngine;
using System.Collections;

public class HullDisplay : MonoBehaviour, ButtonHolder {

	private SpriteRenderer hullImage;

	private TextMesh hullName, hullHealth, weaponCount, armorCount, shieldCount, generatorCount, repairDroidCount, harvesterCount;

	private Button buyBtn;

	public HullType hullType { get; private set; }

	private HullsMarket market;

	private ShipData shipData;

	public int cost { get; private set; }

	public HullDisplay init (HullsMarket market, ShipData shipData) {
		this.market = market;
		this.shipData = shipData;

		hullImage = transform.Find("Hull Image").GetComponent<SpriteRenderer>();
		hullName = transform.Find("Hull Name").GetComponent<TextMesh>();
		hullHealth = transform.Find("Hull Health").GetComponent<TextMesh>();
		weaponCount = transform.Find("Weapon Count").GetComponent<TextMesh>();
		armorCount = transform.Find("Armor Count").GetComponent<TextMesh>();
		shieldCount = transform.Find("Shield Count").GetComponent<TextMesh>();
		generatorCount = transform.Find("Generator Count").GetComponent<TextMesh>();
		repairDroidCount = transform.Find("Repair Droid Count").GetComponent<TextMesh>();
		harvesterCount = transform.Find("Harvester Count").GetComponent<TextMesh>();

		buyBtn = transform.Find("Buy Button").GetComponent<Button>().init();

		MeshRenderer mesh;
		for (int i = 0; i < transform.childCount; i++) {
			mesh = transform.GetChild(i).GetComponent<MeshRenderer>();
			if (mesh != null) { mesh.sortingOrder = 4; }
		}

		return this;
	}

	public void setHull (HullType hullType, Sprite image) {
		this.hullType = hullType;
		hullImage.sprite = image;
		hullName.text = hullType.getName();
		hullHealth.text = "HP: " + hullType.getMaxHealth();
		weaponCount.text = hullType.getWeaponSlots().ToString();
		armorCount.text = hullType.getArmorSlots().ToString();
		shieldCount.text = hullType.getShieldSlots().ToString();
		generatorCount.text = hullType.getGeneratorSlots().ToString();
		repairDroidCount.text = hullType.getRepairDroidSlots().ToString();
		harvesterCount.text = hullType.getHarvesterSlots().ToString();
		updateCost();
	}

	public void fireClickButton (Button btn) {
		if (btn == buyBtn) { market.buyHull(this); }
	}

	public void updateCost () {
		cost = -hullType.getCost() + (shipData.hullType.getCost() - shipData.repairCost);
		buyBtn.setText((cost > 0? "+": "") + cost + "$");
	}
}