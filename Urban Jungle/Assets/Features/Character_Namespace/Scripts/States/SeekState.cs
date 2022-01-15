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

        private Transform _targetPoint;
        private AnimatorState_SO nextState;
        private MovementSpeed seekSpeed;
    
        private enum SeekStates { Move = 0, Rotate = 1}
        private SeekStates _seekStates;

        public void SetNextState(SeekTriggerBehaviour seekTriggerBehaviour)
        {
            nextState = seekTriggerBehaviour.nextState;
            _targetPoint = seekTriggerBehaviour.seekTarget;
            seekSpeed = seekTriggerBehaviour.seekSpeed;
        }

        protected override void Enter()
        {
            _seekStates = SeekStates.Move;
            
            // update animator if using character
            if (Animator != null)
            {
                Animator.SetBool(_animIDGrounded, true);
            }
        }

        public override void Execute()
        {
            _manager.Speed_AnimationBlend = _seekStates switch
            {
                SeekStates.Move => Mathf.Lerp(_manager.Speed_AnimationBlend, (float) seekSpeed,
                    Time.deltaTime * speedChangeRate),
                SeekStates.Rotate => Mathf.Lerp(_manager.Speed_AnimationBlend, (float) MovementSpeed.Stand,
                    Time.deltaTime * speedChangeRate),
                _ => throw new ArgumentOutOfRangeException()
            };

            if (HasAnimator)
            {
                Animator.SetFloat(_animIDSpeed, _manager.Speed_AnimationBlend);
            }
        }
    
        public override void OnAnimatorMove()
        {
            switch (_seekStates)
            {
                case SeekStates.Move:
                    MoveTowardsTarget(_targetPoint.position);
                    break;
                case SeekStates.Rotate:
                    RotateTowardsTarget(_targetPoint.rotation);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public override void Exit()
        {
            base.Exit();
            
            nextState = null;
            
            // update animator if using character
            if (Animator != null)
            {
                Animator.SetBool(_animIDGrounded, false);
            }
        }

        private void RotateTowardsTarget(Quaternion targetRotation)
        {
            _transform.rotation = Quaternion.Lerp(_transform.rotation, targetRotation, Time.deltaTime * rotationChangeRate);

            float offset = Quaternion.Angle(_transform.rotation, targetRotation);
            if (offset < 5f)
            {
                _transform.rotation = targetRotation;

                if (nextState != null)
                {
                    _manager.RequestState(nextState);
                }
                else
                {
                    Debug.LogError($"Use the SetNextState to pass a following state!");
                }
            }
        }
    
        private void MoveTowardsTarget(Vector3 target) 
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

                _seekStates = SeekStates.Rotate;
            }
        }
    }
}
