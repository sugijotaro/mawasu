using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameMasterScript : MonoBehaviour
{
    public bool gameOver = false;
    public float resultTime;
    private bool isProcessing = false;
    private const string HighScoreKey = "HighScore";

    public TextMeshProUGUI currentScoreText;
    public TextMeshProUGUI highScoreText;

    private double highScore = 0.0;

    void Start()
    {
        highScore = LoadHighScore();
        highScoreText.text = "ハイスコア: " + highScore.ToString("f2") + " 秒";
    }

    void Update()
    {
        if (gameOver)
        {
            if (!isProcessing)
            {
                StartCoroutine(FinishEffect());
                isProcessing = true;
            }
        }
    }

    IEnumerator FinishEffect()
    {
        yield return new WaitForSeconds(1.5f);

        string resultTimeString = resultTime.ToString("f2");
        double resultTimeDouble = double.Parse(resultTimeString);

        SceneManager.LoadScene("Ranking", LoadSceneMode.Additive);
        
        bool isNewHighScore = SaveResultTime(resultTimeDouble);

        currentScoreText.text = "今回のスコア: " + resultTimeDouble.ToString("f2") + " 秒";

        if (isNewHighScore)
        {
        }
        else
        {
        }
        highScoreText.text = "ハイスコア: " + resultTimeDouble.ToString("f2") + " 秒";
    }

    bool SaveResultTime(double time)
    {
        if (time > highScore)
        {
            PlayerPrefs.SetFloat(HighScoreKey, (float)time);
            PlayerPrefs.Save();
            Debug.Log("新しいハイスコアを保存しました: " + time);
            highScore = time; // ハイスコアを更新
            return true;      // ハイスコア更新
        }
        else
        {
            Debug.Log("ハイスコアは更新されませんでした。現在のハイスコア: " + highScore);
            return false;     // ハイスコア更新でない
        }
    }

    double LoadHighScore()
    {
        if (PlayerPrefs.HasKey(HighScoreKey))
        {
            float savedTime = PlayerPrefs.GetFloat(HighScoreKey);
            Debug.Log("保存されたハイスコアを読み込みました: " + savedTime);
            return savedTime;
        }
        else
        {
            Debug.Log("ハイスコアが見つかりませんでした。");
            return 0.0;
        }
    }
}