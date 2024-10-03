using UnityEngine;
using UnityEngine.SocialPlatforms;
using UnityEngine.SocialPlatforms.GameCenter;
using Firebase.Analytics;

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

                FirebaseAnalytics.LogEvent("game_center_login", new Parameter("user_name", Social.localUser.userName));
            }
            else
            {
                Debug.Log("Game Center のログインに失敗しました");

                FirebaseAnalytics.LogEvent("game_center_login_failed");
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

                    FirebaseAnalytics.LogEvent("report_score", new Parameter("leaderboard_id", leaderboardID), new Parameter("score", score));
                }
                else
                {
                    Debug.Log("スコアの送信に失敗しました");

                    FirebaseAnalytics.LogEvent("report_score_failed", new Parameter("leaderboard_id", leaderboardID), new Parameter("score", score));
                }
            });
        }
        else
        {
            Debug.Log("ユーザーが認証されていません");

            FirebaseAnalytics.LogEvent("report_score_not_authenticated");
        }
    }

    public void ShowLeaderboard(string leaderboardID)
    {
        if (Social.localUser.authenticated)
        {
            GameCenterPlatform.ShowLeaderboardUI(leaderboardID, UnityEngine.SocialPlatforms.TimeScope.AllTime);

            FirebaseAnalytics.LogEvent("show_leaderboard", new Parameter("leaderboard_id", leaderboardID));
        }
        else
        {
            Debug.Log("ユーザーが認証されていません");

            FirebaseAnalytics.LogEvent("show_leaderboard_not_authenticated");
        }
    }
}