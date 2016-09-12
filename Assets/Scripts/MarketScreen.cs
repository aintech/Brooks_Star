using UnityEngine;
using System.Collections;

public class MarketScreen : MonoBehaviour, ButtonHolder {

	private Planet planet;

	private HullsMarket hullsMarket;

	private EquipmentsMarket equipmentsMarket;

	private Button equipmentsBtn, hullsBtn, closeBtn;

	private TextMesh cashTxt;

	public void init (Planet planet, ShipData shipData, Inventory inventory, Inventory storage, Inventory marketInv, Inventory shipInv, Inventory buybackInv) {
		this.planet = planet;

		equipmentsMarket = transform.Find ("Equipments Market").GetComponent<EquipmentsMarket>();
		equipmentsMarket.init(this, inventory, storage, marketInv, shipInv, buybackInv, shipData);

		hullsMarket = transform.Find ("Hulls Market").GetComponent<HullsMarket> ();
		hullsMarket.init(inventory, storage, shipData);
		hullsMarket.fillWithRandomHulls(15, "Hull Item");

		equipmentsBtn = transform.Find ("Equipments Button").GetComponent<Button> ().init();
		hullsBtn = transform.Find("Hulls Button").GetComponent<Button>().init();
		closeBtn = transform.Find ("Close Button").GetComponent<Button> ().init();

		cashTxt = transform.Find ("Cash Text").GetComponent<TextMesh> ();
		cashTxt.gameObject.GetComponent<MeshRenderer> ().sortingOrder = 1;
		cashTxt.text = Vars.cash.ToString("C0");

		transform.Find("Market BG").gameObject.SetActive(true);

		gameObject.SetActive(false);
	}

	public void showScreen () {
		showEquipmentMarket();
		gameObject.SetActive(true);
	}

	public void closeScreen () {
		hullsMarket.closeScreen();
		equipmentsMarket.closeScreen();
		gameObject.SetActive(false);
		planet.setPlanetBtnsEnabled(true);
	}

	public void fireClickButton (Button button) {
		if (button == closeBtn) {
			closeScreen();
		} else if (button == equipmentsBtn) {
			showEquipmentMarket();
		} else if (button == hullsBtn) {
			showHullsMarket();
		} else {
			Debug.Log("Unknown button: " + button.name);
		}
	}

	private void closeMarket () {
		if (equipmentsMarket.gameObject.activeInHierarchy) { equipmentsMarket.closeScreen(); }
		if (hullsMarket.gameObject.activeInHierarchy) { equipmentsMarket.gameObject.SetActive(false); }
	}

	private void showEquipmentMarket () {
		hullsMarket.closeScreen();
		equipmentsMarket.showScreen();
	}

	private void showHullsMarket () {
		equipmentsMarket.closeScreen();
		hullsMarket.showScreen();
	}
}