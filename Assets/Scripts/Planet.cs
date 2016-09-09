using UnityEngine;
using System.Collections;

public class Planet : MonoBehaviour, ButtonHolder {

	public Sprite coras;

	private SpriteRenderer render;

	private Button marketBtn, hangarBtn, leaveBtn;

	private PlanetScene scene;

	public Planet init (PlanetScene scene) {
		this.scene = scene;
		render = GetComponent<SpriteRenderer>();
		setSprite();
		marketBtn = transform.Find("MarketBtn").GetComponent<Button>();
		hangarBtn = transform.Find("HangarBtn").GetComponent<Button>();
		leaveBtn = transform.Find("LeaveBtn").GetComponent<Button>();
		return this;
	}

	private void setSprite () {
		switch(Variables.planetType) {
			case PlanetType.CORAS: render.sprite = coras; break;
			default: Debug.Log("Неизвестный тип планеты"); break;
		}
	}

	public void setEnabled (bool enable) {
		marketBtn.setEnable(enable);
		hangarBtn.setEnable(enable);
		leaveBtn.setEnable(enable);
	}

	public void fireClickButton (Button btn) {
		if (btn == marketBtn) {
			scene.showMarketScreen();
		} else if (btn == hangarBtn) {
			scene.showHangarScreen();
		} else if (btn == leaveBtn) {
			scene.leavePlanet();
		} else {
			Debug.Log("Неизвестная кнопка: " + btn.name);
		}
	}
}