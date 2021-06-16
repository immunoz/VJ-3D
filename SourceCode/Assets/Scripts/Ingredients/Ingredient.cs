using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Ingredient : MonoBehaviour
{
    public float CuttingTime;
    public bool pot;
    public GameObject raw, sliced;


    protected float leftCuttingTime;
    protected float timer;
    protected bool bChopped;
    protected bool readyToCut;
    protected bool cooked;

    protected enum ingredientState
    {
        RAW, IN_PROCESS, CHOPPED, COOKING, COOKED
    };

    protected ingredientState state;

    public void updatePieces(GameObject raw, GameObject sliced) {
        Debug.Log(raw.name + sliced.name);
        //r = raw;
        //s = sliced;
    }

    // Start is called before the first frame update
    void Awake()
    {
        state = ingredientState.RAW;
        readyToCut = false;
        bChopped = false;
        leftCuttingTime = getTime();
        cooked = false;

    }

    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            case ingredientState.RAW:
                if (readyToCut) {
                    state = ingredientState.IN_PROCESS;
                    timer = leftCuttingTime;
                    bChopped = true;
                }
                if (cooked) state = ingredientState.COOKED;
                break;
            case ingredientState.IN_PROCESS:
                timer -= Time.deltaTime;
                if (timer <= 0) {
                    state = ingredientState.CHOPPED;
                    leftCuttingTime = 0f;
                    if (raw != null) raw.SetActive(false);
                    if (sliced != null) sliced.SetActive(true);
                }
                break;

            case ingredientState.CHOPPED:
                bChopped = false;
                break;
            case ingredientState.COOKED:

                break;
        }

    }



    public float setReadyToCut()
    {
        readyToCut = true;
        //Debug.Log(leftCuttingTime);


        return leftCuttingTime / getTime();
    }

    public void setChoppedFinished()
    {
        state = ingredientState.CHOPPED;
        leftCuttingTime = 0f;
        if (raw != null) raw.SetActive(false);
        if (sliced != null) sliced.SetActive(true);
    }

    public bool putInPlate()
    {
        if (name == "BurgerBread") return true;
        if (name == "PizzaMass" || name == "mushroom pizza raw" || name == "sausage pizza raw" || (name == "Meat" && state != ingredientState.COOKED)) return false;
        return state == ingredientState.CHOPPED || state == ingredientState.COOKED;
    }

    public bool choppingDone()
    {
        return state == ingredientState.CHOPPED;

    }

    public bool ingredientCanBePickedUp()
    {
        return (state == ingredientState.RAW && leftCuttingTime == getTime()) || choppingDone();
    }

    public void stopCutting()
    {
        state = ingredientState.RAW;
        readyToCut = false;
        leftCuttingTime = timer;
    }

    public float getTimeLeftNormalized()
    {
        return timer / getTime();
    }

    public bool inPot()
    {
        return pot;
    }

    public abstract float getTime();

    public string getState()
    {
        switch (state)
        {
            case ingredientState.RAW:
                return "Raw";
                break;
            case ingredientState.CHOPPED:
                return "Chopped";
                break;
            case ingredientState.COOKED:
                return "Cooked";
                break;
        }
        return "";
    }

    public  bool isChopped()
    {
        return state == ingredientState.CHOPPED;
    }
}
