using UnityEngine;
using System.Collections;

public class UserInterface : MonoBehaviour {

	public Texture background, shield, health, planetDescription;

	private Texture planetSurface;

	private float shieldWidth, startX = Screen.width - 10, maxWidth = 200;

	public Font font;

	public GUIStyle statusBtnStyle, shipBtnStyle, cashStyle, planetNameStyle, planetLandStyle;

	private StarSystem starSystem;

	private ShipInformationScreen shipInformation;

	private PlayerShip ship;

	private bool planetDescriptVisible;

	private string planetName;

	private PlanetType planetType;

	private Rect backgroundRect = new Rect (0, 10, Screen.width, 50),
				 cashRect = new Rect(Screen.width - 20, 35, 0, 0),
				 statusBtnRect = new Rect (10, 10, 50, 50),
				 shipInfoBtnRect = new Rect (60, 10, 50, 50),
				 shieldBarRect = new Rect(0, Screen.height - 70, 0, 50),
				 healthBarRect = new Rect(0, Screen.height - 50, 0, 50),
				 planetDescriptRect = new Rect(Screen.width - 220, 80, 210, 290), 
				 planetSurfaceRect, planetNameRect, planetLandRect;

	public UserInterface init (StarSystem starSystem, ShipInformationScreen shipInformation, PlayerShip ship) {
		this.starSystem = starSystem;
		this.shipInformation = shipInformation;
		this.ship = ship;

		if (ship != null) { updateShip(); }

		hidePlanetInfo();

		planetSurfaceRect = new Rect(planetDescriptRect.x + 5, planetDescriptRect.y + 5, 200, 125);
		planetNameRect = new Rect(planetDescriptRect.x + planetDescriptRect.width / 2, planetDescriptRect.y + planetDescriptRect.height / 2 + 10, 0, 0);
		planetLandRect = new Rect(planetDescriptRect.x + 5, planetDescriptRect.y + planetDescriptRect.height - 40, 200, 35);

		setEnabled(true);

		return this;
	}

	void OnGUI () {
		GUI.DrawTexture(backgroundRect, background);
		GUI.Label(cashRect, Vars.cash.ToString(), cashStyle);
		if (GUI.Button(statusBtnRect, "", statusBtnStyle)) {
			
		}
		if (starSystem != null) {
			if (GUI.Button (shipInfoBtnRect, "", shipBtnStyle)) {
				if (!shipInformation.gameObject.activeInHierarchy) {
					showShipInformation();
				}
			}

			GUI.DrawTexture(healthBarRect, health);
			GUI.DrawTexture(shieldBarRect, shield);

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

	public void setEnabled (bool enabled) {
		gameObject.SetActive(enabled);
	}

	private void showShipInformation () {
		shipInformation.showScreen();
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
		shieldBarRect.x = startX - shieldBarRect.width;

		healthBarRect.width = maxWidth * ((float)ship.getHealth() / ship.getFullHealth());
		healthBarRect.x = startX - healthBarRect.width;
	}
}