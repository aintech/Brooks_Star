using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class Planet : MonoBehaviour, ButtonHolder {

	public static bool newGame = true;

	private SpriteRenderer bgRender;

	private Button marketBtn, hangarBtn, leaveBtn;

	private MarketScreen marketScreen;
	
	private HangarScreen hangarScreen;
	
	private Inventory inventory, storage, marketInv, shipInv, buybackInv;

	private ShipData shipData;

	private MessageBox messageBox;

	private Storyline story;

	void Awake () {
		bgRender = GetComponent<SpriteRenderer>();

		marketScreen = GameObject.Find("Market Screen").GetComponent<MarketScreen> ();
		hangarScreen = GameObject.Find ("Hangar Screen").GetComponent<HangarScreen> ();

		Transform inventories = GameObject.Find("Inventories").transform;

		inventory = inventories.Find ("Inventory").GetComponent<Inventory> ().init();
		storage = inventories.Find ("Storage").GetComponent<Inventory> ().init();
		shipInv = inventories.Find ("Ship Inventory").GetComponent<Inventory> ().init();
		marketInv = inventories.Find ("Market Inventory").GetComponent<Inventory> ().init();
		buybackInv = inventories.Find ("Buyback Inventory").GetComponent<Inventory> ().init();

		shipData = GameObject.Find("Ship Data").GetComponent<ShipData> ().init();

		marketBtn = transform.Find("MarketBtn").GetComponent<Button>().init();
		hangarBtn = transform.Find("HangarBtn").GetComponent<Button>().init();
		leaveBtn = transform.Find("LeaveBtn").GetComponent<Button>().init();

		messageBox = GameObject.Find("MessageBox").GetComponent<MessageBox>();
		story = GameObject.Find("Storyline").GetComponent<Storyline>();

		GameObject.Find("Imager").GetComponent<Imager>().init();

		inventory.setCapacity(shipData.getHullType().getStorageCapacity());
		storage.setCapacity (-1);
		shipInv.setCapacity (-1);
		marketInv.setCapacity (-1);
		buybackInv.setCapacity (-1);

		marketScreen.init(this, shipData, inventory, storage, marketInv, shipInv, buybackInv);
		hangarScreen.init(this, shipData, inventory, storage);

		messageBox.init(this);
		story.init();

		if (newGame) {
			startNewGame();
			newGame = false;
		}
	}

	private void startNewGame () {
		shipData.initializeRandomShip(HullType.Corvette);
		shipData.setCurrentHealth (shipData.getHullType ().getMaxHealth ());

		inventory.fillWithRandomItems(30, "Player Item");
		marketInv.fillWithRandomItems(50, "Market Item");

		showPlanet();
		messageBox.showNewMessage(story.getMessageContainer(Storyline.StoryPart.INTRODUCTION));
	}

	public void showPlanet () {
		setPlanetBtnsEnabled(true);
		bgRender.sprite = Imager.getPlanetBG(Variables.planetType);
	}

	public void leavePlanet () {
		setDataToVariables();
		SceneManager.LoadScene("SpaceTravel");
	}

	public void showMarketScreen () {
		marketScreen.showScreen();
		setPlanetBtnsEnabled(false);
	}
	
	public void showHangarScreen () {
		hangarScreen.showScreen ();
		setPlanetBtnsEnabled(false);
	}

	public void setPlanetBtnsEnabled (bool enabled) {
		marketBtn.gameObject.SetActive(enabled);
		hangarBtn.gameObject.SetActive(enabled);
		leaveBtn.gameObject.SetActive(enabled);
	}


	public void fireClickButton (Button btn) {
		if (btn == marketBtn) {
			showMarketScreen();
		} else if (btn == hangarBtn) {
			showHangarScreen();
		} else if (btn == leaveBtn) {
			leavePlanet();
		} else {
			Debug.Log("Unknown button: " + btn.name);
		}
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
