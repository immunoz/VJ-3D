using System.Collections;
using System.Collections.Generic;
using UnityEngine;

 class Onion : Ingredient
{
    void Start()
    {
        pot = true;
        name = "Onion";
    }

    public override float getTime()
    {
        return CuttingTime;
    }
}
