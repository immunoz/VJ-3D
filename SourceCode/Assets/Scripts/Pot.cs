using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pot : MonoBehaviour
{
    public int count;
    public string ingredient;
    public Material[] stewColour;
    public float MaxCookTime;
    public GameObject stewTexture;

    private float cookTime;
    private bool stop;
    private bool burned;

    void Start()
    {
        initCooker();
    }

    private void initCooker()
    {
        stewTexture.SetActive(false);
        count = 0;
        ingredient = "";
        cookTime = 0;
        stop = false;
        burned = false;
    }

    public void hideBar()
    {
        GetComponent<ProcessBar>().hide();
    }

    private void Update()
    {
        if (count != 0 && cookTime < MaxCookTime * count / 3 && !stop && !burned)
        {
            cookTime += Time.deltaTime;
            GetComponent<ProcessBar>().setProcessTime(cookTime / (MaxCookTime * count / 3));
        }
        else if (count != 0 && !stop && !burned)
        {
            GetComponent<AudioSource>().Stop();
            hideBar();
        }
    }

    internal void throwInBin()
    {
        initCooker();
    }

    public bool setObject(GameObject obj)
    {
        Ingredient ingredientScript = obj.GetComponent<Ingredient>();
        if (ingredientScript.inPot() && ingredientScript.choppingDone() && (count == 0 || obj.name == ingredient && count < 3) ) {
            count++;
            if (count == 1) GetComponent<AudioSource>().Play();
            GetComponent<ProcessBar>().setMaxTime(1);
            ingredient = obj.name;
            updateStewTexture();
            Destroy(obj);
            return true;
        }
        return false;
    }

    private void updateStewTexture()
    {
        stewTexture.SetActive(true);
        Renderer rend = stewTexture.GetComponent<Renderer>();
        if ( burned) rend.sharedMaterial = stewColour[3];
        else if (ingredient == "Onion") rend.sharedMaterial = stewColour[0];
        else if (ingredient == "Tomato") rend.sharedMaterial = stewColour[1];
        else rend.sharedMaterial = stewColour[2];
    }

    public void stopCooking()
    {
        GetComponent<AudioSource>().Stop();
        stop = true;
    }

    public bool hasElement()
    {
        return count >= 1;
    }

    public void continueCooking()
    {
        GetComponent<AudioSource>().Play();
        stop = false;
    }

    public bool doneCooking()
    {
        return   count != 0 && cookTime >= MaxCookTime * count / 3;
    }

    
    public bool hasStew()
    {
        return count == 3 && cookTime >= MaxCookTime;
    }

    //Pre: Pot must have stew (hasStew())
    public void getStew(GameObject plate)
    {
        if (burned) return;
        if (ingredient == "Onion") getStewAux(plate,0,"Onion Stew");
        else if (ingredient == "Tomato") getStewAux(plate, 1, "Tomato Stew");
        else getStewAux(plate, 2, "Mushroom Stew");
        
        initCooker();
    }

    private void getStewAux(GameObject plate, int index, string dishName) {
        Plate plateScript = plate.GetComponent<Plate>();
        plateScript.showStew(true);
        plateScript.setStewFlavour(stewColour[index]);
        plateScript.setPreparedDish(dishName);
    }

    public bool isBurned ()
    {
        return burned;
    }

    public void setBurned(bool value)
    {
        burned = value;
        if (burned && hasElement()) updateStewTexture();
    }

    public void finishCooking()
    {
        if (count == 3)
        {
            cookTime = MaxCookTime;
            GetComponent<AudioSource>().Stop();
        }

    }
}
