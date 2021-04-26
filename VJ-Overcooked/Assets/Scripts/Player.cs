using UnityEngine;

public class Player : MonoBehaviour
{
    public int playerVelocity = 1;
    private IngredientSpawner ingredientSpawner;
    private GameObject carriedIngredient, currentTable;
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
    private bool canChopp, canPickUp, carryingObject, nextToTable;
    public float ingredientPosY = 10f;

    void Start()
    {
        state = playerStates.STAND;
        direction = playerDirections.DOWN;
        timer = 0.0f;
        canChopp = false;
        canPickUp = false;
        carryingObject = false;
        nextToTable = false;
        carriedIngredient = null;
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
                    else if ( !tableScript.isFree() && ! carryingObject)
                    {
                        carriedIngredient = tableScript.pickIngredient();
                        carryingObject = true;
                    }

                }
                if ( canChopp && Input.GetKey(KeyCode.LeftControl) && !currentTable.GetComponent<TableScript>().isFree() ) {

                    GameObject ingredientOnTable =  currentTable.GetComponent<TableScript>().getIngredient();
                    ingredientOnTable.GetComponent<Ingredient>().setReadyToCut();
                    state = playerStates.CHOPP;

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
                        tableScript.setIngredient(carriedIngredient);
                        carriedIngredient = null;
                        carryingObject = false;
                    }
                    else if (!tableScript.isFree() && !carryingObject)
                    {
                        carriedIngredient = tableScript.pickIngredient();
                        carryingObject = true;
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
                    if (carryingObject) carriedIngredient.GetComponent<Rigidbody>().velocity = new Vector3(0f, 0f, 0f);
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


                //if (doneChopping) state = playerStates.STAND;
                GameObject tableIngredient = currentTable.GetComponent<TableScript>().getIngredient();
                if (upB || downB || leftB || rightB) {
                    state = playerStates.STAND;
                    tableIngredient.GetComponent<Ingredient>().stopCutting();
                } 
                else if (tableIngredient.GetComponent<Ingredient>().choppingDone() ) state = playerStates.STAND;
                break;
            case playerStates.DISHES:

                

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


    private void initIngridientPosition()
    {

        Vector3 playerCenter = GetComponent<Renderer>().bounds.center;
       // carriedIngredient
       switch (direction)
        {
            case playerDirections.UP:
                carriedIngredient.transform.position = new Vector3(playerCenter.x, ingredientPosY, ingredientSpawnDistance + playerCenter.z);
                break;
            case playerDirections.DOWN:
                carriedIngredient.transform.position = new Vector3(playerCenter.x, ingredientPosY,   playerCenter.z- ingredientSpawnDistance);
                break;
            case playerDirections.LEFT:
                carriedIngredient.transform.position = new Vector3(playerCenter.x- ingredientSpawnDistance, ingredientPosY, playerCenter.z);
                break;
            case playerDirections.RIGHT:
                carriedIngredient.transform.position = new Vector3(ingredientSpawnDistance + playerCenter.x, ingredientPosY, playerCenter.z);
                break;
            case playerDirections.BOTTOMRIGHT:
                carriedIngredient.transform.position = new Vector3(ingredientSpawnDistance + playerCenter.x, ingredientPosY,   playerCenter.z - ingredientSpawnDistance);
                break;
            case playerDirections.BOTTOMLEFT:
                carriedIngredient.transform.position = new Vector3(playerCenter.x - ingredientSpawnDistance, ingredientPosY, playerCenter.z - ingredientSpawnDistance);
                break;
            case playerDirections.TOPLEFT:
                carriedIngredient.transform.position = new Vector3(playerCenter.x - ingredientSpawnDistance, ingredientPosY, playerCenter.z + ingredientSpawnDistance);
                break;
            case playerDirections.TOPRIGHT:
                carriedIngredient.transform.position = new Vector3(playerCenter.x + ingredientSpawnDistance, ingredientPosY, playerCenter.z + ingredientSpawnDistance);
                break;
        }
    }

}
