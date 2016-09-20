using UnityEngine;
using System.Collections;

public class Messenger : MonoBehaviour {

	private static UserInterface userInterface;

	public void init (UserInterface userInterface) {
		Messenger.userInterface = userInterface;
	}

	public static void showMessage (string message) {
//		Debug.Log(message);
		userInterface.setMessageText(message);
	}
}