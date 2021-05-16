using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class extinguisher : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject smoke;
    private bool shoot;
    void Start()
    {
        shoot = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (shoot)
        {
            castSmoke();
        }
    }

    public GameObject getObject()
    {
        return smoke;
    }


    public void startShooting()
    {
        shoot = true;
        //Debug.Log("hola");
        smoke.SetActive(true);
    }

    public void castSmoke()
    {
        RaycastHit hit;
        if ( Physics.Raycast(smoke.transform.position, smoke.transform.forward,out hit,20f))
        {
            GameObject target = hit.collider.gameObject;
            Location targetScript = target.GetComponent<Location>();
            if ( targetScript.burnning()) targetScript.turnOffFire();
        }

    }

    public bool isShooting()
    {
        return shoot;
    }

    public void stopShooting()
    {
        shoot = false;
        smoke.SetActive(false);
    }
}
