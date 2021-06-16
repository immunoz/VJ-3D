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
        //currentObject = obj;
        obj.SetActive(false);
        GameManager gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        gameManager.deliverPlate(obj);
        gameManager.AddPlateTimer(obj);
        timer = cooldownTime;
    }

    public override void setOnFire()
    {
    }
}
