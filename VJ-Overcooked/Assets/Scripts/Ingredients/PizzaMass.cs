using System;
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
    public float cookingTime;

    private List<string> ingredients;
    private bool cooking;

    public override float getTime()
    {
        return CuttingTime;
    }

    public float getCookingTimeLeftNormalized()
    {
        return timer / cookingTime;
    }

    // Start is called before the first frame update
    void Start()
    {
        name = "PizzaMass";
        ingredients = new List<string>();
        cooking = false;
    }

    internal void setDoneCooking()
    {
        cooked = true;
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
                if (cooking)
                {
                    timer = cookingTime;
                    state = ingredientState.COOKING;
                }
                break;

            case ingredientState.COOKING:
                if (timer >= 0) timer -= Time.deltaTime;
                else state = ingredientState.COOKED;
                break;

            case ingredientState.COOKED:

                break;
        }
    }

    internal void startCookingPizza()
    {
        cooking = true;
    }

    public bool putIngredient(GameObject ingredient) {
        if (ingredients.Count >= 4 || (ingredient.name != "Tomato" && ingredient.name != "Cheese" && ingredient.name != "Sausage")) return false;
        Destroy(ingredient);
        ingredients.Add(ingredient.name);
        if (ingredient.name == "Tomato") tomato.SetActive(true);
        else if (ingredient.name == "Cheese") cheese.SetActive(true);
        else if (ingredient.name == "Sausage") salami.SetActive(true);
        return true;
    }


    public bool finished()
    {
        return state == ingredientState.COOKED;
    }
}
