using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public enum StarSystemType {
	ALURIA, CRITA
}
public static class StarSystemDescriptor {
	public static string name (this StarSystemType type) {
		switch (type) {
			case StarSystemType.ALURIA: return "Алурия";
			case StarSystemType.CRITA: return "Крита";
			default: Debug.Log("Unknown starsystem type: " + type); return "";
		}
	}

	public static PlanetType[] planetTypes (this StarSystemType type) {
		if (planetToSystemMap == null) { initPlanetToSystemMap(); }
		return planetToSystemMap[type];
	}

	private static Dictionary<StarSystemType, PlanetType[]> planetToSystemMap;

	private static void initPlanetToSystemMap () {
		planetToSystemMap = new Dictionary<StarSystemType, PlanetType[]>();
		List<PlanetType> types;
		foreach (StarSystemType system in Enum.GetValues(typeof(StarSystemType))) {
			types = new List<PlanetType>();
			foreach(PlanetType planet in Enum.GetValues(typeof(PlanetType))) {
				if (planet.starSystemType() == system) { types.Add(planet); }
			}
			planetToSystemMap.Add(system, types.ToArray());
		}
	}
}