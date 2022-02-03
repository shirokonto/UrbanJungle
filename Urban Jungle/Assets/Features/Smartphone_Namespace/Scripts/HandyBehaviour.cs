using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using Utils.Event_Namespace;

public class HandyBehaviour : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera handyVC;
    [SerializeField] private int priority = 20;
    [SerializeField] private GameEvent disableCameraRotation;
    [SerializeField] private GameEvent enableCameraRotation;
    
    private void Start()
    {
        handyVC.Priority = priority;
        disableCameraRotation?.Raise();
    }

    public void OnHandyInteraction()
    {
        if (handyVC.Priority == priority)
        {
            handyVC.Priority = 0;
            enableCameraRotation?.Raise();
        }
    }
}
