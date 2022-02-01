using System;
using DataStructures.Variables;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Features.UI_Namespace
{
    public class ScoreManager : MonoBehaviour
    {
        [SerializeField] private FloatVariable timeLeft;
        [SerializeField] private TextMeshProUGUI endMsg;
        [SerializeField] private TextMeshProUGUI endPoints;
        [SerializeField] private IntVariable pointsCounter; //need for highscore?
        private int _itemCounter;
        private int _itemPoints;
        private int _timePoints;
        private int _endPoints;
        void Awake()
        {
            _itemCounter = _itemPoints = _timePoints = _endPoints = 0;
        }

        public void AddPoints()
        {
            _itemCounter += 1;
            _itemPoints += 100;
            Debug.Log("Picked up item /// Total points: " + _itemPoints + (int)Math.Round(timeLeft.Get()));
        }

        public void SetEndPoints()
        {
            _timePoints = (int)Math.Round(timeLeft.Get());
            _endPoints= _timePoints <= 0 ? _itemPoints : (_itemPoints + _timePoints);
            endPoints.text = _endPoints.ToString();
        }

        public void SetEndMsg()
        {
            endMsg.text = GetCorrectMessage();
        }
    
        private string GetCorrectMessage()
        {
            if (_timePoints <= 0)
            {
                return "Well shit... I think I owe them an apology...";
            }

            return _itemCounter switch
            {
                0 => "You could have at least put on some pants for me... but at least you made it in time",
                1 => "What...the.. Oh wait, I get it! Am I on Disaster Date? Where are the cameras?",
                2 => "Uhm, you sure look... different from what i expected... But i guess you put on some clothes?",
                3 => "I like that you didn't dress up for me, it shows how down-to-earth you are. *sarcastic*",
                4 => "Well, you look pretty good for someone who just ran over half the city",
                5 => "WOW are you a professional parkour runner? You look really good!",
                _ => throw new ArgumentOutOfRangeException()
            };
        }
    }
}
