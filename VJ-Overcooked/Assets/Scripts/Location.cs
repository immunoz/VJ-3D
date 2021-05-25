using UnityEngine;
using System.Collections.Generic;

abstract class Location : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject currentObject;
    protected float timer;
    public float  cooldownTime;
    public float scaleDelay;
    private Vector3 threshHold;

    public GameObject flame;
    public List<GameObject> nearComponents;


    //---------------------
    //Fire upgrades
    public List<float> timers;
    public float fireExpandTime;
    private bool maxFlame;
    //---------------------

    protected bool onFire;
    void Start()
    {
        nearComponents = new List<GameObject>();
        timers = new List<float>();
        timer = 0f;
        onFire = false;
        maxFlame = false;
        threshHold = new Vector3(2.5f, 2.5f, 2.5f);
        if (currentObject != null) setObject(currentObject);
        if (flame != null) flame.transform.position = GetComponent<Renderer>().bounds.center + new Vector3 (0,2f,0);
    }

    // Update is called once per frame
    void Update()
    {
        if (timer > 0) timer -= Time.deltaTime;
        if (onFire && scaleDelay > 0) scaleDelay -= Time.deltaTime;
        if (maxFlame) setNearObjectsOnFire();
        
        if (onFire && scaleDelay <= 0)
        {
            expand(new Vector3 (0.3f,0.3f,0.3f));
            scaleDelay = 0.2f;
        }
        //else if (onFire &&  expandDelay <= 0 ) setNearObjectsOnFire();
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
        if ( currentObject.name != "extinguisher" )currentObject.transform.position = tableCenter + new Vector3(0f, getGetHeightOffset(), 0f);
        else currentObject.transform.position = tableCenter + new Vector3(0f, 15f, 0f);
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

    public abstract void setOnFire();

    public void expand(Vector3 scale)
    {
        ProcessBar processScript = flame.GetComponent<ProcessBar>();
        if (flame.transform.GetChild(0).transform.localScale.x < threshHold.x || scale.x < 0)
        {
            flame.transform.GetChild(0).transform.localScale += scale;
            flame.transform.GetChild(1).transform.localScale += scale;
            flame.transform.GetChild(2).transform.localScale += scale;
            flame.transform.GetChild(3).transform.localScale += scale;
        }
        else if (flame.transform.GetChild(0).transform.localScale.x >= threshHold.x && !maxFlame)
        {
            maxFlame = true;
            processScript.hide();
        }

        
        if (!maxFlame)
        {
            processScript.setMaxTime(1);
            processScript.setProcessTime(flame.transform.GetChild(0).transform.localScale.x / threshHold.x);
        }
    }

    private void setNearObjectsOnFire()
    {
        /*for (int i = 0; i < nearComponents.Length; ++i)
        {
            if (!nearComponents[i].GetComponent<Location>().burnning())  nearComponents[i].GetComponent<Location>().setOnFire();
        }
        expandDelay = 5f;*/
        for (int i = 0; i < nearComponents.Count; ++i)
        {
            if (!nearComponents[i].GetComponent<Location>().burnning() && timers[i] > 0) timers[i] -= Time.deltaTime;
            else if (!nearComponents[i].GetComponent<Location>().burnning())
            {
                timers[i] = fireExpandTime;
                nearComponents[i].GetComponent<Location>().setOnFire();
            }
            else if (nearComponents[i].GetComponent<Location>().burnning() && timers[i] < fireExpandTime) timers[i] = fireExpandTime;
        }

    }

   public bool burnning ()
    {
        return onFire;
    }

    public void turnOffFire()
    {
        if (flame.transform.GetChild(0).transform.localScale.x > 0) expand(new Vector3(-0.1f, -0.1f, -0.1f));
        else
        {
            flame.SetActive(false);
            onFire = false;
            flame.GetComponent<ProcessBar>().hide();
            /*if (currentObject.name == "pot") currentObject.GetComponent<Pot>().setBurned(false);
            else if (currentObject.name == "pan") currentObject.GetComponent<Pan>().turnedOff();*/
        }
        maxFlame = false;
    }

    public bool hasPizzaMass()
    {
        return currentObject.name == "PizzaMass";
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (!collider.isTrigger && collider.gameObject.name != "Player" && collider.gameObject.name != "IngredientSpawner") 
        {
            nearComponents.Add(collider.gameObject);
            timers.Add(fireExpandTime);
        }
    }
}
