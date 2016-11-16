using UnityEngine;
using System.Collections;

public class FightResultScreen : MonoBehaviour, ButtonHolder {

//	private Location location;

	private SpriteRenderer render;

	private Transform bg, valuesHolder;

	private Vector3 bgScale, initBgScale = new Vector3(.2f, .2f, 1);

	private float bgScaleFactor = .05f;

//	private Sprite winSprite;

	private bool playAnim, countGoldDone;//, countRankDone;

	private TextMesh goldValue, newLevelLabel;//rankPointsValue, 

//	private int rankPoints, rankPointsCounter;

//	private int gold, goldCounter;//, heroRank;

	private FightScreen fightScreen;

//	private PotionBag potionBag;

	private StrokeText chambersAvailable, clickText;

	private Button captureBtn, releaseBtn;

	private StasisChambersHolder chambersHolder;

	private Enemy enemy;

	public FightResultScreen init (FightScreen fightScreen, StasisChambersHolder chambersHolder, Enemy enemy) {
		this.fightScreen = fightScreen;
		this.chambersHolder = chambersHolder;
		this.enemy = enemy;

		render = transform.Find("Enemy Image").GetComponent<SpriteRenderer>();
		bg = transform.Find("BG");
		chambersAvailable = transform.Find("Chambers Available").GetComponent<StrokeText>().init("FightResultScreen", 2);

		captureBtn = transform.Find("Capture Button").GetComponent<Button>().init();
		releaseBtn = transform.Find("Release Button").GetComponent<Button>().init();

//		this.potionBag = potionBag;
//		location = GameObject.FindGameObjectWithTag("LocationScreen").GetComponent<Location>();
//		valuesHolder = transform.Find("ValuesHolder");
//		rankPointsValue = valuesHolder.Find("RankPointsValue").GetComponent<TextMesh>();
//		goldValue = valuesHolder.Find("GoldValue").GetComponent<TextMesh>();
//		newLevelLabel = valuesHolder.Find("NewLevelLabel").GetComponent<TextMesh>();

//		MeshRenderer mesh = valuesHolder.Find("RankPointsLabel").GetComponent<MeshRenderer>();
//		mesh.sortingLayerName = "FightResultLayer";
//		mesh.sortingOrder = 2;
//		MeshRenderer mesh = valuesHolder.Find("GoldLabel").GetComponent<MeshRenderer>();
//		mesh.sortingLayerName = "FightResultLayer";
//		mesh.sortingOrder = 2;
//		mesh = rankPointsValue.GetComponent<MeshRenderer>();
//		mesh.sortingLayerName = "FightResultLayer";
//		mesh.sortingOrder = 2;
//		mesh = goldValue.GetComponent<MeshRenderer>();
//		mesh.sortingLayerName = "FightResultLayer";
//		mesh.sortingOrder = 2;
//		mesh = newLevelLabel.GetComponent<MeshRenderer>();
//		mesh.sortingLayerName = "FightResultLayer";
//		mesh.sortingOrder = 2;

//		newLevelLabel.gameObject.SetActive(false);
//		valuesHolder.gameObject.SetActive(false);

		clickText = transform.Find("Click Text").GetComponent<StrokeText>().init("FightResultScreen", 10);
		gameObject.SetActive(false);

		return this;
	}

	public void fireClickButton (Button btn) {
		if (playAnim) { return; }
		if (btn == captureBtn) { captureEnemy(); }
		else if (btn == releaseBtn) { closeScreen(); }
	}

	private void captureEnemy () {
		foreach (StasisChamber chamber in chambersHolder.chambers) {
			if (chamber.isEmpty) { chamber.putInChamber(enemy.enemyType); break; }
		}
		closeScreen();
	}

	public void showFightResultScreen (Enemy enemy) {
//		this.winSprite = enemy == null? null: enemy.getRandomWinSprite();
//		this.rankPoints = enemy == null? 0: enemy.getEnemyType().getRankPoints();
//		this.rankPoints = rankPoints;
//		this.gold = enemy == null? 0: enemy.getEnemyType().getMoney();
//		if (Quest.currentQuest != null && Quest.currentQuest.enemyType == enemy.getEnemyType()) {
//			Quest.currentQuest.done = true;
//			UserInterface.showQuestInfo(Quest.currentQuest.title + " (done)");
//		}
		render.enabled = false;

//		rankPointsCounter = 0;
//		goldCounter = 0;
		bgScale = initBgScale;
		bg.localScale = bgScale;
		playAnim = true;
		render.sprite = Imager.getEnemy(enemy.enemyType, 0);
		captureBtn.setVisible(false);
		releaseBtn.setVisible(false);
		chambersAvailable.setText("");
//		countRankDone = (this.rankPoints > 0);
		countGoldDone = false;
//		heroRank = Hero.getRank();
//		valuesHolder.gameObject.SetActive(false);
		gameObject.SetActive(true);
//		location.setBackgroundMove(false);
	}

	void Update () {
		if (playAnim) {
			bgScale.x += bgScaleFactor;
			bgScale.y += bgScaleFactor;
			if (bgScale.x >= 1) {
				bgScale.x = bgScale.y = 1;
				playAnim = false;
				render.enabled = true;
				captureBtn.setVisible(true);
				releaseBtn.setVisible(true);
				int chamAvail = 0;
				foreach (StasisChamber cham in chambersHolder.chambers) {
					if (cham.isEmpty) { chamAvail++; }
				}
				captureBtn.setActive(chamAvail > 0);
				chambersAvailable.setText(chamAvail == 0? "Нет свободных стазис камер": "");
//				goldValue.text = "0";
//				rankPointsValue.text = "0";
//				valuesHolder.gameObject.SetActive(true);
			}
			bg.localScale = bgScale;
		}
//		else {
////			if (!countRankDone) {
////				rankPointsCounter++;
////				if (rankPointsCounter <= rankPoints) {
////					Hero.addRankPoints(1);
////					rankPointsValue.text = "+" + rankPointsCounter;
////					if (Hero.getRank() > heroRank) {
////						heroRank = Hero.getRank();
////						newLevelLabel.gameObject.SetActive(true);
////					}
////				} else {
////					countRankDone = true;
////				}
////			} else 
//			if (!countGoldDone) {
////				goldCounter++;
////				if (goldCounter <= gold) {
//////					Vars.gold++;
//////					UserInterface.updateGold();
////					goldValue.text = "+" + goldCounter;
////				} else {
//					countGoldDone = true;
////				}
//			}
//		}

//		if (!playAnim) && Input.GetMouseButtonDown(0)) {
//			if (!countRankDone) {
//				Hero.addRankPoints(rankPoints - rankPointsCounter);
//				rankPointsCounter = rankPoints;
//				rankPointsValue.text = "+" + rankPoints;
//				if (Hero.getRank() > heroRank) {
//					heroRank = Hero.getRank();
//					newLevelLabel.gameObject.SetActive(true);
//				}
//				countRankDone = true;
//			} else 
//			if (!countGoldDone) {
//				Vars.gold += (gold - goldCounter);
//				goldCounter = gold;
//				goldValue.text = "+" + gold;
//				UserInterface.updateGold();
//			} else if (render.sprite == null && winSprite != null) {
//				render.sprite = winSprite;
//			} else {
//				closeScreen();
//			}
//		}
	}

	private void closeScreen () {
		fightScreen.closeFightScreen();
//		potionBag.hideBag();
//        newLevelLabel.gameObject.SetActive(false);
        gameObject.SetActive(false);
//		location.showLocation();
    }
}