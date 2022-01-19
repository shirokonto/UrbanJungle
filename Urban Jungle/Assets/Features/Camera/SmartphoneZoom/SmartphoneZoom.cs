using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class SmartphoneZoom : MonoBehaviour
{
  [SerializeField]  CinemachineVirtualCamera _virtualCamera;
   CinemachineComponentBase _componentBase;
   float cameraDistance;
   [SerializeField] float sensitivity = 10f;

   private void Update()
   {
       if (_componentBase == null)
       {
           _componentBase = _virtualCamera.GetCinemachineComponent(CinemachineCore.Stage.Body);
       }

       if (Input.GetAxis("Mouse ScrollWheel") != 0)
       {
           cameraDistance = Input.GetAxis("Mouse ScrollWheel") * sensitivity;
           if (_componentBase is CinemachineFramingTransposer)
           {
               (_componentBase as CinemachineFramingTransposer).m_CameraDistance -= cameraDistance;
           }
       }
       
   }
}
