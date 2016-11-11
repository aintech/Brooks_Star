using UnityEngine;
using System.Collections;

public class StatusEffect : MonoBehaviour {

	public StatusEffectType statusType;

	public bool asPlayer;

	public int value { get; private set; }

	public int duration { get; private set; }

	private StrokeText turnsText;

	private Enemy enemy;

	public bool inProgress { get; private set; }

	public StatusEffect init () {
		turnsText = transform.Find("Turns").GetComponent<StrokeText>().init("default", 5);
		gameObject.SetActive(false);

		return this;
	}

	public void initEnemy (Enemy enemy) {
		this.enemy = enemy;
	}

	public void addStatus (int value, int duration) {
		this.value = value;
		this.duration = duration;
		turnsText.setText (duration.ToString ());
		turnsText.gameObject.SetActive (true);
		gameObject.SetActive(true);
		inProgress = true;
	}

	public void updateStatus () {
		if (!inProgress) {
			return;
		}
		duration--;
		if (duration >= 0) {
			applyEffect ();
		}
		if (duration == 1) {
			turnsText.gameObject.SetActive (false);
		} else if (duration == 0) {
			endEffect ();
		}
		turnsText.setText (duration.ToString ());
	}

	private void applyEffect () {
		switch (statusType) {
			case StatusEffectType.REGENERATION:
				if (asPlayer) {
					Player.heal (value);
				} else {
					enemy.heal (value);
				}
				break;
		}
	}

	private void endEffect () {
		inProgress = false;
		gameObject.SetActive (false);
	}
}