﻿using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class Planet : MonoBehaviour, ButtonHolder {

	public static bool newGame = true;

	private SpriteRenderer bgRender;

	private Button marketBtn, industrialBtn, hangarBtn, leaveBtn;

	private MarketScreen marketScreen;
	
	private HangarScreen hangarScreen;

	private IndustrialScreen industrialScreen;

	private Inventory inventory, storage, marketInv, shipInv, buybackInv;

	private ShipData shipData;

	private MessageBox messageBox;

	private Storyline story;

	void Awake () {
		bgRender = GetComponent<SpriteRenderer>();

		Vars.userInterface = GameObject.Find("User Interface").GetComponent<UserInterface>().init();
		Vars.userInterface.setEnabled(false);

		marketScreen = GameObject.Find("Market Screen").GetComponent<MarketScreen> ();
		industrialScreen = GameObject.Find("Industrial Screen").GetComponent<IndustrialScreen>();
		hangarScreen = GameObject.Find ("Hangar Screen").GetComponent<HangarScreen> ();

		Transform inventories = GameObject.Find("Inventories").transform;

		inventory = inventories.Find ("Inventory").GetComponent<Inventory> ().init(true);
		storage = inventories.Find ("Storage").GetComponent<Inventory> ().init(false);
		shipInv = inventories.Find ("Ship Inventory").GetComponent<Inventory> ().init(false);
		marketInv = inventories.Find ("Market Inventory").GetComponent<Inventory> ().init(false);
		buybackInv = inventories.Find ("Buyback Inventory").GetComponent<Inventory> ().init(false);

		shipData = GameObject.Find("Ship Data").GetComponent<ShipData> ().init();

		marketBtn = transform.Find("Market Button").GetComponent<Button>().init();
		industrialBtn = transform.Find("Industrial Button").GetComponent<Button>().init();
		hangarBtn = transform.Find("Hangar Button").GetComponent<Button>().init();
		leaveBtn = transform.Find("Leave Button").GetComponent<Button>().init();

		messageBox = GameObject.Find("Message Box").GetComponent<MessageBox>();
		story = GameObject.Find("Storyline").GetComponent<Storyline>();

		GameObject.Find("Imager").GetComponent<Imager>().init();

		inventory.setCapacity(shipData.getHullType().getStorageCapacity());
		storage.setCapacity (-1);
		shipInv.setCapacity (-1);
		marketInv.setCapacity (-1);
		buybackInv.setCapacity (-1);

		marketScreen.init(this, shipData, inventory, storage, marketInv, shipInv, buybackInv);
		industrialScreen.init(this);
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

//		inventory.fillWithRandomItems(30, "Player Item");
		inventory.calculateFreeVolume();
		marketInv.fillWithRandomItems(50, "Market Item");

		showPlanet();
		messageBox.showNewMessage(story.getMessageContainer(Storyline.StoryPart.INTRODUCTION));
	}

	public void showPlanet () {
		setPlanetBtnsEnabled(true);
		bgRender.sprite = Imager.getPlanetBG(Vars.planetType);
	}

	public void leavePlanet () {
		setDataToVars();
		SceneManager.LoadScene("SpaceTravel");
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
			case PlanetType.CORAS: Vars.marketCORAS = marketInv.getItems (); break;
		}
	}
	
	private void getDataFromVars () {
		inventory.loadItems (Vars.inventory);
		storage.loadItems (Vars.storage);
		switch(Vars.planetType) {
			case PlanetType.CORAS: marketInv.loadItems(Vars.marketCORAS); break;
		}
	}

	private enum ScreenType {
		MARKET, INDUSTRIAL, HANGAR
	}
}