using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurgerBread : Ingredient
{
    public override float getTime()
    {
        return 0;
    }

    // Start is called before the first frame update
    void Start()
    {
        name = "BurgerBread";
    }
}
