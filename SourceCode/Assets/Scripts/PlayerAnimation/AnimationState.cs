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
    private bool isWalking;
    private playerStates state;
    Animator animator;
    

    void Start()
    {
        animator = GetComponent<Animator>();
        state = playerStates.STAND;
        carryingObject = false;
        chopping = false;
        isWalking = false;

    }

    // Update is called once per frame
    void Update()
    {
        animator.SetBool("carrying", carryingObject);
        animator.SetBool("chopping", chopping);
        animator.SetBool("dishes", dishes);
        animator.SetBool("isWalking",isWalking);
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

    public void setMoving(bool value)
    {
        isWalking = value;
    }
}
