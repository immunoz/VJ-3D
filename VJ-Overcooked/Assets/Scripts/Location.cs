using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract  class Location : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject currentObject;
    protected float timer;
    public float  cooldownTime;
    void Start()
    {
        timer = 0f;
        if (currentObject != null) setObject(currentObject);
 
    }

    // Update is called once per frame
    void Update()
    {
        if (timer > 0) timer -= Time.deltaTime;
    }


    public void setObject(GameObject obj)
    {
        currentObject = obj;
        if ( name != "pizza" ) setObjectPosition();
        timer = cooldownTime;
    }



    public void setObjectPosition()
    {
        Vector3 tableCenter = GetComponent<Renderer>().bounds.center;
        if ( name == "Sink") currentObject.transform.Rotate(0f, -45f, 0f);
        currentObject.transform.position = tableCenter + new Vector3(0f, getGetHeightOffset(), 0f);
    }



    public bool isFree()
    {
        return currentObject == null;
    }

    public GameObject pickObject()
    {
        GameObject result = currentObject;
        if ( name == "Sink") currentObject.transform.Rotate(0f, 45f, 0f);
        currentObject = null;
        resetCoolDown();
        return result;
    }

  

    public GameObject getObject()
    {
        return currentObject;
    }



    public bool canBeUsed()
    {
        return timer <= 0;
    }

    public bool objectCanBePickedUp()
    {
        return true;
    }
    public abstract float getGetHeightOffset();

    public  string getType()
    {
        return name;
    }

    public void resetCoolDown()
    {
        timer = cooldownTime;
    }

    public abstract bool finished();

    public bool hasPlate()
    {
        return currentObject.name == "plate";
    }
}
