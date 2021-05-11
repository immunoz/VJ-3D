using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pizza : MonoBehaviour
{
    private bool startCooking,done;
    public int count;
    private List<string> ingredients;
    enum pizzaState
    {
        RAW, IN_PROCESS,COOKED
    };

    private pizzaState state;
    void Start()
    {
        startCooking = false;
        ingredients = new List<string>();
        done = false;
        state = pizzaState.RAW;
        count = 0;
    }

    // Update is called once per frame
    void Update()
    {
        switch(state)
        {
            case pizzaState.RAW:
                if ( startCooking) state = pizzaState.IN_PROCESS;
                break;
            case pizzaState.IN_PROCESS:
                if ( done ) state = pizzaState.COOKED;
                break;
            case pizzaState.COOKED:
                break;
        }
        
    }

    public void setDoneCooking()
    {
        done = true;
    }

    public void startCookingPizza ()
    {
        startCooking = true;
    }

    public bool finished()
    {
        return state == pizzaState.COOKED;
    }

    public void addIngredient(GameObject obj)
    {
        if ( count < 4 && !ingredients.Contains(obj.name) )
        {
            ingredients.Add(obj.name);
            count++;
        }
    }
   
}
