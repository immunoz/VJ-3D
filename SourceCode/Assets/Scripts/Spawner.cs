using System.Collections;
using System.Collections.Generic;
using UnityEngine;

 class Spawner : Location
{

    public GameObject ingredientPrefab;
    public float heightOffset;

    public GameObject createIngredient()
    {
        FindObjectOfType<AudioManager>().play("OpenChest");
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

    public override void setOnFire()
    {
        if (!onFire)
        {
            onFire = true;
            flame.SetActive(true);
        }
    }
}
