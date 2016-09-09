using UnityEngine;
using System.Collections;

public class Menu : MonoBehaviour {

	public void startNewGame () {
		PlanetScene.newGame = true;
		Application.LoadLevel("Planet");
	}
}