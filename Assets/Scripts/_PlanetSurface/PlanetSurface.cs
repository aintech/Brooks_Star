using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class PlanetSurface : MonoBehaviour, ButtonHolder, Hideable {

	public static bool newGame = true;

	public static Hideable topHideable;

//	private SpriteRenderer bgRender;

	private Background background;

	private Button exploreBtn, marketBtn, industrialBtn, leaveBtn;

	private ExploreScreen exploreScreen;

	private MarketScreen marketScreen;

	private IndustrialScreen industrialScreen;

	private Inventory inventory, market;

	private MessageBox messageBox;

	private Storyline story;

	private StatusScreen statusScreen;

	void Awake () {
		Imager.initialize();
		Player.init();

		background = transform.Find("Background").GetComponent<Background>().init();

		exploreScreen = GameObject.Find("Explore Screen").GetComponent<ExploreScreen>();
		marketScreen = GameObject.Find("Market Screen").GetComponent<MarketScreen> ();
		industrialScreen = GameObject.Find("Industrial Screen").GetComponent<IndustrialScreen>();

		exploreBtn = transform.Find("Explore Button").GetComponent<Button>().init();
		marketBtn = transform.Find("Market Button").GetComponent<Button>().init();
		industrialBtn = transform.Find("Industrial Button").GetComponent<Button>().init();
		leaveBtn = transform.Find("Leave Button").GetComponent<Button>().init();

		PlanetSurface.topHideable = this;

		ItemDescriptor descriptor = GameObject.Find("Item Descriptor").GetComponent<ItemDescriptor>().init();

		statusScreen = GameObject.Find("Status Screen").GetComponent<StatusScreen>().init(null, descriptor);
		inventory = statusScreen.getInventory();
		descriptor.initPlayerInventory(inventory);

		Vars.userInterface = GameObject.FindGameObjectWithTag("UserInterface").GetComponent<UserInterface>().init(statusScreen, null, null);

		messageBox = GameObject.Find("Message Box").GetComponent<MessageBox>();
		story = GameObject.Find("Storyline").GetComponent<Storyline>();

		marketScreen.init(this, statusScreen.getShipData(), inventory, descriptor);
		exploreScreen.init(this);
		industrialScreen.init(this);

		market = marketScreen.getMarket();

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

		inventory.fillWithRandomItems(50, "Player Item");
		market.fillWithRandomItems(50, "Market Item");

		sendToVars();
//		messageBox.showNewMessage(story.getMessageContainer(Storyline.StoryPart.INTRODUCTION));
	}

	public void landPlanet () {
		initFromVars();
		if (market.getItems().Count == 0) { market.fillWithRandomItems(); }
		statusScreen.getShipData().setShieldToMax();
		inventory.calculateFreeVolume();
		Vars.userInterface.setEnabled(true);
		background.setBackground();
//		bgRender.sprite = Imager.getPlanetSurface(Vars.planetType);
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
			case ScreenType.MARKET: marketScreen.showScreen(); break;
			case ScreenType.INDUSTRIAL: industrialScreen.showScreen(); break;
			default: Debug.Log("Unknown screen type"); break;
		}
		setVisible(false);
	}

	public void fireClickButton (Button btn) {
		if (btn == marketBtn) { showScreen(ScreenType.MARKET); }
		else if (btn == industrialBtn) { showScreen(ScreenType.INDUSTRIAL); }
		else if (btn == exploreBtn) { showScreen(ScreenType.EXPLORE); }
		else if (btn == leaveBtn) { leavePlanet(); }
		else { Debug.Log("Unknown button: " + btn.name); }
	}



	private void sendToVars () {
		statusScreen.sendToVars();
		market.sendToVars();
	}
	
	private void initFromVars () {
		statusScreen.initFromVars();
		market.initFromVars();
	}

	public void setVisible (bool visible) {
		exploreBtn.setVisible(visible);
		marketBtn.setVisible(visible);
		industrialBtn.setVisible(visible);
		leaveBtn.setVisible(visible);
		if (visible) { PlanetSurface.topHideable = this; }
	}

	private enum ScreenType {
		EXPLORE, MARKET, INDUSTRIAL
	}
}