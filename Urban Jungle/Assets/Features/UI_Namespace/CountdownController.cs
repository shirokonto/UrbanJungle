using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CountdownController : MonoBehaviour
{
    public float currentTime = 0f;

    private float startingTime = 600f;

    private string timeGrade;

    [SerializeField] private Text countdownInfo;
    
    [SerializeField] private Text countdownTime;
    
    // Start is called before the first frame update
    void Start()
    { 
        currentTime = startingTime;

        timeGrade = "Ezzzzz";

        countdownInfo.text = "Timing Level: " + timeGrade;

    }

    // Update is called once per frame
    void Update ()
    {
        currentTime -= 1 * Time.deltaTime;

        countdownTime.text = Math.Floor(currentTime / 60).ToString("0") + ":" + Math.Floor(currentTime % 60).ToString("00");
        
        if (currentTime <= 300f)
        {
            timeGrade = "Sweaty Armpits";
            
            countdownInfo.text = "Timing Level: " + timeGrade;
            
            countdownInfo.color = Color.yellow;
            
            countdownTime.color = Color.yellow;
            
            if (currentTime <= 60f)
            {
                timeGrade = "F************!";
                
                countdownInfo.text = timeGrade;

                countdownInfo.fontStyle = FontStyle.BoldAndItalic;

                countdownInfo.color = Color.red;
                
                countdownTime.color = Color.red;
                
                if (currentTime <= 0)
                {
                    countdownTime.text = "";
                    countdownInfo.text = "You are too late!";
                    // player looses?
                }
            }
        }
    }
    
    public float GetTimeLeft()
    {
        return currentTime;
    }
}
