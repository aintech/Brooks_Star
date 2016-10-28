using UnityEngine;
using System.Collections;

public class ExploreScreen : MonoBehaviour, ButtonHolder, Hideable {

	private PlanetSurface planetSurface;

	private FightScreen fightScreen;

	private SpriteRenderer enemyImage;

	private Button exploreBtn, leaveBtn;//, fightBtn, nextBtn, backBtn;

//	private StrokeText enemyName;
//
//	private EnemyType enemyType;

	private ScanningScreen scanningScreen;

	public ExploreScreen init (PlanetSurface planetSurface) {
		this.planetSurface = planetSurface;
		enemyImage = transform.Find("Enemy Image").GetComponent<SpriteRenderer>();

//		fightBtn = transform.Find("Fight Button").GetComponent<Button>().init();
//		nextBtn = transform.Find("Next Button").GetComponent<Button>().init();
//		backBtn = transform.Find("Back Button").GetComponent<Button>().init();
		exploreBtn = transform.Find("Explore Button").GetComponent<Button>().init();
		leaveBtn = transform.Find("Leave Button").GetComponent<Button>().init();

//		enemyName = transform.Find("Enemy Name").GetComponent<StrokeText>().init("default", 2);

		fightScreen = GameObject.Find("Fight Screen").GetComponent<FightScreen>().init(this);
		scanningScreen = GameObject.Find("Scanning Screen").GetComponent<ScanningScreen>().init(this);

//		fightBtn.gameObject.SetActive(false);
//		nextBtn.gameObject.SetActive(false);
//		backBtn.gameObject.SetActive(false);

		gameObject.SetActive(false);

		return this;
	}

	public void showScreen () {
		PlanetSurface.topHideable = this;
		gameObject.SetActive(true);
	}

	private void leavePlanet () {
		gameObject.SetActive(false);
		planetSurface.leavePlanet();
	}

	private void startExplore () {
//		enemyName.setText("");
//		enemyName.gameObject.SetActive(true);
//		fightBtn.gameObject.SetActive(true);
//		nextBtn.gameObject.SetActive(true);
//		backBtn.gameObject.SetActive(true);
		setVisible(false);
		scanningScreen.show();
//		findEnemy();
	}

	private void findEnemy () {
//		enemyType = Vars.planetType.getEnemyTypes()[Random.Range(0, Vars.planetType.getEnemyTypes().Length)];
//		enemyImage.sprite = Imager.getEnemy(enemyType, 1f);
//		enemyImage.gameObject.SetActive(true);
//		enemyName.setText(enemyType.getName());
	}

	private void startFight () {
//		gameObject.SetActive(false);
//		UserInterface.showInterface = false;
//		fightScreen.startFight(enemyType);
	}

	public void endFight () {
		gameObject.SetActive(true);
		UserInterface.showInterface = true;
		turnBack();
	}

	private void turnBack () {
//		enemyName.gameObject.SetActive(false);
//		enemyImage.gameObject.SetActive(false);
//		fightBtn.gameObject.SetActive(false);
//		nextBtn.gameObject.SetActive(false);
//		backBtn.gameObject.SetActive(false);
//		exploreBtn.gameObject.SetActive(true);
//		leaveBtn.gameObject.SetActive(true);
	}

	public void fireClickButton (Button btn) {
		if (btn == exploreBtn) { startExplore(); }
//		else if (btn == fightBtn) { startFight(); }
//		else if (btn == nextBtn) { findEnemy(); }
//		else if (btn == backBtn) { turnBack(); }
		else if (btn == leaveBtn) { leavePlanet(); }
		else { Debug.Log("Unknown button: " + btn.name); }
	}

	public void setVisible (bool visible) {
		exploreBtn.setVisible(visible);
//		fightBtn.setVisible(visible);
//		nextBtn.setVisible(visible);
//		backBtn.setVisible(visible);
		leaveBtn.setVisible(visible);
	}
}