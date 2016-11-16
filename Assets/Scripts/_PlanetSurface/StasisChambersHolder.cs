using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StasisChambersHolder : MonoBehaviour, Closeable, ButtonHolder {

	public StasisChamber[] chambers { get; private set; }

	public Cabin cabin { get; private set; }

	private Button closeBtn;

	public StasisChambersHolder init (Cabin cabin) {
		this.cabin = cabin;
		StasisChamber cham;
		chambers = new StasisChamber[transform.childCount - 1];//За исключением кнопки возврата
		for (int i = 0; i < transform.childCount; i++) {
			cham = transform.GetChild(i).GetComponent<StasisChamber>();
			if (cham != null) {
				chambers[cham.index] = cham.init(this);
			}
		}

		closeBtn = transform.Find("Close Button").GetComponent<Button>().init();
		closeBtn.setVisible(false);

		return this;
	}

	public void fireClickButton (Button btn) {
		if (btn == closeBtn) { close(false); }
	}

	public void show () {
		for (int i = 0; i < chambers.Length; i++) {
			chambers[i].gameObject.SetActive(true);
		}
		closeBtn.setVisible(true);
		InputProcessor.add(this);
	}

	public void close (bool byInputProcessor) {
		for (int i = 0; i < chambers.Length; i++) {
			chambers[i].gameObject.SetActive(false);
		}
		closeBtn.setVisible(false);
		cabin.closeStasisChambers();
		if (!byInputProcessor) { InputProcessor.removeLast(); }
	}

	public void sendToVars () {
		Vars.capturedEnemies.Clear();
		for (int i = 0; i < chambers.Length; i++) {
			if (!chambers[i].isEmpty) { Vars.capturedEnemies.Add(chambers[i].index, chambers[i].enemyType); }
		}
	}

	public void initFromVars () {
		foreach (KeyValuePair<int, EnemyType> pair in Vars.capturedEnemies) {
			chambers[pair.Key].putInChamber(pair.Value);
		}
		Vars.capturedEnemies.Clear();
	}
}