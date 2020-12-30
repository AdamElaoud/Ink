using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class MainMenu : MonoBehaviour {
    
    public void PlayGame() {
        // Scene 1 is Level Base
        SceneManager.LoadScene(1);
    }

    public void PlayLevel(Level level) {
        SceneManager.LoadScene(1);
        SceneManager.LoadScene((int)level, LoadSceneMode.Additive);
    }

    public void Quit() {
        Application.Quit();
    }
}
