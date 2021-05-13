using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cabagge : Ingredient
{
    public override float getTime()
    {
        return CuttingTime;
    }

    // Start is called before the first frame update
    void Start()
    {
        name = "Cabagge";
    }
}
