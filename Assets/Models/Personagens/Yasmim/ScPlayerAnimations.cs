using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScPlayerAnimations : MonoBehaviour
{

    private Animator anim;
    private ScPlayerInputs inputs;

    public enum states { Idle, Walk, Run, Slow_Walking, Crouch, Crouch_Walk, Crawl, Crawl_Walk };
    public states PlayerAnimationState = states.Idle;

    float moveX, moveZ;
    void Start()
    {
        anim = GetComponent<Animator>();
        inputs = GetComponent<ScPlayerInputs>();
    }


    void Update()
    {
        AnimationsController();

        if (!inputs.runInput && !inputs.walkInput && !inputs.slow_WalkingInput) PlayerAnimationState = states.Idle;

        if (!inputs.runInput && inputs.walkInput && !inputs.slow_WalkingInput) PlayerAnimationState = states.Walk;

        if (inputs.walkInput && inputs.runInput && !inputs.slow_WalkingInput) PlayerAnimationState = states.Run;

        if (inputs.walkInput && !inputs.runInput && inputs.slow_WalkingInput) PlayerAnimationState = states.Slow_Walking;

        if (!inputs.walkInput && !inputs.runInput && inputs.crouchInput && !inputs.slow_WalkingInput) PlayerAnimationState = states.Crouch;

        if (inputs.walkInput && !inputs.runInput && inputs.crouchInput && !inputs.slow_WalkingInput) PlayerAnimationState = states.Crouch_Walk;

        if (!inputs.walkInput && !inputs.runInput && !inputs.crouchInput && !inputs.slow_WalkingInput && inputs.crawlInput) PlayerAnimationState = states.Crawl;

        if (inputs.walkInput && !inputs.runInput && !inputs.crouchInput && !inputs.slow_WalkingInput && inputs.crawlInput) PlayerAnimationState = states.Crawl;

        // if (inputs.slow_WalkingInput && inputs.runInput) PlayerAnimationState = states.Run;
    }

    void AnimationsController()
    {
        anim.SetBool("Walk", PlayerAnimationState == states.Walk ? true : false);
        anim.SetBool("Run", PlayerAnimationState == states.Run ? true : false);
        anim.SetBool("SlowWalking", PlayerAnimationState == states.Slow_Walking ? true : false);
        anim.SetBool("Crouch", PlayerAnimationState == states.Crouch ? true : false);
        anim.SetBool("Crawl", PlayerAnimationState == states.Crawl ? true : false);

        if (PlayerAnimationState == states.Crouch_Walk)
        {
            anim.SetBool("Crouch", true);
            anim.SetBool("Walk", true);
        }

        if (PlayerAnimationState == states.Crawl_Walk)
        {
            anim.SetBool("Crawl", true);
            anim.SetBool("Walk", true);
        }
    }

}
