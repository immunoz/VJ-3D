using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pot : MonoBehaviour
{



    public int count;
    public string ingredient;

    void Start()
    {
        count = 0;
        ingredient = "";
    }

    public bool setObject(GameObject obj)
    {
        Ingredient ingredientScript = obj.GetComponent<Ingredient>();
        if (ingredientScript.inPot() && ingredientScript.choppingDone() && (count == 0 || obj.name == ingredient && count < 3) ) {
            count++;
            ingredient = obj.name;
            Destroy(obj);
            return true;
        }
        return false;
    }


}
