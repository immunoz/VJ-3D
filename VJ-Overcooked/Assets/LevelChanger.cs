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

    public void setSlideTrigger()
    {
        transition.SetTrigger("slide");
    }

    public void setSlideLeft()
    {
        transition.SetTrigger("slideLeft");
    }

    public void setSlideRight()
    {
        transition.SetTrigger("slideRight");
    }

    public void setSlideMiddleToRight()
    {
        transition.SetTrigger("middleToRight");
    }
}
