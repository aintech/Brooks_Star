using UnityEngine;
using System.Collections;

public class SupplySlot : Slot {

	public int index;

	override public void init () {
		base.init();
		kind = ItemKind.SUPPLY;
	}
}