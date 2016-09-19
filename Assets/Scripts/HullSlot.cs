using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class HullSlot : MonoBehaviour {

	public Type slotType;

    public int index;

	public Sprite[] hullSlotSprites;

	private SpriteRenderer render;

	public Item item { get; private set; }
	
	private static Dictionary<HullType, List<Vector3>> slotPositions;

	private static Dictionary<HullType, List<bool>> slotAvailables;

	private bool slotAvailable;

	void Awake () {
		init ();
	}

	private void init () {
		setSprite (false);
	}

	public void setSprite (bool cellActive) {
		if (render == null) render = gameObject.GetComponent<SpriteRenderer> ();
		switch (slotType) {
			case Type.WEAPON: render.sprite = cellActive? hullSlotSprites[8]: hullSlotSprites[0]; break;
			case Type.ENGINE: render.sprite = cellActive? hullSlotSprites[9]: hullSlotSprites[1]; break;
			case Type.ARMOR: render.sprite = cellActive? hullSlotSprites[10]: hullSlotSprites[2]; break;
			case Type.GENERATOR: render.sprite = cellActive? hullSlotSprites[11]: hullSlotSprites[3]; break;
			case Type.RADAR: render.sprite = cellActive? hullSlotSprites[12]: hullSlotSprites[4]; break;
			case Type.SHIELD: render.sprite = cellActive? hullSlotSprites[13]: hullSlotSprites[5]; break;
			case Type.REPAIR_DROID: render.sprite = cellActive? hullSlotSprites[14]: hullSlotSprites[6]; break;
			case Type.HARVESTER: render.sprite = cellActive? hullSlotSprites[15]: hullSlotSprites[7]; break;
		}
	}

	public void setItem (Item item) {
		item.hullSlot = this;
		this.item = item;
	}

	public Item takeItem () {
		Item itemRef = item;
		item.hullSlot = null;
		item = null;
		return itemRef;
	}

	public void setSlotAvailable (bool slotAvailable) {
		this.slotAvailable = slotAvailable;
		gameObject.SetActive(slotAvailable);
	}

	public bool isSlotAvailable () {
		return slotAvailable;
	}

	public static Vector3 getSlotPosition (HullType hullType, Type slotType, int index) {
		if (slotPositions == null) {
			slotPositions = new Dictionary<HullType, List<Vector3>>();
            foreach (HullType hType in Enum.GetValues(typeof(HullType))) {
                List<Vector3> pos = new List<Vector3>();
                fillPositions(hType, pos);
                slotPositions.Add(hType, pos);
            }
		}

		List<Vector3> points = slotPositions[hullType];
        return points[getStartIndex(slotType) + index];
	}

    private static int getStartIndex (HullSlot.Type slotType) {
        switch (slotType) {
            case Type.RADAR: return 0;
            case Type.ENGINE: return 1;
            case Type.GENERATOR: return 2;
            case Type.HARVESTER: return 5;
            case Type.REPAIR_DROID: return 7;
            case Type.SHIELD: return 11;
            case Type.WEAPON: return 14;
            case Type.ARMOR: return 19;
            default: Debug.Log("Unknown type: " + slotType); return 0;
        }
    }

	public static bool checkSlotAvailability (HullType hullType, HullSlot.Type slotType, int index) {
		if (slotAvailables == null) {
			slotAvailables = new Dictionary<HullType, List<bool>>();
            foreach (HullType hType in Enum.GetValues(typeof(HullType))) {
                List<bool> avail = new List<bool>();
                fillAvailable(hType, avail);
                slotAvailables.Add(hType, avail);
            }
		}
		
		List<bool> avails = slotAvailables[hullType];
        return avails[getStartIndex(slotType) + index];
	}

    private static void fillPositions(HullType hullType, List<Vector3> vectors) {
        switch (hullType) {
            case HullType.Little:
            vectors.Add(new Vector3(0.6f, -1.15f));//radar
            vectors.Add(new Vector3(0, 2.2f));//engine
            vectors.Add(new Vector3(-0.6f, -1.15f));//generator 1
            vectors.Add(Vector3.zero);//generator 2
            vectors.Add(Vector3.zero);//generator 3
            vectors.Add(new Vector3(-1.15f, 0.55f));//harvester 1
            vectors.Add(Vector3.zero);//harvester 2
            vectors.Add(new Vector3(0f, 1.1f));//repairDroid 1
            vectors.Add(Vector3.zero);//repairDroid 2
            vectors.Add(Vector3.zero);//repairDroid 3
            vectors.Add(Vector3.zero);//repairDroid 4
            vectors.Add(new Vector3(1.15f, 0.55f));//shield 1
            vectors.Add(Vector3.zero);//shield 2
            vectors.Add(Vector3.zero);//shield 3
            vectors.Add(Vector3.zero);//weapon 1
            vectors.Add(Vector3.zero);//weapon 2
            vectors.Add(Vector3.zero);//weapon 3
            vectors.Add(Vector3.zero);//weapon 4
            vectors.Add(Vector3.zero);//weapon 5
            vectors.Add(new Vector3(0, 0));//armor 1
            vectors.Add(Vector3.zero);//armor 2
            vectors.Add(Vector3.zero);//armor 3
            vectors.Add(Vector3.zero);//armor 4
            vectors.Add(Vector3.zero);//armor 5
            break;
            case HullType.Needle:
            vectors.Add(new Vector3(0.6f, -1.15f));//radar
            vectors.Add(new Vector3(0, 2.2f));//engine
            vectors.Add(new Vector3(-0.6f, -1.15f));//generator 1
            vectors.Add(Vector3.zero);//generator 2
            vectors.Add(Vector3.zero);//generator 3
            vectors.Add(new Vector3(-1.15f, 0.55f));//harvester 1
            vectors.Add(Vector3.zero);//harvester 2
            vectors.Add(new Vector3(0f, 1.1f));//repairDroid 1
            vectors.Add(Vector3.zero);//repairDroid 2
            vectors.Add(Vector3.zero);//repairDroid 3
            vectors.Add(Vector3.zero);//repairDroid 4
            vectors.Add(new Vector3(1.15f, 0.55f));//shield 1
            vectors.Add(Vector3.zero);//shield 2
            vectors.Add(Vector3.zero);//shield 3
            vectors.Add(Vector3.zero);//weapon 1
            vectors.Add(Vector3.zero);//weapon 2
            vectors.Add(Vector3.zero);//weapon 3
            vectors.Add(Vector3.zero);//weapon 4
            vectors.Add(Vector3.zero);//weapon 5
            vectors.Add(new Vector3(0, 0));//armor 1
            vectors.Add(Vector3.zero);//armor 2
            vectors.Add(Vector3.zero);//armor 3
            vectors.Add(Vector3.zero);//armor 4
            vectors.Add(Vector3.zero);//armor 5
            break;
            case HullType.Gnome:
            vectors.Add(new Vector3(0.6f, -1.15f));//radar
            vectors.Add(new Vector3(0, 2.2f));//engine
            vectors.Add(new Vector3(-0.6f, -1.15f));//generator 1
            vectors.Add(Vector3.zero);//generator 2
            vectors.Add(Vector3.zero);//generator 3
            vectors.Add(new Vector3(-1.15f, 0.55f));//harvester 1
            vectors.Add(Vector3.zero);//harvester 2
            vectors.Add(new Vector3(0f, 1.1f));//repairDroid 1
            vectors.Add(Vector3.zero);//repairDroid 2
            vectors.Add(Vector3.zero);//repairDroid 3
            vectors.Add(Vector3.zero);//repairDroid 4
            vectors.Add(new Vector3(1.15f, 0.55f));//shield 1
            vectors.Add(Vector3.zero);//shield 2
            vectors.Add(Vector3.zero);//shield 3
            vectors.Add(Vector3.zero);//weapon 1
            vectors.Add(Vector3.zero);//weapon 2
            vectors.Add(Vector3.zero);//weapon 3
            vectors.Add(Vector3.zero);//weapon 4
            vectors.Add(Vector3.zero);//weapon 5
            vectors.Add(new Vector3(0, 0));//armor 1
            vectors.Add(Vector3.zero);//armor 2
            vectors.Add(Vector3.zero);//armor 3
            vectors.Add(Vector3.zero);//armor 4
            vectors.Add(Vector3.zero);//armor 5
            break;
            case HullType.Cricket:
            vectors.Add(new Vector3(0, -2.2f));//radar
            vectors.Add(new Vector3(0, 2.2f));//engine
            vectors.Add(new Vector3(0, -1.1f));//generator 1
            vectors.Add(Vector3.zero);//generator 2
            vectors.Add(Vector3.zero);//generator 3
            vectors.Add(new Vector3(-1.15f, 1.1f));//harvester 1
            vectors.Add(Vector3.zero);//harvester 2
            vectors.Add(new Vector3(1.15f, 1.1f));//repairDroid 1
            vectors.Add(Vector3.zero);//repairDroid 2
            vectors.Add(Vector3.zero);//repairDroid 3
            vectors.Add(Vector3.zero);//repairDroid 4
            vectors.Add(new Vector3(0, 0));//shield 1
            vectors.Add(Vector3.zero);//shield 2
            vectors.Add(Vector3.zero);//shield 3
            vectors.Add(new Vector3(0, 1.1f));//weapon 1
            vectors.Add(Vector3.zero);//weapon 2
            vectors.Add(Vector3.zero);//weapon 3
            vectors.Add(Vector3.zero);//weapon 4
            vectors.Add(Vector3.zero);//weapon 5
            vectors.Add(new Vector3(1.15f, 0));//armor 1
            vectors.Add(new Vector3(-1.15f, 0));//armor 2
            vectors.Add(Vector3.zero);//armor 3
            vectors.Add(Vector3.zero);//armor 4
            vectors.Add(Vector3.zero);//armor 5
            break;
            case HullType.Argo:
            vectors.Add(new Vector3(0, -2.1f));//radar
            vectors.Add(new Vector3(0, 2.4f));//engine
            vectors.Add(new Vector3(0, -0.95f));//generator 1
            vectors.Add(Vector3.zero);//generator 2
            vectors.Add(Vector3.zero);//generator 3
            vectors.Add(new Vector3(-1.15f, 1.3f));//harvester 1
            vectors.Add(Vector3.zero);//harvester 2
            vectors.Add(new Vector3(1.15f, 1.3f));//repairDroid 1
            vectors.Add(Vector3.zero);//repairDroid 2
            vectors.Add(Vector3.zero);//repairDroid 3
            vectors.Add(Vector3.zero);//repairDroid 4
            vectors.Add(new Vector3(0, 0.2f));//shield 1
            vectors.Add(Vector3.zero);//shield 2
            vectors.Add(Vector3.zero);//shield 3
            vectors.Add(new Vector3(0, 1.3f));//weapon 1
            vectors.Add(Vector3.zero);//weapon 2
            vectors.Add(Vector3.zero);//weapon 3
            vectors.Add(Vector3.zero);//weapon 4
            vectors.Add(Vector3.zero);//weapon 5
            vectors.Add(new Vector3(1.15f, 0.2f));//armor 1
            vectors.Add(new Vector3(-1.15f, 0.2f));//armor 2
            vectors.Add(Vector3.zero);//armor 3
            vectors.Add(Vector3.zero);//armor 4
            vectors.Add(Vector3.zero);//armor 5
            break;
            case HullType.Falcon:
            vectors.Add(new Vector3(0, -2.1f));//radar
            vectors.Add(new Vector3(0, 2.4f));//engine
            vectors.Add(new Vector3(0, -0.95f));//generator 1
            vectors.Add(Vector3.zero);//generator 2
            vectors.Add(Vector3.zero);//generator 3
            vectors.Add(new Vector3(-1.15f, 1.3f));//harvester 1
            vectors.Add(Vector3.zero);//harvester 2
            vectors.Add(new Vector3(1.15f, 1.3f));//repairDroid 1
            vectors.Add(Vector3.zero);//repairDroid 2
            vectors.Add(Vector3.zero);//repairDroid 3
            vectors.Add(Vector3.zero);//repairDroid 4
            vectors.Add(new Vector3(0, 0.2f));//shield 1
            vectors.Add(Vector3.zero);//shield 2
            vectors.Add(Vector3.zero);//shield 3
            vectors.Add(new Vector3(0, 1.3f));//weapon 1
            vectors.Add(Vector3.zero);//weapon 2
            vectors.Add(Vector3.zero);//weapon 3
            vectors.Add(Vector3.zero);//weapon 4
            vectors.Add(Vector3.zero);//weapon 5
            vectors.Add(new Vector3(1.15f, 0.2f));//armor 1
            vectors.Add(new Vector3(-1.15f, 0.2f));//armor 2
            vectors.Add(Vector3.zero);//armor 3
            vectors.Add(Vector3.zero);//armor 4
            vectors.Add(Vector3.zero);//armor 5
            break;
            case HullType.Adventurer:
            vectors.Add(new Vector3(0, -2.2f));//radar
            vectors.Add(new Vector3(0, 2.2f));//engine
            vectors.Add(new Vector3(0, 0));//generator 1
            vectors.Add(Vector3.zero);//generator 2
            vectors.Add(Vector3.zero);//generator 3
            vectors.Add(new Vector3(-1.1f, 2.2f));//harvester 1
            vectors.Add(Vector3.zero);//harvester 2
            vectors.Add(new Vector3(1.1f, 0));//repairDroid 1
            vectors.Add(new Vector3(-1.1f, 0));//repairDroid 2
            vectors.Add(Vector3.zero);//repairDroid 3
            vectors.Add(Vector3.zero);//repairDroid 4
            vectors.Add(new Vector3(1.1f, 2.2f));//shield 1
            vectors.Add(Vector3.zero);//shield 2
            vectors.Add(Vector3.zero);//shield 3
            vectors.Add(new Vector3(1.4f, 1.1f));//weapon 1
            vectors.Add(new Vector3(-1.4f, 1.1f));//weapon 2
            vectors.Add(Vector3.zero);//weapon 3
            vectors.Add(Vector3.zero);//weapon 4
            vectors.Add(Vector3.zero);//weapon 5
            vectors.Add(new Vector3(0.7f, -1.1f));//armor 1
            vectors.Add(new Vector3(-0.7f, -1.1f));//armor 2
            vectors.Add(new Vector3(0, 1.1f));//armor 3
            vectors.Add(Vector3.zero);//armor 4
            vectors.Add(Vector3.zero);//armor 5
            break;
            case HullType.Corvette:
            vectors.Add(new Vector3(0, -2.2f));//radar
            vectors.Add(new Vector3(0, 2.2f));//engine
            vectors.Add(new Vector3(0, 0));//generator 1
            vectors.Add(Vector3.zero);//generator 2
            vectors.Add(Vector3.zero);//generator 3
            vectors.Add(new Vector3(-1.1f, 2.2f));//harvester 1
            vectors.Add(Vector3.zero);//harvester 2
            vectors.Add(new Vector3(1.1f, 0));//repairDroid 1
            vectors.Add(new Vector3(-1.1f, 0));//repairDroid 2
            vectors.Add(Vector3.zero);//repairDroid 3
            vectors.Add(Vector3.zero);//repairDroid 4
            vectors.Add(new Vector3(1.1f, 2.2f));//shield 1
            vectors.Add(Vector3.zero);//shield 2
            vectors.Add(Vector3.zero);//shield 3
            vectors.Add(new Vector3(1.4f, 1.1f));//weapon 1
            vectors.Add(new Vector3(-1.4f, 1.1f));//weapon 2
            vectors.Add(Vector3.zero);//weapon 3
            vectors.Add(Vector3.zero);//weapon 4
            vectors.Add(Vector3.zero);//weapon 5
            vectors.Add(new Vector3(0.7f, -1.1f));//armor 1
            vectors.Add(new Vector3(-0.7f, -1.1f));//armor 2
            vectors.Add(new Vector3(0, 1.1f));//armor 3
            vectors.Add(Vector3.zero);//armor 4
            vectors.Add(Vector3.zero);//armor 5
            break;
            case HullType.Buffalo:
            vectors.Add(new Vector3(0, -2.2f));//radar
            vectors.Add(new Vector3(0, 2.2f));//engine
            vectors.Add(new Vector3(0, 0));//generator 1
            vectors.Add(Vector3.zero);//generator 2
            vectors.Add(Vector3.zero);//generator 3
            vectors.Add(new Vector3(-1.1f, 2.2f));//harvester 1
            vectors.Add(Vector3.zero);//harvester 2
            vectors.Add(new Vector3(1.1f, 0));//repairDroid 1
            vectors.Add(new Vector3(-1.1f, 0));//repairDroid 2
            vectors.Add(Vector3.zero);//repairDroid 3
            vectors.Add(Vector3.zero);//repairDroid 4
            vectors.Add(new Vector3(1.1f, 2.2f));//shield 1
            vectors.Add(Vector3.zero);//shield 2
            vectors.Add(Vector3.zero);//shield 3
            vectors.Add(new Vector3(1.4f, 1.1f));//weapon 1
            vectors.Add(new Vector3(-1.4f, 1.1f));//weapon 2
            vectors.Add(Vector3.zero);//weapon 3
            vectors.Add(Vector3.zero);//weapon 4
            vectors.Add(Vector3.zero);//weapon 5
            vectors.Add(new Vector3(0.7f, -1.1f));//armor 1
            vectors.Add(new Vector3(-0.7f, -1.1f));//armor 2
            vectors.Add(new Vector3(0, 1.1f));//armor 3
            vectors.Add(Vector3.zero);//armor 4
            vectors.Add(Vector3.zero);//armor 5
            break;
            case HullType.Legionnaire:
            vectors.Add(new Vector3(0, -2.2f));//radar
            vectors.Add(new Vector3(0, 2.5f));//engine
            vectors.Add(new Vector3(0, 0.3f));//generator 1
            vectors.Add(Vector3.zero);//generator 2
            vectors.Add(Vector3.zero);//generator 3
            vectors.Add(new Vector3(-1.1f, 2.3f));//harvester 1
            vectors.Add(Vector3.zero);//harvester 2
            vectors.Add(new Vector3(1.1f, 0.1f));//repairDroid 1
            vectors.Add(new Vector3(-1.1f, 0.1f));//repairDroid 2
            vectors.Add(Vector3.zero);//repairDroid 3
            vectors.Add(Vector3.zero);//repairDroid 4
            vectors.Add(new Vector3(1.1f, 2.3f));//shield 1
            vectors.Add(Vector3.zero);//shield 2
            vectors.Add(Vector3.zero);//shield 3
            vectors.Add(new Vector3(1.4f, 1.2f));//weapon 1
            vectors.Add(new Vector3(-1.4f, 1.2f));//weapon 2
            vectors.Add(Vector3.zero);//weapon 3
            vectors.Add(Vector3.zero);//weapon 4
            vectors.Add(Vector3.zero);//weapon 5
            vectors.Add(new Vector3(0.7f, -1));//armor 1
            vectors.Add(new Vector3(-0.7f, -1));//armor 2
            vectors.Add(new Vector3(0, 1.4f));//armor 3
            vectors.Add(Vector3.zero);//armor 4
            vectors.Add(Vector3.zero);//armor 5
            break;
            case HullType.StarWalker:
            vectors.Add(new Vector3(0, -2.2f));//radar
            vectors.Add(new Vector3(0, 2.75f));//engine
            vectors.Add(new Vector3(1.15f, 0.35f));//generator 1
            vectors.Add(new Vector3(-1.15f, 0.35f));//generator 2
            vectors.Add(Vector3.zero);//generator 3
            vectors.Add(new Vector3(1.15f, 2.55f));//harvester 1
            vectors.Add(new Vector3(-1.15f, 2.55f));//harvester 2
            vectors.Add(new Vector3(0.65f, -0.75f));//repairDroid 1
            vectors.Add(new Vector3(-0.65f, -0.75f));//repairDroid 2
            vectors.Add(new Vector3(0, 0.45f));//repairDroid 3
            vectors.Add(Vector3.zero);//repairDroid 4
            vectors.Add(new Vector3(1.15f, 1.45f));//shield 1
            vectors.Add(new Vector3(-1.15f, 1.45f));//shield 2
            vectors.Add(Vector3.zero);//shield 3
            vectors.Add(new Vector3(1.75f, -0.75f));//weapon 1
            vectors.Add(new Vector3(-1.75f, -0.75f));//weapon 2
            vectors.Add(new Vector3(0, 1.65f));//weapon 3
            vectors.Add(Vector3.zero);//weapon 4
            vectors.Add(Vector3.zero);//weapon 5
            vectors.Add(new Vector3(1.15f, -1.85f));//armor 1
            vectors.Add(new Vector3(-1.15f, -1.85f));//armor 2
            vectors.Add(new Vector3(2.2f, 1.85f));//armor 3
            vectors.Add(new Vector3(-2.2f, 1.85f));//armor 4
            vectors.Add(Vector3.zero);//armor 5
            break;
            case HullType.Warship:
            vectors.Add(new Vector3(0, -2.2f));//radar
            vectors.Add(new Vector3(0, 2.75f));//engine
            vectors.Add(new Vector3(1.15f, 0.35f));//generator 1
            vectors.Add(new Vector3(-1.15f, 0.35f));//generator 2
            vectors.Add(Vector3.zero);//generator 3
            vectors.Add(new Vector3(1.15f, 2.55f));//harvester 1
            vectors.Add(new Vector3(-1.15f, 2.55f));//harvester 2
            vectors.Add(new Vector3(0.65f, -0.75f));//repairDroid 1
            vectors.Add(new Vector3(-0.65f, -0.75f));//repairDroid 2
            vectors.Add(new Vector3(0, 0.45f));//repairDroid 3
            vectors.Add(Vector3.zero);//repairDroid 4
            vectors.Add(new Vector3(1.15f, 1.45f));//shield 1
            vectors.Add(new Vector3(-1.15f, 1.45f));//shield 2
            vectors.Add(Vector3.zero);//shield 3
            vectors.Add(new Vector3(1.75f, -0.75f));//weapon 1
            vectors.Add(new Vector3(-1.75f, -0.75f));//weapon 2
            vectors.Add(new Vector3(0, 1.65f));//weapon 3
            vectors.Add(Vector3.zero);//weapon 4
            vectors.Add(Vector3.zero);//weapon 5
            vectors.Add(new Vector3(1.15f, -1.85f));//armor 1
            vectors.Add(new Vector3(-1.15f, -1.85f));//armor 2
            vectors.Add(new Vector3(2.2f, 1.85f));//armor 3
            vectors.Add(new Vector3(-2.2f, 1.85f));//armor 4
            vectors.Add(Vector3.zero);//armor 5
            break;
            case HullType.Asterix:
            vectors.Add(new Vector3(0, -2.2f));//radar
            vectors.Add(new Vector3(0, 2.75f));//engine
            vectors.Add(new Vector3(1.15f, 0.35f));//generator 1
            vectors.Add(new Vector3(-1.15f, 0.35f));//generator 2
            vectors.Add(Vector3.zero);//generator 3
            vectors.Add(new Vector3(1.15f, 2.55f));//harvester 1
            vectors.Add(new Vector3(-1.15f, 2.55f));//harvester 2
            vectors.Add(new Vector3(0.65f, -0.75f));//repairDroid 1
            vectors.Add(new Vector3(-0.65f, -0.75f));//repairDroid 2
            vectors.Add(new Vector3(0, 0.45f));//repairDroid 3
            vectors.Add(Vector3.zero);//repairDroid 4
            vectors.Add(new Vector3(1.15f, 1.45f));//shield 1
            vectors.Add(new Vector3(-1.15f, 1.45f));//shield 2
            vectors.Add(Vector3.zero);//shield 3
            vectors.Add(new Vector3(1.9f, -0.75f));//weapon 1
            vectors.Add(new Vector3(-1.9f, -0.75f));//weapon 2
            vectors.Add(new Vector3(0, 1.65f));//weapon 3
            vectors.Add(Vector3.zero);//weapon 4
            vectors.Add(Vector3.zero);//weapon 5
            vectors.Add(new Vector3(1.15f, -1.85f));//armor 1
            vectors.Add(new Vector3(-1.15f, -1.85f));//armor 2
            vectors.Add(new Vector3(2.2f, 1.85f));//armor 3
            vectors.Add(new Vector3(-2.2f, 1.85f));//armor 4
            vectors.Add(Vector3.zero);//armor 5
            break;
            case HullType.Prime:
            vectors.Add(new Vector3(0, -2.3f));//radar
            vectors.Add(new Vector3(0, 2.5f));//engine
            vectors.Add(new Vector3(1.15f, 0.1f));//generator 1
            vectors.Add(new Vector3(-1.15f, 0.1f));//generator 2
            vectors.Add(Vector3.zero);//generator 3
            vectors.Add(new Vector3(1.15f, 2.25f));//harvester 1
            vectors.Add(new Vector3(-1.15f, 2.25f));//harvester 2
            vectors.Add(new Vector3(0.65f, -1));//repairDroid 1
            vectors.Add(new Vector3(-0.65f, -1));//repairDroid 2
            vectors.Add(new Vector3(0, 0.2f));//repairDroid 3
            vectors.Add(Vector3.zero);//repairDroid 4
            vectors.Add(new Vector3(1.15f, 1.2f));//shield 1
            vectors.Add(new Vector3(-1.15f, 1.2f));//shield 2
            vectors.Add(Vector3.zero);//shield 3
            vectors.Add(new Vector3(1.9f, -1));//weapon 1
            vectors.Add(new Vector3(-1.9f, -1));//weapon 2
            vectors.Add(new Vector3(0, 1.4f));//weapon 3
            vectors.Add(Vector3.zero);//weapon 4
            vectors.Add(Vector3.zero);//weapon 5
            vectors.Add(new Vector3(1.15f, -2.1f));//armor 1
            vectors.Add(new Vector3(-1.15f, -2.1f));//armor 2
            vectors.Add(new Vector3(2.2f, 1.6f));//armor 3
            vectors.Add(new Vector3(-2.2f, 1.6f));//armor 4
            vectors.Add(Vector3.zero);//armor 5
            break;
            case HullType.Titan:
            vectors.Add(new Vector3(0, -2.3f));//radar
            vectors.Add(new Vector3(0, 2.4f));//engine
            vectors.Add(new Vector3(0, -1.1f));//generator 1
            vectors.Add(new Vector3(0, 0.05f));//generator 2
            vectors.Add(Vector3.zero);//generator 3
            vectors.Add(new Vector3(1.15f, 1.5f));//harvester 1
            vectors.Add(new Vector3(-1.15f, 1.5f));//harvester 2
            vectors.Add(new Vector3(1.15f, -0.8f));//repairDroid 1
            vectors.Add(new Vector3(-1.15f, -0.8f));//repairDroid 2
            vectors.Add(new Vector3(1.15f, 2.6f));//repairDroid 3
            vectors.Add(new Vector3(-1.15f, 2.6f));//repairDroid 4
            vectors.Add(new Vector3(1.15f, 0.35f));//shield 1
            vectors.Add(new Vector3(-1.15f, 0.35f));//shield 2
            vectors.Add(Vector3.zero);//shield 3
            vectors.Add(new Vector3(2.3f, -0.65f));//weapon 1
            vectors.Add(new Vector3(-2.3f, -0.65f));//weapon 2
            vectors.Add(new Vector3(2.3f, 2.35f));//weapon 3
            vectors.Add(new Vector3(-2.3f, 2.35f));//weapon 4
            vectors.Add(Vector3.zero);//weapon 5
            vectors.Add(new Vector3(1.15f, -2));//armor 1
            vectors.Add(new Vector3(-1.15f, -2));//armor 2
            vectors.Add(new Vector3(2.3f, 0.8f));//armor 3
            vectors.Add(new Vector3(-2.3f, 0.8f));//armor 4
            vectors.Add(new Vector3(0, 1.2f));//armor 5
            break;
            case HullType.Dreadnaut:
            vectors.Add(new Vector3(0, -2.9f));//radar
            vectors.Add(new Vector3(0, 2.8f));//engine
            vectors.Add(new Vector3(0.7f, -1.7f));//generator 1
            vectors.Add(new Vector3(-0.7f, -1.7f));//generator 2
            vectors.Add(new Vector3(0, -0.5f));//generator 3
            vectors.Add(new Vector3(1.15f, 1.7f));//harvester 1
            vectors.Add(new Vector3(-1.15f, 1.7f));//harvester 2
            vectors.Add(new Vector3(1.15f, -0.6f));//repairDroid 1
            vectors.Add(new Vector3(-1.15f, -0.6f));//repairDroid 2
            vectors.Add(new Vector3(1.15f, 2.8f));//repairDroid 3
            vectors.Add(new Vector3(-1.15f, 2.8f));//repairDroid 4
            vectors.Add(new Vector3(1.15f, 0.55f));//shield 1
            vectors.Add(new Vector3(-1.15f, 0.55f));//shield 2
            vectors.Add(Vector3.zero);//shield 3
            vectors.Add(new Vector3(2.3f, -0.6f));//weapon 1
            vectors.Add(new Vector3(-2.3f, -0.6f));//weapon 2
            vectors.Add(new Vector3(2.3f, 2.2f));//weapon 3
            vectors.Add(new Vector3(-2.3f, 2.2f));//weapon 4
            vectors.Add(Vector3.zero);//weapon 5
            vectors.Add(new Vector3(2.25f, -1.7f));//armor 1
            vectors.Add(new Vector3(-2.25f, -1.7f));//armor 2
            vectors.Add(new Vector3(2.3f, 0.9f));//armor 3
            vectors.Add(new Vector3(-2.3f, 0.9f));//armor 4
            vectors.Add(new Vector3(0, 1));//armor 5
            break;
            case HullType.Armageddon:
            vectors.Add(new Vector3(0, -2.9f));//radar
            vectors.Add(new Vector3(0, 2.8f));//engine
            vectors.Add(new Vector3(1.15f, -1.7f));//generator 1
            vectors.Add(new Vector3(0, -1.8f));//generator 2
            vectors.Add(new Vector3(-1.15f, -1.7f));//generator 3
            vectors.Add(new Vector3(1.15f, 1.7f));//harvester 1
            vectors.Add(new Vector3(-1.15f, 1.7f));//harvester 2
            vectors.Add(new Vector3(1.15f, -0.6f));//repairDroid 1
            vectors.Add(new Vector3(-1.15f, -0.6f));//repairDroid 2
            vectors.Add(new Vector3(1.15f, 2.8f));//repairDroid 3
            vectors.Add(new Vector3(-1.15f, 2.8f));//repairDroid 4
            vectors.Add(new Vector3(0, -0.7f));//shield 1
            vectors.Add(new Vector3(1.15f, 0.55f));//shield 2
            vectors.Add(new Vector3(-1.15f, 0.55f));//shield 3
            vectors.Add(new Vector3(2.3f, -1.4f));//weapon 1
            vectors.Add(new Vector3(-2.3f, -1.4f));//weapon 2
            vectors.Add(new Vector3(0, 0.4f));//weapon 3
            vectors.Add(new Vector3(2.3f, 2.2f));//weapon 4
            vectors.Add(new Vector3(-2.3f, 2.2f));//weapon 5
            vectors.Add(new Vector3(1.95f, -2.8f));//armor 1
            vectors.Add(new Vector3(-1.95f, -2.8f));//armor 2
            vectors.Add(new Vector3(2.3f, 0.5f));//armor 3
            vectors.Add(new Vector3(-2.3f, 0.5f));//armor 4
            vectors.Add(new Vector3(0, 1.6f));//armor 5
            break;
            default:
            vectors.Add(Vector3.zero);
            vectors.Add(Vector3.zero);
            vectors.Add(Vector3.zero);
            vectors.Add(Vector3.zero);
            vectors.Add(Vector3.zero);
            vectors.Add(Vector3.zero);
            vectors.Add(Vector3.zero);
            vectors.Add(Vector3.zero);
            vectors.Add(Vector3.zero);
            vectors.Add(Vector3.zero);
            vectors.Add(Vector3.zero);
            vectors.Add(Vector3.zero);
            vectors.Add(Vector3.zero);
            vectors.Add(Vector3.zero);
            vectors.Add(Vector3.zero);
            vectors.Add(Vector3.zero);
            vectors.Add(Vector3.zero);
            vectors.Add(Vector3.zero);
            vectors.Add(Vector3.zero);
            vectors.Add(Vector3.zero);
            vectors.Add(Vector3.zero);
            vectors.Add(Vector3.zero);
            vectors.Add(Vector3.zero);
            vectors.Add(Vector3.zero);
            break;
        }
    }

    private static void fillAvailable (HullType hullType, List<bool> avails) {
		avails.Add(true);// radar
		avails.Add(true);// engine
		avails.Add(true);// generator 1
		avails.Add(hullType.getGeneratorSlots() >= 2);// generator 2
		avails.Add(hullType.getGeneratorSlots() >= 3);// generator 3
		avails.Add(true);// harvester 1
		avails.Add(hullType.getHarvesterSlots() >= 2);// harvester 2
		avails.Add(true);// repairDroid 1
		avails.Add(hullType.getRepairDroidSlots() >= 2);// repairDroid 2
		avails.Add(hullType.getRepairDroidSlots() >= 3);// repairDroid 3
		avails.Add(hullType.getRepairDroidSlots() >= 4);// repairDroid 4
		avails.Add(true);// shield 1
		avails.Add(hullType.getShieldSlots() >= 2);// shield 2
		avails.Add(hullType.getShieldSlots() >= 3);// shield 3
		avails.Add(hullType.getWeaponSlots() >= 1);// weapon 1
		avails.Add(hullType.getWeaponSlots() >= 2);// weapon 2
		avails.Add(hullType.getWeaponSlots() >= 3);// weapon 3
		avails.Add(hullType.getWeaponSlots() >= 4);// weapon 4
		avails.Add(hullType.getWeaponSlots() >= 5);// weapon 5
		avails.Add(true);// armor 1
		avails.Add(hullType.getArmorSlots() >= 2);// armor 2
		avails.Add(hullType.getArmorSlots() >= 3);// armor 3
		avails.Add(hullType.getArmorSlots() >= 4);// armor 4
		avails.Add(hullType.getArmorSlots() >= 5);// armor 5
	}

    public enum Type {
        WEAPON,
        ENGINE,
        ARMOR,
        GENERATOR,
        RADAR,
        SHIELD,
        REPAIR_DROID,
        HARVESTER
    }
}