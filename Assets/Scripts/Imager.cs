using UnityEngine;
using System.Collections;

public class Imager : MonoBehaviour {

	public Texture portraitAlika, portraitRokot;

	public static Texture alikaPortrait, rokotPortrait;

	public void init () {
		alikaPortrait = portraitAlika;
		rokotPortrait = portraitRokot;
		gameObject.SetActive(false);
	}
}