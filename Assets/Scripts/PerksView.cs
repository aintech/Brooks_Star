using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PerksView : MonoBehaviour {

	private StatusScreen statusScreen;

	private List<Perk> perks = new List<Perk>();

	private Transform info;

	private TextMesh perkName, perkDescription;

	private Perk choosedPerk;

	public PerksView init (StatusScreen statusScreen) {
		this.statusScreen = statusScreen;

		for (int i = 0; i < transform.childCount; i++) {
			if (transform.GetChild(i).GetComponent<Perk>() == null) { continue; }
			perks.Add(transform.GetChild(i).GetComponent<Perk>().init());
		}

		info = transform.Find("Info");
		perkName = info.Find("Perk Name").GetComponent<TextMesh>();
		perkDescription = info.Find("Perk Description").GetComponent<TextMesh>();

		MeshRenderer mesh = perkName.GetComponent<MeshRenderer>();
		mesh.sortingLayerName = "Inventory";
		mesh.sortingOrder = 6;
		mesh = perkDescription.GetComponent<MeshRenderer>();
		mesh.sortingLayerName = "Inventory";
		mesh.sortingOrder = 6;

		hideInfo();

		return this;
	}

	void Update () {
		if (Input.GetMouseButtonDown(0)) {
			if (choosedPerk != null && (Utils.hit == null || Utils.hit.GetComponent<Perk>() == null || Utils.hit.GetComponent<Perk>() != choosedPerk)) {
				hideInfo();
			}
			if (choosedPerk == null && Utils.hit != null && Utils.hit.GetComponent<Perk>() != null) {
				choosedPerk = Utils.hit.GetComponent<Perk>();
				showPerkInfo(choosedPerk);
			}
		}
	}

	public void updatePerks () {
		foreach (Perk perk in perks) {
			perk.updatePerk();
		}
	}

	public void showPerkInfo (Perk perk) {
		statusScreen.hideItemInfo(null);
		perkName.text = perk.perkType.getName();
		perkDescription.text = perk.perkType.getDescription() + " <color=lime>" + (perk.perkType.getValuePerLevel() * Player.getPerkLevel(perk.perkType)) + "%</color>";
		perk.setAsChoosed(true);
		info.gameObject.SetActive(true);
	}

	public void hideInfo () {
		if (choosedPerk != null) {
			choosedPerk.setAsChoosed(false);
			choosedPerk = null;
		}
		info.gameObject.SetActive(false);
	}
}