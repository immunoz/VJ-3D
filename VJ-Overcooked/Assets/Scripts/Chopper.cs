using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

 class Chopper : Location
{
    public float heightOffset;
    public GameObject explosion;
    private bool started = false;
    

    public override float getGetHeightOffset()
    {
        return heightOffset;
    }

    private void LateUpdate()
    {
       if ( currentObject != null && started )  GetComponent<ProcessBar>().setProcessTime(currentObject.GetComponent<Ingredient>().getTimeLeftNormalized());
    }


    public void startChopp()
    {
        float time = currentObject.GetComponent<Ingredient>().setReadyToCut();
        started = true;
        if ( time > 0 ) GetComponent<ProcessBar>().setMaxTime(time);
    }

    public void stopChopp()
    {
        GetComponent<ProcessBar>().hide();
        explosion.SetActive(true);
        started = false;
    }

    public void pauseChopping()
    {
        currentObject.GetComponent<Ingredient>().stopCutting();
        started = false;
    }

    public override bool finished()
    {
        return currentObject.GetComponent<Ingredient>().choppingDone();
    }

    public override void setOnFire()
    {
        if (!onFire)
        {
            onFire = true;
            flame.SetActive(true);
        }
    }

    public void hideExplosion()
    {
        explosion.SetActive(false);
    }

    public void setChoppingFinished()
    {
        if (currentObject != null) {
            stopChopp();
            if ( currentObject.name != "PizzaMass" )currentObject.GetComponent<Ingredient>().setChoppedFinished();
            else currentObject.GetComponent<PizzaMass>().setChoppedFinished();
        }
        

        
    }
}
