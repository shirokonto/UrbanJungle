using System.Collections;
using System.Collections.Generic;
using StarterAssets;
using UnityEngine;
using Utils.StateMachine_Namespace;

public abstract class AnimatorState_SO : ScriptableObject, IStateAnimator
{
    [field: SerializeField] protected List<AnimatorState_SO> validStateShifts;
    
    protected Transform transform;
    protected ThirdPersonManager manager;
    
    protected bool hasAnimator => manager.animator != null;
    protected Animator animator => manager.animator;
    protected CharacterController controller => manager.controller;
    protected StarterAssetsInputs input => manager.input;
    protected GameObject mainCamera => manager.mainCamera;
    
    public bool IsValidStateShift(AnimatorState_SO requestedStateAnimator)
    {
        return validStateShifts.Contains(requestedStateAnimator);
    }

    public virtual void Enter(GameObject gameObject)
    {
        manager = gameObject.GetComponent<ThirdPersonManager>();
        transform = gameObject.transform;
    }

    public virtual void Execute() { }
    
    public virtual void OnAnimatorMove() { }

    public virtual void Exit() { }
}
