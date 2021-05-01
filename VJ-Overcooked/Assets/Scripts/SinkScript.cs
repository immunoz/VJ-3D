using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class SinkScript : Location
{

    public float heightOffset;
    private bool started = false;

    public override float getGetHeightOffset()
    {
        return heightOffset;
    }


    private void LateUpdate()
    {
        if (currentObject != null && started ) GetComponent<ProcessBar>().setProcessTime(currentObject.GetComponent<Plate>().getTimeLeftNormalized());
    }
    public void startWashing()
    {
        float time = currentObject.GetComponent<Plate>().setReadyToWash();
        if (time > 0) GetComponent<ProcessBar>().setMaxTime(time);
        started = true;
    }

    public void stopWashing()
    {
        GetComponent<ProcessBar>().hide();
        started = false;
    }
    
    public void pauseWashing()
    {
        currentObject.GetComponent<Plate>().stopWashing();
        started = false;
    }

    public override bool  finished()
    {
        return currentObject.GetComponent<Plate>().doneWashing();
    }



    /*public GameObject currentPlate;
    public float cooldownTime;
    public float heightOffset;
    public float timer;

    void Start()
    {
        timer = 0;
    }


    void Update()
    {
        if (timer > 0) timer -= Time.deltaTime;
    }


    public void setPlate(GameObject plate)
    {
        currentPlate = plate;
        setPlatePosition();
        timer = cooldownTime;
    }


    public void setPlatePosition()
    {
        Vector3 tableCenter = GetComponent<Renderer>().bounds.center;
        currentPlate.transform.Rotate(0f, -45f, 0f);
        currentPlate.transform.position = tableCenter + new Vector3(0f, heightOffset, 0f);
    }


    public bool isFree()
    {
        return currentPlate == null;
    }


    public GameObject pickPlate()
    {
        GameObject result = currentPlate;
        currentPlate.transform.Rotate(0f, 45f, 0f);
        currentPlate = null;

        timer = cooldownTime;
        return result;
    }

    public GameObject getPlate()
    {
        return currentPlate;
    }

    public bool canBeUsed()
    {
        return timer <= 0;
    }

    public bool ingredientCanBePickedUp()
    {
        return currentPlate.GetComponent<Plate>().plateCanBePickedUp();
    }*/
}
