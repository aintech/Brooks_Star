using UnityEngine;
using System.Collections;

public class GalaxyMap : MonoBehaviour, ButtonHolder {

	public GUIStyle closeBtnStyle, systemNameStyle;

	private Rect closeBtnRect = new Rect(Screen.width - 332 - 10, Screen.height - 64 - 10, 332, 64),
				 systemNameRect = new Rect(Screen.width * .5f, 25, 0, 0);

	private string systemName;

	private Button aluriaBtn, critaBtn;

	private Transform galaxy;

	private Vector3 pos;

	private float scrWidth, scrHeight, diffX, diffY, xOffset, yOffset;

	[HideInInspector]
	public bool onScreen;

	private GalaxyJumpController jumper;

	public GalaxyMap init (GalaxyJumpController jumper) {
		this.jumper = jumper;
		galaxy = transform.Find("Galaxy");
		galaxy.gameObject.SetActive(true);
		pos = galaxy.localPosition;

		Transform bg = galaxy.Find("Background");
		Vector3 size = bg.GetComponent<SpriteRenderer>().sprite.bounds.size;
		scrWidth = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height)).x;
		scrHeight = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height)).y;
		diffX = (size.x - scrWidth * 2f) / 2f;
		diffY = (size.y - scrHeight * 2f) / 2f;

		aluriaBtn = galaxy.Find("Aluria").GetComponent<Button>().initWithHolder(this);
		critaBtn = galaxy.Find("Crita").GetComponent<Button>().initWithHolder(this);

		gameObject.SetActive(false);

		return this;
	}

	void Update () {
		if (onScreen) {
			xOffset = (Utils.mousePos.x - transform.position.x) / scrWidth;
			yOffset = (Utils.mousePos.y - transform.position.y) / scrHeight;
			pos.x = -(xOffset < -1? -1: xOffset > 1? 1: xOffset) * diffX;
			pos.y = -(yOffset < -1? -1: yOffset > 1? 1: yOffset) * diffY;
			galaxy.localPosition = pos;

			if (Input.GetKeyDown(KeyCode.Escape)) {
				close();
			}
		}
	}

	void OnGUI () {
		if (onScreen) {
			if (GUI.Button(closeBtnRect, "", closeBtnStyle)) {
				close();
			}
			GUI.Label(systemNameRect, systemName, systemNameStyle);
		}
	}

	public void fireClickButton (Button btn) {
		if (btn == aluriaBtn) { startJump(StarSystemType.ALURIA); }
		else if (btn == critaBtn) { startJump(StarSystemType.CRITA); }
	}

	private void startJump (StarSystemType systemType) {
		close();
		Vector3 fromVec = Vector3.zero;
		Vector3 toVec = Vector3.zero;
		switch (Vars.starSystemType) {
			case StarSystemType.ALURIA: fromVec = aluriaBtn.transform.localPosition; break;
			case StarSystemType.CRITA: fromVec = critaBtn.transform.localPosition; break;
			default: Debug.Log("Unknown system type: " + Vars.starSystemType); break;
		}
		switch (systemType) {
			case StarSystemType.ALURIA: toVec = aluriaBtn.transform.localPosition; break;
			case StarSystemType.CRITA: toVec = critaBtn.transform.localPosition; break;
			default: Debug.Log("Unknown system type: " + systemType); break;
		}
		jumper.startJumpSequence(systemType, (toVec - fromVec).normalized);
	}

	public void show () {
		StarSystem.setGamePause(true);
		transform.position = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, transform.position.z);
		UserInterface.showInterface = false;
		onScreen = true;

		aluriaBtn.setActive(Vars.starSystemType != StarSystemType.ALURIA);
		critaBtn.setActive(Vars.starSystemType != StarSystemType.CRITA);

		systemName = "Текущая система: <color=white><size=40>" + Vars.starSystemType.name() + "</size></color>";

		gameObject.SetActive(true);
	}

	private void close () {
		StarSystem.setGamePause(false);
		UserInterface.showInterface = true;
		onScreen = false;
		gameObject.SetActive(false);
	}
}