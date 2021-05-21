using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meat : Ingredient
{
    public float preparingTime;
    public GameObject rawMeat;
    public GameObject rawMeat2;

    private void Update()
    {
        switch (state)
        {
            case ingredientState.RAW:
                if (readyToCut)
                {
                    state = ingredientState.IN_PROCESS;
                    timer = leftCuttingTime;
                    bChopped = true;
                }
                if (cooked) state = ingredientState.COOKED;
                break;
            case ingredientState.IN_PROCESS:
                timer -= Time.deltaTime;
                if (timer <= 0)
                    state = ingredientState.CHOPPED;
                leftCuttingTime = 0f;
                break;

            case ingredientState.CHOPPED:
                bChopped = false;
                rawMeat.SetActive(false);
                rawMeat2.SetActive(true);
                if (cooked) state = ingredientState.COOKED;
                break;
            case ingredientState.COOKED:

                break;
        }
    }

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
