using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationState : MonoBehaviour
{
    enum playerStates
    {
        MOVE, STAND, CHOPP, DISHES,CARRYING
    };
    private bool chopping;
    private bool carryingObject;
    private bool dishes;
    private playerStates state;
    Animator animator;
    

    void Start()
    {
        animator = GetComponent<Animator>();
        state = playerStates.STAND;
        carryingObject = false;
        chopping = false;

    }

    // Update is called once per frame
    void Update()
    {
        bool forward = Input.GetKey("w");
        bool backward = Input.GetKey("s");
        bool leftward = Input.GetKey("a");
        bool rightward = Input.GetKey("d");
        bool isWalking = animator.GetBool("isWalking");
        animator.SetBool("carrying", carryingObject);
        animator.SetBool("chopping", chopping);
        animator.SetBool("dishes", dishes);

        if ( !isWalking && (rightward || backward || leftward || forward) )
            animator.SetBool("isWalking", true);
        else if ( isWalking && (!rightward && !backward && !leftward && !forward))
            animator.SetBool("isWalking", false);

    }


    public void setCarryingObject(bool value)
    {
        carryingObject = value;
    }

    public void setChopping(bool value) {
        chopping = value;
    }

    public void setDishes(bool value) {
        dishes = value;
    }
}
