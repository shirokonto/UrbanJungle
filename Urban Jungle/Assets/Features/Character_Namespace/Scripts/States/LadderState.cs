using System;
using UnityEngine;

namespace Features.Character_Namespace.Scripts.States
{
    [CreateAssetMenu]
    public class LadderState : AnimatorState_SO
    {
        [SerializeField] private LayerMask floorLayer;
        [SerializeField] private float climbTimeout = 0.3f;
        [Header("Raycast")]
        [SerializeField] private bool drawDebugRay;
        [SerializeField] private float raycastPositionY = 1f;
        [SerializeField] private float raycastDistance = 0.6f;
        
        private Collider _ladderCollider;
        private bool _isControllableClimb;
        private float _climbTimeoutDelta;

        public override void Enter(GameObject gameObject)
        {
            base.Enter(gameObject);

            //reset
            _climbTimeoutDelta = climbTimeout;
            _isControllableClimb = true;

            //cast a raycast for the ladder collider
            _ladderCollider = GetRaycastHit().collider;

            if (HasAnimator)
            {
                Animator.SetBool(_animIDClimbLadder, true);
            }
        }

        public override void Execute()
        {
            if (_isControllableClimb && _climbTimeoutDelta < 0)
            {
                Move();

                if (_manager.IsGroundedToLayer(floorLayer))
                {
                    _manager.EnterGroundedState();
                }
            }
            _climbTimeoutDelta -= Time.deltaTime;

            CheckLadderTopEnd();
        }

        public override void OnAnimatorMove()
        {
            Controller.Move(Animator.deltaPosition);
        }

        public override void Exit()
        {
            base.Exit();
            
            _ladderCollider.enabled = true;
            if (HasAnimator)
            {
                Animator.SetBool(_animIDClimbLadder, false);
                Animator.SetBool(_animIDClimbToLadderTop, false);
            }
        }

        private RaycastHit GetRaycastHit()
        {
            Vector3 forward = _manager.hipsRoot.TransformDirection(Vector3.forward);
            var position = _manager.hipsRoot.position;
            Vector3 forwardAbove = new Vector3(position.x, position.y + raycastPositionY, position.z);
            
            Physics.Raycast(forwardAbove, forward, out RaycastHit rayCast, raycastDistance);
            
            return rayCast;
        }
        
        private void CheckLadderTopEnd()
        {
            Vector3 forward = _manager.hipsRoot.TransformDirection(Vector3.forward);
            var position = _manager.hipsRoot.position;
            Vector3 forwardAbove = new Vector3(position.x, position.y + raycastPositionY, position.z);

            if (!Physics.Raycast(forwardAbove, forward, raycastDistance))
            {
                _ladderCollider.enabled = false;
                _isControllableClimb = false;
                Animator.SetTrigger(_animIDClimbToLadderTop);
                
                if (drawDebugRay)
                {
                    Debug.DrawRay(forwardAbove, forward, _manager.transparentRed);
                }
            }
            else
            {
                if (drawDebugRay)
                {
                    Debug.DrawRay(forwardAbove, forward, _manager.transparentGreen);
                }
            }
        }

        private void Move()
        {
            if (Input.move.y == 0)
            {
                Animator.enabled = false;
            }
            else
            {
                Animator.enabled = true;

                if (Input.move.y > 0)
                {
                    Animator.SetFloat(_animIDMotionSpeed, 1);
                }
                else
                {
                    Animator.SetFloat(_animIDMotionSpeed, -1);
                }
            }
        }
    }
}
