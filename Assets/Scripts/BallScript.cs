using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallScript : MonoBehaviour
{
    Rigidbody rigidbody;
    bool isRotating = false;
    public UIScript uiScript;
    public GameMasterScript gameMasterScript;
    public GameObject AccelerateButton;
    Vector3 initial;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        rigidbody.maxAngularVelocity = 100;

        initial = this.gameObject.transform.position;

        AccelerateButton.active = false;
    }

    // Update is called once per frame
    void Update()
    {

        if(rigidbody.angularVelocity.magnitude > 80)
        {
            isRotating = true;
        }

        if (rigidbody.angularVelocity.magnitude < 20 && isRotating)
        {
            Debug.Log("20以下");
            rigidbody.constraints = RigidbodyConstraints.None;
        }
        if (rigidbody.angularVelocity.magnitude < 1 && isRotating)
        {
            gameOver();
        }

        if (uiScript.start == true)
        {
            StartCoroutine(GameStart());
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Accelerate();
        }
        if(gameMasterScript.gameOver == false　&& AccelerateButton.active)
        {
            Debug.Log(rigidbody.angularVelocity.magnitude);
        }
    }

    IEnumerator GameStart()
    {
        yield return new WaitForSeconds(2.7f);

        Vector3 v = new Vector3(0, 3, 0);
        rigidbody.AddTorque(v, ForceMode.Impulse);
        this.gameObject.transform.position = new Vector3(initial.x + Random.Range(-1.0f, 1.0f) * 0.01f, this.gameObject.transform.position.y, initial.z);

        AccelerateButton.active = true;
    }

    public void Accelerate()
    {
        if (AccelerateButton.active)
        {
            Debug.Log("加速");
            Vector3 v = new Vector3(0, 1, 0);
            rigidbody.AddTorque(v, ForceMode.Impulse);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "Floor")
        {
            gameOver();
        }
    }

    void gameOver()
    {
        if (gameMasterScript.gameOver == false)
        {
            gameMasterScript.gameOver = true;
            isRotating = false;
        }
    }

}
