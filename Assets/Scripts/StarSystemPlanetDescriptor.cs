using UnityEngine;
using System.Collections;

public class StarSystemPlanetDescriptor : MonoBehaviour {

	public Texture planetDescriptionBG;

	public GUIStyle planetNameStyle, planetLandStyle, planetExploreStyle, planetStatusStyle, planetExploredValueStyle, planetProbesStyle, addDroneStyle, removeDroneStyle;

	private Texture planetSurface;

	private bool planetDescriptVisible;

	private string planetName, planetStatus;

	private PlanetType planetType;

	private StarSystem starSystem;

	private Rect  planetDescriptRect = new Rect(20, 20, 210, 340), planetSurfaceRect, planetNameRect,
				  planetLandExploreRect, planetStatusRect, planetExploreRect, planetProbesRect, addDroneRect, removeDroneRect;

	private bool explored, colonized, populated;

	private int probesCount;

	public StarSystemPlanetDescriptor init (StarSystem starSystem) {
		this.starSystem = starSystem;

		planetSurfaceRect = new Rect(planetDescriptRect.x + 5, planetDescriptRect.y + 5, 200, 125);
		planetNameRect = new Rect(planetDescriptRect.x + planetDescriptRect.width / 2, planetDescriptRect.y + planetSurfaceRect.height + 30, 0, 0);
		planetLandExploreRect = new Rect(planetDescriptRect.x + 5, planetDescriptRect.y + planetDescriptRect.height - 40, 200, 35);
		planetStatusRect = new Rect(planetDescriptRect.x + planetDescriptRect.width / 2, planetNameRect.y + 30, 0, 0);
		planetExploreRect = new Rect(planetDescriptRect.x + planetDescriptRect.width / 2, planetStatusRect.y + 40, 0, 0);
		planetProbesRect = new Rect(planetDescriptRect.x + planetDescriptRect.width / 2, planetExploreRect.y + 40, 0, 0);
		addDroneRect = new Rect(planetDescriptRect.x + 4, planetDescriptRect.y + planetDescriptRect.height - 40, 100, 32);
		removeDroneRect = new Rect(addDroneRect.x + 102, addDroneRect.y, addDroneRect.width, addDroneRect.height);

		hidePlanetInfo();

		return this;
	}

	void OnGUI () {
		if (!UserInterface.showInterface) { return; }

		if (planetDescriptVisible) {
			GUI.DrawTexture(planetDescriptRect, planetDescriptionBG);
			GUI.DrawTexture(planetSurfaceRect, planetSurface);
			GUI.Label(planetNameRect, planetName, planetNameStyle);
			GUI.Label(planetStatusRect, planetStatus, planetStatusStyle);
			if (!explored) {
				GUI.Label(planetExploreRect, "0% изучено", planetExploredValueStyle);
				if (probesCount > 0) {
					GUI.Label(planetProbesRect, (probesCount + "/5 зондов"), planetProbesStyle);
				}
				if (probesCount < 5 && GUI.Button(addDroneRect, "", addDroneStyle)) {
					addDroneToPlanet();
				}
				if (probesCount > 0 && GUI.Button(removeDroneRect, "", removeDroneStyle)) {
					removeDroneFromPlanet();
				}
			} else if (colonized || populated) {
				if (GUI.Button(planetLandExploreRect, "", planetLandStyle)) {
					starSystem.landOnPlanet(planetType);
				}
			}
		}
	}

	private void addDroneToPlanet () {
		if (Vars.probesCount > 0) {
			Vars.planetProbes[planetType]++;
			Vars.probesCount--;
			probesCount++;
		}
	}

	private void removeDroneFromPlanet () {
		Vars.planetProbes[planetType]--;
		Vars.probesCount++;
		probesCount--;
	}

	private void explorePlanet () {
		Vars.exploredPlanets.Add(planetType);
		explored = true;
		planetStatus = colonized? "Колонизирована": populated? "Обитаема": "Необитаема";
	}

	public void showPlanetInfo (PlanetType planetType) {
		this.planetType = planetType;
		planetDescriptVisible = true;
		planetSurface = Imager.getPlanetSurface(planetType).texture;
		planetName = planetType.getName();
		explored = true;//Vars.exploredPlanets.Contains(planetType);
		colonized = planetType.isColonized();
		populated = planetType.isPopulated();
		if (!explored && !Vars.planetProbes.ContainsKey(planetType)) {
			Vars.planetProbes.Add(planetType, 0);
			Vars.planetExploredPercent.Add(planetType, 0);
			probesCount = 0;
		}
		planetStatus = !explored? "Не исследована": colonized? "Колонизирована": populated? "Обитаема": "Необитаема";
	}

	public void hidePlanetInfo () {
		planetDescriptVisible = false;
	}
}