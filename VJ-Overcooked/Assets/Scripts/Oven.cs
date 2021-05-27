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

    void Awake()
    {
        delay = burningTime;
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
        delay = burningTime;
    }

    public void finishOven()
    {
        GetComponent<ProcessBar>().hide();
        warning.SetActive(true);
        if (delay > 0) delay -= Time.deltaTime;
        else setOnFire();
    }

    public override void setOnFire()
    {
        if (!onFire)
        {
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
    }

}
