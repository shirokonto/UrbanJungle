using System;
using System.Collections;
using DataStructures.Variables;
using TMPro;
using TMPro.Examples;
using UnityEngine;
using UnityEngine.UI;

namespace Features.Menus_Namespace
{
    // Source: https://www.youtube.com/watch?v=KZuqEyxYZCc

    public class HighScores : MonoBehaviour
    {
        // key & web address to fetch data with
        const string PrivateCode = "FWn86bbbtEWjYDchdkqGFgH2ozULOlhk-r6p9on4AE7w"; 
        const string PublicCode = "61f95e578f40bb1058bfc083";   
        const string WebURL = "http://dreamlo.com/lb/";

        private PlayerScore[] _scoreList;
        DisplayHighscores _myDisplay;
        static HighScores _instance;

        [SerializeField] private Text inputName;
        [SerializeField] private IntVariable endPoints;
        [SerializeField] private Text playerScore;
    
        void Awake()
        {
            _instance = this;
            _myDisplay = GetComponent<DisplayHighscores>();
        
        }
    
        public void UploadScore()
        {
            if (endPoints.Get() == 0) return;
            var username = inputName.text;
            _instance.StartCoroutine(_instance.DatabaseUpload(username,endPoints.Get()));
            endPoints.Set(0);
        }

        public void Update()
        {
            playerScore.text = endPoints.Get().ToString();
        } 

        IEnumerator DatabaseUpload(string userame, int score) //Called when sending new score to Website
        {
            WWW www = new WWW(WebURL + PrivateCode + "/add/" + WWW.EscapeURL(userame) + "/" + score);
            yield return www;

            if (string.IsNullOrEmpty(www.error))
            {
                print("Upload Successful");
                DownloadScores();
            }
            else print("Error uploading" + www.error);
        }

        public void DownloadScores()
        {
            StartCoroutine(nameof(DatabaseDownload));
        }
        IEnumerator DatabaseDownload()
        {
            //WWW www = new WWW(webURL + publicCode + "/pipe/"); //Gets the whole list
            WWW www = new WWW(WebURL + PublicCode + "/pipe/0/10"); //Gets top 10
            yield return www;

            if (string.IsNullOrEmpty(www.error))
            {
                OrganizeInfo(www.text);
                _myDisplay.SetScoresToMenu(_scoreList);
            }
            else print("Error uploading" + www.error);
        }

        void OrganizeInfo(string rawData)
        {
            string[] entries = rawData.Split(new char[] {'\n'}, System.StringSplitOptions.RemoveEmptyEntries);
            _scoreList = new PlayerScore[entries.Length];
            for (int i = 0; i < entries.Length; i ++)
            {
                string[] entryInfo = entries[i].Split(new char[] {'|'});
                string username = entryInfo[0];
                int score = int.Parse(entryInfo[1]);
                _scoreList[i] = new PlayerScore(username,score);
                print(_scoreList[i].Username + ": " + _scoreList[i].Score);
            }
        }
    }

    public struct PlayerScore 
    {
        public readonly string Username;
        public readonly int Score;

        public PlayerScore(string username, int score)
        {
            Username = username;
            Score = score;
        }
    }
}