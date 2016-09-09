using UnityEngine;
using System.Collections;

public class Sector : MonoBehaviour {

	public Sprite[] sectorsImages;

	private Transform mainCamera;

	private Vector2 mainCameraPosition = Vector2.zero;

	private float newSectorX;

	private float newSectorY;

	private float sectorMoveSpeed = 0.1f;

	private SpriteRenderer spriteRender;

	//Локальный номер сектора. Центральный сектор тот в котором находится камера, он всегда 22
	//соответственно окружающие сектора:
	// 	31 32 33
	//	21 22 23
	//	11 12 13
	private int localNumber;

	private float moveOffsetX;

	private float moveOffsetY;

	public void initSector (int localNumber, float moveOffsetX, float moveOffsetY) {
		this.localNumber = localNumber;
		this.moveOffsetX = moveOffsetX;
		this.moveOffsetY = moveOffsetY;
		spriteRender = GetComponent<SpriteRenderer>();
		mainCamera = Camera.main.transform;
	}

	void FixedUpdate () {
		moveSector();
	}

	private void moveSector () {
		mainCameraPosition.Set(mainCamera.position.x, mainCamera.position.y);
		newSectorX = mainCameraPosition.x - (mainCameraPosition.x * sectorMoveSpeed) + moveOffsetX;
		newSectorY = mainCameraPosition.y - (mainCameraPosition.y * sectorMoveSpeed) + moveOffsetY;
		transform.position = new Vector3(newSectorX, newSectorY, transform.position.z);
	}

	public void setBackgroundImage (int sectorNumber) {
		Sprite sprite = new Sprite();
		switch (sectorNumber) {
			case 11: sprite = sectorsImages[0];	break;
			case 12: sprite = sectorsImages[1];	break;
			case 13: sprite = sectorsImages[2];	break;
			case 14: sprite = sectorsImages[3];	break;
			case 15: sprite = sectorsImages[4];	break;
			case 21: sprite = sectorsImages[5];	break;
			case 22: sprite = sectorsImages[6];	break;
			case 23: sprite = sectorsImages[7];	break;
			case 24: sprite = sectorsImages[8];	break;
			case 25: sprite = sectorsImages[9];	break;
			case 31: sprite = sectorsImages[10]; break;
			case 32: sprite = sectorsImages[11]; break;
			case 33: sprite = sectorsImages[12]; break;
			case 34: sprite = sectorsImages[13]; break;
			case 35: sprite = sectorsImages[14]; break;
			case 41: sprite = sectorsImages[15]; break;
			case 42: sprite = sectorsImages[16]; break;
			case 43: sprite = sectorsImages[17]; break;
			case 44: sprite = sectorsImages[18]; break;
			case 45: sprite = sectorsImages[19]; break;
			case 51: sprite = sectorsImages[20]; break;
			case 52: sprite = sectorsImages[21]; break;
			case 53: sprite = sectorsImages[22]; break;
			case 54: sprite = sectorsImages[23]; break;
			case 55: sprite = sectorsImages[24]; break;
		}
		spriteRender.sprite = sprite;
	}
}
