using UnityEngine;
using System.Collections;

public class PlayerShipController : ShipController {
	
	override protected void checkInput () {
		accelarate = Input.GetKey(KeyCode.W);
		turnLeft = Input.GetKey(KeyCode.A);
		turnRight = Input.GetKey(KeyCode.D);
//		sPressed = Input.GetKey(KeyCode.S);
	}
}