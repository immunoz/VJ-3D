using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ingredient : MonoBehaviour
{


    bool readyToCut;
    public float CuttingTime;
    private float timer;

    enum ingredientState
    {
       RAW, IN_PROCESS, CHOPPED
    };

    private ingredientState state;

    // Start is called before the first frame update
    void Start()
    {
        state = ingredientState.RAW;
        readyToCut = false;

    }

    // Update is called once per frame
    void Update()
    {

        switch (state)
        {
            case ingredientState.RAW:
                if (readyToCut) {
                    state = ingredientState.IN_PROCESS;
                    timer = CuttingTime; 
                } 
                break;
            case ingredientState.IN_PROCESS:
                timer -= Time.deltaTime;
                if (timer <= 0)
                    state = ingredientState.CHOPPED;
                Debug.Log("holla");
                break;

            case ingredientState.CHOPPED:
                Debug.Log("Chopped");
                break;
        }
        
    }



    public void setReadyToCut()
    {
        readyToCut = true;
    }

    public bool choppingDone()
    {
        return state == ingredientState.CHOPPED;
    }


    public void stopCutting()
    {
        state = ingredientState.RAW;
        readyToCut = false;
        CuttingTime = timer;
        Debug.Log(CuttingTime);
    }
}
