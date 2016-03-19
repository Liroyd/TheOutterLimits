using UnityEngine;
using System.Collections;

public class Planet {

    public string planetName;

    public Planet(string name) {
        planetName = name;
    }

    public override string ToString() {
        return planetName;
    }
}
