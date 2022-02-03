using Cinemachine;
using UnityEngine;
using Utils.Event_Namespace;

public class SmartphoneBehaviour : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera smartphoneVc;
    [SerializeField] private int priority = 20;
    [SerializeField] private GameEvent disableCameraRotation;
    [SerializeField] private GameEvent enableCameraRotation;
    
    private void Start()
    {
        smartphoneVc.Priority = priority;
        disableCameraRotation?.Raise();
    }

    public void OnSmartphoneInteraction()
    {
        if (smartphoneVc.Priority == priority)
        {
            smartphoneVc.Priority = 0;
            GetComponent<AudioSource>().Play();
            enableCameraRotation?.Raise();
        }
    }
}
