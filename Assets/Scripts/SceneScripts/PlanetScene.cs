using UnityEngine;
using System.Collections;

public class PlanetScene : MonoBehaviour {

	public static bool newGame = true;

	private MarketScreen marketScreen;
	
	private HangarScreen hangarScreen;
	
	private Inventory inventory, storage, marketInv, shipInv, buybackInv;

	private ShipData shipData;

	private Planet planet;

	private MessageBox messageBox;

	private Storyline story;

	void Awake () {
		Variables.planetScene = this;

		marketScreen = GameObject.Find("Market Screen").GetComponent<MarketScreen> ();
		hangarScreen = GameObject.Find ("Hangar Screen").GetComponent<HangarScreen> ();
		
		inventory = GameObject.Find ("Inventory").GetComponent<Inventory> ();
		storage = GameObject.Find ("Storage").GetComponent<Inventory> ();
		shipInv = GameObject.Find ("Ship Inventory").GetComponent<Inventory> ();
		marketInv = GameObject.Find ("Market Inventory").GetComponent<Inventory> ();
		buybackInv = GameObject.Find ("Buyback Inventory").GetComponent<Inventory> ();
		
		shipData = GameObject.Find("Ship Data").GetComponent<ShipData> ();

		messageBox = GameObject.Find("MessageBox").GetComponent<MessageBox>();
		story = GameObject.Find("Storyline").GetComponent<Storyline>();

		GameObject.Find("Imager").GetComponent<Imager>().init();

		inventory.setCapacity(shipData.getHullType().getStorageCapacity());
		storage.setCapacity (-1);
		shipInv.setCapacity (-1);
		marketInv.setCapacity (-1);
		buybackInv.setCapacity (-1);

		marketScreen.gameObject.SetActive (false);
		hangarScreen.gameObject.SetActive (false);
		
		inventory.gameObject.SetActive (false);
		storage.gameObject.SetActive (false);
		marketInv.gameObject.SetActive (false);
		shipInv.gameObject.SetActive (false);
		buybackInv.gameObject.SetActive (false);
		
		shipData.gameObject.SetActive (false);

		messageBox.init();
		story.init();

		planet = GameObject.Find("Planet").GetComponent<Planet>().init(this);

		if (newGame) {
			startNewGame();
			newGame = false;
		}
	}

	private void startNewGame () {
		shipData.initializeRandomShip(HullType.Corvette);
		shipData.setCurrentHealth (shipData.getHullType ().getMaxHealth ());

		inventory.fillWithRandomItems (30, "Player Item");
		marketInv.fillWithRandomItems(50, "Market Item");
		inventory.sortInventory ();
		marketInv.sortInventory ();

		setPlanetEnabled(false);
		messageBox.showNewMessage(story.getMessageContainer(Storyline.StoryPart.INTRODUCTION));
	}

	public void leavePlanet () {
		setDataToVariables();
		Variables.planetScene = null;
		Application.LoadLevel("SpaceTravel");
	}

	public void showMarketScreen () {
		marketScreen.showScreen(this, inventory, storage, marketInv, shipInv, buybackInv, shipData);
		setPlanetEnabled(false);
	}
	
	public void showHangarScreen () {
		hangarScreen.showScreen (this, inventory, storage, shipData);
		setPlanetEnabled(false);
	}

	public void setPlanetEnabled (bool enabled) {
		planet.setEnabled(enabled);
	}

	private void setDataToVariables () {
		shipData.sendToVariables ();
		Variables.inventory = inventory.getItems ();
		Variables.storage = storage.getItems ();
		switch(Variables.planetType) {
		case PlanetType.CORAS: Variables.marketCORAS = marketInv.getItems (); break;
		}
	}
	
	private void getDataFromVariables () {
		inventory.loadItems (Variables.inventory);
		storage.loadItems (Variables.storage);
		switch(Variables.planetType) {
		case PlanetType.CORAS: marketInv.loadItems(Variables.marketCORAS); break;
		}
	}
}
