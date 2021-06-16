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
        SceneManager.LoadScene(6);
    }

    public void quit()
    {
        Application.Quit();
    }

    public void backFromCreditsToMenu ()
    {
        FindObjectOfType<AudioManager>().stopAll();
        SceneManager.LoadScene(0);
    }

    public void setSlideTrigger()
    {
        GameObject.Find("level1").GetComponent<LevelChanger>().setSlideTrigger();
    }
}
