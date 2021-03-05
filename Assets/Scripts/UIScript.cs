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
    public Text CountDown;
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
        CountDown.enabled = false;
        time.enabled = false;
        watch.enabled = false;
        initilal = whiteboard.transform.position;
        Debug.Log(initilal + "white");

        version.text = "ver1.14";
        /*
            1.00　リリース　2021.2.28
            1.10　スマホ対応　2021.3.1
            1.11　いろいろ修正 バージョン表記　2021.3.1
            1.12　ツイート機能　2021.3.2
            1.13　スマホ版操作改善　2021.3.2
            1.131　キャッシュを残さないように　2021.3.2
            1.14　スマホ版操作改善　update関数の方で分岐をつけた　2021.3.4
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
                finish();

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

        StartCoroutine(countDown());
    }

    IEnumerator countDown()
    {
        yield return new WaitForSeconds(1.5f);
        CountDown.text = "Ready";
        CountDown.fontSize = 100;
        CountDown.enabled = true;
        time.enabled = true;
        watch.enabled = true;
        mask.SetActive(false);

        yield return new WaitForSeconds(1.2f);
        if (b == false)
        {
            CountDown.text = "GO!";
            CountDown.fontSize = 130;
            countup = 0.0f;
            counting = true;

            audioSource = gameObject.GetComponent<AudioSource>();
            audioSource.clip = startSE;
            audioSource.Play();
        }

        yield return new WaitForSeconds(0.2f);
        if (b == false)
        {
            CountDown.enabled = false;
        }
    }

    void finish()
    {
        StartCoroutine(finishEffect());
    }

    IEnumerator finishEffect()
    {
        CountDown.text = "Finish";
        CountDown.fontSize = 100;
        CountDown.enabled = true;

        audioSource = gameObject.GetComponent<AudioSource>();
        audioSource.clip = finishSE;
        audioSource.Play();

        yield return new WaitForSeconds(1.5f);
        CountDown.enabled = false;
        mask.SetActive(true);
    }
}
