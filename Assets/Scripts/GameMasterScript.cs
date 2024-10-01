using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameMasterScript : MonoBehaviour
{
    public static GameMasterScript Instance { get; private set; }

    public bool gameOver = false;
    public float resultTime;
    private bool isProcessing = false;
    private const string HighScoreKey = "HighScore";
    public Text currentScoreText;
    public Text highScoreText;

    private double highScore = 0.0;

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
            Debug.Log("ハイスコア更新！");
        }
        else
        {

        }

        highScoreText.text = "ハイスコア: " + highScore.ToString("f2") + " 秒";
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