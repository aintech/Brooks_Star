using UnityEngine;
using System.Collections;

public class Engine : MonoBehaviour {

	private EngineType engineType;

	private float mainAcceleration = 0.01f;
	
	private float rotationAcceleration = 0.2f;

	private float maxMainPower;

	private float maxBackwardPower;

	private float maxRotationPower;

	public void setEngine (EngineType engineType, float maxMainPower, float maxRotationPower) {
		this.engineType = engineType;
		this.maxMainPower = maxMainPower;
		this.maxBackwardPower = -maxMainPower;
		this.maxRotationPower = maxRotationPower;
	}

	public float getMainAcceleration () {
		return mainAcceleration;
	}

	public float getMaxMainPower () {
		return maxMainPower;
	}

	public float getMaxBackwardPower () {
		return maxBackwardPower;
	}

	public float getRotationAcceleration () {
		return rotationAcceleration;
	}

	public float getMaxRotationPower () {
		return maxRotationPower;
	}

	public EngineType getEngineType () {
		return engineType;
	}
}
