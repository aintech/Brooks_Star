using UnityEngine;
using System.Collections;

public class Cabin : MonoBehaviour, ButtonHolder {

	private Button restBtn;

	public Cabin init () {
		restBtn = transform.Find("Rest Button").GetComponent<Button>().init();
		return this;
	}

	public void fireClickButton (Button btn) {
		if (btn == restBtn) { rest(); }
	}

	private void rest () {
		
	}
}