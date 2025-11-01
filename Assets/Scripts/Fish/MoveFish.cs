using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class MoveFish : MonoBehaviour
{
    private Vector3 startPos;
    private Vector3 endPos;
    private Vector3 fishDirection;

    public Camera mainCam;
    public float camX;
    public float camY;
    public float movDuration = 2.0f;

    private float startTime;

    private void Start()
    {
        if (mainCam == null)
        {
            mainCam = Camera.main;
        }
        camX = mainCam.transform.position.x;
        camY = mainCam.transform.position.z;
        startPos = transform.position;
        startTime = Time.time;
        endPos = new Vector3(camX, 4f, camY) * 2;
        Debug.Log(endPos);

        fishDirection = endPos - transform.position;
        fishDirection.Normalize();

        fishDirection = new Vector3(fishDirection.x, 0f, fishDirection.y);
        //Debug.Log(fishDirection);

        float angle = Mathf.Atan2(fishDirection.y, fishDirection.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(90f, angle, 0f);
    }
    void Awake()
    {

    }

    void Update()
    {
        float timeElapsed = Time.time - startTime;
        float fractionOfJourney = timeElapsed / movDuration;

        transform.position = Vector3.Lerp(startPos, endPos, fractionOfJourney);

        if (fractionOfJourney >= 1.0f)
        {
            Destroy(gameObject);
        }
    }
}
