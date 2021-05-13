using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meat : Ingredient
{
    public float preparingTime;

    public override float getTime()
    {
        return CuttingTime;
    }

    // Start is called before the first frame update
    void Start()
    {
        name = "Meat";
    }

    public void setCooked(bool v)
    {
        cooked = true;
    }
}
