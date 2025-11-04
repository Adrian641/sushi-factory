using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.VirtualTexturing;
using UnityEngine.EventSystems;
using System.Linq;
using UnityEngine.Rendering;

public class PlaceMachine : MonoBehaviour
{
    public SelectItems selectItems;

    public int TypeOfMachine;

    public GameObject highlightedExctractor_Up;
    public GameObject highlightedExctractor_Down;
    public GameObject highlightedExctractor_Left;
    public GameObject highlightedExctractor_Right;

    public GameObject Exctractor_Up;
    public GameObject Exctractor_Down;
    public GameObject Exctractor_Left;
    public GameObject Exctractor_Right;

    public string tagToPlaceOn;
    public string factoryTag;

    private bool isSelected;
    public bool isFishNet;

    public Camera mainCam;
    public RaycastHit RayHit;
    public Ray ray;
    public Vector2 hitPoint = Vector2.zero;
    public Vector2 mousePos = Vector2.zero;

    public Vector2 selectedPos = Vector2.zero;

    public List<Vector2> factoryPos;
    private int factoryPosIndex = 0;

    private GameObject selectPrefab;
    private GameObject selectedObject;

    public int fishType = 0;

    private void Start()
    {
        selectPrefab = highlightedExctractor_Up;
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
            isFishNet = true;
        else
            isSelected = false;
        if (isSelected && !IsMouseOverUi())
        {
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

                            selectedPos = hitPoint;
                            selectedObject = Instantiate(selectPrefab, new Vector3(hitPoint.x, 0f, hitPoint.y), Quaternion.identity, gameObject.transform);
                            selectedObject.transform.rotation = Quaternion.Euler(90f, 0f, 0f);
                        }
                    }
                }
            }
            if (Input.GetKeyDown(KeyCode.Mouse0) && !factoryPos.Contains(hitPoint) && selectedPos != Vector2.zero)
            {
                GameObject FactoryToPlace = Exctractor_Up;
                if (selectPrefab == highlightedExctractor_Up)
                    FactoryToPlace = Exctractor_Up;
                else if (selectPrefab == highlightedExctractor_Right)
                    FactoryToPlace = Exctractor_Right;
                else if (selectPrefab == highlightedExctractor_Down)
                    FactoryToPlace = Exctractor_Down;
                else if (selectPrefab == highlightedExctractor_Left)
                    FactoryToPlace = Exctractor_Left;

                Destroy(selectedObject);
                GameObject Factory = Instantiate(FactoryToPlace, new Vector3(hitPoint.x, 0f, hitPoint.y), Quaternion.identity, gameObject.transform);
                Factory.transform.position = new Vector3(mousePos.x, 0f, mousePos.y);
                Factory.transform.rotation = Quaternion.Euler(90f, 0f, 0f);
                factoryPos.Add(new Vector2(Factory.transform.position.x, Factory.transform.position.z));
            }
        }
        if(IsMouseOverUi())
            Destroy(selectedObject);
        if (Physics.Raycast(ray, out RayHit))
        {
            mousePos = new Vector2(MathF.Round(RayHit.point.x), MathF.Round(RayHit.point.z));
            if (mousePos != hitPoint)
            {
                Destroy(selectedObject);
                selectedPos = Vector2.zero;
            }
        }

        if (Input.GetKey(KeyCode.Mouse1))
        {
            DeleteObjects();
        }
    }

    public void Rotate()
    {
        if (selectPrefab == highlightedExctractor_Up)
            selectPrefab = highlightedExctractor_Right;
        else if (selectPrefab == highlightedExctractor_Right)
            selectPrefab = highlightedExctractor_Down;
        else if (selectPrefab == highlightedExctractor_Down)
            selectPrefab = highlightedExctractor_Left;
        else if (selectPrefab == highlightedExctractor_Left)
            selectPrefab = highlightedExctractor_Up;
        Destroy(selectedObject);
        selectedPos = Vector2.zero;
    }

    private bool IsMouseOverUi()
    {
        return EventSystem.current.IsPointerOverGameObject();
    }

    public void DeleteObjects()
    {

        if (Physics.Raycast(ray, out RayHit))
        {
            GameObject hit = RayHit.transform.gameObject;
            if (hit.CompareTag(factoryTag))
            {
                Destroy(hit.gameObject);
                factoryPos.Remove(new Vector2(hit.transform.position.x, hit.transform.position.z));
            }
        }
    }
}
