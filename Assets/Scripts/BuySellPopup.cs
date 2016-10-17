using UnityEngine;
using System.Collections;

public class BuySellPopup : MonoBehaviour, ButtonHolder {

	private Transform bar;

	private Vector3 barScale = Vector3.one, barPos;

	private float barLeft = -2.78f, fullTrack, barRatio, mouseX;

	private BoxCollider2D barCollider;

	private float[] zones;

	private Button decreaseBtn, increaseBtn, applyBtn, denyBtn, denyArea;

	private TextMesh text;

	private bool toBuy;

	private EquipmentsMarket market;

	private int count;

	private Item item;

	private bool drag;

	public BuySellPopup init (EquipmentsMarket market) {
		this.market = market;

		bar = transform.Find("Bar");
		barPos = bar.transform.localPosition;
		barLeft = bar.localPosition.x;
		fullTrack = -barLeft * 2;

		barCollider = bar.GetComponent<BoxCollider2D>();
		increaseBtn = transform.Find("Increase Button").GetComponent<Button>().init();
		decreaseBtn = transform.Find("Decrease Button").GetComponent<Button>().init();
		applyBtn = transform.Find("Apply Button").GetComponent<Button>().init();
		denyBtn = transform.Find("Deny Button").GetComponent<Button>().init();
		denyArea = transform.Find("Deny Area").GetComponent<Button>().init();

		text = transform.Find("Text").GetComponent<TextMesh>();
		MeshRenderer mesh = text.GetComponent<MeshRenderer>();
		mesh.sortingLayerName = GetComponent<SpriteRenderer>().sortingLayerName;
		mesh.sortingOrder = 1;

		close();

		return this;
	}

	public void fireClickButton (Button btn) {
		if (btn == applyBtn) { apply(); }
		else if (btn == decreaseBtn) { decrease(); }
		else if (btn == increaseBtn) { increase(); }
		else if (btn == denyBtn || btn == denyArea) { close(); }
	}

	public void show (Item item, bool toBuy) {
		this.item = item;
		this.toBuy = toBuy;
		count = item.itemData.quantity;
		barScale.x = 1f / (float)count;
		bar.localScale = barScale;
		barRatio = fullTrack / (float)count;
		zones = new float[count];
		for (int i = 0; i < count; i++) {
			zones[i] = barLeft + (barRatio * i);
		}
		updateValues();
		gameObject.SetActive(true);
	}

	void Update () {
		if (!drag && Input.GetMouseButtonDown(0) && Utils.hit != null && Utils.hit == barCollider) { drag = true; }
		if (drag) {
			if (Input.GetMouseButtonUp(0)) {
				drag = false;
			} else {
				adjustBarToMouse();
			}
		}
	}

	private void adjustBarToMouse () {
		mouseX = Utils.mousePos.x;
		if (mouseX < zones[1]) {
			count = 1;
		} else if (mouseX > zones[zones.Length-1] + barRatio) {
			count = item.itemData.quantity;
		} else {
			for (int i = 1; i < item.itemData.quantity; i++) {
				if (mouseX > zones[i] && mouseX < zones[i] + barRatio) {
					count = i + 1;
				}
			}
		}
		updateValues();
	}

	private void increase () {
		count++;
		updateValues();
	}

	private void decrease () {
		count--;
		updateValues();
	}

	private void updateValues () {
		text.text = (toBuy? "Купить <color=blue>": "Продать <color=blue>") + count + "</color> " + item.itemName() + " за <color=yellow>" + (count * item.cost()) + "$</color>";
		barPos.x = zones[count-1];
		bar.transform.localPosition = barPos;
		decreaseBtn.setActive(count > 1);
		increaseBtn.setActive(count < item.itemData.quantity);
	}

	private void apply () {
		if (toBuy) {
			market.buyItem(item, count);
		} else {
			market.sellItem(item, count);
		}
		close();
	}

	private void close () {
		item = null;
		gameObject.SetActive(false);
	}
}