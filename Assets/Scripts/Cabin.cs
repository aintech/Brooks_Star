using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Cabin : MonoBehaviour, ButtonHolder {

	[HideInInspector]
	public ScanningScreen scanningScreen;

	[HideInInspector]
	public AnnouncementScreen announcementScreen;

	private Button terminalBtn, restBtn, stasisBtn;

	public StatusScreen statusScreen { get; private set; }

	public StasisChambersHolder chambersHolder { get; private set; }

	private PlayerTerminal terminal;

	public Cabin init (StatusScreen statusScreen) {
		this.statusScreen = statusScreen;

		terminalBtn = transform.Find("Terminal Button").GetComponent<Button>().init();
		restBtn = transform.Find("Rest Button").GetComponent<Button>().init();
		stasisBtn = transform.Find("Stasis Button").GetComponent<Button>().init();

		terminal = transform.Find("Player Terminal").GetComponent<PlayerTerminal>().init(this);
		chambersHolder = transform.Find("Chambers").GetComponent<StasisChambersHolder>().init(this);

		return this;
	}

	public void fireClickButton (Button btn) {
		if (btn == restBtn) { rest(); }
		else if (btn == stasisBtn) { showStasisChambers(); }
		else if (btn == terminalBtn) { showTerminal(); }
	}

	private void rest () {
		Player.setHealthToMax();
		if (Vars.planetType.isPopulated()) { scanningScreen.resetMarkers(); }
//		if (Vars.planetType.isColonized()) { announcementScreen.randomizeAnnouncements(); }
	}

	public void setButtonVisible (bool visible) {
		terminalBtn.setVisible(visible);
		restBtn.setVisible(visible);
		stasisBtn.setVisible(visible);
		statusScreen.setButtonsVisible(visible);
	}

	private void showTerminal () {
		setButtonVisible(false);
		terminal.show();
	}

	private void showStasisChambers () {
		setButtonVisible(false);
		chambersHolder.show();
	}

	public void sendToVars () {
		chambersHolder.sendToVars();
	}

	public void initFromVars () {
		chambersHolder.initFromVars();
	}
}