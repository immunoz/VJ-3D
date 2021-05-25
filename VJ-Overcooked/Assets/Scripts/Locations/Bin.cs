using System.Collections;
using System.Collections.Generic;
using UnityEngine;

 class Bin : Location
{
    // Start is called before the first frame update


    // Update is called once per frame
    void Update()
    {
        
    }


    public override float getGetHeightOffset()
    {
        return 0;
    }

    public override bool finished()
    {
        return true;
    }

    public override void setOnFire()
    {
        onFire = false;
    }


    public GameObject throwObject(GameObject ojbct)
    {
        if (ojbct.name == "plate")
        {
            ojbct.GetComponent<Plate>().SetDirty();
            return ojbct;
        }
        if (ojbct.name == "pot")
        {
            ojbct.GetComponent<Pot>().throwInBin();
            return ojbct;
        }
        return null;
    }
}
