using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMasterScript : MonoBehaviour
{
    public static GameMasterScript Instance { get; private set; }

    public bool gameOver = false;
    public double resultTime;
    private bool isProcessing = false;
    private const string HighScoreKey = "HighScore";
    private const string ChallengeCountKey = "ChallengeCount";

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
        string weeklyLeaderboardID = "com.infinity.spinningball.highscoreboard.weekly";
        GameCenterManager.Instance.ReportScore(resultTimeDouble, leaderboardID);
        GameCenterManager.Instance.ReportScore(resultTimeDouble, weeklyLeaderboardID);

        IncrementChallengeCount();

        SceneManager.LoadScene("Ranking", LoadSceneMode.Additive);
    }

    private void IncrementChallengeCount()
    {
        int challengeCount = PlayerPrefs.GetInt(ChallengeCountKey, 0);
        challengeCount++;
        PlayerPrefs.SetInt(ChallengeCountKey, challengeCount);
        PlayerPrefs.Save();

        double progress10 = Mathf.Min((challengeCount / 10.0f) * 100.0f, 100.0f);
        GameCenterManager.Instance.ReportAchievement("challenge_10", progress10);

        double progress100 = Mathf.Min((challengeCount / 100.0f) * 100.0f, 100.0f);
        GameCenterManager.Instance.ReportAchievement("challenge_100", progress100);

        double progress1000 = Mathf.Min((challengeCount / 1000.0f) * 100.0f, 100.0f);
        GameCenterManager.Instance.ReportAchievement("challenge_1000", progress1000);

        double progress10000 = Mathf.Min((challengeCount / 10000.0f) * 100.0f, 100.0f);
        GameCenterManager.Instance.ReportAchievement("challenge_10000", progress10000);

        Debug.Log("現在のチャレンジ回数: " + challengeCount);
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