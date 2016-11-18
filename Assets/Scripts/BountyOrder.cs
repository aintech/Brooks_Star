using UnityEngine;
using System.Collections;

public class BountyOrder : MonoBehaviour {

	public Sprite normal, hover;

	private Color32 normalColor = new Color32(255, 255, 255, 255), notActiveColor = new Color32(255, 255, 255, 150), rewardColor, rewardHalfColor;

	private SpriteRenderer render, enemyRender;

	private Collider2D coll;

	private StrokeText rewardValue;

	private State state = State.NORMAL;

	private AnnouncementScreen announcementsScreen;

	private bool active = true;

	private EnemyType enemyType;

	public BountyOrder init (AnnouncementScreen announcementsScreen) {
		this.announcementsScreen = announcementsScreen;
		render = GetComponent<SpriteRenderer>();
		coll = GetComponent<Collider2D>();

		enemyRender = transform.Find("Enemy Image").GetComponent<SpriteRenderer>();
		rewardValue = transform.Find("Reward Value").GetComponent<StrokeText>().init("default", 3);

		rewardColor = rewardValue.textColor;
		rewardHalfColor = rewardColor;
		rewardHalfColor.a = 150;

		gameObject.SetActive(true);

		return this;
	}

	void Update () {
		if (!active) { return; }
		if (Utils.hit != null && Utils.hit == coll) {
			if (state == State.NORMAL) {
				changeState(State.HOVER);
			}
			if (Input.GetMouseButtonDown(0)) {
//				announcementsScreen.fireOrder(this);
			}
		} else if (Utils.hit != null && Utils.hit != coll && state == State.HOVER) {
			changeState(State.NORMAL);
		} else if (Utils.hit == null && state == State.HOVER) {
			changeState(State.NORMAL);
		}
	}

	private void changeState (State state) {
		this.state = state;
		switch (state) {
			case State.NORMAL: render.sprite = normal; break;
			case State.HOVER: render.sprite = hover; break;
		}
	}

	public void setTarget (EnemyType type) {
		rewardValue.setText(type.cost().ToString());
	}

	public void setActive (bool active) {
		this.active = active;
		coll.enabled = active;
		if (!active) { changeState(State.NORMAL); }
		render.color = active? normalColor: notActiveColor;
		rewardValue.textColor = active? rewardColor: rewardHalfColor;
	}

	private enum State {
		NORMAL, HOVER
	}
}