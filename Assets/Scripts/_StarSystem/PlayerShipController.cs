using UnityEngine;
using System.Collections;

public class PlayerShipController : ShipController {
	
	private float mainPower = 0.0f;
	private float mainAcceleration;
	private float maxMainPower;
//	private float maxBackwardPower;
	
	private float rotationPower = 0.0f;
	private float rotationAcceleration;
	private float maxRotationPower;
	private float maxRotationPowerNeg;

	private bool wPressed;
	private bool aPressed;
//	private bool sPressed;
	private bool dPressed;

	private Transform trans;

	private SpriteRenderer leftExhaust, rightExhaust;

	private ParticleSystem exhaust;

	private Vector3 pos = Vector3.zero, rotateAxis = Vector3.forward;

	public void initController (PlayerShip ship) {
		trans = transform;

		exhaust = trans.Find("Main Exhaust").GetComponent<ParticleSystem>();
		leftExhaust = trans.Find("Left Exhaust").GetComponent<SpriteRenderer>();
		rightExhaust = trans.Find("Right Exhaust").GetComponent<SpriteRenderer>();

		Engine engine = ship.getEngine();
		mainAcceleration = engine.getMainAcceleration();
		maxMainPower = engine.getMaxMainPower();
//		maxBackwardPower = engine.getMaxBackwardPower();
		rotationAcceleration = engine.getRotationAcceleration();
		maxRotationPower = engine.getMaxRotationPower();
		maxRotationPowerNeg = maxRotationPower * -1;

		pos.Set(trans.position.x, trans.position.y, StarField.zOffset);
		trans.position = pos;
	}

	void Update () {
		if (StarSystem.gamePaused) { return; }
		checkInput ();

		if (wPressed && exhaust.isStopped) { exhaust.Play(); }
		else if (!wPressed && exhaust.isPlaying) { exhaust.Stop(); }

		if (aPressed && !rightExhaust.enabled) { rightExhaust.enabled = true; }
		else if (!aPressed && rightExhaust.enabled) { rightExhaust.enabled = false; }

		if (dPressed && !leftExhaust.enabled) { leftExhaust.enabled = true; }
		else if (!dPressed && leftExhaust.enabled) { leftExhaust.enabled = false; }
	}
	
	private void checkInput () {
		wPressed = Input.GetKey(KeyCode.W);
		aPressed = Input.GetKey(KeyCode.A);
//		sPressed = Input.GetKey(KeyCode.S);
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
//		} else if (sPressed) {
//			if (mainPower > maxBackwardPower) {
//				mainPower -= mainAcceleration;
//				if (mainPower < maxBackwardPower) mainPower = maxBackwardPower;
//			}
		} else {
			if (mainPower != 0.0) {
				if (mainPower > 0.0) mainPower -= mainAcceleration * 0.5f;
				if (mainPower < 0.0) mainPower += mainAcceleration * 0.5f;

				if (mainPower < mainAcceleration && mainPower > (mainAcceleration * -1)) mainPower = 0.0f;
			}
		}
//		if (!wPressed && !sPressed) {
//			if (mainPower != 0.0) {
//				if (mainPower > 0.0) mainPower -= mainAcceleration * 0.5f;
//				if (mainPower < 0.0) mainPower += mainAcceleration * 0.5f;
//
//				if (mainPower < mainAcceleration && mainPower > (mainAcceleration * -1)) mainPower = 0.0f;
//			}
//		}
		if (mainPower != 0.0) {
			float zValue = transform.rotation.eulerAngles.z + 90;
			float velX = mainPower * Mathf.Cos(Mathf.Deg2Rad * zValue);
			float velY = mainPower * Mathf.Sin(Mathf.Deg2Rad * zValue);
			pos.Set(trans.position.x + velX, trans.position.y + velY, StarField.zOffset);
			trans.position = pos;
		}
	}

	private void turnControl () {
		if (aPressed && !dPressed) {
			if (rotationPower < maxRotationPower) {
				rotationPower += rotationAcceleration;
				if (rotationPower > maxRotationPower) rotationPower = maxRotationPower;
			}
		} else if (dPressed && !aPressed) {
			if (rotationPower > maxRotationPowerNeg) {
				rotationPower -= rotationAcceleration;
				if (rotationPower < maxRotationPowerNeg) rotationPower = maxRotationPowerNeg;
			}
		} else if (rotationPower != 0.0) {
			if (rotationPower > 0.0) rotationPower -= rotationAcceleration;
			if (rotationPower < 0.0) rotationPower += rotationAcceleration;

			if (rotationPower < rotationAcceleration && rotationPower > (rotationAcceleration * -1)) rotationPower = 0.0f;
		}
		if (rotationPower != 0.0) transform.Rotate(rotateAxis, rotationPower);
	}
}