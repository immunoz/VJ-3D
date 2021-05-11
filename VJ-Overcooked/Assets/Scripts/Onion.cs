using System.Collections;
using System.Collections.Generic;
using UnityEngine;

 class Onion : Ingredient
{
    private bool pot;

    private void Start()
    {
        pot = true;
        name = "Onion";
    }


    public override bool inPot()
    {
        return pot;
    }

    public override float getTime()
    {
        return CuttingTime;
    }
}
