using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

 class Cooker2 : Location
{
    public float heightOffset;

    public GameObject warning;
    public GameObject flame;
    public float delay;
    public float scaleDelay;
    public Vector3 scale;
    private Vector3 threshHold;

    void Start()
    {
        threshHold = new Vector3(2.5f, 2.5f, 2.5f);
        if (currentObject.name == "pot") flame.transform.position = GetComponent<Renderer>().bounds.center;
    }

    void FixedUpdate()
    {

        
        if ( currentObject != null)
        {


            
            if (currentObject.name == "pot" && currentObject.GetComponent<Pot>().doneCooking())
            {
                Pot potSricpt = currentObject.GetComponent<Pot>();
                if (delay > 0) delay -= Time.deltaTime;
                if (scaleDelay > 0) scaleDelay -= Time.deltaTime;
                potSricpt.hideBar();
                warning.SetActive(true);
                if (delay <= 0) {
                    warning.SetActive(false);
                    flame.SetActive(true);
                    if (scaleDelay <= 0)
                    {
                        expand();
                        scaleDelay = 0.2f;

                    }
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

    public void expand()
    {
        //
        if (flame.transform.GetChild(0).transform.localScale.x < threshHold.x)
        {
            flame.transform.GetChild(0).transform.localScale += scale;
            flame.transform.GetChild(1).transform.localScale += scale;
            flame.transform.GetChild(2).transform.localScale += scale;
            flame.transform.GetChild(3).transform.localScale += scale;
        }

    }
}
