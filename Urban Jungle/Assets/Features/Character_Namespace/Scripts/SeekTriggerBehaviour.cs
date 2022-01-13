using System.Collections;
using System.Collections.Generic;
using Features.Character_Namespace.Scripts.States;
using UnityEngine;

public class SeekTriggerBehaviour : MonoBehaviour
{
    [Tooltip("The State that should be entered when reaching the destination.")] 
    public AnimatorState_SO nextState;
    public Transform seekTarget;
    [Range(1f, 3f)] public float seekSpeed = 1f;
}
