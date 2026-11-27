using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScreen : MonoBehaviour
{
    // Starts the game
    public void OnPlayButton ()
    {
        SceneManager.LoadScene("Game");
    }
    // Loads Options Menu
    public void OnOptionsButton ()
    {
        SceneManager.LoadScene("OptionsMenu");
    }
    // Exits game
    public void OnQuitButton ()
    {
        Application.Quit();
    }
}
