using UnityEngine;
using System.Collections;

public class GalaxyJumper : MonoBehaviour {

	private PlayerShip ship;

	private StarSystemType destination;

	public GalaxyJumper init (PlayerShip ship)  {
		this.ship = ship;
		return this;
	}

	public void startJumpSequence (StarSystemType destination) {
		this.destination = destination;
		UserInterface.showInterface = false;
		disableAll();
	}

	private void disableAll () {
		
	}
}