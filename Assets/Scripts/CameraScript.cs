using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CameraScript : MonoBehaviour
{
    public UIScript uiScript;
    bool startAnimation = false;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(uiScript.start == true)
        {
            startAnimation = true;
            uiScript.start = false;
        }

        if (startAnimation)
        {
            transform.DOMove(new Vector3(0, -1.75f, -2), 1.2f);
        }
    }

    void moveCamera()
    {

    }
}
