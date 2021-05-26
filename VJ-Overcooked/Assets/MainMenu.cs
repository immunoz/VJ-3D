using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    private void Start()
    {
        FindObjectOfType<AudioManager>().play("MainMenuTheme");
    }

    public void credits()
    {
        SceneManager.LoadScene(4);
    }

    public void quit()
    {
        Application.Quit();
    }

    public void backFromCreditsToMenu ()
    {
        SceneManager.LoadScene(3);
    }
    public void setSlideTrigger()
    {
        GameObject.Find("level1").GetComponent<LevelChanger>().setSlideTrigger();
    }
}
