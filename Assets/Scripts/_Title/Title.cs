using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class Title : MonoBehaviour {
	
	public void startNewGame () {
		PlanetSurface.newGame = true;
		SceneManager.LoadScene("PlanetSurface");
	}
}