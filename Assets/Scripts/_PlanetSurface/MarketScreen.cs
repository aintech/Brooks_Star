using UnityEngine;
using System.Collections;

public class MarketScreen : MonoBehaviour, ButtonHolder, Hideable {
	
	private PlanetSurface planetSurface;

	private HullsMarket hullsMarket;

	private EquipmentsMarket equipmentsMarket;

	private Button equipmentsBtn, hullsBtn, closeBtn;

	public void init (PlanetSurface planetSurface, ShipData shipData, Inventory inventory, ItemDescriptor itemDescriptor) {
		this.planetSurface = planetSurface;

		equipmentsMarket = transform.Find ("Equipments Market").GetComponent<EquipmentsMarket>();
		equipmentsMarket.init(this, inventory, itemDescriptor);

		hullsMarket = transform.Find ("Hulls Market").GetComponent<HullsMarket> ();
		hullsMarket.init(inventory, shipData);
		hullsMarket.fillWithRandomHulls(15, "Hull Item");

		equipmentsBtn = transform.Find ("Equipments Button").GetComponent<Button> ().init();
		hullsBtn = transform.Find("Hulls Button").GetComponent<Button>().init();
		closeBtn = transform.Find ("Close Button").GetComponent<Button> ().init();

		gameObject.SetActive(false);
	}

	public void showScreen () {
		PlanetSurface.topHideable = this;
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

	private void showEquipmentMarket () {
		equipmentsMarket.showScreen();
		setVisible(false);
	}

	private void showHullsMarket () {
		hullsMarket.showScreen();
		setVisible(false);
	}

	public void setVisible (bool visible) {
		hullsBtn.gameObject.SetActive(visible);
		equipmentsBtn.gameObject.SetActive(visible);
		closeBtn.gameObject.SetActive(visible);
//		if (!visible && equipmentsMarket.gameObject.activeInHierarchy) {
//			market.gameObject.SetActive(false);
//			buyback.gameObject.SetActive(false);
//		}

//		gameObject.SetActive(visible);

//		if (visible) {
//			if (equipmentsMarket.gameObject.activeInHierarchy) { showEquipmentMarket(); }
//			else { showHullsMarket(); }
//		}
	}

	public Inventory getMarket () {
		return equipmentsMarket.getMarket();
	}
}