using System.Collections;
using System.Collections.Generic;
using UnityEngine;

 class Spawner : Location
{

    public GameObject ingredientPrefab;
    public float heightOffset;

    public GameObject createIngredient()
    {
        GameObject temp = Instantiate(ingredientPrefab) as GameObject;
        temp.name = "onion";
        return temp;
    }
    

    public override float getGetHeightOffset()
    {
        return heightOffset;
    }
    public override bool finished()
    {
        return true;
    }
}
