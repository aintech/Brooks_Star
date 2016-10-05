using UnityEngine;
using System.Collections;

public class UserInterface : MonoBehaviour {

	public static bool showInterface = true;

	public Texture shield, health, planetDescription, barsHolder;

	private Texture planetSurface;

	private float shieldWidth, maxWidth = 200;//, startX = Screen.width - 10

	public GUIStyle statusBtnStyle, messengerStyle, planetNameStyle, planetLandStyle;

	private StarSystem starSystem;

	private StatusScreen statusScreen;

	private PlayerShip ship;

	private bool planetDescriptVisible;

	private string planetName;

	private PlanetType planetType;

	private string messageText = null;

	private int counter;

	private Color32 textColor;

	private Rect statusBtnRect = new Rect (Screen.width - 44, Screen.height - 56, 48, 48),
				 shieldBarRect = new Rect(13, Screen.height - 65, 10, 50),
				 healthBarRect = new Rect(13, Screen.height - 45, 10, 50),
				 barsHolderRect = new Rect(0, Screen.height - 128, 256, 128),
				 planetDescriptRect = new Rect(20, 20, 210, 290),//planetDescriptRect = new Rect(Screen.width - 220, 80, 210, 290), 
				 messengerRect = new Rect(10, Screen.height - 50, Screen.width, 50),
				 planetSurfaceRect, planetNameRect, planetLandRect;

	public UserInterface init (StatusScreen statusScreen, StarSystem starSystem, PlayerShip ship) {
		this.statusScreen = statusScreen;
		this.starSystem = starSystem;
		this.ship = ship;

		if (ship != null) { updateShip(); }

		hidePlanetInfo();

		planetSurfaceRect = new Rect(planetDescriptRect.x + 5, planetDescriptRect.y + 5, 200, 125);
		planetNameRect = new Rect(planetDescriptRect.x + planetDescriptRect.width / 2, planetDescriptRect.y + planetDescriptRect.height / 2 + 10, 0, 0);
		planetLandRect = new Rect(planetDescriptRect.x + 5, planetDescriptRect.y + planetDescriptRect.height - 40, 200, 35);

		textColor = messengerStyle.normal.textColor;

		GetComponent<Messenger>().init(this);
		GetComponent<Minimap>().init(starSystem, ship.transform);

		setEnabled(true);

		return this;
	}

	void Update () {
		if (Input.GetKeyDown(KeyCode.I)) {
			if (statusScreen == null) { return; }
			if (statusScreen.gameObject.activeInHierarchy) {
				statusScreen.closeScreen();
			} else if (showInterface) {
				statusScreen.showScreen();
			}
		}
	}

	void OnGUI () {
		if (showInterface) {
			if (GUI.Button(statusBtnRect, "", statusBtnStyle)) {
				statusScreen.showScreen();
			}
			if (starSystem != null) {
				GUI.DrawTexture(healthBarRect, health);
				GUI.DrawTexture(shieldBarRect, shield);
				GUI.DrawTexture(barsHolderRect, barsHolder);

				if (planetDescriptVisible) {
					GUI.DrawTexture(planetDescriptRect, planetDescription);
					GUI.DrawTexture(planetSurfaceRect, planetSurface);
					GUI.Label(planetNameRect, planetName, planetNameStyle);
					if (GUI.Button(planetLandRect, "", planetLandStyle)) {
						starSystem.landOnPlanet(planetType);
					}
				}
			}
		}
		if (messageText != null) {
			GUI.Label(messengerRect, messageText, messengerStyle);
			if (--counter <= 0) {
				if (textColor.a > 100) {
					messengerStyle.normal.textColor = textColor;
					--textColor.a;
				} else {
					messageText = null;
				}
			}
		}
	}

	public void setEnabled (bool enabled) {
		gameObject.SetActive(enabled);
	}

	public void showPlanetInfo (PlanetType planetType) {
		this.planetType = planetType;
		planetDescriptVisible = true;
		planetSurface = Imager.getPlanetSurface(planetType).texture;
		planetName = planetType.getName();
    }

    public void hidePlanetInfo () {
		planetDescriptVisible = false;
    }

	public void updateShip () {
		shieldBarRect.width = maxWidth * ((float)ship.getShield() / ship.getFullShield());
//		shieldBarRect.x = startX - shieldBarRect.width;

		healthBarRect.width = maxWidth * ((float)ship.getHealth() / ship.getFullHealth());
//		healthBarRect.x = startX - healthBarRect.width;
	}

	public void setMessageText (string text) {
		counter = 100;
		textColor.a = 255;
		messengerStyle.normal.textColor = textColor;
		messageText = text;
	}
}