using UnityEngine;
using System.Collections;

public class IndustrialScreen : MonoBehaviour, ButtonHolder, Hideable {

	private PlanetSurface planetSurface;

	private Button laboratoryBtn, workshopBtn, factoryshopBtn, closeBtn;

	public void init (PlanetSurface planetSurface) {
		this.planetSurface = planetSurface;

		laboratoryBtn = transform.Find("Laboratory Button").GetComponent<Button>().init();
		workshopBtn = transform.Find("Workshop Button").GetComponent<Button>().init();
		factoryshopBtn = transform.Find("Factoryshop Button").GetComponent<Button>().init();
		closeBtn = transform.Find("Close Button").GetComponent<Button>().init();

		gameObject.SetActive(false);
	}

	public void showScreen () {
		PlanetSurface.topHideable = this;
		gameObject.SetActive(true);
	}

	private void closeScreen () {
		gameObject.SetActive(false);
		planetSurface.setVisible(true);
	}

	public void fireClickButton (Button btn) {
		if (btn == laboratoryBtn) { showShop(ShopType.LABORATORY); }
		else if (btn == workshopBtn) { showShop(ShopType.WORKSHOP); }
		else if (btn == factoryshopBtn) { showShop(ShopType.FACTORYSHOP); }
		else if (btn == closeBtn) { closeScreen(); }
		else { Debug.Log("Unknown button: " + btn.name); }
	}

	private void showShop (ShopType type) {
		
	}

	public void setVisible (bool visible) {
		gameObject.SetActive(visible);
	}

	private enum ShopType {
		LABORATORY, WORKSHOP, FACTORYSHOP
	}
}