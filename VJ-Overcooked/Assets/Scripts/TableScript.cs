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

    public void setInPlate(GameObject carriedObject)
    {
        currentObject.GetComponent<Plate>().putIngredient(carriedObject);
        timer = cooldownTime;
    }




    /*void Start() {
        timer = 0;
        if (currentPlate != null) setPlate(currentPlate);
    }

    void Update() {
        if (timer > 0) timer -= Time.deltaTime;
    }

    public void setIngredient(GameObject ingredient) {
        currentIngredient = ingredient;
        setIngredientPosition();
        timer = cooldownTime;
    }

    public void setPlate(GameObject plate )
    {
        currentPlate = plate;
        setPlatePosition();
        timer = cooldownTime;
    }

    private void setIngredientPosition() {
        Vector3 tableCenter = GetComponent<Renderer>().bounds.center;
        currentIngredient.transform.position = tableCenter + new Vector3(0f, heightOffset, 0f);
    }

    private void setPlatePosition()
    {
        Vector3 tableCenter = GetComponent<Renderer>().bounds.center;
        currentPlate.transform.position = tableCenter + new Vector3(0f, heightOffset, 0f);
    }

    public bool isFree() {
        return (currentIngredient == null && currentPlate == null);
    }

    public GameObject pickIngredient() {
        GameObject result = currentIngredient;
        currentIngredient = null;
        timer = cooldownTime;
        return result;
    }

    public GameObject pickPlate()
    {
        GameObject result = currentPlate;
        currentPlate = null;
        timer = cooldownTime;
        return result;
    }

    public GameObject getIngredient ()
    {
        return currentIngredient;
    }

    public GameObject getPlate()
    {
        return currentPlate;
    }

    public bool canBeUsed() {
        return timer <= 0;
    }

    public bool ingredientCanBePickedUp()
    {
        return currentIngredient.GetComponent<Ingredient>().ingredientCanBePickedUp();
    }

    public bool plateCanBePickedUp()
    {
        return currentPlate.GetComponent<Plate>().plateCanBePickedUp();
    }

    public bool plateOnTable ()
    {
        //  se puede hacer mas en general mas tarde para diferenciar entre plate e ingrediente, etc
        return currentPlate != null;
    }*/



}
