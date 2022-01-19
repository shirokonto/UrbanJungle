using Features.Character_Namespace.Scripts.States;
using UnityEngine;

namespace Features.Character_Namespace.Scripts.CharacterBehaviours
{
    public class ForceSpeedBehaviour : MonoBehaviour
    {
        [SerializeField] private MovementType movementType;
        [SerializeField] private MovementSpeed normalSpeed;
        [SerializeField] private MovementSpeed sprintSpeed;

        public float GetTargetSpeed(bool sprint)
        {
            return GetTargetSpeed(sprint, normalSpeed, sprintSpeed);
        }

        public float GetMovementType()
        {
            return (float) movementType;
        }

        public static float GetTargetSpeed(bool sprint, MovementSpeed normalSpeed, MovementSpeed sprintSpeed)
        {
            return (float) (sprint ? sprintSpeed : normalSpeed);
        }
    }
}