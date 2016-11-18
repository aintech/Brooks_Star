using UnityEngine;
using System.Collections;

public class StasisChamber : MonoBehaviour, ButtonHolder {

	public Sprite emptyChamber;

	private SpriteRenderer render;

	public int index;

	private Button sellBtn, releaseBtn;

	public EnemyType enemyType { get; private set; }

	public bool isEmpty { get; private set; }

	private TextMesh enemyName;

	private StasisChambersHolder chambersHolder;

	public StasisChamber init (StasisChambersHolder chambersHolder) {
		this.chambersHolder = chambersHolder;
		isEmpty = true;
		render = GetComponent<SpriteRenderer>();
		sellBtn = transform.Find("Sell Button").GetComponent<Button>().init();
		releaseBtn = transform.Find("Release Button").GetComponent<Button>().init();

		enemyName = transform.Find("Name").GetComponent<TextMesh>();
		enemyName.text = "";
		MeshRenderer mesh = enemyName.GetComponent<MeshRenderer>();
		mesh.sortingLayerName = "Inventory";
		mesh.sortingOrder = 2;

		sellBtn.setVisible(false);
		releaseBtn.setVisible(false);
		gameObject.SetActive(false);
		return this;
	}

	public void putInChamber (EnemyType type) {
		this.enemyType = type;
		render.sprite = Vars.EROTIC? ImagesProvider.getEnemyStasisSprite(type): emptyChamber;
		sellBtn.setVisible(true);
		releaseBtn.setVisible(true);
		enemyName.text = type.name();
		sellBtn.setText("Продать\n$" + type.cost());
		isEmpty = false;
	}

	public void fireClickButton (Button btn) {
		if (btn == sellBtn) { sell(); }
		else if (btn == releaseBtn) { clearChamber(); }
	}

	private void sell () {
		Vars.cash += enemyType.cost();
		chambersHolder.cabin.statusScreen.updateCashTxt();
		clearChamber();
	}

	private void clearChamber () {
		enemyName.text = "";
		render.sprite = emptyChamber;
		sellBtn.setVisible(false);
		releaseBtn.setVisible(false);
		isEmpty = true;
	}
}