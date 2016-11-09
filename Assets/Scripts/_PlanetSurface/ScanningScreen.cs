using UnityEngine;
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

	private EnemyType[] enemyTypes;

	private float revealDist = .1f;

	private Transform markersHolder;

	private Vector3 holderCenter;

	private EnemyBlock[] enemyBlocks;

	private int revealBlockIndex = 0;

	private EnemyBlock enemyFightBlock;

	private FightScreen fightScreen;

	public ScanningScreen init (ExploreScreen exploreScreen) {
		this.exploreScreen = exploreScreen;

		fightScreen = GameObject.Find("Fight Screen").GetComponent<FightScreen>().init(this);

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

		Transform blockHolder = transform.Find("Enemy Blocks");
		enemyBlocks = new EnemyBlock[blockHolder.childCount];
		EnemyBlock block;
		for (int i = 0; i < blockHolder.childCount; i++) {
			block = blockHolder.GetChild(i).GetComponent<EnemyBlock>().init(this);
			enemyBlocks[block.index] = block;
		}
		blockHolder.gameObject.SetActive(true);
		markersHolder.gameObject.SetActive(true);

		close();
		return this;
	}

	void Update () {
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
		}
	}

	private void addMarker (EnemyType enemyType) {
		markers.Add(Instantiate<Transform>(enemyMarkerPrefab).GetComponent<EnemyMarker>().init(enemyType, markersHolder));
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
				enemyBlocks[revealBlockIndex].setVisible(mark);
				revealBlockIndex++;
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
			for (int i = 0; i < enemyBlocks.Length; i++) {
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

	public void startFight (EnemyBlock enemyBlock) {
		gameObject.SetActive(false);
		enemyFightBlock = enemyBlock;
		fightScreen.startFight(enemyBlock.marker.enemyType);
	}

	public void endFight (bool win) {
		if (win) { enemyFightBlock.setFightingResultWin(); }
		gameObject.SetActive(true);
	}
}