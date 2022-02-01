using System.Collections;
using UnityEngine;

// Source: https://www.youtube.com/watch?v=KZuqEyxYZCc

namespace Features.Menus_Namespace
{
    public class DisplayHighscores : MonoBehaviour 
    {
        public TMPro.TextMeshProUGUI[] rNames;
        public TMPro.TextMeshProUGUI[] rScores;
        private HighScores _myScores;

        private void Start() 
        {
            for (int i = 0; i < rNames.Length;i ++)
            {
                rNames[i].text = i + 1 + ". Fetching...";
            }
            _myScores = GetComponent<HighScores>();
            StartCoroutine(nameof(RefreshHighscores));
        }
        public void SetScoresToMenu(PlayerScore[] highscoreList)
        {
            for (int i = 0; i < rNames.Length;i ++)
            {
                rNames[i].text = "";
                if (highscoreList.Length > i)
                {
                    rScores[i].text = highscoreList[i].Score.ToString();
                    rNames[i].text = highscoreList[i].Username;
                }
            }
        }
        IEnumerator RefreshHighscores()
        {
            while(true)
            {
                _myScores.DownloadScores();
                yield return new WaitForSeconds(30);
            }
        }
    }
}