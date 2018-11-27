﻿using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MoveSMB : StateMachineBehaviour
{
    private float _vx, _vy;

    private readonly int _hashVx = Animator.StringToHash("Vx"), _hashVy = Animator.StringToHash("Vy");

    private AnimatorOverrideController _animatorOverrideController;

    public AnimationClip[] idleClips;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _animatorOverrideController = new AnimatorOverrideController(animator.runtimeAnimatorController);
        animator.runtimeAnimatorController = _animatorOverrideController;
    }


    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _vx = Input.GetAxis("Horizontal");
        _vy = Input.GetAxis("Vertical");
        Vector2 v = new Vector2(_vx, _vy).normalized;
        animator.SetFloat(_hashVx, v.x, 0, Time.deltaTime);
        animator.SetFloat(_hashVy, v.y, 0, Time.deltaTime);


        if (v.y >= 1)
        {
            _animatorOverrideController["Idle"] = idleClips[0];
        }
        else if (v.y <= -1)
        {
            _animatorOverrideController["Idle"] = idleClips[1];
        }

        if (v.x >= 1)
        {
            _animatorOverrideController["Idle"] = idleClips[2];
        }
        else if (v.x <= -1)
        {
            _animatorOverrideController["Idle"] = idleClips[3];
        }
    }


    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    //
    //}

    // OnStateMove is called right after Animator.OnAnimatorMove(). Code that processes and affects root motion should be implemented here
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    //
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK(). Code that sets up animation IK (inverse kinematics) should be implemented here.
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    //
    //}
}