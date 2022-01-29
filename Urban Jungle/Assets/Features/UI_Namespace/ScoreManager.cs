using System;
using System.Collections.Generic;
using UnityEngine;

namespace Features.UI_Namespace
{
    public class ScoreManager : MonoBehaviour
    {
        [SerializeField] private CountdownController countdownController;
        private int _collectedItemCounter;
        private List<String> collectedItems;
        public TextAsset text;
        void Awake()
        {
            _collectedItemCounter = 0;
            collectedItems = new List<string>();
        }

        public void AddPoints(String itemName)
        {
            _collectedItemCounter += 100;
            collectedItems.Add(itemName);
            Debug.Log("Picked up " + itemName + "/// Total points: " + _collectedItemCounter + (int)Math.Round(countdownController.GetTimeLeft()));
        }

        public int GetEndPoints()
        {
            int timeLeft = (int)Math.Round(countdownController.GetTimeLeft());
            GetEndMsg(timeLeft);
            return timeLeft <= 0 ? _collectedItemCounter : (_collectedItemCounter + timeLeft);
        }
    
        public String GetEndMsg(int timeLeft)
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
                5 => "WOW look at you! You look really good ;)"
            };
        }
    }
}
