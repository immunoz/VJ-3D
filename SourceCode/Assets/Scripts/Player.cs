using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int playerVelocity = 1;
    public float ingredientSpawnDistance = 5f;
    public GameObject currentLocation, carriedObject;
    // Start is called before the first frame update

    private bool canUse, settedBlade;
    private GameObject carriedIngredient, currentTable, currentWasher, carriedPlate;
    private bool play;
    Vector3 initialPosition;

    //------------- GOD MODE -------------
    public GameObject dishTemplate;


    enum playerStates
    {
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
    public float ingredientPosY = 8f;

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
        settedBlade = false;


        ////////////////////
        canUse = false;
        currentLocation = null;
    }

    void FixedUpdate()
    {
        if (!play) return;

        //----------- GOD MODE -----------
        if (Input.GetKey(KeyCode.P) && carriedObject == null) SpawnPlateLocation();
        else if (carriedObject != null && carriedObject.GetComponent<Plate>() != null && Input.GetKey(KeyCode.N)) checkGodMode(); 

        //----------- GOD MODE -----------
        bool leftB, rightB, upB, downB, spaceB;
        leftB = Input.GetKey("a");
        rightB = Input.GetKey("d");
        upB = Input.GetKey("w");
        downB = Input.GetKey("s");
        spaceB = Input.GetKey(KeyCode.Space);
        Rigidbody rb = GetComponent<Rigidbody>();
        GetComponent<AnimationState>().setCarryingObject(carryingObject);
        checkIfNearLocation();

        Vector3 movement = new Vector3(0f, rb.velocity.y, 0f);

        if (carryingObject) adjustPosition();

        switch (state) {
            case playerStates.STAND:
                GetComponent<AnimationState>().setMoving(false);

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
                    if (locationScript.getType() == "Table")
                    {
                        if (locationScript.isFree() && carryingObject) setObjectInLocation(locationScript);
                        else if (!locationScript.isFree() && !carryingObject && locationScript.finished()/*&& locationScript.objectCanBePickedUp()*/) getObjectInLocation(locationScript);
                        else if (!locationScript.isFree() && locationScript.hasPlate() && carryingObject)
                        {
                            TableScript tableScript = currentLocation.GetComponent<TableScript>();
                            if (ingredientScript != null && ingredientScript.putInPlate() && !tableScript.currentObject.GetComponent<Plate>().isDirty())
                            {
                                if (tableScript.setInPlate(carriedObject))
                                {
                                    carriedObject = null;
                                    carryingObject = false;
                                }
                            }
                            else if (carriedObject.name == "pot")
                            {
                                Pot potScript = carriedObject.GetComponent<Pot>();
                                if (potScript.hasStew() && !tableScript.getObject().GetComponent<Plate>().isDirty()) potScript.getStew(tableScript.getObject());
                            }
                            else if (carriedObject.name == "pan")
                            {
                                Pan panScript = carriedObject.GetComponent<Pan>();
                                if (panScript.hasMeat() && !tableScript.getObject().GetComponent<Plate>().isDirty()) panScript.GetMeat(tableScript.getObject());
                            }
                        }
                        else if (!locationScript.isFree() && locationScript.hasPizzaMass() && carryingObject && locationScript.currentObject.GetComponent<PizzaMass>().putIngredient(carriedObject))
                        {
                            locationScript.resetCoolDown();
                            carriedObject = null;
                            carryingObject = false;
                        }
                        else if (!locationScript.isFree() && carryingObject && carriedObject.name == "plate" && !carriedObject.GetComponent<Plate>().isDirty() && locationScript.currentObject.name == "pot" && locationScript.currentObject.GetComponent<Pot>().hasStew())
                            locationScript.currentObject.GetComponent<Pot>().getStew(carriedObject);
                    }
                    else if (locationScript.getType() == "Chopper" || locationScript.getType() == "plateGenerator") {
                        if (locationScript.isFree() && carryingObject && ingredientScript != null && !ingredientScript.choppingDone() && carriedObject.name != "BurgerBread") setObjectInLocation(locationScript);
                        else if (!locationScript.isFree() && !carryingObject && locationScript.finished()) getObjectInLocation(locationScript);
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
                        if (carriedObject != null  && !cooker.burnning() && (carriedObject.name == "pot" || carriedObject.name == "pan") && locationScript.isFree() /*&& carryingObject*/) //carrying object es redundante
                        {
                            locationScript.setObject(carriedObject);
                            if (carriedObject.name == "pot")
                            {
                                Pot potScript = carriedObject.GetComponent<Pot>();
                                if (potScript.hasElement()) potScript.continueCooking();
                            }
                            else carriedObject.GetComponent<Pan>().resume();
                            setObjectInLocation(locationScript);
                        }
                        else if (!locationScript.isFree() && !carryingObject && !locationScript.burnning())
                        {
                            carriedObject = locationScript.pickObject();
                            carryingObject = true;
                            if (carriedObject.name == "pot") carriedObject.GetComponent<Pot>().stopCooking();
                            else carriedObject.GetComponent<Pan>().setPause(true);

                            // Debug.Log(carriedObject.name);
                            currentLocation.GetComponent<Cooker2>().hideWarning();

                        }
                        else if (carryingObject && !cooker.burnning() && cooker.addIngredient(carriedObject) )
                        {
                            carryingObject = false;
                            carriedObject = null;
                        }
                        else if (carryingObject && carriedObject.name == "plate" && cooker.currentObject.name == "pot" && cooker.hasStew()) cooker.GetStew(carriedObject);
                        else if (carryingObject && carriedObject.name == "plate" && cooker.currentObject.name == "pan" && cooker.hasMeat()) cooker.GetMeat(carriedObject);
                    }
                    else if (locationScript.getType() == "oven")
                    {
                        if (carryingObject && carriedObject.name != "plate" && carriedObject.GetComponent<PizzaMass>().isRawPizza() && locationScript.isFree() && !carriedObject.GetComponent<PizzaMass>().finished() && !locationScript.burnning()) // EL Not finished is para ver si la pizza no esta cocinada.
                        {
                            // poner otro if para comprobar que la pizza tiene todos los ingredientes necesarios.
                            locationScript.setObject(carriedObject);
                            carryingObject = false;
                            carriedObject = null;
                            locationScript.GetComponent<Oven>().initOven();
                        }
                        else if (!locationScript.isFree() && !carryingObject && locationScript.finished())
                        {
                            carriedObject = locationScript.pickObject();
                            carryingObject = true;
                            locationScript.GetComponent<Oven>().hideProcessBar();
                        }
                        else if (!locationScript.isFree() && locationScript.finished() && carryingObject && carriedObject.name == "plate") {
                            GameObject temp = locationScript.pickObject();
                            Plate plateScript = carriedObject.GetComponent<Plate>();
                            if (plateScript.putIngredient(temp))
                            {
                                currentLocation.GetComponent<Oven>().hideProcessBar();
                            }
                        }


                    }
                    else if (locationScript.getType() == "Sink")
                    {
                        Plate plateScript = null;
                        if (carriedObject != null) plateScript = carriedObject.GetComponent<Plate>();
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
                    else if ( locationScript.getType() == "bin" && carryingObject)
                    {
                        Bin bin = currentLocation.GetComponent<Bin>();
                        if (bin.throwObject(carriedObject))
                        {
                            carriedObject = null;
                            carryingObject = false;
                        }
                    }
                }


                if (Input.GetKey(KeyCode.LeftControl) && canUse && currentLocation.GetComponent<Location>().canBeUsed() && carriedObject == null)
                {
                    Location locationScript = currentLocation.GetComponent<Location>();
                    if (locationScript.getType() == "Chopper")
                    {
                        if (!locationScript.isFree() && !carryingObject)
                        {
                            Chopper chopperScript = currentLocation.GetComponent<Chopper>();
                            chopperScript.startChopp();
                            
                            state = playerStates.CHOPP;
                        }
                    }
                    else if (locationScript.getType() == "Sink")
                    {
                        SinkScript sink = currentLocation.GetComponent<SinkScript>();
                        sink.startWashing();
                        state = playerStates.DISHES;
                        FindObjectOfType<AudioManager>().play("DoingTheDishes");
                        GetComponent<AnimationState>().setDishes(true);
                    }
                }
                else if ( carryingObject && carriedObject.name == "extinguisher") setShooting();


                break;
            case playerStates.MOVE:
                GetComponent<AnimationState>().setMoving(true);
                Quaternion rotate = Quaternion.Euler(0, 0, 0);
       
                if (leftB) {
                    movement = movement + new Vector3(-playerVelocity, 0f, 0f);
                    direction = playerDirections.LEFT;
                    rotate = Quaternion.Euler(0, -90, 0);
                }
                else if (rightB) {
                    movement = movement + new Vector3(playerVelocity, 0f, 0f);
                    direction = playerDirections.RIGHT;
                    rotate = Quaternion.Euler(0, 90, 0);
                }
                else if (upB)
                {
                    movement = movement + new Vector3(0f, 0f, playerVelocity);
                    direction = playerDirections.UP;
                    rotate = Quaternion.Euler(0, 0, 0);
                }
                else if (downB) {

                    movement = movement + new Vector3(0f, 0f, -playerVelocity);
                    direction = playerDirections.DOWN;
                    rotate = Quaternion.Euler(0, 180, 0);

                }

                if (rightB && upB)
                {
                    movement = movement + new Vector3(playerVelocity / 2, 0f, playerVelocity / 2);
                    direction = playerDirections.TOPRIGHT;
                    rotate = Quaternion.Euler(0, 45, 0);
                }
                else if (leftB && upB)
                {
                    movement = movement + new Vector3(-playerVelocity / 2, 0f, playerVelocity / 2);
                    direction = playerDirections.TOPLEFT;
                    rotate = Quaternion.Euler(0, -45, 0);
                }
                else if (rightB && downB)
                {

                    direction = playerDirections.BOTTOMRIGHT;
                    movement = movement + new Vector3(playerVelocity / 2, 0f, -playerVelocity / 2);
                    rotate = Quaternion.Euler(0, 135, 0);
                }
                else if (leftB && downB) {
                    direction = playerDirections.BOTTOMLEFT;
                    movement = movement + new Vector3(-playerVelocity / 2, 0f, -playerVelocity / 2);
                    rotate = Quaternion.Euler(0, -135, 0);
                }
                if ( carryingObject && carriedObject.name == "extinguisher") setShooting();

                if (!upB && !downB && !leftB && !rightB)
                {
                    gameObject.GetComponent<Rigidbody>().velocity = new Vector3(0f, 0f, 0f);
                    state = playerStates.STAND;
                }
                else
                {
                    //gameObject.GetComponent<Rigidbody>().AddForce(movement);
                    if (carryingObject) initObjectPosition(); //updateCarryingObjectPosition(movement);
                    transform.rotation = rotate;
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
                GetComponent<AnimationState>().setChopping(true);
                GameObject blade = currentLocation.transform.GetChild(0).gameObject;
                if (!settedBlade)
                {
                    initialPosition = blade.transform.position;
                    settedBlade = true;
                }
                adjustPositionOfBlade(blade);
                Chopper chopperScr = currentLocation.GetComponent<Chopper>();
                if (upB || downB || leftB || rightB || chopperScr.finished())
                {
                    if (chopperScr.finished()) chopperScr.stopChopp();
                    else chopperScr.pauseChopping();
                    state = playerStates.STAND;
                    blade.transform.position = initialPosition;
                    Quaternion rotation = Quaternion.Euler(90f, 0f, 62.512f);
                    blade.transform.rotation = rotation;
                    settedBlade = false;
                    GetComponent<AnimationState>().setChopping(false);
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
                    GetComponent<AnimationState>().setDishes(false);
                    state = playerStates.STAND;
                    if (sinkScript.finished()) sinkScript.stopWashing();
                    else sinkScript.pauseWashing();
                    FindObjectOfType<AudioManager>().stop("DoingTheDishes");
                }
                break;

        }
    }

    private void checkGodMode()
    {
        Plate plateScript = carriedObject.GetComponent<Plate>();
        string plate = GameObject.Find("GameManager").GetComponent<GameManager>().getNextDelivery();
        if (plate != null) plateScript.putDish(plate);
    }

    private void SpawnPlateLocation()
    {
        carriedObject = Instantiate(dishTemplate) as GameObject;
        carryingObject = true;
        initObjectPosition();
    }

    private void adjustPositionOfBlade(GameObject blade)
    {
        GameObject hand = GameObject.Find("RightHandIndex1");
        blade.transform.position = hand.transform.position;
        blade.transform.eulerAngles = hand.transform.eulerAngles;
        Quaternion rotation = Quaternion.Euler(9.6f, 84.4f, 79.4f);
        blade.transform.rotation = rotation;

    }

    private void setObjectInLocation(Location locationScript) {
        FindObjectOfType<AudioManager>().play("PutOnTable");
        locationScript.setObject(carriedObject);
        carriedObject = null;
        carryingObject = false;
    }

    private void getObjectInLocation(Location locationScript)
    {
        carriedObject = locationScript.pickObject();
        carryingObject = true;
        if ( locationScript.getType() == "Chopper")
        {
            currentLocation.GetComponent<Chopper>().hideExplosion();
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
    private void initObjectPosition()
    {

        Vector3 playerCenter = GetComponent<Renderer>().bounds.center;
       // carriedIngredient
       switch (direction)
        {

            case playerDirections.UP:

                if (carriedObject.name == "extinguisher") {
                    Quaternion target = Quaternion.Euler(0, 90,78);
                    carriedObject.transform.rotation = target;
                }
                 carriedObject.transform.position = new Vector3(playerCenter.x, ingredientPosY, ingredientSpawnDistance + playerCenter.z);

                break;
            case playerDirections.DOWN:
                if (carriedObject.name == "extinguisher")
                {
                    Quaternion target = Quaternion.Euler(0, -90,78);
                    carriedObject.transform.rotation = target;
                }
                carriedObject.transform.position = new Vector3(playerCenter.x, ingredientPosY, playerCenter.z - ingredientSpawnDistance);

                break;
            case playerDirections.LEFT:
                if (carriedObject.name == "extinguisher")
                {
                    Quaternion target = Quaternion.Euler(0, 0, 78);
                    carriedObject.transform.rotation = target;
                }
                carriedObject.transform.position = new Vector3(playerCenter.x - ingredientSpawnDistance, ingredientPosY, playerCenter.z);

                break;
            case playerDirections.RIGHT:
                if (carriedObject.name == "extinguisher")
                {
                    Quaternion target = Quaternion.Euler(0, 180, 78);
                    carriedObject.transform.rotation = target;
                }
                carriedObject.transform.position = new Vector3(ingredientSpawnDistance + playerCenter.x, ingredientPosY, playerCenter.z);

                break;
            case playerDirections.BOTTOMRIGHT:
                if (carriedObject.name == "extinguisher") {
                    Quaternion target = Quaternion.Euler(0, -135, 78);
                    carriedObject.transform.rotation = target;
                } 

                carriedObject.transform.position = new Vector3(ingredientSpawnDistance + playerCenter.x, ingredientPosY, playerCenter.z - ingredientSpawnDistance);

                break;
            case playerDirections.BOTTOMLEFT:
                if (carriedObject.name == "extinguisher")
                {
                    Quaternion target = Quaternion.Euler(0, -45, 78);
                    carriedObject.transform.rotation = target;
                }
                carriedObject.transform.position = new Vector3(playerCenter.x - ingredientSpawnDistance, ingredientPosY, playerCenter.z - ingredientSpawnDistance);
                break;
            case playerDirections.TOPLEFT:
                if (carriedObject.name == "extinguisher")
                {
                    Quaternion target = Quaternion.Euler(0, 45, 78);
                    carriedObject.transform.rotation = target;
                }
                carriedObject.transform.position = new Vector3(playerCenter.x - ingredientSpawnDistance, ingredientPosY, playerCenter.z + ingredientSpawnDistance);

                break;
            case playerDirections.TOPRIGHT:
                if (carriedObject.name == "extinguisher")
                {
                    Quaternion target = Quaternion.Euler(0, 135, 78);
                    carriedObject.transform.rotation = target;
                }
                carriedObject.transform.position = new Vector3(playerCenter.x + ingredientSpawnDistance, ingredientPosY, playerCenter.z + ingredientSpawnDistance);

                break;
        }
    }

    public void setPlay(bool value) {
        play = value;
    }


    public void setShooting()
    {
        if (Input.GetKey(KeyCode.LeftControl))
        {
            extinguisher extinguisherScript = carriedObject.GetComponent<extinguisher>();
            if (!extinguisherScript.isShooting()) extinguisherScript.startShooting();
        }
        else if (carriedObject.GetComponent<extinguisher>().isShooting()) carriedObject.GetComponent<extinguisher>().stopShooting();
    }

    public void adjustPosition()
    {
        if ( carriedObject.name == "extinguisher")
        {
            ingredientSpawnDistance = 3.6f;
            ingredientPosY = 17f;
        }
        else
        {
            ingredientSpawnDistance = 6.1f;
            ingredientPosY = 11.7f;
        }

    }

    public void checkIfNearLocation()
    {
        RaycastHit loc;
        if (Physics.Raycast(transform.position, transform.forward, out loc, 5f))
        {
            currentLocation = loc.collider.gameObject;
            canUse = true;
        }
        else
        {
            currentLocation = null;
            canUse = false;
        }
    }
}
