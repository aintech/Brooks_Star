using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class AnnouncementScreen : MonoBehaviour, ButtonHolder, Closeable {

	private PlanetSurface planetSurface;

	private Button closeBtn, applyBtn;

	private EnemyType enemyType;

	private StrokeText nameText, rewardText, planetText;

	public AnnouncementScreen init (PlanetSurface planetSurface, Cabin cabin) {
		this.planetSurface = planetSurface;

		cabin.announcementScreen = this;

		closeBtn = transform.Find("Close Button").GetComponent<Button>().init();
		applyBtn = transform.Find("Apply Button").GetComponent<Button>().init();
		closeBtn.gameObject.SetActive(true);

		nameText = transform.Find("Name").GetComponent<StrokeText>().init("default", 5);
		rewardText = transform.Find("Reward").GetComponent<StrokeText>().init("default", 5);
		planetText = transform.Find("Planet").GetComponent<StrokeText>().init("default", 5);

		randomizeAnnouncement();

		gameObject.SetActive(false);

		return this;
	}

	public void showScreen () {
		UserInterface.showInterface = false;
		InputProcessor.add(this);
		gameObject.SetActive(true);
	}

	public void randomizeAnnouncement () {
		enemyType = (EnemyType)UnityEngine.Random.Range(0, Enum.GetValues(typeof(EnemyType)).Length);

	}

	public void fireClickButton (Button btn) {
		if (btn == applyBtn) { applyAnnouncement(); }
		if (btn == closeBtn) { close(false); }
	}

	private void applyAnnouncement () {
		
	}

	public void close (bool byInputProcessor) {
		gameObject.SetActive(false);
		planetSurface.setVisible(true);
		UserInterface.showInterface = true;
		if (!byInputProcessor) { InputProcessor.removeLast(); }
	}
}