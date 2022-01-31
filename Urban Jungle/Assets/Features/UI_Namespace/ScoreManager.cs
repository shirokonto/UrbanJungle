using System;
using DataStructures.Variables;
using UnityEngine;
using UnityEngine.Serialization;

namespace Features.UI_Namespace
{
    public class ScoreManager : MonoBehaviour
    {
        [SerializeField] private FloatVariable timeLeft;
        [SerializeField] private String endMsg;
        private int _collectedItemCounter;
        private int _itemPoints;
        private int _endPoints;
        void Awake()
        {
            _collectedItemCounter = 0;
            _itemPoints = 0;
        }

        public void AddPoints()
        {
            _collectedItemCounter += 100;
            _itemPoints += 1;
            Debug.Log("Picked up item /// Total points: " + _collectedItemCounter + (int)Math.Round(timeLeft.Get()));
        }

        public void SetEndPoints()
        {
            var timeLeftAtEnd = (int)Math.Round(timeLeft.Get());
            _endPoints= timeLeftAtEnd <= 0 ? _collectedItemCounter : (_collectedItemCounter + timeLeftAtEnd);
        }

        public int GetEndPoints()
        {
            return _endPoints;
        }
    
        public string GetEndMsg(int timeLeft)
        {
            if (timeLeft <= 0)
            {
                return "Well shit... I think I owe them an apology...";
            }

            return _itemPoints switch
            {
                0 => "You could have at least put on some pants for me... but at least you made it in time",
                1 => "Collected one item",
                2 => "Collected two items",
                3 => "Collected three items",
                4 => "Collected four items",
                5 => "WOW look at you! You look really good ;)",
                _ => throw new ArgumentOutOfRangeException()
            };
        }
    }
}
