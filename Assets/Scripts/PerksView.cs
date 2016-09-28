using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PerksView : MonoBehaviour {

	private List<Perk> perks = new List<Perk>();

	public PerksView init () {
		for (int i = 0; i < transform.childCount; i++) {
			if (transform.GetChild(i).GetComponent<Perk>() == null) { continue; }
			perks.Add(transform.GetChild(i).GetComponent<Perk>().init());
		}

		return this;
	}

	public void updatePerks () {
		foreach (Perk perk in perks) {
			perk.updatePerk();
		}
	}
}