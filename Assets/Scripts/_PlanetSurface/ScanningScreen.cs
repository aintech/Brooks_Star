using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class ScanningScreen : MonoBehaviour, ButtonHolder {

	public Transform enemyMarkerPrefab;

	public const float FIELD_RADIUS = 4.5f;

	private ExploreScreen exploreScreen;

	private Button closeBtn;

	private GameObject[] barBlocks;

	private Transform cursor;

	private Vector2 cursorPos;

	private List<EnemyMarker> markers = new List<EnemyMarker>();

	private float ableDistance = 1, distance, tempFloat;

	private List<EnemyType> enemyTypes = new List<EnemyType>();

	private float revealDist = .1f;

	private Transform markersHolder;

	private Vector3 holderCenter;

//	private EnemyBlock[] enemyBlocks;
//
//	private int revealBlockIndex = 0;
//
//	private EnemyBlock enemyFightBlock;

	private FightScreen fightScreen;

	private int markersOnStage = 5;

	private bool active;
	public bool isActive { get { return active; } set { closeBtn.setVisible(value); active = value; }}

	private ScanningDetails scanningDetails;

	public ScanningScreen init (ExploreScreen exploreScreen, StatusScreen statusScreen, ItemDescriptor itemDescriptor) {
		this.exploreScreen = exploreScreen;

		fightScreen = GameObject.Find("Fight Screen").GetComponent<FightScreen>().init(this, statusScreen, itemDescriptor);

		scanningDetails = transform.Find("Scanning Details").GetComponent<ScanningDetails>().init(this);

		statusScreen.cabin.scanningScreen = this;

		cursor = transform.Find("Cursor").transform;
		cursor.gameObject.SetActive(true);
		closeBtn = transform.Find("Close Button").GetComponent<Button>().init();
		transform.Find("Background").gameObject.SetActive(true);

		markersHolder = transform.Find("Markers Holder");
		holderCenter = markersHolder.localPosition;

		Transform barsHolder = transform.Find("Bars Holder");
		barBlocks = new GameObject[barsHolder.childCount];
		char[] separ = new char[]{' '};
		int pos;
		for (int i = 0; i < barsHolder.childCount; i++) {
			pos = int.Parse(barsHolder.GetChild(i).name.Split(separ, int.MaxValue)[2]);
			barBlocks[i] = barsHolder.GetChild(i).gameObject;
		}
		barsHolder.gameObject.SetActive(true);

//		Transform blockHolder = transform.Find("Enemy Blocks");
//		enemyBlocks = new EnemyBlock[blockHolder.childCount];
//		EnemyBlock block;
//		for (int i = 0; i < blockHolder.childCount; i++) {
//			block = blockHolder.GetChild(i).GetComponent<EnemyBlock>().init(this);
//			enemyBlocks[block.index] = block;
//		}
//		blockHolder.gameObject.SetActive(true);
		markersHolder.gameObject.SetActive(true);

		foreach (EnemyType eType in Enum.GetValues(typeof(EnemyType))) {
			if (eType.planet() == Vars.planetType) {
				enemyTypes.Add(eType);
			}
		}

		isActive = true;

		close();
		return this;
	}

	void Update () {
		if (!isActive) { return; }
		tempFloat = Vector3.Distance(holderCenter, Utils.mousePos);
		if (tempFloat < FIELD_RADIUS) {
			if (!cursor.gameObject.activeInHierarchy) {
				cursor.gameObject.SetActive(true);
			}
			cursor.localPosition = Utils.mousePos;
			findNearestTarget();
			if (Input.GetMouseButtonDown(0)) {
				tryRevealEnemy();
			}
		} else if (cursor.gameObject.activeInHierarchy) {
			cursor.gameObject.SetActive(false);
			for (int i = 0; i < barBlocks.Length; i++) {
				if (barBlocks[i].activeInHierarchy) { barBlocks[i].SetActive(false); }
			}
		}
	}

	private void addMarker (EnemyType enemyType) {
		markers.Add(Instantiate<Transform>(enemyMarkerPrefab).GetComponent<EnemyMarker>().init(enemyType, markersHolder));
	}

	public void resetMarkers () {
		foreach (EnemyMarker marker in markers) {
			marker.resetMarker(enemyTypes[UnityEngine.Random.Range(0, enemyTypes.Count)]);
		}
//		foreach(EnemyBlock block in enemyBlocks) {
//			block.hide();
//		}
//		revealBlockIndex = 0;
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
				scanningDetails.showDetails(mark.enemyType);
				cursor.gameObject.SetActive(false);
				mark.resetMarker(enemyTypes[UnityEngine.Random.Range(0, enemyTypes.Count)]);
//				mark.revealMarker();
//				enemyBlocks[revealBlockIndex].setVisible(mark);
//				revealBlockIndex++;
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
		if (markers.Count == 0) {
			for (int i = 0; i < markersOnStage; i++) {
				addMarker(enemyTypes[UnityEngine.Random.Range(0, enemyTypes.Count)]);
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

	public void startFight (EnemyType enemyType) {
		gameObject.SetActive(false);
//		enemyFightBlock = enemyBlock;
		fightScreen.startFight(enemyType);
	}

	public void endFight (bool win) {
//		if (win) { enemyFightBlock.setFightingResultWin(); }
		gameObject.SetActive(true);
	}
}