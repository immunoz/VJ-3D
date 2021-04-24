using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngredientSpawner : MonoBehaviour
{
    public GameObject ingredientPrefab;
    public Player player;

    public GameObject createIngredient() {
        GameObject temp = Instantiate(ingredientPrefab) as GameObject;
        temp.transform.position = GetComponent<Renderer>().bounds.center + new Vector3(0f,7.5f,0f);
        //temp.transform.Translate(-2.5f, 5f, -5f);
        return temp;
    }
}
