using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

	private Transform spaceship, trans;

	private float followingSpeed = 0.075f;

	private float cameraRestDistance = 0.1f;

	private float cameraZOffset = -10;

	private Vector2 cameraPosition, shipPosition;

	private float cameraToShipDistance;

	private Vector2 moveVector;

	private int cameraSizeDefault;
//				cameraSizeMax,
//				cameraSizeMin = 2;

	private StarField starField;

	private Vector3 pos = Vector3.zero;

	public void init (Transform spaceship, StarField starField) {
		this.spaceship = spaceship;
		this.starField = starField;
		trans = transform;
//		cameraSizeMax = cameraStandartSize + 4;
		cameraSizeDefault = (int)Camera.main.orthographicSize;
		setDirectlyToShip();
	}

	public void setDirectlyToShip () {
		transform.position = new Vector3(spaceship.position.x, spaceship.position.y, transform.position.z);
		starField.adjustStarField(trans.position);
	}

	void Update () {
		if(StarSystem.gamePaused) { return; }

//		if (Input.GetAxis("Mouse ScrollWheel") < 0) {
//			Camera.main.orthographicSize = Mathf.Min(Camera.main.orthographicSize+1, cameraSizeMax);
//		} else if (Input.GetAxis("Mouse ScrollWheel") > 0) {
//			Camera.main.orthographicSize = Mathf.Max(Camera.main.orthographicSize-1, cameraSizeMin);
//		}
	}

	public void setCameraSizeToDefault () {
		Camera.main.orthographicSize = cameraSizeDefault;
	}

	void FixedUpdate () {
		if (StarSystem.gamePaused) { return; }

		cameraPosition.Set(trans.position.x, trans.position.y);
		shipPosition.Set(spaceship.position.x, spaceship.position.y);
		cameraToShipDistance = Vector2.Distance(cameraPosition, shipPosition);
		if (cameraToShipDistance > cameraRestDistance) {
			moveCamera ();
		}
	}

	private void moveCamera () {
		float cameraFollow = Camera.main.orthographicSize <= 3? 0.15f: followingSpeed;
		moveVector = Vector2.Lerp(cameraPosition, shipPosition, cameraFollow);
		pos.Set(moveVector.x, moveVector.y, cameraZOffset);
		trans.position = pos;
		starField.adjustStarField(pos);
	}

	public Vector2 getCameraPosition () {
		return cameraPosition;
	}
}