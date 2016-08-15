using UnityEngine;
using UnityEngine.SceneManagement;

public class ReturnToFirstLevel : MonoBehaviour {

    /*void OnLevelWasLoaded() {
        LevelInfo.getInstance().currentLevel = "2. Planet Details";

        Debug.Log(LevelInfo.getInstance());
        Invoke("LoadFirstLevel", 3f);
    }*/

    public void LoadFirstLevel() {
        SceneManager.LoadScene("1. Welcome");
    }
}
