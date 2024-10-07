using UnityEngine;
using UnityEngine.SocialPlatforms;
#if UNITY_IOS
using UnityEngine.SocialPlatforms.GameCenter;
#endif
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
        #if UNITY_IOS
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
            return;
        }

        _instance = this;
        DontDestroyOnLoad(this.gameObject);

        AuthenticateUser();
        #endif
    }

    private void AuthenticateUser()
    {
        #if UNITY_IOS
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
        #endif
    }

    public void ReportScore(double timeInSeconds, string leaderboardID)
    {
        #if UNITY_IOS
        if (Social.localUser.authenticated)
        {
            long score = (long)(timeInSeconds * 100);

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
        #endif
    }

    public void ShowLeaderboard(string leaderboardID)
    {
        #if UNITY_IOS
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
        #endif
    }

    public void ReportAchievement(string achievementID, double progress)
    {
        #if UNITY_IOS
        if (Social.localUser.authenticated)
        {
            Social.ReportProgress(achievementID, progress, success =>
            {
                if (success)
                {
                    Debug.Log($"達成項目 {achievementID} を授与しました");
                    FirebaseAnalytics.LogEvent("achievement_unlocked", new Parameter("achievement_id", achievementID));
                }
                else
                {
                    Debug.Log($"達成項目 {achievementID} の授与に失敗しました");
                }
            });
        }
        else
        {
            Debug.Log("ユーザーが認証されていません");
        }
        #endif
    }
}