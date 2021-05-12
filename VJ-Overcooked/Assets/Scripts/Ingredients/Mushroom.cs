using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mushroom : Ingredient
{
    public override float getTime()
    {
        return CuttingTime;
    }

    void Start()
    {
        name = "Mushroom";
    }
}
