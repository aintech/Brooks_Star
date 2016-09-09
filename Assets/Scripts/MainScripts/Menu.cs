using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class Menu : MonoBehaviour {
	
	public void startNewGame () {
		Planet.newGame = true;
		SceneManager.LoadScene("Planet");
	}
}