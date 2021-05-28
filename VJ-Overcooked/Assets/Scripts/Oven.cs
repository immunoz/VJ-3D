using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

 class Oven : Location
{

    public float heightOffset;
    public GameObject warning;
    public float MaxCookTime;
    private float delay;
    public float burningTime;
    
    public float beepTimer;
    private float btimer;

    private bool playingAlert;

    void Awake()
    {
        delay = burningTime;
        playingAlert = false;
        btimer = 0;
    }

    void FixedUpdate()

    {
        if (currentObject != null && !currentObject.GetComponent<PizzaMass>().finished())
        {
            GetComponent<ProcessBar>().setProcessTime(currentObject.GetComponent<PizzaMass>().getCookingTimeLeftNormalized());
        }
        else if (currentObject != null && !currentObject.GetComponent<PizzaMass>().burned && !fireDisable) finishOven();
    }
    public override float getGetHeightOffset()
    {
        return heightOffset;
    }

    public override bool finished()
    {
        return currentObject.GetComponent<PizzaMass>().finished() && !onFire;
    }

    public void initOven()
    {
        warning.SetActive(false);
        GetComponent<ProcessBar>().setMaxTime(1);
        currentObject.GetComponent<PizzaMass>().startCookingPizza();
        GetComponent<AudioSource>().Play();
        delay = burningTime;
    }

    public void finishOven()
    {
        if (!playingAlert)
        {
            Alert.Play();
            playingAlert = true;
            GetComponent<AudioSource>().Stop();
        }
        else {
            if (btimer > 0) btimer -= Time.deltaTime;
            else {
                Alert.Play();
                btimer = beepTimer;
            }
        }
        GetComponent<ProcessBar>().hide();
        warning.SetActive(true);
        if (delay > 0) delay -= Time.deltaTime;
        else setOnFire();
    }

    public override void setOnFire()
    {
        if (!onFire)
        {
            fire.Play();
            Alert.Stop();
            playingAlert = false;
            warning.SetActive(false);
            onFire = true;
            flame.SetActive(true);
            currentObject.GetComponent<PizzaMass>().setBurned(true);
            delay = 3;
        }
    }

    public void  hideProcessBar ()
    {
        GetComponent<ProcessBar>().hide();
        warning.SetActive(false);
        GetComponent<AudioSource>().Stop();
    }

    public void setCookingFinished()
    {
       if ( currentObject != null )
        {
            currentObject.GetComponent<PizzaMass>().finishCooking();
            GetComponent<ProcessBar>().hide();
            GetComponent<AudioSource>().Stop();
        }
    }
}
