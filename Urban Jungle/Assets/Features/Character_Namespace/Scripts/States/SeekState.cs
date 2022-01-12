using System;
using UnityEngine;

namespace Features.Character_Namespace.Scripts.States
{
    [CreateAssetMenu]
    public class SeekState : AnimatorState_SO
    {
    [Header("Movement")]
        [Tooltip("How fast the character turns to face movement direction")]
        [Range(0.0f, 10.0f)]
        [SerializeField] private float rotationChangeRate = 5.0f;
        [Tooltip("Acceleration and deceleration")]
        [SerializeField] private float speedChangeRate = 10.0f;
    
        private Collider _ladderCollider;
        private Vector3 _targetPoint;
    
        private enum LadderStates { Seek = 0, RotateTowards = 1, Climb = 2 }
        private LadderStates _ladderStates;

        public override void Enter(GameObject gameObject)
        {
            base.Enter(gameObject);

            _ladderStates = LadderStates.Seek;

            var currentColliderTransform = _manager.CurrentTrigger.transform;
            _ladderCollider = currentColliderTransform.parent.GetComponent<Collider>();
        
            _targetPoint = currentColliderTransform.position;
        }

        public override void Execute()
        {
            switch (_ladderStates)
            {
                case LadderStates.Seek:
                    _manager.Speed_AnimationBlend = Mathf.Lerp(_manager.Speed_AnimationBlend, AnimBlendThreshold_Walk, Time.deltaTime * speedChangeRate);
                    if (HasAnimator)
                    {
                        Animator.SetFloat(_animIDSpeed, _manager.Speed_AnimationBlend);
                    }
                    break;
            
                case LadderStates.RotateTowards:
                    _manager.Speed_AnimationBlend = Mathf.Lerp(_manager.Speed_AnimationBlend, AnimBlendThreshold_DefaultMovement, Time.deltaTime * speedChangeRate);
                    if (HasAnimator)
                    {
                        Animator.SetFloat(_animIDSpeed, _manager.Speed_AnimationBlend);
                    }
                    break;
            
                case LadderStates.Climb:
                    Vector3 forward = _manager.hipsRoot.TransformDirection(Vector3.forward);
                    var position = _manager.hipsRoot.position;
                    Vector3 forwardAbove = new Vector3(position.x, position.y + 1, position.z);

                    Debug.DrawRay(forwardAbove, forward, _manager.transparentRed);
                    if (!Physics.Raycast(forwardAbove, forward, 2f, _manager.ladderLayer))
                    {
                        _ladderCollider.enabled = false;
                        Animator.SetBool(_animIDClimbLadder, false);
                        Debug.DrawRay(forwardAbove, forward, _manager.transparentRed);
                    }
                    else
                    {
                        Debug.DrawRay(forwardAbove, forward, _manager.transparentGreen);
                    }
                    break;
            
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    
        public override void OnAnimatorMove()
        {
            switch (_ladderStates)
            {
                case LadderStates.Seek:
                    MoveTowardsTarget(_targetPoint);
                    break;
                case LadderStates.RotateTowards:
                    RotateTowardsTarget(_manager.CurrentTrigger.transform.rotation);
                    break;
                case LadderStates.Climb:
                    Controller.Move(Animator.deltaPosition);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public override void Exit()
        {
            base.Exit();
            _ladderCollider.enabled = true;
        }

        private void RotateTowardsTarget(Quaternion targetRotation)
        {
            _transform.rotation = Quaternion.Lerp(_transform.rotation, targetRotation, Time.deltaTime * rotationChangeRate);

            float offset = Quaternion.Angle(_transform.rotation, _manager.CurrentTrigger.transform.rotation);
            if (offset < 5f)
            {
                _transform.rotation = _manager.CurrentTrigger.transform.rotation;
                
                if (HasAnimator)
                {
                    Animator.SetBool(_animIDClimbLadder, true);
                }
                
                _ladderStates = LadderStates.Climb;
            }
        }
    
        void MoveTowardsTarget(Vector3 target) 
        {
            Vector3 offset = target - _transform.position;

            Quaternion rotation = Quaternion.LookRotation(new Vector3(offset.x, 0, offset.z));
            _transform.rotation = Quaternion.Lerp(_transform.rotation, rotation, Time.deltaTime * rotationChangeRate);
        
            if (offset.magnitude > .1f) 
            {
                Vector3 velocity = Animator.deltaPosition;
                velocity.y = _manager.VerticalVelocity * Time.deltaTime;
                Controller.Move(velocity);
            }
            else
            {
                Controller.Move(offset);

                _ladderStates = LadderStates.RotateTowards;
            }
        }
    }
}
