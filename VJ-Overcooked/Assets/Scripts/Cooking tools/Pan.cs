using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pan : MonoBehaviour
{
    enum States
    {
        IDLE, COOKING, READY, BURNED
    }
    
    public float burnedTime;
    public GameObject friedMeat;
    public Material burnedMaterial;

    private States state;
    private float timer;
    private GameObject meat;
    private bool pause;
    private bool fireDisable;

    public bool isReady()
    {
        return state == States.READY;
    }

    public bool burned()
    {
        return state == States.BURNED;
    }

    // Start is called before the first frame update
    void Start()
    {
        initPan();
        fireDisable = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.F)) fireDisable = true;
        switch (state) {
            case States.IDLE:
                if (meat != null && ! meat.GetComponent<Meat>().getBurned())
                {
                    state = States.COOKING;
                    GetComponent<ProcessBar>().setMaxTime(1);
                }

                break;
            case States.COOKING:
                if (pause) return;
                if (timer > 0)
                {
                    timer -= Time.deltaTime;
                    float preparationTime = meat.GetComponent<Meat>().preparingTime;
                    GetComponent<ProcessBar>().setProcessTime((preparationTime - timer) / preparationTime);
                }
                else
                {
                    state = States.READY;
                    timer = burnedTime;
                    meat.GetComponent<Meat>().setCooked(true);
                    GetComponent<ProcessBar>().hide();
                    
                }
                break;
            case States.READY:
                if (pause || fireDisable) return;
                //if (timer > 0) timer -= Time.deltaTime;
                //else {
                //    state = States.BURNED;
                //}
                break;
            case States.BURNED:
                
                break;
        }
    }



    public bool setObject(GameObject obj)
    {
        if (meat != null) return false;
        meat = obj;
        timer = obj.GetComponent<Meat>().preparingTime;
        //Destroy(obj);
        obj.SetActive(false);
        friedMeat.SetActive(true);
        return true;
    }

    public void setPause(bool value)
    {
        pause = value;
    }

    public void resume()
    {
        if (meat == null) return;
        setPause(false);
    }

    public bool hasMeat()
    {
        return state == States.READY;
    }

    public bool GetMeat(GameObject plate)
    {
        Plate plateScript = plate.GetComponent<Plate>();
        bool result = plateScript.putIngredient(meat);
        if (result)
        {
            meat = null;
            initPan();
        }
        return result;
    }

    public void initPan()
    {
        friedMeat.SetActive(false);
        state = States.IDLE;
        timer = 0;
        pause = false;
        if (meat != null) Destroy(meat);
        meat = null;
    }

    public void setBurned() {
        state = States.BURNED;
        meat.GetComponent<Meat>().setBurned();
        Renderer rend = friedMeat.GetComponent<Renderer>();
        rend.sharedMaterial = burnedMaterial;
    }
}
