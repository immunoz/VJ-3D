using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SinkScript : MonoBehaviour
{

    public GameObject currentPlate;
    public float cooldownTime;
    public float heightOffset;
    public float timer;

    void Start()
    {
        timer = 0;
    }


    void Update()
    {
        if (timer > 0) timer -= Time.deltaTime;
    }


    public void setPlate(GameObject plate)
    {
        currentPlate = plate;
        setPlatePosition();
        timer = cooldownTime;
    }


    public void setPlatePosition()
    {
        Vector3 tableCenter = GetComponent<Renderer>().bounds.center;
        currentPlate.transform.Rotate(0f, -45f, 0f);
        currentPlate.transform.position = tableCenter + new Vector3(0f, heightOffset, 0f);
    }


    public bool isFree()
    {
        return currentPlate == null;
    }


    public GameObject pickPlate()
    {
        GameObject result = currentPlate;
        currentPlate.transform.Rotate(0f, 45f, 0f);
        currentPlate = null;

        timer = cooldownTime;
        return result;
    }

    public GameObject getPlate()
    {
        return currentPlate;
    }

    public bool canBeUsed()
    {
        return timer <= 0;
    }

    public bool ingredientCanBePickedUp()
    {
        return currentPlate.GetComponent<Plate>().plateCanBePickedUp();
    }
}
