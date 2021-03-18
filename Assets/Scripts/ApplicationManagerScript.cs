using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ApplicationManagerScript : MonoBehaviour
{
    private static string SCENE_MAIN_MENU = "MainMenuScene";
    private static string SCENE_GAME = "GameScene";

    public void OnClickPlay()
    {
        SceneManager.LoadScene(SCENE_GAME);
    }

    public void OnClickMainMenu()
    {
        SceneManager.LoadScene(SCENE_MAIN_MENU);
    }

    public void OnClickQuit()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #endif
        #if UNITY_STANDALONE
            Application.Quit();
        #endif
    }
}
