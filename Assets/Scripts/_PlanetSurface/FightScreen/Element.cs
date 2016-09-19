using UnityEngine;
using System.Collections;

public class Element : MonoBehaviour {

	public Sprite[] elementSprites;

	public Sprite fireElement, waterElement, earthElement, airElement, lightElement, darkElement;

	private SpriteRenderer render;

	private Transform trans;

	private ElementType elementType;

	private Vector3 cellCenter, target;

	private int row, column;

	private const float MOVE_SPEED = .2f;

	private const float CENTER_DIST = MOVE_SPEED * 2;

	private float x, y;

	private bool goToTarget;

	private bool fadeOut;

	private Color color = new Color(1, 1, 1, 1);

	private Vector3 scale = Vector3.one, rotatePoint = new Vector3(0, 0, 1);

	private Collider2D col;

	void Awake () {
		render = GetComponent<SpriteRenderer>();
		col = GetComponent<Collider2D>();
		trans = transform;
	}

	public ElementType getElementType () {
		return elementType;
	}

	public void initRandomElement () {
		int rand = Random.Range(0, ElementDescriptor.getElementsCount());
		switch (rand) {
			case 0: initElement(ElementType.FIRE); break;
			case 1: initElement(ElementType.WATER); break;
			case 2: initElement(ElementType.EARTH); break;
			case 3: initElement(ElementType.AIR); break;
			case 4: initElement(ElementType.LIGHT); break;
			case 5: initElement(ElementType.DARK); break;
			default: Debug.Log("Unknown element type");break;
		}
	}

	public void initElement (ElementType elementType) {
		this.elementType = elementType;
		setSprite();
	}

	public SpriteRenderer getRender () {
		return render;
	}

	void Update () {
		if (goToTarget) {
			moveToTarget();
		}
		if (fadeOut) {
			if (render.color.a > 0) {
				color.a -= .05f;
				render.color = color;
				scale.x -= .05f;
				scale.y -= .05f;
				trans.localScale = scale;
				trans.Rotate(rotatePoint, -10);
			} else {
				fadeOut = false;
				FightProcessor.ELEMENTS_ANIM_DONE = true;
			}
		}
	}

	public void refreshElement () {
		color.a = 1;
		trans.localRotation = new Quaternion();
		trans.localScale = scale = Vector3.one;
		render.color = color;
	}

	private void moveToTarget () {
		if (Vector2.Distance(trans.localPosition, target) <= CENTER_DIST) {
			trans.localPosition = target;
			render.sortingOrder = ElementsHolder.START_SORT_ORDER;
			goToTarget = false;
		} else {
			if ((target.x - trans.localPosition.x) < -MOVE_SPEED) {
				x = trans.localPosition.x - MOVE_SPEED;
			} else if ((target.x - trans.localPosition.x) > MOVE_SPEED) {
				x = trans.localPosition.x + MOVE_SPEED;
			}
			if ((target.y - trans.localPosition.y) < -MOVE_SPEED) {
				y = trans.localPosition.y - MOVE_SPEED;
			} else if ((target.y - trans.localPosition.y) > MOVE_SPEED) {
				y = trans.localPosition.y + MOVE_SPEED;
			}
			trans.localPosition = new Vector2(x, y);
		}
	}

	private void setSprite () {
		switch (elementType) {
			case ElementType.FIRE: render.sprite = fireElement; break;
			case ElementType.WATER: render.sprite = waterElement; break;
			case ElementType.EARTH: render.sprite = earthElement; break;
			case ElementType.AIR: render.sprite = airElement; break;
			case ElementType.LIGHT: render.sprite = lightElement; break;
			case ElementType.DARK: render.sprite = darkElement; break;
			default: Debug.Log("Unknown element type"); break;
		}
	}

	public void setRowAndColumn (int row, int column) {
		this.row = row;
		this.column = column;
	}

	public int getRow () {
		return row;
	}

	public int getColumn () {
		return column;
	}

	public void setCellCenter (Vector3 cellCenter) {
		this.cellCenter = cellCenter;
	}

	public Vector3 getCellCenter () {
		return cellCenter;
	}

	public void setTarget (Vector3 target) {
		this.target = target;
	}

	public Vector3 getTarget () {
		return target;
	}

	public void setFadeOut () {
		fadeOut = true;
	}

	public bool isGoToTarget () {
		return goToTarget;
	}

	public void setGoToTarget () {
		this.goToTarget = true;
		x = trans.localPosition.x;
		y = trans.localPosition.y;
	}

	public void setActive (bool active) {
		col.enabled = active;
	}
}