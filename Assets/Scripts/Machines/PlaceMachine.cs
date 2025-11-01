using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.VirtualTexturing;
using UnityEngine.EventSystems;

public class PlaceMachine : MonoBehaviour
{
    public SelectItems selectItems;

    public int TypeOfMachine;

    public GameObject exctractor_Up;
    public GameObject exctractor_Down;
    public GameObject exctractor_Left;
    public GameObject exctractor_Right;

    public string tagToPlaceOn;

    private bool isSelected;

    public Camera mainCam;
    public RaycastHit RayHit;
    public Ray ray;
    public Vector2 hitPoint = Vector2.zero;

    public Vector2 selectedPos = Vector2.zero;

    private GameObject selectPrefab;
    private GameObject selectedObject;

    private void Start()
    {
        selectPrefab = exctractor_Up;
    }
    void Update()
    {
        if (selectItems.SelectExtractor_2 && TypeOfMachine == 2)
            isSelected = true;
        else if (selectItems.SelectFishNet_3 && TypeOfMachine == 3)
            isSelected = true;
        else if (selectItems.SelectRiceField_4 && TypeOfMachine == 4)
            isSelected = true;
        else if (selectItems.SelectCutter_5 && TypeOfMachine == 5)
            isSelected = true;
        else if (selectItems.SelectStacker_6 && TypeOfMachine == 6)
            isSelected = true;
        else if (selectItems.SelectRoller_7 && TypeOfMachine == 7)
            isSelected = true;
        else
            isSelected = false;
        if (isSelected && !IsMouseOverUi())
        {
            Debug.Log(IsMouseOverUi());
            ray = mainCam.ScreenPointToRay(Input.mousePosition);
            RaycastHit[] RayHit = Physics.RaycastAll(ray);
            if (RayHit != null)
            {
                for (int i = 0; i < RayHit.Length; i++)
                {
                    if (RayHit[i].collider.CompareTag(tagToPlaceOn))
                    {
                        hitPoint = new Vector2(MathF.Round(RayHit[i].point.x), MathF.Round(RayHit[i].point.z));
                        if (Input.GetKeyDown(KeyCode.R))
                            Rotate();
                        if (hitPoint != selectedPos)
                        {
                            Destroy(selectedObject);

                            selectedObject = Instantiate(selectPrefab, new Vector3(hitPoint.x, 0f, hitPoint.y), Quaternion.identity, gameObject.transform);
                            selectedObject.transform.rotation = Quaternion.Euler(90f, 0f, 0f);
                            selectedPos = hitPoint;
                        }
                    }
                }

            }
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                Destroy(selectedObject);
                //Place Prefab
            }
        }
        if(IsMouseOverUi())
            Destroy(selectedObject);

    }

    public void Rotate()
    {
        if (selectPrefab == exctractor_Up)
            selectPrefab = exctractor_Right;
        else if (selectPrefab == exctractor_Right)
            selectPrefab = exctractor_Down;
        else if (selectPrefab == exctractor_Down)
            selectPrefab = exctractor_Left;
        else if (selectPrefab == exctractor_Left)
            selectPrefab = exctractor_Up;
        Destroy(selectedObject);
        selectedPos = Vector2.zero;
    }

    private bool IsMouseOverUi()
    {
        return EventSystem.current.IsPointerOverGameObject();
    }
}
