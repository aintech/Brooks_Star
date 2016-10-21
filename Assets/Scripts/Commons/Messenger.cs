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

	public static void inventoryCapacityLow (string itemName, int count) {
		userInterface.setMessageText("Объёма инвентаря не достаточно для добавления предмета(ов): " + (count == 1? "": count + " X ") + itemName);
	}

	public static void notEnoughtCash (string itemName, int count) {
		userInterface.setMessageText("Не достаточно кредитов на покупку: " + (count == 1? "": count + " X ") + itemName);
	}
}