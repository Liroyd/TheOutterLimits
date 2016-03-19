using UnityEngine;
using UnityEngine.SceneManagement;

public class PlanetEventHandler : MonoBehaviour {

    Planet planet;

	void Start () {
        planet = new Planet ("Earth");
        gameObject.name = planet.planetName;
	}

    void OnMouseDown() {
        LevelInfo.getInstance().planet = this.planet;
        SceneManager.LoadScene("2. Planet Details");
    }
}
