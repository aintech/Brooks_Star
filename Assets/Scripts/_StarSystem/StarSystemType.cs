using UnityEngine;
using System.Collections;

public enum StarSystemType {
	ALURIA
}
public static class StarSystemDescriptor {
	public static string getName (this StarSystemType type) {
		switch (type) {
			case StarSystemType.ALURIA: return "Алурия";
			default: Debug.Log("Unknown starsystem type: " + type); return "";
		}
	}

    public static PlanetType[] getPlanetTypes (this StarSystemType type) {
        switch (type) {
			case StarSystemType.ALURIA:  return new PlanetType[] { PlanetType.CORAS, PlanetType.PALETTE };
            default: Debug.Log("Unknown system type: " + type); return new PlanetType[] { };
        }
    }
}