using UnityEngine;
using UnityEngine.UI;

public class RankingSceneManager : MonoBehaviour
{
    public Text currentScoreText;
    public Text highScoreText;

    void Start()
    {
        double resultTime = GameMasterScript.Instance.resultTime;
        double highScore = GameMasterScript.Instance.highScore;
        bool isNewHighScore = GameMasterScript.Instance.isNewHighScore;

        currentScoreText.text = resultTime.ToString("f2");
        highScoreText.text = highScore.ToString("f2");

        if (isNewHighScore)
        {
        }
        else
        {
        }
    }
}