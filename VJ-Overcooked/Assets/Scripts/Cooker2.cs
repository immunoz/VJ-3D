using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

 class Cooker2 : Location
{
    public float heightOffset;

    public GameObject warning;

    public float delay;

    void Start()
    {
        delay = 0.0f;
    }

    void FixedUpdate()
    {
       
        
        if ( currentObject != null)
        {


            
            if (currentObject.name == "pot" && currentObject.GetComponent<Pot>().doneCooking())
            {
                Pot potSricpt = currentObject.GetComponent<Pot>();
                if (delay > 0) delay -= Time.deltaTime;
                potSricpt.hideBar();
                warning.SetActive(true);
                if (delay <= 0) {
                    warning.SetActive(false);
                    delay = 0.5f;
                } 
            }

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
}
