using System.Collections.Generic;
using UnityEngine;

class SpawnPlateLocation : Location
{
    private List<GameObject> plates;
    public float heightOffset;

    public override bool finished()
    {
        return true;
    }

    public override float getGetHeightOffset()
    {
        return heightOffset;
    }

    public override void setOnFire()
    {
    }
}
