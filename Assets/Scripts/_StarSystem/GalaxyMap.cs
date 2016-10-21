using UnityEngine;
using System.Collections;

public class GalaxyMap : MonoBehaviour, ButtonHolder {

	private Button aluriaBtn, closeBtn;

	private Transform galaxy;

	private Vector3 pos;

	private float scrWidth, scrHeight, diffX, diffY, xOffset, yOffset;

	[HideInInspector]
	public bool onScreen;

	public GalaxyMap init () {
		galaxy = transform.Find("Galaxy");
		galaxy.gameObject.SetActive(true);
		pos = galaxy.localPosition;

		Transform bg = galaxy.Find("Background");
		Vector3 size = bg.GetComponent<SpriteRenderer>().sprite.bounds.size;
		scrWidth = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height)).x;
		scrHeight = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height)).y;
		diffX = (size.x - scrWidth * 2f) / 2f;
		diffY = (size.y - scrHeight * 2f) / 2f;

		aluriaBtn = galaxy.Find("Aluria").GetComponent<Button>().init();
		closeBtn = transform.Find("Close Button").GetComponent<Button>().init();

		gameObject.SetActive(false);

		return this;
	}

	void Update () {
		xOffset = Utils.mousePos.x / scrWidth;
		yOffset = Utils.mousePos.y / scrHeight;
		pos.x = -(xOffset < -1? -1: xOffset > 1? 1: xOffset) * diffX;
		pos.y = -(yOffset < -1? -1: yOffset > 1? 1: yOffset) * diffY;
		galaxy.localPosition = pos;	
	}

	public void fireClickButton (Button btn) {
		if (btn == closeBtn) { close(); }
	}

	public void show () {
		transform.position = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, transform.position.z);
		UserInterface.showInterface = false;
		onScreen = true;
		gameObject.SetActive(true);
	}

	private void close () {
		UserInterface.showInterface = true;
		onScreen = false;
		gameObject.SetActive(false);
	}
}