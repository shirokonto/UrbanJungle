using System;
using UnityEngine;

namespace Features.UI_Namespace
{
    public class ScoreManager : MonoBehaviour
    {
        [SerializeField] private CountdownController countdownController;
        [SerializeField] private int endPoints;
        private int _collectedItemCounter;
        void Awake()
        {
            _collectedItemCounter = 0;
        }

        public void AddPoints()
        {
            _collectedItemCounter += 100;
            Debug.Log("Picked up item /// Total points: " + _collectedItemCounter + (int)Math.Round(countdownController.GetTimeLeft()));
        }

        public void SetEndPoints()
        {
            var timeLeft = (int)Math.Round(countdownController.GetTimeLeft());
            endPoints= timeLeft <= 0 ? _collectedItemCounter : (_collectedItemCounter + timeLeft);
        }

        public int GetEndPoints()
        {
            return endPoints;
        }
    
        public string SetEndMsg(int timeLeft)
        {
            if (timeLeft <= 0)
            {
                return "Well shit... I think I owe them an apology...";
            }

            return _collectedItemCounter switch
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
