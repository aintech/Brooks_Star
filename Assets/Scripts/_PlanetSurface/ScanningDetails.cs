using UnityEngine;
using System.Collections;

public class ScanningDetails : MonoBehaviour, ButtonHolder, Closeable {

	private SpriteRenderer enemyImage;

	private StrokeText enemyName;

	private Button attackBtn, retreatBtn;

	private ScanningScreen scanningScreen;

	private EnemyType enemyType;

	public ScanningDetails init (ScanningScreen scanningScreen) {
		this.scanningScreen = scanningScreen;

		enemyImage = transform.Find("Enemy Image").GetComponent<SpriteRenderer>();
		enemyName = transform.Find("Name").GetComponent<StrokeText>().init("default", 5);
		attackBtn = transform.Find("Attack Button").GetComponent<Button>().init();
		retreatBtn = transform.Find("Retreat Button").GetComponent<Button>().init();

		gameObject.SetActive(false);

		return this;
	}

	public void fireClickButton (Button btn) {
		if (btn == attackBtn) { attack(); }
		else if (btn == retreatBtn) { close(false); }
	}

	private void attack () {
		close(false);
		scanningScreen.startFight(enemyType);
	}

	public void showDetails (EnemyType enemyType) {
		this.enemyType = enemyType;
		enemyName.setText(enemyType.name());
		scanningScreen.isActive = false;
		enemyImage.sprite = Imager.getEnemy(enemyType, 1);
		InputProcessor.add(this);
		gameObject.SetActive(true);
	}

	public void close (bool byInputProcessor) {
		scanningScreen.isActive = true;
		gameObject.SetActive(false);
		if (!byInputProcessor) { InputProcessor.removeLast(); }
	}
}
