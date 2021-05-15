using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationState : MonoBehaviour
{
    // Start is called before the first frame update
    Animator animator;

    enum playerStates
    {
        MOVE, STAND, CHOPP, DISHES,CARRYING
    };
    private playerStates state;
    void Start()
    {
        animator = GetComponent<Animator>();
        state = playerStates.STAND;

    }

    // Update is called once per frame
    void Update()
    {
        bool forward = Input.GetKey("w");
        bool backward = Input.GetKey("s");
        bool leftward = Input.GetKey("a");
        bool rightward = Input.GetKey("d");
        bool isWalking = animator.GetBool("isWalking");

        switch (state)
        {
            case playerStates.STAND:
                animator.SetBool("isWalking", false);
                if (forward || backward || rightward || leftward)
                    state = playerStates.MOVE;
                break;
            case playerStates.MOVE:
                animator.SetBool("isWalking", true);
                if ( !forward && ! backward && ! leftward&& !rightward) state = playerStates.STAND;
                break;
            case playerStates.CARRYING:
                animator.SetBool("carrying", true);
                break;


        }
    }
}
