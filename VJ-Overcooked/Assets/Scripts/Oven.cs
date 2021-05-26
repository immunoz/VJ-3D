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
            // cookTime += Time.deltaTime;
            GetComponent<ProcessBar>().setProcessTime(currentObject.GetComponent<PizzaMass>().getCookingTimeLeftNormalized());
            //if ( cookTime >= MaxCookTime ) currentObject.GetComponent<PizzaMass>().setDoneCooking();
        }
        else if (currentObject != null && ! currentObject.GetComponent<PizzaMass>().burned ) finishOven();
    }
    public override float getGetHeightOffset()
    {
        return heightOffset;
    }

    public override bool finished()
    {
        return currentObject.GetComponent<PizzaMass>().finished();
    }

    public void initOven()
    {
        // cambairlo por pizza finishedCooking
        GetComponent<ProcessBar>().setMaxTime(1);
        currentObject.GetComponent<PizzaMass>().startCookingPizza();
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
    }

}
