using UnityEngine;
using System.Collections;

public class UserInterface : MonoBehaviour, ButtonHolder {

	private Button statusBtn;

	public UserInterface init () {
		statusBtn = transform.Find("Status Button").GetComponent<Button>().init();
		return this;
	}

	public void setEnabled (bool enabled) {
		gameObject.SetActive(enabled);
	}

	public void fireClickButton (Button btn) {
		
	}
}