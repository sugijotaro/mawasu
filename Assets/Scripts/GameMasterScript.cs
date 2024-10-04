using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameMasterScript : MonoBehaviour
{
    public static GameMasterScript Instance { get; private set; }

    public bool gameOver = false;
    public double resultTime;
    private bool isProcessing = false;
    private const string HighScoreKey = "HighScore";

    public double highScore = 0.0;
    public bool isNewHighScore = false;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        highScore = LoadHighScore();
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

        isNewHighScore = SaveResultTime(resultTimeDouble);

        resultTime = resultTimeDouble;

        string leaderboardID = "com.infinity.spinningball.highscoreboard";
        GameCenterManager.Instance.ReportScore(resultTimeDouble, leaderboardID);

        SceneManager.LoadScene("Ranking", LoadSceneMode.Additive);
    }

    bool SaveResultTime(double time)
    {
        if (time > highScore)
        {
            PlayerPrefs.SetFloat(HighScoreKey, (float)time);
            PlayerPrefs.Save();
            Debug.Log("新しいハイスコアを保存しました: " + time);
            highScore = time;
            return true;
        }
        else
        {
            Debug.Log("ハイスコアは更新されませんでした。現在のハイスコア: " + highScore);
            return false;
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