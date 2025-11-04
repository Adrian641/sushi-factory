using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraSystem : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera cinemachineVirtualCamera;
    [SerializeField] private float fieldOfViewMax = 80f;
    [SerializeField] private float fieldOfViewMin = 30f;
    [SerializeField] private Vector2 limitX = new Vector2(30f, 123f);
    [SerializeField] private Vector2 limitZ = new Vector2(30f, 123f);
    private float targetFieldOfView = 60f;

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

        Vector3 clampedPos = transform.position;
        clampedPos.x = Mathf.Clamp(clampedPos.x, limitX.x, limitX.y);
        clampedPos.z = Mathf.Clamp(clampedPos.z, limitZ.x, limitZ.y);
        transform.position = clampedPos;

        float rotateDirX = 0f;
        float rotateDirY = 0f;
        float sensitivity = 15f;
        if (Input.GetKey(KeyCode.Mouse2))
        {
            rotateDirX += Input.GetAxis("Mouse X") * sensitivity;
            //Debug.Log(rotateDirX);

            rotateDirY += Input.GetAxis("Mouse Y") * sensitivity;
            //Debug.Log(rotateDirY);
        }

        transform.Rotate(Vector3.up, rotateDirX * sensitivity * Time.deltaTime);

        HandleCameraZoom();



        //please please help i dont understand anything about this T-T
        //the whole normal moving part works and it would work with the Q and E input but idk how to put a mouse input instead of it T-T

        //float rotateDir = 0f;
        //if (Input.GetKey(KeyCode.Q)) rotateDir = +1f;
        //if (Input.GetKey(KeyCode.E)) rotateDir = -1f;

        //float rotateSpeed = 20f;
        //transform.eulerAngles += new Vector3(0, rotateDir * rotateSpeed * Time.deltaTime, 0);



    }

    private void HandleCameraZoom()
    {
        if (Input.mouseScrollDelta.y < 0)
        {
            targetFieldOfView += 5f;
        }
        if (Input.mouseScrollDelta.y > 0)
        {
            targetFieldOfView -= 5f;
        }

        targetFieldOfView = Mathf.Clamp(targetFieldOfView, fieldOfViewMin, fieldOfViewMax);
        cinemachineVirtualCamera.m_Lens.FieldOfView = targetFieldOfView;
    }
}