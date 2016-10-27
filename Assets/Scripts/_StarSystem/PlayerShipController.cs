using UnityEngine;
using System.Collections;

public class PlayerShipController : ShipController {

	public bool inControl = true;

	override public void checkInput () {
		if (!ship.alive) {
			if (accelerate) { accelerate = false; }
			if (turnRight) { turnRight = false; }
			if (turnLeft) { turnLeft = false; }
			return;
		}

		if (inControl) {
			accelerate = Input.GetKey(KeyCode.W);
			turnLeft = Input.GetKey(KeyCode.A);
			turnRight = Input.GetKey(KeyCode.D);
			decelerate = Input.GetKey(KeyCode.S);
		}
	}

	override protected void Update () {
		if (inControl) { base.Update(); }
	}

	override protected void FixedUpdate () {
		if (inControl) { base.FixedUpdate(); }
	}
}