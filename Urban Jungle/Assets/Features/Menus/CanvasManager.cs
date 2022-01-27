using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class CanvasManager : MonoBehaviour
{
    private List<CanvasController> canvasControllerList;
    private CanvasController lastActiveCanvas;

    private void Awake()
    {
        canvasControllerList = GetComponentsInChildren<CanvasController>().ToList();
        canvasControllerList.ForEach(x => x.gameObject.SetActive(false));
        
        MenuType_SO startingMenu = canvasControllerList.Find(controller => controller.isStartMenu).canvasType;
        SwitchCanvas(startingMenu);
    }

    public void SwitchCanvas(MenuType_SO type)
    {
        if (lastActiveCanvas != null)
        {
            lastActiveCanvas.gameObject.SetActive(false);
        }
        CanvasController desiredCanvas = canvasControllerList.Find(x => x.canvasType == type);
        if (desiredCanvas != null)
        {
            desiredCanvas.gameObject.SetActive(true);
            lastActiveCanvas = desiredCanvas;
        }
        else
        {
            Debug.LogWarning("Desired canvas was not found");
        }
        
        
    }

    public void CloseCanvas()
    {
        if (lastActiveCanvas != null)
        {
            lastActiveCanvas.gameObject.SetActive(false);
            lastActiveCanvas = null;
        }
        else
        {
            Debug.LogWarning("No last active canvas");
        }
        
    }
    
}
