using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class PlanetSurface : MonoBehaviour, ButtonHolder {

	public static bool newGame = true;

	private SpriteRenderer bgRender;

	private Button marketBtn, industrialBtn, hangarBtn, leaveBtn;

	private MarketScreen marketScreen;
	
	private HangarScreen hangarScreen;

	private IndustrialScreen industrialScreen;

	private Inventory inventory, storage, market, inUse, buyback;

	private ShipData shipData;

	private MessageBox messageBox;

	private Storyline story;

	void Awake () {
		bgRender = GetComponent<SpriteRenderer>();

		Imager.initialize();

		Vars.userInterface = GameObject.FindGameObjectWithTag("UserInterface").GetComponent<UserInterface>().init(null, null, null);

		marketScreen = GameObject.Find("Market Screen").GetComponent<MarketScreen> ();
		industrialScreen = GameObject.Find("Industrial Screen").GetComponent<IndustrialScreen>();
		hangarScreen = GameObject.Find ("Hangar Screen").GetComponent<HangarScreen> ();

        shipData = GameObject.Find("Ship Data").GetComponent<ShipData>().init();

        Transform inventories = GameObject.Find("Inventories").transform;
        inventory = inventories.Find ("Inventory").GetComponent<Inventory> ().init(Inventory.InventoryType.INVENTORY);
		storage = inventories.Find ("Storage").GetComponent<Inventory> ().init(Inventory.InventoryType.STORAGE);
		inUse = inventories.Find ("InUse").GetComponent<Inventory> ().init(Inventory.InventoryType.INUSE);
		market = inventories.Find ("Market").GetComponent<Inventory> ().init(Inventory.InventoryType.MARKET);
		buyback = inventories.Find ("Buyback").GetComponent<Inventory> ().init(Inventory.InventoryType.BUYBACK);

        inventory.setCapacity(shipData.getHullType().getStorageCapacity());

        marketBtn = transform.Find("Market Button").GetComponent<Button>().init();
		industrialBtn = transform.Find("Industrial Button").GetComponent<Button>().init();
		hangarBtn = transform.Find("Hangar Button").GetComponent<Button>().init();
		leaveBtn = transform.Find("Leave Button").GetComponent<Button>().init();

		messageBox = GameObject.Find("Message Box").GetComponent<MessageBox>();
		story = GameObject.Find("Storyline").GetComponent<Storyline>();

		marketScreen.init(this, shipData, inventory, storage, market, inUse, buyback);
		industrialScreen.init(this);
		hangarScreen.init(this, shipData, inventory, storage);

		messageBox.init(this);
		story.init();

		if (newGame) {
			startNewGame();
			newGame = false;
		}

		landPlanet();
	}

	private void startNewGame () {
		shipData.initializeRandomShip(HullType.Corvette);
		shipData.setCurrentHealth (shipData.getHullType ().getMaxHealth ());

//		inventory.fillWithRandomItems(30, "Player Item");
		market.fillWithRandomItems(50, "Market Item");

		setDataToVars();
//		messageBox.showNewMessage(story.getMessageContainer(Storyline.StoryPart.INTRODUCTION));
	}

	public void landPlanet () {
		//TODO: По прибытии ломается рынок
		getDataFromVars();
		shipData.setCurrentShield(shipData.getShield());
		inventory.calculateFreeVolume();
		Vars.userInterface.setEnabled(true);
		bgRender.sprite = Imager.getPlanetSurface(Vars.planetType);
		setPlanetBtnsEnabled(true);
	}

	public void leavePlanet () {
		if (shipData.getEnergyNeeded() < 0) {
			Messenger.showMessage("Кораблю не хватает энергии");
		} else if (shipData.getSlotByType(HullSlot.HullSlotType.Engine, 0).getItem() == null) {
			Messenger.showMessage("У корабля отсутствует двигатель");
		} else {
			setDataToVars();
			SceneManager.LoadScene("StarSystem");
		}
	}

	private void showScreen (ScreenType type) {
		switch (type) {
			case ScreenType.MARKET: marketScreen.showScreen(); break;
			case ScreenType.HANGAR: hangarScreen.showScreen(); break;
			case ScreenType.INDUSTRIAL: industrialScreen.showScreen(); break;
			default: Debug.Log("Unknown screen type"); break;
		}
		setPlanetBtnsEnabled(false);
	}

	public void setPlanetBtnsEnabled (bool enabled) {
		marketBtn.gameObject.SetActive(enabled);
		industrialBtn.gameObject.SetActive(enabled);
		hangarBtn.gameObject.SetActive(enabled);
		leaveBtn.gameObject.SetActive(enabled);
	}

	public void fireClickButton (Button btn) {
		if (btn == marketBtn) { showScreen(ScreenType.MARKET); }
		else if (btn == hangarBtn) { showScreen(ScreenType.HANGAR); }
		else if (btn == industrialBtn) { showScreen(ScreenType.INDUSTRIAL); }
		else if (btn == leaveBtn) { leavePlanet(); }
		else { Debug.Log("Unknown button: " + btn.name); }
	}

	private void setDataToVars () {
		shipData.sendToVars ();
		Vars.inventory = inventory.getItems ();
		Vars.storage = storage.getItems ();
		switch(Vars.planetType) {
			case PlanetType.CORAS: Vars.marketCORAS = market.getItems (); break;
		}
	}
	
	private void getDataFromVars () {
		inventory.loadItems (Vars.inventory);
		storage.loadItems (Vars.storage);
		switch(Vars.planetType) {
			case PlanetType.CORAS: market.loadItems(Vars.marketCORAS); break;
		}
	}

	private enum ScreenType {
		MARKET, INDUSTRIAL, HANGAR
	}
}