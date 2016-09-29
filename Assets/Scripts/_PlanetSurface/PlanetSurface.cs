using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class PlanetSurface : MonoBehaviour, ButtonHolder, Hideable {

	public static bool newGame = true;

	public static Hideable topHideable;

	private Background background;

	private Button exploreBtn, marketBtn, hangarBtn, industrialBtn, leaveBtn;

	private Button[] btns;

	private ExploreScreen exploreScreen;

	private HangarScreen hangarScreen;

	private EquipmentsMarket market;

	private IndustrialScreen industrialScreen;

	private Inventory inventory, buyMarket;

	private MessageBox messageBox;

	private Storyline story;

	private StatusScreen statusScreen;

	void Awake () {
		Imager.initialize();
		Player.init();

		background = transform.Find("Background").GetComponent<Background>().init();

		exploreBtn = transform.Find("Explore Button").GetComponent<Button>().init();
		marketBtn = transform.Find("Market Button").GetComponent<Button>().init();
		hangarBtn = transform.Find("Hangar Button").GetComponent<Button>().init();
		industrialBtn = transform.Find("Industrial Button").GetComponent<Button>().init();
		leaveBtn = transform.Find("Leave Button").GetComponent<Button>().init();

		btns = new Button[]{exploreBtn, marketBtn, hangarBtn, industrialBtn, leaveBtn};

		PlanetSurface.topHideable = this;

		ItemDescriptor descriptor = GameObject.Find("Item Descriptor").GetComponent<ItemDescriptor>().init();

		statusScreen = GameObject.Find("Status Screen").GetComponent<StatusScreen>().init(null, descriptor);
		inventory = statusScreen.getInventory();
		descriptor.initPlayerInventory(inventory);

		Vars.userInterface = GameObject.FindGameObjectWithTag("UserInterface").GetComponent<UserInterface>().init(statusScreen, null, null);

		messageBox = GameObject.Find("Message Box").GetComponent<MessageBox>();
		story = GameObject.Find("Storyline").GetComponent<Storyline>();

		exploreScreen = GameObject.Find("Explore Screen").GetComponent<ExploreScreen>().init(this);
		market = GameObject.Find("Equipments Market").GetComponent<EquipmentsMarket> ().init(this, inventory, descriptor);
		hangarScreen = GameObject.Find("Hangar Screen").GetComponent<HangarScreen>().init(this, inventory, statusScreen.getShipData());
		industrialScreen = GameObject.Find("Industrial Screen").GetComponent<IndustrialScreen>().init(this);

		buyMarket = market.getBuyMarket();

		messageBox.init(this);
		story.init();

		if (newGame) {
			startNewGame();
			newGame = false;
		}

		landPlanet();
	}

	private void startNewGame () {
		statusScreen.getShipData().initializeRandomShip(HullType.Corvette);

		buyMarket.fillWithRandomItems(50, "Market Item");

		sendToVars();
//		messageBox.showNewMessage(story.getMessageContainer(Storyline.StoryPart.INTRODUCTION));
	}

	public void landPlanet () {
		initFromVars();
		if (buyMarket.getItems().Count == 0) { buyMarket.fillWithRandomItems(); }
		statusScreen.getShipData().setShieldToMax();
		inventory.calculateFreeVolume();
		Vars.userInterface.setEnabled(true);
		background.setBackground();
		setVisible(true);
	}

	public void leavePlanet () {
		if (statusScreen.getShipData().getEnergyNeeded() < 0) {
			Messenger.showMessage("Кораблю не хватает энергии!");
		} else if (statusScreen.getShipData().getSlot(HullSlot.Type.ENGINE, 0).item == null) {
			Messenger.showMessage("У корабля отсутствует двигатель!");
		} else if (inventory.getFreeVolume() < .01f) {
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
		else if (btn == exploreBtn) { showScreen(ScreenType.EXPLORE); }
		else if (btn == leaveBtn) { leavePlanet(); }
		else { Debug.Log("Unknown button: " + btn.name); }
	}



	private void sendToVars () {
		statusScreen.sendToVars();
		buyMarket.sendToVars();
	}
	
	private void initFromVars () {
		statusScreen.initFromVars();
		buyMarket.initFromVars();
	}

	public void setVisible (bool visible) {
		foreach (Button btn in btns) { btn.setVisible(visible); }
		if (visible) { PlanetSurface.topHideable = this; }
	}

	private enum ScreenType {
		EXPLORE, MARKET, HANGAR, INDUSTRIAL
	}
}