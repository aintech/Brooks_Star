using UnityEngine;
using System.Collections;

public class ShipController : MonoBehaviour {
	
	private float mainPower = 0.0f;
	private float mainAcceleration;
	private float maxMainPower;
	private float maxBackwardPower;
	
	private float rotationPower = 0.0f;
	private float rotationAcceleration;
	private float maxRotationPower;
	private float maxRotationPowerNeg;

	private bool wPressed;
	private bool aPressed;
	private bool sPressed;
	private bool dPressed;

	public void initController (PlayerShip ship) {
		Engine engine = ship.getEngine();
		mainAcceleration = engine.getMainAcceleration();
		maxMainPower = engine.getMaxMainPower();
		maxBackwardPower = engine.getMaxBackwardPower();
		rotationAcceleration = engine.getRotationAcceleration();
		maxRotationPower = engine.getMaxRotationPower();
		maxRotationPowerNeg = maxRotationPower * -1;
	}

	void Update () {
		if (StarSystem.gamePaused) { return; }

		checkInput ();
	}
	
	private void checkInput () {
		wPressed = Input.GetKey(KeyCode.W);
		aPressed = Input.GetKey(KeyCode.A);
		sPressed = Input.GetKey(KeyCode.S);
		dPressed = Input.GetKey(KeyCode.D);
	}

	void FixedUpdate () {
		if (StarSystem.gamePaused) { return; }

		forwardMoveControl ();
		turnControl ();
	}

	private void forwardMoveControl () {
		if (wPressed) {
			if (mainPower < maxMainPower) {
				mainPower += mainAcceleration;
				if (mainPower > maxMainPower) mainPower = maxMainPower;
			}
		} else if (sPressed) {
			if (mainPower > maxBackwardPower) {
				mainPower -= mainAcceleration;
				if (mainPower < maxBackwardPower) mainPower = maxBackwardPower;
			}
		}
		if (!wPressed && !sPressed) {
			if (mainPower != 0.0) {
				if (mainPower > 0.0) mainPower -= mainAcceleration * 0.5f;
				if (mainPower < 0.0) mainPower += mainAcceleration * 0.5f;

				if (mainPower < mainAcceleration && mainPower > (mainAcceleration * -1)) mainPower = 0.0f;
			}
		}
		if (mainPower != 0.0) {
			float zValue = transform.rotation.eulerAngles.z + 90;
			float velX = mainPower * Mathf.Cos(Mathf.Deg2Rad * zValue);
			float velY = mainPower * Mathf.Sin(Mathf.Deg2Rad * zValue);
			transform.position = new Vector3(transform.position.x + velX, transform.position.y + velY, transform.position.z);
		}
	}

	private void turnControl () {
		if (aPressed && dPressed) {
			//Do nothing
		} else if (aPressed) {
			if (rotationPower < maxRotationPower) {
				rotationPower += rotationAcceleration;
				if (rotationPower > maxRotationPower) rotationPower = maxRotationPower;
			}
		} else if (dPressed) {
			if (rotationPower > maxRotationPowerNeg) {
				rotationPower -= rotationAcceleration;
				if (rotationPower < maxRotationPowerNeg) rotationPower = maxRotationPowerNeg;
			}
		} else if (rotationPower != 0.0) {
			if (rotationPower > 0.0) rotationPower -= rotationAcceleration;
			if (rotationPower < 0.0) rotationPower += rotationAcceleration;

			if (rotationPower < rotationAcceleration && rotationPower > (rotationAcceleration * -1)) rotationPower = 0.0f;
		}
		if (rotationPower != 0.0) transform.Rotate(new Vector3(0, 0, 1), rotationPower);
	}
}