﻿using UnityEngine;
using System.Collections;

public class Utils : MonoBehaviour {
	
	public static Collider2D hit;
	
	public static Vector2 mousePos;
	
	private static Camera cam;
	
	private static Vector2 zeroV = Vector2.zero;

	private static float seed = 0;
	
	void Awake () {
		cam = GetComponent<Camera>();
	}
	
	void Update () {
		if (cam == null) { Debug.Log("Camera is null"); cam = GetComponent<Camera>(); }
		mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
		hit = Physics2D.Raycast(mousePos, zeroV, 1).collider;
	}

	public static float getRandomValue (float value, float percent) {
		seed = value * 0.01f * percent;
		return Mathf.Round(Random.Range(value - seed, value + seed) * 10) * 0.1f;
	}

	public static int getRandomValue (int value, int percent) {
		return Mathf.RoundToInt(getRandomValue((float)value, (float)percent));
	}
}