using System;
using System.Collections.Generic;
using UnityEngine;

public class Recipe : MonoBehaviour
{
    public GameObject[] ingredients;
    public string[] state;
    public int[] quantity;
    public string recipeName;

    public int getSize() {
        int result = 0;
        foreach(int x in quantity) result += x;
        return result;
    }
}
