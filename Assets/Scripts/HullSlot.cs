using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class HullSlot : Slot {
	
    public int index;

	private static Dictionary<HullType, List<Vector3>> slotPositions;

	private static Dictionary<HullType, List<bool>> slotAvailables;

	private bool slotAvailable;

	override public void init () {
		base.init();
		kind = ItemKind.SHIP_EQUIPMENT;
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
            case HullType.LITTLE:
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
            case HullType.NEEDLE:
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
            case HullType.GNOME:
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
            case HullType.CRICKET:
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
            case HullType.ARGO:
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
            case HullType.FALCON:
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
            case HullType.ADVENTURER:
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
            case HullType.CORVETTE:
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
            case HullType.BUFFALO:
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
            case HullType.LEGIONNAIRE:
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
            case HullType.STARWALKER:
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
            case HullType.WARSHIP:
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
            case HullType.ASTERIX:
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
            case HullType.PRIME:
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
            case HullType.TITAN:
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
            case HullType.DREADNAUT:
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
            case HullType.ARMAGEDDON:
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
}