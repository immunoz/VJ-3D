using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngredientSpawner : MonoBehaviour
{
    public GameObject ingredientPrefab;
    public Player player;

    public GameObject createIngredient() {
        GameObject temp = Instantiate(ingredientPrefab) as GameObject;
        return temp;
    }
}
