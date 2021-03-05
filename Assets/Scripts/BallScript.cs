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
    public GameObject AccelerateParticle;
    Vector3 initial;
    IEnumerator routine;
    public AudioClip bound;
    public AudioClip tap;
    private AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        rigidbody.maxAngularVelocity = 100;

        initial = this.gameObject.transform.position;

        AccelerateButton.active = false;
        AccelerateParticle.SetActive(false);

        routine = AccelerateEffect();


        audioSource = gameObject.GetComponent<AudioSource>();
        audioSource.clip = tap;
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
            GameOver();
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

        AccelerateParticle.transform.position = new Vector3(this.transform.position.x, this.transform.position.y - 0.1f, this.transform.position.z);
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

            StopCoroutine(routine);
            routine = null;
            routine = AccelerateEffect();
            StartCoroutine(routine);

            audioSource.volume = 0.5f;
            audioSource.Play();
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "Floor")
        {
            GameOver();
            AccelerateButton.active = false;
        }

        if (gameMasterScript.gameOver)
        {
            audioSource.clip = bound;
            audioSource.volume = collision.relativeVelocity.magnitude / 10 * 0.5f;
            audioSource.Play();
        }
    }

    void GameOver()
    {
        if (gameMasterScript.gameOver == false)
        {
            gameMasterScript.gameOver = true;
            isRotating = false;
        }
    }

    IEnumerator AccelerateEffect()
    {
        AccelerateParticle.SetActive(true);
        yield return new WaitForSeconds(0.2f);
        AccelerateParticle.SetActive(false);
    }

}
