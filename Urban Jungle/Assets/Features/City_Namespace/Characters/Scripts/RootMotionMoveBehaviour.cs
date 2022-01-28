using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RootMotionMoveBehaviour : MonoBehaviour
{
    public float gravity = -15.0f;
    
    private CharacterController _characterController;
    private Animator _animator;
    
    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
        _animator = GetComponent<Animator>();
    }

    private void OnAnimatorMove()
    {
        Vector3 velocity = _animator.deltaPosition;
        velocity.y = gravity * Time.deltaTime;
        _characterController.Move(velocity);
    }
}
