using UnityEngine;

 class TableScript : Location
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

    public bool setInPlate(GameObject carriedObject)
    {
        bool result = currentObject.GetComponent<Plate>().putIngredient(carriedObject);
        if (result) timer = cooldownTime;
        return result;
    }

    public override void setOnFire()
    {
        fire.Play();
        onFire = true;
        flame.SetActive(true);
    }
}
