using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.UI;

public class PlayerScript : MonoBehaviour
{
    bool isMobile;

    private Vector3 moveTo;
    private Vector3 initialPosition;
    private bool beRay = false;
    public BallScript ballScript;


#if !UNITY_EDITOR && UNITY_WEBGL
    [System.Runtime.InteropServices.DllImport("__Internal")]
    static extern bool IsMobile();
#endif

    // Start is called before the first frame update
    void Start()
    {
#if !UNITY_EDITOR && UNITY_WEBGL
        isMobile = IsMobile();
#endif

        //GetComponent<Text>().text = isMobile ? "Mobile" : "PC";
        Debug.Log(isMobile ? "Mobile" : "PC");

        initialPosition = this.transform.position;
        Debug.Log(initialPosition);
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(Input.touchCount);
        if (isMobile)
        {
            //Mobile
            for (int i = 0; i < Input.touchCount; i++)
            {
                // タッチ情報をコピー
                Touch t = Input.GetTouch(i);
                //タッチした位置からRayを飛ばす
                Ray ray = Camera.main.ScreenPointToRay(t.position);
                Debug.Log(ray);
                RaycastHit hit = new RaycastHit();
                if (Physics.Raycast(ray, out hit))
                {
                    //Rayを飛ばしてあたったオブジェクトが自分自身だったら
                    if (hit.collider.gameObject == transform.Find("Cube").gameObject)
                    {
                        if (Input.GetMouseButtonDown(0))
                        {
                            RayCheckMobile();
                        }

                        if (beRay)
                        {
                            MovePoisitionMobile();
                        }

                        if (Input.GetMouseButtonUp(0))
                        {
                            beRay = false;
                        }
                    }
                }
            }
        }
        else
        {
            //PC
            if (Input.GetMouseButtonDown(0))
            {
                RayCheck();
            }

            if (beRay)
            {
                MovePoisition();
            }

            if (Input.GetMouseButtonUp(0))
            {
                beRay = false;
            }
        }
    }

    private void RayCheckMobile()
    {
        for (int i = 0; i < Input.touchCount; i++)
        {
            // タッチ情報をコピー
            Touch t = Input.GetTouch(i);
            //タッチした位置からRayを飛ばす
            Ray ray = Camera.main.ScreenPointToRay(t.position);
            RaycastHit hit = new RaycastHit();
            if (Physics.Raycast(ray, out hit))
            {
                //Rayを飛ばしてあたったオブジェクトが自分自身だったら
                if (hit.collider.gameObject == transform.Find("Cube").gameObject)
                {
                    ray = Camera.main.ScreenPointToRay(t.position);

                    if (Physics.Raycast(ray.origin, ray.direction, out hit, Mathf.Infinity) && hit.collider == transform.Find("Cube").gameObject.GetComponent<Collider>())
                    {
                        beRay = true;
                    }
                    else
                    {
                        beRay = false;
                    }
                }
            }
        }
    }

    private void MovePoisitionMobile()
    {
        for (int i = 0; i < Input.touchCount; i++)
        {
            // タッチ情報をコピー
            Touch t = Input.GetTouch(i);
            //タッチした位置からRayを飛ばす
            Ray ray = Camera.main.ScreenPointToRay(t.position);
            RaycastHit hit = new RaycastHit();
            if (Physics.Raycast(ray, out hit))
            {
                //Rayを飛ばしてあたったオブジェクトが自分自身だったら
                if (hit.collider.gameObject == transform.Find("Cube").gameObject)
                {
                    Vector3 mousePos = t.position;
                    mousePos.z = initialPosition.z + 2;

                    moveTo = Camera.main.ScreenToWorldPoint(mousePos);
                    moveTo.y = initialPosition.y;
                    transform.position = moveTo;
                }
            }
        }
    }

    private void RayCheck()
    {
        Ray ray = new Ray();
        RaycastHit hit = new RaycastHit();
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray.origin, ray.direction, out hit, Mathf.Infinity) && hit.collider == transform.Find("Cube").gameObject.GetComponent<Collider>())
        {
            beRay = true;
        }
        else
        {
            beRay = false;
        }
    }

    private void MovePoisition()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = initialPosition.z + 2;

        moveTo = Camera.main.ScreenToWorldPoint(mousePos);
        moveTo.y = initialPosition.y;
        transform.position = moveTo;
    }

    public void AccelerateCubeTapped()
    {
        Debug.Log("cubeタップ");
        ballScript.Accelerate();
    }

}
