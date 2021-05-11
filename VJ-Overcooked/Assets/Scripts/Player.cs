using UnityEngine;

public class Player : MonoBehaviour
{
    public int playerVelocity = 1;
    public float ingredientSpawnDistance = 5f;
    public GameObject currentLocation, carriedObject;
    // Start is called before the first frame update

    private bool canUse;
    private GameObject carriedIngredient, currentTable, currentWasher, carriedPlate;
    private bool play;

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
        play = false;


        ////////////////////
        canUse = false;
        currentLocation = null;
    }

    void FixedUpdate()
    {
        if (!play) return;
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

                if (spaceB && canUse && currentLocation.GetComponent<Location>().canBeUsed())
                {
                    Location locationScript = currentLocation.GetComponent<Location>();
                    Ingredient ingredientScript = null;
                    if (carriedObject != null) ingredientScript = carriedObject.GetComponent<Ingredient>();
                    if (locationScript.getType() != "IngredientSpawner" && locationScript.getType() != "cooker" && locationScript.getType() != "oven" && locationScript.getType() != "Sink" && locationScript.getType() != "DeliveryZone")
                    {
                        if (locationScript.isFree() && carryingObject)
                        {
                            locationScript.setObject(carriedObject);
                            carriedObject = null;
                            carryingObject = false;


                        }
                        else if (!locationScript.isFree() && !carryingObject /*&& locationScript.objectCanBePickedUp()*/)
                        {
                            if (locationScript.finished())
                            {
                                carriedObject = locationScript.pickObject();
                                carryingObject = true;
                            }

                        }
                        else if (!locationScript.isFree() && locationScript.hasPlate() && carryingObject && ingredientScript != null && ingredientScript.putInPlate()) {
                            TableScript tableScript = currentLocation.GetComponent<TableScript>();
                            tableScript.setInPlate(carriedObject);
                            carriedObject = null;
                            carryingObject = false;
                        }
                    }
                    else if (locationScript.getType() == "IngredientSpawner" && !carryingObject)
                    {
                        Spawner ingredientSpawner = currentLocation.GetComponent<Spawner>();
                        carriedObject = ingredientSpawner.createIngredient();
                        initObjectPosition();
                        carryingObject = true;
                    }
                    else if (locationScript.getType() == "cooker")
                    {

                        Cooker2 cooker = locationScript.GetComponent<Cooker2>();
                        if (carriedObject != null && carriedObject.name == "pot" && locationScript.isFree() && carryingObject)
                        {
                            locationScript.setObject(carriedObject);
                            Pot potScript = carriedObject.GetComponent<Pot>();
                            if (potScript.hasElement()) potScript.continueCooking();
                            carryingObject = false;
                            carriedObject = null;

                        }
                        else if (!locationScript.isFree() && !carryingObject)
                        {

                            carriedObject = locationScript.pickObject();
                            
                            carryingObject = true;
                            carriedObject.GetComponent<Pot>().stopCooking();
                           // Debug.Log(carriedObject.name);
                            currentLocation.GetComponent<Cooker2>().hideWarning();

                        }
                        else if (cooker.addIngredient(carriedObject))
                        {
                            carryingObject = false;
                            carriedObject = null;
                        }
                    }
                    else if (locationScript.getType() == "oven")
                    {

                        if (carryingObject && carriedObject.name == "pizza" && locationScript.isFree() && ! carriedObject.GetComponent<Pizza>().finished()) // EL Not finished is para ver si la pizza no esta cocinada.
                        {
                            // poner otro if para comprobar que la pizza tiene todos los ingredientes necesarios.
                            locationScript.setObject(carriedObject);
                            carryingObject = false;
                            carriedObject = null;
                            locationScript.GetComponent<Oven>().initOven();
                        }
                        else if (!locationScript.isFree() && !carryingObject && locationScript.finished() )
                        {
                            carriedObject = locationScript.pickObject();
                            carryingObject = true;
                            locationScript.GetComponent<Oven>().finishOven();

                        }


                    }
                    else if (locationScript.getType() == "sink")
                    {
                        Plate plateScript = carriedObject.GetComponent<Plate>();
                        if (locationScript.isFree() && carryingObject && carriedObject.name == "plate" && plateScript.isDirty()) setObjectInLocation(locationScript);
                        else if (!locationScript.isFree() && !carryingObject && locationScript.finished()) getObjectInLocation(locationScript);
                    }
                    else if (locationScript.getType() == "DeliveryZone" && carryingObject && carriedObject.name == "plate")
                    {
                        DeliveryZone deliveryZone = currentLocation.GetComponent<DeliveryZone>();
                        deliveryZone.setObject(carriedObject);
                        carriedObject = null;
                        carryingObject = false;
                    }
                }
                
                if (Input.GetKey(KeyCode.LeftControl) && canUse && currentLocation.GetComponent<Location>().canBeUsed())
                {
                    Location locationScript = currentLocation.GetComponent<Location>();
                    if (locationScript.getType() == "Chopper")
                    {
                        if (!locationScript.isFree() && !carryingObject )
                        {
                            Chopper chopperScript = currentLocation.GetComponent<Chopper>();
                            chopperScript.startChopp();
                            state = playerStates.CHOPP;
                        }
                    }else if ( locationScript.getType() == "Sink")
                    {
                        SinkScript sink = currentLocation.GetComponent<SinkScript>();
                        sink.startWashing();
                        state = playerStates.DISHES;
                    }
                }


                /*  if (canChopp && spaceB  && currentTable.GetComponent<TableScript>().canBeUsed())
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

                  }*/


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
                    state = playerStates.STAND;
                }
                else
                {
                    //gameObject.GetComponent<Rigidbody>().AddForce(movement);
                    if (carryingObject) initObjectPosition(); //updateCarryingObjectPosition(movement);
                    gameObject.GetComponent<Rigidbody>().velocity = movement * Time.deltaTime;//Vector3.ClampMagnitude(gameObject.GetComponent<Rigidbody>().velocity, maxSpeed);
                }
                /*if (!upB && !downB && !leftB && !rightB)
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
                }*/

                break;
            case playerStates.CHOPP:
                Chopper chopperScr = currentLocation.GetComponent<Chopper>();
                if (upB || downB || leftB || rightB || chopperScr.finished())
                {
                    if (chopperScr.finished()) chopperScr.stopChopp();
                    else chopperScr.pauseChopping();
                    state = playerStates.STAND;
                }
 

                /*GameObject tableIngredient = currentTable.GetComponent<TableScript>().getIngredient();
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
                }*/
                break;
            case playerStates.DISHES:
                SinkScript sinkScript = currentLocation.GetComponent<SinkScript>();
                if (upB || downB || leftB || rightB || sinkScript.finished())
                { 
                    state = playerStates.STAND;
                    if (sinkScript.finished()) sinkScript.stopWashing();
                    else sinkScript.pauseWashing();
                }
                break;

        }
    }

    private void setObjectInLocation(Location locationScript) {
        locationScript.setObject(carriedObject);
        carriedObject = null;
        carryingObject = false;
    }

    private void getObjectInLocation(Location locationScript)
    {
        carriedObject = locationScript.pickObject();
        carryingObject = true;
    }


    void OnTriggerEnter(Collider collider)
    {
        /*if (collider.name == "Chopper") {
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
            
        }*/
        canUse = true;
        currentLocation = collider.gameObject;

    }

    void OnTriggerExit(Collider collider) {
        /* if (collider.name == "Chopper") canChopp = false;
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
        }*/
        canUse = false;
        currentLocation = null;
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
    private void initObjectPosition()
    {

        Vector3 playerCenter = GetComponent<Renderer>().bounds.center;
       // carriedIngredient
       switch (direction)
        {

            case playerDirections.UP:
                carriedObject.transform.position = new Vector3(playerCenter.x, ingredientPosY, ingredientSpawnDistance + playerCenter.z);

                break;
            case playerDirections.DOWN:
                carriedObject.transform.position = new Vector3(playerCenter.x, ingredientPosY, playerCenter.z - ingredientSpawnDistance);

                break;
            case playerDirections.LEFT:
                carriedObject.transform.position = new Vector3(playerCenter.x - ingredientSpawnDistance, ingredientPosY, playerCenter.z);

                break;
            case playerDirections.RIGHT:
                carriedObject.transform.position = new Vector3(ingredientSpawnDistance + playerCenter.x, ingredientPosY, playerCenter.z);

                break;
            case playerDirections.BOTTOMRIGHT:
                carriedObject.transform.position = new Vector3(ingredientSpawnDistance + playerCenter.x, ingredientPosY, playerCenter.z - ingredientSpawnDistance);

                break;
            case playerDirections.BOTTOMLEFT:
                carriedObject.transform.position = new Vector3(playerCenter.x - ingredientSpawnDistance, ingredientPosY, playerCenter.z - ingredientSpawnDistance);
                break;
            case playerDirections.TOPLEFT:
                carriedObject.transform.position = new Vector3(playerCenter.x - ingredientSpawnDistance, ingredientPosY, playerCenter.z + ingredientSpawnDistance);

                break;
            case playerDirections.TOPRIGHT:
                carriedObject.transform.position = new Vector3(playerCenter.x + ingredientSpawnDistance, ingredientPosY, playerCenter.z + ingredientSpawnDistance);
 
                break;
                /*
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
                    break;*/
        }
    }

    public void setPlay(bool value) {
        play = value;
    }

}
