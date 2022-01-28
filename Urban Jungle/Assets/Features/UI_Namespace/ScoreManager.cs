using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;
    [SerializeField] private CountdownController countdownController;
    private int _collectedItemCounter;
    private List<String> collectedItems;
    void Awake()
    {
        _collectedItemCounter = 0;
        instance = this;
    }

    public void AddPoints(String itemName)
    {
        _collectedItemCounter += 100;
        collectedItems.Add(itemName);
        Debug.Log("Points for picking up in total: " + _collectedItemCounter);
        Debug.Log("item: " + itemName);
    }
    
    public int GetEndResult()
    {
        int timeLeft = (int)Math.Round(countdownController.GetTimeLeft()*10);
        if (timeLeft<=0)
        {
            Debug.Log("Well shit... I think I owe them an apology...");
            return _collectedItemCounter;
        } else if ((!collectedItems.Any())) //did not collect anything
        {
            Debug.Log("You could have at least put on some pants for me... but at least you made it in time");
            return _collectedItemCounter + timeLeft;
        } else if (collectedItems.Count == 1) //did collect one things
        {
            Debug.Log("Collected one item text");
            return _collectedItemCounter + timeLeft;
        } else if (collectedItems.Count == 2) //did collect two things
        {
            Debug.Log("Collected two items text");
            return _collectedItemCounter + timeLeft;
        } else if(collectedItems.Count == 3) //did collect three things
        {
            Debug.Log("Collected three items text");
            return _collectedItemCounter + timeLeft;
        } else if(collectedItems.Count == 4) //did collect four things
        {
            Debug.Log("Collected four items text");
            return _collectedItemCounter + timeLeft;
        } else //did collect everything
        {
            Debug.Log("WOW look at you! You look really good ;)");
            return _collectedItemCounter + timeLeft;
        }
    }
}
