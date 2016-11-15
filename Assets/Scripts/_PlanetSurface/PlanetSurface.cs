using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class PlanetSurface : MonoBehaviour, ButtonHolder, Hideable {

	public Transform itemPrefab;

	public static Hideable topHideable;

	private Background background;

	private Button marketBtn, hangarBtn, industrialBtn, leaveBtn;

	private Button[] btns;

	private ExploreScreen exploreScreen;

	private HangarScreen hangarScreen;

	private EquipmentsMarket market;

	private IndustrialScreen industrialScreen;

	private MessageBox messageBox;

	private Storyline story;

	private StatusScreen statusScreen;

	void Awake () {
		ItemFactory.itemPrefab = itemPrefab;
		Imager.initialize();
		Player.init();

		GameObject.Find("Images Provider").GetComponent<ImagesProvider>().init();

		background = transform.Find("Background").GetComponent<Background>().init();

		marketBtn = transform.Find("Market Button").GetComponent<Button>().init();
		hangarBtn = transform.Find("Hangar Button").GetComponent<Button>().init();
		industrialBtn = transform.Find("Industrial Button").GetComponent<Button>().init();
		leaveBtn = transform.Find("Leave Button").GetComponent<Button>().init();

		btns = new Button[]{marketBtn, hangarBtn, industrialBtn, leaveBtn};

		PlanetSurface.topHideable = this;

		ItemDescriptor descriptor = GameObject.Find("Item Descriptor").GetComponent<ItemDescriptor>().init();

		statusScreen = GameObject.Find("Status Screen").GetComponent<StatusScreen>().init(null, descriptor);

		descriptor.playerData = statusScreen.playerData;

		Vars.userInterface = GameObject.FindGameObjectWithTag("UserInterface").GetComponent<UserInterface>().init(statusScreen);

		messageBox = GameObject.Find("Message Box").GetComponent<MessageBox>();
		story = GameObject.Find("Storyline").GetComponent<Storyline>();

		exploreScreen = GameObject.Find("Explore Screen").GetComponent<ExploreScreen>().init(this, statusScreen.playerData, descriptor);
		market = GameObject.Find("Equipments Market").GetComponent<EquipmentsMarket> ().init(this, statusScreen.inventory, descriptor);
		hangarScreen = GameObject.Find("Hangar Screen").GetComponent<HangarScreen>().init(this, statusScreen.inventory, statusScreen.shipData);
		industrialScreen = GameObject.Find("Industrial Screen").GetComponent<IndustrialScreen>().init(this);

		messageBox.init(this);
		story.init();

		if (Vars.shipCurrentHealth == -1) {
			startNewGame();
		}

		landPlanet();
	}

	private void startNewGame () {
		statusScreen.shipData.initializeRandomShip(HullType.CORVETTE);

		statusScreen.inventory.fillWithRandomItems(50, "Player Item");
		market.buyMarket.fillWithRandomItems(50, "Market Item");

		sendToVars();
//		messageBox.showNewMessage(story.getMessageContainer(Storyline.StoryPart.INTRODUCTION));
	}

	public void landPlanet () {
		initFromVars();
		if (Vars.planetType.isColonized() && market.buyMarket.getItems().Count == 0) { market.buyMarket.fillWithRandomItems(); }
		statusScreen.shipData.setShieldToMax();
		statusScreen.inventory.calculateFreeVolume();
		UserInterface.showInterface = true;
		background.setBackground();
		if (Vars.planetType.isColonized()) {
			setVisible(true);
		} else if (Vars.planetType.isPopulated()) {
			showScreen(ScreenType.EXPLORE);
		}
	}

	public void leavePlanet () {
		if (statusScreen.shipData.energyNeeded() < 0) {
			Messenger.showMessage("Кораблю не хватает энергии!");
		} else if (statusScreen.shipData.getSlot(HullSlot.Type.ENGINE, 0).item == null) {
			Messenger.showMessage("У корабля отсутствует двигатель!");
		} else if (statusScreen.inventory.getFreeVolume() < .01f) {
			Messenger.showMessage("Корабль перегружен!");
		} else {
			sendToVars();
			SceneManager.LoadScene("StarSystem");
		}
	}

	private void showScreen (ScreenType type) {
		switch (type) {
			case ScreenType.EXPLORE: exploreScreen.showScreen(); break;
			case ScreenType.MARKET: market.showScreen(); break;
			case ScreenType.HANGAR: hangarScreen.showScreen(); break;
			case ScreenType.INDUSTRIAL: industrialScreen.showScreen(); break;
			default: Debug.Log("Unknown screen type"); break;
		}
		setVisible(false);
	}

	public void fireClickButton (Button btn) {
		if (btn == marketBtn) { showScreen(ScreenType.MARKET); }
		else if (btn == hangarBtn) { showScreen(ScreenType.HANGAR); }
		else if (btn == industrialBtn) { showScreen(ScreenType.INDUSTRIAL); }
		else if (btn == leaveBtn) { leavePlanet(); }
		else { Debug.Log("Unknown button: " + btn.name); }
	}



	private void sendToVars () {
		statusScreen.sendToVars();
		if (Vars.planetType.isColonized()) { market.buyMarket.sendToVars(); }
	}
	
	private void initFromVars () {
		statusScreen.initFromVars();
		if (Vars.planetType.isColonized()) { market.buyMarket.initFromVars(); }
	}

	public void setVisible (bool visible) {
		foreach (Button btn in btns) { btn.setVisible(visible); }
		if (visible) { PlanetSurface.topHideable = this; }
	}

	private enum ScreenType {
		EXPLORE, MARKET, HANGAR, INDUSTRIAL
	}
}