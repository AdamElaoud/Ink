using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public static GameManager instance;

    // Serialize to control starting level (testing)
    [SerializeField]
    Level currentLevel;

    void Start() {
        // load next level additively
        if (SceneManager.sceneCount < 2)
            SceneManager.LoadScene((int)currentLevel, LoadSceneMode.Additive);
    }

    void Awake() {
        if (instance == null) {
            instance = this;
            DontDestroyOnLoad(gameObject);

        } else {
            Destroy(gameObject);
            return;
        }
    }

    public void RestartLevel() {
        SceneManager.UnloadSceneAsync((int)currentLevel);
        SceneManager.LoadScene((int)currentLevel, LoadSceneMode.Additive);
    }

    public void NextLevel() {
        if ((int)currentLevel + 1 != (int)Level.COUNT) {
            SceneManager.UnloadSceneAsync((int)currentLevel);

            currentLevel++;

            SceneManager.LoadScene((int)currentLevel, LoadSceneMode.Additive);

        } else {
            SceneManager.UnloadSceneAsync((int)currentLevel);
            SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());
            SceneManager.LoadScene(0);
            Destroy(gameObject);
        }
    }

    public void PreviousLevel() {
        if ((int)currentLevel - 1 != 1) {
            SceneManager.UnloadSceneAsync((int)currentLevel);

            currentLevel--;

            SceneManager.LoadScene((int)currentLevel, LoadSceneMode.Additive);
        }
    }
}
