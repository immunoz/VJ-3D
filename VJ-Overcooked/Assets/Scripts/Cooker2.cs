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

        
        if ( currentObject != null && currentObject.GetComponent<Pot>().doneCooking() && !currentObject.GetComponent<Pot>().isBurned() )
        {
            Pot potSricpt = currentObject.GetComponent<Pot>();
            potSricpt.hideBar();
            warning.SetActive(true);
            if (delay > 0) delay -= Time.deltaTime;
            
            else setOnFire();
            //Debug.Log(delay);
        }
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
        //asumimos que solo pueden haber ollas y sartenes sobre la cocina
        if ( currentObject != null && obj.name != "plate")
        {
            Pot pot = currentObject.GetComponent<Pot>();
            
            bool set = pot.setObject(obj);
            if ( set ) resetCoolDown();
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
            Debug.Log("setOnfire");
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
}
