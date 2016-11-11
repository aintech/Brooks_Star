using UnityEngine;
using System.Collections;

public class StatusEffect : MonoBehaviour {

	public StatusEffectType statusType;

	public int value { get; private set; }

	public int duration { get; private set; }

	private StrokeText turnsText;

	public StatusEffect init () {
		turnsText = transform.Find("Turns").GetComponent<StrokeText>().init("default", 5);
		gameObject.SetActive(false);

		return this;
	}

	public void addStatus (int value, int duration) {
		this.value = value;
		this.duration = duration;
		gameObject.SetActive(true);
	}

	public void updateStatus () {
		
	}
}