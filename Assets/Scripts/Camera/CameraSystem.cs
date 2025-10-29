using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSystem : MonoBehaviour
{
    private void Update()
    {
        Vector3 inputDir = new Vector3(0, 0, 0);

        if (Input.GetKey(KeyCode.W)) inputDir.z = +1f;
        if (Input.GetKey(KeyCode.S)) inputDir.z = -1f;
        if (Input.GetKey(KeyCode.A)) inputDir.x = -1f;
        if (Input.GetKey(KeyCode.D)) inputDir.x = +1f;

        Vector3 moveDir = transform.forward * inputDir.z + transform.right * inputDir.x;
        float moveSpeed = 5f;
        transform.position += moveDir * moveSpeed * Time.deltaTime;

        float rotateDir = 0f;
        float sensitivity = 10f;
        if (Input.GetKey(KeyCode.Mouse2))
        {
            rotateDir += Input.GetAxis("Mouse X") * sensitivity;
            Debug.Log(rotateDir);
        }

        transform.Rotate(Vector3.up, rotateDir * sensitivity * Time.deltaTime);

        //please please help i dont understand anything about this T-T
        //the whole normal moving part works and it would work with the Q and E input but idk how to put a mouse input instead of it T-T

        //float rotateDir = 0f;
        //if (Input.GetKey(KeyCode.Q)) rotateDir = +1f;
        //if (Input.GetKey(KeyCode.E)) rotateDir = -1f;

        //float rotateSpeed = 20f;
        //transform.eulerAngles += new Vector3(0, rotateDir * rotateSpeed * Time.deltaTime, 0);



    }
}