using UnityEngine;
using System.Collections;

public class Button : MonoBehaviour {
	
	public Sprite normal, hover;
	
	private SpriteRenderer render;
	
	private Collider2D collider;
	
	private TextMesh text;

	private MeshRenderer textRender;

	private State state = State.NORMAL;
	
	private ButtonHolder holder;
	
	void Awake () {
		render = GetComponent<SpriteRenderer>();
		collider = GetComponent<Collider2D>();
		holder = transform.parent.GetComponent<ButtonHolder>();
		text = transform.Find("BtnText").GetComponent<TextMesh>();
		textRender= text.GetComponent<MeshRenderer>();
		textRender.sortingLayerName = render.sortingLayerName;
		textRender.sortingOrder = render.sortingOrder + 1;
	}
	
	void Update () {
		if (Utils.hit != null && Utils.hit == collider) {
			if (state == State.NORMAL) {
				changeState(State.HOVER);
			}
			if (Input.GetMouseButtonDown(0)) {
				holder.fireClickButton(this);
			}
		} else if (Utils.hit != null && Utils.hit != collider && state == State.HOVER) {
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

	public void setEnable (bool enable) {
		render.enabled = enable;
		collider.enabled = enable;
		textRender.enabled = enable;
	}
	
	private enum State {
		NORMAL, HOVER
	}
}