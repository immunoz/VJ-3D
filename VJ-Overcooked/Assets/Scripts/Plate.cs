using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plate : MonoBehaviour
{
    public float washingTime;
    public GameObject stewTexture;
    public GameObject[] recipes;
    
    private float leftWasshingTime;
    private float timer;
    private bool washing;
    public List<GameObject> ingredients;
    private string preparedDish;


    enum plateState
    {
        DIRTY, IN_PROCESS, WASHED
    };

    private plateState state;


    void Start()
    {
        ingredients = new List<GameObject>();
        state = plateState.WASHED;
        initDish();
        if (recipes.Length != 0)
        {
            recipes[0] = Instantiate(recipes[0]) as GameObject;
            recipes[1] = Instantiate(recipes[0]) as GameObject;
            Debug.Log(recipes[0].name);
            foreach (string x in recipes[0].GetComponent<Recipe>().state) Debug.Log(x);
            foreach (GameObject x in recipes[0].GetComponent<Recipe>().ingredients) Debug.Log(x.name);
        }
    }

    private void initDish()
    {
        stewTexture.SetActive(false);
        washing = false;
        leftWasshingTime = washingTime;
        preparedDish = "";
    }

    public void putIngredient(GameObject carriedObject)
    {
        ingredients.Add(carriedObject);
        bool recipeFound = false;
        for (int i = 0; i < recipes.Length && !recipeFound; ++i) {
            Debug.Log(recipes.Length);
            Debug.Log(recipes[i].GetComponent<Recipe>().getSize());
            if (recipes[i].GetComponent<Recipe>().getSize() == ingredients.Count)
            {
                recipeFound = true;
                checkRecipe(i);
            }
        }
    }

    private void checkRecipe(int i)
    {
        //... codigo de comprovar los ingredientes
        Debug.Log("TEST");
    }

    void Update()
    {
        switch (state)
        {
            case plateState.DIRTY:
                if (washing)
                {
                    state = plateState.IN_PROCESS;
                    timer = leftWasshingTime;
                    washing = false;
                }
                break;
            case plateState.IN_PROCESS:
                timer -= Time.deltaTime;
                if (timer <= 0)
                {
                    state = plateState.WASHED;
                    leftWasshingTime = 0f;
                }
                break;
            case plateState.WASHED:
                washing = false;
                break;

        }
        
    }

    public float setReadyToWash()
    {
        washing = true;
        return leftWasshingTime / washingTime;
    }

    public bool doneWashing()
    {
        return state == plateState.WASHED;
    } 

    public void stopWashing()
    {

        state = plateState.DIRTY;
        washing = false;
        leftWasshingTime = timer;
    }

    public void showStew(bool value)
    {
        stewTexture.SetActive(value);
    }

    public void setStewFlavour(Material colour)
    {
        stewTexture.GetComponent<Renderer>().sharedMaterial = colour;
    }

    public bool plateCanBePickedUp()
    {
        return (state == plateState.DIRTY && leftWasshingTime == washingTime || doneWashing());
    }


    public float getTimeLeftNormalized()
    {
        return timer / washingTime;
    }

    public bool isDirty()
    {
        return state == plateState.DIRTY;
    }

    public List<GameObject> getIngredients()
    {
        return ingredients;
    }

    public void SetDirty()
    {
        for (int i = 0; i < ingredients.Count; ++i) Destroy(ingredients[i]);
        ingredients.Clear();
        state = plateState.DIRTY;
        initDish();
    }

    public void setPreparedDish(string dish) {
        preparedDish = dish;
    }

    public string getPreparedDish()
    {
        return preparedDish;
    }
}
