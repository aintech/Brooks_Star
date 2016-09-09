using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

	private Transform spaceship;

	private float followingSpeed = 0.075f;

	private float cameraRestDistance = 0.1f;

	private float cameraZOffset = -10;

	private Vector2 cameraPosition;

	private Vector2 shipPosition;

	private float cameraToShipDistance;

	private Vector2 moveVector;

	private int cameraStandartSize;

	private int cameraSizeMax;

	private int cameraSizeMin = 2;

	private bool gamePaused = false;

	public void init (Transform spaceship) {
		this.spaceship = spaceship;
		this.cameraStandartSize = (int) Camera.main.orthographicSize;
		cameraSizeMax = cameraStandartSize + 4;
		transform.position = new Vector3(spaceship.position.x, spaceship.position.y, transform.position.z);
	}

	void Update () {
		if (!gamePaused) {
			if (Input.GetAxis("Mouse ScrollWheel") < 0) {
				Camera.main.orthographicSize = Mathf.Min(Camera.main.orthographicSize+1, cameraSizeMax);
			} else if (Input.GetAxis("Mouse ScrollWheel") > 0) {
				Camera.main.orthographicSize = Mathf.Max(Camera.main.orthographicSize-1, cameraSizeMin);
			}
		}
	}

	void FixedUpdate () {
		if (!gamePaused) {
			cameraPosition.Set(transform.position.x, transform.position.y);
			shipPosition.Set(spaceship.position.x, spaceship.position.y);
			cameraToShipDistance = Vector2.Distance(cameraPosition, shipPosition);
			if (cameraToShipDistance > cameraRestDistance) {
				moveCamera ();
			}
		}
	}

	private void moveCamera () {
		float cameraFollow = Camera.main.orthographicSize <= 3? 0.15f: followingSpeed;
		moveVector = Vector2.Lerp(cameraPosition, shipPosition, cameraFollow);
		transform.position = new Vector3(moveVector.x, moveVector.y, cameraZOffset);
	}

	public Vector2 getCameraPosition () {
		return cameraPosition;
	}

	public void setGamePaused (bool gamePaused) {
		this.gamePaused = gamePaused;
	}
}
