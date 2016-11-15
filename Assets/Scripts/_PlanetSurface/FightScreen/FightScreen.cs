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

	public List<StatusEffect> playerStatusEffects = new List<StatusEffect>();

	public List<StatusEffect> enemyStatusEffects = new List<StatusEffect>();

	private List<StatusEffect> effList;

	private Vector3 playerStatusStartPosition = new Vector3(6.8f, 0, 0),
					enemyStatusStartPosition = new Vector3(6.9f, 7.5f, 0);

	private float playerStatusStep = 1.1f, enemyStatusStep = -.9f;

	private StrokeText playerActionsText, enemyActionsText;

	public FightScreen init (ScanningScreen scanningScreen, PlayerData playerData, ItemDescriptor itemDescriptor) {
		this.scanningScreen = scanningScreen;
		this.playerData = playerData;
		this.itemDescriptor = itemDescriptor;

		itemDescriptor.fightScreen = this;
		elementsHolder = transform.Find("ElementsHolder").GetComponent<ElementsHolder>();
		iconsHolderRender = elementsHolder.GetComponent<SpriteRenderer>();
		fightEffectPlayer = transform.Find("Fight Effect Player").GetComponent<FightEffectPlayer>().init();
		elementEffectPlayer = transform.Find("ElementEffectPlayer").GetComponent<ElementEffectPlayer>();
		fightInterface = transform.Find("Fight Interface").GetComponent<FightInterface>().init(this);
		enemy = transform.Find("Enemy").GetComponent<Enemy>();
		resultScreen = transform.Find("FightResultScreen").GetComponent<FightResultScreen>().init(this);
		enemyDeadAnimator = transform.Find("EnemyDeadAnim").GetComponent<Animator>();
		fightProcessor = GetComponent<FightProcessor>();
		deadStone = enemyDeadAnimator.transform.Find("DeadStone");
		enemyPos = enemy.transform.localPosition;
		elementsHolder.init();
		enemy.init(this);
		elementEffectPlayer.init(this, enemy);
		fightProcessor.init(this, elementsHolder, enemy);

		elementsHolder.gameObject.SetActive(true);
		enemyDeadAnimator.gameObject.SetActive(false);
		gameObject.SetActive(false);

		Transform actionsHolder = transform.Find("Actions Holder");
		actionsHolder.Find("Delimiter").GetComponent<StrokeText>().init("default", 5);
		playerActionsText = actionsHolder.Find("Player Actions").GetComponent<StrokeText>().init("default", 5);
		enemyActionsText = actionsHolder.Find("Enemy Actions").GetComponent<StrokeText>().init("default", 5);

		Transform supplyHolder = transform.Find("Supply Holder");
		SupplySlot slot;
		for (int i = 0; i < supplyHolder.childCount; i++) {
			slot = supplyHolder.GetChild(i).GetComponent<SupplySlot>();
			slot.init();
			supplySlots.Add(slot);
		}

		Player.fightScreen = this;

		return this;
	}

	public void startFight (EnemyType type) {
		SupplySlot supSlot;
		foreach (SupplySlot slot in playerData.supplySlots) {
			if (slot.item != null) {
				supSlot = getSlot (slot.index);
				supSlot.setItem(slot.takeItem());
				supSlot.item.transform.localScale = Vector3.one;
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
		fightStarted = startAnimDone = fightOver = false;
		foreach (StatusEffect eff in enemyStatusEffects) {
			eff.initEnemy (enemy);
		}

		gameObject.SetActive(true);
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

		foreach (StatusEffect eff in playerStatusEffects) { eff.endEffect(); }
		foreach (StatusEffect eff in enemyStatusEffects) { eff.endEffect(); }

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
		SupplySlot supSlot;
		foreach (SupplySlot slot in supplySlots) {
			if (slot.item != null) {
				supSlot = playerData.getSupplySlot (slot.index);
				supSlot.setItem(slot.takeItem());
				supSlot.item.transform.localScale = Vector3.one;
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
		if (slot.item != null) {
			SupplyData data = (SupplyData)slot.item.itemData;
			if (fightProcessor.canUseSupply (data.type)) {
				bool toPlayer = data.type != SupplyType.GRENADE_FLASH && data.type != SupplyType.GRENADE_PARALIZE;
				StatusEffect statusEffect = getStatusEffectByType(data.type.toStatusEffectType(), toPlayer);
				FightEffectType fightEffectType = data.type.toFightEffectType();

				if (data.type.isMedkit()) {
					fightEffectPlayer.playEffect(fightEffectType, Player.heal(data.value));
				} else if (data.type.isGrenade()) {
					fightEffectPlayer.playEffectOnEnemy (fightEffectType, 0);
					statusEffect.addStatus (data.value, data.duration);
				} else if (data.type.isInjection()) {
					fightEffectPlayer.playEffect (fightEffectType, data.value);
					statusEffect.addStatus (data.value, data.duration);
				} else {
					Debug.Log("Unknown status effect type");
				}

				Vector3 effectPos = toPlayer? playerStatusStartPosition: enemyStatusStartPosition;

				int activeEffects = -1;//вычитаем добавляемый эффект

				effList = toPlayer? playerStatusEffects: enemyStatusEffects;

				for (int i = 0; i < effList.Count; i++) {
					if (effList[i].isFired) { activeEffects++; }
				}

				if (toPlayer) { effectPos.x += activeEffects * playerStatusStep; }
				else { effectPos.y += activeEffects * enemyStatusStep; }

				statusEffect.transform.localPosition = effectPos;

				fightProcessor.skipAction ();
				FightProcessor.FIGHT_ANIM_PLAYER_DONE = false;
				slot.takeItem ().destroy ();
			}
		}
	}

	public StatusEffect getStatusEffectByType (StatusEffectType type, bool toPlayer) {
		if (type.withoutStatusHolder()) { return null; }
		effList = toPlayer? playerStatusEffects: enemyStatusEffects;
		foreach (StatusEffect eff in effList) {
			if (eff.statusType == type) {
				return eff;
			}
		}
		Debug.Log ("Cant find holder of effect type: " + type);
		return null;
	}

	public void updateActionTexts (int playerActions, int enemyActions) {
		playerActionsText.setText(playerActions == 0? "-": playerActions.ToString());
		enemyActionsText.setText(enemyActions == 0? "-": enemyActions.ToString());
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