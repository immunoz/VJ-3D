using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

 class Oven : Location
{

    public float heightOffset;

    public float MaxCookTime;

    void FixedUpdate()

    {
        if (currentObject != null && !currentObject.GetComponent<PizzaMass>().finished())
        {
            // cookTime += Time.deltaTime;
            GetComponent<ProcessBar>().setProcessTime(currentObject.GetComponent<PizzaMass>().getCookingTimeLeftNormalized());
            //if ( cookTime >= MaxCookTime ) currentObject.GetComponent<PizzaMass>().setDoneCooking();
        }
        else if (currentObject != null) finishOven();
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
