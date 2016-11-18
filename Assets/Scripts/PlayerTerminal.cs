using UnityEngine;
using System.Collections;

public class PlayerTerminal : MonoBehaviour, ButtonHolder, Closeable {

	private Cabin cabin;

	private Button closeBtn;

	public PlayerTerminal init (Cabin cabin) {
		this.cabin = cabin;

		closeBtn = transform.Find("Close Button").GetComponent<Button>().init();
		closeBtn.setVisible(false);
		gameObject.SetActive(true);

		return this;
	}

	public void show () {
		closeBtn.setVisible(true);
		InputProcessor.add(this);
	}

	public void fireClickButton (Button btn) {
		if (btn == closeBtn) { close(false); }
	}

	public void close (bool byInputProcessor) {
		closeBtn.setVisible(false);
		cabin.setButtonVisible(true);
		if (!byInputProcessor) { InputProcessor.removeLast(); }
	}
}