using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ScanningScreen : MonoBehaviour, ButtonHolder {

	public Transform enemyMarkerPrefab;

	private ExploreScreen exploreScreen;

	private Button closeBtn;

	private GameObject[] barBlocks;

	private Transform cursor;

	private Vector2 cursorPos;

	private List<EnemyMarker> markers = new List<EnemyMarker>();

	private int markersCount = 5;

	private float ableDistance = 1, distance, tempFloat;

	private EnemyType[] enemyTypes;

	private float revealDist = .1f;

	public ScanningScreen init (ExploreScreen exploreScreen) {
		this.exploreScreen = exploreScreen;

		cursor = transform.Find("Cursor").transform;
		cursor.gameObject.SetActive(true);
		closeBtn = transform.Find("Close Button").GetComponent<Button>().init();
		transform.Find("Background").gameObject.SetActive(true);


		Transform barsHolder = transform.Find("Bars Holder");
		barBlocks = new GameObject[barsHolder.childCount];
		char[] separ = new char[]{' '};
		int pos;
		for (int i = 0; i < barsHolder.childCount; i++) {
			pos = int.Parse(barsHolder.GetChild(i).name.Split(separ, int.MaxValue)[2]);
			barBlocks[i] = barsHolder.GetChild(i).gameObject;
		}
		barsHolder.gameObject.SetActive(true);

		close();
		return this;
	}

	void Update () {
		cursor.localPosition = Utils.mousePos;
		findNearestTarget();
		if (Input.GetMouseButtonDown(0)) {
			tryRevealEnemy();
		}
	}

	private void addMarker (EnemyType enemyType) {
		markers.Add(Instantiate<Transform>(enemyMarkerPrefab).GetComponent<EnemyMarker>().init(enemyType, this));
	}

	private void findNearestTarget () {
		distance = 1000000;
		foreach (EnemyMarker mark in markers) {
			if (mark.isFound) { continue; }
			tempFloat = Vector2.Distance(mark.trans.position, Utils.mousePos);
			if (tempFloat < distance) { distance = tempFloat; }
		}
		arrangeBlocks(distance < ableDistance?  (distance / ableDistance): 0);
	}

	private void tryRevealEnemy () {
		foreach (EnemyMarker mark in markers) {
			if (mark.isFound) { continue; }
			tempFloat = Vector2.Distance(mark.trans.position, Utils.mousePos);
			if (tempFloat <= revealDist) {
				mark.revealMarker();
				return;
			}
		}
	}

	private void arrangeBlocks (float value) {
		if (value == 0) {
			for (int i = 0; i < barBlocks.Length; i++) {
				if (barBlocks[i].activeInHierarchy) { barBlocks[i].SetActive(false); }
			}
		} else if (value < .05f) {
			for (int i = 0; i < barBlocks.Length; i++) {
				if (barBlocks[i].activeInHierarchy) { barBlocks[i].SetActive(true); }
			}
		} else {
			int activeBlocks = barBlocks.Length - Mathf.RoundToInt(value * barBlocks.Length);
			for (int i = 0; i < barBlocks.Length; i++) {
				barBlocks[i].SetActive(i <= activeBlocks);
			}
		}
	}

	public void show () {
		gameObject.SetActive(true);
		UserInterface.showInterface = false;
		enemyTypes = Vars.planetType.getEnemyTypes();
		if (markers.Count == 0) {
			for (int i = 0; i < markersCount; i++) {
				addMarker(enemyTypes[Random.Range(0, enemyTypes.Length)]);
			}
		}
	}

	public void close () {
		gameObject.SetActive(false);
		exploreScreen.setVisible(true);
		UserInterface.showInterface = true;
	}

	public void fireClickButton (Button btn) {
		if (btn == closeBtn) { close(); }
	}
}