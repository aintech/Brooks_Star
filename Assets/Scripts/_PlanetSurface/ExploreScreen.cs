﻿using UnityEngine;
using System.Collections;

public class ExploreScreen : MonoBehaviour, ButtonHolder, Hideable {

	private PlanetSurface planetSurface;

	private FightScreen fightScreen;

	private SpriteRenderer enemyImage;

	private Button exploreBtn, leaveBtn;

	private ScanningScreen scanningScreen;

	public ExploreScreen init (PlanetSurface planetSurface) {
		this.planetSurface = planetSurface;
		enemyImage = transform.Find("Enemy Image").GetComponent<SpriteRenderer>();

		exploreBtn = transform.Find("Explore Button").GetComponent<Button>().init();
		leaveBtn = transform.Find("Leave Button").GetComponent<Button>().init();

		scanningScreen = GameObject.Find("Scanning Screen").GetComponent<ScanningScreen>().init(this);

		gameObject.SetActive(false);

		return this;
	}

	public void showScreen () {
		PlanetSurface.topHideable = this;
		gameObject.SetActive(true);
	}

	private void leavePlanet () {
		gameObject.SetActive(false);
		planetSurface.leavePlanet();
	}

	private void startExplore () {
		setVisible(false);
		scanningScreen.show();
	}

	public void fireClickButton (Button btn) {
		if (btn == exploreBtn) { startExplore(); }
		else if (btn == leaveBtn) { leavePlanet(); }
		else { Debug.Log("Unknown button: " + btn.name); }
	}

	public void setVisible (bool visible) {
		exploreBtn.setVisible(visible);
		leaveBtn.setVisible(visible);
	}
}