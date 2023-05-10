using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public void LoadMainMenu() => LoadScene(SceneNames.MainMenu);
    public void LoadGameScene() => LoadScene(SceneNames.GameScene);

    private void LoadScene(string sceneName)=> SceneManager.LoadScene(sceneName);
}
