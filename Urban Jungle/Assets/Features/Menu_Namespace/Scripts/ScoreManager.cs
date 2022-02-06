using System;
using DataStructures.Variables;
using TMPro;
using UnityEngine;

namespace Features.UI_Namespace
{
    public class ScoreManager : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI endMsgTxt;
        [SerializeField] private TextMeshProUGUI endPointsTxt;
        [SerializeField] private TextMeshProUGUI itemCounterTxt;
        [SerializeField] private TextMeshProUGUI timeLeftTxt;
        [SerializeField] private TextMeshProUGUI senderTxt;
        [SerializeField] private IntVariable itemCounter;
        [SerializeField] private IntVariable endPoints;
        [SerializeField] private FloatVariable timeLeft;
        
        private int _itemPoints;
        private int _timePoints;
        private int _endPoints;
        void Awake()
        {
              ResetPoints();
        }

        public void SetEndPoints()
        {
            _itemPoints = itemCounter.Get() * 1500;
            _timePoints = (int)Math.Round(timeLeft.Get()*10);
            _endPoints= _timePoints <= 0 ? GetDeductedItemPoints() : (_itemPoints + _timePoints);
            endPoints.Set(_endPoints);
            endPointsTxt.text = _endPoints.ToString();
        }
        
        public void SetEndTxts()
        {
            endMsgTxt.text = GetCorrectMessage();
            timeLeftTxt.text = GetTimeLeft();
            senderTxt.text = _timePoints <= 0 ? "Wilma" : "Alex";
            itemCounterTxt.text = itemCounter.Get().ToString();
            ResetItems();
        }

        //If there is no more time left, item points are deducted in ascending order (10% to 50%)
        public int GetDeductedItemPoints()
        {
            return (int)Math.Round((_itemPoints) * (1 - (0.1 * itemCounter.Get())));
        }

        private string GetTimeLeft()
        {
            if (timeLeft.Get() <= 0)
            {
                return "00:00";
            }
            return Math.Floor(timeLeft.Get()/60).ToString("0") + ":" + Math.Floor(timeLeft.Get() % 60).ToString("00");
        }
        
        private string GetCorrectMessage()
        {
            if (_timePoints <= 0)
            {
                return "Well shit... I think I owe them an apology...";
            }

            return itemCounter.Get() switch
            {
                0 => "You could have at least put on some pants for me... but at least you made it in time.",
                1 => "What...the.. Oh wait, I get it! Am I on Disaster Date? Where are the cameras?",
                2 => "Uhm, you sure look... different from what I expected... But I guess you put on some clothes?",
                3 => "I like that you didn't dress up for me, it shows how down-to-earth you are. *sarcastic*",
                4 => "Well, you look pretty good for someone who just ran over half the city.",
                5 => "WOW are you a professional parkour runner? You look really good!",
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        private void ResetPoints()
        {
            _itemPoints = _timePoints = _endPoints = 0;
            timeLeft.Set(0);
        }

        private void ResetItems()
        {
            itemCounter.Set(0);
        }
    }
}
