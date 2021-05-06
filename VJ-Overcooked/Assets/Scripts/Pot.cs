using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pot : MonoBehaviour
{



    public int count;
    public string ingredient;
    private float cookTime;
    public float MaxCookTime;

    void Start()
    {
        count = 0;
        ingredient = "";
        cookTime = 0;
    }

    private void Update()
    {
        if (count != 0 && cookTime < MaxCookTime * count / 3) {
            cookTime += Time.deltaTime;
            //GetComponent<ProcessBar>().setProcessTime(cookTime/MaxCookTime); //cookTime - (cookTime / (MaxCookTime * count))
            GetComponent<ProcessBar>().setProcessTime(cookTime/ (MaxCookTime * count / 3)); //cookTime - (cookTime / (MaxCookTime * count))
        }
    }

    public bool setObject(GameObject obj)
    {
        Ingredient ingredientScript = obj.GetComponent<Ingredient>();
        if (ingredientScript.inPot() && ingredientScript.choppingDone() && (count == 0 || obj.name == ingredient && count < 3) ) {
            count++;
            //if (count > 1) cookTime -= MaxCookTime / count;
            //cookTime = cookTime - (cookTime / (MaxCookTime * count-1));
            //cookTime /= MaxCookTime;
            GetComponent<ProcessBar>().setMaxTime(1);
            ingredient = obj.name;
            Destroy(obj);
            return true;
        }
        return false;
    }


}
