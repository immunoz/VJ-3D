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
    public GameObject[] recipes;

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
                else
                {
                    if (name == recipes[0].GetComponent<Recipe>().recipeName) name = recipes[0].GetComponent<Recipe>().cookedName;
                    else name = recipes[1].GetComponent<Recipe>().cookedName;
                    state = ingredientState.COOKED;
                }
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
        if (ingredients.Count >= 4 || (ingredient.name != "Tomato" && ingredient.name != "Cheese" && ingredient.name != "Sausage" && ingredient.name != "Mushroom") || state != ingredientState.CHOPPED) return false;
        Destroy(ingredient);
        ingredients.Add(ingredient.name);
        if (ingredient.name == "Tomato") tomato.SetActive(true);
        else if (ingredient.name == "Cheese") cheese.SetActive(true);
        else if (ingredient.name == "Sausage") salami.SetActive(true);

        checkRecipe();
        return true;
    }

    private void checkRecipe()
    {
        bool recipeFound = false;
        for (int i = 0; i < recipes.Length && !recipeFound; ++i)
        {
            if (recipes[i].GetComponent<Recipe>().getSize() == ingredients.Count)
            {
                if (checkRecipeII(i))
                {
                    name = recipes[i].GetComponent<Recipe>().recipeName;
                    recipeFound = true;
                }
            }
        }
    }

    private bool checkRecipeII(int i)
    {
        Recipe recipeScript = recipes[i].GetComponent<Recipe>();
        int[] tempQuantity = new int[recipeScript.quantity.Length];
        Array.Copy(recipeScript.quantity, 0, tempQuantity, 0, recipeScript.quantity.Length);

        foreach (string k in ingredients)
        {
            bool not_found = true;
            for (int j = 0; j < recipeScript.ingredients.Length && not_found; ++j)
            {
                if (recipeScript.ingredients[j].name == k)
                {
                    if (--tempQuantity[j] < 0) return false;
                    not_found = false;
                }
            }
        }

        foreach (int e in tempQuantity)
        {
            if (e != 0) return false;
        }
        return true;
    }

    public bool finished()
    {
        return state == ingredientState.COOKED;
    }

    public bool isRawPizza()
    {
        return name == "sausage pizza raw" || name == "mushroom pizza raw";
    }
}
