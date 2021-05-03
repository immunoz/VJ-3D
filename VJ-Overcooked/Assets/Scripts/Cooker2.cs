using System.Collections;
using System.Collections.Generic;
using UnityEngine;

 class Cooker2 : Location
{
    public float heightOffset;



    public override float getGetHeightOffset()
    {
        return heightOffset;
    }

    public override bool finished()
    {
        return true;
    }

    public bool addIngredient(GameObject obj)
    {
        //asumimos que solo pueden haber ollas y sartenes sobre la cocina
        if ( currentObject != null && obj.name != "plate")
        {
            Pot pot = currentObject.GetComponent<Pot>();
            return pot.setObject(obj);
        }
        return false;
    }




}
