using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMasterScript : MonoBehaviour
{
    public bool gameOver = false;
    public float resultTime;
    bool a = false;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (gameOver)
        {
            if (a == false)
            {
                StartCoroutine(finishEffect());
                a = true;
            }
        }
    }

    IEnumerator finishEffect()
    {
        yield return new WaitForSeconds(1.5f);

        string a = resultTime.ToString("f2");
        double z = double.Parse(a);
        toTitleScript.resultTimeDouble = z;
        naichilab.RankingLoader.Instance.SendScoreAndShowRanking(z);
    }
}
