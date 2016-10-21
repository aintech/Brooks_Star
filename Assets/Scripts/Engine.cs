using UnityEngine;
using System.Collections;

public class Engine : MonoBehaviour {

	public EngineType engineType { get; private set;}

	public float mainAcceleration { get; private set; }

	public float backwardAcceleration { get; private set; }

	public float rotationAcceleration { get; private set; }

	public float maxMainPower { get; private set; }

	public float maxBackwardPower { get; private set; }

	public float maxRotationPower { get; private set; }

	public void setEngine (EngineType engineType, float maxMainPower, float maxRotationPower) {
		this.engineType = engineType;
		this.maxMainPower = maxMainPower;
		this.maxBackwardPower = -maxMainPower * .5f;
		this.maxRotationPower = maxRotationPower;

		mainAcceleration = .01f;
		backwardAcceleration = mainAcceleration * .5f;
		rotationAcceleration = .2f;
	}
}