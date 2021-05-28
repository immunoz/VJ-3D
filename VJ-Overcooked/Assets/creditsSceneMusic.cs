using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class creditsSceneMusic : MonoBehaviour
{
    void Start()
    {
        FindObjectOfType<AudioManager>().stopAll();
        FindObjectOfType<AudioManager>().play("oldTown");
    }
}
