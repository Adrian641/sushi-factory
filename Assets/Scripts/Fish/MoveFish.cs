using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class MoveFish : MonoBehaviour
{
    private Vector3 startPos;
    private Vector3 endPos;
    private Vector3 moveDir;

    private float fishDirection;

    public Camera mainCam;
    public float camX;
    public float camY;

    public float speed = 5.0f;

    private float distance;
    private float fractionOfJourney;
    private float startTime;

    private void Start()
    {
        if (mainCam == null)
        {
            mainCam = Camera.main;
        }

        //Get camera position
        camX = mainCam.transform.position.x;
        camY = mainCam.transform.position.z;

        //Setup starting position
        startTime = Time.time;
        startPos = transform.position;

        //Calculate endPos
        endPos = new Vector3(camX, 4f, camY) * 2;
        endPos.x = camX - (startPos.x - camX);
        endPos.z = camY - (startPos.z - camY);

        //Compute direction
        moveDir = (endPos - startPos).normalized;
        distance = Vector3.Distance(startPos, endPos);

        //rotate gameObject to face movement direction
        Vector3 direction = endPos - startPos;
        float angle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(90f, angle, 0f);
    }

    void Update()
    {
        float step = speed * Time.deltaTime;
        transform.position += moveDir * step;

        if (Vector3.Distance(transform.position, startPos) >= distance)
        {
            transform.position = endPos;
            Destroy(gameObject);
        }

        //distance = Vector2.Distance(new Vector2(transform.position.x, transform.position.y), endPos);
        //fractionOfJourney += speed * Time.deltaTime / distance;

        //transform.position = Vector3.Lerp(startPos, endPos, fractionOfJourney);

        //if (fractionOfJourney >= 1.0f)
        //{
        //    transform.position = endPos;
        //    Destroy(gameObject);
        //}
    }
}
