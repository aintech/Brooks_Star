using UnityEngine;
using System.Collections;

public class UserInterface : MonoBehaviour, ButtonHolder {

	private Button statusBtn;

	private StrokeText cashText;

	public UserInterface init () {
		statusBtn = transform.Find("Status Button").GetComponent<Button>().init();
		cashText = transform.Find("Cash Text").GetComponent<StrokeText>().init("User Interface", 1);
		updateCash();
		return this;
	}

	public void setEnabled (bool enabled) {
		gameObject.SetActive(enabled);
	}

	public void updateCash () {
		cashText.setText(Vars.cash.ToString());
	}

	public void fireClickButton (Button btn) {
		
	}

    public void showPlanetInfo (PlanetType type) {
        Debug.Log("Ship near " + type.getName());
    }

    public void hidePlanetInfo () {
        Debug.Log("Leave Planet!");//CONTINUE: UserInterface to GUI
    }
}