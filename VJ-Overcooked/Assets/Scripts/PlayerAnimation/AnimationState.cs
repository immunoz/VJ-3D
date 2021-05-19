using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationState : MonoBehaviour
{
    // Start is called before the first frame update
    Animator animator;
    bool carryingObject;
    enum playerStates
    {
        MOVE, STAND, CHOPP, DISHES,CARRYING
    };
    private playerStates state;
    void Start()
    {
        animator = GetComponent<Animator>();
        state = playerStates.STAND;
        carryingObject = false;

    }

    // Update is called once per frame
    void Update()
    {
        bool forward = Input.GetKey("w");
        bool backward = Input.GetKey("s");
        bool leftward = Input.GetKey("a");
        bool rightward = Input.GetKey("d");
        bool isWalking = animator.GetBool("isWalking");

        if ( !isWalking && (rightward || backward || leftward || forward) )
            animator.SetBool("isWalking", true);
        else if ( isWalking && (!rightward && !backward && !leftward && !forward))
            animator.SetBool("isWalking", false);

        animator.SetBool("carrying", carryingObject);

    }


    public void setCarryingObject(bool value)
    {
        carryingObject = value;
    }
}
