using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelChanger : MonoBehaviour
{
    public Animator transition;


    // Update is called once per frame
    void Update()
    {
        
    }

    internal void setSlideTrigger()
    {
        transition.SetTrigger("slide");
    }
}
