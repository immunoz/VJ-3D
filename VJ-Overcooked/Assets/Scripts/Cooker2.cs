using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

 class Cooker2 : Location
{
    public float heightOffset;

    public GameObject warning;
    public float delay;

    void FixedUpdate()
    {
        if (currentObject != null && currentObject.name == "pan") {
            if (currentObject.GetComponent<Pan>().isReady() && !currentObject.GetComponent<Pan>().burned()) showWarning();
            return;
        }
        
        
        
        if ( currentObject != null && currentObject.GetComponent<Pot>().doneCooking() && !currentObject.GetComponent<Pot>().isBurned() )
        {
            Pot potSricpt = currentObject.GetComponent<Pot>();
            potSricpt.hideBar();
            showWarning();
            //Debug.Log(delay);
        }
    }

    private void showWarning() {
        warning.SetActive(true);
        if (delay > 0) delay -= Time.deltaTime;
        else setOnFire();
    }

    public override float getGetHeightOffset()
    {
        return heightOffset;
    }

    public override bool finished()
    {
        return true;
    }

    public bool addIngredient(GameObject obj)
    {
        bool set;
        //asumimos que solo pueden haber ollas y sartenes sobre la cocina
        if (currentObject != null && currentObject.name == "pan" && obj.name != "plate") {
            if (obj.name != "Meat" || !obj.GetComponent<Meat>().choppingDone()) return false;
            Pan panScript = currentObject.GetComponent<Pan>();
            set = panScript.setObject(obj);
            if (set) resetCoolDown();
            return set;
        }
        else if (currentObject != null && obj.name != "plate")
        {
            Pot pot = currentObject.GetComponent<Pot>();
            set = pot.setObject(obj);
            if (set) resetCoolDown();
            return set;
        }
        return false;
    }

    public void hideWarning()
    {
        warning.SetActive(false);
    }

    public override void setOnFire()
    {
        if (!onFire)
        {
            warning.SetActive(false);
            onFire = true;
            flame.SetActive(true);
            currentObject.GetComponent<Pot>().setBurned(true);
            delay = 3; // poner en una  funcion inicializadora.
        }
    }

    public bool hasStew()
    {
        return currentObject.GetComponent<Pot>().hasStew();
    }

    public void GetStew(GameObject plate)
    {
        Plate plateScript = plate.GetComponent<Plate>();
        if (!plateScript.isDirty()) currentObject.GetComponent<Pot>().getStew(plate);
    }

    public bool hasMeat()
    {
        return currentObject.GetComponent<Pan>().hasMeat();
    }

    internal void GetMeat(GameObject plate)
    {
        Plate plateScript = plate.GetComponent<Plate>();
        if (!plateScript.isDirty()) currentObject.GetComponent<Pan>().GetMeat(plate);
    }
}
