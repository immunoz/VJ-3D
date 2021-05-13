using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Ingredient : MonoBehaviour
{


    bool readyToCut;
    public float CuttingTime;
    public bool pot;
    
    private float leftCuttingTime;
    private float timer;
    private bool bChopped;
    
    protected bool cooked;
    
    enum ingredientState
    {
       RAW, IN_PROCESS, CHOPPED, COOKED
    };

    private ingredientState state;

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
                if (timer <= 0)
                    state = ingredientState.CHOPPED;
                    leftCuttingTime = 0f;
                break;

            case ingredientState.CHOPPED:
                bChopped = false;
                if (cooked) state = ingredientState.COOKED;
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

    public bool putInPlate()
    {
        if (name == "BurgerBread") return true;
        return state == ingredientState.CHOPPED;
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

}
