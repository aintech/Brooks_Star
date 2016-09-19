using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FightScreen : MonoBehaviour {

	public static bool ENEMY_DEAD_ANIM_DONE = false;

	private ElementsHolder elementsHolder;

	private SpriteRenderer iconsHolderRender;

	private Color holderColor = new Color(1, 1, 1, 0);

	private float enemyFinalX = 4.4f, enemyAppearSpeed = .2f;

	private Vector3 enemyPos;

	private FightEffectPlayer fightEffectPlayer;

	private ElementEffectPlayer elementEffectPlayer;

	private FightInterface fightInterface;

	private Enemy enemy;

	private FightResultScreen resultScreen;

	private Animator enemyDeadAnimator;

	private Transform deadStone;

	private Vector2 deadStoneInitPos = new Vector2(0, 2f);

//	private PotionBag potionBag;

	private FightProcessor fightProcessor;

	private ExploreScreen exploreScreen;

	private bool fightStarted, fightOver, startAnimDone, enemyDeadPlaying;

	public FightScreen init (ExploreScreen exploreScreen) {
		this.exploreScreen = exploreScreen;
		elementsHolder = transform.Find("ElementsHolder").GetComponent<ElementsHolder>();
		iconsHolderRender = elementsHolder.GetComponent<SpriteRenderer>();
		fightEffectPlayer = transform.Find("FightEffectPlayer").GetComponent<FightEffectPlayer>().init();
		elementEffectPlayer = transform.Find("ElementEffectPlayer").GetComponent<ElementEffectPlayer>();
		fightInterface = transform.Find("FightInterface").GetComponent<FightInterface>();
		enemy = transform.Find("Enemy").GetComponent<Enemy>();
		resultScreen = transform.Find("FightResultScreen").GetComponent<FightResultScreen>().init(this);
		enemyDeadAnimator = transform.Find("EnemyDeadAnim").GetComponent<Animator>();
		fightProcessor = GetComponent<FightProcessor>();
		deadStone = enemyDeadAnimator.transform.Find("DeadStone");
		enemyPos = enemy.transform.localPosition;
//		potionBag = Vars.gameplay.getPotionBag();
		elementsHolder.init();
		enemy.init();
//		resultScreen.init(this, potionBag);
		fightInterface.init();
		elementEffectPlayer.init(this, enemy);
		fightProcessor.init(this, elementsHolder, enemy);

		elementsHolder.gameObject.SetActive(true);
		fightInterface.gameObject.SetActive(true);
		enemyDeadAnimator.gameObject.SetActive(false);
		gameObject.SetActive(false);

		return this;
	}

	public void startFight (EnemyType type) {
		enemy.initEnemy(type);
//		giveSwordToHero();
		holderColor = new Color(1, 1, 1, 0);
		iconsHolderRender.color = holderColor;
		elementsHolder.initializeElements();
		enemy.transform.localPosition = new Vector2(10, enemyPos.y);
		enemyPos = enemy.transform.localPosition;
		fightInterface.setEnemy(enemy);
		elementsHolder.setActive(true);

		deadStone.transform.localPosition = deadStoneInitPos;
		enemyDeadAnimator.gameObject.SetActive(false);
//		potionBag.setOnScreen(ScreenBagType.FIGHT);
		gameObject.SetActive(true);
		fightStarted = startAnimDone = fightOver = false;
	}

//	private void giveSwordToHero () {
//		if (Vars.gameplay.getEquipmentScreen().getHolder(ItemType.WEAPON).getItem() == null) {
//			Vars.gameplay.getEquipmentScreen().getHolder(ItemType.WEAPON).placeItem(ItemFactory.createRandomItem(ItemType.WEAPON));
//		}
//		if (Vars.gameplay.getEquipmentScreen().getHolder(ItemType.ARMOR).getItem() == null) {
//			Vars.gameplay.getEquipmentScreen().getHolder(ItemType.ARMOR).placeItem(ItemFactory.createRandomItem(ItemType.ARMOR));
//		}
//	}

	void Update () {
		if (!fightStarted) {
			if (!startAnimDone) {
				animatingFightStart();
			} else if (!elementsHolder.isAllElementsOnCells()) {
				fightStarted = true;
				fightProcessor.startFight();
			}
		}
		if (enemyDeadPlaying) {
			if (ENEMY_DEAD_ANIM_DONE) {
				showFightResultScreen(true);
			}
		}
	}

	private void animatingFightStart () {
		if (enemy.transform.localPosition.x > enemyFinalX) { enemyPos.x -= enemyAppearSpeed; }
		else { enemyPos.x = enemyFinalX; }

		if (holderColor.a < 1) { holderColor.a += .03f; }
		else { holderColor.a = 1;}

		iconsHolderRender.color = holderColor;
		enemy.transform.localPosition = enemyPos;

		if (holderColor.a >= 1 && enemy.transform.position.x <= enemyFinalX) {
			startAnimDone = true;
			elementsHolder.startElementsDrop();
		}
		//		if (enemyScale.x < 1) {
		//			enemyScale.x += .03f;
		//			enemyScale.y += .03f;
		//		} else {
		//			enemyScale.x = 1;
		//			enemyScale.y = 1;
		//		}
	}

	public void finishFight (bool playerWin) {
//		potionBag.setBagActive(false);

		ENEMY_DEAD_ANIM_DONE = !playerWin;

		if (playerWin) {
			enemyDeadAnimator.gameObject.SetActive(true);
			enemyDeadAnimator.Play("EnemyDead");
			enemy.destroyEnemy();
			enemyDeadPlaying = true;
		} else {
			showFightResultScreen(false);
		}
		elementsHolder.setActive(false);
		fightOver = true;
	}

	private void showFightResultScreen (bool playerWin) {
		enemyDeadPlaying = false;
		resultScreen.showFightResultScreen(playerWin? enemy: null);
	}

	public bool isFightOver () {
		return fightOver;
	}

	public void closeFightScreen () {
		//		Vars.player.clearAfterFight();
		//		Vars.enemy.clearAfterFight();
		//		enemyImage.clearImage();
		//		FightMessenger.clearMessages();
		//		fightResultScreen.closeScreen();
		gameObject.SetActive(false);
		exploreScreen.endFight();
	}

	public FightResultScreen getResultScreen () {
		return resultScreen;
	}

	public FightProcessor getFightProcessor () {
		return fightProcessor;
	}

	public ElementEffectPlayer getIconEffectPlayer () {
		return elementEffectPlayer;
	}

	public FightEffectPlayer getFightEffectPlayer () {
		return fightEffectPlayer;
	}

//	void Update () {
//		switch (machineState) {
//			case StateMachine.FIGHT_START:
//				if(!animatingFightStart()) { switchMachineState(StateMachine.ICONS_DROP); }
//				break;
//			case StateMachine.ICONS_DROP:
//				if (iconsHolder.isAllIconsOnCells()) { switchMachineState(StateMachine.PLAYER_TURN); }
//				break;
//			case StateMachine.PLAYER_TURN:
//				if (!playerChecked) {
//					if (!checkNextTurn(true)) { switchMachineState(StateMachine.PLAYER_MOVE_DONE); }
//				} else {
//					if (PLAYER_MOVE_DONE) {
//						switchMachineState(StateMachine.ICONS_ANIMATION);
//						PLAYER_MOVE_DONE = false;
//					} else {
//						iconsHolder.checkPlayerInput();
//					}
//				}
//				break;
//			case StateMachine.ICONS_ANIMATION:
//				if (ICONS_ANIM_DONE && FIGHT_ANIM_PLAYER_DONE && FIGHT_ANIM_ENEMY_DONE) {
//	//				if (Hero.getHealth() > Vars.player.getMaxHealth()) { Vars.player.setHealthToMax(); }
//	//				if (Vars.enemy.getHealth() > Vars.enemy.getMaxHealth()) { Vars.enemy.setHealthToMax(); }
//					switchMachineState(StateMachine.PLAYER_MOVE_DONE);
//				}
//				break;
//			case StateMachine.PLAYER_MOVE_DONE:
//				afterPlayerMove();
//	//			Vars.player.getStatusEffectHolder().updateEffects();
//				playerChecked = false;
//				switchMachineState(StateMachine.ICONS_POSITIONING);
//				break;
//			case StateMachine.ICONS_POSITIONING:
//				if (iconsHolder.isAllIconsOnCells()) {
//					if (enemy.getHealth() <= 0) { switchMachineState(StateMachine.PLAYER_WIN); }
//					else if (Hero.getHealth() <= 0) { switchMachineState(StateMachine.ENEMY_WIN); }
//					else if (iconsHolder.checkIconsMatch()) {
//						ICONS_ANIM_DONE = false;
//						switchMachineState(StateMachine.ICONS_ANIMATION);
//					}
//					else { switchMachineState(StateMachine.ENEMY_TURN); }
//				}
//				break;
//			case StateMachine.ENEMY_TURN:
//				if (checkNextTurn(false)) { calculateBossTurnResult(); }
//				switchMachineState(StateMachine.ENEMY_MOVE_DONE);
//				break;
//			case StateMachine.ENEMY_MOVE_DONE:
//				if (FIGHT_ANIM_PLAYER_DONE && FIGHT_ANIM_ENEMY_DONE) {
//	//				if (Vars.player.getHealth() > Vars.player.getMaxHealth()) { Vars.player.setHealthToMax(); }
//	//				if (Vars.enemy.getHealth() > Vars.enemy.getMaxHealth()) { Vars.enemy.setHealthToMax(); }
//					if (Hero.getHealth() <= 0) { switchMachineState(StateMachine.ENEMY_WIN); }
//					else { switchMachineState(StateMachine.PLAYER_TURN); }//Vars.enemy.getStatusEffectHolder().updateEffects(); }
//				}
//				break;
//			case StateMachine.PLAYER_WIN:
//				playerWin = true;
//				finishFight();
//				break;
//			case StateMachine.ENEMY_WIN:
//				playerWin = false;
//				finishFight();
//				break;
//			case StateMachine.ENEMY_DEAD_ANIM:
//				if (ENEMY_BOSS_DEAD_ANIM_DONE) { showFightResultScreen(); }
//				break;
//			case StateMachine.NOT_IN_FIGHT: break;
//		}
//	}
}