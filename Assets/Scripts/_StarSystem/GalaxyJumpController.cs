using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GalaxyJumpController : ShipController {

	public static bool systemLoaded = true;

	public GUIStyle messageStyle;

	public static GUIStyle style;

	private StarSystemType destination;

	private Vector3 direction, lookVector = Vector3.zero;

	private bool inControl = false, onCourse = false, inJump, loadSystem;

	private PlayerShipController playerController;

	private float angle;

	private bool onLeftSide;

	private List<JumpMessage> messages = new List<JumpMessage>();

	private JumpMessage msg;

	private int warmEngineCounter;

	private SpriteRenderer jumpBG;

	private Color32 jumpColor = new Color32(255, 255, 255, 255);

	private JumpStage stage;

	private StarSystem starSystem;

	private Vector3 bgPos = Vector3.zero;

	private Transform bgTrans;

	private Transform cameraTrans;

	private CameraController cameraController;

	public void initJumper (StarSystem starSystem) {
		this.starSystem = starSystem;
		base.init();
		jumpBG = GameObject.Find("Jump Background").GetComponent<SpriteRenderer>();
		jumpBG.gameObject.SetActive(false);
		jumpBG.enabled = true;
		bgTrans = jumpBG.transform;
		style = messageStyle;
		cameraController = Camera.main.GetComponent<CameraController>();
		cameraTrans = cameraController.transform;
		playerController = GetComponent<PlayerShipController>();
	}

	public void startJumpSequence (StarSystemType destination, Vector3 direction) {
		stage = JumpStage.SETTING_COURSE;
		this.destination = destination;
		this.direction = direction;
		this.direction.x *= -1;
		UserInterface.showInterface = false;
		inControl = true;
		onCourse = false;
		playerController.inControl = false;
		inJump = false;
		loadSystem = false;
		warmEngineCounter = 100;
		jumpColor.a = 0;
		jumpBG.color = jumpColor;
		jumpBG.gameObject.SetActive(true);
		ship.prepareWeaponsToJump();
		accelerate = decelerate = turnLeft = turnRight = false;
		addMessage("Выход на курс прыжка", false);
		setRotationForJump(true);
	}

	override protected void Update () {
		if (inControl) {
			switch (stage) {
				case JumpStage.SETTING_COURSE: turnControl(); break;
				case JumpStage.WARMING_ENGINES:
					warmEngineCounter--;
					if (warmEngineCounter <= 0) {
						setMainEngineForJump(true);
						addMessage("Прыжок в систему " + destination.name(), false);
						inJump = true;
						stage = JumpStage.FILL_WHITE;
					}
					break;
				case JumpStage.FILL_WHITE:
					jumpColor.a += 2;
					if (jumpColor.a >= 250) {
						jumpColor.a = 255;
						stage = JumpStage.lOAD_SYSTEM;
						systemLoaded = false;
						StarSystem.setGamePause(true);
						Vars.starSystemType = destination;
						starSystem.loadStarSystem();
					}
					jumpBG.color = jumpColor;
					bgPos.x = cameraTrans.position.x;
					bgPos.y = cameraTrans.position.y;
					bgTrans.position = bgPos;
					break;
				case JumpStage.lOAD_SYSTEM:
					if (systemLoaded) {
						addMessage("Прибытие в систему " + Vars.starSystemType.name(), true);
						stage = JumpStage.ARRIVE_TO_SYSTEM;
						ship.transform.position = Vector3.zero;
						StarSystem.setGamePause(false);
						setRotationForJump(false);
						setMainEngineForJump(false);
						UserInterface.showInterface = true;
					}
					break;
				case JumpStage.ARRIVE_TO_SYSTEM:
					jumpColor.a -= 2;
					if (jumpColor.a <= 10) {
						inControl = false;
						playerController.inControl = true;
						jumpColor.a = 0;
						stage = JumpStage.DONE;
						jumpBG.gameObject.SetActive(false);
						cameraController.setDirectlyToShip();
						accelerate = false;
						inJump = false;
						ship.activateWeapons();
					}
					jumpBG.color = jumpColor;
					bgPos.x = cameraTrans.position.x;
					bgPos.y = cameraTrans.position.y;
					bgTrans.position = bgPos;
					break;
				default: Debug.Log("Unknown stage: " + stage); break;
			}
			base.Update();
			if (Input.anyKeyDown && !inJump) {
				addMessage("Прыжок прерван командиром", true);
				setRotationForJump(false);
				setMainEngineForJump(false);
				inControl = false;
				UserInterface.showInterface = true;
				playerController.inControl = true;
				ship.activateWeapons();
				playerController.checkInput();
			}
		}
	}

	override protected void FixedUpdate () {
		if (inControl) { base.FixedUpdate(); }
	}

	void OnGUI () {
		if (messages.Count > 0) {
			for (int i = 0; i < messages.Count; i++) {
				msg = messages[i];
				msg.update(i == messages.Count - 1);
				GUI.Label(msg.rect, msg.message, msg.style);
				if (!msg.onPosition) { return; }
			}
		}
	}

	private void addMessage (string message, bool selfDisabling) {
		messages.Add(new JumpMessage(message, selfDisabling, messages));
	}

	private void turnControl () {
		lookVector.x = Mathf.Sin(Mathf.Deg2Rad * trans.rotation.eulerAngles.z);
		lookVector.y = Mathf.Cos(Mathf.Deg2Rad * trans.rotation.eulerAngles.z);
		angle = Vector2.Angle(lookVector.normalized, direction);
		if (angle > 10) {
			onLeftSide = Vector3.Cross(lookVector, direction).z < 0;
			turnLeft = onLeftSide;
			turnRight = !onLeftSide;
		} else if (angle < 5) {
			turnLeft = turnRight = false;
			addMessage("Разогрев ускорителей", false);
			accelerate = true;
			stage = JumpStage.WARMING_ENGINES;
		}
	}

	private class JumpMessage {
		public string message { get; private set; }
		public GUIStyle style { get; private set; }
		public Rect rect { get; private set; }
		public bool onPosition { get; private set; }

		private Color32 color = new Color32(200, 255, 0, 5);
		private float stopY = 25, lastY = -10, speed = .2f;
		private float liveTime = 100;
		private bool disapear = false;
		private bool selfDisabling;
		private List<JumpMessage> container;

		public JumpMessage (string message, bool selfDisabling, List<JumpMessage> container) {
			this.message = message;
			this.selfDisabling = selfDisabling;
			this.container = container;
			style = new GUIStyle(GalaxyJumpController.style);
			rect = new Rect(Screen.width * .5f, 60, 0, 0);
		}

		public void update (bool lastInQueue) {
			disapear = !lastInQueue;
			if (!onPosition) {
				if (color.a < 255) {
					color.a += 2;
					if (color.a < 5) {
						color.a = 255;
					}
					style.normal.textColor = color;
				}
				if (rect.y > stopY) {
					rect = new Rect(rect.x, rect.y - speed, rect.width, rect.height);
					if (rect.y <= stopY) {
						rect = new Rect(rect.x, stopY, rect.width, rect.height);
						color.a = 255;
						style.normal.textColor = color;
						onPosition = true;
					}
				}
			} else if (selfDisabling) {
				liveTime--;
				if (liveTime <= 0) {
					fadeOff();
				}
			} else if (disapear) {
				fadeOff();
			}
		}

		private void fadeOff () {
			if (rect.y > lastY) {
				rect = new Rect(rect.x, rect.y - speed, rect.width, rect.height);
				if (color.a > 5) {
					color.a -= 2;
					style.normal.textColor = color;
				}
				if (rect.y <= lastY) {
					container.Remove(this);
				}
			}
		}
	}

	private enum JumpStage {
		SETTING_COURSE, WARMING_ENGINES, FILL_WHITE, lOAD_SYSTEM, ARRIVE_TO_SYSTEM, DONE
	}
}