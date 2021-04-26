using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TableScript : MonoBehaviour
{
    private GameObject currentIngredient;
    public float heightOffset;

    public void setIngredient(GameObject ingredient) {
        if (currentIngredient == null) Debug.Log("isFreee");
        currentIngredient = ingredient;
        setIngredientPosition();
    }

    private void setIngredientPosition() {
        Vector3 tableCenter = GetComponent<Renderer>().bounds.center;
        currentIngredient.transform.position = tableCenter + new Vector3(0f, heightOffset, 0f);
    }

    public bool isFree() {
        return currentIngredient == null;
    }

    public GameObject getIngredient() {
        GameObject result = currentIngredient;
        currentIngredient = null;
        return result;
    }
}
