using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    private Vector3 moveTo;
    private Vector3 initialPosition;
    private bool isDragging = false;
    private int activeFingerId = -1;
    public BallScript ballScript;

    void Start()
    {
        initialPosition = this.transform.position;
        Debug.Log("Initial Position: " + initialPosition);
    }

    void Update()
    {
        // タッチ入力があるか確認
        if (Input.touchCount > 0)
        {
            // すべてのタッチをループ
            for (int i = 0; i < Input.touchCount; i++)
            {
                Touch touch = Input.GetTouch(i);

                // デバッグログでタッチ情報を出力
                Debug.Log($"Touch {i}: fingerId={touch.fingerId}, phase={touch.phase}, position={touch.position}");

                // タッチ開始時
                if (touch.phase == TouchPhase.Began)
                {
                    RayCheck(touch);
                }

                // アクティブな指の場合のみ処理
                if (touch.fingerId == activeFingerId)
                {
                    if (isDragging && (touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary))
                    {
                        MovePosition(touch);
                    }

                    if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
                    {
                        isDragging = false;
                        activeFingerId = -1;
                        Debug.Log("Touch ended or canceled for fingerId: " + touch.fingerId);
                    }
                }
            }
        }
    }

    private void RayCheck(Touch touch)
    {
        Ray ray = Camera.main.ScreenPointToRay(touch.position);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            // Ray が「Cube」にヒットした場合
            GameObject cube = transform.Find("Cube").gameObject;
            if (hit.collider.gameObject == cube)
            {
                isDragging = true;
                activeFingerId = touch.fingerId;
                Debug.Log("Ray hit Cube with fingerId: " + activeFingerId);

                // 「Cube」をタップしたときの処理
                AccelerateCubeTapped();
            }
        }
    }

    private void MovePosition(Touch touch)
    {
        Vector3 touchPos = touch.position;
        touchPos.z = Camera.main.WorldToScreenPoint(transform.position).z;

        moveTo = Camera.main.ScreenToWorldPoint(touchPos);
        moveTo.y = initialPosition.y;
        transform.position = moveTo;

        Debug.Log("Moved to position: " + transform.position + " using touch with fingerId: " + touch.fingerId);
    }

    public void AccelerateCubeTapped()
    {
        Debug.Log("Cube tapped");
        ballScript.Accelerate();
    }
}