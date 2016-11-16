using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Cabin : MonoBehaviour, ButtonHolder {

	[HideInInspector]
	public ScanningScreen scanningScreen;

	private Button restBtn, stasisBtn;

	public StatusScreen statusScreen { get; private set; }

	public StasisChambersHolder chambersHolder { get; private set; }

	public Cabin init (StatusScreen statusScreen) {
		this.statusScreen = statusScreen;
		restBtn = transform.Find("Rest Button").GetComponent<Button>().init();
		stasisBtn = transform.Find("Stasis Button").GetComponent<Button>().init();

		chambersHolder = transform.Find("Chambers").GetComponent<StasisChambersHolder>().init(this);

		return this;
	}

	public void fireClickButton (Button btn) {
		if (btn == restBtn) { rest(); }
		else if (btn == stasisBtn) { showStasisChambers(); }
	}

	private void rest () {
		Player.setHealthToMax();
		if (Vars.planetType.isPopulated()) { scanningScreen.resetMarkers(); }
	}

	private void showStasisChambers () {
		restBtn.setVisible(false);
		stasisBtn.setVisible(false);
		statusScreen.setButtonsVisible(false);
		chambersHolder.show();
	}

	public void closeStasisChambers () {
		restBtn.setVisible(true);
		stasisBtn.setVisible(true);
		statusScreen.setButtonsVisible(true);
	}

	public void sendToVars () {
		chambersHolder.sendToVars();
	}

	public void initFromVars () {
		chambersHolder.initFromVars();
	}
}