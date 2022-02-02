using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class HandyBehaviour : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera handyVC;
    [SerializeField] private int priority = 20;
    
    private void Start()
    {
        handyVC.Priority = priority;
    }

    public void OnHandyInteraction()
    {
        if (handyVC.Priority == priority)
        {
            handyVC.Priority = 0;
        }
    }
}
