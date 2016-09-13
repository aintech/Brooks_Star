using UnityEngine;
using System.Collections;

public class Messenger : MonoBehaviour {

	public static void showMessage (string message) {
		//Показываем сообщение внизу экрана
		Debug.Log(message);
	}
}