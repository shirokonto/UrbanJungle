using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class CanvasManager : MonoBehaviour
{
    private List<CanvasController> canvasControllerList;
    
    private List<CanvasController> canvasCommandList = new List<CanvasController>();
    private CanvasController CurrentCanvas => canvasCommandList[canvasCommandList.Count - 1];

    private void Awake()
    {
        canvasControllerList = GetComponentsInChildren<CanvasController>().ToList();
        canvasControllerList.ForEach(x => x.gameObject.SetActive(false));
        
        MenuType_SO startingMenu = canvasControllerList.Find(controller => controller.isStartMenu).canvasType;
        AddCanvas(startingMenu);
    }

    public void HideCanvas()
    {
        CurrentCanvas.gameObject.SetActive(false);
    }
    
    public void ShowCanvas()
    {
        CurrentCanvas.gameObject.SetActive(true);
    }

    public void AddCanvas(MenuType_SO type)
    {
        if (canvasCommandList.Count != 0)
        {
            CurrentCanvas.gameObject.SetActive(false);
        }
        
        CanvasController desiredCanvas = canvasControllerList.Find(x => x.canvasType == type);
        if (desiredCanvas != null)
        {
            desiredCanvas.gameObject.SetActive(true);
            canvasCommandList.Add(desiredCanvas);
        }
        else
        {
            Debug.LogWarning("Desired canvas was not found");
        }
    }

    public void RemoveCanvas()
    {
        if (canvasCommandList.Count != 0)
        {
            HideCanvas();
            canvasCommandList.Remove(CurrentCanvas);
            ShowCanvas();
        }
        else
        {
            Debug.LogWarning("No last active canvas");
        }
    }

    public void RemoveCanvasTo(MenuType_SO type)
    {
        if (canvasCommandList.Count != 0)
        {
            CanvasController desiredCanvas = canvasCommandList.Find(x => x.canvasType == type);
            
            if (desiredCanvas != null)
            {
                HideCanvas();
                
                int canvasIndex = canvasCommandList.FindIndex(x => x.canvasType == type);
                canvasCommandList.RemoveRange(canvasIndex + 1, canvasCommandList.Count - 1 - canvasIndex);
                
                desiredCanvas.gameObject.SetActive(true);
            }
            else
            {
                Debug.LogError("The requested menu wasn't opened before!");
            }
        }
        else
        {
            Debug.LogWarning("No last active canvas");
        }
    }

    public void RemoveAllCanvas()
    {
        CurrentCanvas.gameObject.SetActive(false);
        canvasCommandList.Clear();
    }
}
