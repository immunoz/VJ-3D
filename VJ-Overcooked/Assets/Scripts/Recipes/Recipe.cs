using System;
using System.Collections.Generic;
using UnityEngine;

public class Recipe : MonoBehaviour
{
    public GameObject[] ingredients;
    public int[] quantity;

    public int getSize() {
        int result = 0;
        foreach(int x in quantity) result += x;
        return result;
    }

    public bool checkRecipe(List<GameObject> inputIngredients) { //tal vez el array no se copie y sea una copia por referencia
        if (inputIngredients.Capacity - 1 != getSize()) return false;
        
        int[] tempQuantity = new int[quantity.Length];
        Array.Copy(quantity, 0, tempQuantity, 0, quantity.Length);
        foreach (int x in tempQuantity) Debug.Log(x);

        foreach (GameObject i in inputIngredients) {
            bool not_found = true;
            for (int j = 0; j < ingredients.Length && not_found; ++j) {
                if (ingredients[j].name == i.name) {
                    if (--tempQuantity[j] < 0) return false;
                }
            }
        }

        foreach (int e in tempQuantity) {
            if (e != 0) return false;
        }
        return true;
    }
}
