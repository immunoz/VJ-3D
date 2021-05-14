using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PizzaMass : Ingredient
{
    public GameObject preparedMass;
    public GameObject rawMass;
    public GameObject tomato;
    public GameObject cheese;
    public GameObject salami;

    public override float getTime()
    {
        return CuttingTime;
    }

    void Update()
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
                if (timer <= 0) {
                    state = ingredientState.CHOPPED;
                    leftCuttingTime = 0f;
                    preparedMass.SetActive(true);
                    rawMass.SetActive(false);
                }
                break;

            case ingredientState.CHOPPED:
                bChopped = false;
                if (cooked) state = ingredientState.COOKED;
                break;
            case ingredientState.COOKED:

                break;
        }

    }
    // Start is called before the first frame update
    void Start()
    {
        name = "PizzaMass";
    }
}
