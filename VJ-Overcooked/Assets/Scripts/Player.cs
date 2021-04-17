using UnityEngine;

public class Player : MonoBehaviour
{
    public int playerVelocity = 1;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 movement = new Vector3(0f, 0f, 0f);
        if (Input.GetKey("w"))
            movement =  movement + new Vector3(playerVelocity * Time.deltaTime, 0f, 0f);
        if (Input.GetKey("s"))
            movement = movement + new Vector3(-playerVelocity * Time.deltaTime, 0f, 0f);
        if (Input.GetKey("a"))
            movement = movement + new Vector3(0f, 0f, playerVelocity * Time.deltaTime);
        if (Input.GetKey("d"))
            movement = movement + new Vector3(0f, 0f, -playerVelocity * Time.deltaTime);
        transform.position = transform.position + movement;
    }
}
