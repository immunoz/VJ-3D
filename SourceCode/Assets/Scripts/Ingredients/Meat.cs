using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meat : Ingredient
{
    public float preparingTime;
    private bool burned ;

    //public bool putInPlate() {//esto se tiene que hacer abstracto
    //    return state == ingredientState.COOKED;
    //}

    public override float getTime()
    {
        return CuttingTime;
    }

    // Start is called before the first frame update
    void Start()
    {
        name = "Meat";
        burned = false;
    }

    public void setCooked(bool v)
    {
        state = ingredientState.COOKED;
        cooked = v;
    }

    public void setBurned()
    {
        burned = true;
    }

    public bool getBurned()
    {
        return burned;
    }
}
