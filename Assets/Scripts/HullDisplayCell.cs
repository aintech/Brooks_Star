using UnityEngine;
using System.Collections;

public class HullDisplayCell : MonoBehaviour {

	public int index;

	private HullDisplayItem item;

	public void setItem (HullDisplayItem item) {
		this.item = item;
	}

	public HullDisplayItem getItem () {
		return item;
	}
}
