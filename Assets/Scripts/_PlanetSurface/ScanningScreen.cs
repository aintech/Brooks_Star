using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ScanningScreen : MonoBehaviour, ButtonHolder {

	private ExploreScreen exploreScreen;

	private Button closeBtn;

	private GameObject[] barBlocks;

	private Transform cursor;

	private Vector2 cursorPos;

	private List<Vector2> targets = new List<Vector2>();

	private float ableDistance = 1, distance, tempFloat;

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
	}

	private void addTargets () {
		targets.Clear();
		targets.Add(Vector2.zero);
	}

	private void findNearestTarget () {
		distance = 1000000;
		foreach (Vector2 targ in targets) {
			tempFloat = Vector2.Distance(targ, Utils.mousePos);
			if (tempFloat < distance) { distance = tempFloat; }
		}
		arrangeBlocks(distance < ableDistance?  (distance / ableDistance): 0);
	}

	private void arrangeBlocks (float value) {
		if (value < .01f) {
			for (int i = 0; i < barBlocks.Length; i++) {
				if (barBlocks[i].activeInHierarchy) { barBlocks[i].SetActive(false); }
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
		addTargets();
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