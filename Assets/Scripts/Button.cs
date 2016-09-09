using UnityEngine;
using System.Collections;

public class Button : MonoBehaviour {
	
	public Sprite normal, hover;

	public Color32 normalColor, hoverColor;

	private SpriteRenderer render;
	
	private Collider2D coll;
	
	private TextMesh text;

	private MeshRenderer textRender;

	private State state = State.NORMAL;
	
	private ButtonHolder holder;
	
	public Button init () {
		render = GetComponent<SpriteRenderer>();
		coll = GetComponent<Collider2D>();
		holder = transform.parent.GetComponent<ButtonHolder>();
		text = transform.Find("BtnText").GetComponent<TextMesh>();
		textRender = text.GetComponent<MeshRenderer>();
		textRender.sortingLayerName = render.sortingLayerName;
		textRender.sortingOrder = render.sortingOrder + 1;

		return this;
	}
	
	void Update () {
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
			case State.NORMAL: render.sprite = normal; text.color = normalColor; break;
			case State.HOVER: render.sprite = hover; text.color = hoverColor; break;
		}
	}

	public void setEnable (bool enable) {
		render.enabled = enable;
		coll.enabled = enable;
		textRender.enabled = enable;
	}
	
	private enum State {
		NORMAL, HOVER
	}
}