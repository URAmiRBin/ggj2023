using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour {
    public void LoadLevel(string level) {
        SceneManager.LoadScene(level);
    }

    public void QuitGame() {
        Application.Quit();
    }
}
