using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TableScript : MonoBehaviour
{
    private GameObject currentIngredient;
    public float heightOffset;
    public float cooldownTime;
    private float timer;

    void Start() {
        timer = 0;
    }

    void Update() {
        if (timer > 0) timer -= Time.deltaTime;
    }

    public void setIngredient(GameObject ingredient) {
        currentIngredient = ingredient;
        setIngredientPosition();
        timer = cooldownTime;
    }

    private void setIngredientPosition() {
        Vector3 tableCenter = GetComponent<Renderer>().bounds.center;
        currentIngredient.transform.position = tableCenter + new Vector3(0f, heightOffset, 0f);
    }

    public bool isFree() {
        return currentIngredient == null;
    }

    public GameObject pickIngredient() {
        GameObject result = currentIngredient;
        currentIngredient = null;
        timer = cooldownTime;
        return result;
    }

    public GameObject getIngredient ()
    {
        return currentIngredient;
    }

    public bool canBeUsed() {
        return timer <= 0;
    }

    public bool ingredientCanBePickedUp()
    {
        return currentIngredient.GetComponent<Ingredient>().ingredientCanBePickedUp();
    }
}
