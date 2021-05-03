using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pot : MonoBehaviour
{



    public ArrayList currentObjects;

    public bool setObject(GameObject obj)
    {
        Onion onionScript = obj.GetComponent<Onion>();
        if (onionScript.inPot() && onionScript.choppingDone()) {
            currentObjects.Add(obj);
            return true;
        }
        return false;
    }


}
