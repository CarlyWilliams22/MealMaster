using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuManagerScript : MonoBehaviour
{
    public GameObject MenuButtons, Naming, ContinueSavedGame;
    public Text DinerName;

    private void Start()
    {
        if (!Prefs.GetIsGameInProgress())
        {
            ContinueSavedGame.SetActive(false);
        }
    }


    public void OnStartNewGame()
    {
        Prefs.SetAllToDefault();
        Naming.SetActive(true);
        MenuButtons.SetActive(false);
    }

    public void OnNameEntered()
    {
        Prefs.SetDinerName(DinerName.text);
    }

    public void OnContinueGame()
    {
        if (Prefs.GetCurrentScene().Equals("GameScene")){
            Prefs.SetDayNumber(Prefs.GetDayNumber() - 1);
        }
        SceneManager.LoadScene(Prefs.GetCurrentScene());
    }
}
