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

    private States state;
    private float timer;
    private GameObject meat;
    private bool pause;

    // Start is called before the first frame update
    void Start()
    {
        state = States.IDLE;
        timer = 0;
        pause = false;
    }

    // Update is called once per frame
    void Update()
    {
        switch (state) {
            case States.IDLE:
                if (meat != null)
                {
                    state = States.COOKING;
                    GetComponent<ProcessBar>().setMaxTime(1);
                }

                break;
            case States.COOKING:
                if (pause) break;
                if (timer > 0)
                {
                    timer -= Time.deltaTime;
                    float preparationTime = meat.GetComponent<Meat>().preparingTime;
                    GetComponent<ProcessBar>().setProcessTime((preparationTime - timer)/preparationTime);
                    break;
                }

                state = States.READY;
                timer = burnedTime;
                meat.GetComponent<Meat>().setCooked(true);
                GetComponent<ProcessBar>().hide();
                break;
            case States.READY:
                if (timer > 0) timer -= Time.deltaTime;
                else {
                    state = States.BURNED;
                    //init fire...¿?
                }
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
}
