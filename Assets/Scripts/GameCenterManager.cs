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
}