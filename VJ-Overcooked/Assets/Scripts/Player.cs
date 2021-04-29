using UnityEngine;

public class Player : MonoBehaviour
{
    public int playerVelocity = 1;
    private IngredientSpawner ingredientSpawner;
    private GameObject carriedIngredient, currentTable, currentWasher, carriedPlate;
    public float ingredientSpawnDistance = 5f;
    // Start is called before the first frame update

    // Update is called once per frame


     enum playerStates{
        MOVE, STAND, CHOPP, DISHES 
    };
     enum playerDirections {
        UP, DOWN, LEFT, RIGHT, TOPLEFT,TOPRIGHT,BOTTOMLEFT,BOTTOMRIGHT
    };

    private playerStates  state;
    private playerDirections direction;
    public float timer;
    public float maxSpeed;
    private bool canChopp, canPickUp, carryingObject, nextToTable, canWash;
    public float ingredientPosY = 10f;

    void Start()
    {
        state = playerStates.STAND;
        direction = playerDirections.DOWN;
        timer = 0.0f;
        canChopp = false;
        canPickUp = false;
        carryingObject = false;
        canWash = false;
        nextToTable = false;
        carriedIngredient = null;
        carriedPlate = null;
        ingredientSpawner = null;
    }

    void FixedUpdate()
    {
        bool leftB, rightB, upB, downB, spaceB;
        leftB = Input.GetKey("a");
        rightB = Input.GetKey("d");
        upB = Input.GetKey("w");
        downB = Input.GetKey("s");
        spaceB = Input.GetKey(KeyCode.Space);
        Rigidbody rb = GetComponent<Rigidbody>();

        Vector3 movement = new Vector3(0f, rb.velocity.y, 0f);

        switch (state) {
            case playerStates.STAND:


                if (upB) {
                    direction = playerDirections.UP;
                    state = playerStates.MOVE;

                }

                if (downB) {
                    direction = playerDirections.DOWN;
                    state = playerStates.MOVE;
                }

                if (leftB)
                {
                    direction = playerDirections.LEFT;
                    state = playerStates.MOVE;
                }

                if (rightB)
                {
                    direction = playerDirections.RIGHT;
                    state = playerStates.MOVE;
                }

                
                if (canChopp && spaceB  && currentTable.GetComponent<TableScript>().canBeUsed())
                {
                    TableScript tableScript = currentTable.GetComponent<TableScript>();
                    if (tableScript.isFree() && carryingObject)
                    {
                        tableScript.setIngredient(carriedIngredient);
                        carriedIngredient = null;
                        carryingObject = false;
                    }
                    else if (!tableScript.isFree() && !carryingObject && tableScript.ingredientCanBePickedUp())
                    {
                        carriedIngredient = tableScript.pickIngredient();
                        carryingObject = true;
                    }

                }
                if ( canChopp && Input.GetKey(KeyCode.LeftControl) && !currentTable.GetComponent<TableScript>().isFree()) {
                    GameObject ingredientOnTable =  currentTable.GetComponent<TableScript>().getIngredient();
                    if (!ingredientOnTable.GetComponent<Ingredient>().choppingDone()) {
                        float timeLeft = ingredientOnTable.GetComponent<Ingredient>().setReadyToCut();
                        currentTable.GetComponent<ProcessBar>().setMaxTime(timeLeft);
                        state = playerStates.CHOPP;
                    }
                }
                else if (canPickUp && spaceB && !carryingObject)
                {
                    carriedIngredient = ingredientSpawner.createIngredient();
                    initIngridientPosition();
                    carriedIngredient.GetComponent<Rigidbody>().useGravity = false;
                    carriedIngredient.GetComponent<Rigidbody>().detectCollisions = false;
                    carriedIngredient.GetComponent<Rigidbody>().isKinematic = true;
                    carryingObject = true;
                }
                else if (nextToTable && spaceB && currentTable.GetComponent<TableScript>().canBeUsed()) {
                    TableScript tableScript = currentTable.GetComponent<TableScript>();
                    if (tableScript.isFree() && carryingObject)
                    {
                        if (carriedIngredient != null) {
                            tableScript.setIngredient(carriedIngredient);
                            carriedIngredient = null;
                        }
                        else if (carriedPlate != null )
                        {
                            tableScript.setPlate(carriedPlate);
                            carriedPlate = null;
                        }


                        carryingObject = false;
                    }
                    else if (!tableScript.isFree() && !carryingObject)
                    {
                        if (!tableScript.plateOnTable()) carriedIngredient = tableScript.pickIngredient();
                        else carriedPlate = tableScript.pickPlate();

                        //como diferencio entre plato y ingridiente
                        carryingObject = true;
                    }
                }
                else if ( canWash && spaceB && currentWasher.GetComponent<SinkScript>().canBeUsed() )
                {
                    SinkScript sinkScript = currentWasher.GetComponent<SinkScript>();
                    if (sinkScript.isFree() && carryingObject)
                    {
                        //hacer cambios hay que adaptarlo al plato 
                        sinkScript.setPlate(carriedPlate);
                        carriedPlate = null;
                        carryingObject = false;
                    }
                    else if ( !sinkScript.isFree() && !carryingObject)
                    {
                        GameObject plateOnTable = currentWasher.GetComponent<SinkScript>().getPlate();
                        if (plateOnTable.GetComponent<Plate>().doneWashing())
                        {
                            carriedPlate = sinkScript.pickPlate();
                            carryingObject = true;
                        }

                    }
                    // no esta entrando 

                }
                if (canWash && Input.GetKey(KeyCode.LeftControl) && !currentWasher.GetComponent<SinkScript>().isFree())
                {
                    GameObject plateOnTable = currentWasher.GetComponent<SinkScript>().getPlate();
                    if (!plateOnTable.GetComponent<Plate>().doneWashing())
                    {
                        float timeLeft = plateOnTable.GetComponent<Plate>().setReadyToWash();
                        currentWasher.GetComponent<ProcessBar>().setMaxTime(timeLeft);
                        state = playerStates.DISHES;
                    }

                }


                break;
            case playerStates.MOVE:

                if (leftB) {
                    movement = movement + new Vector3(-playerVelocity, 0f, 0f);
                    direction = playerDirections.LEFT;
                }
                else if (rightB) {
                    movement = movement + new Vector3(playerVelocity, 0f, 0f);
                    direction = playerDirections.RIGHT;
                }
                else if (upB)
                {
                    movement = movement + new Vector3(0f, 0f, playerVelocity);
                    direction = playerDirections.UP;
                }
                else if (downB) {

                    movement = movement + new Vector3(0f, 0f, -playerVelocity);
                    direction = playerDirections.DOWN;

                }

                if (rightB && upB)
                {
                    movement = movement + new Vector3(playerVelocity / 2, 0f, playerVelocity / 2);
                    direction = playerDirections.TOPRIGHT;
                }
                else if (leftB && upB)
                {
                    movement = movement + new Vector3(-playerVelocity / 2, 0f, playerVelocity / 2);
                    direction = playerDirections.TOPLEFT;
                }
                else if (rightB && downB)
                {

                    direction = playerDirections.BOTTOMRIGHT;
                    movement = movement + new Vector3(playerVelocity / 2, 0f, -playerVelocity / 2);
                }
                else if (leftB && downB) {
                    direction = playerDirections.BOTTOMLEFT;
                    movement = movement + new Vector3(-playerVelocity / 2, 0f, -playerVelocity / 2);
                } 

                if (!upB && !downB && !leftB && !rightB)
                {
                    gameObject.GetComponent<Rigidbody>().velocity = new Vector3(0f, 0f, 0f);
                    if (carryingObject) {
                        if ( carriedIngredient != null )carriedIngredient.GetComponent<Rigidbody>().velocity = new Vector3(0f, 0f, 0f);
                        else if ( carriedPlate != null ) carriedPlate.GetComponent<Rigidbody>().velocity = new Vector3(0f, 0f, 0f);
                    }
                    
                    //gameObject.GetComponent<Rigidbody>().angularVelocity = new Vector3(0f, 0f, 0f);
                    state = playerStates.STAND;
                }
                else
                {
                    //gameObject.GetComponent<Rigidbody>().AddForce(movement);
                    if (carryingObject) initIngridientPosition(); //updateCarryingObjectPosition(movement);
                    gameObject.GetComponent<Rigidbody>().velocity = movement * Time.deltaTime;//Vector3.ClampMagnitude(gameObject.GetComponent<Rigidbody>().velocity, maxSpeed);
                }

                break;
            case playerStates.CHOPP:
                GameObject tableIngredient = currentTable.GetComponent<TableScript>().getIngredient();
                currentTable.GetComponent<ProcessBar>().setProcessTime(tableIngredient.GetComponent<Ingredient>().getTimeLeftNormalized());
                if (upB || downB || leftB || rightB)
                {
                    state = playerStates.STAND;
                    tableIngredient.GetComponent<Ingredient>().stopCutting();
                }
                else if (tableIngredient.GetComponent<Ingredient>().choppingDone())
                {
                    state = playerStates.STAND;
                    currentTable.GetComponent<ProcessBar>().hide();
                }
                break;
            case playerStates.DISHES:
                GameObject plate = currentWasher.GetComponent<SinkScript>().getPlate();
                currentWasher.GetComponent<ProcessBar>().setProcessTime(plate.GetComponent<Plate>().getTimeLeftNormalized());
                if (upB || downB || leftB || rightB)
                {
                    state = playerStates.STAND;
                    plate.GetComponent<Plate>().stopWashing();

                }
                else if (plate.GetComponent<Plate>().doneWashing())
                {
                    state = playerStates.STAND;
                    currentWasher.GetComponent<ProcessBar>().hide();
                }
                break;

        }
    }


    void OnTriggerEnter(Collider collider)
    {
        if (collider.name == "Chopper") {
            canChopp = true;
            currentTable = collider.gameObject;
        } 
        else if (collider.name == "IngredientSpawner")
        {
            ingredientSpawner = collider.gameObject.GetComponent<IngredientSpawner>();
            canPickUp = true;
        }
        else if (collider.name == "Table") {
            currentTable = collider.gameObject;
            nextToTable = true;
        }
        else if ( collider.name == "Sink")
        {
            canWash = true;
            currentWasher = collider.gameObject;
            
        }

    }

    void OnTriggerExit(Collider collider) {
        if (collider.name == "Chopper") canChopp = false;
        else if (collider.name == "IngredientSpawner")
        {
            ingredientSpawner = null;
            canPickUp = false;
        }
        else if (collider.name == "Table")
        {
            currentTable = null;
            nextToTable = false;
        }
        else if (collider.name == "Sink")
        {
            currentWasher = null;
            canWash = false;
        }
    }

    public bool playerCanPickUP() {
        return canPickUp;
    }

    public void setCarryingObject(bool value) {
        carryingObject = value;
    }

    public bool isCarryingObject() {
        return carryingObject;
    }



    //Aprovecho esta llamada para usarla tambien para los platos. Tenemos que cambiarle de nombre.
    private void initIngridientPosition()
    {

        Vector3 playerCenter = GetComponent<Renderer>().bounds.center;
       // carriedIngredient
       switch (direction)
        {
            case playerDirections.UP:
                if (carriedIngredient != null ) carriedIngredient.transform.position = new Vector3(playerCenter.x, ingredientPosY, ingredientSpawnDistance + playerCenter.z);
                else if ( carriedPlate != null) carriedPlate.transform.position = new Vector3(playerCenter.x, ingredientPosY, ingredientSpawnDistance + playerCenter.z);
                break;
            case playerDirections.DOWN:
                if (carriedIngredient != null)  carriedIngredient.transform.position = new Vector3(playerCenter.x, ingredientPosY,   playerCenter.z- ingredientSpawnDistance);
                else if (carriedPlate != null)  carriedPlate.transform.position = new Vector3(playerCenter.x, ingredientPosY, playerCenter.z - ingredientSpawnDistance);
                break;
            case playerDirections.LEFT:
                if (carriedIngredient != null)  carriedIngredient.transform.position = new Vector3(playerCenter.x- ingredientSpawnDistance, ingredientPosY, playerCenter.z);
                else if (carriedPlate != null)  carriedPlate.transform.position = new Vector3(playerCenter.x - ingredientSpawnDistance, ingredientPosY, playerCenter.z);
                break;
            case playerDirections.RIGHT:
                if (carriedIngredient != null) carriedIngredient.transform.position = new Vector3(ingredientSpawnDistance + playerCenter.x, ingredientPosY, playerCenter.z);
                else if (carriedPlate != null) carriedPlate.transform.position = new Vector3(ingredientSpawnDistance + playerCenter.x, ingredientPosY, playerCenter.z);
                break;
            case playerDirections.BOTTOMRIGHT:
                if (carriedIngredient != null) carriedIngredient.transform.position = new Vector3(ingredientSpawnDistance + playerCenter.x, ingredientPosY,   playerCenter.z - ingredientSpawnDistance);
                else if (carriedPlate != null) carriedPlate.transform.position = new Vector3(ingredientSpawnDistance + playerCenter.x, ingredientPosY, playerCenter.z - ingredientSpawnDistance);
                break;
            case playerDirections.BOTTOMLEFT:
                if (carriedIngredient != null) carriedIngredient.transform.position = new Vector3(playerCenter.x - ingredientSpawnDistance, ingredientPosY, playerCenter.z - ingredientSpawnDistance);
                else if (carriedPlate != null) carriedPlate.transform.position = new Vector3(playerCenter.x - ingredientSpawnDistance, ingredientPosY, playerCenter.z - ingredientSpawnDistance);
                break;
            case playerDirections.TOPLEFT:
                if (carriedIngredient != null)  carriedIngredient.transform.position = new Vector3(playerCenter.x - ingredientSpawnDistance, ingredientPosY, playerCenter.z + ingredientSpawnDistance);
                else if (carriedPlate != null)  carriedPlate.transform.position = new Vector3(playerCenter.x - ingredientSpawnDistance, ingredientPosY, playerCenter.z + ingredientSpawnDistance);
                break;
            case playerDirections.TOPRIGHT:
                if (carriedIngredient != null) carriedIngredient.transform.position = new Vector3(playerCenter.x + ingredientSpawnDistance, ingredientPosY, playerCenter.z + ingredientSpawnDistance);
                else if (carriedPlate != null) carriedPlate.transform.position = new Vector3(playerCenter.x + ingredientSpawnDistance, ingredientPosY, playerCenter.z + ingredientSpawnDistance);
                break;
        }
    }

}
