using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class toTitleScript : MonoBehaviour
{
    public static double resultTimeDouble = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ToTitleButtonTapped()
    {
        Debug.Log("タイトルへ");
        SceneManager.LoadScene(0);
    }

    public void Tweet()
    {
        Debug.Log("ツイート");
        string sentence = "バスケットボールを" + resultTimeDouble + "秒回しました";
        Debug.Log(sentence);
        naichilab.UnityRoomTweet.Tweet("spinningball", sentence, "unity1week", "SpinningBall");
    }
}
