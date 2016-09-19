using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FightProcessor : MonoBehaviour {
	
	private List<TurnResult> turnResults = new List<TurnResult>();

	private StateMachine machineState = StateMachine.NOT_IN_FIGHT;

	private ElementsHolder elementsHolder;

	private Enemy enemy;

	public static bool PLAYER_MOVE_DONE = false;

	public static bool ELEMENTS_ANIM_DONE = true;

	public static bool FIGHT_ANIM_PLAYER_DONE = true, FIGHT_ANIM_ENEMY_DONE = true;

	private bool playerChecked = false;

	private FightScreen fightScreen;

//	private PotionBag potionBag;

	public void init (FightScreen fightScreen, ElementsHolder elementsHolder, Enemy enemy) {
		this.fightScreen = fightScreen;
		this.elementsHolder = elementsHolder;
		this.enemy = enemy;
//		this.potionBag = potionBag;
	}

	public void startFight () {
		switchMachineState(StateMachine.PLAYER_TURN);
	}

	void Update () {
		switch (machineState) {
//			case StateMachine.FIGHT_START:
//				if(!animatingFightStart()) { switchMachineState(StateMachine.ICONS_DROP); }
//				break;
//			case StateMachine.ICONS_DROP:
//				if (elementsHolder.isAllIconsOnCells()) { switchMachineState(StateMachine.PLAYER_TURN); }
//				break;
			case StateMachine.NOT_IN_FIGHT: break;
			case StateMachine.PLAYER_TURN:
				if (!playerChecked) {
					if (!checkNextTurn(true)) { switchMachineState(StateMachine.PLAYER_MOVE_DONE); }
				} else {
					if (PLAYER_MOVE_DONE) {
						switchMachineState(StateMachine.ICONS_ANIMATION);
						PLAYER_MOVE_DONE = false;
					} else {
						elementsHolder.checkPlayerInput();
					}
				}
				break;
			case StateMachine.ICONS_ANIMATION:
				if (ELEMENTS_ANIM_DONE && FIGHT_ANIM_PLAYER_DONE && FIGHT_ANIM_ENEMY_DONE) {
					//				if (Hero.getHealth() > Vars.player.getMaxHealth()) { Vars.player.setHealthToMax(); }
					//				if (Vars.enemy.getHealth() > Vars.enemy.getMaxHealth()) { Vars.enemy.setHealthToMax(); }
					switchMachineState(StateMachine.PLAYER_MOVE_DONE);
				}
				break;
			case StateMachine.PLAYER_MOVE_DONE:
				afterPlayerMove();
				//			Vars.player.getStatusEffectHolder().updateEffects();
				playerChecked = false;
				switchMachineState(StateMachine.ICONS_POSITIONING);
				break;
			case StateMachine.ICONS_POSITIONING:
				if (elementsHolder.isAllElementsOnCells()) {
					if (enemy.health <= 0) { switchMachineState(StateMachine.PLAYER_WIN); }
					else if (Player.health <= 0) { switchMachineState(StateMachine.ENEMY_WIN); }
					else if (elementsHolder.checkElementsMatch()) {
						ELEMENTS_ANIM_DONE = false;
						switchMachineState(StateMachine.ICONS_ANIMATION);
					}
					else { switchMachineState(StateMachine.ENEMY_TURN); }
				}
				break;
			case StateMachine.ENEMY_TURN:
				if (checkNextTurn(false)) { calculateEnemyTurnResult(); }
				switchMachineState(StateMachine.ENEMY_MOVE_DONE);
				break;
			case StateMachine.ENEMY_MOVE_DONE:
				if (FIGHT_ANIM_PLAYER_DONE && FIGHT_ANIM_ENEMY_DONE) {
					//				if (Vars.player.getHealth() > Vars.player.getMaxHealth()) { Vars.player.setHealthToMax(); }
					//				if (Vars.enemy.getHealth() > Vars.enemy.getMaxHealth()) { Vars.enemy.setHealthToMax(); }
					if (Player.health <= 0) { switchMachineState(StateMachine.ENEMY_WIN); }
					else { switchMachineState(StateMachine.PLAYER_TURN); }//Vars.enemy.getStatusEffectHolder().updateEffects(); }
				}
				break;
			case StateMachine.PLAYER_WIN:
				endFight(true);
				break;
			case StateMachine.ENEMY_WIN:
				endFight(false);
				break;
		}
	}

	private bool checkNextTurn (bool mustBePlayerTurn) {
		if (!playerChecked && machineState == StateMachine.PLAYER_TURN) { playerChecked = true; }//???
		return true;
	}

	private void afterPlayerMove () {
		elementsHolder.refreshSortingOrder();
		elementsHolder.repositionMatchingElements();
		elementsHolder.setelEmentsGoToCenter();
	}

	private void rearrangeIcons () {
		elementsHolder.rearrangeElements();
	}

	public void addToTurnResult (ElementType type, int count, Vector2 pos) {
		foreach (TurnResult result in turnResults) {
			if (result.getElementType() == type) {
				result.addCount(count);
				result.setPosition(getMiddlePoint(result.getPosition(), pos));
				return;
			}
		}
		turnResults.Add(new TurnResult(type, count, pos));
	}

	private Vector2 getMiddlePoint (Vector2 pos1, Vector2 pos2) {
		float x = (pos1.x + pos2.x) * .5f;
		float y = (pos1.y + pos2.y) * .5f;
		return new Vector2(x, y);
	}

	public void calculateHeroTurnResults () {
		if (turnResults.Count == 0) { return; }

		foreach (TurnResult result in turnResults) {
			int damage = Player.damage;
			if (result.getCount() > 3) {
				damage += Mathf.RoundToInt((float)Player.damage * .5f) *  (result.getCount() - 3);
			}
			fightScreen.getIconEffectPlayer().addEffect(result.getElementType(), damage, result.getPosition(), result.getCount());
		}

		turnResults.Clear();
	}

	private void calculateEnemyTurnResult () {
		//		if (Vars.enemy.getStatusEffectHolder().haveActiveEffect(StatusEffectType.BLINDED)) {
		//			rand = Random.value;
		//			if (rand > .5f) {
		//				FightMessenger.addMessage(Vars.enemy.getEnemyType().getName(), "промахивается");
		//				return;
		//			}
		//		}
		FIGHT_ANIM_PLAYER_DONE = false;
		fightScreen.getFightEffectPlayer().playEffect(FightEffectType.DAMAGE, Player.hitPlayer(enemy.damage));
		//		FightMessenger.addDamageMessage(Vars.playerName, ShotElementType.SIMPLE, damage);
	}


	public void checkEffectsActive () {
		if (!fightScreen.getIconEffectPlayer().isPlayingEffect()) {
			FIGHT_ANIM_ENEMY_DONE = true;
		}
	}

	private void switchMachineState (StateMachine machineState) {
		this.machineState = machineState;
//		if (potionBag.isActive() && !canDrinkPotion()) { potionBag.setBagActive(false); }
//		else if (!potionBag.isActive() && canDrinkPotion()) { potionBag.setBagActive(true); }
	}

	public void skipTurn () {
		switchMachineState(StateMachine.PLAYER_MOVE_DONE);
	}

	public bool canDrinkPotion () {
		return machineState == StateMachine.PLAYER_TURN;
	}

	private void endFight (bool playerWin) {
		switchMachineState(StateMachine.NOT_IN_FIGHT);
		ELEMENTS_ANIM_DONE = true;

		fightScreen.finishFight(playerWin);
	}

	private enum StateMachine {
		NOT_IN_FIGHT,
		ICONS_ANIMATION, ICONS_POSITIONING,
		PLAYER_TURN, PLAYER_MOVE_DONE,
		ENEMY_TURN, ENEMY_MOVE_DONE,
		PLAYER_WIN, ENEMY_WIN
	}

	public class TurnResult {
		private ElementType elementType = ElementType.FIRE;
		private int count = 0;
		private Vector2 pos;

		public TurnResult (ElementType elementType, int count, Vector2 pos) {
			this.elementType = elementType;
			this.count = count;
			this.pos = pos;
		}

		public void addCount (int value) {
			this.count += value;
		}

		public int getCount () {
			return count;
		}

		public ElementType getElementType () {
			return elementType;
		}

		public Vector2 getPosition () {
			return pos;
		}

		public void setPosition (Vector2 pos) {
			this.pos = pos;
		}
	}
}