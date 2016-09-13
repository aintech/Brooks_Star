using UnityEngine;
using System.Collections;

public class Button : MonoBehaviour {
	
	public Sprite normal, hover;

	public Color32 normalTextColor, hoverTextColor;

	private Color32 normalColor = new Color32(255, 255, 255, 255), notActiveColor = new Color32(255, 255, 255, 150);

	private SpriteRenderer render;
	
	private Collider2D coll;
	
	private TextMesh text;

	private MeshRenderer textRender;

	private State state = State.NORMAL;
	
	private ButtonHolder holder;

	private bool active = true;

	public Button init () {
		render = GetComponent<SpriteRenderer>();
		coll = GetComponent<Collider2D>();
		holder = transform.parent.GetComponent<ButtonHolder>();
		text = transform.Find("BtnText").GetComponent<TextMesh>();
		textRender = text.GetComponent<MeshRenderer>();
		textRender.sortingLayerName = render.sortingLayerName;
		textRender.sortingOrder = render.sortingOrder + 1;

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
				holder.fireClickButton(this);
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
			case State.NORMAL: render.sprite = normal; text.color = normalTextColor; break;
			case State.HOVER: render.sprite = hover; text.color = hoverTextColor; break;
		}
	}

	public void setEnable (bool enable) {
		render.enabled = enable;
		coll.enabled = enable;
		textRender.enabled = enable;
	}

	public void setActive (bool active) {
		this.active = active;
		coll.enabled = active;
		if (!active) { changeState(State.NORMAL); }
		render.color = active? normalColor: notActiveColor;
		text.color = active? normalColor: notActiveColor;
	}

	private enum State {
		NORMAL, HOVER
	}
}