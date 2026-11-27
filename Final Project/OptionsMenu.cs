using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OptionsMenu : MonoBehaviour
{
    // Takes the player back to the main menu
    public void OnMainMenuButton ()
    {
        SceneManager.LoadScene("TitleScreen");
    }
}
