using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonScript : MonoBehaviour
{
    public GameObject actualLevel;
    private GameObject container;

    void Start()
    {
        container = GameObject.Find("Levels");
    }
    public void clickedRight()
    {
        
        switch (actualLevel.name)
        {
            case "level1":
                actualLevel.GetComponent<LevelChanger>().setSlideLeft();
                GameObject level2 = container.transform.GetChild(1).gameObject; 
               // level2.SetActive(true);
                level2.GetComponent<LevelChanger>().setSlideTrigger();
                actualLevel = level2;
                break;
            case "level2":
                actualLevel.GetComponent<LevelChanger>().setSlideLeft();
                GameObject level3 = container.transform.GetChild(2).gameObject; 
                //level3.SetActive(true);
                level3.GetComponent<LevelChanger>().setSlideTrigger();
                actualLevel = level3;
                break;

        }
        GameObject.Find("ButtonLeft").GetComponent<ButtonScript>().setActualLevel(actualLevel);
    }

    public void clickedLeft()
    {
        switch (actualLevel.name)
        {
            case "level2":
                actualLevel.GetComponent<LevelChanger>().setSlideMiddleToRight();
                
                GameObject level1 = container.transform.GetChild(0).gameObject;
                //level1.SetActive(true);
                level1.GetComponent<LevelChanger>().setSlideRight();
                actualLevel = level1;
                break;
            case "level3":
                actualLevel.GetComponent<LevelChanger>().setSlideMiddleToRight();
                GameObject level2 = container.transform.GetChild(1).gameObject;
                //level2.SetActive(true);
                level2.GetComponent<LevelChanger>().setSlideRight();
                actualLevel = level2;
                break;

                

        }
        GameObject.Find("ButtonRight").GetComponent<ButtonScript>().setActualLevel(actualLevel);
    }

    public void loadLevel() {
        switch (actualLevel.name) {
            case "level1":
                GameObject.Find("LevelLoader").GetComponent<Level_loader>().loadNextLevel(0);
                break;
            case "level2":
                GameObject.Find("LevelLoader").GetComponent<Level_loader>().loadNextLevel(1);
                break;
        }
        FindObjectOfType<AudioManager>().stop("MainMenuTheme");
    }

    public void setActualLevel(GameObject level) {
        actualLevel = level;
    }
}
