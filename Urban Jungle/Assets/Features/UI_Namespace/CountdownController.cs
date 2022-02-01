using System;
using DataStructures.Variables;
using UnityEngine;
using UnityEngine.UI;
using Utils.Event_Namespace;

public class CountdownController : MonoBehaviour
{
    [SerializeField] private FloatVariable currentTime;
    [SerializeField] private Text countdownInfo;
    [SerializeField] private Text countdownTime;
    [SerializeField] private GameEvent onLoadLoseMenu;
    private float _startingTime = 600f;
    private string _timeGrade;
    
    // Start is called before the first frame update
    void Start()
    { 
        currentTime.Set(_startingTime);
        _timeGrade = "Ezzzzz";
        countdownInfo.text = "Timing Level: " + _timeGrade;
    }

    // Update is called once per frame
    void Update ()
    {
        currentTime.Add(-1 * Time.deltaTime);
        countdownTime.text = Math.Floor(currentTime.Get()/60).ToString("0") + ":" + Math.Floor(currentTime.Get() % 60).ToString("00");
        
        if (currentTime.Get() <= 300f) {
            _timeGrade = "Sweaty Armpits";
            countdownInfo.text = "Timing Level: " + _timeGrade;
            countdownInfo.color = Color.yellow;
            countdownTime.color = Color.yellow;
            
            if (currentTime.Get() <= 60f)
            {
                _timeGrade = "F************!";
                countdownInfo.text = _timeGrade;
                countdownInfo.fontStyle = FontStyle.BoldAndItalic;
                countdownInfo.color = Color.red;
                countdownTime.color = Color.red;
                
                if (currentTime.Get() <= 0)
                {
                    countdownTime.text = "";
                    countdownInfo.text = "You are too late!";
                    onLoadLoseMenu?.Raise();
                }
            }
        }
    }
}
