using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HullSlot : MonoBehaviour {

	public HullSlotType hullSlotType;

	public Sprite[] hullSlotSprites;

	private SpriteRenderer render;

	private Item item;
	
	private static Dictionary<HullType, List<Vector3>> slotPositions;

	private static Dictionary<HullType, List<bool>> slotAvailables;

	private bool slotAvailable;

	public enum HullSlotType {
		Weapon,
		Engine,
		Armor,
		Generator,
		Radar,
		Shield,
		RepairDroid,
		Harvester
	}

	void Awake () {
		init ();
	}

	private void init () {
		setSprite (false);
	}

	public void setSprite (bool cellActive) {
		if (render == null) render = gameObject.GetComponent<SpriteRenderer> ();
		switch (hullSlotType) {
			case HullSlotType.Weapon: render.sprite = cellActive? hullSlotSprites[8]: hullSlotSprites[0]; break;
			case HullSlotType.Engine: render.sprite = cellActive? hullSlotSprites[9]: hullSlotSprites[1]; break;
			case HullSlotType.Armor: render.sprite = cellActive? hullSlotSprites[10]: hullSlotSprites[2]; break;
			case HullSlotType.Generator: render.sprite = cellActive? hullSlotSprites[11]: hullSlotSprites[3]; break;
			case HullSlotType.Radar: render.sprite = cellActive? hullSlotSprites[12]: hullSlotSprites[4]; break;
			case HullSlotType.Shield: render.sprite = cellActive? hullSlotSprites[13]: hullSlotSprites[5]; break;
			case HullSlotType.RepairDroid: render.sprite = cellActive? hullSlotSprites[14]: hullSlotSprites[6]; break;
			case HullSlotType.Harvester: render.sprite = cellActive? hullSlotSprites[15]: hullSlotSprites[7]; break;
		}
	}

	public void setItem (Item item) {
		item.setHullSlot(this);
		this.item = item;
	}

	public Item getItem () {
		return item;
	}

	public Item takeItem () {
		Item itemRef = getItem ();
		item.setHullSlot(null);
		item = null;
		return itemRef;
	}

	public HullSlotType getHullSlotType () {
		return hullSlotType;
	}

	public void setSlotAvailable (bool slotAvailable) {
		this.slotAvailable = slotAvailable;
		gameObject.SetActive(slotAvailable);
	}

	public bool isSlotAvailable () {
		return slotAvailable;
	}

	public static Vector3 getSlotPosition (HullType hullType, string slotName) {
		if (slotPositions == null) {
			slotPositions = new Dictionary<HullType, List<Vector3>>();
			slotPositions.Add(HullType.Little, new List<Vector3>());
			slotPositions.Add(HullType.Needle, new List<Vector3>());
			slotPositions.Add(HullType.Gnome, new List<Vector3>());
			slotPositions.Add(HullType.Cricket, new List<Vector3>());
			slotPositions.Add(HullType.Argo, new List<Vector3>());
			slotPositions.Add(HullType.Falcon, new List<Vector3>());
			slotPositions.Add(HullType.Adventurer, new List<Vector3>());
			slotPositions.Add(HullType.Corvette, new List<Vector3>());
			slotPositions.Add(HullType.Buffalo, new List<Vector3>());
			slotPositions.Add(HullType.Legionnaire, new List<Vector3>());
			slotPositions.Add(HullType.StarWalker, new List<Vector3>());
			slotPositions.Add(HullType.Warship, new List<Vector3>());
			slotPositions.Add(HullType.Asterix, new List<Vector3>());
			slotPositions.Add(HullType.Prime, new List<Vector3>());
			slotPositions.Add(HullType.Titan, new List<Vector3>());
			slotPositions.Add(HullType.Dreadnaut, new List<Vector3>());
			slotPositions.Add(HullType.Armageddon, new List<Vector3>());

			foreach (HullType huType in slotPositions.Keys) {
				List<Vector3> pos;
				slotPositions.TryGetValue(huType, out pos);
				fillPositions (huType, pos);
			}
		}

		List<Vector3> vectors;
		slotPositions.TryGetValue(hullType, out vectors);
		switch (slotName) {
			case "Radar Slot": return vectors[0];
			case "Engine Slot": return vectors[1];
			case "Generator Slot 1": return vectors[2];
			case "Generator Slot 2": return vectors[3];
			case "Generator Slot 3": return vectors[4];
			case "Harvester Slot 1": return vectors[5];
			case "Harvester Slot 2": return vectors[6];
			case "RepairDroid Slot 1": return vectors[7];
			case "RepairDroid Slot 2": return vectors[8];
			case "RepairDroid Slot 3": return vectors[9];
			case "RepairDroid Slot 4": return vectors[10];
			case "Shield Slot 1": return vectors[11];
			case "Shield Slot 2": return vectors[12];
			case "Shield Slot 3": return vectors[13];
			case "Weapon Slot 1": return vectors[14];
			case "Weapon Slot 2": return vectors[15];
			case "Weapon Slot 3": return vectors[16];
			case "Weapon Slot 4": return vectors[17];
			case "Weapon Slot 5": return vectors[18];
			case "Armor Slot 1": return vectors[19];
			case "Armor Slot 2": return vectors[20];
			case "Armor Slot 3": return vectors[21];
			case "Armor Slot 4": return vectors[22];
			case "Armor Slot 5": return vectors[23];
			default: return Vector2.zero;
		}
	}

	private static void fillPositions (HullType hullType, List<Vector3> vectors) {
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

	public static bool checkSlotAvailability (HullType hullType, string slotName) {
		if (slotAvailables == null) {
			slotAvailables = new Dictionary<HullType, List<bool>>();
			slotAvailables.Add(HullType.Little, new List<bool>());
			slotAvailables.Add(HullType.Needle, new List<bool>());
			slotAvailables.Add(HullType.Gnome, new List<bool>());
			slotAvailables.Add(HullType.Cricket, new List<bool>());
			slotAvailables.Add(HullType.Argo, new List<bool>());
			slotAvailables.Add(HullType.Falcon, new List<bool>());
			slotAvailables.Add(HullType.Adventurer, new List<bool>());
			slotAvailables.Add(HullType.Corvette, new List<bool>());
			slotAvailables.Add(HullType.Buffalo, new List<bool>());
			slotAvailables.Add(HullType.Legionnaire, new List<bool>());
			slotAvailables.Add(HullType.StarWalker, new List<bool>());
			slotAvailables.Add(HullType.Warship, new List<bool>());
			slotAvailables.Add(HullType.Asterix, new List<bool>());
			slotAvailables.Add(HullType.Prime, new List<bool>());
			slotAvailables.Add(HullType.Titan, new List<bool>());
			slotAvailables.Add(HullType.Dreadnaut, new List<bool>());
			slotAvailables.Add(HullType.Armageddon, new List<bool>());
			
			foreach (HullType huType in slotAvailables.Keys) {
				List<bool> avail;
				slotAvailables.TryGetValue(huType, out avail);
				fillAvailable (huType, avail);
			}
		}
		
		List<bool> avails;
		slotAvailables.TryGetValue(hullType, out avails);
		switch (slotName) {
			case "Radar Slot": return avails[0];
			case "Engine Slot": return avails[1];
			case "Generator Slot 1": return avails[2];
			case "Generator Slot 2": return avails[3];
			case "Generator Slot 3": return avails[4];
			case "Harvester Slot 1": return avails[5];
			case "Harvester Slot 2": return avails[6];
			case "RepairDroid Slot 1": return avails[7];
			case "RepairDroid Slot 2": return avails[8];
			case "RepairDroid Slot 3": return avails[9];
			case "RepairDroid Slot 4": return avails[10];
			case "Shield Slot 1": return avails[11];
			case "Shield Slot 2": return avails[12];
			case "Shield Slot 3": return avails[13];
			case "Weapon Slot 1": return avails[14];
			case "Weapon Slot 2": return avails[15];
			case "Weapon Slot 3": return avails[16];
			case "Weapon Slot 4": return avails[17];
			case "Weapon Slot 5": return avails[18];
			case "Armor Slot 1": return avails[19];
			case "Armor Slot 2": return avails[20];
			case "Armor Slot 3": return avails[21];
			case "Armor Slot 4": return avails[22];
			case "Armor Slot 5": return avails[23];
			default: return false;
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