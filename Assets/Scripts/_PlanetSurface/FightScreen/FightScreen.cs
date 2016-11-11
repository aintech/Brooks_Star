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

	public FightEffectPlayer fightEffectPlayer { get; private set; }

	private ElementEffectPlayer elementEffectPlayer;

	private FightInterface fightInterface;

	private Enemy enemy;

	private FightResultScreen resultScreen;

	private Animator enemyDeadAnimator;

	private Transform deadStone;

	private Vector2 deadStoneInitPos = new Vector2(0, 2f);

	private FightProcessor fightProcessor;

	private ScanningScreen scanningScreen;

	private bool fightStarted, fightOver, startAnimDone, enemyDeadPlaying;

	private bool playerWin;

	private PlayerData playerData;

	private List<SupplySlot> supplySlots = new List<SupplySlot>();

	private ItemDescriptor itemDescriptor;

	public FightScreen init (ScanningScreen scanningScreen, PlayerData playerData, ItemDescriptor itemDescriptor) {
		this.scanningScreen = scanningScreen;
		this.playerData = playerData;
		this.itemDescriptor = itemDescriptor;
		itemDescriptor.setFightScreen(this);
		elementsHolder = transform.Find("ElementsHolder").GetComponent<ElementsHolder>();
		iconsHolderRender = elementsHolder.GetComponent<SpriteRenderer>();
		fightEffectPlayer = transform.Find("FightEffectPlayer").GetComponent<FightEffectPlayer>().init();
		elementEffectPlayer = transform.Find("ElementEffectPlayer").GetComponent<ElementEffectPlayer>();
		fightInterface = transform.Find("Fight Interface").GetComponent<FightInterface>();
		enemy = transform.Find("Enemy").GetComponent<Enemy>();
		resultScreen = transform.Find("FightResultScreen").GetComponent<FightResultScreen>().init(this);
		enemyDeadAnimator = transform.Find("EnemyDeadAnim").GetComponent<Animator>();
		fightProcessor = GetComponent<FightProcessor>();
		deadStone = enemyDeadAnimator.transform.Find("DeadStone");
		enemyPos = enemy.transform.localPosition;
		elementsHolder.init();
		enemy.init();
		fightInterface.init();
		elementEffectPlayer.init(this, enemy);
		fightProcessor.init(this, elementsHolder, enemy);

		elementsHolder.gameObject.SetActive(true);
		fightInterface.gameObject.SetActive(true);
		enemyDeadAnimator.gameObject.SetActive(false);
		gameObject.SetActive(false);

		Transform supplyHolder = transform.Find("Supply Holder");
		SupplySlot slot;
		for (int i = 0; i < supplyHolder.childCount; i++) {
			slot = supplyHolder.GetChild(i).GetComponent<SupplySlot>();
			slot.init();
			supplySlots.Add(slot);
		}

		return this;
	}

	public void startFight (EnemyType type) {
		foreach (SupplySlot slot in playerData.supplySlots) {
			if (slot.item != null) {
				getSlot(slot.index).setItem(slot.takeItem());
			}
		}
		itemDescriptor.setEnabled(ItemDescriptor.Type.FIGHT, null);
		playerWin = false;
		Player.updateMinMaxDamage();
		enemy.initEnemy(type);
		holderColor = new Color(1, 1, 1, 0);
		iconsHolderRender.color = holderColor;
		elementsHolder.initializeElements();
		enemy.transform.localPosition = new Vector2(10, enemyPos.y);
		enemyPos = enemy.transform.localPosition;
		fightInterface.setEnemy(enemy);
		elementsHolder.setActive(true);

		deadStone.transform.localPosition = deadStoneInitPos;
		enemyDeadAnimator.gameObject.SetActive(false);
		gameObject.SetActive(true);
		fightStarted = startAnimDone = fightOver = false;
	}

	private SupplySlot getSlot (int index) {
		foreach (SupplySlot slot in supplySlots) {
			if (slot.index == index) { return slot; }
		}
		Debug.Log("Unknown slot index: " + index);
		return null;
	}

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
				showFightResultScreen();
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
		this.playerWin = playerWin;
		ENEMY_DEAD_ANIM_DONE = !playerWin;

		if (playerWin) {
			enemyDeadAnimator.gameObject.SetActive(true);
			enemyDeadAnimator.Play("EnemyDead");
			enemy.destroyEnemy();
			enemyDeadPlaying = true;
		} else {
			showFightResultScreen();
		}
		elementsHolder.setActive(false);
		fightOver = true;
	}

	private void showFightResultScreen () {
		enemyDeadPlaying = false;
		resultScreen.showFightResultScreen(playerWin? enemy: null);
	}

	public bool isFightOver () {
		return fightOver;
	}

	public void closeFightScreen () {
		gameObject.SetActive(false);
		foreach (SupplySlot slot in supplySlots) {
			if (slot.item != null) {
				playerData.getSupplySlot(slot.index).setItem(slot.takeItem());
			}
		}
		scanningScreen.endFight(playerWin);
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

	public void useSupply (SupplySlot slot) {
		if (slot.item != null && fightProcessor.canUseSupply()) {
			SupplyData data = (SupplyData)slot.item.itemData;
			switch (data.type) {
				case SupplyType.MEDKIT_SMALL:
				case SupplyType.MEDKIT_MEDIUM:
				case SupplyType.MEDKIT_LARGE:
				case SupplyType.MEDKIT_ULTRA:
					fightEffectPlayer.playEffect(FightEffectType.HEAL, Player.healPlayer(data.value));
					break;
			}
			fightProcessor.skipTurn();
			slot.takeItem().destroy();
		}
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