using Features.UI_Namespace;
using UnityEngine;
using UnityEngine.UI;

public class ScoreScreen : MonoBehaviour
{
    [SerializeField] private Text endMsgTxt;
    [SerializeField] private Text pointsTxt;
    [SerializeField] private ScoreManager scoreManager;
    public void SetUp(int score)
    {
        gameObject.SetActive(true);
        pointsTxt.text = score.ToString();
        endMsgTxt.text = score.ToString();
    }
}
