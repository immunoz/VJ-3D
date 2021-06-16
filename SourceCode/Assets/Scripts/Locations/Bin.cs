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

    public bool throwObject(GameObject obj)
    {
        if (obj.name == "extinguisher") return false;
        if (obj.name == "plate")
        {
            obj.GetComponent<Plate>().SetDirty();
            return false;
        }
        if (obj.name == "pot")
        {
            obj.GetComponent<Pot>().throwInBin();
            return false;
        }
        if (obj.name == "pan")
        {
            obj.GetComponent<Pan>().initPan();
            return false;
        }
        Destroy(obj);
        return true;
    }
}
