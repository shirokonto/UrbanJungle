using UnityEngine;

namespace Features.Character_Namespace.Scripts
{
    public class BounceBehaviour : MonoBehaviour
    {
        [SerializeField] private float velocityAddition = 1;
        [SerializeField] private float maxVelocity = 12;
        [SerializeField] private float jumpSpeedAddition = 1;
        [SerializeField] private float maxJumpSpeed = 3;

        public void ApplyBounce(ThirdPersonManager manager)
        {
            manager.VerticalVelocity = manager.VerticalVelocity * -1;
            if (manager.VerticalVelocity < maxVelocity)
            {
                manager.VerticalVelocity += velocityAddition;
            }

            if (manager.Input.move == Vector2.zero)
            {
                if (manager.JumpSpeed > 0)
                {
                    manager.JumpSpeed -= jumpSpeedAddition;
                }
            }
            else
            {
                if (manager.JumpSpeed < maxJumpSpeed)
                {
                    manager.JumpSpeed += jumpSpeedAddition;
                }
            }

            manager.JumpSpeed = Mathf.Clamp(Mathf.Round(manager.JumpSpeed), 0, 3);
        }
    }
}
