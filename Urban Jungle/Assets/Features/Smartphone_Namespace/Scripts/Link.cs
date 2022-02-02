using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Source: https://www.youtube.com/watch?v=_wpnqOnWELY
public class Link : MonoBehaviour
{

    public string URL;

    public void Open()
    {
        Application.OpenURL(URL);
    }
   
}
