using UnityEngine;
using System.Collections;

public class MarketScreen : MonoBehaviour, ButtonHolder, Hideable {

	private PlanetSurface planetSurface;

	private HullsMarket hullsMarket;

	private EquipmentsMarket equipmentsMarket;

	private Button equipmentsBtn, hullsBtn, closeBtn;

	public void init (PlanetSurface planetSurface, ShipData shipData, Inventory inventory, Inventory storage, Inventory marketInv, Inventory shipInv, Inventory buybackInv) {
		this.planetSurface = planetSurface;

		equipmentsMarket = transform.Find ("Equipments Market").GetComponent<EquipmentsMarket>();
		equipmentsMarket.init(this, inventory, storage, marketInv, shipInv, buybackInv, shipData);

		hullsMarket = transform.Find ("Hulls Market").GetComponent<HullsMarket> ();
		hullsMarket.init(inventory, storage, shipData);
		hullsMarket.fillWithRandomHulls(15, "Hull Item");

		equipmentsBtn = transform.Find ("Equipments Button").GetComponent<Button> ().init();
		hullsBtn = transform.Find("Hulls Button").GetComponent<Button>().init();
		closeBtn = transform.Find ("Close Button").GetComponent<Button> ().init();

		transform.Find("Market BG").gameObject.SetActive(true);

		gameObject.SetActive(false);
	}

	public void showScreen () {
		PlanetSurface.topHideable = this;
		showEquipmentMarket();
		gameObject.SetActive(true);
	}

	public void closeScreen () {
		hullsMarket.closeScreen();
		equipmentsMarket.closeScreen();
		gameObject.SetActive(false);
		planetSurface.setVisible(true);
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
		hullsBtn.setActive(true);
		equipmentsBtn.setActive(false);
	}

	private void showHullsMarket () {
		equipmentsMarket.closeScreen();
		hullsMarket.showScreen();
		hullsBtn.setActive(false);
		equipmentsBtn.setActive(true);
	}

	public void setVisible (bool visible) {
		gameObject.SetActive(visible);
	}
}