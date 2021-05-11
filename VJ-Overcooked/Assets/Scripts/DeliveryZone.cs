using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class DeliveryZone : Location
{
    public override bool finished()
    {
        return false;
    }

    public override float getGetHeightOffset()
    {
        throw new System.NotImplementedException();
    }

    public void setObject(GameObject obj)
    {
        currentObject = obj;
        currentObject.SetActive(false);
        GameManager gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        if (gameManager.deliverPlate(obj)) gameManager.increaseScore();
        timer = cooldownTime;
    }

    public override void setOnFire()
    {
        throw new System.NotImplementedException();
    }
}
