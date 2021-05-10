using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plate : MonoBehaviour
{

    public float washingTime;
    
    private float leftWasshingTime;
    private float timer;
    private bool washing;
    public List<GameObject> ingredients;


    enum plateState
    {
        DIRTY, IN_PROCESS, WASHED
    };

    private plateState state;


    void Start()
    {
        ingredients = new List<GameObject>();
        washing = false;
        state = plateState.WASHED;
        leftWasshingTime = washingTime;
    }

    public void putIngredient(GameObject carriedObject)
    {
        ingredients.Add(carriedObject);
    }

    void Update()
    {
        switch (state)
        {
            case plateState.DIRTY:
                if ( washing)
                {
                    state = plateState.IN_PROCESS;
                    timer = leftWasshingTime;
                    washing = false;
                }
                break;
            case plateState.IN_PROCESS:
                timer -= Time.deltaTime;
                if (timer <= 0)
                {
                    state = plateState.WASHED;
                    leftWasshingTime = 0f;
                }
                break;
            case plateState.WASHED:
                washing = false;
                break;

        }
        
    }

    public float setReadyToWash()
    {
        washing = true;
        return leftWasshingTime / washingTime;
    }

    public bool doneWashing()
    {
        return state == plateState.WASHED;
    } 

    public void stopWashing()
    {

        state = plateState.DIRTY;
        washing = false;
        leftWasshingTime = timer;
    }

    public bool plateCanBePickedUp()
    {
        return (state == plateState.DIRTY && leftWasshingTime == washingTime || doneWashing());
    }


    public float getTimeLeftNormalized()
    {
        return timer / washingTime;
    }

    public bool isDirty()
    {
        return state == plateState.DIRTY;
    }
}
