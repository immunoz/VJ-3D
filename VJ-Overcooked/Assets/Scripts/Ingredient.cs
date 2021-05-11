using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Ingredient : MonoBehaviour
{


    bool readyToCut;
    public float CuttingTime;
    private float leftCuttingTime;
    private float timer;
    private bool bChopped;
    
    enum ingredientState
    {
       RAW, IN_PROCESS, CHOPPED
    };

    private ingredientState state;

    // Start is called before the first frame update
    void Awake()
    {
        state = ingredientState.RAW;
        readyToCut = false;
        bChopped = false;
        leftCuttingTime = getTime();
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
                break;
            case ingredientState.IN_PROCESS:
                timer -= Time.deltaTime;
                if (timer <= 0)
                    state = ingredientState.CHOPPED;
                    leftCuttingTime = 0f;
                break;

            case ingredientState.CHOPPED:
                bChopped = false;
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

    public abstract bool inPot();

    public abstract float getTime();

}
