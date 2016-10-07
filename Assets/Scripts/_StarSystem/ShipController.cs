using UnityEngine;
using System.Collections;

public class ShipController : MonoBehaviour {

	protected float mainPower, mainAcceleration, maxMainPower,//	private float maxBackwardPower;
					rotationPower, rotationAcceleration, maxRotationPower, maxRotationPowerNeg;

	protected bool accelarate, turnLeft, turnRight;

	protected Transform trans;

	protected SpriteRenderer leftExhaust, rightExhaust;

	protected ParticleSystem exhaust;

	protected Vector3 pos = Vector3.zero, rotateAxis = Vector3.forward;

	public void init (Ship ship) {
		trans = transform;
		exhaust = trans.Find("Main Exhaust").GetComponent<ParticleSystem>();
		leftExhaust = trans.Find("Left Exhaust").GetComponent<SpriteRenderer>();
		rightExhaust = trans.Find("Right Exhaust").GetComponent<SpriteRenderer>();

		Engine engine = ship.engine;
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

		if (accelarate && exhaust.isStopped) { exhaust.Play(); }
		else if (!accelarate && exhaust.isPlaying) { exhaust.Stop(); }

		if (turnLeft && !rightExhaust.enabled) { rightExhaust.enabled = true; }
		else if (!turnLeft && rightExhaust.enabled) { rightExhaust.enabled = false; }

		if (turnRight && !leftExhaust.enabled) { leftExhaust.enabled = true; }
		else if (!turnRight && leftExhaust.enabled) { leftExhaust.enabled = false; }
	}

	virtual protected void checkInput () {}

	void FixedUpdate () {
		if (StarSystem.gamePaused) { return; }

		decideNextMove();
		forwardMoveControl ();
		turnControl ();
	}

	virtual protected void decideNextMove () {}

	private void forwardMoveControl () {
		if (accelarate) {
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
		if (turnLeft && !turnRight) {
			if (rotationPower < maxRotationPower) {
				rotationPower += rotationAcceleration;
				if (rotationPower > maxRotationPower) rotationPower = maxRotationPower;
			}
		} else if (turnRight && !turnLeft) {
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