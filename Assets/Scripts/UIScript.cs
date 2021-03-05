using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;

public class UIScript : MonoBehaviour
{
    public bool start = false;
    bool started = false;
    bool counting = false;
    public GameObject startButton;
    public GameObject mask;
    public Text title;
    public Text version;
    public RectTransform whiteboard;
    public Text countDownText;
    public Text time;
    public Image watch;
    public GameMasterScript gameMasterScript;
    Vector3 initilal;
    private float countup = 0.0f;
    bool a = false;
    bool b = false;
    public AudioClip startSE;
    public AudioClip finishSE;
    private AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        countDownText.enabled = false;
        time.enabled = false;
        watch.enabled = false;
        initilal = whiteboard.transform.position;
        Debug.Log(initilal + "white");

        version.text = "ver1.15";
        /*
            1.00　リリース　2021.2.28
            1.10　スマホ対応　2021.3.1
            1.11　いろいろ修正 バージョン表記　2021.3.1
            1.12　ツイート機能　2021.3.2
            1.13　スマホ版操作改善　2021.3.2
            1.131　キャッシュを残さないように　2021.3.2
            1.14　スマホ版操作改善　update関数の方で分岐をつけた　2021.3.4
            1.15　バウンド、タップの効果音追加、回転エフェクト追加　2021.3.5
        */
    }

    // Update is called once per frame
    void Update()
    {
        if (start)
        {
            started = true;
        }

        if (started)
        {
            whiteboard.transform.DOMove(new Vector3(initilal.x, initilal.y + 106, initilal.z), 1f);
        }
        if (counting)
        {
            countup += Time.deltaTime;
            time.text = countup.ToString("f2");
        }
        if (gameMasterScript.gameOver)
        {
            if (a == false)
            {
                counting = false;
                if(countup == 0.0f)
                {
                    b = true;
                }
                else
                {
                    gameMasterScript.resultTime = countup;
                }
                Finish();

                a = true;
            }
        }
        if (b)
        {
            time.text = "0.00";
            gameMasterScript.resultTime = 0.0f;
        }
    }

    public void StartButtonTapped()
    {
        Debug.Log("スタート");
        start = true;
        title.enabled = false;
        version.enabled = false;
        startButton.SetActive(false);

        StartCoroutine(CountDown());
    }

    IEnumerator CountDown()
    {
        mask.SetActive(false);
        yield return new WaitForSeconds(1.5f);
        countDownText.text = "Ready";
        countDownText.fontSize = 100;
        countDownText.enabled = true;
        time.enabled = true;
        watch.enabled = true;

        yield return new WaitForSeconds(1.2f);
        if (b == false)
        {
            countDownText.text = "GO!";
            countDownText.fontSize = 130;
            countup = 0.0f;
            counting = true;

            audioSource = gameObject.GetComponent<AudioSource>();
            audioSource.clip = startSE;
            audioSource.volume = 0.5f;
            audioSource.Play();
        }

        yield return new WaitForSeconds(0.2f);
        if (b == false)
        {
            countDownText.enabled = false;
        }
    }

    void Finish()
    {
        StartCoroutine(FinishEffect());
    }

    IEnumerator FinishEffect()
    {
        countDownText.text = "Finish";
        countDownText.fontSize = 100;
        countDownText.enabled = true;

        audioSource = gameObject.GetComponent<AudioSource>();
        audioSource.clip = finishSE;
        audioSource.volume = 0.5f;
        audioSource.Play();

        yield return new WaitForSeconds(1.5f);
        countDownText.enabled = false;
        mask.SetActive(true);
    }
}
