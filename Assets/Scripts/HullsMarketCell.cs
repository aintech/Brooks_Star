using UnityEngine;
using System.Collections;

public class HullsMarketCell : MonoBehaviour {

	public int index;

	private HullsMarketItem item;

	public void setItem (HullsMarketItem item) {
		this.item = item;
	}

	public HullsMarketItem getItem () {
		return item;
	}
}
