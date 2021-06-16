using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Level_loader : MonoBehaviour
{
    public Animator transition;
    public float transitionTime = 1f;
    void Update()
    {
       if (Input.GetKey("0")) loadNextLevel(0);
       else if (Input.GetKey("1")) loadNextLevel(1);
       else if (Input.GetKey("2")) loadNextLevel(2);
       else if (Input.GetKey("3")) loadNextLevel(3);
       else if (Input.GetKey("4")) loadNextLevel(4);
       else if (Input.GetKey("5")) loadNextLevel(5);
    }

    public void loadNextLevel(int levelIndex) {
        StartCoroutine(loadLevel(levelIndex)); // index of the level
    }

    

    IEnumerator loadLevel(int levelIndex)
    {
        FindObjectOfType<AudioManager>().stopAll();
        transition.SetTrigger("Start");
        yield return new WaitForSeconds(transitionTime);
        SceneManager.LoadScene(levelIndex);
    }
}
