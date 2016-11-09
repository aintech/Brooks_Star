using UnityEngine;
using System.Collections;

public class EnemyBlock : MonoBehaviour, ButtonHolder {

	public int index;

	private SpriteRenderer portrait;

	private StrokeText enemyName;

	private TextMesh fightResult;

	private Button attackBtn;

	public EnemyMarker marker { get; private set; }

	private ScanningScreen scanningScreen;

	public EnemyBlock init (ScanningScreen scanningScreen) {
		this.scanningScreen = scanningScreen;
		portrait = transform.Find("Portrait").GetComponent<SpriteRenderer>();
		enemyName = transform.Find("Name").GetComponent<StrokeText>().init("default", 3);
		attackBtn = transform.Find("Attack Button").GetComponent<Button>().init();
		fightResult = transform.Find("Fight Result Text").GetComponent<TextMesh>();

		MeshRenderer mesh = fightResult.GetComponent<MeshRenderer>();
		mesh.sortingOrder = portrait.sortingOrder;
		mesh.sortingLayerName = portrait.sortingLayerName;
		fightResult.gameObject.SetActive(false);

		gameObject.SetActive(false);
		return this;
	}

	public void setVisible (EnemyMarker marker) {
		this.marker = marker;
		portrait.sprite = ImagesProvider.getMarkerSprite(marker.enemyType);
		enemyName.setText(marker.enemyType.getName());
		gameObject.SetActive(true);
	}

	public void fireClickButton (Button btn) {
		if (btn == attackBtn) { attackEnemy(); }
	}

	private void attackEnemy () {
		scanningScreen.startFight(this);
	}

	public void setFightingResultWin () {
		fightResult.gameObject.SetActive(true);
		attackBtn.setVisible(false);
		marker.closeMarker();
	}
}