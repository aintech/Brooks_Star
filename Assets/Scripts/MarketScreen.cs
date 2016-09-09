using UnityEngine;
using System.Collections;

public class MarketScreen : MonoBehaviour, ButtonHolder {
	
	private HullsMarket hullsMarket;

	private EquipmentsMarket equipmentsMarket;

	private EquipmentsMarket goodsMarket, gearsMarket;

	private Planet planet;

	private Button goodsBtn, gearsBtn, equipmentsBtn, hullsBtn, closeBtn;

	private TextMesh cashTxt;

	public void init (Planet planet, ShipData shipData, Inventory inventory, Inventory storage, Inventory marketInv, Inventory shipInv, Inventory buybackInv) {
		this.planet = planet;

		equipmentsMarket = transform.Find ("EquipmentsMarket").GetComponent<EquipmentsMarket>();
		equipmentsMarket.init(this, inventory, storage, marketInv, shipInv, buybackInv, shipData);

		hullsMarket = transform.Find ("HullsMarket").GetComponent<HullsMarket> ();
		hullsMarket.init(inventory, storage, shipData);
		hullsMarket.fillWithRandomHulls(15, "Hull Item");

		goodsBtn = transform.Find ("GoodsBtn").GetComponent<Button> ().init();
		gearsBtn = transform.Find ("GearsBtn").GetComponent<Button> ().init();
		equipmentsBtn = transform.Find ("EquipmentsBtn").GetComponent<Button> ().init();
		hullsBtn = transform.Find("HullsBtn").GetComponent<Button>().init();
		closeBtn = transform.Find ("CloseBtn").GetComponent<Button> ().init();

		cashTxt = transform.Find ("CashTxt").GetComponent<TextMesh> ();
		cashTxt.gameObject.GetComponent<MeshRenderer> ().sortingOrder = 1;
		cashTxt.text = Variables.cash.ToString("C0");

		gameObject.SetActive(false);
	}

	public void showScreen () {
		gameObject.SetActive (true);
	}

	public void fireClickButton (Button button) {
		if (button == closeBtn) {
			if (goodsMarket.gameObject.activeInHierarchy || 
				gearsMarket.gameObject.activeInHierarchy || 
				equipmentsMarket.gameObject.activeInHierarchy || 
				hullsMarket.gameObject.activeInHierarchy)
			{
				closeMarket();
			} else { closeScreen(); }
		} else if (button == goodsBtn) {
			showMarket(MarketType.GOODS);
		} else if (button == gearsBtn) {
			showMarket(MarketType.GEARS);
		} else if (button == equipmentsBtn) {
			showMarket(MarketType.EQUIPMENTS);
		} else if (button == hullsBtn) {
			showMarket(MarketType.HULLS);
		} else {
			Debug.Log("Unknown button");
		}
	}

//	void Update () {
//		if (Input.GetMouseButtonDown (0) && Utils.hit != null) {
//			if (Utils.hit == closeBtn) {
//				closeScreen();
//			} else if (Utils.hit == equipmentHullsBtn) {
//				changeScreens();
//			}
////			mouseToWorldPosition = Camera.main.ScreenToWorldPoint (Input.mousePosition);
////			hit = Physics2D.Raycast(mouseToWorldPosition, Vector2.zero, 1);
////			if (hit.collider != null) {
////				switch (hit.collider.name) {
////					case "Close Btn": closeScreen(); break;
////					case "Equipment Hulls Btn": changeScreens (); break;
////				}
////			}
//		}
//	}

	private void closeMarket () {
		if (goodsMarket.gameObject.activeInHierarchy) { goodsMarket.gameObject.SetActive(false); }
		if (gearsMarket.gameObject.activeInHierarchy) { gearsMarket.gameObject.SetActive(false); }
		if (equipmentsMarket.gameObject.activeInHierarchy) { equipmentsMarket.closeScreen(); }
		if (hullsMarket.gameObject.activeInHierarchy) { equipmentsMarket.gameObject.SetActive(false); }
		setBtnsEnabled(true);
	}

	private void showMarket (MarketType type) {
		switch (type) {
			case MarketType.GOODS: goodsMarket.showScreen(); break;
			case MarketType.GEARS: gearsMarket.showScreen(); break;
			case MarketType.EQUIPMENTS: equipmentsMarket.showScreen(); break;
			case MarketType.HULLS: hullsMarket.showScreen(); break;
			default: Debug.Log("Unknown market type"); break;
		}
		setBtnsEnabled(false);
	}

	private void setBtnsEnabled (bool enabled) {
		goodsBtn.gameObject.SetActive(enabled);
		gearsBtn.gameObject.SetActive(enabled);
		equipmentsBtn.gameObject.SetActive(enabled);
		hullsBtn.gameObject.SetActive(enabled);
	}

	private void closeScreen () {
		gameObject.SetActive (false);
		planet.setPlanetBtnsEnabled(true);
	}

	private enum MarketType {
		GOODS, GEARS, EQUIPMENTS, HULLS
	}
}