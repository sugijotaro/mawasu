using UnityEngine;
using UnityEngine.SocialPlatforms;
using UnityEngine.SocialPlatforms.GameCenter;

public class GameCenterManager : MonoBehaviour
{
    private static GameCenterManager _instance;

    public static GameCenterManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<GameCenterManager>();

                if (_instance == null)
                {
                    GameObject singletonObject = new GameObject("GameCenterManager");
                    _instance = singletonObject.AddComponent<GameCenterManager>();
                    DontDestroyOnLoad(singletonObject);
                }
            }
            return _instance;
        }
    }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
            return;
        }

        _instance = this;
        DontDestroyOnLoad(this.gameObject);

        AuthenticateUser();
    }

    private void AuthenticateUser()
    {
        Social.Active = new GameCenterPlatform();
        Social.localUser.Authenticate(success =>
        {
            if (success)
            {
                Debug.Log("Game Center にログインしました: " + Social.localUser.userName);
            }
            else
            {
                Debug.Log("Game Center のログインに失敗しました");
            }
        });
    }

    public void ReportScore(double timeInSeconds, string leaderboardID)
    {
        if (Social.localUser.authenticated)
        {
            long score = (long)(timeInSeconds * 1000);

            Social.ReportScore(score, leaderboardID, success =>
            {
                if (success)
                {
                    Debug.Log("スコアを送信しました: " + score);
                }
                else
                {
                    Debug.Log("スコアの送信に失敗しました");
                }
            });
        }
        else
        {
            Debug.Log("ユーザーが認証されていません");
        }
    }
    public void ShowLeaderboard(string leaderboardID)
    {
        if (Social.localUser.authenticated)
        {
            GameCenterPlatform.ShowLeaderboardUI(leaderboardID, UnityEngine.SocialPlatforms.TimeScope.AllTime);
        }
        else
        {
            Debug.Log("ユーザーが認証されていません");
        }
    }
}