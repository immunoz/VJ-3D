using UnityEngine;

public class Player : MonoBehaviour
{
    public int playerVelocity = 1;
    // Start is called before the first frame update

    // Update is called once per frame


     enum playerStates{
        MOVE, STAND, CHOPP, DISHES 
    };
     enum playerDirections {
        UP, DOWN, LEFT, RIGHT 
    };

    private playerStates  state;
    private playerDirections direction;
    public float timer;
    public float maxSpeed;
    private bool canChopp;


    void Start() {
        state = playerStates.STAND;
        direction = playerDirections.DOWN;
        timer = 0.0f;
        canChopp = false;
    }

    void FixedUpdate()
    {
        Vector3 movement = new Vector3(0f, 0f, 0f);
        switch (state) {
            case playerStates.STAND:
                if (Input.GetKey("w")) {
                    direction = playerDirections.UP;
                    state = playerStates.MOVE;

                }

                if (Input.GetKey("s")) {
                    direction = playerDirections.DOWN;
                    state = playerStates.MOVE;
                }

                if (Input.GetKey("a"))
                {
                    direction = playerDirections.LEFT;
                    state = playerStates.MOVE;
                }

                if (Input.GetKey("d"))
                {
                    direction = playerDirections.RIGHT;
                    state = playerStates.MOVE;
                }

                if (canChopp && Input.GetKey(KeyCode.Space)) {

                    state = playerStates.CHOPP;
                    timer = 2.0f;
                } 


                break;
            case playerStates.MOVE:

                if (direction == playerDirections.LEFT) movement = movement + new Vector3(0f, 0f, playerVelocity * Time.deltaTime);
                if (direction == playerDirections.RIGHT) movement = movement + new Vector3(0f, 0f, -playerVelocity * Time.deltaTime);
                if (direction == playerDirections.UP) movement = movement + new Vector3(playerVelocity * Time.deltaTime, 0f, 0f);
                if (direction == playerDirections.DOWN) movement = movement + new Vector3(-playerVelocity * Time.deltaTime, 0f, 0f);

                if (Input.GetKey("w") && direction != playerDirections.UP  ) {
                    direction = playerDirections.UP;
                    gameObject.GetComponent<Rigidbody>().velocity = new Vector3(0f, 0f, 0f);
                }
                if (Input.GetKey("s") && direction != playerDirections.DOWN)
                {
                    direction = playerDirections.DOWN;
                    gameObject.GetComponent<Rigidbody>().velocity = new Vector3(0f, 0f, 0f);
                }
                if (Input.GetKey("a") && direction != playerDirections.LEFT)
                {
                    direction = playerDirections.LEFT;
                    gameObject.GetComponent<Rigidbody>().velocity = new Vector3(0f, 0f, 0f);
                }
                if (Input.GetKey("d") && direction != playerDirections.RIGHT)
                {
                    direction = playerDirections.RIGHT;
                    gameObject.GetComponent<Rigidbody>().velocity = new Vector3(0f, 0f, 0f);
                } 

                if (!Input.GetKey("w") && !Input.GetKey("s") && !Input.GetKey("a") && !Input.GetKey("d"))
                {
                    gameObject.GetComponent<Rigidbody>().velocity = new Vector3(0f, 0f, 0f);
                    state = playerStates.STAND;
                }

                else
                {
                    gameObject.GetComponent<Rigidbody>().AddForce(movement);
                    gameObject.GetComponent<Rigidbody>().velocity = Vector3.ClampMagnitude(gameObject.GetComponent<Rigidbody>().velocity, maxSpeed);
                }


                break;
            case playerStates.CHOPP:

                timer -= Time.deltaTime;
                if ( timer  <= 0 ) state = playerStates.STAND;


                break;
            case playerStates.DISHES:

                break;

        }
       // 

           //Vector3 movement = new Vector3(0f, 0f, 0f);
           /*if (Input.GetKey("w"))
               movement =  movement + new Vector3(playerVelocity * Time.deltaTime, 0f, 0f);
           if (Input.GetKey("s"))
               movement = movement + new Vector3(-playerVelocity * Time.deltaTime, 0f, 0f);
           if (Input.GetKey("a"))
               movement = movement + new Vector3(0f, 0f, playerVelocity * Time.deltaTime);
           if (Input.GetKey("d"))
               movement = movement + new Vector3(0f, 0f, -playerVelocity * Time.deltaTime);
           //gameObject.GetComponent<Rigidbody>().AddForce(movement);
           transform.position = transform.position + movement;*/
    }


    void OnTriggerEnter(Collider collider)
    {
        if (collider.name == "cooker") canChopp = true;

    }
    void OnTriggerExit(Collider collider) {

        if (collider.name == "cooker") canChopp = false;
    }
}
