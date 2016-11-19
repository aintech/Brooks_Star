using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class AnnouncementScreen : MonoBehaviour, ButtonHolder, Closeable {

	private PlanetSurface planetSurface;

	private Button closeBtn, applyBtn, previousBtn, nextBtn;

	private int index;

	private EnemyType[] enemyTypes;

	private StrokeText nameText, rewardText, planetOfLivingText, destinationPlanetText, appliedText;

	private SpriteRenderer enemyImage;

	public AnnouncementScreen init (PlanetSurface planetSurface, Cabin cabin) {
		this.planetSurface = planetSurface;

		cabin.announcementScreen = this;

		enemyImage = transform.Find ("Enemy Image").GetComponent<SpriteRenderer> ();

		closeBtn = transform.Find("Close Button").GetComponent<Button>().init();
		applyBtn = transform.Find("Apply Button").GetComponent<Button>().init();
		previousBtn = transform.Find ("Previous Button").GetComponent<Button> ().init ();
		nextBtn = transform.Find ("Next Button").GetComponent<Button> ().init ();

		nameText = transform.Find("Name").GetComponent<StrokeText>().init("default", 5);
		rewardText = transform.Find("Reward").GetComponent<StrokeText>().init("default", 5);
		planetOfLivingText = transform.Find("Planet of Living").GetComponent<StrokeText>().init("default", 5);
		destinationPlanetText = transform.Find ("Destination Planet").GetComponent<StrokeText> ().init ("default", 5);
		appliedText = transform.Find("Applied").GetComponent<StrokeText>().init("default", 5);

		applyBtn.gameObject.SetActive (true);
		closeBtn.gameObject.SetActive(true);
		previousBtn.gameObject.SetActive (true);
		nextBtn.gameObject.SetActive (true);
		transform.Find ("Background").gameObject.SetActive (true);
		transform.Find ("Foreground").gameObject.SetActive (true);
		transform.Find ("Enemy Image").gameObject.SetActive (true);
		nameText.gameObject.SetActive (true);
		rewardText.gameObject.SetActive (true);
		planetOfLivingText.gameObject.SetActive (true);
		destinationPlanetText.gameObject.SetActive (true);

		randomizeAnnouncements();

		gameObject.SetActive(false);

		return this;
	}

	public void showScreen () {
		UserInterface.showInterface = false;
		InputProcessor.add(this);
		checkButtonsVisibility ();
		gameObject.SetActive(true);
	}

	public void randomizeAnnouncements () {
		enemyTypes = new EnemyType[1];//new EnemyType[UnityEngine.Random.Range (1, 4)];
		for (int i = 0; i < enemyTypes.Length; i++) {
			enemyTypes[i] = (EnemyType)UnityEngine.Random.Range(0, Enum.GetValues(typeof(EnemyType)).Length);
		}
		index = 0;
		updateAnnnouncementInfo ();
		checkButtonsVisibility ();
		appliedText.gameObject.SetActive (false);
		applyBtn.setVisible (true);
	}

	public void fireClickButton (Button btn) {
		if (btn == previousBtn) {
			switchAnnouncement (true);
		} else if (btn == nextBtn) {
			switchAnnouncement (false);
		}
		if (btn == applyBtn) { applyAnnouncement(); }
		if (btn == closeBtn) { close(false); }
	}

	private void applyAnnouncement () {
		Vars.bounties.Add (new Bounty (enemyTypes [index]));
		applyBtn.setVisible (false);
		appliedText.gameObject.SetActive (true);
	}

	private void switchAnnouncement (bool previous) {
		index += previous ? -1 : 1;
		checkButtonsVisibility ();
		updateAnnnouncementInfo ();
	}

	private void updateAnnnouncementInfo () {
		enemyImage.sprite = Imager.getEnemy (enemyTypes [index], 1);
		nameText.setText (enemyTypes [index].name ());
		rewardText.setText ("$" + (enemyTypes [index].cost () * 2));
		planetOfLivingText.setText (enemyTypes [index].planet ().name () + " (" + enemyTypes[index].planet().starSystemType().name() + ")");
		destinationPlanetText.setText (Vars.planetType.name () + " (" + Vars.planetType.starSystemType ().name () + ")");
	}

	private void checkButtonsVisibility () {
		previousBtn.setVisible (index > 0);
		nextBtn.setVisible (index < enemyTypes.Length - 1);
	}

	public void close (bool byInputProcessor) {
		gameObject.SetActive(false);
		planetSurface.setVisible(true);
		UserInterface.showInterface = true;
		if (!byInputProcessor) { InputProcessor.removeLast(); }
	}
}