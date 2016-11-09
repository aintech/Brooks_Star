using UnityEngine;
using System.Collections;

public class EnemyMarker : MonoBehaviour {

	private SpriteRenderer portrait;

	public Transform trans { get; private set; }

	private Transform visual;

	public EnemyType enemyType { get; private set; }

	public bool isFound { get; private set; }

	private float waitTime, minWaitTime = 10, maxWaitTime = 20;

	private Vector3 targetPosition = Vector3.zero;

	private float dist, angle, x, y;

	private float speed = .001f;

	private bool inMotion;

	public EnemyMarker init (EnemyType enemyType, Transform holder) {
		this.enemyType = enemyType;
		trans = transform;
		visual = trans.Find("Visual");
		portrait = visual.Find("Portrait").GetComponent<SpriteRenderer>();
		portrait.sprite = ImagesProvider.getMarkerSprite(enemyType);
		trans.SetParent(holder);
		initPos();
		visual.gameObject.SetActive(false);

		return this;
	}

	void Update () {
		if (inMotion) {
			dist = Vector3.Distance(trans.position, targetPosition);
			if (dist < .05f) {
				inMotion = false;
				waitTime = Time.time + Random.Range(minWaitTime, maxWaitTime);
			} else {
				trans.localPosition = Vector3.Lerp(trans.localPosition, targetPosition, speed);
			}
		} else {
			if (waitTime <= Time.time) {
				findNewTargetPosition();
				inMotion = true;
			}
		}
	}

	private void initPos () {
		findNewTargetPosition();
		trans.localPosition = targetPosition;
		waitTime = Time.time + Random.Range(minWaitTime, maxWaitTime);
	}

	private void findNewTargetPosition () {
		dist = Random.value * ScanningScreen.FIELD_RADIUS;
		angle = Random.value * 360f;
		targetPosition.x = dist * Mathf.Cos(angle);
		targetPosition.y = dist * Mathf.Sin(angle);
	}

	public void revealMarker () {
		isFound = true;
		visual.gameObject.SetActive(true);
	}

	public void closeMarker () {
		gameObject.SetActive(false);
	}
}