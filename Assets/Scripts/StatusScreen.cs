using UnityEngine;
using System.Collections;

public class StatusScreen : InventoryContainedScreen {

	private bool onPlanetSurface;

	private Button gearSlotsBtn, perksBtn, closeBtn;

	private StarSystem starSystem;

	public StatusScreen init (bool onPlanetSurface, Inventory inventory, Inventory storage, StarSystem starSystem) {
		this.onPlanetSurface = onPlanetSurface;
		this.starSystem = starSystem;

		innerInit(inventory, storage);

		gearSlotsBtn = transform.Find("Gear Slots Button").GetComponent<Button>().init();
		perksBtn = transform.Find("Perks Button").GetComponent<Button>().init();
		closeBtn = transform.Find("Close Button").GetComponent<Button>().init();

		closeScreen();

		return this;
	}

	public void showScreen () {
		//TODO: прятать нижележащие экраны
		if (gameObject.activeInHierarchy) { return; }

		inventory.setContainerScreen(this);
		inventory.setInventoryToBegin ();

		if (onPlanetSurface) {
			transform.position = Vector3.zero;
			PlanetSurface.topHideable.setVisible(false);
			storage.setContainerScreen(this);
			storage.setInventoryToBegin ();
			storageBtn.setVisible(true);
			inventoryBtn.setVisible(true);
			setInventoryActive();
		} else {
			transform.position = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, transform.position.z);
			inventory.transform.parent.position = transform.position;
			foreach (Planet planet in starSystem.getPlanets()) {
				planet.setShipIsNear(false);
			}
			StarSystem.setGamePause(true);
			inventoryBtn.setVisible(false);
			storageBtn.setVisible(false);
			inventory.gameObject.SetActive(true);
		}

		gameObject.SetActive(true);
	}

	public void closeScreen () {
		hideItemInfo();
		inventory.gameObject.SetActive(false);
		gameObject.SetActive(false);

		if (onPlanetSurface) { 
			storage.gameObject.SetActive(false);
			PlanetSurface.topHideable.setVisible(true);
		}
		else { StarSystem.setGamePause(false); }
	}

	private void setInventoryActive () {
		storage.gameObject.SetActive(false);
		storageBtn.setActive(true);
		inventory.gameObject.SetActive(true);
		inventoryBtn.setActive(false);
		hideItemInfo();
	}

	private void setStorageActive () {
		inventory.gameObject.SetActive(false);
		inventoryBtn.setActive(true);
		storage.gameObject.SetActive(true);
		storageBtn.setActive(false);
		hideItemInfo();
	}

	private void setGearSlotsActive () {
		
	}

	private void setPerksActive () {
		
	}

	protected override void checkBtnPress (Button btn) {
		if (btn == inventoryBtn) { setInventoryActive(); }
		else if (btn == storageBtn) { setStorageActive(); }
		else if (btn == gearSlotsBtn) { setGearSlotsActive(); }
		else if (btn == perksBtn) { setPerksActive(); }
		else if (btn == closeBtn) { closeScreen(); }
		else { Debug.Log("Unknown button: " + btn.name); }
	}
}