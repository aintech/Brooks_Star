using UnityEngine;
using System.Collections;

public class MarketScreen : MonoBehaviour {

	public Sprite equipmentActiveSprite, hullActiveSprite;

	private SpriteRenderer equipmentHullsBtnRender;

	private Inventory inventory, storage, marketInv, shipInv, buybackInv;

	private HullsDisplay hullsDisplay;

	private MarketEquipmentScreen marketEquipmentScreen;

	private ShipData shipData;

	private PlanetScene planetScene;

	private BoxCollider2D closeBtn, equipmentHullsBtn;

	private Vector3 mouseToWorldPosition;

	private RaycastHit2D hit;

	private TextMesh cashTxt;

	void Awake () {
		marketEquipmentScreen = transform.FindChild ("Equipment Market").GetComponent<MarketEquipmentScreen>();
		hullsDisplay = transform.FindChild ("Hulls Market").GetComponent<HullsDisplay> ();
		hullsDisplay.fillWithRandomHulls(15, "Hull Item");

		closeBtn = transform.FindChild ("Close Btn").GetComponent<BoxCollider2D> ();
		equipmentHullsBtn = transform.FindChild ("Equipment Hulls Btn").GetComponent<BoxCollider2D> ();
		equipmentHullsBtnRender = equipmentHullsBtn.transform.GetComponent<SpriteRenderer> ();

		cashTxt = transform.FindChild ("CashTxt").GetComponent<TextMesh> ();
		cashTxt.gameObject.GetComponent<MeshRenderer> ().sortingOrder = 1;
	}

	public void showScreen (PlanetScene planetScene, Inventory inventory, Inventory storage, Inventory marketInv,
	                        Inventory shipInv, Inventory buybackInv, ShipData shipData)
	{
		gameObject.SetActive (true);

		this.planetScene = planetScene;
		this.inventory = inventory;
		this.storage = storage;
		this.marketInv = marketInv;
		this.shipInv = shipInv;
		this.buybackInv = buybackInv;
		this.shipData = shipData;

		showEquipmentScreen ();
	}

	void Update () {
		if (Input.GetMouseButtonDown (0)) {
			mouseToWorldPosition = Camera.main.ScreenToWorldPoint (Input.mousePosition);
			hit = Physics2D.Raycast(mouseToWorldPosition, Vector2.zero, 1);
			if (hit.collider != null) {
				switch (hit.collider.name) {
					case "Close Btn": closeScreen(); break;
					case "Equipment Hulls Btn": changeScreens (); break;
				}
			}
		}
		cashTxt.text = Variables.cash.ToString("C0");
	}

	private void changeScreens () {
		if (!marketEquipmentScreen.gameObject.activeInHierarchy) {
			showEquipmentScreen();	
		} else {
			showHullsScreen();
		}
	}

	private void showEquipmentScreen () {
		equipmentHullsBtnRender.sprite = equipmentActiveSprite;
		hullsDisplay.gameObject.SetActive (false);
		marketEquipmentScreen.showScreen (this, inventory, storage, marketInv, shipInv, buybackInv, shipData);
	}

	private void showHullsScreen () {
		equipmentHullsBtnRender.sprite = hullActiveSprite;
		marketEquipmentScreen.closeScreen ();
		hullsDisplay.showScreen (inventory, storage, shipInv);
	}

	private void closeScreen () {
		marketEquipmentScreen.closeScreen ();
		hullsDisplay.gameObject.SetActive (false);
		planetScene.setPlanetEnabled(true);
		gameObject.SetActive (false);
	}
}