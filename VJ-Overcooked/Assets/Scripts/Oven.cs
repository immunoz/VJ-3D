using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

 class Oven : Location
{

    public float heightOffset;

    private float cookTime;
    public float MaxCookTime;

    void Start()
    {
        cookTime = 0.0f;
    }

    void FixedUpdate()

    {
     if ( currentObject != null )
        {
            cookTime += Time.deltaTime;
            GetComponent<ProcessBar>().setProcessTime(cookTime / MaxCookTime );
            if ( cookTime >= MaxCookTime ) currentObject.GetComponent<Pizza>().setDoneCooking();
        }
    }
    public override float getGetHeightOffset()
    {
        return heightOffset;
    }

    public override bool finished()
    {
        return currentObject.GetComponent<Pizza>().finished();
    }

    public void initOven()
    {
        // cambairlo por pizza finishedCooking
        GetComponent<ProcessBar>().setMaxTime(1);
        currentObject.GetComponent<Pizza>().startCookingPizza();
    }

    public void finishOven()
    {
        cookTime = 0.0f;
        GetComponent<ProcessBar>().hide();
    }

    public override void setOnFire()
    {
        if (!onFire)
        {
            onFire = true;
            flame.SetActive(true);
        }
    }

}
