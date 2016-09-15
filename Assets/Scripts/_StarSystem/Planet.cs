using UnityEngine;
using System.Collections;

public class Planet : MonoBehaviour {

    private PlanetType planetType;

    private SpriteRenderer surfaceRender;

    private Vector3 systemCenter = Vector3.zero, rotateVector = Vector3.back;

    private Quaternion noRotation = new Quaternion();

    private float orbitingSpeed, contactDistance = 3, toShipDistance;

    private Transform trans, ship;

    private bool shipIsNear;

    public Planet init (PlanetType planetType, Transform ship) {
        this.planetType = planetType;
        this.ship = ship;
        trans = transform;
        surfaceRender = transform.Find("Surface").GetComponent<SpriteRenderer>();
        surfaceRender.sprite = Imager.getPlanet(planetType);
        float angle = Random.Range(0, 359);
        transform.localPosition = new Vector2(planetType.getDistanceToStar() * Mathf.Sin(angle), planetType.getDistanceToStar() * Mathf.Cos(angle));
        orbitingSpeed = 10 / planetType.getDistanceToStar();
        return this;
    }

    void Update () {
		if (StarSystem.gamePaused) { return; }

        orbitingStar();
        checkShipIsCloseEnought();
    }

    private void orbitingStar () {
        trans.RotateAround(systemCenter, rotateVector, orbitingSpeed * Time.deltaTime);
        trans.rotation = noRotation;
    }

    private void checkShipIsCloseEnought () {
        toShipDistance = Vector2.Distance(ship.position, trans.position);
        if (!shipIsNear && toShipDistance <= contactDistance) {
            setShipIsNear(true);
        } else if (shipIsNear && toShipDistance > contactDistance) {
            setShipIsNear(false);
        }
    }

	public void setShipIsNear (bool shipIsNear) {
        this.shipIsNear = shipIsNear;
        if (shipIsNear) { Vars.userInterface.showPlanetInfo(planetType); }
        else { Vars.userInterface.hidePlanetInfo(); }
    }

	public PlanetType getPlanetType () {
		return planetType;
	}
}